using MediaPlayer_X_Ark.Engine.Config;
using NFluidsynth;
using System;
using System.IO;
using System.Linq;
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
        private readonly IPlayerEngine _engine;
        private readonly IConfigService _config;

        // ── イベント ────────────────────────────────────────────────

        /// <summary>曲が切り替わったときに発火。引数は新しい PlayingIndex。</summary>
        public event Action<int> TrackChanged;

        /// <summary>再生状態が変化したときに発火（再生開始・停止・一時停止）。</summary>
        public event Action PlaybackStateChanged;

        // ── プロパティ ───────────────────────────────────────────────

        public IPlayerEngine Engine  => _engine;
        public IConfigService Config => _config;

        public PlayerController(IPlayerEngine engine, IConfigService config)
        {
            _engine = engine;
            _config = config;
        }

        // ── 再生制御 ─────────────────────────────────────────────────

        /// <summary>
        /// 指定インデックスを再生する。
        /// 音量・パン・タグ取得・ReplayGain適用を一括で行う。
        /// </summary>
        public void PlayAt(int index)
        {
            if (index < 0 || index >= _engine.PlayList.Count) return;

            _engine.SetDevice(_config.settings.Device);
            _engine.PlaySound(index);

            // タイトル等はエンジンが GetTags() で取得済み
            // トラックバーの最大値は MainForm 側で設定（UI依存のため）

            ApplyVolumeFromConfig();
            ApplyPanFromConfig();

            TrackChanged?.Invoke(index);
            PlaybackStateChanged?.Invoke();
        }

        /// <summary>再生／一時停止をトグルする</summary>
        public void TogglePlayPause()
        {
            if (_engine.IsPlaying())
                _engine.Pause();
            else if (_engine.PlayingIndex < _engine.PlayList.Count)
                PlayAt(_engine.PlayingIndex);

            PlaybackStateChanged?.Invoke();
        }

        /// <summary>停止する</summary>
        public void Stop()
        {
            _engine.Stop();
            PlaybackStateChanged?.Invoke();
        }

        /// <summary>次の曲へ（ループモードを考慮）</summary>
        public void PlayNext()
        {
            _engine.PlayNext();
            TrackChanged?.Invoke(_engine.PlayingIndex);
            PlaybackStateChanged?.Invoke();
        }

        /// <summary>前の曲へ（ループモードを考慮）</summary>
        public void PlayPrevious()
        {
            _engine.PlayPrevious();
            TrackChanged?.Invoke(_engine.PlayingIndex);
            PlaybackStateChanged?.Invoke();
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

        /// <summary>URLをストリーミング再生する</summary>
        public bool OpenUrl(string url)
        {
            if (_engine.CreateSound(url, out int index) == FMOD.RESULT.OK)
            {
                PlayAt(index);
                return true;
            }
            return false;
        }

        // ── ループモード制御 ─────────────────────────────────────────

        /// <summary>ループモードを設定する（ランダムフラグは保持）</summary>
        public void SetLoopMode(LOOP_MODE mode)
        {
            bool isRandom = (_engine.loop & LOOP_MODE.LOOP_RANDOM) != 0;
            _engine.loop = mode;
            if (isRandom) _engine.loop |= LOOP_MODE.LOOP_RANDOM;
        }

        /// <summary>ランダム再生をトグルする</summary>
        public void ToggleRandom()
        {
            _engine.loop ^= LOOP_MODE.LOOP_RANDOM;
        }

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

        // ── タイマーティックから呼ぶ更新処理 ────────────────────────

        /// <summary>
        /// PlayerTimer_Tick から毎フレーム呼ぶ。
        /// 曲終了検知・クロスフェード更新を行い、必要に応じてイベントを発火する。
        /// </summary>
        /// <param name="timerIntervalMs">タイマーのインターバル（ms）</param>
        public void OnTimerTick(int timerIntervalMs)
        {
            // クロスフェード更新
            if (_engine.CrossfadeEnabled)
                _engine.UpdateCrossfade(timerIntervalMs);

            if (!_engine.NowPlaying) return;

            if (_engine.CrossfadeEnabled && !_engine.CrossfadeTriggered && _engine.IsPlaying())
            {
                // 残り時間検知によるクロスフェード開始
                int playingIndex = _engine.PlayingIndex;
                if (playingIndex >= 0)
                {
                    uint remaining = _engine.GetLength(playingIndex) - _engine.GetPosition();
                    if ((int)remaining <= _engine.CrossfadeDurationMs)
                    {
                        _engine.CrossfadeTriggered = true;
                        PlayNext();
                    }
                }
            }
            else if (!_engine.IsPlaying())
            {
                // 通常の曲終了 → 次曲へ
                PlayNext();
            }
        }

        // ── 音量・パン ───────────────────────────────────────────────

        public void SetVolume(int sliderValue)
        {
            float vol = sliderValue / 100f;
            _engine.SetVolume(vol);
            _config.settings.Volume = sliderValue;
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
        public string BuildTitleText(int index)
        {
            if (index < 0 || index >= _engine.PlayList.Count) return "";
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
			var list = _engine.PlayList.Select(p => p.FileName).ToList();
			File.WriteAllText(path,
				System.Text.Json.JsonSerializer.Serialize(list),
				System.Text.Encoding.UTF8);
		}

        public void Close()
        {
			// ★プレイリスト自動保存
			if (_config.settings.RestorePlaylist)
				SavePlaylistToFile(Path.Combine(
					Application.StartupPath, "last_playlist.json"));
			// ★再生位置を保存
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
