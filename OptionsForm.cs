using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
	public partial class OptionsForm : Form
	{
		private PlayerEngine _engine;
		private Engine.Configration _config;
		public OptionsForm(ref PlayerEngine engine, ref Engine.Configration config)
		{
			InitializeComponent();
			_engine = engine;
			_config = config;
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			TreeMenu.ExpandAll();
			OptionOutput();
			EffectControlInitialize();
			PaintGEQGraph();
			
		}
		private bool internalChanged = false;
		private void EqualizerChanged(object sender, PropertyChangedEventArgs e)
        {
			if (e.PropertyName == "Gain")
            {
				internalChanged = true;
				for(int i = 0; i < (int)Engine.Effector.GEqualizer.EQ_HZ.HZ_MAX; i++)
                {
					switch(i)
                    {
						case 0:
							TrkGEQ32.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 1:
							TrkGEQ60.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 2:
							TrkGEQ125.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 3:
							TrkGEQ250.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 4:
							TrkGEQ500.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 5:
							TrkGEQ1K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 6:
							TrkGEQ2K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 7:
							TrkGEQ4K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 8:
							TrkGEQ8K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 9:
							TrkGEQ16K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 10:
							TrkGEQ20K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
						case 11:
							TrkGEQ22K.Value = ((int)_engine.effector.GEqualizer.Gain[i] * 10);
							break;
					}
				}
				internalChanged = false;
			}
		}

		private void PitchChenged(object sender, PropertyChangedEventArgs e)
		{
			if (CheckSpeed.Checked)
			{
				CheckPitch.Checked = _engine.effector.PitchShift.Enabled;
				KnobPitchPitch.Value = (int)(_engine.effector.PitchShift.Pitch * 100) - 50;
			}
		}
		private void FrequencyChenged(object sender, PropertyChangedEventArgs e)
		{
			if (CheckSpeed.Checked)
			{
				CheckFrequency.Checked = _engine.effector.Frequency.Enabled;
				KnobFrequency.Value = (int)(_engine.effector.Frequency.Hz / 44100f * 100f - 100f);
			}
		}
		//// <summary>
		//// Effectors On/Off
		//// </summary>

		#region Effectors ON/OFF
		/// <summary>
		/// Distortion
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckDistortion_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Distortion.Switch(GroupControl(sender));
			_config.settings.Effectors.Distortion.Enable = _engine.effector.Distortion.Enabled;
		}
		/// <summary>
		/// Chorus
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckChorus_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Switch(GroupControl(sender));
			_config.settings.Effectors.Chorus.Enable = _engine.effector.Chorus.Enabled;
		}
		/// <summary>
		/// Echo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckEcho_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Switch(GroupControl(sender));
			_config.settings.Effectors.Echo.Enable = _engine.effector.Echo.Enabled;
		}
		/// <summary>
		/// Highpass
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckHighpass_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.Switch(GroupControl(sender));
			_config.settings.Effectors.Highpass.Enable = _engine.effector.Highpass.Enabled;
		}

		private void CheckLowpass_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.Switch(GroupControl(sender));
			_config.settings.Effectors.Lowpass.Enable = _engine.effector.Lowpass.Enabled;
		}

		private void CheckFlanger_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Switch(GroupControl(sender));
			_config.settings.Effectors.Flanger.Enable = _engine.effector.Flanger.Enabled;
		}
		private void CheckCompressor_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Switch(GroupControl(sender));
			_config.settings.Effectors.Compressor.Enable = _engine.effector.Compressor.Enabled;
		}
		private void CheckPitch_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.PitchShift.Switch(GroupControl(sender));
			_config.settings.Effectors.PitchShift.Enable = _engine.effector.PitchShift.Enabled;
		}
		private void CheckFrequency_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Frequency.Switch(GroupControl(sender));
			_config.settings.Effectors.Frequency.Enable = _engine.effector.Frequency.Enabled;
		}
		private void CheckSpeed_CheckedChanged(object sender, EventArgs e)
		{
			_config.settings.Effectors.Speed.Enable = _engine.effector.SpeedEnabled = GroupControl(sender);
			if (_engine.effector.SpeedEnabled)
			{
				_engine.effector.PitchShift.Switch(true);
				_engine.effector.Frequency.Switch(true);
				GroupFrequency.Enabled = false;
				GroupPitchShift.Enabled = false;
			}
			else
			{
				GroupPitchShift.Enabled = true;
				GroupFrequency.Enabled = true;
			}
		}
		private void CheckReverb_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.Switch(GroupControl(sender));
			_config.settings.Effectors.Reverb.Enable = _engine.effector.SFXReverb.Enabled;
		}

		private void CheckGEQ_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.GEqualizer.Switch(GroupControl(sender));
			_config.settings.Effectors.GEqualizer.Enable = _engine.effector.GEqualizer.Enabled;
		}
		#endregion
		private void KnobDistortionLevel_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Distortion.Level = ((UI.Knob)sender).Value / 100F;
			lblValDistortionLevel.Text = _engine.effector.Distortion.Level.ToString("##0.00");
		}

		private void KnobChorusMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Mix = ((UI.Knob)sender).Value / 10F;
			lblValChorusMix.Text = _engine.effector.Chorus.Mix.ToString("##0.0");
			_config.settings.Effectors.Chorus.Mix = ((UI.Knob)sender).Value;
		}

		private void KnobChorusRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Rate = ((UI.Knob)sender).Value / 10F;
			lblValChorusRate.Text = _engine.effector.Chorus.Rate.ToString("##0.0");
			_config.settings.Effectors.Chorus.Rate = ((UI.Knob)sender).Value;
		}

		private void KnobChorusDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Depth = ((UI.Knob)sender).Value / 10F;
			lblValChorusDepth.Text = _engine.effector.Chorus.Depth.ToString("##0.0");
			_config.settings.Effectors.Chorus.Depth = ((UI.Knob)sender).Value;
		}

		private void KnobEchoDelay_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Delay = ((UI.Knob)sender).Value / 10F;
			lblValEchoDelay.Text = _engine.effector.Echo.Delay.ToString("##0.0");
			_config.settings.Effectors.Echo.Delay = ((UI.Knob)sender).Value;
		}

		private void KnobEchoFeedback_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Feedback = ((UI.Knob)sender).Value / 10F;
			lblValEchoFeedback.Text = _engine.effector.Echo.Feedback.ToString("##0.0");
			_config.settings.Effectors.Echo.Feedback = ((UI.Knob)sender).Value;
		}

		private void KnobEchoDry_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.DryLevel = ((UI.Knob)sender).Value / 10F;
			lblValEchoDry.Text = _engine.effector.Echo.DryLevel.ToString("##0.0");
			_config.settings.Effectors.Echo.Dry = ((UI.Knob)sender).Value;
		}

		private void KnobEchoWet_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.WetLevel = ((UI.Knob)sender).Value / 10F;
			lblValEchoWet.Text = _engine.effector.Echo.WetLevel.ToString("##0.0");
			_config.settings.Effectors.Echo.Wet = ((UI.Knob)sender).Value;
		}

		private void KnobHighpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.CutOff = ((UI.Knob)sender).Value / 10F;
			lblValHighpassCutoff.Text = _engine.effector.Highpass.CutOff.ToString("##0.0");
			_config.settings.Effectors.Highpass.Cutoff = ((UI.Knob)sender).Value;
		}

		private void KnobHighpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValHighpassResonance.Text = _engine.effector.Highpass.Resonance.ToString("##0.0");
			_config.settings.Effectors.Highpass.Resonance = ((UI.Knob)sender).Value;
		}

		private void KnobLowpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.CutOff = ((UI.Knob)sender).Value / 10F;
			lblValLowpassCutoff.Text = _engine.effector.Lowpass.CutOff.ToString("##0.0");
			_config.settings.Effectors.Lowpass.Cutoff = ((UI.Knob)sender).Value;
		}

		private void KnobLowpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValLowpassResonance.Text = _engine.effector.Lowpass.Resonance.ToString("##0.0");
			_config.settings.Effectors.Lowpass.Resonance = ((UI.Knob)sender).Value;
		}

		private void KnobFlangerMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Mix = ((UI.Knob)sender).Value / 10F;
			lblValFlangerMix.Text = _engine.effector.Flanger.Mix.ToString("##0.0");
			_config.settings.Effectors.Flanger.Mix = ((UI.Knob)sender).Value;
		}

		private void KnobFlangerRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Rate = ((UI.Knob)sender).Value / 10F;
			lblValFlangerRate.Text = _engine.effector.Flanger.Rate.ToString("##0.0");
			_config.settings.Effectors.Flanger.Rate = ((UI.Knob)sender).Value;
		}

		private void KnobFlangerDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Depth = ((UI.Knob)sender).Value / 100F;
			lblValFlangerDepth.Text = _engine.effector.Flanger.Depth.ToString("##0.0");
			_config.settings.Effectors.Flanger.Depth = ((UI.Knob)sender).Value;
		}

		private void KnobCompThreshold_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Threshold = ((UI.Knob)sender).Value / 10F;
			lblValCompThreshold.Text = _engine.effector.Compressor.Threshold.ToString("##0.0");
			_config.settings.Effectors.Compressor.Threshold = ((UI.Knob)sender).Value;
		}

		private void KnobCompRatio_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Ratio = ((UI.Knob)sender).Value / 10F;
			lblValCompRatio.Text = _engine.effector.Compressor.Ratio.ToString("##0.0");
			_config.settings.Effectors.Compressor.Ratio = ((UI.Knob)sender).Value;
		}

		private void KnobCompAttack_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Attack = ((UI.Knob)sender).Value / 10F;
			lblValCompAttack.Text = _engine.effector.Compressor.Attack.ToString("##0.0");
			_config.settings.Effectors.Compressor.Attack = ((UI.Knob)sender).Value;
		}

		private void KnobCompRelease_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Release = ((UI.Knob)sender).Value / 10F;
			lblValCompRelease.Text = _engine.effector.Compressor.Release.ToString("##0.0");
			_config.settings.Effectors.Compressor.Release = ((UI.Knob)sender).Value;
		}

		private void KnobCompGain_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Gain = ((UI.Knob)sender).Value / 10F;
			lblValCompGain.Text = _engine.effector.Compressor.Gain.ToString("##0.0");
			_config.settings.Effectors.Compressor.Gain = ((UI.Knob)sender).Value;
		}

		private void KnobPitchPitch_ValueChanged(object sender, EventArgs e)
		{
			//			if (!CheckSpeed.Checked)
			{
				_engine.effector.PitchShift.Pitch = (((UI.Knob)sender).Value + 50) / 100F;
			}
			lblValPitchPitch.Text = _engine.effector.PitchShift.Pitch.ToString("##0.00");
			_config.settings.Effectors.PitchShift.Pitch = ((UI.Knob)sender).Value;
		}

		private void KnobPitchFFT_ValueChanged(object sender, EventArgs e)
		{
			float[] fftsize = { 256, 512, 1024, 2048, 4096 };
			_engine.effector.PitchShift.FFTSize = fftsize[((UI.Knob)sender).Value];
			lblValPitchFFT.Text = _engine.effector.PitchShift.FFTSize.ToString("###0");
			_config.settings.Effectors.PitchShift.FFT = ((UI.Knob)sender).Value;
		}

		private void KnobFrequency_ValueChanged(object sender, EventArgs e)
		{
			//			if (!CheckSpeed.Checked)
			{
				_engine.effector.Frequency.SetFrequency(((UI.Knob)sender).Value);
			}
			lblValFrequency.Text = _engine.effector.Frequency.Hz.ToString();
			_config.settings.Effectors.Frequency.Frequency = ((UI.Knob)sender).Value;
		}

		private void KnobSpeed_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Speed = ((UI.Knob)sender).Value;
			lblValSpeed.Text = _engine.effector.Speed.ToString();
			_config.settings.Effectors.Speed.Speed = ((UI.Knob)sender).Value;
		}

		private bool GroupControl(object sender)
		{
			if (sender.GetType() == typeof(CheckBox))
			{
				foreach (Control control in ((CheckBox)sender).Parent.Controls)
				{
					if (control != sender)
					{
						control.Enabled = ((CheckBox)sender).Checked;
					}
				}
			}
			return ((CheckBox)sender).Checked;
		}

		private void EffectControlInitialize()
		{
			// Equalizer
			_engine.effector.GEqualizer.PropertyChanged += new PropertyChangedEventHandler(EqualizerChanged);

			// Distortion
			CheckDistortion.Checked = _engine.effector.Distortion.Enabled;
			// Min:0.0	- Max:1.0		Def:0.5
			KnobDistortionLevel.Maximum = 100;
			KnobDistortionLevel.Minimum = 0;
			KnobDistortionLevel.LargeChange = 10;
			KnobDistortionLevel.Value = (int)(_engine.effector.Distortion.Level * 100);
			KnobDistortionLevel.Refresh();

			// Chorus
			CheckChorus.Checked = _engine.effector.Chorus.Enabled;
			// Min:0.0	- Max:100.0		Def:50.0
			KnobChorusMix.Maximum = 1000;
			KnobChorusMix.Minimum = 0;
			KnobChorusMix.LargeChange = 10;
			KnobChorusMix.Value = (int)(_engine.effector.Chorus.Mix * 10);
			KnobChorusMix.Refresh();
			// Min:0.0	- Max:20.00		Def:0.8
			KnobChorusRate.Maximum = 200;
			KnobChorusRate.Minimum = 0;
			KnobChorusRate.LargeChange = 10;
			KnobChorusRate.Value = (int)(_engine.effector.Chorus.Rate * 10);
			KnobChorusRate.Refresh();
			// Min:0.0	- Max:100.0		Def:3.00
			KnobChorusDepth.Maximum = 1000;
			KnobChorusDepth.Minimum = 0;
			KnobChorusDepth.LargeChange = 10;
			KnobChorusDepth.Value = (int)(_engine.effector.Chorus.Depth * 10);
			KnobChorusDepth.Refresh();

			// Echo
			CheckEcho.Checked = _engine.effector.Echo.Enabled;
			// Min:1.0	- Max:5000		Def:500.0
			KnobEchoDelay.Maximum = 50000;
			KnobEchoDelay.Minimum = 0;
			KnobEchoDelay.LargeChange = 2000;
			KnobEchoDelay.Value = (int)(_engine.effector.Echo.Delay * 10);
			KnobEchoDelay.Refresh();
			// Min:0.0	- Max:100.0		Def:50
			KnobEchoFeedback.Maximum = 1000;
			KnobEchoFeedback.Minimum = 0;
			KnobEchoFeedback.LargeChange = 100;
			KnobEchoFeedback.Value = (int)(_engine.effector.Echo.Feedback * 10);
			KnobEchoFeedback.Refresh();
			// Min:-80  - Max:10.0		Def:0
			KnobEchoDry.Maximum = 100;
			KnobEchoDry.Minimum = -800;
			KnobEchoDry.LargeChange = 10;
			KnobEchoDry.Value = (int)(_engine.effector.Echo.DryLevel * 10);
			KnobEchoDry.Refresh();
			// Min:-80  - Max:10.0		Def:0
			KnobEchoWet.Maximum = 100;
			KnobEchoWet.Minimum = -800;
			KnobEchoWet.LargeChange = 10;
			KnobEchoWet.Value = (int)(_engine.effector.Echo.WetLevel * 10);
			KnobEchoWet.Refresh();

			// Flanger
			CheckFlanger.Checked = _engine.effector.Flanger.Enabled;
			// Min:0	- Max:100.0		Def:50
			KnobFlangerMix.Maximum = 1000;
			KnobFlangerMix.Minimum = 0;
			KnobFlangerMix.LargeChange = 10;
			KnobFlangerMix.Value = (int)(_engine.effector.Flanger.Mix * 10);
			// Min:0	- Max:20.0		Def:0.1
			KnobFlangerRate.Maximum = 200;
			KnobFlangerRate.Minimum = 0;
			KnobFlangerRate.LargeChange = 10;
			KnobFlangerRate.Value = (int)(_engine.effector.Flanger.Rate * 10);
			// Min:0.01 - Max:1.0		Def:1
			KnobFlangerDepth.Maximum = 100;
			KnobFlangerDepth.Minimum = 1;
			KnobFlangerDepth.LargeChange = 10;
			KnobFlangerDepth.Value = (int)(_engine.effector.Flanger.Depth * 100);

			// Highpass
			CheckHighpass.Checked = _engine.effector.Highpass.Enabled;
			// Min:1	- Max:22000		Def:5000
			KnobHighpassCutoff.Maximum = 220000;
			KnobHighpassCutoff.Minimum = 1;
			KnobHighpassCutoff.LargeChange = 1000;
			KnobHighpassCutoff.Value = (int)(_engine.effector.Highpass.CutOff * 10);
			// Min:0	- Max:10		Def:1
			KnobHighpassResonance.Maximum = 100;
			KnobHighpassResonance.Minimum = 0;
			KnobHighpassResonance.LargeChange = 10;
			KnobHighpassResonance.Value = (int)(_engine.effector.Highpass.Resonance * 10);

			// Lowpass
			CheckLowpass.Checked = _engine.effector.Lowpass.Enabled;
			// Min:1	- Max:22000		Def:5000
			KnobLowpassCutoff.Maximum = 220000;
			KnobLowpassCutoff.Minimum = 1;
			KnobLowpassCutoff.LargeChange = 1000;
			KnobLowpassCutoff.Value = (int)(_engine.effector.Lowpass.CutOff * 10);
			// Min:0	- Max:10		Def:1
			KnobLowpassResonance.Maximum = 100;
			KnobLowpassResonance.Minimum = 0;
			KnobLowpassResonance.LargeChange = 10;
			KnobLowpassResonance.Value = (int)(_engine.effector.Lowpass.Resonance * 10);

			// Compressor
			CheckCompressor.Checked = _engine.effector.Compressor.Enabled;
			// Min:-60	- Max:0			Def:0
			KnobCompThreshold.Maximum = 0;
			KnobCompThreshold.Minimum = -600;
			KnobCompThreshold.LargeChange = 10;
			KnobCompThreshold.Value = (int)(_engine.effector.Compressor.Threshold * 10);
			// Min:1	- Max:50		Def:2.5
			KnobCompRatio.Maximum = 500;
			KnobCompRatio.Minimum = 1;
			KnobCompRatio.LargeChange = 10;
			KnobCompRatio.Value = (int)(_engine.effector.Compressor.Ratio * 10);
			// Min:0.1	- Max:500		Def:20
			KnobCompAttack.Maximum = 5000;
			KnobCompAttack.Minimum = 1;
			KnobCompAttack.LargeChange = 10;
			KnobCompAttack.Value = (int)(_engine.effector.Compressor.Attack * 10);
			// Min:10	- Max:5000		Def:100
			KnobCompRelease.Maximum = 50000;
			KnobCompRelease.Minimum = 100;
			KnobCompRelease.LargeChange = 10;
			KnobCompRelease.Value = (int)(_engine.effector.Compressor.Release * 10);
			// Min:-30	- Max:30		Def:0
			KnobCompGain.Maximum = 300;
			KnobCompGain.Minimum = -300;
			KnobCompGain.LargeChange = 10;
			KnobCompGain.Value = (int)(_engine.effector.Compressor.Gain * 10);

			// PichShift
			CheckPitch.Checked = _engine.effector.PitchShift.Enabled;
			// Min:0.5	- Max:2.0	Def:1
			_engine.effector.PitchShift.PropertyChanged += new PropertyChangedEventHandler(PitchChenged);
			KnobPitchPitch.Maximum = 150;
			KnobPitchPitch.Minimum = 0;
			KnobPitchPitch.LargeChange = 1;
			KnobPitchPitch.Value = (int)(_engine.effector.PitchShift.Pitch * 100) - 50;

			// Min:0.5	- Max:2.0	Def:1
			KnobPitchFFT.Maximum = 4;
			KnobPitchFFT.Minimum = 0;
			KnobPitchFFT.LargeChange = 1;
			float[] fftsize = { 256, 512, 1024, 2048, 4096 };
			KnobPitchFFT.Value = Array.IndexOf(fftsize, _engine.effector.PitchShift.FFTSize);

			CheckFrequency.Checked = _engine.effector.Frequency.Enabled;
			_engine.effector.Frequency.PropertyChanged += new PropertyChangedEventHandler(FrequencyChenged);
			CheckSpeed.Checked = _engine.effector.SpeedEnabled;

			// Def:false
			//			CheckCompLinked.Checked = (bool)(_engine.effector.Compressor.Linked);
			// Def:false
			//			CheckCompSidechain.Checked = (bool)(_engine.effector.Compressor.SideChain);

			// SFX Reverb
			this.Refresh();

		}
		private void OptionOutput()
		{
			cmbOutput.SelectedIndex = _config.settings.OutputType;
            //			cmbOutput.Items.Add("WAVEファイル出力");
            _engine.GetDeviceList();
            cmbDevice.DataSource = _engine.DeviceList;
			cmbDevice.DisplayMember = "Name";
			cmbDevice.ValueMember = "GUID";
			cmbDevice.SelectedValue = _config.settings.Device;
			cmbSampleRate.SelectedIndex = _config.settings.SampleRate;
			cmbFormat.SelectedIndex = _config.settings.Format;
			cmbSampling.SelectedIndex = _config.settings.SamplingMode;
			cmbSpeaker.SelectedIndex = _config.settings.SpeakerMode;

			// Equalizer
			CheckGEQ.Checked = _config.settings.Effectors.GEqualizer.Enable;
			cmbEqPreset.SelectedIndex = _config.settings.Effectors.GEqualizer.Preset;
			TrkGEQ32.Value = _config.settings.Effectors.GEqualizer.GEQ_32;
			TrkGEQ60.Value = _config.settings.Effectors.GEqualizer.GEQ_60;
			TrkGEQ125.Value = _config.settings.Effectors.GEqualizer.GEQ_125;
			TrkGEQ250.Value = _config.settings.Effectors.GEqualizer.GEQ_250;
			TrkGEQ500.Value = _config.settings.Effectors.GEqualizer.GEQ_500;
			TrkGEQ1K.Value = _config.settings.Effectors.GEqualizer.GEQ_1K;
			TrkGEQ2K.Value = _config.settings.Effectors.GEqualizer.GEQ_2K;
			TrkGEQ4K.Value = _config.settings.Effectors.GEqualizer.GEQ_4K;
			TrkGEQ8K.Value = _config.settings.Effectors.GEqualizer.GEQ_8K;
			TrkGEQ16K.Value = _config.settings.Effectors.GEqualizer.GEQ_16K;
			TrkGEQ20K.Value = _config.settings.Effectors.GEqualizer.GEQ_20K;
			TrkGEQ22K.Value = _config.settings.Effectors.GEqualizer.GEQ_22K;


		}
		private void TreeMenu_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Name == "OUTPUT")
			{
				tabControlEffects.SelectedIndex = 0;
			}
			if (e.Node.Name == "GEQ")
			{
				tabControlEffects.SelectedIndex = 1;
			}
			else if (e.Node.Name == "PITCH")
			{
				tabControlEffects.SelectedIndex = 2;
			}
			else if (e.Node.Name == "DISTORTION")
			{
				tabControlEffects.SelectedIndex = 3;
			}
			else if (e.Node.Name == "CHORUS")
			{
				tabControlEffects.SelectedIndex = 4;
			}
			else if (e.Node.Name == "ECHO")
			{
				tabControlEffects.SelectedIndex = 5;
			}
			else if (e.Node.Name == "FLANGER")
			{
				tabControlEffects.SelectedIndex = 6;
			}
			else if (e.Node.Name == "HIGHPASS")
			{
				tabControlEffects.SelectedIndex = 7;
			}
			else if (e.Node.Name == "LOWPASS")
			{
				tabControlEffects.SelectedIndex = 8;
			}
			else if (e.Node.Name == "COMPRESSOR")
			{
				tabControlEffects.SelectedIndex = 9;
			}
			else if (e.Node.Name == "REVERB")
			{
				tabControlEffects.SelectedIndex = 10;
			}
		}

		private void PaintGEQGraph()
		{
			//描画先とするImageオブジェクトを作成する
			Bitmap canvas = new Bitmap(PictGEQGraph.Width, PictGEQGraph.Height);
			//ImageオブジェクトのGraphicsオブジェクトを作成する
			Graphics g = Graphics.FromImage(canvas);

			int hCenter = this.PictGEQGraph.Height / 2;
			int wWidth = this.PictGEQGraph.Width / 13;
			float hHeight = this.PictGEQGraph.Height / 200f;
			int value = 0;
			Point[] curvePoints = new Point[14];
			curvePoints[0] = new Point(0, hCenter);
			curvePoints[13] = new Point(PictGEQGraph.Width, hCenter);
			for (int i = 0; i < 12; i++)
			{
				value = GetIndexToTrackValue(i);
				curvePoints[1 + i] = new Point(wWidth * i + wWidth, hCenter - (int)(hHeight * value));
			}

			//幅3の赤色のPenオブジェクトを作成
			Pen pen = new Pen(Color.Black, 1);
			//テンション1のカーディナルスプラインを描画
			g.DrawCurve(pen, curvePoints, 0.5f);

			//リソースを解放する
			pen.Dispose();
			g.Dispose();

			PictGEQGraph.Image = canvas;
		}

		private int GetIndexToTrackValue(int index)
		{
			int value = 0;
			switch (index)
			{
				case 0:
					value = Convert.ToInt32(TrkGEQ32.Value);
					break;
				case 1:
					value = Convert.ToInt32(TrkGEQ60.Value);
					break;
				case 2:
					value = Convert.ToInt32(TrkGEQ125.Value);
					break;
				case 3: 
					value = Convert.ToInt32(TrkGEQ250.Value); 
					break;
				case 4: 
					value = Convert.ToInt32(TrkGEQ500.Value); 
					break;
				case 5: 
					value = Convert.ToInt32(TrkGEQ1K.Value); 
					break;
				case 6: 
					value = Convert.ToInt32(TrkGEQ2K.Value); 
					break;
				case 7: 
					value = Convert.ToInt32(TrkGEQ4K.Value); 
					break;
				case 8: 
					value = Convert.ToInt32(TrkGEQ8K.Value); 
					break;
				case 9: 
					value = Convert.ToInt32(TrkGEQ16K.Value); 
					break;
				case 10: 
					value = Convert.ToInt32(TrkGEQ20K.Value); 
					break;
				case 11: 
					value = Convert.ToInt32(TrkGEQ22K.Value); 
					break;
			}
			return value;
		}

        private void TrkGEQ32_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_32, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_32 = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ60_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_60, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_60 = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ125_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_125, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_125 = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ250_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_250, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_250 = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ500_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_500, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_500 = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ1K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_1K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_1K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ2K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_2K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_2K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ4K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_4K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_4K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ8K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_8K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_8K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ16K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_16K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_16K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ20K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_20K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_20K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void TrkGEQ22K_ValueChanged(object sender, EventArgs e)
		{
			PaintGEQGraph();
			if (!internalChanged)
				_engine.effector.GEqualizer.SetGain(Engine.Effector.GEqualizer.EQ_HZ.HZ_22K, (float)((ColorSlider.ColorSlider)sender).Value / 10f);
			_config.settings.Effectors.GEqualizer.GEQ_22K = ((ColorSlider.ColorSlider)sender).Value;
		}

		private void BtnUpdate_Click(object sender, EventArgs e)
        {
			// デバイスはPlayLoad()で反映されるため、ここでの即時反映は不要
			// OutputType/SampleRate/SpeakerModeは次回起動時に反映
			bool requiresRestart =
				_config.settings.OutputType != cmbOutput.SelectedIndex ||
				_config.settings.SampleRate != cmbSampleRate.SelectedIndex ||
				_config.settings.SpeakerMode != cmbSpeaker.SelectedIndex;

			if (cmbDevice.Enabled)
				_config.settings.Device = cmbDevice.SelectedValue.ToString();
			_config.settings.SampleRate = cmbSampleRate.SelectedIndex;
			_config.settings.OutputType = cmbOutput.SelectedIndex;
			_config.settings.SamplingMode = cmbSampling.SelectedIndex;
			_config.settings.SpeakerMode = cmbSpeaker.SelectedIndex;
			_config.Save();

			string message = requiresRestart
				? "設定を保存しました。\n出力形式・サンプルレート・スピーカーモードは次回起動時に反映されます。"
				: "設定を保存しました。\nデバイスは次回再生時に反映されます。";

			MessageBox.Show(message, "設定保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			e.Cancel = true;
			this.Hide();
        }

        private void cmbEqPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (_engine.effector != null)
			_engine.effector.GEqualizer.SetPreset(((ComboBox)sender).SelectedIndex);
			_config.settings.Effectors.GEqualizer.Preset = ((ComboBox)sender).SelectedIndex;
		}

        private void CheckCompLinked_CheckedChanged(object sender, EventArgs e)
        {
			_engine.effector.Compressor.Linked = ((CheckBox)sender).Checked;
		}

		private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
		{
			var newOutputType = IndexToOutputType(((ComboBox)sender).SelectedIndex);
			var currentOutputType = _engine.GetOutputType();

			// ASIO起動中に別の出力タイプへ変更しようとした場合
			// → ブロックせず、注意ラベルを表示して続行
			if (currentOutputType == FMOD.OUTPUTTYPE.ASIO &&
				newOutputType != FMOD.OUTPUTTYPE.ASIO)
			{
				lblOutputNote.Visible = true;
				lblOutputNote.Text = "※ ASIO起動中のため次回起動時に反映されます。";
			}
			else
			{
				lblOutputNote.Visible = false;
			}

			// 保存済みの出力タイプと一致する場合のみ保存済みGUIDを復元候補にする
			string preferredGuid = (cmbOutput.SelectedIndex == _config.settings.OutputType)
				? _config.settings.Device
				: null;

			RefreshDeviceList(newOutputType, preferredGuid);
		}
		private void RefreshDeviceList(FMOD.OUTPUTTYPE outputType, string preferredGuid)
		{
			cmbDevice.DataSource = null;

			List<DEVICE_INFO> devices;

			// ASIO起動中のASIOデバイス列挙は現在のFMODSystemを直接使う
			// （テンポラリSystemを作ると競合する可能性があるため）
			if (_engine.GetOutputType() == FMOD.OUTPUTTYPE.ASIO &&
				outputType == FMOD.OUTPUTTYPE.ASIO)
			{
				devices = _engine.GetCurrentDeviceList();
			}
			else
			{
				devices = _engine.GetDeviceListForOutputType(outputType);
			}

			if (devices.Count == 0)
			{
				cmbDevice.Enabled = false;
				cmbDevice.Items.Clear();
				cmbDevice.Items.Add("（デバイスなし）");
				cmbDevice.SelectedIndex = 0;
				return;
			}

			cmbDevice.Enabled = true;
			cmbDevice.DataSource = devices;
			cmbDevice.DisplayMember = "Name";
			cmbDevice.ValueMember = "GUID";

			bool found = preferredGuid != null
				&& devices.Any(d => d.GUID == preferredGuid);

			if (found)
				cmbDevice.SelectedValue = preferredGuid;
			else
				cmbDevice.SelectedIndex = 0;
		}
		private FMOD.OUTPUTTYPE IndexToOutputType(int index)
		{
			switch (index)
			{
				case 1: return FMOD.OUTPUTTYPE.WASAPI;
				case 2: return FMOD.OUTPUTTYPE.ASIO;
				case 3: return FMOD.OUTPUTTYPE.WINSONIC;
				default: return FMOD.OUTPUTTYPE.AUTODETECT;
			}
		}
		private int OutputTypeToIndex(FMOD.OUTPUTTYPE type)
		{
			switch (type)
			{
				case FMOD.OUTPUTTYPE.WASAPI: return 1;
				case FMOD.OUTPUTTYPE.ASIO: return 2;
				case FMOD.OUTPUTTYPE.WINSONIC: return 3;
				default: return 0;
			}
		}
		private void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
		{
            //_engine.SetDevice((string)((ComboBox)sender).SelectedValue.ToString());
        }
	}
}
