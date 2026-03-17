using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Skin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
		private MainForm _mainForm; // 追加
        private static readonly string[] _geqBuiltinPresets =
        {
			"Normal", "Rock", "Pop", "Bass Boost",
			"Trable Boost", "Total Boost", "Total Reduce", "Custom"
		};
        public OptionsForm(ref PlayerEngine engine, ref Engine.Configration config, MainForm mainForm)
		{
			InitializeComponent();
			_engine = engine;
			_config = config;
			_mainForm = mainForm; // 追加

            LoadGEQPresets();
            LoadChorusPresets();
            LoadDistortionPresets();
            LoadEchoPresets();
            LoadFlangerPresets();
            LoadHighpassPresets();
            LoadLowpassPresets();
            LoadCompressorPresets();
            LoadReverbPresets();
            LoadPitchPresets();
        }

        private void LoadEffectPresets<T>(ComboBox cmb, string effectName)
    where T : EffectPreset
        {
            cmb.Items.Clear();
            cmb.Items.Add(""); // 空選択
            foreach (var name in EffectPreset.GetPresetNames(effectName))
                cmb.Items.Add(name);

            if (_config.settings.EffectPresets.TryGetValue(effectName, out var current))
                cmb.SelectedItem = current;
        }

        private void SaveEffectPreset<T>(
            ComboBox cmb,
            string effectName,
            Func<string, T> createPreset)
            where T : EffectPreset
        {
            using (var form = new PresetNameInputForm())
            {
                if (form.ShowDialog() != DialogResult.OK) return;
                var preset = createPreset(form.PresetNameValue);
                preset.Save();
                LoadEffectPresets<T>(cmb, effectName);
                cmb.SelectedItem = preset.Name;
                _config.settings.EffectPresets[effectName] = preset.Name;
            }
        }

        private void DeleteEffectPreset<T>(
            ComboBox cmb,
            string effectName)
            where T : EffectPreset, new()
        {
            var name = cmb.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;

            if (MessageBox.Show($"プリセット「{name}」を削除しますか？",
                "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            new T { Name = name }.Delete();
            _config.settings.EffectPresets.Remove(effectName);
            LoadEffectPresets<T>(cmb, effectName);
        }


        private void LoadGEQPresets()
        {
            // 組み込みプリセットは保持したままユーザープリセットを追記
            // まず組み込み以外を削除してからユーザープリセットを追加
            for (int i = cmbEqPreset.Items.Count - 1; i >= 0; i--)
            {
                var item = cmbEqPreset.Items[i] as string;
                if (!_geqBuiltinPresets.Contains(item))
                    cmbEqPreset.Items.RemoveAt(i);
            }

            foreach (var name in EffectPreset.GetPresetNames("GEQ"))
                if (!_geqBuiltinPresets.Contains(name))
                    cmbEqPreset.Items.Add(name);

            // config から現在のプリセットを復元
            if (_config.settings.EffectPresets.TryGetValue("GEQ", out var current))
                cmbEqPreset.SelectedItem = current;
        }

        private void BtnGEQPresetSave_Click(object sender, EventArgs e)
        {
            //using (var form = new PresetNameInputForm())
            //{
            //    if (form.ShowDialog() != DialogResult.OK) return;
            //    if (_geqBuiltinPresets.Contains(form.PresetName))
            //    {
            //        MessageBox.Show("組み込みプリセット名は使用できません。",
            //            "保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    var preset = new GEqualizerPreset
            //    {
            //        Name = form.PresetName,
            //        Hz32 = (float)TrkGEQ32.Value,
            //        Hz60 = (float)TrkGEQ60.Value,
            //        Hz125 = (float)TrkGEQ125.Value,
            //        Hz250 = (float)TrkGEQ250.Value,
            //        Hz500 = (float)TrkGEQ500.Value,
            //        Hz1K = (float)TrkGEQ1K.Value,
            //        Hz2K = (float)TrkGEQ2K.Value,
            //        Hz4K = (float)TrkGEQ4K.Value,
            //        Hz8K = (float)TrkGEQ8K.Value,
            //        Hz16K = (float)TrkGEQ16K.Value,
            //        Hz20K = (float)TrkGEQ20K.Value,
            //        Hz22K = (float)TrkGEQ22K.Value,
            //    };
            //    preset.Save();
            //    LoadGEQPresets();
            //    cmbEqPreset.SelectedItem = preset.Name;
            //    _config.settings.EffectPresets["GEQ"] = preset.Name;
            //}
        }

        private void BtnGEQPresetDelete_Click(object sender, EventArgs e)
        {
            var name = cmbEqPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;

            // 組み込みプリセットは削除不可
            if (_geqBuiltinPresets.Contains(name))
            {
                MessageBox.Show("組み込みプリセットは削除できません。",
                    "削除エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"プリセット「{name}」を削除しますか？",
                "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            new GEqualizerPreset { Name = name }.Delete();
            _config.settings.EffectPresets.Remove("GEQ");
            LoadGEQPresets();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
		{
			TreeMenu.ExpandAll();
			OptionOutput();
			OptionSkin(); // 追加
			EffectControlInitialize();
			PaintGEQGraph();

            // AssemblyInfo から情報を取得して表示
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);

            lblAboutAppName.Text = info.ProductName ?? "MediaPlayer X-Ark";
            lblAboutVersion.Text = "Version " + (info.ProductVersion ?? "1.0.0.0");
            lblAboutCopyright.Text = info.LegalCopyright ?? "";
            lblAboutCompany.Text = info.CompanyName ?? "";
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
			// Speed ON時のみKnob表示を同期（変換式修正）
			if (CheckSpeed.Checked)
			{
				KnobPitchPitch.Value = (int)(_engine.effector.PitchShift.Pitch * 100);
				lblValPitchPitch.Text = _engine.effector.PitchShift.Pitch.ToString("0.00");
			}
		}
		private void FrequencyChenged(object sender, PropertyChangedEventArgs e)
		{
			if (CheckSpeed.Checked)
			{
				// SetFrequency の逆算：Hz = 44100 * (value+100)/100 → value = Hz/44100*100 - 100
				int knobVal = (int)(_engine.effector.Frequency.Hz / 44100f * 100f - 100f);
				KnobFrequency.Value = Math.Max(KnobFrequency.Minimum, Math.Min(KnobFrequency.Maximum, knobVal));
				lblValFrequency.Text = _engine.effector.Frequency.Hz.ToString("0");
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

			// ON時：Knobの現在値をDSPに再適用
			if (_engine.effector.PitchShift.Enabled)
				_engine.effector.PitchShift.Pitch = KnobPitchPitch.Value / 100f;
		}
		private void CheckFrequency_CheckedChanged(object sender, EventArgs e)
		{
			_engine.effector.Frequency.Switch(GroupControl(sender));
			_config.settings.Effectors.Frequency.Enable = _engine.effector.Frequency.Enabled;

			// ON時：Knobの現在値をDSPに再適用
			if (_engine.effector.Frequency.Enabled)
				_engine.effector.Frequency.SetFrequency(KnobFrequency.Value);
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
				KnobSpeed.Enabled = true;   // Speed Knob有効化
			}
			else
			{
				GroupPitchShift.Enabled = true;
				GroupFrequency.Enabled = true;
				KnobSpeed.Enabled = false;  // Speed Knob無効化
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
		// Distortion
		private void KnobDistortionLevel_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Distortion.Level = ((UI.Knob)sender).Value / 100F;
			lblValDistortionLevel.Text = _engine.effector.Distortion.Level.ToString("0.00");
		}

		// Chorus
		private void KnobChorusMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Mix = ((UI.Knob)sender).Value;
			lblValChorusMix.Text = _engine.effector.Chorus.Mix.ToString("0");
			_config.settings.Effectors.Chorus.Mix = ((UI.Knob)sender).Value;
		}
		private void KnobChorusRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Rate = ((UI.Knob)sender).Value / 10F;
			lblValChorusRate.Text = _engine.effector.Chorus.Rate.ToString("0.0");
			_config.settings.Effectors.Chorus.Rate = ((UI.Knob)sender).Value;
		}
		private void KnobChorusDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Chorus.Depth = ((UI.Knob)sender).Value;
			lblValChorusDepth.Text = _engine.effector.Chorus.Depth.ToString("0");
			_config.settings.Effectors.Chorus.Depth = ((UI.Knob)sender).Value;
		}

		// Echo
		private void KnobEchoDelay_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Delay = ((UI.Knob)sender).Value;
			lblValEchoDelay.Text = _engine.effector.Echo.Delay.ToString("0");
			_config.settings.Effectors.Echo.Delay = ((UI.Knob)sender).Value;
		}
		private void KnobEchoFeedback_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.Feedback = ((UI.Knob)sender).Value;
			lblValEchoFeedback.Text = _engine.effector.Echo.Feedback.ToString("0");
			_config.settings.Effectors.Echo.Feedback = ((UI.Knob)sender).Value;
		}
		private void KnobEchoDry_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.DryLevel = ((UI.Knob)sender).Value;
			lblValEchoDry.Text = _engine.effector.Echo.DryLevel.ToString("0");
			_config.settings.Effectors.Echo.Dry = ((UI.Knob)sender).Value;
		}
		private void KnobEchoWet_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Echo.WetLevel = ((UI.Knob)sender).Value;
			lblValEchoWet.Text = _engine.effector.Echo.WetLevel.ToString("0");
			_config.settings.Effectors.Echo.Wet = ((UI.Knob)sender).Value;
		}

		// Flanger
		private void KnobFlangerMix_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Mix = ((UI.Knob)sender).Value;
			lblValFlangerMix.Text = _engine.effector.Flanger.Mix.ToString("0");
			_config.settings.Effectors.Flanger.Mix = ((UI.Knob)sender).Value;
		}
		private void KnobFlangerRate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Rate = ((UI.Knob)sender).Value / 10F;
			lblValFlangerRate.Text = _engine.effector.Flanger.Rate.ToString("0.0");
			_config.settings.Effectors.Flanger.Rate = ((UI.Knob)sender).Value;
		}
		private void KnobFlangerDepth_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Flanger.Depth = ((UI.Knob)sender).Value / 100F;
			lblValFlangerDepth.Text = _engine.effector.Flanger.Depth.ToString("0.00");
			_config.settings.Effectors.Flanger.Depth = ((UI.Knob)sender).Value;
		}
		// Highpass
		private void KnobHighpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.CutOff = ((UI.Knob)sender).Value;
			lblValHighpassCutoff.Text = _engine.effector.Highpass.CutOff.ToString("0");
			_config.settings.Effectors.Highpass.Cutoff = ((UI.Knob)sender).Value;
		}
		private void KnobHighpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Highpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValHighpassResonance.Text = _engine.effector.Highpass.Resonance.ToString("0.0");
			_config.settings.Effectors.Highpass.Resonance = ((UI.Knob)sender).Value;
		}

		// Lowpass
		private void KnobLowpassCutoff_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.CutOff = ((UI.Knob)sender).Value;
			lblValLowpassCutoff.Text = _engine.effector.Lowpass.CutOff.ToString("0");
			_config.settings.Effectors.Lowpass.Cutoff = ((UI.Knob)sender).Value;
		}
		private void KnobLowpassResonance_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Lowpass.Resonance = ((UI.Knob)sender).Value / 10F;
			lblValLowpassResonance.Text = _engine.effector.Lowpass.Resonance.ToString("0.0");
			_config.settings.Effectors.Lowpass.Resonance = ((UI.Knob)sender).Value;
		}


		// Compressor
		private void KnobCompThreshold_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Threshold = ((UI.Knob)sender).Value;
			lblValCompThreshold.Text = _engine.effector.Compressor.Threshold.ToString("0");
			_config.settings.Effectors.Compressor.Threshold = ((UI.Knob)sender).Value;
		}
		private void KnobCompRatio_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Ratio = ((UI.Knob)sender).Value;  // ÷1
			lblValCompRatio.Text = _engine.effector.Compressor.Ratio.ToString("0");
			_config.settings.Effectors.Compressor.Ratio = ((UI.Knob)sender).Value;
		}
		private void KnobCompAttack_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Attack = ((UI.Knob)sender).Value / 10F;
			lblValCompAttack.Text = _engine.effector.Compressor.Attack.ToString("0.0");
			_config.settings.Effectors.Compressor.Attack = ((UI.Knob)sender).Value;
		}
		private void KnobCompRelease_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Release = ((UI.Knob)sender).Value;
			lblValCompRelease.Text = _engine.effector.Compressor.Release.ToString("0");
			_config.settings.Effectors.Compressor.Release = ((UI.Knob)sender).Value;
		}
		private void KnobCompGain_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Compressor.Gain = ((UI.Knob)sender).Value;  // ÷1
			lblValCompGain.Text = _engine.effector.Compressor.Gain.ToString("0");
			_config.settings.Effectors.Compressor.Gain = ((UI.Knob)sender).Value;
		}

		// PitchShift
		private void KnobPitchPitch_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.PitchShift.Pitch = ((UI.Knob)sender).Value / 100F;
			lblValPitchPitch.Text = _engine.effector.PitchShift.Pitch.ToString("0.00");
			_config.settings.Effectors.PitchShift.Pitch = ((UI.Knob)sender).Value;
		}
		private void KnobPitchFFT_ValueChanged(object sender, EventArgs e)
		{
			float[] fftsize = { 256, 512, 1024, 2048, 4096 };
			_engine.effector.PitchShift.FFTSize = fftsize[((UI.Knob)sender).Value];
			lblValPitchFFT.Text = _engine.effector.PitchShift.FFTSize.ToString("0");
			_config.settings.Effectors.PitchShift.FFT = ((UI.Knob)sender).Value;
		}

		private void KnobFrequency_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Frequency.SetFrequency(((UI.Knob)sender).Value);
			lblValFrequency.Text = _engine.effector.Frequency.Hz.ToString("0");
			_config.settings.Effectors.Frequency.Frequency = ((UI.Knob)sender).Value;
		}

		private void KnobSpeed_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.Speed = ((UI.Knob)sender).Value;
			lblValSpeed.Text = _engine.effector.Speed.ToString();
			_config.settings.Effectors.Speed.Speed = ((UI.Knob)sender).Value;
		}
		// ===========================
		// Reverb ValueChanged handlers
		// ===========================
		private void KnobReverbDecayTime_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.DecayTime = ((UI.Knob)sender).Value;
			lblValReverbDecayTime.Text = _engine.effector.SFXReverb.DecayTime.ToString("0");
			_config.settings.Effectors.Reverb.DecayTime = ((UI.Knob)sender).Value;
		}
		private void KnobReverbEarlyDelay_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.EarlyDelay = ((UI.Knob)sender).Value;
			lblValReverbEarlyDelay.Text = _engine.effector.SFXReverb.EarlyDelay.ToString("0");
			_config.settings.Effectors.Reverb.EarlyDelay = ((UI.Knob)sender).Value;
		}
		private void KnobReverbLateDelay_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.LateDelay = ((UI.Knob)sender).Value;
			lblValReverbLateDelay.Text = _engine.effector.SFXReverb.LateDelay.ToString("0");
			_config.settings.Effectors.Reverb.LateDelay = ((UI.Knob)sender).Value;
		}
		private void KnobReverbHFRef_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.HFReference = ((UI.Knob)sender).Value;
			lblValReverbHFRef.Text = _engine.effector.SFXReverb.HFReference.ToString("0");
			_config.settings.Effectors.Reverb.HFRef = ((UI.Knob)sender).Value;
		}
		private void KnobReverbHFDcRatio_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.HFDecayRatio = ((UI.Knob)sender).Value;
			lblValReverbHFDcRatio.Text = _engine.effector.SFXReverb.HFDecayRatio.ToString("0");
			_config.settings.Effectors.Reverb.HFDecayRatio = ((UI.Knob)sender).Value;
		}
		private void KnobReverbDiffusion_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.Diffusion = ((UI.Knob)sender).Value;
			lblValReverbDiffusion.Text = _engine.effector.SFXReverb.Diffusion.ToString("0");
			_config.settings.Effectors.Reverb.Diffusion = ((UI.Knob)sender).Value;
		}
		private void KnobReverbDensity_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.Density = ((UI.Knob)sender).Value;
			lblValReverbDensity.Text = _engine.effector.SFXReverb.Density.ToString("0");
			_config.settings.Effectors.Reverb.Density = ((UI.Knob)sender).Value;
		}
		private void KnobReverbLowShelfFrequency_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.LowShelfFrequency = ((UI.Knob)sender).Value;
			lblValReverbLowShelfFreq.Text = _engine.effector.SFXReverb.LowShelfFrequency.ToString("0");
			_config.settings.Effectors.Reverb.LowShelfFrequency = ((UI.Knob)sender).Value;
		}
		private void KnobReverbLowshelfGain_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.LowShelfGain = ((UI.Knob)sender).Value;
			lblValReverbLowShelfGain.Text = _engine.effector.SFXReverb.LowShelfGain.ToString("0");
			_config.settings.Effectors.Reverb.LowShelfGain = ((UI.Knob)sender).Value;
		}
		private void KnobReverbHighCut_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.HighCut = ((UI.Knob)sender).Value;
			lblValReverbHighCut.Text = _engine.effector.SFXReverb.HighCut.ToString("0");
			_config.settings.Effectors.Reverb.HighCut = ((UI.Knob)sender).Value;
		}
		private void KnobReverbEarlyLate_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.EarlyLateMix = ((UI.Knob)sender).Value;
			lblValReverbEarlyLate.Text = _engine.effector.SFXReverb.EarlyLateMix.ToString("0");
			_config.settings.Effectors.Reverb.EarlyLate = ((UI.Knob)sender).Value;
		}
		private void KnobReverbWet_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.WetLevel = ((UI.Knob)sender).Value;
			lblValReverbWet.Text = _engine.effector.SFXReverb.WetLevel.ToString("0");
			_config.settings.Effectors.Reverb.WetLevel = ((UI.Knob)sender).Value;
		}
		private void KnobReverbDry_ValueChanged(object sender, EventArgs e)
		{
			_engine.effector.SFXReverb.DryLevel = ((UI.Knob)sender).Value;
			lblValReverbDry.Text = _engine.effector.SFXReverb.DryLevel.ToString("0");
			_config.settings.Effectors.Reverb.DryLevel = ((UI.Knob)sender).Value;
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

			// ===========================
			// Distortion
			// Level: 0.0～1.0 → ×100
			// ===========================
			CheckDistortion.Checked = _engine.effector.Distortion.Enabled;
			KnobDistortionLevel.ParameterName = "Level";
			KnobDistortionLevel.Unit = "";
			KnobDistortionLevel.Scales = 100f;
			KnobDistortionLevel.Minimum = 0;
			KnobDistortionLevel.Maximum = 100;
			KnobDistortionLevel.LargeChange = 5;
			KnobDistortionLevel.Value = (int)(_engine.effector.Distortion.Level * 100);


			// ===========================
			// Chorus
			// Mix:   0.0～100.0 → ×1
			// Rate:  0.0～20.0  → ×10
			// Depth: 0.0～100.0 → ×1
			// ===========================
			CheckChorus.Checked = _engine.effector.Chorus.Enabled;

			KnobChorusMix.ParameterName = "Mix";
			KnobChorusMix.Unit = "%";
			KnobChorusMix.Scales = 1f;
			KnobChorusMix.Minimum = 0;
			KnobChorusMix.Maximum = 100;
			KnobChorusMix.LargeChange = 5;
			KnobChorusMix.Value = (int)_engine.effector.Chorus.Mix;

			KnobChorusRate.ParameterName = "Rate";
			KnobChorusRate.Unit = "";
			KnobChorusRate.Scales = 10f;
			KnobChorusRate.Minimum = 0;
			KnobChorusRate.Maximum = 200;
			KnobChorusRate.LargeChange = 10;
			KnobChorusRate.Value = (int)(_engine.effector.Chorus.Rate * 10);

			KnobChorusDepth.ParameterName = "Depth";
			KnobChorusDepth.Unit = "%";
			KnobChorusDepth.Scales = 1f;
			KnobChorusDepth.Minimum = 0;
			KnobChorusDepth.Maximum = 100;
			KnobChorusDepth.LargeChange = 5;
			KnobChorusDepth.Value = (int)_engine.effector.Chorus.Depth;

			// ===========================
			// Echo
			// Delay:    1.0～5000.0ms → ×1
			// Feedback: 0.0～100.0%   → ×1
			// Dry/Wet: -80.0～10.0dB  → ×1
			// ===========================
			CheckEcho.Checked = _engine.effector.Echo.Enabled;

			KnobEchoDelay.ParameterName = "Delay";
			KnobEchoDelay.Unit = "ms";
			KnobEchoDelay.Scales = 1f;
			KnobEchoDelay.Minimum = 1;
			KnobEchoDelay.Maximum = 5000;
			KnobEchoDelay.LargeChange = 100;
			KnobEchoDelay.Value = (int)_engine.effector.Echo.Delay;

			KnobEchoFeedback.ParameterName = "Feedback";
			KnobEchoFeedback.Unit = "%";
			KnobEchoFeedback.Scales = 1f;
			KnobEchoFeedback.Minimum = 0;
			KnobEchoFeedback.Maximum = 100;
			KnobEchoFeedback.LargeChange = 5;
			KnobEchoFeedback.Value = (int)_engine.effector.Echo.Feedback;

			KnobEchoDry.ParameterName = "Dry Level";
			KnobEchoDry.Unit = "dB";
			KnobEchoDry.Scales = 1f;
			KnobEchoDry.Minimum = -80;
			KnobEchoDry.Maximum = 10;
			KnobEchoDry.LargeChange = 5;
			KnobEchoDry.Value = (int)_engine.effector.Echo.DryLevel;

			KnobEchoWet.ParameterName = "Wet Level";
			KnobEchoWet.Unit = "dB";
			KnobEchoWet.Scales = 1f;
			KnobEchoWet.Minimum = -80;
			KnobEchoWet.Maximum = 10;
			KnobEchoWet.LargeChange = 5;
			KnobEchoWet.Value = (int)_engine.effector.Echo.WetLevel;

			// ===========================
			// Flanger
			// Mix:   0.0～100.0 → ×1
			// Rate:  0.0～20.0  → ×10
			// Depth: 0.01～1.0  → ×100
			// ===========================
			CheckFlanger.Checked = _engine.effector.Flanger.Enabled;

			KnobFlangerMix.ParameterName = "Mix";
			KnobFlangerMix.Unit = "%";
			KnobFlangerMix.Scales = 1f;
			KnobFlangerMix.Minimum = 0;
			KnobFlangerMix.Maximum = 100;
			KnobFlangerMix.LargeChange = 5;
			KnobFlangerMix.Value = (int)_engine.effector.Flanger.Mix;

			KnobFlangerRate.ParameterName = "Rate";
			KnobFlangerRate.Unit = "";
			KnobFlangerRate.Scales = 10f;
			KnobFlangerRate.Minimum = 0;
			KnobFlangerRate.Maximum = 200;
			KnobFlangerRate.LargeChange = 10;
			KnobFlangerRate.Value = (int)(_engine.effector.Flanger.Rate * 10);

			KnobFlangerDepth.ParameterName = "Depth";
			KnobFlangerDepth.Unit = "";
			KnobFlangerDepth.Scales = 100f;
			KnobFlangerDepth.Minimum = 1;
			KnobFlangerDepth.Maximum = 100;
			KnobFlangerDepth.LargeChange = 5;
			KnobFlangerDepth.Value = (int)(_engine.effector.Flanger.Depth * 100);


			// ===========================
			// Highpass
			// Cutoff:    1.0～22000.0Hz → ×1
			// Resonance: 0.0～10.0      → ×10
			// ===========================
			CheckHighpass.Checked = _engine.effector.Highpass.Enabled;

			KnobHighpassCutoff.ParameterName = "Cutoff";
			KnobHighpassCutoff.Unit = "Hz";
			KnobHighpassCutoff.Scales = 1f;
			KnobHighpassCutoff.Minimum = 1;
			KnobHighpassCutoff.Maximum = 22000;
			KnobHighpassCutoff.LargeChange = 500;
			KnobHighpassCutoff.Value = (int)_engine.effector.Highpass.CutOff;

			KnobHighpassResonance.ParameterName = "Resonance";
			KnobHighpassResonance.Unit = "";
			KnobHighpassResonance.Scales = 10f;
			KnobHighpassResonance.Minimum = 0;
			KnobHighpassResonance.Maximum = 100;
			KnobHighpassResonance.LargeChange = 5;
			KnobHighpassResonance.Value = (int)(_engine.effector.Highpass.Resonance * 10);


			// ===========================
			// Lowpass
			// Cutoff:    1.0～22000.0Hz → ×1
			// Resonance: 0.0～10.0      → ×10
			// ===========================
			CheckLowpass.Checked = _engine.effector.Lowpass.Enabled;

			KnobLowpassCutoff.ParameterName = "Cutoff";
			KnobLowpassCutoff.Unit = "Hz";
			KnobLowpassCutoff.Scales = 1f;
			KnobLowpassCutoff.Minimum = 1;
			KnobLowpassCutoff.Maximum = 22000;
			KnobLowpassCutoff.LargeChange = 500;
			KnobLowpassCutoff.Value = (int)_engine.effector.Lowpass.CutOff;

			KnobLowpassResonance.ParameterName = "Resonance";
			KnobLowpassResonance.Unit = "";
			KnobLowpassResonance.Scales = 10f;
			KnobLowpassResonance.Minimum = 0;
			KnobLowpassResonance.Maximum = 100;
			KnobLowpassResonance.LargeChange = 5;
			KnobLowpassResonance.Value = (int)(_engine.effector.Lowpass.Resonance * 10);

			// ===========================
			// Compressor
			// Threshold: -60.0～0.0dB   → ×1
			// Ratio:      1.0～50.0      → ×10
			// Attack:     0.1～500.0ms  → ×10
			// Release:   10.0～5000.0ms → ×1
			// Gain:     -30.0～30.0dB   → ×10
			// ===========================
			CheckCompressor.Checked = _engine.effector.Compressor.Enabled;

			KnobCompThreshold.ParameterName = "Threshold";
			KnobCompThreshold.Unit = "dB";
			KnobCompThreshold.Scales = 1f;
			KnobCompThreshold.Minimum = -60;
			KnobCompThreshold.Maximum = 0;
			KnobCompThreshold.LargeChange = 5;
			KnobCompThreshold.Value = (int)_engine.effector.Compressor.Threshold;

			KnobCompRatio.ParameterName = "Ratio";
			KnobCompRatio.Unit = "";
			KnobCompRatio.Scales = 1f;
			KnobCompRatio.Minimum = 1;
			KnobCompRatio.Maximum = 50;
			KnobCompRatio.LargeChange = 5;
			KnobCompRatio.Value = (int)(_engine.effector.Compressor.Ratio * 10);

			KnobCompAttack.ParameterName = "Attack";
			KnobCompAttack.Unit = "ms";
			KnobCompAttack.Scales = 10f;
			KnobCompAttack.Minimum = 1;
			KnobCompAttack.Maximum = 5000;
			KnobCompAttack.LargeChange = 100;
			KnobCompAttack.Value = (int)(_engine.effector.Compressor.Attack * 10);

			KnobCompRelease.ParameterName = "Release";
			KnobCompRelease.Unit = "ms";
			KnobCompRelease.Scales = 1f;
			KnobCompRelease.Minimum = 10;
			KnobCompRelease.Maximum = 5000;
			KnobCompRelease.LargeChange = 100;
			KnobCompRelease.Value = (int)_engine.effector.Compressor.Release;

			KnobCompGain.ParameterName = "Gain";
			KnobCompGain.Unit = "dB";
			KnobCompGain.Scales = 1f;
			KnobCompGain.Minimum = -30;
			KnobCompGain.Maximum = 30;
			KnobCompGain.LargeChange = 2;
			KnobCompGain.Value = (int)(_engine.effector.Compressor.Gain * 10);

			// ===========================
			// PitchShift
			// Pitch:   0.5～2.0  → ×100
			// FFTSize: 256～4096 → ×1（固定値のみ）
			// ===========================
			CheckPitch.Checked = _engine.effector.PitchShift.Enabled;
			_engine.effector.PitchShift.PropertyChanged += new PropertyChangedEventHandler(PitchChenged);

			KnobPitchPitch.ParameterName = "Pitch";
			KnobPitchPitch.Unit = "";
			KnobPitchPitch.Scales = 100f;
			KnobPitchPitch.Minimum = 50;
			KnobPitchPitch.Maximum = 200;
			KnobPitchPitch.LargeChange = 10;
			KnobPitchPitch.Value = (int)(_engine.effector.PitchShift.Pitch * 100);

			KnobPitchFFT.ParameterName = "FFT Size";
			KnobPitchFFT.Unit = "";
			KnobPitchFFT.Scales = 1f;
			KnobPitchFFT.Minimum = 0;
			KnobPitchFFT.Maximum = 4;
			KnobPitchFFT.LargeChange = 1;
			KnobPitchFFT.Value = Array.IndexOf(new float[] { 256, 512, 1024, 2048, 4096 },
											 _engine.effector.PitchShift.FFTSize);
			// PitchShift初期化の末尾に追加
			if (_engine.effector.PitchShift.Enabled)
				_engine.effector.PitchShift.Pitch = KnobPitchPitch.Value / 100f;

			CheckFrequency.Checked = _engine.effector.Frequency.Enabled;
			KnobFrequency.ParameterName = "Frequency";
			KnobFrequency.Unit = "(−100=低速 / 0=標準 / 100=高速)";
			KnobFrequency.Scales = 1f;
			KnobFrequency.Minimum = -100;
			KnobFrequency.Maximum = 100;
			KnobFrequency.LargeChange = 5;
			KnobFrequency.Value = _config.settings.Effectors.Frequency.Frequency;
			_engine.effector.Frequency.PropertyChanged += new PropertyChangedEventHandler(FrequencyChenged);

			// Frequency初期化の末尾に追加
			if (_engine.effector.Frequency.Enabled)
				_engine.effector.Frequency.SetFrequency(KnobFrequency.Value);
			CheckSpeed.Checked = _engine.effector.SpeedEnabled;
			// Speed ON/OFFに応じてKnobの有効・無効を初期設定
			if (_engine.effector.SpeedEnabled)
			{
				GroupFrequency.Enabled = false;
				GroupPitchShift.Enabled = false;
				KnobSpeed.Enabled = true;
			}
			else
			{
				KnobSpeed.Enabled = false;
			}

			// ===========================
			// Reverb
			// ===========================
			CheckReverb.Checked = _config.settings.Effectors.Reverb.Enable;

			KnobReverbDecayTime.ParameterName = "Decay Time"; KnobReverbDecayTime.Unit = "ms"; KnobReverbDecayTime.Scales = 1f; KnobReverbDecayTime.LargeChange = 500;
			KnobReverbEarlyDelay.ParameterName = "Early Delay"; KnobReverbEarlyDelay.Unit = "ms"; KnobReverbEarlyDelay.Scales = 1f; KnobReverbEarlyDelay.LargeChange = 10;
			KnobReverbLateDelay.ParameterName = "Late Delay"; KnobReverbLateDelay.Unit = "ms"; KnobReverbLateDelay.Scales = 1f; KnobReverbLateDelay.LargeChange = 5;
			KnobReverbHFRef.ParameterName = "HF Reference"; KnobReverbHFRef.Unit = "Hz"; KnobReverbHFRef.Scales = 1f; KnobReverbHFRef.LargeChange = 500;
			KnobReverbHFDcRatio.ParameterName = "HF Decay Ratio"; KnobReverbHFDcRatio.Unit = "%"; KnobReverbHFDcRatio.Scales = 1f; KnobReverbHFDcRatio.LargeChange = 5;
			KnobReverbDiffusion.ParameterName = "Diffusion"; KnobReverbDiffusion.Unit = "%"; KnobReverbDiffusion.Scales = 1f; KnobReverbDiffusion.LargeChange = 5;
			KnobReverbDensity.ParameterName = "Density"; KnobReverbDensity.Unit = "%"; KnobReverbDensity.Scales = 1f; KnobReverbDensity.LargeChange = 5;
			KnobReverbLowShelfFrequency.ParameterName = "Low Shelf Freq"; KnobReverbLowShelfFrequency.Unit = "Hz"; KnobReverbLowShelfFrequency.Scales = 1f; KnobReverbLowShelfFrequency.LargeChange = 50;
			KnobReverbLowshelfGain.ParameterName = "Low Shelf Gain"; KnobReverbLowshelfGain.Unit = "dB"; KnobReverbLowshelfGain.Scales = 1f; KnobReverbLowshelfGain.LargeChange = 2;
			KnobReverbHighCut.ParameterName = "High Cut"; KnobReverbHighCut.Unit = "Hz"; KnobReverbHighCut.Scales = 1f; KnobReverbHighCut.LargeChange = 500;
			KnobReverbEarlyLate.ParameterName = "Early/Late Mix"; KnobReverbEarlyLate.Unit = "%"; KnobReverbEarlyLate.Scales = 1f; KnobReverbEarlyLate.LargeChange = 5;
			KnobReverbWet.ParameterName = "Wet Level"; KnobReverbWet.Unit = "dB"; KnobReverbWet.Scales = 1f; KnobReverbWet.LargeChange = 5;
			KnobReverbDry.ParameterName = "Dry Level"; KnobReverbDry.Unit = "dB"; KnobReverbDry.Scales = 1f; KnobReverbDry.LargeChange = 5;

			// 初期値を設定から反映
			KnobReverbDecayTime.Value = _config.settings.Effectors.Reverb.DecayTime > 0 ? _config.settings.Effectors.Reverb.DecayTime : 1500;
			KnobReverbEarlyDelay.Value = _config.settings.Effectors.Reverb.EarlyDelay;
			KnobReverbLateDelay.Value = _config.settings.Effectors.Reverb.LateDelay;
			KnobReverbHFRef.Value = _config.settings.Effectors.Reverb.HFRef > 0 ? _config.settings.Effectors.Reverb.HFRef : 5000;
			KnobReverbHFDcRatio.Value = _config.settings.Effectors.Reverb.HFDecayRatio > 0 ? _config.settings.Effectors.Reverb.HFDecayRatio : 50;
			KnobReverbDiffusion.Value = _config.settings.Effectors.Reverb.Diffusion > 0 ? _config.settings.Effectors.Reverb.Diffusion : 50;
			KnobReverbDensity.Value = _config.settings.Effectors.Reverb.Density > 0 ? _config.settings.Effectors.Reverb.Density : 50;
			KnobReverbLowShelfFrequency.Value = _config.settings.Effectors.Reverb.LowShelfFrequency > 0 ? _config.settings.Effectors.Reverb.LowShelfFrequency : 250;
			KnobReverbLowshelfGain.Value = _config.settings.Effectors.Reverb.LowShelfGain;
			KnobReverbHighCut.Value = _config.settings.Effectors.Reverb.HighCut > 0 ? _config.settings.Effectors.Reverb.HighCut : 20000;
			KnobReverbEarlyLate.Value = _config.settings.Effectors.Reverb.EarlyLate > 0 ? _config.settings.Effectors.Reverb.EarlyLate : 50;
			KnobReverbWet.Value = _config.settings.Effectors.Reverb.WetLevel != 0 ? _config.settings.Effectors.Reverb.WetLevel : -6;
			KnobReverbDry.Value = _config.settings.Effectors.Reverb.DryLevel;

			InitGroupBoxState(GroupDistortion, CheckDistortion);
			InitGroupBoxState(GroupChorus, CheckChorus);
			InitGroupBoxState(GroupEcho, CheckEcho);
			InitGroupBoxState(GroupFlanger, CheckFlanger);
			InitGroupBoxState(GroupHighpass, CheckHighpass);
			InitGroupBoxState(GroupLowpass, CheckLowpass);
			InitGroupBoxState(GroupCompressor, CheckCompressor);
			InitGroupBoxState(GroupPitchShift, CheckPitch);
			InitGroupBoxState(GroupFrequency, CheckFrequency);
			InitGroupBoxState(GroupReverb, CheckReverb);

			// Def:false
			//			CheckCompLinked.Checked = (bool)(_engine.effector.Compressor.Linked);
			// Def:false
			//			CheckCompSidechain.Checked = (bool)(_engine.effector.Compressor.SideChain);

			// SFX Reverb
			this.Refresh();

		}
		private void InitGroupBoxState(GroupBox groupBox, CheckBox checkBox)
		{
			foreach (Control c in groupBox.Controls)
				if (c != checkBox) c.Enabled = checkBox.Checked;
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

        private void lnkAboutGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = lnkAboutGitHub.Text,
                UseShellExecute = true
            });
        }

        private void TreeMenu_AfterSelect(object sender, TreeViewEventArgs e)
		{
			SelectTab(e.Node.Name);
		}

		public void SelectTab(string tabName)
		{
			switch(tabName)
			{
				case "OUTPUT":
		            tabControlEffects.SelectedIndex = 0;
					break;
				case "GEQ":
					tabControlEffects.SelectedIndex = 1;
					break;
				case "PITCH":
					tabControlEffects.SelectedIndex = 2;
					break;
				case "DISTORTION":
					tabControlEffects.SelectedIndex = 3;
					break;
				case "CHORUS":
					tabControlEffects.SelectedIndex = 4;
					break;
				case "ECHO":
					tabControlEffects.SelectedIndex = 5;
					break;
				case "FLANGER":
					tabControlEffects.SelectedIndex = 6;
					break;
				case "HIGHPASS":
					tabControlEffects.SelectedIndex = 7;
					break;
				case "LOWPASS":
					tabControlEffects.SelectedIndex = 8;
					break;
				case "COMPRESSOR":
					tabControlEffects.SelectedIndex = 9;
					break;
				case "REVERB":
					tabControlEffects.SelectedIndex = 10;
					break;
				case "SKIN":
					tabControlEffects.SelectedIndex = 11;
					break;
                case "ABOUT":
                    tabControlEffects.SelectedTab = tabAbout;
                    break;
                case "EXTENSIONS":
				case "OTHER":
				default:
					break;
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
				_config.settings.OutputType != cmbOutput.SelectedIndex;

			if (cmbDevice.Enabled)
				_config.settings.Device = cmbDevice.SelectedValue.ToString();
			_config.settings.OutputType = cmbOutput.SelectedIndex;
			_config.Save();

			string message = requiresRestart
				? "設定を保存しました。\n出力形式は次回起動時に反映されます。"
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

		private void OptionSkin()
		{
			var skinPath = _config.settings.Skin;
			// 未設定またはファイルが存在しない場合はデフォルトスキンを使用
			if (string.IsNullOrEmpty(skinPath) ||
				(!File.Exists(skinPath) &&
				 !File.Exists(Path.Combine(Application.StartupPath, "Skins", skinPath))))
			{
				// 新形式を優先、なければ旧形式
				var defaultXsk = Path.Combine(Application.StartupPath, "Skins", "Default", "Default.xsk");
				var defaultXsf = Path.Combine(Application.StartupPath, "Skins", "Default", "Default.xsf");

				if (File.Exists(defaultXsk))
					skinPath = defaultXsk;
				else if (File.Exists(defaultXsf))
					skinPath = defaultXsf;
			}
			txtSkinPath.Text = skinPath;            
			// プレビュー画像を表示
			LoadSkinPreview(_config.settings.Skin);
		}

		private void LoadSkinPreview(string skinPath)
		{
			// メタ情報をリセット
			lblSkinName.Text = "";
			lblSkinAuthor.Text = "";
			lblSkinDesc.Text = "";

			try
			{
				using (var pkg = SkinPackage.Open(skinPath))
				{
					// プレビュー画像
					if (pkg.MainImagePath != null && File.Exists(pkg.MainImagePath))
					{
						// ファイルロックを避けるためメモリストリーム経由でロード
						using (var stream = new FileStream(pkg.MainImagePath, FileMode.Open, FileAccess.Read))
						{
							PictSkinPreview.Image = new Bitmap(stream);
							PictSkinPreview.SizeMode = PictureBoxSizeMode.Zoom;
						}
					}
					else
					{
						PictSkinPreview.Image = null;
					}

					// メタ情報（新形式のみ）
					if (pkg.Format == SkinPackage.SkinFormat.NewXsk &&
						pkg.DefinitionPath != null &&
						File.Exists(pkg.DefinitionPath))
					{
						var json = File.ReadAllText(pkg.DefinitionPath,
							System.Text.Encoding.UTF8);
						var skin = System.Text.Json.JsonSerializer
							.Deserialize<Skin.NewSkinSystem.SkinJson>(json);

						lblSkinName.Text = skin?.Meta?.Name ?? "";
						lblSkinAuthor.Text = skin?.Meta?.Author ?? "";
						lblSkinDesc.Text = skin?.Meta?.Description ?? "";
					}
					else
					{
						// 旧形式はファイル名を表示
						lblSkinName.Text = Path.GetFileNameWithoutExtension(skinPath);
					}
				}
			}
			catch
			{
				PictSkinPreview.Image = null;
			}
		}

		private void BtnSkinBrowse_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = "スキンファイル|*.xsk;*.xsf|" +
							 "新形式スキン (*.xsk)|*.xsk|" +
							 "旧形式スキン (*.xsf)|*.xsf|" +
							 "全てのファイル|*.*";
				dlg.InitialDirectory = Path.Combine(
					Application.StartupPath, "Skins");

				if (dlg.ShowDialog() != DialogResult.OK) return;

				txtSkinPath.Text = dlg.FileName;
				LoadSkinPreview(dlg.FileName);
			}
		}

		private void BtnSkinApply_Click(object sender, EventArgs e)
		{
			var skinPath = txtSkinPath.Text;
			if (string.IsNullOrEmpty(skinPath)) return;

			try
			{
				_config.settings.Skin = skinPath;
				_config.Save();
				_mainForm.SkinLoad(skinPath); // MainForm.SkinLoadをpublicに変更が必要
				MessageBox.Show(
					"スキンを適用しました。",
					"完了",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"スキンの適用に失敗しました。\n{ex.Message}",
					"エラー",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

        private void LoadChorusPresets() => LoadEffectPresets<ChorusPreset>(cmbChorusPreset, "Chorus");

        private void BtnChorusPresetSave_Click(object sender, EventArgs e) => SaveEffectPreset<ChorusPreset>(cmbChorusPreset, "Chorus", name => new ChorusPreset
            {
                Name = name,
                Mix = KnobChorusMix.Value,
                Rate = KnobChorusRate.Value / 10f, // Scale=10f
                Depth = KnobChorusDepth.Value / 100f, // Scale=100f
            });

        private void BtnChorusPresetDelete_Click(object sender, EventArgs e) => DeleteEffectPreset<ChorusPreset>(cmbChorusPreset, "Chorus");

        private void cmbChorusPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbChorusPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<ChorusPreset>("Chorus", name);
            if (preset == null) return;
            KnobChorusMix.Value = (int)preset.Mix;
            KnobChorusRate.Value = (int)(preset.Rate * 10f);
            KnobChorusDepth.Value = (int)(preset.Depth * 100f);
            _config.settings.EffectPresets["Chorus"] = name;
        }

        private void LoadDistortionPresets() => LoadEffectPresets<DistortionPreset>(cmbDistortionPreset, "Distortion");

        private void BtnDistortionPresetSave_Click(object sender, EventArgs e) => SaveEffectPreset<DistortionPreset>(cmbDistortionPreset, "Distortion", name => new DistortionPreset
            {
                Name = name,
                Level = KnobDistortionLevel.Value / 100f, // Scale=100f
            });

        private void BtnDistortionPresetDelete_Click(object sender, EventArgs e) => DeleteEffectPreset<DistortionPreset>(cmbDistortionPreset, "Distortion");

        private void cmbDistortionPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbDistortionPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<DistortionPreset>("Distortion", name);
            if (preset == null) return;
            KnobDistortionLevel.Value = (int)(preset.Level * 100f);
            _config.settings.EffectPresets["Distortion"] = name;
        }
        private void LoadEchoPresets()
            => LoadEffectPresets<EchoPreset>(cmbEchoPreset, "Echo");

        private void BtnEchoPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<EchoPreset>(cmbEchoPreset, "Echo", name => new EchoPreset
            {
                Name = name,
                Delay = KnobEchoDelay.Value,
                Feedback = KnobEchoFeedback.Value,
                Dry = KnobEchoDry.Value,
                Wet = KnobEchoWet.Value,
            });

        private void BtnEchoPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<EchoPreset>(cmbEchoPreset, "Echo");

        private void cmbEchoPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbEchoPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<EchoPreset>("Echo", name);
            if (preset == null) return;
            KnobEchoDelay.Value = (int)preset.Delay;
            KnobEchoFeedback.Value = (int)preset.Feedback;
            KnobEchoDry.Value = (int)preset.Dry;
            KnobEchoWet.Value = (int)preset.Wet;
            _config.settings.EffectPresets["Echo"] = name;
        }

        private void LoadFlangerPresets()
            => LoadEffectPresets<FlangerPreset>(cmbFlangerPreset, "Flanger");

        private void BtnFlangerPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<FlangerPreset>(cmbFlangerPreset, "Flanger", name => new FlangerPreset
            {
                Name = name,
                Mix = KnobFlangerMix.Value,
                Rate = KnobFlangerRate.Value / 10f,
                Depth = KnobFlangerDepth.Value / 100f,
            });

        private void BtnFlangerPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<FlangerPreset>(cmbFlangerPreset, "Flanger");

        private void cmbFlangerPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbFlangerPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<FlangerPreset>("Flanger", name);
            if (preset == null) return;
            KnobFlangerMix.Value = (int)preset.Mix;
            KnobFlangerRate.Value = (int)(preset.Rate * 10f);
            KnobFlangerDepth.Value = (int)(preset.Depth * 100f);
            _config.settings.EffectPresets["Flanger"] = name;
        }

        private void LoadHighpassPresets()
            => LoadEffectPresets<HighpassPreset>(cmbHighpassPreset, "Highpass");

        private void BtnHighpassPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<HighpassPreset>(cmbHighpassPreset, "Highpass", name => new HighpassPreset
            {
                Name = name,
                Cutoff = KnobHighpassCutoff.Value,
                Resonance = KnobHighpassResonance.Value,
            });

        private void BtnHighpassPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<HighpassPreset>(cmbHighpassPreset, "Highpass");

        private void cmbHighpassPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbHighpassPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<HighpassPreset>("Highpass", name);
            if (preset == null) return;
            KnobHighpassCutoff.Value = (int)preset.Cutoff;
            KnobHighpassResonance.Value = (int)preset.Resonance;
            _config.settings.EffectPresets["Highpass"] = name;
        }

        private void LoadLowpassPresets()
            => LoadEffectPresets<LowpassPreset>(cmbLowpassPreset, "Lowpass");

        private void BtnLowpassPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<LowpassPreset>(cmbLowpassPreset, "Lowpass", name => new LowpassPreset
            {
                Name = name,
                Cutoff = KnobLowpassCutoff.Value,
                Resonance = KnobLowpassResonance.Value,
            });

        private void BtnLowpassPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<LowpassPreset>(cmbLowpassPreset, "Lowpass");

        private void cmbLowpassPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbLowpassPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<LowpassPreset>("Lowpass", name);
            if (preset == null) return;
            KnobLowpassCutoff.Value = (int)preset.Cutoff;
            KnobLowpassResonance.Value = (int)preset.Resonance;
            _config.settings.EffectPresets["Lowpass"] = name;
        }

        private void LoadCompressorPresets()
            => LoadEffectPresets<CompressorPreset>(cmbCompressorPreset, "Compressor");

        private void BtnCompressorPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<CompressorPreset>(cmbCompressorPreset, "Compressor", name => new CompressorPreset
            {
                Name = name,
                Threshold = KnobCompThreshold.Value,
                Ratio = KnobCompRatio.Value,
                Attack = KnobCompAttack.Value,
                Release = KnobCompRelease.Value,
                Gain = KnobCompGain.Value,
                Linked = CheckCompLinked.Checked,
            });

        private void BtnCompressorPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<CompressorPreset>(cmbCompressorPreset, "Compressor");

        private void cmbCompressorPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbCompressorPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<CompressorPreset>("Compressor", name);
            if (preset == null) return;
            KnobCompThreshold.Value = (int)preset.Threshold;
            KnobCompRatio.Value = (int)preset.Ratio;
            KnobCompAttack.Value = (int)preset.Attack;
            KnobCompRelease.Value = (int)preset.Release;
            KnobCompGain.Value = (int)preset.Gain;
            CheckCompLinked.Checked = preset.Linked;
            _config.settings.EffectPresets["Compressor"] = name;
        }

        private void LoadReverbPresets()
            => LoadEffectPresets<ReverbPreset>(cmbReverbPreset, "Reverb");

        private void BtnReverbPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<ReverbPreset>(cmbReverbPreset, "Reverb", name => new ReverbPreset
            {
                Name = name,
                DecayTime = KnobReverbDecayTime.Value,
                EarlyDelay = KnobReverbEarlyDelay.Value,
                LateDelay = KnobReverbLateDelay.Value,
                HFReference = KnobReverbHFRef.Value,
                HFDecayRatio = KnobReverbHFDcRatio.Value,
                Diffusion = KnobReverbDiffusion.Value,
                Density = KnobReverbDensity.Value,
                LowShelfFreq = KnobReverbLowShelfFrequency.Value,
                LowShelfGain = KnobReverbLowshelfGain.Value,
                HighCut = KnobReverbHighCut.Value,
                EarlyLateMix = KnobReverbEarlyLate.Value,
                WetLevel = KnobReverbWet.Value,
                DryLevel = KnobReverbDry.Value,
            });

        private void BtnReverbPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<ReverbPreset>(cmbReverbPreset, "Reverb");

        private void cmbReverbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbReverbPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<ReverbPreset>("Reverb", name);
            if (preset == null) return;
            KnobReverbDecayTime.Value = (int)preset.DecayTime;
            KnobReverbEarlyDelay.Value = (int)preset.EarlyDelay;
            KnobReverbLateDelay.Value = (int)preset.LateDelay;
            KnobReverbHFRef.Value = (int)preset.HFReference;
            KnobReverbHFDcRatio.Value = (int)preset.HFDecayRatio;
            KnobReverbDiffusion.Value = (int)preset.Diffusion;
            KnobReverbDensity.Value = (int)preset.Density;
            KnobReverbLowShelfFrequency.Value = (int)preset.LowShelfFreq;
            KnobReverbLowshelfGain.Value = (int)preset.LowShelfGain;
            KnobReverbHighCut.Value = (int)preset.HighCut;
            KnobReverbEarlyLate.Value = (int)preset.EarlyLateMix;
            KnobReverbWet.Value = (int)preset.WetLevel;
            KnobReverbDry.Value = (int)preset.DryLevel;
            _config.settings.EffectPresets["Reverb"] = name;
        }

        private void LoadPitchPresets()
            => LoadEffectPresets<PitchPreset>(cmbPitchPreset, "Pitch");

        private void BtnPitchPresetSave_Click(object sender, EventArgs e)
            => SaveEffectPreset<PitchPreset>(cmbPitchPreset, "Pitch", name => new PitchPreset
            {
                Name = name,
                Pitch = KnobPitchPitch.Value / 100f,
                FFTSize = KnobPitchFFT.Value,
                Frequency = KnobFrequency.Value,
                Speed = KnobSpeed.Value,
            });

        private void BtnPitchPresetDelete_Click(object sender, EventArgs e)
            => DeleteEffectPreset<PitchPreset>(cmbPitchPreset, "Pitch");

        private void cmbPitchPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = cmbPitchPreset.SelectedItem as string;
            if (string.IsNullOrEmpty(name)) return;
            var preset = EffectPreset.Load<PitchPreset>("Pitch", name);
            if (preset == null) return;
            KnobPitchPitch.Value = (int)(preset.Pitch * 100f);
            KnobPitchFFT.Value = (int)preset.FFTSize;
            KnobFrequency.Value = (int)preset.Frequency;
            KnobSpeed.Value = (int)preset.Speed;
            _config.settings.EffectPresets["Pitch"] = name;
        }
    }
}
