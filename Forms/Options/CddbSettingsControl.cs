using MediaPlayer_X_Ark.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
	/// <summary>
	/// CDDB サーバー設定コントロール。
	/// OptionsForm の「CDDB」タブに組み込む。
	/// サーバーは上から順に試みられ、最初に結果が返ったものを使用する。
	/// </summary>
	public class CddbSettingsControl : OptionsControlBase
	{
		// ─────────────────────────────────────────
		//  コントロール
		// ─────────────────────────────────────────

		private ListBox _lstServers;
		private TextBox _txtUrl;
		private Button _btnAdd;
		private Button _btnRemove;
		private Button _btnUp;
		private Button _btnDown;
		private Button _btnReset;
		private Button _btnSave;
		private Label _lblDesc;
		private Label _lblUrl;
		private Label _lblCount;
		private Label _lblNote;

		private const int MaxServers = 20;

		// ─────────────────────────────────────────
		//  コンストラクタ
		// ─────────────────────────────────────────

		public CddbSettingsControl(IPlayerEngine engine, IConfigService config)
			: base(engine, config)
		{
			BuildLayout();
		}

		// ─────────────────────────────────────────
		//  OptionsControlBase 実装
		// ─────────────────────────────────────────

		public override void LoadSettings()
		{
			_lstServers.Items.Clear();
			foreach (var url in Config.settings.CddbServers)
				_lstServers.Items.Add(url);
			UpdateButtonStates();
		}

		public override void SaveSettings()
		{
			Config.settings.CddbServers = _lstServers.Items
				.Cast<string>()
				.ToList();
			Config.Save();
		}

		// ─────────────────────────────────────────
		//  レイアウト構築
		// ─────────────────────────────────────────

		private void BuildLayout()
		{
			const int lineH = 28;

			// ── 説明ラベル ────────────────────────────────────────
			_lblDesc = new Label
			{
				Text = "CDDBサーバーを上から順に試み、最初に結果が返ったものを使用します。\n" +
						   "上へ移動することで優先順位を上げられます。",
				Location = new Point(0, 0),
				Size = new Size(500, 36),
				AutoSize = false,
			};

			// ── サーバーリスト ────────────────────────────────────
			_lstServers = new ListBox
			{
				Location = new Point(0, 44),
				Size = new Size(380, 160),
				SelectionMode = SelectionMode.One,
			};
			_lstServers.SelectedIndexChanged += (s, e) => UpdateButtonStates();

			// ── 上下移動ボタン ────────────────────────────────────
			_btnUp = new Button
			{
				Text = "▲ 上へ",
				Location = new Point(388, 44),
				Size = new Size(80, lineH),
				Enabled = false,
			};
			_btnUp.Click += BtnUp_Click;

			_btnDown = new Button
			{
				Text = "▼ 下へ",
				Location = new Point(388, 44 + lineH + 4),
				Size = new Size(80, lineH),
				Enabled = false,
			};
			_btnDown.Click += BtnDown_Click;

			_btnRemove = new Button
			{
				Text = "削除",
				Location = new Point(388, 44 + (lineH + 4) * 2),
				Size = new Size(80, lineH),
				Enabled = false,
			};
			_btnRemove.Click += BtnRemove_Click;

			// ── 件数表示 ──────────────────────────────────────────
			_lblCount = new Label
			{
				Text = $"0 / {MaxServers} 件",
				Location = new Point(0, 208),
				Size = new Size(30, 22),
				AutoSize = true,
				TextAlign = ContentAlignment.MiddleLeft,
			};

			// ── URL 入力行 ────────────────────────────────────────
			_lblUrl = new Label
			{
				Text = "URL:",
				Location = new Point(0, 240),
				AutoSize = true,
			};

			_txtUrl = new TextBox
			{
				Location = new Point(36, 238),
				Size = new Size(340, 22),
				PlaceholderText = "http://freedbtest.dyndns.org/~cddb/cddb.cgi",
			};
			_txtUrl.TextChanged += (s, e) =>
				_btnAdd.Enabled = !string.IsNullOrWhiteSpace(_txtUrl.Text)
								  && _lstServers.Items.Count < MaxServers;

			_btnAdd = new Button
			{
				Text = "追加",
				Location = new Point(388, 236),
				Size = new Size(80, lineH),
				Enabled = false,
			};
			_btnAdd.Click += BtnAdd_Click;

			// ── 注釈 ─────────────────────────────────────────────
			_lblNote = new Label
			{
				Text = "※ MusicBrainz は常にフォールバックとして使用されます（リストに表示されません）",
				Location = new Point(0, 272),
				Size = new Size(480, 20),
				AutoSize = false,
				ForeColor = Color.Gray,
				Font = new Font(Font.FontFamily, Font.Size - 0.5f),
			};

			// ── リセット／保存ボタン ──────────────────────────────
			_btnReset = new Button
			{
				Text = "デフォルトに戻す",
				Location = new Point(0, 300),
				Size = new Size(140, lineH),
			};
			_btnReset.Click += BtnReset_Click;

			_btnSave = new Button
			{
				Text = "保存",
				Location = new Point(388, 300),
				Size = new Size(80, lineH),
				BackColor = Color.FromArgb(0, 120, 215),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
			};
			_btnSave.Click += (s, e) => SaveSettings();

			Controls.AddRange(new Control[]
			{
				_lblDesc,
				_lstServers,
				_btnUp, _btnDown, _btnRemove,
				_lblCount,
				_lblUrl, _txtUrl, _btnAdd,
				_lblNote,
				_btnReset, _btnSave,
			});
		}

		// ─────────────────────────────────────────
		//  ボタンイベント
		// ─────────────────────────────────────────

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			string url = _txtUrl.Text.Trim();
			if (string.IsNullOrEmpty(url)) return;

			if (_lstServers.Items.Count >= MaxServers)
			{
				MessageBox.Show($"サーバーは最大 {MaxServers} 件まで登録できます。", "MediaPlayer X Ark",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// 重複チェック
			if (_lstServers.Items.Cast<string>().Any(u =>
					string.Equals(u, url, StringComparison.OrdinalIgnoreCase)))
			{
				MessageBox.Show("同じURLが既に登録されています。", "MediaPlayer X Ark",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			_lstServers.Items.Add(url);
			_txtUrl.Clear();
			_txtUrl.Focus();
			UpdateButtonStates();
		}

		private void BtnRemove_Click(object sender, EventArgs e)
		{
			int idx = _lstServers.SelectedIndex;
			if (idx < 0) return;

			_lstServers.Items.RemoveAt(idx);

			if (_lstServers.Items.Count > 0)
				_lstServers.SelectedIndex = Math.Min(idx, _lstServers.Items.Count - 1);

			UpdateButtonStates();
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			int idx = _lstServers.SelectedIndex;
			if (idx <= 0) return;
			SwapItems(idx, idx - 1);
			_lstServers.SelectedIndex = idx - 1;
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			int idx = _lstServers.SelectedIndex;
			if (idx < 0 || idx >= _lstServers.Items.Count - 1) return;
			SwapItems(idx, idx + 1);
			_lstServers.SelectedIndex = idx + 1;
		}

		private void BtnReset_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(
					"サーバーリストをデフォルトに戻しますか？",
					"確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			Config.settings.CddbServers = ConfigurationData.DefaultCddbServers();
			LoadSettings();
		}

		// ─────────────────────────────────────────
		//  ユーティリティ
		// ─────────────────────────────────────────

		private void SwapItems(int a, int b)
		{
			var tmp = _lstServers.Items[a];
			_lstServers.Items[a] = _lstServers.Items[b];
			_lstServers.Items[b] = tmp;
		}

		private void UpdateButtonStates()
		{
			int idx = _lstServers.SelectedIndex;
			int count = _lstServers.Items.Count;

			_lblCount.Text = $"{count} / {MaxServers} 件";
			_lblCount.ForeColor = count >= MaxServers ? Color.Red : SystemColors.WindowText;

			_btnRemove.Enabled = idx >= 0;
			_btnUp.Enabled = idx > 0;
			_btnDown.Enabled = idx >= 0 && idx < count - 1;

			// 上限に達したら追加ボタンを無効化
			_btnAdd.Enabled = !string.IsNullOrWhiteSpace(_txtUrl.Text)
							  && count < MaxServers;
		}
	}
}
