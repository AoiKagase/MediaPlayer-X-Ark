using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Frequency : INotifyPropertyChanged
	{
		private float _frequency;
		private FMOD.System _system;
		private FMOD.Channel _channel;

		/// <summary>
		/// </summary>
		public float Hz
		{
			get
			{
				return _frequency;
			}
			set
			{
				if (_frequency != value)
				{
					if (Enabled)
                    {
						_frequency = value;
					}
					NotifyPropertyChanged("Hz");
				}
			}
		}
		
		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				NotifyPropertyChanged("Enabled");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// CREATE DSP FOR LOWPASS FILTER
		/// </summary>
		/// <param name="system"></param>
		public Frequency(FMOD.System system)
		{
			_system = system;
			_system.getChannel(0, out _channel);
			SetDefault();
		}

		public void SetDefault()
		{
			Hz = 0;
		}
		public FMOD.RESULT Switch(bool sw)
		{
			FMOD.RESULT result = FMOD.RESULT.OK;
			if (sw == true)
            {
				Enabled = true;
				_channel.setFrequency(Hz);
			}
			else
			if (sw == false)
            {
				_channel.setFrequency(44100);
				Enabled = false;
			}

			return result;
		}
		public void SetFrequency(int value)
        {
			Hz = 44100 * ((_frequency + 100) / 100);
		}
	}
}
