using MediaPlayer_X_Ark.Engine;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Skin.New;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System;
using System.Runtime.InteropServices;
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
        private readonly Dictionary<(Image Image, int ScaleKey), Image> _scaledImageCache = new();

        public SkinApplicator(INewSkinSystem skin)
        {
            _skin = skin;
        }

        /// <summary>メインフォームにスキンを適用する</summary>
        public void ApplyToMainForm(Form form, Controls.SpectrumAnalyzer spectrum)
        {
            // 全変更が終わるまで描画を停止して中間状態の残像を防ぐ
            if (form.IsHandleCreated)
                Win32API.SendMessage(form.Handle, Win32API.WM_SETREDRAW, false, 0);
            try
            {
                float scale = GetScaleFactor(form);
                var formRect = ScaleRect(_skin.MainForm.Position, scale);
                var spectrumRect = ScaleRectWithFlooredPosition(_skin.Spectrum.Position, scale);

                form.BackgroundImage = ScaleImage(_skin.MainForm.BackImage, scale);
                form.TransparencyKey = _skin.MainForm.TransparentKey;
                form.Width           = formRect.Width;
                form.Height          = formRect.Height;

                // スペクトラム配置
                form.SuspendLayout();
                spectrum.Left   = spectrumRect.Left;
                spectrum.Top    = spectrumRect.Top;
                spectrum.Width  = spectrumRect.Width;
                spectrum.Height = spectrumRect.Height;
                form.ResumeLayout(false);

                // スペクトラムビットマップ再生成
                int sw = spectrumRect.Width;
                int sh = spectrumRect.Height;

                if (_skin.Spectrum.Image != null)
                {
                    spectrum.BitmapSpectrum = (Bitmap)_skin.Spectrum.Image.Clone();
                }
                else
                {
                    spectrum.BitmapSpectrum = new Bitmap(sw, sh);
                    using (var g = Graphics.FromImage(spectrum.BitmapSpectrum))
                        g.Clear(_skin.Spectrum.Color);
                }

                // 背景からスペクトラム領域を切り出し
                if (_skin.MainForm.BackImage != null)
                {
                    var rect = spectrumRect;
                    var bmp = new Bitmap(sw, sh);
                    var scaledBackground = form.BackgroundImage;
                    using (var g = Graphics.FromImage(bmp))
                        g.DrawImage(scaledBackground,
                            new Rectangle(0, 0, sw, sh), rect, GraphicsUnit.Pixel);
                    spectrum.BitmapBackground = bmp;
                }
                else
                {
                    spectrum.BitmapBackground = null;
                }

                // ビットマップ設定直後に D2D 描画を有効化して残像を防ぐ
                spectrum.Initialize();

                // ボタン・スライダー・ラベルを適用
                ApplyControls(form.Controls);
            }
            finally
            {
                if (form.IsHandleCreated)
                    Win32API.SendMessage(form.Handle, Win32API.WM_SETREDRAW, true, 0);
                form.Refresh();
            }
        }

        /// <summary>プレイリストフォームにスキンを適用する</summary>
        public void ApplyToPlayListForm(Form playListForm)
        {
            float scale = GetScaleFactor(playListForm);
            var rect = ScaleRect(_skin.SubForms["PlayListForm"].Position, scale);

            playListForm.BackgroundImage = ScaleImage(_skin.SubForms["PlayListForm"].BackImage, scale);
            playListForm.Width           = rect.Width;
            playListForm.Height          = rect.Height;
            playListForm.TransparencyKey = _skin.SubForms["PlayListForm"].TransparentKey;
            playListForm.Refresh();

            ApplyControls(playListForm.Controls);
        }
        /// <summary>ファイル情報フォームにスキンを適用する</summary>
        public void ApplyToFileInfoForm(Form form)
        {
            if (!_skin.SubForms.TryGetValue("FileInfoForm", out var def)) return;

            if (def.BackImage != null)
            {
				form.FormBorderStyle = FormBorderStyle.None;
				form.BackgroundImage = ScaleImage(def.BackImage, GetScaleFactor(form));
			}
			else if (def.BackColor != Color.Empty)
            {
				form.BackColor = def.BackColor;
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
			}

			if (def.ForeColor != Color.Empty)
                form.ForeColor = def.ForeColor;

            float scale = GetScaleFactor(form);

            if (def.Position.Width > 0) form.Width = ScaleLength(def.Position.Width, scale);
            if (def.Position.Height > 0) form.Height = ScaleLength(def.Position.Height, scale);

            ApplyLabelsRecursive(form.Controls, def, scale);
            ApplyControls(form.Controls);
            form.Refresh();
        }

        private void ApplyLabelsRecursive(
            System.Windows.Forms.Control.ControlCollection controls,
            FormComponents def,
            float scale)
        {
            foreach (Control c in controls)
            {
                if (c is Label lbl)
                {
                    lbl.BackColor = Color.Transparent;
                    if (def.ForeColor != Color.Empty) lbl.ForeColor = def.ForeColor;
                    if (def.Font != null) lbl.Font = ScaleFont(def.Font, scale);
                }
                if (c.Controls.Count > 0)
                    ApplyLabelsRecursive(c.Controls, def, scale);
            }
        }

        /// <summary>ミニプレイヤーフォームにスキンを適用する</summary>
        public void ApplyToMiniPlayerForm(Form miniPlayerForm)
        {
            if (!_skin.SubForms.TryGetValue("MiniPlayerForm", out var def)) return;

            if (def.BackImage != null)
                miniPlayerForm.BackgroundImage = ScaleImage(def.BackImage, GetScaleFactor(miniPlayerForm));
            else if (def.BackColor != Color.Empty)
                miniPlayerForm.BackColor = def.BackColor;

            float scale = GetScaleFactor(miniPlayerForm);

            if (def.Position.Width > 0) miniPlayerForm.Width = ScaleLength(def.Position.Width, scale);
            if (def.Position.Height > 0) miniPlayerForm.Height = ScaleLength(def.Position.Height, scale);
            miniPlayerForm.TransparencyKey = def.TransparentKey;

            ApplyControls(miniPlayerForm.Controls);
            miniPlayerForm.Refresh();
        }
        /// <summary>プレイリストフォームの位置をマグネットモードに合わせて更新する</summary>
        public void UpdatePlayListPosition(Form mainForm, Form playListForm)
        {
            float scale = GetScaleFactor(mainForm);
            playListForm.Left = mainForm.Left + ScaleCoordinate(_skin.SubForms["PlayListForm"].Position.Left, scale);
            playListForm.Top  = mainForm.Top  + ScaleCoordinate(_skin.SubForms["PlayListForm"].Position.Top, scale);
        }

        public int ScaleValue(Control control, int value) => ScaleCoordinate(value, GetScaleFactor(control));

        /// <summary>ボタンの押下画像をセットする</summary>
        public void SetButtonDown(Button btn)
        {
            try
            {
                var parent = btn.Parent.Name;
                if (!_skin.Buttons.TryGetValue(parent, out var btnMap)) return;
                if (btnMap.TryGetValue(btn.Name, out var bc))
                {
                    btn.BackgroundImage = ScaleImage(bc.DownImage, GetScaleFactor(btn));
                    btn.Refresh();
                }
            }
            catch { }
        }

        /// <summary>ボタンの通常画像をセットする</summary>
        public void SetButtonUp(Button btn)
        {
            try
            {
                var parent = btn.Parent?.Name;
                if (!_skin.Buttons.TryGetValue(parent, out var btnMap)) return;
                if (btnMap.TryGetValue(btn.Name, out var bc))
                {
                    btn.BackgroundImage = ScaleImage(bc.BackImage, GetScaleFactor(btn));
                    btn.Refresh();
                }
            }
            catch { }
        }

        /// <summary>ループボタンの状態画像をセットする</summary>
        public void UpdateLoopButton(Button btn, LOOP_MODE loop)
        {
            var loopOnly = loop & ~LOOP_MODE.LOOP_RANDOM;
            var image = loopOnly switch
            {
                LOOP_MODE.LOOP_ONE_REPEAT => _skin.Buttons["MainForm"]["BtnLoop"].DownImage,
                LOOP_MODE.LOOP_ALL        => _skin.Buttons["MainForm"]["BtnLoop"].OptionalImage,
				_                         => _skin.Buttons["MainForm"]["BtnLoop"].BackImage,

			};
            btn.BackgroundImage = ScaleImage(image, GetScaleFactor(btn));
            btn.Refresh();
        }

        /// <summary>ランダムボタンの状態画像をセットする</summary>
        public void UpdateRandomButton(Button btn, LOOP_MODE loop)
        {
            bool isRandom = (loop & LOOP_MODE.LOOP_RANDOM) != 0;
            btn.BackgroundImage = ScaleImage(isRandom
                ? _skin.Buttons["MainForm"]["BtnRandom"].DownImage
                : _skin.Buttons["MainForm"]["BtnRandom"].BackImage,
                GetScaleFactor(btn));
            btn.Refresh();
        }

        // ── 内部ヘルパー ─────────────────────────────────────────────

        private void ApplyControls(System.Windows.Forms.Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                float scale = GetScaleFactor(c);
                var parentName = c.Parent?.Name ?? "";
                _skin.Buttons.TryGetValue(parentName, out var btnMap);
                _skin.Labels.TryGetValue(parentName, out var labelMap);
                _skin.Grids.TryGetValue(parentName, out var gridMap);
                _skin.Pictures.TryGetValue(parentName, out var pictureMap);
                if (!_skin.FormSliders.TryGetValue(parentName, out var sliderMap))
                    sliderMap = _skin.Sliders;
                if (c is Button btn && (btnMap?.TryGetValue(c.Name, out var bc) ?? false))
                {
                    if (bc.BackImage == null || !bc.Enabled)
                    {
                        btn.Visible = false;
                        btn.Enabled = false;
                        continue;
                    }
                    var rect = ScaleRect(bc.Position, scale);
                    btn.AutoSize = false;
                    btn.BackgroundImage = ScaleImage(bc.BackImage, scale);
                    btn.BackgroundImageLayout = ImageLayout.None;
                    btn.Top = rect.Top;
                    btn.Left = rect.Left;
                    btn.Width = rect.Width;
                    btn.Height = rect.Height;
                    btn.Enabled = btn.Visible = bc.Enabled;
                    btn.Refresh();
                }
                else if (c is Button unskinnedButton && ShouldHideUnskinnedFileInfoButton(parentName, unskinnedButton, btnMap))
                {
                    unskinnedButton.Visible = false;
                    unskinnedButton.Enabled = false;
                }
                else if (c is CustomSlider slider && (sliderMap?.TryGetValue(c.Name, out var sc) ?? false))
                {
                    if (!sc.Enabled)
                    {
                        slider.Visible = false;
                        slider.Enabled = false;
                        continue;
                    }
                    if (sc.SliderImage == null) continue;
                    int previousMinimum = slider.Minimum;
                    int previousMaximum = slider.Maximum;
                    int previousValue = slider.Value;
                    bool preserveRange = previousMaximum > previousMinimum;
                    var rect = ScaleRect(sc.Position, scale);
                    slider.SliderImage = ScaleImage(sc.SliderImage, scale);
                    slider.Orientation = sc.Orientation;
                    slider.Minimum = preserveRange ? previousMinimum : sc.Minimum;
                    slider.Maximum = preserveRange ? previousMaximum : sc.Maximum;
                    slider.Top = rect.Top;
                    slider.Left = rect.Left;
                    slider.Width = rect.Width;
                    slider.Height = rect.Height;
                    slider.Enabled = slider.Visible = sc.Enabled;
                    slider.SetValueSilently(previousValue);
                    slider.Refresh();
                }
                else if (c is ScrollLabel lbl && (labelMap?.TryGetValue(c.Name, out var gc) ?? false))
                {
                    ApplyScrollLabel(lbl, gc, scale);
                }
                else if (c is Label label && (labelMap?.TryGetValue(c.Name, out var lc) ?? false))
                {
                    var rect = ScaleRectWithFlooredPosition(lc.Position, scale);
                    label.BackColor = lc.BackColor == Color.Empty ? Color.Transparent : lc.BackColor;
                    label.Font = ScaleFont(lc.Font, scale);
                    label.ForeColor = lc.FontColor;
                    label.TextAlign = lc.HorizontalAlign switch
                    {
                        HorizontalAlignment.Center => ContentAlignment.MiddleCenter,
                        HorizontalAlignment.Right => ContentAlignment.MiddleRight,
                        _ => ContentAlignment.MiddleLeft,
                    };
                    label.Left = rect.Left;
                    label.Top = rect.Top;
                    label.Width = rect.Width;
                    label.Height = rect.Height;
                    label.Enabled = label.Visible = lc.Enabled;
                }
                else if (c is DataGridView dgv && (gridMap?.TryGetValue(c.Name, out var plGrid) ?? false))
                {
                    var rect = ScaleRect(plGrid.ListPosition, scale);
					dgv.BackgroundColor = plGrid.ListBackColor;
					dgv.RowsDefaultCellStyle.BackColor = plGrid.ListBackColor;
					dgv.RowsDefaultCellStyle.ForeColor = plGrid.ListForeColor;
					dgv.ForeColor = plGrid.ListForeColor;
                    dgv.Font = ScaleFont(dgv.Font, scale);
                    dgv.RowTemplate.Height = ScaleLength(dgv.RowTemplate.Height, scale);
					dgv.Left = rect.Left;
					dgv.Top = rect.Top;
					dgv.Width = rect.Width;
					dgv.Height = rect.Height;
				}
                else if (c is PictureBox picture && (pictureMap?.TryGetValue(c.Name, out var pc) ?? false))
                {
                    var rect = ScaleRect(pc.Position, scale);
                    picture.Left = rect.Left;
                    picture.Top = rect.Top;
                    picture.Width = rect.Width;
                    picture.Height = rect.Height;
                    picture.BackColor = pc.Color == Color.Empty ? Color.Transparent : pc.Color;
                    if (picture is Controls.RoundedPictureBox roundedPicture)
                    {
                        roundedPicture.BorderColor = pc.BorderColor;
                        roundedPicture.BorderWidth = ScaleLength(pc.BorderWidth, scale);
                        roundedPicture.CornerRadius = ScaleLength(pc.CornerRadius, scale);
                        roundedPicture.Visible = pc.Enabled;
                    }
                    if (pc.Image != null)
                        picture.Image = ScaleImage(pc.Image, scale);
                    picture.Enabled = picture.Visible = pc.Enabled;
                }

                if (c.Controls.Count > 0)
                    ApplyControls(c.Controls);
            }
        }

        private static bool ShouldHideUnskinnedFileInfoButton(
            string parentName,
            Button button,
            Dictionary<string, ButtonComponents> buttonMap)
            => parentName == "FileInfoForm"
               && button.Name == "BtnClose"
               && !(buttonMap?.ContainsKey(button.Name) ?? false);

        private void ApplyScrollLabel(ScrollLabel lbl, LabelComponents gc, float scale)
        {
            var rect = ScaleRectWithFlooredPosition(gc.Position, scale);
            lbl.BackColor = gc.BackColor == Color.Empty ? Color.Transparent : gc.BackColor;
            lbl.HorizontalAlign = gc.HorizontalAlign;
            lbl.Value.Font = ScaleFont(gc.Font, scale);
            lbl.Value.ForeColor = gc.FontColor;
            lbl.Value.BackColor = lbl.BackColor;
            lbl.Top = rect.Top;
            lbl.Left = rect.Left;
            lbl.Width = rect.Width;
            lbl.Height = rect.Height;
            lbl.Enabled = lbl.Visible = gc.Enabled;
            lbl.Value.Left = 0;
            lbl.Value.Width = rect.Width;
            lbl.Value.Height = rect.Height;
            lbl.ScrollEnable = gc.ScrollEnable;
            lbl.Timer.Interval = gc.Interval > 0 ? gc.Interval : 100;
            lbl.Timer.Enabled = gc.Interval > 0;
        }

        private static float GetScaleFactor(Control control)
        {
            if (control == null)
                return 1f;

            if (control.DeviceDpi > 0)
                return control.DeviceDpi / 96f;

            using var g = control.CreateGraphics();
            return g.DpiX / 96f;
        }

        private static int ScaleCoordinate(int value, float scale)
            => (int)Math.Round(value * scale, MidpointRounding.AwayFromZero);

        private static int ScaleLength(int value, float scale)
        {
            if (value <= 0)
                return value;

            return Math.Max(1, (int)Math.Round(value * scale, MidpointRounding.AwayFromZero));
        }

        private static Rectangle ScaleRect(RECT rect, float scale)
            => new Rectangle(
                ScaleCoordinate(rect.Left, scale),
                ScaleCoordinate(rect.Top, scale),
                ScaleLength(rect.Width, scale),
                ScaleLength(rect.Height, scale));

        private static Rectangle ScaleRectWithFlooredPosition(RECT rect, float scale)
            => new Rectangle(
                ScaleCoordinateFloor(rect.Left, scale),
                ScaleCoordinateFloor(rect.Top, scale),
                ScaleLength(rect.Width, scale),
                ScaleLength(rect.Height, scale));

        private static int ScaleCoordinateFloor(int value, float scale)
            => (int)Math.Floor(value * scale);

        private Font ScaleFont(Font font, float scale)
        {
            if (font == null)
                return null;

            return new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Point);
        }

        private Image ScaleImage(Image image, float scale)
        {
            if (image == null || scale <= 0f)
                return image;

            if (Math.Abs(scale - 1f) < 0.001f)
                return image;

            int scaleKey = (int)Math.Round(scale * 1000f, MidpointRounding.AwayFromZero);
            if (_scaledImageCache.TryGetValue((image, scaleKey), out var cached))
                return cached;

            int width = ScaleLength(image.Width, scale);
            int height = ScaleLength(image.Height, scale);
            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(image, new Rectangle(0, 0, width, height));
            }

            _scaledImageCache[(image, scaleKey)] = bitmap;
            return bitmap;
        }


	}
}
