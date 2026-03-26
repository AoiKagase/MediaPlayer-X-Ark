using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class HighpassControl : EffectControlBase
	{
		protected override string EffectName => "Highpass";
		protected override string EnableText => "Highpass";

		private UI.Knob _knobCutoff, _knobResonance;
		private TextBox _lblCutoff, _lblResonance;

		public HighpassControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobCutoff = CreateKnob(8, 8, "Cutoff", "Hz", 1, 22000, 500,
				(int)Engine.effector.Highpass.CutOff,
				(s, e) =>
				{
					Engine.effector.Highpass.CutOff = ((UI.Knob)s).Value;
					_lblCutoff.Text = Engine.effector.Highpass.CutOff.ToString("0");
					Config.settings.Effectors.Highpass.Cutoff = ((UI.Knob)s).Value;
				});
			_lblCutoff = CreateValueLabel(8, 8);

			_knobResonance = CreateKnob(78, 8, "Resonance", "", 0, 100, 5,
				(int)(Engine.effector.Highpass.Resonance * 10),
				(s, e) =>
				{
					Engine.effector.Highpass.Resonance = ((UI.Knob)s).Value / 10f;
					_lblResonance.Text = Engine.effector.Highpass.Resonance.ToString("0.0");
					Config.settings.Effectors.Highpass.Resonance = ((UI.Knob)s).Value;
				});
			_lblResonance = CreateValueLabel(78, 8);
		}

		public override void LoadSettings()
		{
			_loading = true;
			ChkEnable.Checked = Engine.effector.Highpass.Enabled;
			_knobCutoff.Value = (int)Engine.effector.Highpass.CutOff;
			_knobResonance.Value = (int)(Engine.effector.Highpass.Resonance * 10);
			_lblCutoff.Text = Engine.effector.Highpass.CutOff.ToString("0");
			_lblResonance.Text = Engine.effector.Highpass.Resonance.ToString("0.0");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
			_loading = false;
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Highpass.Switch(enabled);
			Config.settings.Effectors.Highpass.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Highpass.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<HighpassPreset>("Highpass", name);
			if (preset == null) return;
			_knobCutoff.Value = (int)preset.Cutoff;
			_knobResonance.Value = (int)preset.Resonance;
		}

		protected override EffectPreset CreatePreset(string name) =>
			new HighpassPreset
			{
				Name = name,
				Cutoff = _knobCutoff.Value,
				Resonance = _knobResonance.Value,
			};

		protected override void ApplyToEngine() { }
	}
}