using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class ReverbControl : EffectControlBase
	{
		protected override string EffectName => "Reverb";
		protected override string EnableText => "Reverb";

		private UI.Knob _knobDecayTime, _knobEarlyDelay, _knobLateDelay,
						_knobHFRef, _knobHFDcRatio, _knobDiffusion, _knobDensity,
						_knobLowShelfFreq, _knobLowShelfGain, _knobHighCut,
						_knobEarlyLate, _knobWet, _knobDry;
		private TextBox _lblDecayTime, _lblEarlyDelay, _lblLateDelay,
						_lblHFRef, _lblHFDcRatio, _lblDiffusion, _lblDensity,
						_lblLowShelfFreq, _lblLowShelfGain, _lblHighCut,
						_lblEarlyLate, _lblWet, _lblDry;

		public ReverbControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();
			BuildKnobs();
		}

		private void BuildKnobs()
		{
			const int spacing = 70;
			const int row2 = 110;

			// Row 1
			_knobDecayTime = CreateKnob(8 + spacing * 0, 8, "Decay Time", "ms",
				100, 20000, 500, DefaultOrConfig(Config.settings.Effectors.Reverb.DecayTime, 1500),
				(s, e) => { Engine.effector.SFXReverb.DecayTime = ((UI.Knob)s).Value; _lblDecayTime.Text = Engine.effector.SFXReverb.DecayTime.ToString("0"); Config.settings.Effectors.Reverb.DecayTime = ((UI.Knob)s).Value; });
			_lblDecayTime = CreateValueLabel(8 + spacing * 0, 8);

			_knobEarlyDelay = CreateKnob(8 + spacing * 1, 8, "Early Delay", "ms",
				0, 300, 10, Config.settings.Effectors.Reverb.EarlyDelay,
				(s, e) => { Engine.effector.SFXReverb.EarlyDelay = ((UI.Knob)s).Value; _lblEarlyDelay.Text = Engine.effector.SFXReverb.EarlyDelay.ToString("0"); Config.settings.Effectors.Reverb.EarlyDelay = ((UI.Knob)s).Value; });
			_lblEarlyDelay = CreateValueLabel(8 + spacing * 1, 8);

			_knobLateDelay = CreateKnob(8 + spacing * 2, 8, "Late Delay", "ms",
				0, 100, 5, Config.settings.Effectors.Reverb.LateDelay,
				(s, e) => { Engine.effector.SFXReverb.LateDelay = ((UI.Knob)s).Value; _lblLateDelay.Text = Engine.effector.SFXReverb.LateDelay.ToString("0"); Config.settings.Effectors.Reverb.LateDelay = ((UI.Knob)s).Value; });
			_lblLateDelay = CreateValueLabel(8 + spacing * 2, 8);

			_knobHFRef = CreateKnob(8 + spacing * 3, 8, "HF Ref", "Hz",
				20, 20000, 500, DefaultOrConfig(Config.settings.Effectors.Reverb.HFRef, 5000),
				(s, e) => { Engine.effector.SFXReverb.HFReference = ((UI.Knob)s).Value; _lblHFRef.Text = Engine.effector.SFXReverb.HFReference.ToString("0"); Config.settings.Effectors.Reverb.HFRef = ((UI.Knob)s).Value; });
			_lblHFRef = CreateValueLabel(8 + spacing * 3, 8);

			_knobHFDcRatio = CreateKnob(8 + spacing * 4, 8, "HF Decay", "%",
				10, 100, 5, DefaultOrConfig(Config.settings.Effectors.Reverb.HFDecayRatio, 50),
				(s, e) => { Engine.effector.SFXReverb.HFDecayRatio = ((UI.Knob)s).Value; _lblHFDcRatio.Text = Engine.effector.SFXReverb.HFDecayRatio.ToString("0"); Config.settings.Effectors.Reverb.HFDecayRatio = ((UI.Knob)s).Value; });
			_lblHFDcRatio = CreateValueLabel(8 + spacing * 4, 8);

			_knobDiffusion = CreateKnob(8 + spacing * 5, 8, "Diffusion", "%",
				0, 100, 5, DefaultOrConfig(Config.settings.Effectors.Reverb.Diffusion, 50),
				(s, e) => { Engine.effector.SFXReverb.Diffusion = ((UI.Knob)s).Value; _lblDiffusion.Text = Engine.effector.SFXReverb.Diffusion.ToString("0"); Config.settings.Effectors.Reverb.Diffusion = ((UI.Knob)s).Value; });
			_lblDiffusion = CreateValueLabel(8 + spacing * 5, 8);

			_knobDensity = CreateKnob(8 + spacing * 6, 8, "Density", "%",
				0, 100, 5, DefaultOrConfig(Config.settings.Effectors.Reverb.Density, 50),
				(s, e) => { Engine.effector.SFXReverb.Density = ((UI.Knob)s).Value; _lblDensity.Text = Engine.effector.SFXReverb.Density.ToString("0"); Config.settings.Effectors.Reverb.Density = ((UI.Knob)s).Value; });
			_lblDensity = CreateValueLabel(8 + spacing * 6, 8);

			// Row 2
			_knobLowShelfFreq = CreateKnob(8 + spacing * 0, row2, "Low Shelf", "Hz",
				20, 1000, 50, DefaultOrConfig(Config.settings.Effectors.Reverb.LowShelfFrequency, 250),
				(s, e) => { Engine.effector.SFXReverb.LowShelfFrequency = ((UI.Knob)s).Value; _lblLowShelfFreq.Text = Engine.effector.SFXReverb.LowShelfFrequency.ToString("0"); Config.settings.Effectors.Reverb.LowShelfFrequency = ((UI.Knob)s).Value; });
			_lblLowShelfFreq = CreateValueLabel(8 + spacing * 0, row2);

			_knobLowShelfGain = CreateKnob(8 + spacing * 1, row2, "Shelf Gain", "dB",
				-36, 12, 2, Config.settings.Effectors.Reverb.LowShelfGain,
				(s, e) => { Engine.effector.SFXReverb.LowShelfGain = ((UI.Knob)s).Value; _lblLowShelfGain.Text = Engine.effector.SFXReverb.LowShelfGain.ToString("0"); Config.settings.Effectors.Reverb.LowShelfGain = ((UI.Knob)s).Value; });
			_lblLowShelfGain = CreateValueLabel(8 + spacing * 1, row2);

			_knobHighCut = CreateKnob(8 + spacing * 2, row2, "High Cut", "Hz",
				20, 20000, 500, DefaultOrConfig(Config.settings.Effectors.Reverb.HighCut, 20000),
				(s, e) => { Engine.effector.SFXReverb.HighCut = ((UI.Knob)s).Value; _lblHighCut.Text = Engine.effector.SFXReverb.HighCut.ToString("0"); Config.settings.Effectors.Reverb.HighCut = ((UI.Knob)s).Value; });
			_lblHighCut = CreateValueLabel(8 + spacing * 2, row2);

			_knobEarlyLate = CreateKnob(8 + spacing * 3, row2, "Early/Late", "%",
				0, 100, 5, DefaultOrConfig(Config.settings.Effectors.Reverb.EarlyLate, 50),
				(s, e) => { Engine.effector.SFXReverb.EarlyLateMix = ((UI.Knob)s).Value; _lblEarlyLate.Text = Engine.effector.SFXReverb.EarlyLateMix.ToString("0"); Config.settings.Effectors.Reverb.EarlyLate = ((UI.Knob)s).Value; });
			_lblEarlyLate = CreateValueLabel(8 + spacing * 3, row2);

			_knobWet = CreateKnob(8 + spacing * 4, row2, "Wet", "dB",
				-80, 0, 5, DefaultOrConfig(Config.settings.Effectors.Reverb.WetLevel, -6),
				(s, e) => { Engine.effector.SFXReverb.WetLevel = ((UI.Knob)s).Value; _lblWet.Text = Engine.effector.SFXReverb.WetLevel.ToString("0"); Config.settings.Effectors.Reverb.WetLevel = ((UI.Knob)s).Value; });
			_lblWet = CreateValueLabel(8 + spacing * 4, row2);

			_knobDry = CreateKnob(8 + spacing * 5, row2, "Dry", "dB",
				-80, 0, 5, Config.settings.Effectors.Reverb.DryLevel,
				(s, e) => { Engine.effector.SFXReverb.DryLevel = ((UI.Knob)s).Value; _lblDry.Text = Engine.effector.SFXReverb.DryLevel.ToString("0"); Config.settings.Effectors.Reverb.DryLevel = ((UI.Knob)s).Value; });
			_lblDry = CreateValueLabel(8 + spacing * 5, row2);
		}

		private int DefaultOrConfig(int value, int defaultValue) =>
			value != 0 ? value : defaultValue;

		public override void LoadSettings()
		{
			ChkEnable.Checked = Config.settings.Effectors.Reverb.Enable;
			_knobDecayTime.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.DecayTime, 1500);
			_knobEarlyDelay.Value = Config.settings.Effectors.Reverb.EarlyDelay;
			_knobLateDelay.Value = Config.settings.Effectors.Reverb.LateDelay;
			_knobHFRef.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.HFRef, 5000);
			_knobHFDcRatio.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.HFDecayRatio, 50);
			_knobDiffusion.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.Diffusion, 50);
			_knobDensity.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.Density, 50);
			_knobLowShelfFreq.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.LowShelfFrequency, 250);
			_knobLowShelfGain.Value = Config.settings.Effectors.Reverb.LowShelfGain;
			_knobHighCut.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.HighCut, 20000);
			_knobEarlyLate.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.EarlyLate, 50);
			_knobWet.Value = DefaultOrConfig(Config.settings.Effectors.Reverb.WetLevel, -6);
			_knobDry.Value = Config.settings.Effectors.Reverb.DryLevel;

			_lblDecayTime.Text = _knobDecayTime.Value.ToString();
			_lblEarlyDelay.Text = _knobEarlyDelay.Value.ToString();
			_lblLateDelay.Text = _knobLateDelay.Value.ToString();
			_lblHFRef.Text = _knobHFRef.Value.ToString();
			_lblHFDcRatio.Text = _knobHFDcRatio.Value.ToString();
			_lblDiffusion.Text = _knobDiffusion.Value.ToString();
			_lblDensity.Text = _knobDensity.Value.ToString();
			_lblLowShelfFreq.Text = _knobLowShelfFreq.Value.ToString();
			_lblLowShelfGain.Text = _knobLowShelfGain.Value.ToString();
			_lblHighCut.Text = _knobHighCut.Value.ToString();
			_lblEarlyLate.Text = _knobEarlyLate.Value.ToString();
			_lblWet.Text = _knobWet.Value.ToString();
			_lblDry.Text = _knobDry.Value.ToString();

			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.SFXReverb.Switch(enabled);
			Config.settings.Effectors.Reverb.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.SFXReverb.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<ReverbPreset>("Reverb", name);
			if (preset == null) return;
			_knobDecayTime.Value = (int)preset.DecayTime;
			_knobEarlyDelay.Value = (int)preset.EarlyDelay;
			_knobLateDelay.Value = (int)preset.LateDelay;
			_knobHFRef.Value = (int)preset.HFReference;
			_knobHFDcRatio.Value = (int)preset.HFDecayRatio;
			_knobDiffusion.Value = (int)preset.Diffusion;
			_knobDensity.Value = (int)preset.Density;
			_knobLowShelfFreq.Value = (int)preset.LowShelfFreq;
			_knobLowShelfGain.Value = (int)preset.LowShelfGain;
			_knobHighCut.Value = (int)preset.HighCut;
			_knobEarlyLate.Value = (int)preset.EarlyLateMix;
			_knobWet.Value = (int)preset.WetLevel;
			_knobDry.Value = (int)preset.DryLevel;
		}

		protected override EffectPreset CreatePreset(string name) =>
			new ReverbPreset
			{
				Name = name,
				DecayTime = _knobDecayTime.Value,
				EarlyDelay = _knobEarlyDelay.Value,
				LateDelay = _knobLateDelay.Value,
				HFReference = _knobHFRef.Value,
				HFDecayRatio = _knobHFDcRatio.Value,
				Diffusion = _knobDiffusion.Value,
				Density = _knobDensity.Value,
				LowShelfFreq = _knobLowShelfFreq.Value,
				LowShelfGain = _knobLowShelfGain.Value,
				HighCut = _knobHighCut.Value,
				EarlyLateMix = _knobEarlyLate.Value,
				WetLevel = _knobWet.Value,
				DryLevel = _knobDry.Value,
			};

		protected override void ApplyToEngine() { }
	}
}