using ATL;
using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class OutputSettingsControl : OptionsControlBase
	{
		private ComboBox _cmbOutput;
		private ComboBox _cmbDevice;
		private Label _lblOutputNote;
		private Label _lblSoundFontNote;
		private NumericUpDown _nudStreamBuffer;
		private NumericUpDown _nudDspBufferSize;
		private NumericUpDown _nudDspBufferCount;
		private ComboBox _cmbMidiRenderer;
		private TextBox _txtSoundFont;
		private Button _btnSoundFontBrowse;
		private Button _btnSave;

		private MainForm _mainForm;

		public OutputSettingsControl(IPlayerEngine engine, IConfigService config, MainForm mainForm)
			: base(engine, config)
		{
			_mainForm = mainForm;
			BuildLayout();
		}

		private void BuildLayout()
		{
			var y = 16;
			const int labelWidth = 140;
			const int controlX = 160;
			const int controlWidth = 240;
			const int lineHeight = 32;

			// ===========================
			// 出力設定
			// ===========================
			var grpOutput = new GroupBox
			{
				Text = "出力設定",
				Location = new Point(16, y),
				Size = new Size(520, 120),
			};

			grpOutput.Controls.Add(new Label
			{
				Text = "出力方式",
				Location = new Point(12, 28),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});

			_cmbOutput = new ComboBox
			{
				Location = new Point(controlX, 24),
				Size = new Size(controlWidth, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			_cmbOutput.Items.AddRange(new object[]
			{
				"自動検出",
				"Windows Audio Session API",
				"Low latency ASIO 2.0",
				"Windows Sonic",
			});
			_cmbOutput.SelectedIndexChanged += CmbOutput_SelectedIndexChanged;

			grpOutput.Controls.Add(new Label
			{
				Text = "デバイス",
				Location = new Point(12, 28 + lineHeight),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});

			_cmbDevice = new ComboBox
			{
				Location = new Point(controlX, 24 + lineHeight),
				Size = new Size(controlWidth, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};

			_lblOutputNote = new Label
			{
				Location = new Point(controlX, 24 + lineHeight * 2),
				Size = new Size(controlWidth, 30),
				ForeColor = System.Drawing.Color.Gray,
				Text = "",
			};

			grpOutput.Controls.AddRange(new Control[]
			{
				_cmbOutput, _cmbDevice, _lblOutputNote
			});

			y += grpOutput.Height + 12;

			// ===========================
			// バッファ設定
			// ===========================
			var grpBuffer = new GroupBox
			{
				Text = "バッファ設定",
				Location = new Point(16, y),
				Size = new Size(520, 145),
			};

			grpBuffer.Controls.Add(new Label
			{
				Text = "ストリームバッファ",
				Location = new Point(12, 28),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});
			_nudStreamBuffer = new NumericUpDown
			{
				Location = new Point(controlX, 24),
				Size = new Size(80, 23),
				Minimum = 16,
				Maximum = 512,
				Value = 128,
			};
			grpBuffer.Controls.Add(new Label
			{
				Text = "KB",
				Location = new Point(controlX + 84, 28),
				AutoSize = true,
			});

			grpBuffer.Controls.Add(new Label
			{
				Text = "DSPバッファサイズ",
				Location = new Point(12, 28 + lineHeight),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});
			_nudDspBufferSize = new NumericUpDown
			{
				Location = new Point(controlX, 24 + lineHeight),
				Size = new Size(80, 23),
				Minimum = 512,
				Maximum = 4096,
				Increment = 512,
				Value = 2048,
			};
			grpBuffer.Controls.Add(new Label
			{
				Text = "サンプル",
				Location = new Point(controlX + 84, 28 + lineHeight),
				AutoSize = true,
			});

			grpBuffer.Controls.Add(new Label
			{
				Text = "DSPバッファ数",
				Location = new Point(12, 28 + lineHeight * 2),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});
			_nudDspBufferCount = new NumericUpDown
			{
				Location = new Point(controlX, 24 + lineHeight * 2),
				Size = new Size(80, 23),
				Minimum = 2,
				Maximum = 8,
				Value = 4,
			};
			grpBuffer.Controls.Add(new Label
			{
				Text = "個",
				Location = new Point(controlX + 84, 28 + lineHeight * 2),
				AutoSize = true,
			});

			grpBuffer.Controls.Add(new Label
			{
				Text = "※ 次回起動時に反映",
				Location = new Point(controlX, 28 + lineHeight * 3),
				AutoSize = true,
				ForeColor = System.Drawing.Color.Gray,
			});

			grpBuffer.Controls.AddRange(new Control[]
			{
				_nudStreamBuffer, _nudDspBufferSize, _nudDspBufferCount
			});

			y += grpBuffer.Height + 17;

			// ===========================
			// MIDIサウンドフォント
			// ===========================
			var grpSoundFont = new GroupBox
			{
				Text = "MIDIサウンドフォント（SF2）",
				Location = new Point(16, y),
				Size = new Size(520, 118),
			};
			grpSoundFont.Controls.Add(new Label
			{
				Text = "レンダラー",
				Location = new Point(12, 28),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});
			_cmbMidiRenderer = new ComboBox
			{
				Location = new Point(controlX, 24),
				Size = new Size(controlWidth, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			grpSoundFont.Controls.Add(_cmbMidiRenderer);
			grpSoundFont.Controls.Add(new Label
			{
				Text = "SF2ファイル",
				Location = new Point(12, 60),
				Size = new Size(labelWidth, 23),
				TextAlign = System.Drawing.ContentAlignment.MiddleRight,
			});
			_lblSoundFontNote = new Label
			{
				Location = new Point(controlX, 82),
				Size = new Size(controlWidth + 60, 30),
				ForeColor = System.Drawing.Color.Gray,
				Font = new Font("Yu Gothic UI", 8f),
			};
			grpSoundFont.Controls.Add(_lblSoundFontNote);
			_txtSoundFont = new TextBox
			{
				Location = new Point(controlX, 56),
				Size = new Size(controlWidth - 60, 23),
				ReadOnly = true,
			};

			_btnSoundFontBrowse = new Button
			{
				Text = "参照",
				Location = new Point(controlX + controlWidth - 58, 56),
				Size = new Size(50, 23),
			};
			_btnSoundFontBrowse.Click += BtnSoundFontBrowse_Click;

			grpSoundFont.Controls.AddRange(new Control[]
			{
				_txtSoundFont, _btnSoundFontBrowse
			});

			y += grpSoundFont.Height + 12;
			var _btnSoundFontClear = new Button
			{
				Text = "クリア",
				Location = new Point(controlX + controlWidth - 4, 56),
				Size = new Size(50, 23),
			};
			_btnSoundFontClear.Click += BtnSoundFontClear_Click;
			grpSoundFont.Controls.Add(_btnSoundFontClear);
			// ===========================
			// 保存ボタン
			// ===========================
			_btnSave = new Button
			{
				Text = "適用",
				Location = new Point(16, y),
				Size = OptionsStyle.SaveButtonSize,
				BackColor = OptionsStyle.PrimaryBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
			};
			_btnSave.Click += BtnSave_Click;

			Controls.AddRange(new Control[]
			{
				grpOutput, grpBuffer, grpSoundFont, _btnSave
			});
		}

		public override void LoadSettings()
		{
			_cmbOutput.SelectedIndex = Config.settings.OutputType;
			Engine.GetDeviceList();
			RefreshDeviceList(IndexToOutputType(Config.settings.OutputType),
				Config.settings.Device);

			_nudStreamBuffer.Value = Config.settings.Buffer.StreamBufferSizeKB;
			_nudDspBufferSize.Value = Config.settings.Buffer.DspBufferSize;
			_nudDspBufferCount.Value = Config.settings.Buffer.DspBufferCount;

			PopulateMidiRendererOptions();
			_txtSoundFont.Text = Config.settings.SoundFontPath ?? "";

			UpdateMidiRendererNote();
			Engine.SoundFontPath = Config.settings.SoundFontPath;
			Engine.MidiRendererBackend = Config.settings.MidiRendererBackend;
		}

		public override void SaveSettings()
		{
			bool requiresRestart =
				Config.settings.OutputType != _cmbOutput.SelectedIndex;

			if (_cmbDevice.Enabled && _cmbDevice.SelectedValue != null)
				Config.settings.Device = _cmbDevice.SelectedValue.ToString();

			Config.settings.OutputType = _cmbOutput.SelectedIndex;
			Config.settings.Buffer.StreamBufferSizeKB = (int)_nudStreamBuffer.Value;
			Config.settings.Buffer.DspBufferSize = (int)_nudDspBufferSize.Value;
			Config.settings.Buffer.DspBufferCount = (int)_nudDspBufferCount.Value;
			Config.settings.SoundFontPath = _txtSoundFont.Text;
			Config.settings.MidiRendererBackend = GetSelectedMidiRendererBackend();

			// ★エンジンに即時反映
			Engine.SoundFontPath = _txtSoundFont.Text;
			Engine.MidiRendererBackend = Config.settings.MidiRendererBackend;

			Config.Save();

			string message = requiresRestart
				? "設定を保存しました。\n出力形式・バッファサイズは次回起動時に反映されます。"
				: "設定を保存しました。\nデバイスは次回再生時に反映されます。";

			MessageBox.Show(message, "設定保存",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void BtnSave_Click(object sender, EventArgs e) => SaveSettings();

		private void BtnSoundFontBrowse_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = (Engine.FluidSynthAvailable || Engine.BassMidiAvailable)
					? "サウンドフォント|*.sf2;*.dls|SF2|*.sf2|DLS|*.dls|すべて|*.*"
					: "DLSサウンドフォント|*.dls|すべて|*.*";

				dlg.InitialDirectory =
					Path.GetDirectoryName(_txtSoundFont.Text) is string d
					&& Directory.Exists(d)
					? d
					: System.Windows.Forms.Application.StartupPath;

				if (dlg.ShowDialog() == DialogResult.OK)
					_txtSoundFont.Text = dlg.FileName;
			}
		}

		private void CmbOutput_SelectedIndexChanged(object sender, EventArgs e)
		{
			var newType = IndexToOutputType(_cmbOutput.SelectedIndex);
			var currentType = Engine.GetOutputType();

			if (currentType == FMOD.OUTPUTTYPE.ASIO && newType != FMOD.OUTPUTTYPE.ASIO)
			{
				_lblOutputNote.Text = "※ ASIO起動中のため次回起動時に反映されます。";
			}
			else
			{
				_lblOutputNote.Text = "";
			}

			string preferredGuid = (_cmbOutput.SelectedIndex == Config.settings.OutputType)
				? Config.settings.Device : null;

			RefreshDeviceList(newType, preferredGuid);
		}

		private void RefreshDeviceList(FMOD.OUTPUTTYPE outputType, string preferredGuid)
		{
			_cmbDevice.DataSource = null;

			List<DEVICE_INFO> devices;
			if (Engine.GetOutputType() == FMOD.OUTPUTTYPE.ASIO &&
				outputType == FMOD.OUTPUTTYPE.ASIO)
				devices = Engine.GetCurrentDeviceList();
			else
				devices = Engine.GetDeviceListForOutputType(outputType);

			if (devices.Count == 0)
			{
				_cmbDevice.Enabled = false;
				_cmbDevice.Items.Clear();
				_cmbDevice.Items.Add("（デバイスなし）");
				_cmbDevice.SelectedIndex = 0;
				return;
			}

			_cmbDevice.Enabled = true;
			_cmbDevice.DataSource = devices;
			_cmbDevice.DisplayMember = "Name";
			_cmbDevice.ValueMember = "GUID";

			bool found = preferredGuid != null
				&& devices.Any(d => d.GUID == preferredGuid);

			if (found) _cmbDevice.SelectedValue = preferredGuid;
			else _cmbDevice.SelectedIndex = 0;
		}

		private FMOD.OUTPUTTYPE IndexToOutputType(int index)
		{
			switch (index)
			{
				case 1: return FMOD.OUTPUTTYPE.WASAPI;
				case 2: return FMOD.OUTPUTTYPE.ASIO;
				case 3: return FMOD.OUTPUTTYPE.WINSONIC;
				default: return FMOD.OUTPUTTYPE.AUTODETECT;
			}
		}
		private void BtnSoundFontClear_Click(object sender, EventArgs e)
		{
			_txtSoundFont.Text = "";
		}

		private void PopulateMidiRendererOptions()
		{
			var items = new List<KeyValuePair<string, MidiRendererBackend>>
			{
				new KeyValuePair<string, MidiRendererBackend>("自動選択", MidiRendererBackend.Auto),
			};

			if (Engine.FluidSynthAvailable)
				items.Add(new KeyValuePair<string, MidiRendererBackend>("FluidSynth", MidiRendererBackend.FluidSynth));

			if (Engine.BassMidiAvailable)
				items.Add(new KeyValuePair<string, MidiRendererBackend>("BASSMIDI", MidiRendererBackend.BassMidi));

			_cmbMidiRenderer.DataSource = items;
			_cmbMidiRenderer.DisplayMember = "Key";
			_cmbMidiRenderer.ValueMember = "Value";

			var requested = Config.settings.MidiRendererBackend;
			bool hasRequested = items.Any(x => x.Value == requested);
			_cmbMidiRenderer.SelectedValue = hasRequested ? requested : MidiRendererBackend.Auto;
			_cmbMidiRenderer.Enabled = items.Count > 1;
		}

		private MidiRendererBackend GetSelectedMidiRendererBackend()
		{
			if (_cmbMidiRenderer.SelectedValue is MidiRendererBackend selected)
				return selected;
			if (_cmbMidiRenderer.SelectedValue is int selectedInt)
				return (MidiRendererBackend)selectedInt;
			return MidiRendererBackend.Auto;
		}

		private void UpdateMidiRendererNote()
		{
			if (Engine.FluidSynthAvailable && Engine.BassMidiAvailable)
			{
				_lblSoundFontNote.Text =
					"✓ fluidsynth.dll / bass.dll / bassmidi.dll を検出しました。レンダラーを選択できます。";
				_lblSoundFontNote.ForeColor = System.Drawing.Color.Green;
				return;
			}

			if (Engine.BassMidiAvailable)
			{
				_lblSoundFontNote.Text =
					"✓ bass.dll / bassmidi.dll を検出しました。BASSMIDI が使用できます。";
				_lblSoundFontNote.ForeColor = System.Drawing.Color.Green;
				return;
			}

			if (Engine.FluidSynthAvailable)
			{
				_lblSoundFontNote.Text =
					"✓ fluidsynth.dll が検出されました。FluidSynth が使用できます。";
				_lblSoundFontNote.ForeColor = System.Drawing.Color.Green;
				return;
			}

			_lblSoundFontNote.Text =
				"※ Libs に fluidsynth.dll または bass.dll + bassmidi.dll を配置すると選択できます。";
			_lblSoundFontNote.ForeColor = System.Drawing.Color.Gray;
		}
	}
}
