using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MediaPlayer_X_Ark.Engine.CUE;
namespace MediaPlayer_X_Ark.Engine.Player
{
    public class PlayList : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private uint _length;
		private string _title;
		private string _fileName;
		[Browsable(false)]
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
		[Browsable(false)] 
		public FMOD.Sound Sound { get; set; }

		[Browsable(false)]
		public bool IsLoaded => Sound.hasHandle();

		/// <summary>
		/// メモリ上のPCMデータから生成されたサウンド\uff08CDDA等\uff09。
		/// ファイルパスから再ロードできないため、クリーンアップ対象外にする。
		/// </summary>
		[Browsable(false)]
		public bool IsPcm { get; set; } = false;

		[Browsable(false)]
		internal XArkMidiFmodStream XArkMidiStream { get; set; }

		[DisplayName("Title")]
		public string Title
		{
			get
			{
				if (_title != null && !_title.Equals(""))
					return _title;
				else
					return Path.GetFileName(_fileName);
			}
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}
		[DisplayName("Artist")]
		public string Artist { get; set; }
		[DisplayName("Album")]
		public string Album { get; set; }
		[Browsable(false)]
		public int Year { get; set; }
		[Browsable(false)]
		public int TrackNumber { get; set; }
		[Browsable(false)]
		public int TrackTotal { get; set; }
		/// <summary>ReplayGain トラックゲイン（dB）。タグがなければ null。</summary>
		[Browsable(false)]
		public float? ReplayGainTrack { get; set; } = null;

		/// <summary>ReplayGain アルバムゲイン（dB）。タグがなければ null。</summary>
		[Browsable(false)]
		public float? ReplayGainAlbum { get; set; } = null;
		[Browsable(false)] 
		public FMOD.SOUND_TYPE SoundType { get; set; }
		[Browsable(false)] 
		public FMOD.SOUND_FORMAT Format { get; set; }
		[Browsable(false)] 
		public int Bit { get; set; }
		[DisplayName("Length")]
		public string length
		{
			get
			{
				TimeSpan time1 = TimeSpan.FromMilliseconds(this._length);
				return time1.ToString(@"mm\:ss");
			}
		}
		[Browsable(false)]
		public uint LengthMs
		{
			get { return this._length; }
		}
		[Browsable(false)]
		public string MusicBrainzDiscId { get; set; } = null;
		/// <summary>波形サマリー Lチャンネル（1000サンプル、0.0〜1.0）</summary>
		[Browsable(false)]
		public float[] WaveformL { get; set; } = null;

		/// <summary>波形サマリー Rチャンネル（1000サンプル、0.0〜1.0）</summary>
		[Browsable(false)]
		public float[] WaveformR { get; set; } = null;

		/// <summary>波形解析が完了しているか</summary>
		[Browsable(false)]
		public bool WaveformReady => WaveformL != null;

        /// <summary>
        /// NonStopMix用：実音終了位置（ms）。
        /// WaveformAnalyzer が解析後に設定する。
        /// -1 = 未解析または全域有音。
        /// </summary>
        [Browsable(false)]
        public int AudioEndMs { get; set; } = -1;

        /// <summary>CUEトラック開始位置（ms）。null = 通常ファイル</summary>
        [Browsable(false)]
        public int? CueStartMs { get; set; }

        /// <summary>CUEトラック終了位置（ms）。null = ファイル末尾または通常ファイル</summary>
        [Browsable(false)]
        public int? CueEndMs { get; set; }

        /// <summary>親CUEシートへの参照（CDDBクエリ・カバーアート取得に使用）</summary>
        [Browsable(false)]
        public CueSheet CueSheetRef { get; set; }

        /// <summary>CUEシートから生成されたトラックかどうか</summary>
        [Browsable(false)]
        public bool IsCueTrack => CueStartMs.HasValue;


        public void SetLength(uint length)
		{
			this._length = length;
			OnPropertyChanged(nameof(length));
		}

		/// <summary>ファイルパスのみで作成。Sound は再生時に遅延ロードされる。</summary>
		public PlayList(string fileName)
		{
			this.FileName = fileName;
			this.Sound = default;
		}

		public PlayList(string fileName, FMOD.Sound sound)
        {
			this.FileName = fileName;
			this.Sound = sound;
        }

		~PlayList()
        {
			if (this.Sound.hasHandle())
				this.Sound.release();
			XArkMidiStream?.Dispose();
        }

	}
}
