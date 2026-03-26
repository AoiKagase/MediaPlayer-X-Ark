using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MediaPlayer_X_Ark.Engine.Player;
namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Frequency : IEffector
    {
		private float _frequency;
		private FMOD.System _system;
		private FMOD.Channel[] _channel = null;

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
						foreach (var channel in _channel)
						{
							channel.setFrequency(_frequency);
						}
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
		public Frequency(FMOD.System system, IPlayerEngine _engine)
		{
			_system = system;
			_channel = new FMOD.Channel[_engine.ChannelCount];
			for (int i = 0; i < _engine.ChannelCount; i++)
			{
				_system.getChannel(i, out _channel[i]);
			}
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
				foreach (var channel in _channel)
				{
					channel.setFrequency(Hz);
				}
			}
			else
				if (sw == false)
				{
					foreach (var channel in _channel)
					{
						channel.setFrequency(44100);
					}
					Enabled = false;
				}

			return result;
		}
		public void SetFrequency(int value)
        {
			Hz = 44100f * ((value + 100f) / 100f);
		}
	}
}
