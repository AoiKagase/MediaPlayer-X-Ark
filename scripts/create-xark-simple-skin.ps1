Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot
$buildDir = Join-Path $repo "Resources\Skins\X-Ark-Simple"
$upperSkins = Join-Path $repo "Resources\Skins"

New-Item -ItemType Directory -Force -Path $buildDir | Out-Null

function New-ArgbColor($hex) {
	$hex = $hex.TrimStart("#")
	return [System.Drawing.Color]::FromArgb(
		[Convert]::ToInt32($hex.Substring(0, 2), 16),
		[Convert]::ToInt32($hex.Substring(2, 2), 16),
		[Convert]::ToInt32($hex.Substring(4, 2), 16))
}

function Draw-RoundedRect($graphics, $brush, $pen, $rect, $radius) {
	$path = [System.Drawing.Drawing2D.GraphicsPath]::new()
	$d = $radius * 2
	$path.AddArc($rect.X, $rect.Y, $d, $d, 180, 90)
	$path.AddArc($rect.Right - $d, $rect.Y, $d, $d, 270, 90)
	$path.AddArc($rect.Right - $d, $rect.Bottom - $d, $d, $d, 0, 90)
	$path.AddArc($rect.X, $rect.Bottom - $d, $d, $d, 90, 90)
	$path.CloseFigure()
	$graphics.FillPath($brush, $path)
	if ($pen -ne $null) { $graphics.DrawPath($pen, $path) }
	$path.Dispose()
}

function Draw-Button($graphics, $x, $y, $w, $h, $text, $pressed) {
	$base = if ($pressed) { New-ArgbColor "123a46" } else { New-ArgbColor "151b22" }
	$edge = if ($pressed) { New-ArgbColor "49d8e8" } else { New-ArgbColor "287789" }
	$textColor = if ($pressed) { New-ArgbColor "ffffff" } else { New-ArgbColor "b8ecf0" }
	$rect = [System.Drawing.Rectangle]::new($x + 1, $y + 1, $w - 2, $h - 2)
	$brush = [System.Drawing.SolidBrush]::new($base)
	$pen = [System.Drawing.Pen]::new($edge, 1)
	Draw-RoundedRect $graphics $brush $pen $rect 5
	$font = [System.Drawing.Font]::new("Segoe UI", 8, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Point)
	$stringBrush = [System.Drawing.SolidBrush]::new($textColor)
	$format = [System.Drawing.StringFormat]::new()
	$format.Alignment = [System.Drawing.StringAlignment]::Center
	$format.LineAlignment = [System.Drawing.StringAlignment]::Center
	$textRect = [System.Drawing.RectangleF]::new($rect.X, $rect.Y, $rect.Width, $rect.Height)
	$graphics.DrawString($text, $font, $stringBrush, $textRect, $format)
	$format.Dispose()
	$stringBrush.Dispose()
	$font.Dispose()
	$pen.Dispose()
	$brush.Dispose()
}

function Draw-BackgroundPanel($graphics, $rect, $title, $brand) {
	$bg = [System.Drawing.SolidBrush]::new((New-ArgbColor "11161c"))
	$graphics.FillRectangle($bg, $rect)
	$bg.Dispose()
	$border = [System.Drawing.Pen]::new((New-ArgbColor "25323b"), 1)
	$graphics.DrawRectangle($border, $rect.X, $rect.Y, $rect.Width - 1, $rect.Height - 1)
	$border.Dispose()
	$accent = [System.Drawing.Pen]::new((New-ArgbColor "49d8e8"), 2)
	$graphics.DrawLine($accent, $rect.X + 14, $rect.Y + 22, $rect.Right - 14, $rect.Y + 22)
	$accent.Dispose()
	$titleBrush = [System.Drawing.SolidBrush]::new((New-ArgbColor "dce7ea"))
	$mutedBrush = [System.Drawing.SolidBrush]::new((New-ArgbColor "789099"))
	$titleFont = [System.Drawing.Font]::new("Segoe UI", 9, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Point)
	$brandFont = [System.Drawing.Font]::new("Segoe UI", 8, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Point)
	$graphics.DrawString($title, $titleFont, $titleBrush, $rect.X + 14, $rect.Y + 5)
	$graphics.DrawString($brand, $brandFont, $mutedBrush, $rect.Right - 118, $rect.Y + 6)
	$titleFont.Dispose()
	$brandFont.Dispose()
	$titleBrush.Dispose()
	$mutedBrush.Dispose()
}

$background = [System.Drawing.Bitmap]::new(620, 720)
$g = [System.Drawing.Graphics]::FromImage($background)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
$g.Clear((New-ArgbColor "202030"))
Draw-BackgroundPanel $g ([System.Drawing.Rectangle]::new(0, 0, 579, 174)) "X-Ark Simple" "MediaPlayer X-Ark"
$panelBrush = [System.Drawing.SolidBrush]::new((New-ArgbColor "0a0f14"))
$g.FillRectangle($panelBrush, 17, 29, 229, 76)
$g.FillRectangle($panelBrush, 260, 35, 248, 36)
$g.FillRectangle($panelBrush, 15, 111, 497, 8)
$g.FillRectangle($panelBrush, 521, 27, 12, 134)
$panelBrush.Dispose()
$accentPen = [System.Drawing.Pen]::new((New-ArgbColor "49d8e8"), 1)
for ($i = 0; $i -lt 20; $i++) {
	$x = 24 + ($i * 10)
	$h = 8 + (($i * 17) % 58)
	$g.DrawLine($accentPen, $x, 101, $x, 101 - $h)
}
$accentPen.Dispose()
Draw-BackgroundPanel $g ([System.Drawing.Rectangle]::new(0, 200, 316, 174)) "Playlist" "X-Ark"
$g.FillRectangle(([System.Drawing.SolidBrush]::new((New-ArgbColor "0a0f14"))), 14, 229, 288, 132)
Draw-BackgroundPanel $g ([System.Drawing.Rectangle]::new(0, 400, 420, 520)) "File Info" "X-Ark"
$g.FillRectangle(([System.Drawing.SolidBrush]::new((New-ArgbColor "0a0f14"))), 18, 44 + 400, 132, 132)
Draw-BackgroundPanel $g ([System.Drawing.Rectangle]::new(340, 200, 360, 78)) "Mini" "X-Ark"
$g.Dispose()
$background.Save((Join-Path $buildDir "background.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$background.Dispose()

$buttons = [System.Drawing.Bitmap]::new(360, 220)
$g = [System.Drawing.Graphics]::FromImage($buttons)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
$g.Clear([System.Drawing.Color]::Transparent)
$labels = @("BACK","-10","PLAY","STOP","+10","NEXT","RND","LOOP","OPEN","LIST","OPT","X","CD","MINI","SAVE","DEL","UP","DOWN","CLR","PAUSE")
for ($i = 0; $i -lt $labels.Count; $i++) {
	$x = ($i % 5) * 72
	$y = [Math]::Floor($i / 5) * 48
	Draw-Button $g $x $y 64 18 $labels[$i] $false
	Draw-Button $g $x ($y + 22) 64 18 $labels[$i] $true
}
Draw-Button $g 0 192 14 14 "S" $false
Draw-Button $g 20 192 10 18 "S" $false
$g.Dispose()
$buttons.Save((Join-Path $buildDir "buttons.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$buttons.Dispose()

$skinJson = @'
{
  "version": "2.1",
  "meta": {
    "name": "X-Ark Simple",
    "author": "MediaPlayer X-Ark",
    "description": "Logo/icon inspired simple skin using Background and Button sprites."
  },
  "settings": { "transparentKey": "202030" },
  "images": {
    "background": "background.png",
    "button": "buttons.png"
  },
  "mainForm": {
    "src": { "imageKey": "background", "x": 0, "y": 0, "w": 579, "h": 174 },
    "location": { "x": 0, "y": 0, "w": 579, "h": 174 },
    "buttons": {
      "BtnBack": { "location": { "x": 14, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnSeekBack": { "location": { "x": 98, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnPlay": { "location": { "x": 266, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 144, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 144, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnPause": { "location": { "x": 266, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 288, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 288, "y": 166, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnStop": { "location": { "x": 182, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnSeekForward": { "location": { "x": 350, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 288, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 288, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnNext": { "location": { "x": 434, "y": 128, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnRandom": { "location": { "x": 14, "y": 146, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnLoop": { "location": { "x": 98, "y": 146, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 144, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 144, "y": 70, "w": 64, "h": 18 }, "optional": { "imageKey": "button", "x": 144, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnOpen": { "location": { "x": 350, "y": 146, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnPlaylist": { "location": { "x": 355, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 288, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 288, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnSetting": { "location": { "x": 411, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 118, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnClose": { "location": { "x": 523, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 118, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnCD": { "location": { "x": 434, "y": 146, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 144, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 144, "y": 118, "w": 64, "h": 18 }, "isDisabled": false },
      "BtnMinisize": { "location": { "x": 467, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 118, "w": 64, "h": 18 }, "isDisabled": false }
    },
    "sliders": {
      "SldVolume": { "src": { "imageKey": "button", "x": 20, "y": 192, "w": 10, "h": 18 }, "location": { "x": 521, "y": 27, "w": 43, "h": 134 }, "min": 0, "max": 200, "orientation": "vertical" },
      "SldTrack": { "src": { "imageKey": "button", "x": 0, "y": 192, "w": 14, "h": 14 }, "location": { "x": 15, "y": 111, "w": 497, "h": 14 }, "min": 0, "max": 100, "orientation": "horizontal" },
      "SldPan": { "src": { "imageKey": "button", "x": 0, "y": 192, "w": 14, "h": 14 }, "location": { "x": 184, "y": 148, "w": 163, "h": 14 }, "min": -10, "max": 10, "orientation": "horizontal" }
    },
    "spectrum": { "src": { "imageKey": "background", "x": 17, "y": 29, "w": 229, "h": 76 }, "location": { "x": 17, "y": 29, "w": 229, "h": 76 }, "color": "0a0f14", "waveColorL": "49d8e8", "waveColorR": "e5f65a" },
    "waveArea": { "target": "trackbar", "mode": "mix", "exponent": 2.0, "location": { "x": 0, "y": 0, "w": 0, "h": 0 }, "colorMix": "49d8e8", "colorUnplayed": "25323b", "colorPlayed": "e5f65a" },
    "labels": {
      "LabelTitle": { "location": { "x": 260, "y": 35, "w": 248, "h": 36 }, "font": "Yu Gothic UI", "size": 14, "bold": true, "italic": false, "foreColor": "dce7ea", "interval": 100, "scrollEnable": true, "align": "left" },
      "LabelTime": { "location": { "x": 304, "y": 74, "w": 210, "h": 30 }, "font": "Segoe UI", "size": 16, "bold": true, "italic": false, "foreColor": "49d8e8", "interval": 0, "scrollEnable": false, "align": "right" }
    }
  },
  "subForms": {
    "PlayListForm": {
      "src": { "imageKey": "background", "x": 0, "y": 200, "w": 316, "h": 174 },
      "location": { "x": 0, "y": 0, "w": 316, "h": 174 },
      "offset": { "x": 580, "y": 0, "w": 0, "h": 0 },
      "buttons": {
        "PBtnOpen": { "location": { "x": 83, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnSave": { "location": { "x": 121, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 288, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 288, "y": 118, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnRemove": { "location": { "x": 160, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 166, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnUp": { "location": { "x": 213, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 166, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnDown": { "location": { "x": 253, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 144, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 144, "y": 166, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnClose": { "location": { "x": 293, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 118, "w": 64, "h": 18 }, "isDisabled": false },
        "PBtnClear": { "location": { "x": 293, "y": 2, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 166, "w": 64, "h": 18 }, "isDisabled": true }
      },
      "grids": { "PlayListGrid": { "location": { "x": 14, "y": 29, "w": 288, "h": 132 }, "backColor": "0a0f14", "foreColor": "dce7ea" } },
      "magnetic": true
    },
    "MiniPlayerForm": {
      "src": { "imageKey": "background", "x": 340, "y": 200, "w": 360, "h": 78 },
      "location": { "x": 0, "y": 0, "w": 360, "h": 78 },
      "buttons": {
        "BtnBack": { "location": { "x": 10, "y": 34, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
        "BtnPlay": { "location": { "x": 78, "y": 34, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 144, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 144, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
        "BtnPause": { "location": { "x": 78, "y": 34, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 288, "y": 144, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 288, "y": 166, "w": 64, "h": 18 }, "isDisabled": false },
        "BtnStop": { "location": { "x": 146, "y": 34, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 216, "y": 0, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 216, "y": 22, "w": 64, "h": 18 }, "isDisabled": false },
        "BtnNext": { "location": { "x": 214, "y": 34, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 0, "y": 48, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 0, "y": 70, "w": 64, "h": 18 }, "isDisabled": false },
        "BtnClose": { "location": { "x": 286, "y": 4, "w": 0, "h": 0 }, "up": { "imageKey": "button", "x": 72, "y": 96, "w": 64, "h": 18 }, "down": { "imageKey": "button", "x": 72, "y": 118, "w": 64, "h": 18 }, "isDisabled": false }
      },
      "sliders": {
        "SldTrack": { "src": { "imageKey": "button", "x": 0, "y": 192, "w": 14, "h": 14 }, "location": { "x": 10, "y": 58, "w": 240, "h": 14 }, "min": 0, "max": 100, "orientation": "horizontal" },
        "SldVolume": { "src": { "imageKey": "button", "x": 0, "y": 192, "w": 14, "h": 14 }, "location": { "x": 260, "y": 58, "w": 88, "h": 14 }, "min": 0, "max": 200, "orientation": "horizontal" }
      },
      "labels": {
        "LabelTitle": { "location": { "x": 10, "y": 7, "w": 260, "h": 20 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "italic": false, "foreColor": "dce7ea", "interval": 100, "scrollEnable": true, "align": "left" }
      }
    },
    "FileInfoForm": {
      "src": { "imageKey": "background", "x": 0, "y": 400, "w": 420, "h": 520 },
      "location": { "x": 0, "y": 0, "w": 420, "h": 520 },
      "backColor": "11161c",
      "foreColor": "dce7ea",
      "font": "Yu Gothic UI",
      "fontSize": 9,
      "pictures": { "picCover": { "location": { "x": 18, "y": 44, "w": 132, "h": 132 }, "borderColor": "49d8e8", "borderWidth": 1 } },
      "labels": {
        "lblTitleKey": { "location": { "x": 168, "y": 48, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblTitleVal": { "location": { "x": 258, "y": 48, "w": 144, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblArtistKey": { "location": { "x": 168, "y": 72, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblArtistVal": { "location": { "x": 258, "y": 72, "w": 144, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblAlbumKey": { "location": { "x": 168, "y": 96, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblAlbumVal": { "location": { "x": 258, "y": 96, "w": 144, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblYearKey": { "location": { "x": 168, "y": 120, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblYearVal": { "location": { "x": 258, "y": 120, "w": 144, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblTrackKey": { "location": { "x": 168, "y": 144, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblTrackVal": { "location": { "x": 258, "y": 144, "w": 144, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblFileNameKey": { "location": { "x": 18, "y": 194, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblFileNameVal": { "location": { "x": 118, "y": 194, "w": 284, "h": 42 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblFormatKey": { "location": { "x": 18, "y": 250, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblFormatVal": { "location": { "x": 118, "y": 250, "w": 284, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblBitKey": { "location": { "x": 18, "y": 274, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblBitVal": { "location": { "x": 118, "y": 274, "w": 284, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblLengthKey": { "location": { "x": 18, "y": 298, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblLengthVal": { "location": { "x": 118, "y": 298, "w": 284, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblSampleRateKey": { "location": { "x": 18, "y": 322, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblSampleRateVal": { "location": { "x": 118, "y": 322, "w": 284, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" },
        "lblChannelKey": { "location": { "x": 18, "y": 346, "w": 96, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "789099" },
        "lblChannelVal": { "location": { "x": 118, "y": 346, "w": 284, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": false, "foreColor": "dce7ea" }
      }
    }
  }
}
'@

Set-Content -Path (Join-Path $buildDir "skin.json") -Value $skinJson -Encoding UTF8

$xskUpper = Join-Path $upperSkins "xark-simple.xsk"
if (Test-Path $xskUpper) { Remove-Item -LiteralPath $xskUpper -Force }
[IO.Compression.ZipFile]::CreateFromDirectory($buildDir, $xskUpper)
