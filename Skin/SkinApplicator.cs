using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin.New;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Skin
{
    /// <summary>
    /// スキンデータをフォームコントロールに適用するクラス。
    /// MainForm から ApplySkin / GetButtonMap ロジックを抽出。
    /// </summary>
    public class SkinApplicator
    {
        private readonly INewSkinSystem _skin;

        public SkinApplicator(INewSkinSystem skin)
        {
            _skin = skin;
        }

        /// <summary>メインフォームにスキンを適用する</summary>
        public void ApplyToMainForm(Form form, Controls.SpectrumAnalyzer spectrum)
        {
            form.BackgroundImage = _skin.MainForm.BackImage;
            form.TransparencyKey = _skin.MainForm.TransparentKey;
            form.Width           = _skin.MainForm.Position.Width;
            form.Height          = _skin.MainForm.Position.Height;

            // スペクトラム配置
            form.SuspendLayout();
            spectrum.Left   = _skin.Spectrum.Position.Left;
            spectrum.Top    = _skin.Spectrum.Position.Top;
            spectrum.Width  = _skin.Spectrum.Position.Width;
            spectrum.Height = _skin.Spectrum.Position.Height;
            form.ResumeLayout(false);

            // スペクトラムビットマップ再生成
            int sw = _skin.Spectrum.Position.Width;
            int sh = _skin.Spectrum.Position.Height;
            spectrum.BitmapSnow       = new Bitmap(sw, sh);
            spectrum.BitmapWave       = new Bitmap(sw, sh);
            spectrum.BitmapBackground = new Bitmap(sw, sh);

            if (_skin.Spectrum.Image != null)
            {
                spectrum.BitmapSpectrum = new Bitmap(_skin.Spectrum.Image);
            }
            else
            {
                spectrum.BitmapSpectrum = new Bitmap(sw, sh);
                using (var g = Graphics.FromImage(spectrum.BitmapSpectrum))
                    g.Clear(_skin.Spectrum.Color);
                using (var g = Graphics.FromImage(spectrum.BitmapSnow))
                    g.Clear(_skin.Spectrum.Color);
                using (var g = Graphics.FromImage(spectrum.BitmapWave))
                    g.Clear(_skin.Spectrum.Color);
            }

            // 背景からスペクトラム領域を切り出し
            if (_skin.MainForm.BackImage != null)
            {
                var rect = new Rectangle(
                    _skin.Spectrum.Position.Left,
                    _skin.Spectrum.Position.Top,
                    sw, sh);
                var bmp = new Bitmap(sw, sh);
                using (var g = Graphics.FromImage(bmp))
                    g.DrawImage(_skin.MainForm.BackImage,
                        new Rectangle(0, 0, sw, sh), rect, GraphicsUnit.Pixel);
                spectrum.BitmapBackground = bmp;
            }
            else
            {
                spectrum.BitmapBackground = null;
            }

            // ボタン・スライダー・ラベルを適用
            ApplyControls(form.Controls);
            form.Refresh();
        }

        /// <summary>プレイリストフォームにスキンを適用する</summary>
        public void ApplyToPlayListForm(Form playListForm)
        {
            playListForm.BackgroundImage = _skin.SubForms["PlayListForm"].BackImage;
            playListForm.Width           = _skin.SubForms["PlayListForm"].Position.Width;
            playListForm.Height          = _skin.SubForms["PlayListForm"].Position.Height;
            playListForm.TransparencyKey = _skin.SubForms["PlayListForm"].TransparentKey;
            playListForm.Refresh();

            ApplyControls(playListForm.Controls);
        }

        /// <summary>プレイリストフォームの位置をマグネットモードに合わせて更新する</summary>
        public void UpdatePlayListPosition(Form mainForm, Form playListForm)
        {
            playListForm.Left = mainForm.Left - _skin.SubForms["PlayListForm"].Position.Left;
            playListForm.Top  = mainForm.Top  - _skin.SubForms["PlayListForm"].Position.Top;
        }

        /// <summary>ボタンの押下画像をセットする</summary>
        public void SetButtonDown(Button btn)
        {
            var parent = btn.Parent.Name;
            var btnMap = _skin.Buttons[parent];
            if (btnMap.TryGetValue(btn.Name, out var bc))
            {
                btn.BackgroundImage = bc.DownImage;
                btn.Refresh();
            }
        }

        /// <summary>ボタンの通常画像をセットする</summary>
        public void SetButtonUp(Button btn)
        {
            var parent = btn.Parent.Name;
            var btnMap = _skin.Buttons[parent];
            if (btnMap.TryGetValue(btn.Name, out var bc))
            {
                btn.BackgroundImage = bc.BackImage;
                btn.Refresh();
            }
        }

        /// <summary>ループボタンの状態画像をセットする</summary>
        public void UpdateLoopButton(Button btn, LOOP_MODE loop)
        {
            var loopOnly = loop & ~LOOP_MODE.LOOP_RANDOM;
            btn.BackgroundImage = loopOnly switch
            {
                LOOP_MODE.LOOP_ONE_REPEAT => _skin.Buttons["MainForm"]["BtnLoop"].DownImage,
                LOOP_MODE.LOOP_ALL        => _skin.Buttons["MainForm"]["BtnLoop"].OptionalImage,
				_                         => _skin.Buttons["MainForm"]["BtnLoop"].BackImage,

			};
            btn.Refresh();
        }

        /// <summary>ランダムボタンの状態画像をセットする</summary>
        public void UpdateRandomButton(Button btn, LOOP_MODE loop)
        {
            bool isRandom = (loop & LOOP_MODE.LOOP_RANDOM) != 0;
            btn.BackgroundImage = isRandom
                ? _skin.Buttons["MainForm"]["BtnRandom"].DownImage
                : _skin.Buttons["MainForm"]["BtnRandom"].BackImage;
            btn.Refresh();
        }

        // ── 内部ヘルパー ─────────────────────────────────────────────

        private void ApplyControls(System.Windows.Forms.Control.ControlCollection controls)
        {
			foreach (Control c in controls)
            {
                var parentName = c.Parent?.Name ?? "";
                var sliderMap = _skin.Sliders;
                _skin.Buttons.TryGetValue(parentName, out var btnMap);
                _skin.Labels.TryGetValue(parentName, out var labelMap);
                _skin.Grids.TryGetValue(parentName, out var gridMap);
                if (c is Button btn && btnMap.TryGetValue(c.Name, out var bc))
                {
                    if (bc.BackImage == null || !bc.Enabled)
                    {
                        btn.Visible = false;
                        btn.Enabled = false;
                        continue;
                    }
                    btn.AutoSize = false;
                    btn.BackgroundImage = bc.BackImage;
                    btn.BackgroundImageLayout = ImageLayout.None;
                    btn.Top = bc.Position.Top;
                    btn.Left = bc.Position.Left;
                    btn.Width = bc.Position.Width;
                    btn.Height = bc.Position.Height;
                    btn.Enabled = btn.Visible = bc.Enabled;
                    btn.Refresh();
                }
                else if (c is CustomSlider slider && sliderMap.TryGetValue(c.Name, out var sc))
                {
                    if (sc.SliderImage == null) continue;
                    slider.SliderImage = sc.SliderImage;
                    slider.Orientation = sc.Orientation;
                    slider.Minimum = sc.Minimum;
                    slider.Maximum = sc.Maximum;
                    slider.Top = sc.Position.Top;
                    slider.Left = sc.Position.Left;
                    slider.Width = sc.Position.Width;
                    slider.Height = sc.Position.Height;
                    slider.Enabled = slider.Visible = sc.Enabled;
                    slider.Value = 0;
                    slider.Refresh();
                }
                else if (c is ScrollLabel lbl && labelMap.TryGetValue(c.Name, out var gc))
                {
                    lbl.BackColor = Color.Transparent;
                    lbl.Value.Font = gc.Font;
                    lbl.Value.ForeColor = gc.FontColor;
                    lbl.Top = gc.Position.Top;
                    lbl.Left = gc.Position.Left;
                    lbl.Width = gc.Position.Width;
                    lbl.Height = gc.Position.Height;
                    lbl.Enabled = lbl.Visible = gc.Enabled;
                    lbl.Value.Left = 0;
                    lbl.Value.Width = gc.Position.Width;
                    lbl.Value.Height = gc.Position.Height;
                    lbl.ScrollEnable = gc.ScrollEnable;
                    lbl.Timer.Interval = gc.Interval > 0 ? gc.Interval : 100;
                    lbl.Timer.Enabled = gc.Interval > 0;
                }
                else if (c is DataGridView dgv && gridMap.TryGetValue(c.Name, out var plGrid))
                {
					dgv.BackgroundColor = plGrid.ListBackColor;
					dgv.RowsDefaultCellStyle.BackColor = plGrid.ListBackColor;
					dgv.RowsDefaultCellStyle.ForeColor = plGrid.ListForeColor;
					dgv.ForeColor = plGrid.ListForeColor;
					dgv.Left = plGrid.ListPosition.Left;
					dgv.Top = plGrid.ListPosition.Top;
					dgv.Width = plGrid.ListPosition.Width;
					dgv.Height = plGrid.ListPosition.Height;
				}
            }
        }
	}
}
