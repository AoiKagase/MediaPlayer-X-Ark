using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class FlangerControl : EffectControlBase
	{
		protected override string EffectName => "Flanger";
		protected override string EnableText => "Flanger";

		private UI.Knob _knobMix, _knobRate, _knobDepth;
		private TextBox _lblMix, _lblRate, _lblDepth;

		public FlangerControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobMix = CreateKnob(8, 8, "Mix", "%", 0, 100, 5,
				(int)Engine.effector.Flanger.Mix,
				(s, e) =>
				{
					Engine.effector.Flanger.Mix = ((UI.Knob)s).Value;
					_lblMix.Text = Engine.effector.Flanger.Mix.ToString("0");
					Config.settings.Effectors.Flanger.Mix = ((UI.Knob)s).Value;
				});
			_lblMix = CreateValueLabel(8, 8);

			_knobRate = CreateKnob(78, 8, "Rate", "", 0, 200, 10,
				(int)(Engine.effector.Flanger.Rate * 10),
				(s, e) =>
				{
					Engine.effector.Flanger.Rate = ((UI.Knob)s).Value / 10f;
					_lblRate.Text = Engine.effector.Flanger.Rate.ToString("0.0");
					Config.settings.Effectors.Flanger.Rate = ((UI.Knob)s).Value;
				});
			_lblRate = CreateValueLabel(78, 8);

			_knobDepth = CreateKnob(148, 8, "Depth", "", 1, 100, 5,
				(int)(Engine.effector.Flanger.Depth * 100),
				(s, e) =>
				{
					Engine.effector.Flanger.Depth = ((UI.Knob)s).Value / 100f;
					_lblDepth.Text = Engine.effector.Flanger.Depth.ToString("0.00");
					Config.settings.Effectors.Flanger.Depth = ((UI.Knob)s).Value;
				});
			_lblDepth = CreateValueLabel(148, 8);
		}

		public override void LoadSettings()
		{
			ChkEnable.Checked = Engine.effector.Flanger.Enabled;
			_knobMix.Value = (int)Engine.effector.Flanger.Mix;
			_knobRate.Value = (int)(Engine.effector.Flanger.Rate * 10);
			_knobDepth.Value = (int)(Engine.effector.Flanger.Depth * 100);
			_lblMix.Text = Engine.effector.Flanger.Mix.ToString("0");
			_lblRate.Text = Engine.effector.Flanger.Rate.ToString("0.0");
			_lblDepth.Text = Engine.effector.Flanger.Depth.ToString("0.00");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Flanger.Switch(enabled);
			Config.settings.Effectors.Flanger.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Flanger.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<FlangerPreset>("Flanger", name);
			if (preset == null) return;
			_knobMix.Value = (int)preset.Mix;
			_knobRate.Value = (int)(preset.Rate * 10f);
			_knobDepth.Value = (int)(preset.Depth * 100f);
		}

		protected override EffectPreset CreatePreset(string name) =>
			new FlangerPreset
			{
				Name = name,
				Mix = _knobMix.Value,
				Rate = _knobRate.Value / 10f,
				Depth = _knobDepth.Value / 100f,
			};

		protected override void ApplyToEngine() { }
	}
}