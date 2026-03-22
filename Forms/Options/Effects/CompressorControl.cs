using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class CompressorControl : EffectControlBase
	{
		protected override string EffectName => "Compressor";
		protected override string EnableText => "Compressor";

		private UI.Knob _knobThreshold, _knobRatio, _knobAttack,
						_knobRelease, _knobGain;
		private TextBox _lblThreshold, _lblRatio, _lblAttack,
						_lblRelease, _lblGain;
		private CheckBox _chkLinked;

		public CompressorControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildBaseLayout();

			_knobThreshold = CreateKnob(8, 8, "Threshold", "dB", -60, 0, 5,
				(int)Engine.effector.Compressor.Threshold,
				(s, e) =>
				{
					Engine.effector.Compressor.Threshold = ((UI.Knob)s).Value;
					_lblThreshold.Text = Engine.effector.Compressor.Threshold.ToString("0");
					Config.settings.Effectors.Compressor.Threshold = ((UI.Knob)s).Value;
				});
			_lblThreshold = CreateValueLabel(8, 8);

			_knobRatio = CreateKnob(78, 8, "Ratio", "", 1, 50, 5,
				(int)Engine.effector.Compressor.Ratio,
				(s, e) =>
				{
					Engine.effector.Compressor.Ratio = ((UI.Knob)s).Value;
					_lblRatio.Text = Engine.effector.Compressor.Ratio.ToString("0");
					Config.settings.Effectors.Compressor.Ratio = ((UI.Knob)s).Value;
				});
			_lblRatio = CreateValueLabel(78, 8);

			_knobAttack = CreateKnob(148, 8, "Attack", "ms", 1, 5000, 100,
				(int)(Engine.effector.Compressor.Attack * 10),
				(s, e) =>
				{
					Engine.effector.Compressor.Attack = ((UI.Knob)s).Value / 10f;
					_lblAttack.Text = Engine.effector.Compressor.Attack.ToString("0.0");
					Config.settings.Effectors.Compressor.Attack = ((UI.Knob)s).Value;
				});
			_lblAttack = CreateValueLabel(148, 8);

			_knobRelease = CreateKnob(218, 8, "Release", "ms", 10, 5000, 100,
				(int)Engine.effector.Compressor.Release,
				(s, e) =>
				{
					Engine.effector.Compressor.Release = ((UI.Knob)s).Value;
					_lblRelease.Text = Engine.effector.Compressor.Release.ToString("0");
					Config.settings.Effectors.Compressor.Release = ((UI.Knob)s).Value;
				});
			_lblRelease = CreateValueLabel(218, 8);

			_knobGain = CreateKnob(288, 8, "Gain", "dB", -30, 30, 2,
				(int)Engine.effector.Compressor.Gain,
				(s, e) =>
				{
					Engine.effector.Compressor.Gain = ((UI.Knob)s).Value;
					_lblGain.Text = Engine.effector.Compressor.Gain.ToString("0");
					Config.settings.Effectors.Compressor.Gain = ((UI.Knob)s).Value;
				});
			_lblGain = CreateValueLabel(288, 8);

			_chkLinked = new CheckBox
			{
				Text = "Linked",
				Location = new System.Drawing.Point(8, 110),
				AutoSize = true,
			};
			_chkLinked.CheckedChanged += (s, e) =>
				Engine.effector.Compressor.Linked = _chkLinked.Checked;
			KnobPanel.Controls.Add(_chkLinked);
		}

		public override void LoadSettings()
		{
			_loading = true;
			ChkEnable.Checked = Engine.effector.Compressor.Enabled;
			_knobThreshold.Value = (int)Engine.effector.Compressor.Threshold;
			_knobRatio.Value = (int)Engine.effector.Compressor.Ratio;
			_knobAttack.Value = (int)(Engine.effector.Compressor.Attack * 10);
			_knobRelease.Value = (int)Engine.effector.Compressor.Release;
			_knobGain.Value = (int)Engine.effector.Compressor.Gain;
			_chkLinked.Checked = Engine.effector.Compressor.Linked;
			_lblThreshold.Text = Engine.effector.Compressor.Threshold.ToString("0");
			_lblRatio.Text = Engine.effector.Compressor.Ratio.ToString("0");
			_lblAttack.Text = Engine.effector.Compressor.Attack.ToString("0.0");
			_lblRelease.Text = Engine.effector.Compressor.Release.ToString("0");
			_lblGain.Text = Engine.effector.Compressor.Gain.ToString("0");
			LoadEffectPresets();
			SetControlsEnabled(ChkEnable.Checked);
			_loading = false;
		}

		public override void SaveSettings() { }

		protected override void SwitchEngine(bool enabled)
		{
			Engine.effector.Compressor.Switch(enabled);
			Config.settings.Effectors.Compressor.Enable = enabled;
		}

		protected override bool GetEngineEnabled() =>
			Engine.effector.Compressor.Enabled;

		protected override void ApplyPreset(string name)
		{
			var preset = EffectPreset.Load<CompressorPreset>("Compressor", name);
			if (preset == null) return;
			_knobThreshold.Value = (int)preset.Threshold;
			_knobRatio.Value = (int)preset.Ratio;
			_knobAttack.Value = (int)preset.Attack;
			_knobRelease.Value = (int)preset.Release;
			_knobGain.Value = (int)preset.Gain;
			_chkLinked.Checked = preset.Linked;
		}

		protected override EffectPreset CreatePreset(string name) =>
			new CompressorPreset
			{
				Name = name,
				Threshold = _knobThreshold.Value,
				Ratio = _knobRatio.Value,
				Attack = _knobAttack.Value,
				Release = _knobRelease.Value,
				Gain = _knobGain.Value,
				Linked = _chkLinked.Checked,
			};

		protected override void ApplyToEngine() { }
	}
}