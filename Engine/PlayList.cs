using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MediaPlayer_X_Ark.Engine
{
    public class PlayList
    {
		private uint _length;
		private string _title;
		private string _fileName;
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
		public FMOD.Sound Sound { get; set; }

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
			}
		}
		[DisplayName("Artist")]
		public string Artist { get; set; }
		[DisplayName("Album")]
		public string Album { get; set; }
		public FMOD.SOUND_TYPE SoundType { get; set; }
		public FMOD.SOUND_FORMAT Format { get; set; }
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
		public void SetLength(uint length)
		{
			this._length = length;
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
