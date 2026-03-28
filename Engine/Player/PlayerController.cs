using MediaPlayer_X_Ark.Engine.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Engine.Player
{
    /// <summary>
    /// 再生制御の高レベルAPIを提供するコントローラー。
    /// MainForm はこのクラスのメソッドを呼ぶだけで再生制御が完結する。
    /// UI更新が必要な場合は TrackChanged / PlaybackStateChanged イベントを購読する。
    /// </summary>
    public class PlayerController
    {
		// ── Win32 高精度タイマー ─────────────────────────────────────
		[DllImport("winmm.dll")] private static extern uint timeBeginPeriod(uint uPeriod);
		[DllImport("winmm.dll")] private static extern uint timeEndPeriod(uint uPeriod);

		/// <summary>
		/// NonStopMix切替監視専用の高精度タイマー（1ms精度）。
		/// 切替5秒前に起動し、切替後に停止する。
		/// </summary>
		private const int PreciseIntervalMs = 5; // 5ms精度
		private System.Threading.Timer _preciseTimer;
		private volatile bool _preciseTimerRunning = false;
		private long _lastTickMs = 0;

		// NonStopMix 二重発火防止
		private volatile bool _nextTriggered = false;
		// クロスフェード開始済みフラグ（残り時間検知の二重発火防止）
		private volatile bool _crossfadeTriggered = false;

		private readonly IPlayerEngine _engine;
        private readonly IConfigService _config;
		private readonly SynchronizationContext _syncContext;

		// ── イベント ────────────────────────────────────────────────

		/// <summary>曲が切り替わったときに発火。引数は新しい PlayingIndex。</summary>
		public event Action<int> TrackChanged;

        /// <summary>再生状態が変化したときに発火（再生開始・停止・一時停止）。</summary>
        public event Action PlaybackStateChanged;
		
        /// <summary>波形解析が完了したときに発火（常にUIスレッドで呼ばれる）。</summary>
		public event Action<int> WaveformReady;
        public event EventHandler<PlayerErrorEventArgs> ErrorOccurred;
        // ── プロパティ ───────────────────────────────────────────────
        // ── AB リピート ───────────────────────────────────────────────
        public uint AbStart { get; private set; } = uint.MaxValue;
        public uint AbEnd { get; private set; } = uint.MaxValue;
        public bool AbRepeatEnabled => AbStart != uint.MaxValue && AbEnd != uint.MaxValue;

		public IPlayerEngine Engine  => _engine;
        public IConfigService Config => _config;
        public void SetAbStart(uint ms) => AbStart = ms;
        public void SetAbEnd(uint ms) => AbEnd = ms;
        public void ClearAbRepeat()
        {
            AbStart = uint.MaxValue;
            AbEnd = uint.MaxValue;
        }
        public PlayerController(IPlayerEngine engine, IConfigService config)
        {
            _engine = engine;
            _config = config;
			_syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
			Initialize();
		}

        private void Initialize()
        {
            _engine.ErrorOccurred += (s, e) => _syncContext.Post(_ => ErrorOccurred?.Invoke(s, e), null);
            // OutputType は init() より前に設定する必要がある
            _engine.SetOutputTypeBeforeInit(_config.GetOutputType());

			_engine.Initialize(_config.settings.Buffer);
			_engine.WaveformReady += (index) =>	_syncContext.Post(_ => WaveformReady?.Invoke(index), null);
			// Device は init() 後に設定する
			_engine.SetDevice(_config.settings.Device);

			_engine.ReplayGainEnabled = _config.settings.ReplayGainEnabled;
			_engine.ReplayGainMode = _config.settings.ReplayGainMode;
			_engine.ReplayGainPreamp = _config.settings.ReplayGainPreamp;
			_engine.CrossfadeEnabled = _config.settings.CrossfadeEnabled;
			_engine.CrossfadeDurationMs = _config.settings.CrossfadeDurationMs;
            _engine.NonStopMixEnabled = _config.settings.NonStopMixEnabled;

            _engine.SoundFontPath = _config.settings.SoundFontPath;

            _engine.effector.ApplySettings(_config.settings.Effectors);

            if (_config.settings.RestorePlaylist)
			{
				var playlistPath = Path.Combine(
					Application.StartupPath, "last_playlist.json");
				if (File.Exists(playlistPath))
					RestorePlaylistFromFile(playlistPath);
			}
			if (_config.settings.RestorePosition
				&& _config.settings.LastPlayingIndex >= 0
				&& _config.settings.LastPlayingIndex < _engine.PlayList.Count)
			{
				// 前回の再生位置を一時停止状態で復元する
				_engine.SetDevice(_config.settings.Device);
				_engine.PlaySoundPaused(_config.settings.LastPlayingIndex,
					_config.settings.LastPlayingPosition);

				TrackChanged?.Invoke(_config.settings.LastPlayingIndex);
				PlaybackStateChanged?.Invoke();
			}
		}

		private void RestorePlaylistFromFile(string path)
		{
			try
			{
				var list = System.Text.Json.JsonSerializer
					.Deserialize<List<string>>(
						File.ReadAllText(path, System.Text.Encoding.UTF8));

				if (list == null) return;

				foreach (var file in list)
				{
					if (File.Exists(file))
						_engine.CreateSound(file, out _);
				}
			}
			catch { }
		}
		// ── 再生制御 ─────────────────────────────────────────────────
        public bool IsPlaying => _engine.IsPlaying();
        public int PlayingIndex => _engine.PlayingIndex;
		/// <summary>
		/// 指定インデックスを再生する。
		/// 音量・パン・タグ取得・ReplayGain適用を一括で行う。
		/// </summary>
		public void PlayAt(int index)
        {
			if (index < 0 || index >= _engine.PlayList.Count) return;

			_nextTriggered = false;
			_crossfadeTriggered = false;

			_engine.SetDevice(_config.settings.Device);
            _engine.PlaySound(index);

            ApplyVolumeFromConfig();
            ApplyPanFromConfig();

            TrackChanged?.Invoke(index);
            PlaybackStateChanged?.Invoke();
			UpdatePreciseTimer();
		}

        /// <summary>再生／一時停止をトグルする</summary>
        public void TogglePlayPause()
        {
            _engine.SwitchPause();

            PlaybackStateChanged?.Invoke();
			UpdatePreciseTimer();
		}

        /// <summary>停止する</summary>
        public void Stop()
        {
			_nextTriggered = false;
			_crossfadeTriggered = false;
			StopPreciseTimer();
			_engine.Stop();
            PlaybackStateChanged?.Invoke();
        }

        /// <summary>次の曲へ（ループモードを考慮）</summary>
        public void PlayNext(bool manual = false)
        {
			int currentIndex = _engine.PlayingIndex;
			_nextTriggered = false;
			_crossfadeTriggered = false;
			_engine.SetDevice(_config.settings.Device);
			_engine.PlayNext(currentIndex, manual);
            TrackChanged?.Invoke(_engine.PlayingIndex);
            PlaybackStateChanged?.Invoke();
			UpdatePreciseTimer();
		}

        /// <summary>前の曲へ（ループモードを考慮）</summary>
        public void PlayPrevious(bool manual = false)
        {
			int currentIndex = _engine.PlayingIndex;
			_nextTriggered = false;
			_crossfadeTriggered = false;
			_engine.SetDevice(_config.settings.Device);
			_engine.PlayPrevious(currentIndex, manual);
            TrackChanged?.Invoke(_engine.PlayingIndex);
            PlaybackStateChanged?.Invoke();
			UpdatePreciseTimer();
		}

		public void SetPosition(uint ms)
        {
            _engine.SetPosition(ms);
        }
		public uint GetPosition() => _engine.GetPosition();
		public uint GetLength() => _engine.GetLength(_engine.PlayingIndex);
		public bool OpenFiles(string[] filenames)
        {
			int idx = 0;

			bool anyAdded = false;
            foreach (var file in filenames)
            {
				if (idx++ == 0)
				{
					// 先頭ファイルのみ即再生
					OpenAndPlay(file);
				}
				else
				{
					// 2曲目以降はプレイリストへ追加のみ
					_engine.CreateSound(file, out _);
				}
                anyAdded = true;
            }

            return anyAdded;
        }

        /// <summary>
        /// ファイルをプレイリストに追加して再生する。
        /// </summary>
        public bool OpenAndPlay(string filename)
        {
			if (_engine.CreateSound(filename, out int index) == FMOD.RESULT.OK)
            {
				switch (_config.settings.OpenFileAction)
				{
					case 1: // 常に再生
						PlayAt(index);
						break;

					case 2: // 常に追加のみ
							// 再生しない
						break;

					default: // 再生中なら追加・停止中なら再生
						if (!_engine.IsPlaying())
							PlayAt(index);
						break;
				}

                return true;
            }
            return false;
        }

        public bool OpenUrl(string url)
        {
            if (_engine.PlayUrl(url) == FMOD.RESULT.OK)
            {
                TrackChanged?.Invoke(_engine.PlayingIndex);
                PlaybackStateChanged?.Invoke();
                return true;
            }
            return false;
        }

        // ── ループモード制御 ─────────────────────────────────────────
        public LOOP_MODE GetLoopMode()
        {
            return _engine.loop;
		}
		/// <summary>ループモードを設定する（ランダムフラグは保持）</summary>
		public void SetLoopMode(LOOP_MODE mode)
        {
            bool isRandom = (_engine.loop & LOOP_MODE.LOOP_RANDOM) != 0;
            _engine.loop = mode;
            if (isRandom) _engine.loop |= LOOP_MODE.LOOP_RANDOM;
        }

		/// <summary>ランダム再生をトグルする</summary>
		public void ToggleRandom() => _engine.loop ^= LOOP_MODE.LOOP_RANDOM;

		/// <summary>ループボタン押下時のサイクル（NONE → ONE_REPEAT → ALL → NONE）</summary>
		public void CycleLoop()
        {
            var loopOnly = _engine.loop & ~LOOP_MODE.LOOP_RANDOM;
            var next = loopOnly switch
            {
                LOOP_MODE.LOOP_NONE       => LOOP_MODE.LOOP_ONE_REPEAT,
                LOOP_MODE.LOOP_ONE_REPEAT => LOOP_MODE.LOOP_ALL,
                _                         => LOOP_MODE.LOOP_NONE,
            };
            SetLoopMode(next);
        }

		/// <summary>
		/// 再生状態・設定に応じて高精度タイマーを起動または停止する。
		/// PlayAt / PlayNext / PlayPrevious / Stop / TogglePlayPause から呼ぶ。
		/// </summary>
		public void UpdatePreciseTimer()
		{
			bool needed = _engine.NowPlaying && _engine.IsPlaying()
				&& (_engine.CrossfadeEnabled
					|| _config.settings.NonStopMixEnabled
					|| AbRepeatEnabled);

			if (needed && !_preciseTimerRunning)
				StartPreciseTimer();
			else if (!needed && _preciseTimerRunning)
				StopPreciseTimer();
		}
		private void StartPreciseTimer()
		{
			if (_preciseTimerRunning) return;
			Interlocked.Exchange(ref _lastTickMs, Environment.TickCount64);
			_preciseTimerRunning = true;
			timeBeginPeriod(1);
			_preciseTimer = new System.Threading.Timer(
				PreciseTimerCallback, null,
				PreciseIntervalMs, PreciseIntervalMs);
		}

		private void StopPreciseTimer()
		{
			if (!_preciseTimerRunning) return;
			_preciseTimerRunning = false;
			_preciseTimer?.Dispose();
			_preciseTimer = null;
			timeEndPeriod(1);
		}

		private void PreciseTimerCallback(object state)
		{
			if (!_preciseTimerRunning) return;

			long now = Environment.TickCount64;
			int elapsedMs = (int)(now - Interlocked.Read(ref _lastTickMs));
			Interlocked.Exchange(ref _lastTickMs, now);

			if (!_engine.NowPlaying)
			{
				StopPreciseTimer();
				//_syncContext.Post(_ => PlayNext(), null);
				return;
			}

			// ── CUEトラック終端監視 ──────────────────────────────────────
			// FMODはトラック境界で止まらないため、CueEndMs到達を明示的に検知する
			{
				int pidxCue = _engine.PlayingIndex;
				if (pidxCue >= 0 && pidxCue < _engine.PlayList.Count && !_nextTriggered)
				{
					var cuEntry = _engine.PlayList[pidxCue];
					if (cuEntry.IsCueTrack && cuEntry.CueEndMs.HasValue)
					{
						uint relPos = _engine.GetPosition();
						if (relPos >= cuEntry.LengthMs && cuEntry.LengthMs > 0)
						{
							_nextTriggered = true;
							_syncContext.Post(_ => PlayNext(), null);
							return;
						}
					}
				}
			}

			// ── クロスフェード音量更新 ────────────────────────────────────────
			if (_engine.CrossfadeEnabled)
				_engine.UpdateCrossfade(elapsedMs);

			// ── クロスフェード開始検知 ────────────────────────────────────────
			if (_engine.CrossfadeEnabled
				&& !_engine.NonStopMixEnabled
				&& !_crossfadeTriggered
				&& !_engine.CrossfadeTriggered)
			{
				int pidx = _engine.PlayingIndex;
				if (pidx >= 0)
				{
					uint remaining = _engine.GetLength(pidx) - _engine.GetPosition();
					if ((int)remaining <= _engine.CrossfadeDurationMs)
					{
						_crossfadeTriggered = true;
						_engine.CrossfadeTriggered = true;
						_syncContext.Post(_ => PlayNext(), null);
					}
				}
			}

			// ── ABリピート ────────────────────────────────────────────────────
			if (AbRepeatEnabled)
			{
				uint pos = _engine.GetPosition();
				if (pos >= AbEnd)
					_engine.SetPosition(AbStart);
			}

			// ── NonStopMix ────────────────────────────────────────────────────
			if (_config.settings.NonStopMixEnabled)
			{
				// 退避チャンネル（旧曲）が自然終了していたら解放
				_engine.ReleaseNonStopFadingIfDone();

				if (!_nextTriggered)
				{
					int idx = _engine.PlayingIndex;
					if (idx >= 0 && idx < _engine.PlayList.Count)
					{
						var entry = _engine.PlayList[idx];
						// CUEトラックはNonStopMixをバイパス（連続録音CDを想定）
						if (!entry.IsCueTrack && entry.WaveformReady && entry.AudioEndMs > 0)
						{
							uint pos = _engine.GetPosition();
							// AudioEndMs + オフセット（秒）を過ぎたら次曲へ
							// 負値オフセット = 無音前から早めに次曲をスタート
							if ((int)pos >= entry.AudioEndMs + _config.settings.NonStopMixOffsetSec)
							{
								_nextTriggered = true;
								_syncContext.Post(_ => PlayNext(), null);
							}
						}
					}
				}
			}


			if (!_engine.IsPlaying())
			{
				// 再生が自然終了 → UIスレッドで次曲へ
				StopPreciseTimer();
				_syncContext.Post(_ => PlayNext(), null);
				return;
			}
		}

			/// <summary>
		/// UIタイマー（PlayerTimer_Tick）から呼ぶ後方互換メソッド。
		/// 曲終了検知・クロスフェード更新は高精度タイマーに移管済みだが、
		/// NonStopMix/Crossfade 無効時は UI タイマーでも次曲検知を行う。
		/// </summary>
		public void OnTimerTick(int timerIntervalMs)
		{
			if (!_config.settings.NonStopMixEnabled && !_config.settings.CrossfadeEnabled)
			{
				if (_engine.NowPlaying && !_engine.IsPlaying() && _engine.PlayingIndex < _engine.PlayList.Count - 1)
				{
					StopPreciseTimer();
					_syncContext.Post(_ => PlayNext(), null);
					return;
				}
			}
		}
		// ── 音量・パン ───────────────────────────────────────────────

		public void SetVolume(int sliderValue)
        {
            float vol = sliderValue / 100f;
            _engine.SetVolume(vol);
            _config.settings.Volume = sliderValue;
        }
        public int GetVolume()
        {
            return _engine.GetVolume();
        }

        public void SetPan(int sliderValue)
        {
            float pan = sliderValue / 10f;
            _engine.SetPan(pan);
            _config.settings.Pan = sliderValue;
        }

        // ── 内部ヘルパー ─────────────────────────────────────────────

        private void ApplyVolumeFromConfig()
        {
            _engine.SetVolume(_config.settings.Volume / 100f);
        }

        private void ApplyPanFromConfig()
        {
            _engine.SetPan(_config.settings.Pan / 10f);
        }

        /// <summary>再生中の曲のタイトル文字列を生成する</summary>
        public string BuildTitleText()
        {
            int index = _engine.PlayingIndex;
            if (index < 0 || index >= _engine.PlayList.Count) return string.Empty;
            var entry = _engine.PlayList[index];
            string title = !string.IsNullOrEmpty(entry.Title)
                ? entry.Title
                : Path.GetFileName(entry.FileName);
            if (!string.IsNullOrEmpty(entry.Artist)) title += " - " + entry.Artist;
            if (!string.IsNullOrEmpty(entry.Album))  title += " - " + entry.Album;
            return title;
        }

		public void AutoSavePlaylist()
		{
			if (!_config.settings.AutoSavePlaylist) return;
			SavePlaylistToFile(Path.Combine(
				Application.StartupPath, "last_playlist.json"));
		}

		private void SavePlaylistToFile(string path)
		{
			// CUEトラックはCUEファイルパスを保存（重複排除）
			var seen = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
			var list = new System.Collections.Generic.List<string>();
			foreach (var p in _engine.PlayList)
			{
				string entry = (p.IsCueTrack && p.CueSheetRef != null)
					? p.CueSheetRef.CuePath
					: p.FileName;
				if (seen.Add(entry))
					list.Add(entry);
			}
			File.WriteAllText(path,
				System.Text.Json.JsonSerializer.Serialize(list),
				System.Text.Encoding.UTF8);
		}

        public void Close()
        {
			StopPreciseTimer();
			if (_config.settings.RestorePlaylist)
				SavePlaylistToFile(Path.Combine(
					Application.StartupPath, "last_playlist.json"));
			if (_config.settings.RestorePosition)
			{
				_config.settings.LastPlayingIndex = _engine.PlayingIndex;
				_config.settings.LastPlayingPosition = _engine.GetPosition();
			}
			else
			{
				_config.settings.LastPlayingIndex = -1;
				_config.settings.LastPlayingPosition = 0;
			}
		}
	}
}
