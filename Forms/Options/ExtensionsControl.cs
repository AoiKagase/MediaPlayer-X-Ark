using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
    /// <summary>
    /// ファイル関連付け設定コントロール。
    /// OptionsForm の「Extensions」タブに組み込む。
    ///
    /// 対応形式は SupportedFormats.GetAll() から動的に取得する。
    /// LoadPlugins() 後に LoadSettings() が呼ばれると codec 形式も含まれた
    /// リストになる（OptionsForm がタブ切り替えで呼ぶ既存の仕組みに乗る）。
    /// </summary>
    public class ExtensionsControl : OptionsControlBase
    {
        // ─────────────────────────────────────────
        //  コントロール
        // ─────────────────────────────────────────

        private ListView _lvExtensions;
        private Label    _lblStatus;
        private Label    _lblAdminStatus;
        private Label    _lblFluidSynthStatus;
        private Button   _btnSelectAll;
        private Button   _btnDeselectAll;
        private Button   _btnRegister;
        private Button   _btnUnregister;
        private Button   _btnUnregisterAll;
        private Button   _btnAdminRestart;
        private Button   _btnRefresh;

        // ─────────────────────────────────────────
        //  コンストラクタ
        // ─────────────────────────────────────────

        public ExtensionsControl(IPlayerEngine engine, IConfigService config)
            : base(engine, config)
        {
            BuildLayout();
            PopulateListView();
        }

        // ─────────────────────────────────────────
        //  OptionsControlBase 実装
        // ─────────────────────────────────────────

        public override void LoadSettings()
        {
            // タブ表示のたびにリストを再構築してレジストリ状態を反映する
            PopulateListView();
            RefreshStatus();
        }

        public override void SaveSettings()
        {
            // 登録・解除はボタン操作で即時反映済みのため何もしない
        }

        // ─────────────────────────────────────────
        //  レイアウト構築
        // ─────────────────────────────────────────

        private void BuildLayout()
        {
            // ── 上部パネル（説明 + 一括選択ボタン）──────────────────
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 80 };

            var lblDesc = new Label
            {
                Text     = "このアプリをデフォルト プレイヤーとして登録するファイル形式にチェックを入れ、\n「選択した形式を登録」をクリックしてください。",
                Location = new Point(0, 0),
                Size     = new Size(560, 40),
                AutoSize = false,
            };

            _btnSelectAll = new Button
            {
                Text = "全て選択", Location = new Point(0, 46), Size = new Size(88, 26),
            };
            _btnSelectAll.Click += (s, e) =>
            {
                foreach (ListViewItem item in _lvExtensions.Items) item.Checked = true;
            };

            _btnDeselectAll = new Button
            {
                Text = "全て解除", Location = new Point(92, 46), Size = new Size(88, 26),
            };
            _btnDeselectAll.Click += (s, e) =>
            {
                foreach (ListViewItem item in _lvExtensions.Items) item.Checked = false;
            };

            _btnRefresh = new Button
            {
                Text = "↻ 状態を更新", Location = new Point(184, 46), Size = new Size(104, 26),
            };
            _btnRefresh.Click += (s, e) => RefreshStatus();

            panelTop.Controls.AddRange(new Control[] { lblDesc, _btnSelectAll, _btnDeselectAll, _btnRefresh });

            // ── 下部パネル（登録ボタン + ステータス）─────────────────
            var panelBottom = new Panel { Dock = DockStyle.Bottom, Height = 62 };

            _btnRegister = new Button
            {
                Text      = "✔ 選択した形式を登録",
                Location  = new Point(0, 4),
                Size      = new Size(160, 28),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            _btnRegister.Click += BtnRegister_Click;

            _btnUnregister = new Button
            {
                Text = "✖ 選択した形式を解除", Location = new Point(164, 4), Size = new Size(160, 28),
            };
            _btnUnregister.Click += BtnUnregister_Click;

            _btnUnregisterAll = new Button
            {
                Text = "全て解除", Location = new Point(328, 4), Size = new Size(80, 28),
            };
            _btnUnregisterAll.Click += BtnUnregisterAll_Click;

            _btnAdminRestart = new Button
            {
                Text = "🔑 管理者で再起動", Location = new Point(412, 4), Size = new Size(148, 28),
            };
            _btnAdminRestart.Click += BtnAdminRestart_Click;

            _lblStatus = new Label
            {
                Text = "", Location = new Point(0, 40), AutoSize = true,
            };
            _lblAdminStatus = new Label
            {
                Text = "", Location = new Point(220, 40), AutoSize = true, ForeColor = Color.DarkOrange,
            };
            _lblFluidSynthStatus = new Label
            {
                Text      = "",
                Location  = new Point(0, 60),
                AutoSize  = true,
                ForeColor = Color.Gray,
                Font      = new System.Drawing.Font(Font.FontFamily, Font.Size - 0.5f),
            };

            panelBottom.Height = 88;   // FluidSynth 通知行ぶん拡張
            panelBottom.Controls.AddRange(new Control[]
            {
                _btnRegister, _btnUnregister, _btnUnregisterAll, _btnAdminRestart,
                _lblStatus, _lblAdminStatus, _lblFluidSynthStatus,
            });

            // ── ListView（中央・Fill）────────────────────────────────
            _lvExtensions = new ListView
            {
                Dock          = DockStyle.Fill,
                View          = View.Details,
                CheckBoxes    = true,
                FullRowSelect = true,
                GridLines     = true,
                ShowGroups    = true,
            };
            _lvExtensions.Columns.Add("拡張子",  80);
            _lvExtensions.Columns.Add("説明",    260);
            _lvExtensions.Columns.Add("デコーダー", 90);  // FMOD / FluidSynth / App / codec_*.dll
            _lvExtensions.Columns.Add("状態",    90);

            // Fill は最後に追加
            Controls.Add(_lvExtensions);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
        }

        // ─────────────────────────────────────────
        //  ListView データ構築
        // ─────────────────────────────────────────

        private void PopulateListView()
        {
            _lvExtensions.BeginUpdate();
            _lvExtensions.Groups.Clear();
            _lvExtensions.Items.Clear();

            // FluidSynth 導入済みなら MIDI のデコーダー列を "FluidSynth" で上書き表示する
            bool fluidSynthAvailable = Engine.FluidSynthAvailable;
            var allFormats = SupportedFormats.GetAll().ToList();

            foreach (var groupName in allFormats.Select(f => f.Group).Distinct())
            {
                var lvGroup = new ListViewGroup(groupName, groupName);
                _lvExtensions.Groups.Add(lvGroup);

                foreach (var fmt in allFormats.Where(f => f.Group == groupName))
                {
                    // MIDI グループかつ FluidSynth 導入済みの場合はデコーダー表示を切り替える
                    string decoderLabel = (fmt.Group == "MIDI" && fluidSynthAvailable)
                        ? "FluidSynth"
                        : fmt.Source;

                    var item = new ListViewItem(new[] { fmt.Ext, fmt.Description, decoderLabel, "" })
                    {
                        Tag   = fmt.Ext,
                        Group = lvGroup,
                    };
                    _lvExtensions.Items.Add(item);
                }
            }

            _lvExtensions.EndUpdate();
        }

        // ─────────────────────────────────────────
        //  状態更新（レジストリ → ListView）
        // ─────────────────────────────────────────

        private void RefreshStatus()
        {
            _lvExtensions.BeginUpdate();
            foreach (ListViewItem item in _lvExtensions.Items)
            {
                bool reg = FileAssociationManager.IsRegistered((string)item.Tag);
                item.Checked          = reg;
                item.SubItems[3].Text = reg ? "✔ 登録済み" : "─";
                item.ForeColor        = reg ? Color.DarkGreen : SystemColors.WindowText;
            }
            _lvExtensions.EndUpdate();

            int total      = _lvExtensions.Items.Count;
            int registered = _lvExtensions.Items.Cast<ListViewItem>()
                .Count(i => FileAssociationManager.IsRegistered((string)i.Tag));

            _lblStatus.Text = $"登録済み: {registered} / {total} 形式";

            bool isAdmin = FileAssociationManager.IsRunningAsAdmin();
            _lblAdminStatus.Text      = isAdmin ? "🔑 管理者権限あり" : "⚠ 通常権限（ユーザー別登録）";
            _lblAdminStatus.ForeColor = isAdmin ? Color.DarkGreen : Color.DarkOrange;

            if (!Engine.FluidSynthAvailable)
            {
                _lblFluidSynthStatus.Text      = "※ fluidsynth.dll 未導入：MIDI は FMOD のデフォルト デコーダーで再生されます";
                _lblFluidSynthStatus.ForeColor = Color.Gray;
            }
            else
            {
                _lblFluidSynthStatus.Text      = "✔ FluidSynth 導入済み：MIDI は FluidSynth でデコードされます";
                _lblFluidSynthStatus.ForeColor = Color.DarkGreen;
            }
        }

        // ─────────────────────────────────────────
        //  ボタンイベント
        // ─────────────────────────────────────────

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var selected = GetCheckedExtensions();
            if (selected.Count == 0)
            {
                MessageBox.Show("登録する形式を選択してください。", "MediaPlayer X Ark",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                FileAssociationManager.RegisterExtensions(selected, Application.ExecutablePath);
                RefreshStatus();
                MessageBox.Show($"{selected.Count} 種類のファイル形式を登録しました。",
                    "MediaPlayer X Ark", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登録に失敗しました:\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnUnregister_Click(object sender, EventArgs e)
        {
            var selected = GetCheckedExtensions();
            if (selected.Count == 0)
            {
                MessageBox.Show("解除する形式を選択してください。", "MediaPlayer X Ark",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show($"{selected.Count} 種類のファイル関連付けを解除しますか？",
                    "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                Cursor = Cursors.WaitCursor;
                FileAssociationManager.UnregisterExtensions(selected);
                RefreshStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解除に失敗しました:\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnUnregisterAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての形式の関連付けを解除しますか？",
                    "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                Cursor = Cursors.WaitCursor;
                FileAssociationManager.UnregisterAll();
                RefreshStatus();
            }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnAdminRestart_Click(object sender, EventArgs e)
        {
            if (FileAssociationManager.IsRunningAsAdmin())
            {
                MessageBox.Show("既に管理者権限で実行中です。", "MediaPlayer X Ark",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show(
                    "管理者権限で再起動しますか？\n（システム全体への関連付けが可能になります）",
                    "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if (FileAssociationManager.RestartAsAdmin())
                Application.Exit();
        }

        // ─────────────────────────────────────────
        //  ユーティリティ
        // ─────────────────────────────────────────

        private List<string> GetCheckedExtensions()
            => _lvExtensions.Items.Cast<ListViewItem>()
                .Where(i => i.Checked)
                .Select(i => (string)i.Tag)
                .ToList();
    }
}
