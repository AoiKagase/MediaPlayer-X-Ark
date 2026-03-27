using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms.Options
{
    public class PluginsControl : OptionsControlBase
    {
        private DataGridView _grid;
        private Button _btnReload;

        public PluginsControl(IPlayerEngine engine, IConfigService config)
            : base(engine, config)
        {
            BuildLayout();
        }

        public override void LoadSettings()
        {
            RefreshGrid();
        }

        public override void SaveSettings() { }

        private void BuildLayout()
        {
            var lblSection = new Label
            {
                Text = "ロード済みプラグイン",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
            };
            var pnlLine = new Panel
            {
                Location = new Point(0, 20),
                Size = new Size(560, 1),
                BackColor = Color.Gray,
            };

            _grid = new DataGridView
            {
                Location = new Point(0, 28),
                Size = new Size(560, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            };

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFile",
                HeaderText = "ファイル名",
                FillWeight = 30,
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "プラグイン名",
                FillWeight = 30,
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colType",
                HeaderText = "種別",
                FillWeight = 15,
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colVersion",
                HeaderText = "バージョン",
                FillWeight = 15,
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "状態",
                FillWeight = 10,
            });

            _btnReload = new Button
            {
                Text = "再スキャン",
                Location = new Point(0, 356),
                Size = new Size(88, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
            };
            _btnReload.Click += (s, e) =>
            {
                Engine.LoadPlugins();
                RefreshGrid();
            };

            Controls.AddRange(new Control[]
            {
                lblSection, pnlLine, _grid, _btnReload,
            });
        }

        private void RefreshGrid()
        {
            _grid.Rows.Clear();
            foreach (var p in Engine.LoadedPlugins)
            {
                _grid.Rows.Add(
                    p.FileName,
                    p.Success ? p.PluginName : "―",
                    p.Success ? p.TypeLabel : "―",
                    p.Success ? p.VersionLabel : "―",
                    p.Success ? "OK" : "FAIL");

                // FAIL行を赤背景に
                if (!p.Success)
                {
                    var row = _grid.Rows[_grid.Rows.Count - 1];
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                }
            }
        }
    }
}