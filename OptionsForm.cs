using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
		}
		/// <summary>
		/// Chorus
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckChorus_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Switch(GroupControl(sender));
		}
		/// <summary>
		/// Echo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckEcho_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Switch(GroupControl(sender));
		}
		/// <summary>
		/// Highpass
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckHighpass_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.Switch(GroupControl(sender));
		}

		private void CheckLowpass_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.Switch(GroupControl(sender));
		}

		private void CheckFlanger_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Switch(GroupControl(sender));
		}
		private void CheckCompressor_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Switch(GroupControl(sender));
		}
		private void CheckPitch_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.PitchShift.Switch(GroupControl(sender));
		}
		private void CheckFrequency_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Frequency.Switch(GroupControl(sender));
		}
		private void CheckSpeed_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.SpeedEnabled = GroupControl(sender);
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
		#endregion
		private void DistortionLevel_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Distortion.Level = ((UI.Knob)sender).Value / 100F;
			lblValDistortionLevel.Text = _engine.effector.Distortion.Level.ToString("##0.00");
		}

		private void KnobChorusMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Mix = ((UI.Knob)sender).Value / 10F;
			lblValChorusMix.Text = _engine.effector.Chorus.Mix.ToString("##0.0");
		}

		private void KnobChorusRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Rate = ((UI.Knob)sender).Value / 10F;
			lblValChorusRate.Text = _engine.effector.Chorus.Rate.ToString("##0.0");
		}

		private void KnobChorusDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Depth = ((UI.Knob)sender).Value / 10F;
			lblValChorusDepth.Text = _engine.effector.Chorus.Depth.ToString("##0.0");
		}

		private void KnobEchoDelay_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Delay = ((UI.Knob)sender).Value / 10F;
			lblValEchoDelay.Text = _engine.effector.Echo.Delay.ToString("##0.0");
		}

		private void KnobEchoFeedback_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Feedback = ((UI.Knob)sender).Value / 10F;
			lblValEchoFeedback.Text = _engine.effector.Echo.Feedback.ToString("##0.0");
		}

		private void KnobEchoDry_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.DryLevel = ((UI.Knob)sender).Value / 10F;
			lblValEchoDry.Text = _engine.effector.Echo.DryLevel.ToString("##0.0");
		}

		private void KnobEchoWet_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.WetLevel = ((UI.Knob)sender).Value / 10F;
			lblValEchoWet.Text = _engine.effector.Echo.WetLevel.ToString("##0.0");
		}

		private void KnobHighpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.CutOff = ((UI.Knob)sender).Value / 10F;
			lblValHighpassCutoff.Text = _engine.effector.Highpass.CutOff.ToString("##0.0");
		}

		private void KnobHighpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValHighpassResonance.Text = _engine.effector.Highpass.Resonance.ToString("##0.0");
		}

		private void KnobLowpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.CutOff = ((UI.Knob)sender).Value / 10F;
			lblValLowpassCutoff.Text = _engine.effector.Lowpass.CutOff.ToString("##0.0");
		}

		private void KnobLowpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValLowpassResonance.Text = _engine.effector.Lowpass.Resonance.ToString("##0.0");
		}

		private void KnobFlangerMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Mix = ((UI.Knob)sender).Value / 10F;
			lblValFlangerMix.Text = _engine.effector.Flanger.Mix.ToString("##0.0");
		}

		private void KnobFlangerRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Rate = ((UI.Knob)sender).Value / 10F;
			lblValFlangerRate.Text = _engine.effector.Flanger.Rate.ToString("##0.0");
		}

		private void KnobFlangerDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Depth = ((UI.Knob)sender).Value / 100F;
			lblValFlangerDepth.Text = _engine.effector.Flanger.Depth.ToString("##0.0");
		}

		private void KnobCompThreshold_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Threshold = ((UI.Knob)sender).Value / 10F;
			lblValCompThreshold.Text = _engine.effector.Compressor.Threshold.ToString("##0.0");
		}

		private void KnobCompRatio_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Ratio = ((UI.Knob)sender).Value / 10F;
			lblValCompRatio.Text = _engine.effector.Compressor.Ratio.ToString("##0.0");
		}

		private void KnobCompAttack_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Attack = ((UI.Knob)sender).Value / 10F;
			lblValCompAttack.Text = _engine.effector.Compressor.Attack.ToString("##0.0");
		}

		private void KnobCompRelease_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Release = ((UI.Knob)sender).Value / 10F;
			lblValCompRelease.Text = _engine.effector.Compressor.Release.ToString("##0.0");
		}

		private void KnobCompGain_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Gain = ((UI.Knob)sender).Value / 10F;
			lblValCompGain.Text = _engine.effector.Compressor.Gain.ToString("##0.0");
		}

		private void KnobPitchPitch_ValueChanged(object sender, EventArgs e)
		{
			//			if (!CheckSpeed.Checked)
			{
				_engine.effector.PitchShift.Pitch = (((UI.Knob)sender).Value + 50) / 100F;
			}
			lblValPitchPitch.Text = _engine.effector.PitchShift.Pitch.ToString("##0.00");
		}

		private void KnobPitchFFT_ValueChanged(object sender, EventArgs e)
		{
			float[] fftsize = { 256, 512, 1024, 2048, 4096 };
			_engine.effector.PitchShift.FFTSize = fftsize[((UI.Knob)sender).Value];
			lblValPitchFFT.Text = _engine.effector.PitchShift.FFTSize.ToString("###0");
		}

		private void KnobFrequency_ValueChanged(object sender, EventArgs e)
		{
			//			if (!CheckSpeed.Checked)
			{
				_engine.effector.Frequency.SetFrequency(((UI.Knob)sender).Value);
			}
			lblValFrequency.Text = _engine.effector.Frequency.Hz.ToString();
		}

		private void KnobSpeed_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Speed = ((UI.Knob)sender).Value;
			lblValSpeed.Text = _engine.effector.Speed.ToString();
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
			cmbDevice.DataSource = _engine.DeviceList;
			cmbDevice.DisplayMember = "Name";
			cmbDevice.ValueMember = "GUID";
			cmbDevice.SelectedValue = _config.settings.Device;
			cmbSampleRate.SelectedIndex = _config.settings.SampleRate;
			cmbFormat.SelectedIndex = _config.settings.Format;
			cmbSampling.SelectedIndex = _config.settings.SamplingMode;
			cmbSpeaker.SelectedIndex = _config.settings.SpeakerMode;
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
			int hHeight = this.PictGEQGraph.Height / 60;
			int value = 0;
			Point[] curvePoints = new Point[14];
			curvePoints[0] = new Point(0, hCenter);
			curvePoints[13] = new Point(PictGEQGraph.Width, hCenter);
			for (int i = 0; i < 12; i++)
			{
				value = GetIndexToTrackValue(i);
				curvePoints[1 + i] = new Point(wWidth * i + wWidth, hCenter - hHeight * value);
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
					value = TrkGEQ32.Value;
					break;
				case 1:
					value = TrkGEQ60.Value;
					break;
				case 2:
					value = TrkGEQ125.Value;
					break;
				case 3: 
					value = TrkGEQ250.Value; 
					break;
				case 4: 
					value = TrkGEQ500.Value; 
					break;
				case 5: 
					value = TrkGEQ1K.Value; 
					break;
				case 6: 
					value = TrkGEQ2K.Value; 
					break;
				case 7: 
					value = TrkGEQ4K.Value; 
					break;
				case 8: 
					value = TrkGEQ8K.Value; 
					break;
				case 9: 
					value = TrkGEQ16K.Value; 
					break;
				case 10: 
					value = TrkGEQ20K.Value; 
					break;
				case 11: 
					value = TrkGEQ22K.Value; 
					break;
			}
			return value;
		}

        private void TrkGEQ32_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

        private void TrkGEQ60_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ125_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ250_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ500_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ1K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ2K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ4K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ8K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ16K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ20K_ValueChanged(object sender, EventArgs e)
        {
			PaintGEQGraph();
		}

		private void TrkGEQ22K_ValueChanged(object sender, EventArgs e)
		{
			PaintGEQGraph();
		}

        private void BtnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			e.Cancel = true;
			this.Hide();
        }
    }
}
