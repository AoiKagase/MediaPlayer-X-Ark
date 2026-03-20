using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Effector;
using MediaPlayer_X_Ark.Engine.Effector.Presets;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class GEQControl : OptionsControlBase
	{
		private CheckBox _chkEnable;
		private PictureBox _pictGraph;
		private ColorSlider.ColorSlider[] _sliders;
		private ComboBox _cmbPreset;
		private Button _btnPresetSave;
		private Button _btnPresetDelete;
		private bool _internalChanged = false;

		private static readonly string[] _builtinPresets =
		{
			"Normal", "Rock", "Pop", "Bass Boost",
			"Trable Boost", "Total Boost", "Total Reduce"
		};

		private static readonly string[] _bandLabels =
		{
			"32", "60", "125", "250", "500",
			"1K", "2K", "4K", "8K", "16K", "20K", "22K"
		};

		public GEQControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			const int pad = 8;
			const int graphHeight = 137;
			const int sliderY = graphHeight + 60;
			const int sliderHeight = 154;
			const int sliderWidth = 40;
			const int sliderSpacing = 45;
			const int sliderStartX = 30;

			// ===========================
			// チェックボックス
			// ===========================
			_chkEnable = new CheckBox
			{
				Text = "Graphic Equalizer",
				Location = new Point(pad, 4),
				AutoSize = true,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_chkEnable.CheckedChanged += ChkEnable_CheckedChanged;

			// ===========================
			// プリセット
			// ===========================
			_cmbPreset = new ComboBox
			{
				Location = new Point(pad, 26),
				Size = new Size(140, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			_cmbPreset.Items.AddRange(_builtinPresets);
			_cmbPreset.SelectedIndexChanged += CmbPreset_SelectedIndexChanged;

			_btnPresetSave = new Button
			{
				Text = "保存",
				Location = new Point(pad + 144, 26),
				Size = new Size(50, 23),
			};
			_btnPresetSave.Click += BtnPresetSave_Click;

			_btnPresetDelete = new Button
			{
				Text = "削除",
				Location = new Point(pad + 198, 26),
				Size = new Size(50, 23),
			};
			_btnPresetDelete.Click += BtnPresetDelete_Click;

			// ===========================
			// グラフ
			// ===========================
			_pictGraph = new PictureBox
			{
				Location = new Point(pad, 54),
				Size = new Size(sliderStartX + sliderSpacing * 12 + 10, graphHeight),
				BorderStyle = BorderStyle.Fixed3D,
			};

			// ===========================
			// スライダー12本
			// ===========================
			_sliders = new ColorSlider.ColorSlider[12];
			for (int i = 0; i < 12; i++)
			{
				_sliders[i] = CreateSlider(
					sliderStartX + i * sliderSpacing,
					sliderY,
					sliderWidth,
					sliderHeight,
					i == 0); // 最初だけ目盛り表示
				_sliders[i].ValueChanged += Slider_ValueChanged;
			}

			// ===========================
			// バンドラベル
			// ===========================
			for (int i = 0; i < _bandLabels.Length; i++)
			{
				Controls.Add(new Label
				{
					Text = _bandLabels[i],
					Location = new Point(sliderStartX + i * sliderSpacing + 2,
						sliderY + sliderHeight + 4),
					Size = new Size(sliderWidth, 16),
					TextAlign = ContentAlignment.MiddleCenter,
					Font = new Font("Yu Gothic UI", 8f),
				});
			}

			// ===========================
			// 中央線
			// ===========================
			var centerLine = new Label
			{
				BorderStyle = BorderStyle.FixedSingle,
				Location = new Point(sliderStartX - 4,
					sliderY + sliderHeight / 2),
				Size = new Size(sliderSpacing * 12 + 8, 1),
			};

			Controls.Add(_chkEnable);
			Controls.Add(_cmbPreset);
			Controls.Add(_btnPresetSave);
			Controls.Add(_btnPresetDelete);
			Controls.Add(_pictGraph);
			Controls.Add(centerLine);
			Controls.AddRange(_sliders);
		}

		private ColorSlider.ColorSlider CreateSlider(
			int x, int y, int w, int h, bool showDivisions)
		{
			return new ColorSlider.ColorSlider
			{
				AutoSize = false,
				BackColor = Color.Transparent,
				Location = new Point(x, y),
				Size = new Size(w, h),
				Orientation = Orientation.Vertical,
				Minimum = -100,
				Maximum = 100,
				LargeChange = 5,
				SmallChange = 1,
				Padding = 10,
				ShowDivisionsText = showDivisions,
				ShowSmallScale = false,
				BarInnerColor = SystemColors.ControlLight,
				BarPenColorBottom = SystemColors.ControlDark,
				BarPenColorTop = SystemColors.ControlDark,
				ElapsedInnerColor = SystemColors.ControlLight,
				ElapsedPenColorBottom = SystemColors.ControlDark,
				ElapsedPenColorTop = SystemColors.ControlDark,
				ThumbInnerColor = SystemColors.ControlDark,
				ThumbOuterColor = SystemColors.ControlDarkDark,
				ThumbPenColor = SystemColors.ControlDark,
				ThumbRoundRectSize = new Size(1, 1),
				ThumbSize = new Size(16, 8),
				TickColor = SystemColors.ControlDarkDark,
				TickDivide = 10f,
				Font = new Font("Yu Gothic UI", 6f),
			};
		}

		public override void LoadSettings()
		{
			_chkEnable.Checked = Config.settings.Effectors.GEqualizer.Enable;

			LoadPresets();
			_cmbPreset.SelectedIndex = Config.settings.Effectors.GEqualizer.Preset;

			_internalChanged = true;
			_sliders[0].Value = Config.settings.Effectors.GEqualizer.GEQ_32;
			_sliders[1].Value = Config.settings.Effectors.GEqualizer.GEQ_60;
			_sliders[2].Value = Config.settings.Effectors.GEqualizer.GEQ_125;
			_sliders[3].Value = Config.settings.Effectors.GEqualizer.GEQ_250;
			_sliders[4].Value = Config.settings.Effectors.GEqualizer.GEQ_500;
			_sliders[5].Value = Config.settings.Effectors.GEqualizer.GEQ_1K;
			_sliders[6].Value = Config.settings.Effectors.GEqualizer.GEQ_2K;
			_sliders[7].Value = Config.settings.Effectors.GEqualizer.GEQ_4K;
			_sliders[8].Value = Config.settings.Effectors.GEqualizer.GEQ_8K;
			_sliders[9].Value = Config.settings.Effectors.GEqualizer.GEQ_16K;
			_sliders[10].Value = Config.settings.Effectors.GEqualizer.GEQ_20K;
			_sliders[11].Value = Config.settings.Effectors.GEqualizer.GEQ_22K;
			_internalChanged = false;

			SetControlsEnabled(_chkEnable.Checked);
			PaintGraph();

			// PropertyChangedイベント登録
			Engine.effector.GEqualizer.PropertyChanged += GEqualizer_PropertyChanged;
		}

		public override void SaveSettings() { }

		private void LoadPresets()
		{
			// 組み込みプリセット以外を削除してユーザープリセットを追加
			for (int i = _cmbPreset.Items.Count - 1; i >= 0; i--)
			{
				var item = _cmbPreset.Items[i] as string;
				if (!_builtinPresets.Contains(item))
					_cmbPreset.Items.RemoveAt(i);
			}
			foreach (var name in EffectPreset.GetPresetNames("GEQ"))
				if (!_builtinPresets.Contains(name))
					_cmbPreset.Items.Add(name);
		}

		private void PaintGraph()
		{
			var canvas = new Bitmap(_pictGraph.Width, _pictGraph.Height);
			using (var g = Graphics.FromImage(canvas))
			{
				int hCenter = _pictGraph.Height / 2;
				int wWidth = _pictGraph.Width / 13;
				float hHeight = _pictGraph.Height / 200f;
				var points = new Point[14];
				points[0] = new Point(0, hCenter);
				points[13] = new Point(_pictGraph.Width, hCenter);
				for (int i = 0; i < 12; i++)
					points[1 + i] = new Point(
						wWidth * i + wWidth,
						hCenter - (int)(hHeight * (int)_sliders[i].Value));

				using (var pen = new Pen(Color.Black, 1))
					g.DrawCurve(pen, points, 0.5f);
			}
			_pictGraph.Image = canvas;
		}

		private void SetControlsEnabled(bool enabled)
		{
			foreach (var s in _sliders) s.Enabled = enabled;
			_cmbPreset.Enabled = enabled;
			_btnPresetSave.Enabled = enabled;
			_btnPresetDelete.Enabled = enabled;
			_pictGraph.Enabled = enabled;
		}

		// ===========================
		// イベント
		// ===========================
		private void ChkEnable_CheckedChanged(object sender, EventArgs e)
		{
			Engine.effector.GEqualizer.Switch(_chkEnable.Checked);
			Config.settings.Effectors.GEqualizer.Enable = _chkEnable.Checked;
			SetControlsEnabled(_chkEnable.Checked);
		}

		private void Slider_ValueChanged(object sender, EventArgs e)
		{
			PaintGraph();
			if (_internalChanged) return;

			int index = Array.IndexOf(_sliders,
				(ColorSlider.ColorSlider)sender);
			if (index < 0) return;

			int value = (int)((ColorSlider.ColorSlider)sender).Value;
			Engine.effector.GEqualizer.SetGain(
				(GEqualizer.EQ_HZ)index, value / 10f);
			Config.settings.Effectors.GEqualizer.SetByIndex(index, value);
		}

		private void GEqualizer_PropertyChanged(
			object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "Gain") return;
			_internalChanged = true;
			for (int i = 0; i < _sliders.Length; i++)
				_sliders[i].Value = (int)(Engine.effector.GEqualizer.Gain[i] * 10);
			_internalChanged = false;
		}

		private void CmbPreset_SelectedIndexChanged(object sender, EventArgs e)
		{
			var name = _cmbPreset.SelectedItem as string;
			if (string.IsNullOrEmpty(name)) return;

			int builtinIndex = Array.IndexOf(_builtinPresets, name);
			if (builtinIndex >= 0)
			{
				Engine.effector.GEqualizer.SetPreset(builtinIndex);
				Config.settings.Effectors.GEqualizer.Preset = builtinIndex;
				return;
			}

			var preset = EffectPreset.Load<GEqualizerPreset>("GEQ", name);
			if (preset == null) return;

			_internalChanged = true;
			_sliders[0].Value = (decimal)preset.Hz32;
			_sliders[1].Value = (decimal)preset.Hz60;
			_sliders[2].Value = (decimal)preset.Hz125;
			_sliders[3].Value = (decimal)preset.Hz250;
			_sliders[4].Value = (decimal)preset.Hz500;
			_sliders[5].Value = (decimal)preset.Hz1K;
			_sliders[6].Value = (decimal)preset.Hz2K;
			_sliders[7].Value = (decimal)preset.Hz4K;
			_sliders[8].Value = (decimal)preset.Hz8K;
			_sliders[9].Value = (decimal)preset.Hz16K;
			_sliders[10].Value = (decimal)preset.Hz20K;
			_sliders[11].Value = (decimal)preset.Hz22K;
			_internalChanged = false;

			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_32, preset.Hz32 / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_60, preset.Hz60 / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_125, preset.Hz125 / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_250, preset.Hz250 / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_500, preset.Hz500 / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_1K, preset.Hz1K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_2K, preset.Hz2K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_4K, preset.Hz4K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_8K, preset.Hz8K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_16K, preset.Hz16K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_20K, preset.Hz20K / 10f);
			Engine.effector.GEqualizer.SetGain(GEqualizer.EQ_HZ.HZ_22K, preset.Hz22K / 10f);

			Config.settings.EffectPresets["GEQ"] = name;
			PaintGraph();
		}

		private void BtnPresetSave_Click(object sender, EventArgs e)
		{
			using (var form = new PresetNameInputForm())
			{
				if (form.ShowDialog() != DialogResult.OK) return;
				if (_builtinPresets.Contains(form.PresetNameValue))
				{
					MessageBox.Show("組み込みプリセット名は使用できません。",
						"保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var preset = new GEqualizerPreset
				{
					Name = form.PresetNameValue,
					Hz32 = (float)_sliders[0].Value,
					Hz60 = (float)_sliders[1].Value,
					Hz125 = (float)_sliders[2].Value,
					Hz250 = (float)_sliders[3].Value,
					Hz500 = (float)_sliders[4].Value,
					Hz1K = (float)_sliders[5].Value,
					Hz2K = (float)_sliders[6].Value,
					Hz4K = (float)_sliders[7].Value,
					Hz8K = (float)_sliders[8].Value,
					Hz16K = (float)_sliders[9].Value,
					Hz20K = (float)_sliders[10].Value,
					Hz22K = (float)_sliders[11].Value,
				};
				preset.Save();
				LoadPresets();
				_cmbPreset.SelectedItem = preset.Name;
				Config.settings.EffectPresets["GEQ"] = preset.Name;
			}
		}

		private void BtnPresetDelete_Click(object sender, EventArgs e)
		{
			var name = _cmbPreset.SelectedItem as string;
			if (string.IsNullOrEmpty(name)) return;

			if (_builtinPresets.Contains(name))
			{
				MessageBox.Show("組み込みプリセットは削除できません。",
					"削除エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (MessageBox.Show($"プリセット「{name}」を削除しますか？",
				"削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				!= DialogResult.Yes) return;

			new GEqualizerPreset { Name = name }.Delete();
			Config.settings.EffectPresets.Remove("GEQ");
			LoadPresets();
			_cmbPreset.SelectedItem = "Normal";
			Config.settings.Effectors.GEqualizer.Preset = 0;
		}
	}
}