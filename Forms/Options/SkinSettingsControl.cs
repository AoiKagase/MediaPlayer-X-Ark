using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	public class SkinSettingsControl : OptionsControlBase
	{
		private TextBox _txtSkinPath;
		private PictureBox _pictPreview;
		private Label _lblName;
		private Label _lblAuthor;
		private Label _lblDesc;
		private Button _btnBrowse;
		private Button _btnApply;

		private MainForm _mainForm;

		public SkinSettingsControl(IPlayerEngine engine, IConfigService config, MainForm mainForm)
			: base(engine, config)
		{
			_mainForm = mainForm;
			BuildLayout();
		}

		private void BuildLayout()
		{
			const int pad = 16;

			// スキンファイル: ラベル + テキスト + 参照ボタン
			var lblPath = new Label
			{
				Text = "スキンファイル:",
				Location = new Point(pad, 16),
				AutoSize = true,
			};

			_txtSkinPath = new TextBox
			{
				Location = new Point(pad, 36),
				Size = new Size(460, 23),
				ReadOnly = true,
			};

			_btnBrowse = new Button
			{
				Text = "参照...",
				Location = new Point(pad + 464, 36),
				Size = new Size(60, 23),
			};
			_btnBrowse.Click += BtnBrowse_Click;

			// プレビュー画像（大きめ・横幅いっぱい）
			_pictPreview = new PictureBox
			{
				Location = new Point(pad, 68),
				Size = new Size(524, 200),
				BorderStyle = BorderStyle.FixedSingle,
				SizeMode = PictureBoxSizeMode.Zoom,
				BackColor = System.Drawing.Color.Black,
			};

			// スキン名（大きめフォント）
			_lblName = new Label
			{
				Location = new Point(pad, 280),
				Size = new Size(524, 28),
				Font = new Font("Yu Gothic UI", 12f, FontStyle.Bold),
			};

			// Author
			var lblAuthorLabel = new Label
			{
				Text = "Author:",
				Location = new Point(pad, 314),
				Size = new Size(80, 20),
			};
			_lblAuthor = new Label
			{
				Location = new Point(pad + 84, 314),
				Size = new Size(440, 20),
			};

			// Description
			var lblDescLabel = new Label
			{
				Text = "Description:",
				Location = new Point(pad, 338),
				Size = new Size(80, 20),
			};
			_lblDesc = new Label
			{
				Location = new Point(pad + 84, 338),
				Size = new Size(440, 40),
			};

			// 適用ボタン（右下）
			_btnApply = new Button
			{
				Text = "適用",
				Location = new Point(pad + 449, 390),
				Size = new Size(75, 23),
			};
			_btnApply.Click += BtnApply_Click;

			Controls.AddRange(new Control[]
			{
				lblPath, _txtSkinPath, _btnBrowse,
				_pictPreview,
				_lblName,
				lblAuthorLabel, _lblAuthor,
				lblDescLabel, _lblDesc,
				_btnApply,
			});
		}

		public override void LoadSettings()
		{
			var skinPath = Config.settings.Skin;
			_txtSkinPath.Text = skinPath ?? "";
			LoadSkinPreview(skinPath);
		}

		public override void SaveSettings() { }

		private void BtnBrowse_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = "スキンファイル|*.xsk;*.xsf|新形式 (*.xsk)|*.xsk|旧形式 (*.xsf)|*.xsf|すべて|*.*";
				dlg.InitialDirectory = Path.Combine(
					System.Windows.Forms.Application.StartupPath, "Skins");

				if (dlg.ShowDialog() != DialogResult.OK) return;

				_txtSkinPath.Text = dlg.FileName;
				LoadSkinPreview(dlg.FileName);
			}
		}

		private async void BtnApply_Click(object sender, EventArgs e)
		{
			var skinPath = _txtSkinPath.Text;
			if (string.IsNullOrEmpty(skinPath)) return;

			_btnApply.Enabled = false;
			_btnBrowse.Enabled = false;

			try
			{
				await Task.Run(() =>
				{
					using (var pkg = SkinPackage.Open(skinPath)) { }
				});

				Config.settings.Skin = skinPath;
				Config.Save();
				_mainForm.SkinLoad(skinPath);

				MessageBox.Show("スキンを適用しました。", "完了",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"スキンの適用に失敗しました。\n{ex.Message}", "エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				_btnApply.Enabled = true;
				_btnBrowse.Enabled = true;
			}
		}

		private async void LoadSkinPreview(string skinPath)
		{
			_lblName.Text = "";
			_lblAuthor.Text = "";
			_lblDesc.Text = "";
			_pictPreview.Image = null;

			if (string.IsNullOrEmpty(skinPath)) return;

			try
			{
				Image previewImage = null;
				string name = "", author = "", desc = "";

				await Task.Run(() =>
				{
					using (var pkg = SkinPackage.Open(skinPath))
					{
						if (pkg.MainImagePath != null && File.Exists(pkg.MainImagePath))
						{
							using (var stream = new FileStream(
								pkg.MainImagePath, FileMode.Open, FileAccess.Read))
							{
								var ms = new System.IO.MemoryStream();
								stream.CopyTo(ms);
								ms.Seek(0, System.IO.SeekOrigin.Begin);
								previewImage = new Bitmap(ms);
							}
						}

						if (pkg.Format == SkinPackage.SkinFormat.NewXsk &&
							pkg.DefinitionPath != null &&
							File.Exists(pkg.DefinitionPath))
						{
							var json = File.ReadAllText(pkg.DefinitionPath,
								System.Text.Encoding.UTF8);
							var skin = System.Text.Json.JsonSerializer
								.Deserialize<NewSkinSystem.SkinJson>(json);
							name = skin?.Meta?.Name ?? "";
							author = skin?.Meta?.Author ?? "";
							desc = skin?.Meta?.Description ?? "";
						}
						else
						{
							name = Path.GetFileNameWithoutExtension(skinPath);
						}
					}
				});

				_pictPreview.Image = previewImage;
				_lblName.Text = name;
				_lblAuthor.Text = author;
				_lblDesc.Text = desc;
			}
			catch
			{
				_pictPreview.Image = null;
			}
		}
	}
}