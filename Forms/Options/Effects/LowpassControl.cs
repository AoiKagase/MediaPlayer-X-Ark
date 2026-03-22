using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class LowpassControl : EffectControlBase
	{
		protected override string EffectName => "Lowpass";
		protected override string EnableText => "Lowpass";

		private UI.Knob _knobCutoff, _knobResonance;
		private TextBox _lblCutoff, _lblResonance;

		public LowpassControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobCutoff = CreateKnob(8, 8, "Cutoff", "Hz", 1, 22000, 500,
				(int)Engine.effector.Lowpass.CutOff,
				(s, e) =>
				{
					Engine.effector.Lowpass.CutOff = ((UI.Knob)s).Value;
					_lblCutoff.Text = Engine.effector.Lowpass.CutOff.ToString("0");
					Config.settings.Effectors.Lowpass.Cutoff = ((UI.Knob)s).Value;
				});
			_lblCutoff = CreateValueLabel(8, 8);

			_knobResonance = CreateKnob(78, 8, "Resonance", "", 0, 100, 5,
				(int)(Engine.effector.Lowpass.Resonance * 10),
				(s, e) =>
				{
					Engine.effector.Lowpass.Resonance = ((UI.Knob)s).Value / 10f;
					_lblResonance.Text = Engine.effector.Lowpass.Resonance.ToString("0.0");
					Config.settings.Effectors.Lowpass.Resonance = ((UI.Knob)s).Value;
				});
			_lblResonance = CreateValueLabel(78, 8);
		}

		public override void LoadSettings()
		{
			ChkEnable.Checked = Engine.effector.Lowpass.Enabled;
			_knobCutoff.Value = (int)Engine.effector.Lowpass.CutOff;
			_knobResonance.Value = (int)(Engine.effector.Lowpass.Resonance * 10);
			_lblCutoff.Text = Engine.effector.Lowpass.CutOff.ToString("0");
			_lblResonance.Text = Engine.effector.Lowpass.Resonance.ToString("0.0");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Lowpass.Switch(enabled);
			Config.settings.Effectors.Lowpass.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Lowpass.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<LowpassPreset>("Lowpass", name);
			if (preset == null) return;
			_knobCutoff.Value = (int)preset.Cutoff;
			_knobResonance.Value = (int)preset.Resonance;
		}

		protected override EffectPreset CreatePreset(string name) =>
			new LowpassPreset
			{
				Name = name,
				Cutoff = _knobCutoff.Value,
				Resonance = _knobResonance.Value,
			};

		protected override void ApplyToEngine() { }
	}
}