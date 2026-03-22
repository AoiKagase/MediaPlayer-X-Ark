using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class EchoControl : EffectControlBase
	{
		protected override string EffectName => "Echo";
		protected override string EnableText => "Echo";

		private UI.Knob _knobDelay, _knobFeedback, _knobDry, _knobWet;
		private TextBox _lblDelay, _lblFeedback, _lblDry, _lblWet;

		public EchoControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobDelay = CreateKnob(8, 8, "Delay", "ms", 1, 5000, 100,
				(int)Engine.effector.Echo.Delay,
				(s, e) =>
				{
					Engine.effector.Echo.Delay = ((UI.Knob)s).Value;
					_lblDelay.Text = Engine.effector.Echo.Delay.ToString("0");
					Config.settings.Effectors.Echo.Delay = ((UI.Knob)s).Value;
				});
			_lblDelay = CreateValueLabel(8, 8);

			_knobFeedback = CreateKnob(78, 8, "Feedback", "%", 0, 100, 5,
				(int)Engine.effector.Echo.Feedback,
				(s, e) =>
				{
					Engine.effector.Echo.Feedback = ((UI.Knob)s).Value;
					_lblFeedback.Text = Engine.effector.Echo.Feedback.ToString("0");
					Config.settings.Effectors.Echo.Feedback = ((UI.Knob)s).Value;
				});
			_lblFeedback = CreateValueLabel(78, 8);

			_knobDry = CreateKnob(148, 8, "Dry", "dB", -80, 10, 5,
				(int)Engine.effector.Echo.DryLevel,
				(s, e) =>
				{
					Engine.effector.Echo.DryLevel = ((UI.Knob)s).Value;
					_lblDry.Text = Engine.effector.Echo.DryLevel.ToString("0");
					Config.settings.Effectors.Echo.Dry = ((UI.Knob)s).Value;
				});
			_lblDry = CreateValueLabel(148, 8);

			_knobWet = CreateKnob(218, 8, "Wet", "dB", -80, 10, 5,
				(int)Engine.effector.Echo.WetLevel,
				(s, e) =>
				{
					Engine.effector.Echo.WetLevel = ((UI.Knob)s).Value;
					_lblWet.Text = Engine.effector.Echo.WetLevel.ToString("0");
					Config.settings.Effectors.Echo.Wet = ((UI.Knob)s).Value;
				});
			_lblWet = CreateValueLabel(218, 8);
		}

		public override void LoadSettings()
		{
			_loading = true;
			ChkEnable.Checked = Engine.effector.Echo.Enabled;
			_knobDelay.Value = (int)Engine.effector.Echo.Delay;
			_knobFeedback.Value = (int)Engine.effector.Echo.Feedback;
			_knobDry.Value = (int)Engine.effector.Echo.DryLevel;
			_knobWet.Value = (int)Engine.effector.Echo.WetLevel;
			_lblDelay.Text = Engine.effector.Echo.Delay.ToString("0");
			_lblFeedback.Text = Engine.effector.Echo.Feedback.ToString("0");
			_lblDry.Text = Engine.effector.Echo.DryLevel.ToString("0");
			_lblWet.Text = Engine.effector.Echo.WetLevel.ToString("0");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
			_loading = false;
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Echo.Switch(enabled);
			Config.settings.Effectors.Echo.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Echo.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<EchoPreset>("Echo", name);
			if (preset == null) return;
			_knobDelay.Value = (int)preset.Delay;
			_knobFeedback.Value = (int)preset.Feedback;
			_knobDry.Value = (int)preset.Dry;
			_knobWet.Value = (int)preset.Wet;
		}

		protected override EffectPreset CreatePreset(string name) =>
			new EchoPreset
			{
				Name = name,
				Delay = _knobDelay.Value,
				Feedback = _knobFeedback.Value,
				Dry = _knobDry.Value,
				Wet = _knobWet.Value,
			};

		protected override void ApplyToEngine() { }
	}
}