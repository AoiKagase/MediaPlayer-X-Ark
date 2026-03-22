using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options.Effects
{
	public class PitchControl : OptionsControlBase
	{
		private bool _loading = false;
		private CheckBox _chkPitch, _chkFrequency, _chkSpeed;
		private UI.Knob _knobPitch, _knobFFT, _knobFrequency, _knobSpeed;
		private TextBox _lblPitch, _lblFFT, _lblFrequency, _lblSpeed;
		private GroupBox _grpPitch, _grpFrequency, _grpSpeed;

		public PitchControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		private void BuildLayout()
		{
			const int pad = 8;

			// ===========================
			// PitchShift グループ
			// ===========================
			_grpPitch = new GroupBox
			{
				Text = "",
				Location = new Point(pad, pad),
				Size = new Size(160, 200),
			};

			_chkPitch = new CheckBox
			{
				Text = "PitchShift",
				Location = new Point(6, 0),
				AutoSize = true,
				BackColor = System.Drawing.SystemColors.Control,
			};
			_chkPitch.CheckedChanged += ChkPitch_CheckedChanged;

			_knobPitch = CreateKnob(6, 20, "Pitch", "", 50, 200, 10,
				(int)(Engine.effector.PitchShift.Pitch * 100), _grpPitch,
				(s, e) =>
				{
					if (_loading) return;
					Engine.effector.PitchShift.Pitch = ((UI.Knob)s).Value / 100f;
					_lblPitch.Text = Engine.effector.PitchShift.Pitch.ToString("0.00");
					Config.settings.Effectors.PitchShift.Pitch = ((UI.Knob)s).Value;
				});
			_lblPitch = CreateValueLabel(6, 20, _grpPitch);

			_knobFFT = CreateKnob(76, 20, "FFT", "", 0, 4, 1,
				Config.settings.Effectors.PitchShift.FFT, _grpPitch,
				(s, e) =>
				{
					if (_loading) return;
					float[] fftsize = { 256, 512, 1024, 2048, 4096 };
					Engine.effector.PitchShift.FFTSize = fftsize[((UI.Knob)s).Value];
					_lblFFT.Text = Engine.effector.PitchShift.FFTSize.ToString("0");
					Config.settings.Effectors.PitchShift.FFT = ((UI.Knob)s).Value;
				});
			_lblFFT = CreateValueLabel(76, 20, _grpPitch);

			_grpPitch.Controls.Add(_chkPitch);

			// ===========================
			// Frequency グループ
			// ===========================
			_grpFrequency = new GroupBox
			{
				Text = "",
				Location = new Point(pad + 168, pad),
				Size = new Size(120, 200),
			};

			_chkFrequency = new CheckBox
			{
				Text = "Frequency",
				Location = new Point(6, 0),
				AutoSize = true,
				BackColor = System.Drawing.SystemColors.Control,
			};
			_chkFrequency.CheckedChanged += ChkFrequency_CheckedChanged;

			_knobFrequency = CreateKnob(6, 20, "Frequency", "", -100, 100, 5,
				Config.settings.Effectors.Frequency.Frequency, _grpFrequency,
				(s, e) =>
				{
					if (_loading) return; 
					Engine.effector.Frequency.SetFrequency(((UI.Knob)s).Value);
					_lblFrequency.Text = Engine.effector.Frequency.Hz.ToString("0");
					Config.settings.Effectors.Frequency.Frequency = ((UI.Knob)s).Value;
				});
			_lblFrequency = CreateValueLabel(6, 20, _grpFrequency);

			_grpFrequency.Controls.Add(_chkFrequency);

			// ===========================
			// Speed グループ
			// ===========================
			_grpSpeed = new GroupBox
			{
				Text = "",
				Location = new Point(pad + 296, pad),
				Size = new Size(120, 200),
			};

			_chkSpeed = new CheckBox
			{
				Text = "Speed",
				Location = new Point(6, 0),
				AutoSize = true,
				BackColor = System.Drawing.SystemColors.Control,
			};
			_chkSpeed.CheckedChanged += ChkSpeed_CheckedChanged;

			_knobSpeed = CreateKnob(6, 20, "Speed", "", -100, 100, 5,
				Config.settings.Effectors.Speed.Speed, _grpSpeed,
				(s, e) =>
				{
					if (_loading) return;
					Engine.effector.Speed = ((UI.Knob)s).Value;
					_lblSpeed.Text = Engine.effector.Speed.ToString();
					Config.settings.Effectors.Speed.Speed = ((UI.Knob)s).Value;
				});
			_lblSpeed = CreateValueLabel(6, 20, _grpSpeed);

			_grpSpeed.Controls.Add(_chkSpeed);

			Controls.AddRange(new Control[]
			{
				_grpPitch, _grpFrequency, _grpSpeed
			});

			Engine.effector.PitchShift.PropertyChanged += PitchShift_PropertyChanged;
			Engine.effector.Frequency.PropertyChanged += Frequency_PropertyChanged;
		}

		private UI.Knob CreateKnob(int x, int y, string name, string unit,
			int min, int max, int largeChange, int value,
			GroupBox parent, EventHandler onChanged)
		{
			parent.Controls.Add(new Label
			{
				Text = name,
				Location = new Point(x, y),
				Size = new Size(64, 16),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Yu Gothic UI", 9f),
			});
			var knob = new UI.Knob
			{
				Location = new Point(x, y + 20),
				Size = new Size(55, 55),
				ParameterName = name,
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
			knob.ValueChanged += onChanged;
			parent.Controls.Add(knob);
			return knob;
		}

		private TextBox CreateValueLabel(int x, int y, GroupBox parent)
		{
			var txt = new TextBox
			{
				Location = new Point(x, y + 80),
				Size = new Size(55, 16),
				BorderStyle = BorderStyle.None,
				ReadOnly = true,
				Text = "0",
				TextAlign = HorizontalAlignment.Center,
				Font = new Font("Yu Gothic UI", 9f),
			};
			parent.Controls.Add(txt);
			return txt;
		}

		public override void LoadSettings()
		{
			_loading = true;
			_chkPitch.Checked = Engine.effector.PitchShift.Enabled;
			_chkFrequency.Checked = Engine.effector.Frequency.Enabled;
			_chkSpeed.Checked = Engine.effector.SpeedEnabled;

			_knobPitch.Value = (int)(Engine.effector.PitchShift.Pitch * 100);
			_knobFFT.Value = Config.settings.Effectors.PitchShift.FFT;
			_knobFrequency.Value = Config.settings.Effectors.Frequency.Frequency;
			_knobSpeed.Value = Config.settings.Effectors.Speed.Speed;

			_lblPitch.Text = Engine.effector.PitchShift.Pitch.ToString("0.00");
			_lblFFT.Text = Engine.effector.PitchShift.FFTSize.ToString("0");
			_lblFrequency.Text = Engine.effector.Frequency.Hz.ToString("0");
			_lblSpeed.Text = Engine.effector.Speed.ToString();
			_loading = false;
			UpdateSpeedMode(Engine.effector.SpeedEnabled);
		}

		public override void SaveSettings() { }

		private void UpdateSpeedMode(bool speedEnabled)
		{
			_grpPitch.Enabled = !speedEnabled;
			_grpFrequency.Enabled = !speedEnabled;
			_knobSpeed.Enabled = speedEnabled;
		}

		private void ChkPitch_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			Engine.effector.PitchShift.Switch(_chkPitch.Checked);
			Config.settings.Effectors.PitchShift.Enable = _chkPitch.Checked;
			if (_chkPitch.Checked)
				Engine.effector.PitchShift.Pitch = _knobPitch.Value / 100f;
		}

		private void ChkFrequency_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return; 
			Engine.effector.Frequency.Switch(_chkFrequency.Checked);
			Config.settings.Effectors.Frequency.Enable = _chkFrequency.Checked;
			if (_chkFrequency.Checked)
				Engine.effector.Frequency.SetFrequency(_knobFrequency.Value);
		}

		private void ChkSpeed_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return; 
			Config.settings.Effectors.Speed.Enable =
				Engine.effector.SpeedEnabled = _chkSpeed.Checked;

			if (_chkSpeed.Checked)
			{
				Engine.effector.PitchShift.Switch(true);
				Engine.effector.Frequency.Switch(true);
			}
			UpdateSpeedMode(_chkSpeed.Checked);
		}

		private void PitchShift_PropertyChanged(
			object sender, PropertyChangedEventArgs e)
		{
			if (_chkSpeed.Checked)
			{
				_knobPitch.Value = (int)(Engine.effector.PitchShift.Pitch * 100);
				_lblPitch.Text = Engine.effector.PitchShift.Pitch.ToString("0.00");
			}
		}

		private void Frequency_PropertyChanged(
			object sender, PropertyChangedEventArgs e)
		{
			if (_chkSpeed.Checked)
			{
				int val = (int)(Engine.effector.Frequency.Hz / 44100f * 100f - 100f);
				_knobFrequency.Value = Math.Max(_knobFrequency.Minimum,
					Math.Min(_knobFrequency.Maximum, val));
				_lblFrequency.Text = Engine.effector.Frequency.Hz.ToString("0");
			}
		}
	}
}