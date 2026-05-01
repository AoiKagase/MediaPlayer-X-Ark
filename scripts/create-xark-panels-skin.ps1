Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot
$buildDir = Join-Path $repo "Resources\Skins\X-Ark-Panels"
$skinDir = Join-Path $repo "Resources\Skins"
New-Item -ItemType Directory -Force -Path $buildDir | Out-Null

function C($hex) {
	$hex = $hex.TrimStart("#")
	[System.Drawing.Color]::FromArgb(
		[Convert]::ToInt32($hex.Substring(0, 2), 16),
		[Convert]::ToInt32($hex.Substring(2, 2), 16),
		[Convert]::ToInt32($hex.Substring(4, 2), 16))
}

function RoundPath($rect, $radius) {
	$path = [System.Drawing.Drawing2D.GraphicsPath]::new()
	$d = $radius * 2
	$path.AddArc($rect.X, $rect.Y, $d, $d, 180, 90)
	$path.AddArc($rect.Right - $d, $rect.Y, $d, $d, 270, 90)
	$path.AddArc($rect.Right - $d, $rect.Bottom - $d, $d, $d, 0, 90)
	$path.AddArc($rect.X, $rect.Bottom - $d, $d, $d, 90, 90)
	$path.CloseFigure()
	$path
}

function FillRound($g, $rect, $fill, $stroke, $radius = 8) {
	$path = RoundPath $rect $radius
	$b = [System.Drawing.SolidBrush]::new((C $fill))
	$p = [System.Drawing.Pen]::new((C $stroke), 1)
	$g.FillPath($b, $path)
	$g.DrawPath($p, $path)
	$b.Dispose(); $p.Dispose(); $path.Dispose()
}

function Text($g, $s, $x, $y, $w, $h, $size, $color, $bold = $false, $align = "Near") {
	$style = if ($bold) { [System.Drawing.FontStyle]::Bold } else { [System.Drawing.FontStyle]::Regular }
	$f = [System.Drawing.Font]::new("Segoe UI", $size, $style, [System.Drawing.GraphicsUnit]::Point)
	$b = [System.Drawing.SolidBrush]::new((C $color))
	$fmt = [System.Drawing.StringFormat]::new()
	$fmt.Alignment = [System.Drawing.StringAlignment]::$align
	$fmt.LineAlignment = [System.Drawing.StringAlignment]::Center
	$g.DrawString($s, $f, $b, [System.Drawing.RectangleF]::new($x, $y, $w, $h), $fmt)
	$fmt.Dispose(); $b.Dispose(); $f.Dispose()
}

function SaveBackground($name, $w, $h, $title, $drawBlock) {
	$bmp = [System.Drawing.Bitmap]::new($w, $h)
	$g = [System.Drawing.Graphics]::FromImage($bmp)
	$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
	$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
	$g.Clear((C "241b2f"))
	FillRound $g ([System.Drawing.Rectangle]::new(0, 0, $w, $h)) "141510" "3b432f" 10
	Text $g $title 16 7 220 22 10 "7fd35b" $true
	Text $g "X-Ark" ($w - 86) 7 70 22 8 "a0aa8c" $false "Far"
	& $drawBlock $g
	$g.Dispose()
	$bmp.Save((Join-Path $buildDir $name), [System.Drawing.Imaging.ImageFormat]::Png)
	$bmp.Dispose()
}

SaveBackground "main-background.png" 640 164 "X-ARK PANELS" {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(18, 38, 220, 76)) "0b1209" "30402a" 7
	FillRound $g ([System.Drawing.Rectangle]::new(258, 38, 220, 34)) "0b1209" "283224" 7
	FillRound $g ([System.Drawing.Rectangle]::new(258, 90, 220, 12)) "0b1209" "30402a" 5
	FillRound $g ([System.Drawing.Rectangle]::new(500, 38, 22, 76)) "0b1209" "30402a" 5
	$p = [System.Drawing.Pen]::new((C "7fd35b"), 2)
	for ($i = 0; $i -lt 24; $i++) {
		$x = 30 + ($i * 8)
		$h = 10 + (($i * 19) % 52)
		$g.DrawLine($p, $x, 102, $x, 102 - $h)
	}
	$p.Dispose()
}
SaveBackground "playlist-background.png" 360 210 "PLAYLIST" {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(16, 40, 328, 150)) "0b1209" "283224" 7
}
SaveBackground "mini-background.png" 310 56 "MINI PANEL" {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(12, 32, 196, 10)) "0b1209" "30402a" 5
}
SaveBackground "fileinfo-background.png" 460 520 "FILE INFO" {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(22, 48, 128, 128)) "0b1209" "30402a" 8
	FillRound $g ([System.Drawing.Rectangle]::new(170, 48, 266, 128)) "0b1209" "283224" 8
	FillRound $g ([System.Drawing.Rectangle]::new(22, 206, 414, 284)) "0b1209" "283224" 8
}

function DrawIcon($g, $idx, $down, $kind) {
	$x = ($idx % 6) * 48
	$y = [Math]::Floor($idx / 6) * 48 + ($(if ($down) { 22 } else { 0 }))
	$fill = if ($down) { "20363b" } else { "141b1f" }
	$stroke = if ($down) { "ffb45a" } else { "7fd35b" }
	$fg = if ($down) { "ffffff" } else { "edf0dc" }
	FillRound $g ([System.Drawing.Rectangle]::new($x + 1, $y + 1, 40, 18)) $fill $stroke 6
	$b = [System.Drawing.SolidBrush]::new((C $fg))
	$p = [System.Drawing.Pen]::new((C $fg), 2)
	switch ($kind) {
		"prev" { $g.FillPolygon($b, @([Drawing.Point]::new($x+23,$y+5),[Drawing.Point]::new($x+14,$y+10),[Drawing.Point]::new($x+23,$y+15))); $g.DrawLine($p,$x+13,$y+5,$x+13,$y+15) }
		"back" { $g.FillPolygon($b, @([Drawing.Point]::new($x+25,$y+5),[Drawing.Point]::new($x+14,$y+10),[Drawing.Point]::new($x+25,$y+15))) }
		"play" { $g.FillPolygon($b, @([Drawing.Point]::new($x+16,$y+5),[Drawing.Point]::new($x+16,$y+15),[Drawing.Point]::new($x+27,$y+10))) }
		"pause" { $g.FillRectangle($b,$x+15,$y+5,4,10); $g.FillRectangle($b,$x+23,$y+5,4,10) }
		"stop" { $g.FillRectangle($b,$x+16,$y+6,10,10) }
		"fwd" { $g.FillPolygon($b, @([Drawing.Point]::new($x+15,$y+5),[Drawing.Point]::new($x+26,$y+10),[Drawing.Point]::new($x+15,$y+15))) }
		"next" { $g.FillPolygon($b, @([Drawing.Point]::new($x+14,$y+5),[Drawing.Point]::new($x+23,$y+10),[Drawing.Point]::new($x+14,$y+15))); $g.DrawLine($p,$x+25,$y+5,$x+25,$y+15) }
		"random" { $g.DrawLine($p,$x+13,$y+7,$x+28,$y+7); $g.DrawLine($p,$x+13,$y+13,$x+28,$y+13); $g.DrawLine($p,$x+24,$y+4,$x+29,$y+7); $g.DrawLine($p,$x+24,$y+10,$x+29,$y+13) }
		"loop" { $g.DrawArc($p,$x+12,$y+5,18,10,20,300); $g.FillPolygon($b,@([Drawing.Point]::new($x+28,$y+8),[Drawing.Point]::new($x+33,$y+10),[Drawing.Point]::new($x+28,$y+12))) }
		"open" { $g.DrawRectangle($p,$x+13,$y+8,16,8); $g.DrawLine($p,$x+13,$y+8,$x+18,$y+4); $g.DrawLine($p,$x+18,$y+4,$x+29,$y+4) }
		"list" { for($i=0;$i -lt 3;$i++){ $g.FillRectangle($b,$x+13,$y+5+$i*5,4,2); $g.DrawLine($p,$x+20,$y+6+$i*5,$x+30,$y+6+$i*5) } }
		"gear" { $g.DrawEllipse($p,$x+15,$y+5,12,12); $g.FillEllipse($b,$x+19,$y+9,4,4) }
		"close" { $g.DrawLine($p,$x+15,$y+6,$x+27,$y+14); $g.DrawLine($p,$x+27,$y+6,$x+15,$y+14) }
		"cd" { $g.DrawEllipse($p,$x+14,$y+4,14,14); $g.FillEllipse($b,$x+19,$y+9,4,4) }
		"mini" { $g.DrawRectangle($p,$x+13,$y+5,16,10); $g.DrawLine($p,$x+18,$y+13,$x+29,$y+13) }
		"save" { $g.DrawRectangle($p,$x+14,$y+5,14,12); $g.FillRectangle($b,$x+18,$y+6,7,4) }
		"trash" { $g.DrawRectangle($p,$x+16,$y+7,10,10); $g.DrawLine($p,$x+15,$y+5,$x+27,$y+5) }
		"up" { $g.FillPolygon($b,@([Drawing.Point]::new($x+21,$y+5),[Drawing.Point]::new($x+14,$y+14),[Drawing.Point]::new($x+28,$y+14))) }
		"down" { $g.FillPolygon($b,@([Drawing.Point]::new($x+14,$y+6),[Drawing.Point]::new($x+28,$y+6),[Drawing.Point]::new($x+21,$y+15))) }
		"clear" { $g.DrawRectangle($p,$x+14,$y+6,14,10); $g.DrawLine($p,$x+17,$y+9,$x+25,$y+13) }
	}
	$b.Dispose(); $p.Dispose()
}

$buttons = [System.Drawing.Bitmap]::new(288, 216)
$g = [System.Drawing.Graphics]::FromImage($buttons)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.Clear([System.Drawing.Color]::Transparent)
$icons = @("prev","back","play","stop","fwd","next","random","loop","open","list","gear","close","cd","mini","save","trash","up","down","clear","pause")
for ($i = 0; $i -lt $icons.Count; $i++) {
	DrawIcon $g $i $false $icons[$i]
	DrawIcon $g $i $true $icons[$i]
}
FillRound $g ([System.Drawing.Rectangle]::new(0, 198, 14, 14)) "141b1f" "7fd35b" 5
FillRound $g ([System.Drawing.Rectangle]::new(22, 196, 10, 18)) "141b1f" "7fd35b" 5
$g.Dispose()
$buttons.Save((Join-Path $buildDir "buttons.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$buttons.Dispose()

function Button($name, $lx, $ly, $idx, $disabled = $false) {
	$x = ($idx % 6) * 48
	$y = [Math]::Floor($idx / 6) * 48
	'"' + $name + '":{"location":{"x":' + $lx + ',"y":' + $ly + ',"w":0,"h":0},"up":{"imageKey":"buttons","x":' + $x + ',"y":' + $y + ',"w":40,"h":18},"down":{"imageKey":"buttons","x":' + $x + ',"y":' + ($y + 22) + ',"w":40,"h":18},"isDisabled":' + $disabled.ToString().ToLowerInvariant() + '}'
}

$mainButtons = @(
	(Button "BtnBack" 540 38 0), (Button "BtnSeekBack" 540 62 1), (Button "BtnPlay" 540 86 2),
	(Button "BtnPause" 540 86 19), (Button "BtnStop" 586 86 3), (Button "BtnSeekForward" 586 62 4),
	(Button "BtnNext" 586 38 5), (Button "BtnRandom" 18 128 6), (Button "BtnLoop" 64 128 7),
	(Button "BtnOpen" 110 128 8), (Button "BtnPlaylist" 156 128 9), (Button "BtnSetting" 202 128 10),
	(Button "BtnClose" 584 8 11), (Button "BtnCD" 248 128 12), (Button "BtnMinisize" 294 128 13)
) -join ","
$plButtons = @(
	(Button "PBtnOpen" 16 8 8), (Button "PBtnSave" 62 8 14), (Button "PBtnRemove" 108 8 15),
	(Button "PBtnUp" 248 8 16), (Button "PBtnDown" 294 8 17), (Button "PBtnClose" 304 184 11),
	(Button "PBtnClear" 304 184 18 $true)
) -join ","
$miniButtons = @(
	(Button "BtnBack" 12 30 0), (Button "BtnPlay" 58 30 2), (Button "BtnPause" 58 30 19),
	(Button "BtnStop" 104 30 3), (Button "BtnNext" 150 30 5), (Button "BtnClose" 256 8 11)
) -join ","

$json = @"
{
  "version": "2.1",
  "meta": { "name": "X-Ark Panels", "author": "MediaPlayer X-Ark", "description": "Per-form backgrounds with icon-only buttons. Mini sliders are hidden." },
  "settings": { "transparentKey": "241b2f" },
  "images": {
    "mainBackground": "main-background.png",
    "playlistBackground": "playlist-background.png",
    "miniBackground": "mini-background.png",
    "fileInfoBackground": "fileinfo-background.png",
    "buttons": "buttons.png"
  },
  "mainForm": {
    "src": { "imageKey": "mainBackground", "x": 0, "y": 0, "w": 640, "h": 164 },
    "location": { "x": 0, "y": 0, "w": 640, "h": 164 },
    "buttons": { $mainButtons },
    "sliders": {
      "SldVolume": { "src": { "imageKey": "buttons", "x": 22, "y": 196, "w": 10, "h": 18 }, "location": { "x": 500, "y": 38, "w": 22, "h": 76 }, "min": 0, "max": 200, "orientation": "vertical" },
      "SldTrack": { "src": { "imageKey": "buttons", "x": 0, "y": 198, "w": 14, "h": 14 }, "location": { "x": 258, "y": 90, "w": 220, "h": 14 }, "min": 0, "max": 100, "orientation": "horizontal" },
      "SldPan": { "src": { "imageKey": "buttons", "x": 0, "y": 198, "w": 14, "h": 14 }, "location": { "x": 258, "y": 112, "w": 220, "h": 14 }, "min": -10, "max": 10, "orientation": "horizontal" }
    },
    "spectrum": { "src": { "imageKey": "mainBackground", "x": 18, "y": 38, "w": 220, "h": 76 }, "location": { "x": 18, "y": 38, "w": 220, "h": 76 }, "color": "0b1209", "waveColorL": "7fd35b", "waveColorR": "ffb45a" },
    "waveArea": { "target": "trackbar", "mode": "mix", "exponent": 2.0, "location": { "x": 0, "y": 0, "w": 0, "h": 0 }, "colorMix": "7fd35b", "colorUnplayed": "30402a", "colorPlayed": "ffb45a" },
    "labels": {
      "LabelTitle": { "location": { "x": 258, "y": 38, "w": 220, "h": 34 }, "font": "Yu Gothic UI", "size": 12, "bold": true, "foreColor": "edf0dc", "interval": 100, "scrollEnable": true },
      "LabelTime": { "location": { "x": 392, "y": 8, "w": 150, "h": 22 }, "font": "Segoe UI", "size": 12, "bold": true, "foreColor": "ffb45a", "interval": 0, "scrollEnable": false, "align": "right" }
    }
  },
  "subForms": {
    "PlayListForm": {
      "src": { "imageKey": "playlistBackground", "x": 0, "y": 0, "w": 360, "h": 210 },
      "location": { "x": 0, "y": 0, "w": 360, "h": 210 },
      "offset": { "x": 644, "y": 0, "w": 0, "h": 0 },
      "buttons": { $plButtons },
      "grids": { "PlayListGrid": { "location": { "x": 16, "y": 40, "w": 328, "h": 150 }, "backColor": "0b1209", "foreColor": "edf0dc" } },
      "magnetic": true
    },
    "MiniPlayerForm": {
      "src": { "imageKey": "miniBackground", "x": 0, "y": 0, "w": 310, "h": 56 },
      "location": { "x": 0, "y": 0, "w": 310, "h": 56 },
      "buttons": { $miniButtons },
      "sliders": {
        "SldTrack": { "isDisabled": true },
        "SldVolume": { "isDisabled": true }
      },
      "labels": { "LabelTitle": { "location": { "x": 12, "y": 8, "w": 238, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "edf0dc", "interval": 100, "scrollEnable": true } }
    },
    "FileInfoForm": {
      "src": { "imageKey": "fileInfoBackground", "x": 0, "y": 0, "w": 460, "h": 520 },
      "location": { "x": 0, "y": 0, "w": 460, "h": 520 },
      "backColor": "141510", "foreColor": "edf0dc", "font": "Yu Gothic UI", "fontSize": 9,
      "pictures": { "picCover": { "location": { "x": 22, "y": 48, "w": 128, "h": 128 }, "borderColor": "7fd35b", "borderWidth": 1 } },
      "labels": {
        "lblTitleKey": { "location": { "x": 178, "y": 54, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblTitleVal": { "location": { "x": 268, "y": 54, "w": 158, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblArtistKey": { "location": { "x": 178, "y": 78, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblArtistVal": { "location": { "x": 268, "y": 78, "w": 158, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblAlbumKey": { "location": { "x": 178, "y": 102, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblAlbumVal": { "location": { "x": 268, "y": 102, "w": 158, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblYearKey": { "location": { "x": 178, "y": 126, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblYearVal": { "location": { "x": 268, "y": 126, "w": 158, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblTrackKey": { "location": { "x": 178, "y": 150, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblTrackVal": { "location": { "x": 268, "y": 150, "w": 158, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblFileNameKey": { "location": { "x": 34, "y": 222, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblFileNameVal": { "location": { "x": 144, "y": 222, "w": 278, "h": 42 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblFormatKey": { "location": { "x": 34, "y": 282, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblFormatVal": { "location": { "x": 144, "y": 282, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblBitKey": { "location": { "x": 34, "y": 306, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblBitVal": { "location": { "x": 144, "y": 306, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblLengthKey": { "location": { "x": 34, "y": 330, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblLengthVal": { "location": { "x": 144, "y": 330, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblSampleRateKey": { "location": { "x": 34, "y": 354, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblSampleRateVal": { "location": { "x": 144, "y": 354, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" },
        "lblChannelKey": { "location": { "x": 34, "y": 378, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "a0aa8c" },
        "lblChannelVal": { "location": { "x": 144, "y": 378, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "edf0dc" }
      }
    }
  }
}
"@

Set-Content -Path (Join-Path $buildDir "skin.json") -Value $json -Encoding UTF8
$xsk = Join-Path $skinDir "xark-panels.xsk"
if (Test-Path $xsk) { Remove-Item -LiteralPath $xsk -Force }
[IO.Compression.ZipFile]::CreateFromDirectory($buildDir, $xsk)
