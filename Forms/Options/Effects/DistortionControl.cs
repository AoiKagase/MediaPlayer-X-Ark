using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class DistortionControl : EffectControlBase
	{
		protected override string EffectName => "Distortion";
		protected override string EnableText => "Distortion";

		private UI.Knob _knobLevel;
		private TextBox _lblLevel;

		public DistortionControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobLevel = CreateKnob(8, 8, "Level", "", 0, 100, 5,
				(int)(Engine.effector.Distortion.Level * 100),
				(s, e) =>
				{
					Engine.effector.Distortion.Level =
						((UI.Knob)s).Value / 100f;
					_lblLevel.Text =
						Engine.effector.Distortion.Level.ToString("0.00");
				});
			_lblLevel = CreateValueLabel(8, 8);
		}

		public override void LoadSettings()
		{
			_loading = true;
			ChkEnable.Checked = Engine.effector.Distortion.Enabled;
			_knobLevel.Value = (int)(Engine.effector.Distortion.Level * 100);
			_lblLevel.Text = Engine.effector.Distortion.Level.ToString("0.00");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
			_loading = false;
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Distortion.Switch(enabled);
			Config.settings.Effectors.Distortion.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Distortion.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<DistortionPreset>("Distortion", name);
			if (preset == null) return;
			_knobLevel.Value = (int)(preset.Level * 100f);
		}

		protected override EffectPreset CreatePreset(string name) =>
			new DistortionPreset
			{
				Name = name,
				Level = _knobLevel.Value / 100f,
			};

		protected override void ApplyToEngine() { }
	}
}