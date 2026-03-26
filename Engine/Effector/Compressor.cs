using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace MediaPlayer_X_Ark.Engine.Effector
{
	public class Compressor : AbstractEffectorBase
	{
		private float _threshold;
		private float _ratio;
		private float _attack;
		private float _release;
		private float _gainmakeup;
		private bool _usesidechain;
		private bool _linked;

		/// <summary>
		/// Threshold level.
		/// Type: float Units: Decibels Range: [-60, 0] Default: 0
		/// </summary>
		public float Threshold
		{
			get
			{
				return _threshold;
			}
			set
			{
				if (_threshold != value)
				{
					_threshold = Math.Clamp(value, -60.0F, 0.0F);
					SetParameterFloat((int)FMOD.DSP_COMPRESSOR.THRESHOLD, _threshold);
				}
			}
		}

		/// <summary>
		/// Compression Ratio.
		/// Type: float Units: Linear Range: [1, 50] Default: 2.5
		/// </summary>
		public float Ratio
		{
			get
			{
				return _ratio;
			}
			set
			{
				if (value != _ratio)
				{
					_ratio = Math.Clamp(value, 1.0F, 50.0F);
					SetParameterFloat((int)FMOD.DSP_COMPRESSOR.RATIO, _ratio);
				}
			}
		}

		/// <summary>
		/// Attack time.
		/// Type: float Units: Milliseconds Range: [0.1, 500] Default: 20
		/// </summary>
		public float Attack
		{
			get
			{
				return _attack;
			}
			set
			{
				if (value != _attack)
				{
					_attack = Math.Clamp(value, 0.1F, 500.0F);
					SetParameterFloat((int)FMOD.DSP_COMPRESSOR.ATTACK, _attack);
				}
			}
		}
		/// <summary>
		/// Release time.
		/// Type: float Units: Milliseconds Range: [10, 5000] Default: 100
		/// </summary>
		public float Release
		{
			get
			{
				return _release;
			}
			set
			{
				if (value != _release)
				{
					_release = Math.Clamp(value, 10.0F, 5000.0F);
					SetParameterFloat((int)FMOD.DSP_COMPRESSOR.RELEASE, _release);
				}
			}
		}
		/// <summary>
		/// Make-up gain applied after limiting.
		/// Type: float Units: Decibels Range: [-30, 30] Default: 0
		/// </summary>
		public float Gain
		{
			get
			{
				return _gainmakeup;
			}
			set
			{
				if (value != _gainmakeup)
				{
					_gainmakeup = Math.Clamp(value, -30.0F, 30.0F);
					SetParameterFloat((int)FMOD.DSP_COMPRESSOR.GAINMAKEUP, _gainmakeup);
				}

			}
		}
		/// <summary>
		/// Data of type FMOD_DSP_PARAMETER_SIDECHAIN. Whether to analyse the sidechain signal instead of the input signal. 
		/// The FMOD_DSP_PARAMETER_SIDECHAIN::sidechainenable default is false.
		/// </summary>
		public bool SideChain
		{
			get
			{
				return _usesidechain;
			}
			set
			{
				if (value != _usesidechain)
				{
					_usesidechain = value;

					FMOD.DSP_PARAMETER_SIDECHAIN sidechain = new FMOD.DSP_PARAMETER_SIDECHAIN();
					sidechain.sidechainenable = (_usesidechain) ? 1 : 0;

					byte[] dspdatabytes = new byte[Marshal.SizeOf(typeof(FMOD.DSP_PARAMETER_SIDECHAIN))];

					GCHandle pinStructure = GCHandle.Alloc(sidechain, GCHandleType.Pinned);

					try
					{
						Marshal.Copy(pinStructure.AddrOfPinnedObject(), dspdatabytes, 0, dspdatabytes.Length);
						SetParameterData((int)FMOD.DSP_COMPRESSOR.USESIDECHAIN, dspdatabytes);
					}
					finally
					{
						pinStructure.Free();
					}
				}
			}
		}

		/// <summary>
		/// false = Independent (compressor per channel), true = Linked.
		/// </summary>
		public bool Linked
		{
            get
            {
				return _linked;
            }
			set
            {
				if (value != _linked)
                {
					_linked = value;
					SetParameterBool((int)FMOD.DSP_COMPRESSOR.LINKED, value);
                }
            }
		}

        /// <summary>
        /// CREATE DSP FOR LOWPASS FILTER
        /// </summary>
        /// <param name="system"></param>
        public Compressor(FMOD.System system)
		{
			Initialize(system, FMOD.DSP_TYPE.COMPRESSOR);
		}

		/// <summary>
		/// Set default parameters.
		/// </summary>
		public override void SetDefault()
        {
			Threshold = 0;
			Ratio = 2.5f;
			Attack = 20;
			Release = 100;
			Gain = 0;
			SideChain = false;
			Linked = false;
        }
	}
}
