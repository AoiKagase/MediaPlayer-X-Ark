using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class NormalizeControl : EffectControlBase
	{
		protected override string EffectName => "Normalize";
		protected override string EnableText => "Normalize";

		private UI.Knob _knobFadeTime;
		private UI.Knob _knobMaxAmp;
		private UI.Knob _knobThreshold;
		private TextBox _lblFadeTime;
		private TextBox _lblMaxAmp;
		private TextBox _lblThreshold;

		public NormalizeControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			// FadeTime: 1〜20000ms、×10でKnob管理（1〜2000 → 10〜20000ms）
			_knobFadeTime = CreateKnob(8, 8, "FadeTime", "ms", 1, 2000, 100,
				(int)(Engine.effector.Normalize.FadeTime / 10f),
				(s, e) =>
				{
					Engine.effector.Normalize.FadeTime = ((UI.Knob)s).Value * 10f;
					_lblFadeTime.Text = Engine.effector.Normalize.FadeTime.ToString("0");
					Config.settings.Effectors.Normalize.FadeTime = Engine.effector.Normalize.FadeTime;
				});
			_lblFadeTime = CreateValueLabel(8, 8, "5000");

			// MaxAmp: 0.0〜2.0、×10でKnob管理（0〜20 → 0.0〜2.0）
			_knobMaxAmp = CreateKnob(78, 8, "MaxAmp", "", 0, 20, 1,
				(int)(Engine.effector.Normalize.MaxAmp * 10f),
				(s, e) =>
				{
					Engine.effector.Normalize.MaxAmp = ((UI.Knob)s).Value / 10f;
					_lblMaxAmp.Text = Engine.effector.Normalize.MaxAmp.ToString("0.0");
					Config.settings.Effectors.Normalize.MaxAmp = Engine.effector.Normalize.MaxAmp;
				});
			_lblMaxAmp = CreateValueLabel(78, 8, "1.0");

			// Threshold: 0.00〜1.00、×100でKnob管理（0〜100 → 0.00〜1.00）
			_knobThreshold = CreateKnob(148, 8, "Threshold", "", 0, 100, 5,
				(int)(Engine.effector.Normalize.Threshold * 100f),
				(s, e) =>
				{
					Engine.effector.Normalize.Threshold = ((UI.Knob)s).Value / 100f;
					_lblThreshold.Text = Engine.effector.Normalize.Threshold.ToString("0.00");
					Config.settings.Effectors.Normalize.Threshold = Engine.effector.Normalize.Threshold;
				});
			_lblThreshold = CreateValueLabel(148, 8, "0.00");
		}

		public override void LoadSettings()
		{

			_loading = true;
			ChkEnable.Checked = Engine.effector.Normalize.Enabled;
			_knobFadeTime.Value = (int)(Engine.effector.Normalize.FadeTime / 10f);
			_knobMaxAmp.Value = (int)(Engine.effector.Normalize.MaxAmp * 10f);
			_knobThreshold.Value = (int)(Engine.effector.Normalize.Threshold * 100f);
			_lblFadeTime.Text = Engine.effector.Normalize.FadeTime.ToString("0");
			_lblMaxAmp.Text = Engine.effector.Normalize.MaxAmp.ToString("0.0");
			_lblThreshold.Text = Engine.effector.Normalize.Threshold.ToString("0.00");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
			_loading = false;
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Normalize.Switch(enabled);
			Config.settings.Effectors.Normalize.Enable = enabled;
		}

		protected override bool GetEngineEnabled()
			=> Engine.effector.Normalize.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<NormalizePreset>("Normalize", name);
			if (preset == null) return;
			_knobFadeTime.Value = (int)(preset.FadeTime / 10f);
			_knobMaxAmp.Value = (int)(preset.MaxAmp * 10f);
			_knobThreshold.Value = (int)(preset.Threshold * 100f);
		}

		protected override EffectPreset CreatePreset(string name) =>
			new NormalizePreset
			{
				Name = name,
				FadeTime = _knobFadeTime.Value * 10f,
				MaxAmp = _knobMaxAmp.Value / 10f,
				Threshold = _knobThreshold.Value / 100f,
			};

		protected override void ApplyToEngine() { }
	}
}
