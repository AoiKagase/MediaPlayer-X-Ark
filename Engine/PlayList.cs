using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MediaPlayer_X_Ark.Engine
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

		// ロード済みかどうか
		[Browsable(false)]
		public bool IsLoaded => Sound.hasHandle();

		// TAG
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

		public void SetLength(uint length)
		{
			this._length = length;
			OnPropertyChanged(nameof(length));
		}

		// ファイルパスのみで作成（遅延ロード用）
		public PlayList(string fileName)
		{
			this.FileName = fileName;
			this.Sound = default; // 未ロード状態
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
        }

	}
}
