using System;
using System.Windows.Forms;
using System.Drawing;

namespace UI
{
	internal class KnobInputForm : Form
	{
		public int InputValue { get; private set; }

		private readonly int _minimum;
		private readonly int _maximum;
		private readonly float _scale;
		private readonly string _unit;

		private TextBox _textBox;
		private Label _lblRange;
		private Button _btnOk;
		private Button _btnCancel;

		public KnobInputForm(
			int currentValue,
			int minimum,
			int maximum,
			string parameterName,
			string unit,
			float scale)
		{
			InputValue = currentValue;
			_minimum = minimum;
			_maximum = maximum;
			_scale = scale;
			_unit = unit;

			// FMOD実値での範囲表示
			string minText = FormatValue(minimum);
			string maxText = FormatValue(maximum);
			string rangeText = $"範囲: {minText} ～ {maxText}";
			if (!string.IsNullOrEmpty(unit))
				rangeText += $" {unit}";

			// フォーム設定
			MediaPlayer_X_Ark.ApplicationIcon.ApplyTo(this);
			Text = string.IsNullOrEmpty(parameterName)
				? "値を入力"
				: parameterName;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowInTaskbar = false;
			ClientSize = new Size(220, 95);
			TopMost = true;

			// 範囲ラベル
			_lblRange = new Label
			{
				Text = rangeText,
				Location = new Point(10, 8),
				Size = new Size(200, 18),
				ForeColor = SystemColors.GrayText,
				Font = new Font(Font.FontFamily, 8f),
			};

			// テキストボックス（FMOD実値で表示）
			_textBox = new TextBox
			{
				Text = FormatValue(currentValue),
				Location = new Point(10, 30),
				Size = new Size(200, 23),
				TabIndex = 0,
			};
			_textBox.SelectAll();
			_textBox.KeyDown += (s, e) =>
			{
				if (e.KeyCode == Keys.Enter) TryApply();
				if (e.KeyCode == Keys.Escape) DialogResult = DialogResult.Cancel;
			};

			_btnOk = new Button
			{
				Text = "OK",
				Location = new Point(10, 60),
				Size = new Size(95, 25),
				DialogResult = DialogResult.None,
				TabIndex = 1,
			};
			_btnOk.Click += (s, e) => TryApply();

			_btnCancel = new Button
			{
				Text = "キャンセル",
				Location = new Point(115, 60),
				Size = new Size(95, 25),
				DialogResult = DialogResult.Cancel,
				TabIndex = 2,
			};

			Controls.Add(_lblRange);
			Controls.Add(_textBox);
			Controls.Add(_btnOk);
			Controls.Add(_btnCancel);

			AcceptButton = _btnOk;
			CancelButton = _btnCancel;

			Shown += (s, e) => _textBox.Focus();
		}

		/// <summary>
		/// Knob内部値をFMOD実値の文字列に変換
		/// </summary>
		private string FormatValue(int knobValue)
		{
			if (_scale == 1f)
				return knobValue.ToString();
			return (knobValue / _scale).ToString("0.##");
		}

		private void TryApply()
		{
			if (float.TryParse(_textBox.Text, out float realVal))
			{
				// FMOD実値 → Knob内部値に変換
				int knobVal = (int)Math.Round(realVal * _scale);

				// クランプ
				if (knobVal < _minimum) knobVal = _minimum;
				if (knobVal > _maximum) knobVal = _maximum;

				InputValue = knobVal;
				DialogResult = DialogResult.OK;
			}
			else
			{
				string minText = FormatValue(_minimum);
				string maxText = FormatValue(_maximum);
				string unit = string.IsNullOrEmpty(_unit) ? "" : $" {_unit}";

				MessageBox.Show(
					$"有効な数値を入力してください。\n範囲: {minText} ～ {maxText}{unit}",
					"入力エラー",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);

				_textBox.Focus();
				_textBox.SelectAll();
			}
		}
	}
}
