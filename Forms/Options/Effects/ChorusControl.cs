using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class ChorusControl : EffectControlBase
	{
		protected override string EffectName => "Chorus";
		protected override string EnableText => "Chorus";

		private UI.Knob _knobMix, _knobRate, _knobDepth;
		private TextBox _lblMix, _lblRate, _lblDepth;

		public ChorusControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobMix = CreateKnob(8, 8, "Mix", "%", 0, 100, 5,
				(int)Engine.effector.Chorus.Mix,
				(s, e) =>
				{
					Engine.effector.Chorus.Mix = ((UI.Knob)s).Value;
					_lblMix.Text = Engine.effector.Chorus.Mix.ToString("0");
					Config.settings.Effectors.Chorus.Mix = ((UI.Knob)s).Value;
				});
			_lblMix = CreateValueLabel(8, 8);

			_knobRate = CreateKnob(78, 8, "Rate", "", 0, 200, 10,
				(int)(Engine.effector.Chorus.Rate * 10),
				(s, e) =>
				{
					Engine.effector.Chorus.Rate = ((UI.Knob)s).Value / 10f;
					_lblRate.Text = Engine.effector.Chorus.Rate.ToString("0.0");
					Config.settings.Effectors.Chorus.Rate = ((UI.Knob)s).Value;
				});
			_lblRate = CreateValueLabel(78, 8);

			_knobDepth = CreateKnob(148, 8, "Depth", "%", 0, 100, 5,
				(int)Engine.effector.Chorus.Depth,
				(s, e) =>
				{
					Engine.effector.Chorus.Depth = ((UI.Knob)s).Value;
					_lblDepth.Text = Engine.effector.Chorus.Depth.ToString("0");
					Config.settings.Effectors.Chorus.Depth = ((UI.Knob)s).Value;
				});
			_lblDepth = CreateValueLabel(148, 8);
		}

		public override void LoadSettings()
		{
			ChkEnable.Checked = Engine.effector.Chorus.Enabled;
			_knobMix.Value = (int)Engine.effector.Chorus.Mix;
			_knobRate.Value = (int)(Engine.effector.Chorus.Rate * 10);
			_knobDepth.Value = (int)Engine.effector.Chorus.Depth;
			_lblMix.Text = Engine.effector.Chorus.Mix.ToString("0");
			_lblRate.Text = Engine.effector.Chorus.Rate.ToString("0.0");
			_lblDepth.Text = Engine.effector.Chorus.Depth.ToString("0");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Chorus.Switch(enabled);
			Config.settings.Effectors.Chorus.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Chorus.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<ChorusPreset>("Chorus", name);
			if (preset == null) return;
			_knobMix.Value = (int)preset.Mix;
			_knobRate.Value = (int)(preset.Rate * 10f);
			_knobDepth.Value = (int)(preset.Depth * 100f);
		}

		protected override EffectPreset CreatePreset(string name) =>
			new ChorusPreset
			{
				Name = name,
				Mix = _knobMix.Value,
				Rate = _knobRate.Value / 10f,
				Depth = _knobDepth.Value / 100f,
			};

		protected override void ApplyToEngine() { }
	}
}