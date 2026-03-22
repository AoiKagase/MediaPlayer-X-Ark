using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public abstract class EffectControlBase : OptionsControlBase
	{
		protected CheckBox ChkEnable;
		protected ComboBox CmbPreset;
		protected Button BtnPresetSave;
		protected Button BtnPresetDelete;
		protected Panel KnobPanel;

		protected abstract string EffectName { get; }
		protected abstract string EnableText { get; }

		protected bool _loading = false;
		protected EffectControlBase(IPlayerEngine engine, IConfigService config)
			: base(engine, config) { }

		protected void BuildBaseLayout()
		{
			const int pad = 8;

			ChkEnable = new CheckBox
			{
				Text = EnableText,
				Location = new Point(pad, 4),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
				BackColor = System.Drawing.SystemColors.Control,
			};
			ChkEnable.CheckedChanged += ChkEnable_CheckedChanged;

			CmbPreset = new ComboBox
			{
				Location = new Point(pad, 26),
				Size = new Size(140, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			CmbPreset.SelectedIndexChanged += CmbPreset_SelectedIndexChanged;

			BtnPresetSave = new Button
			{
				Text = "保存",
				Location = new Point(pad + 144, 26),
				Size = new Size(50, 23),
			};
			BtnPresetSave.Click += BtnPresetSave_Click;

			BtnPresetDelete = new Button
			{
				Text = "削除",
				Location = new Point(pad + 198, 26),
				Size = new Size(50, 23),
			};
			BtnPresetDelete.Click += BtnPresetDelete_Click;

			KnobPanel = new Panel
			{
				Location = new Point(pad, 56),
				Size = new Size(560, 360),
			};

			Controls.AddRange(new Control[]
			{
				ChkEnable, CmbPreset,
				BtnPresetSave, BtnPresetDelete,
				KnobPanel,
			});
		}

		// ===========================
		// Knobヘルパー
		// ===========================
		protected UI.Knob CreateKnob(
			int x, int y,
			string paramName, string unit,
			int min, int max, int largeChange,
			int value,
			EventHandler onValueChanged)
		{
			var knob = new UI.Knob
			{
				Location = new Point(x, y + 20),
				Size = new Size(55, 55),
				ParameterName = paramName,
				Unit = unit,
				Minimum = min,
				Maximum = max,
				LargeChange = largeChange,
				Value = value,
				BorderColor = System.Drawing.SystemColors.ControlDarkDark,
				BorderWidth = 2,
				HasTicks = true,
				KnobColor = System.Drawing.SystemColors.Control,
				PointerColor = System.Drawing.SystemColors.ControlText,
				TickColor = System.Drawing.SystemColors.ControlDarkDark,
				PointerWidth = 2,
				PointerOffset = 4,
			};
			knob.ValueChanged += onValueChanged;

			KnobPanel.Controls.Add(new Label
			{
				Text = paramName,
				Location = new Point(x, y),
				Size = new Size(64, 16),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Yu Gothic UI", 9f),
			});
			KnobPanel.Controls.Add(knob);

			return knob;
		}

		protected TextBox CreateValueLabel(int x, int y, string initialValue = "0")
		{
			var txt = new TextBox
			{
				Location = new Point(x, y + 80),
				Size = new Size(55, 16),
				BorderStyle = BorderStyle.None,
				ReadOnly = true,
				Text = initialValue,
				TextAlign = HorizontalAlignment.Center,
				Font = new Font("Yu Gothic UI", 9f),
			};
			KnobPanel.Controls.Add(txt);
			return txt;
		}

		// ===========================
		// プリセット
		// ===========================
		protected void LoadEffectPresets()
		{
			_loading = true;
			CmbPreset.Items.Clear();
			CmbPreset.Items.Add("");
			foreach (var name in EffectPreset.GetPresetNames(EffectName))
				CmbPreset.Items.Add(name);

			if (Config.settings.EffectPresets.TryGetValue(EffectName, out var current))
				CmbPreset.SelectedItem = current;
			_loading = false;
		}

		protected void SetControlsEnabled(bool enabled)
		{
			KnobPanel.Enabled = enabled;
			CmbPreset.Enabled = enabled;
			BtnPresetSave.Enabled = enabled;
			BtnPresetDelete.Enabled = enabled;
		}

		// ===========================
		// 抽象メソッド
		// ===========================
		protected abstract void ApplyPreset(string name);
		protected abstract EffectPreset CreatePreset(string name);
		protected abstract void ApplyToEngine();
		protected abstract bool GetEngineEnabled();
		protected abstract void SwitchEngine(bool enabled);

		// ===========================
		// イベント
		// ===========================
		private void ChkEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			SwitchEngine(ChkEnable.Checked);
			SetControlsEnabled(ChkEnable.Checked);
		}

		private void CmbPreset_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			var name = CmbPreset.SelectedItem as string;
			if (string.IsNullOrEmpty(name)) return;
			ApplyPreset(name);
			Config.settings.EffectPresets[EffectName] = name;
		}

		private void BtnPresetSave_Click(object sender, EventArgs e)
		{
			using (var form = new PresetNameInputForm())
			{
				if (form.ShowDialog() != DialogResult.OK) return;
				var preset = CreatePreset(form.PresetNameValue);
				preset.Save();
				LoadEffectPresets();
				CmbPreset.SelectedItem = preset.Name;
				Config.settings.EffectPresets[EffectName] = preset.Name;
			}
		}

		private void BtnPresetDelete_Click(object sender, EventArgs e)
		{
			var name = CmbPreset.SelectedItem as string;
			if (string.IsNullOrEmpty(name)) return;

			if (MessageBox.Show($"プリセット「{name}」を削除しますか？",
				"削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				!= DialogResult.Yes) return;

			// ★直接パスを構築して削除
			var path = Path.Combine(EffectPreset.PresetRoot, EffectName, name + ".json");
			if (File.Exists(path))
				File.Delete(path);

			Config.settings.EffectPresets.Remove(EffectName);
			LoadEffectPresets();
		}
	}
}