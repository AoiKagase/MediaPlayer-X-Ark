Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot
$buildDir = Join-Path $repo "Resources\Skins\X-Ark-Deck"
$skinDir = Join-Path $repo "Resources\Skins"

New-Item -ItemType Directory -Force -Path $buildDir | Out-Null

function ColorFromHex($hex) {
	$hex = $hex.TrimStart("#")
	return [System.Drawing.Color]::FromArgb(
		[Convert]::ToInt32($hex.Substring(0, 2), 16),
		[Convert]::ToInt32($hex.Substring(2, 2), 16),
		[Convert]::ToInt32($hex.Substring(4, 2), 16))
}

function FillRound($g, $rect, $radius, $fill, $stroke) {
	$path = [System.Drawing.Drawing2D.GraphicsPath]::new()
	$d = $radius * 2
	$path.AddArc($rect.X, $rect.Y, $d, $d, 180, 90)
	$path.AddArc($rect.Right - $d, $rect.Y, $d, $d, 270, 90)
	$path.AddArc($rect.Right - $d, $rect.Bottom - $d, $d, $d, 0, 90)
	$path.AddArc($rect.X, $rect.Bottom - $d, $d, $d, 90, 90)
	$path.CloseFigure()
	$b = [System.Drawing.SolidBrush]::new((ColorFromHex $fill))
	$p = [System.Drawing.Pen]::new((ColorFromHex $stroke), 1)
	$g.FillPath($b, $path)
	$g.DrawPath($p, $path)
	$b.Dispose()
	$p.Dispose()
	$path.Dispose()
}

function DrawText($g, $text, $x, $y, $w, $h, $size, $color, $bold = $false, $align = "Near") {
	$style = if ($bold) { [System.Drawing.FontStyle]::Bold } else { [System.Drawing.FontStyle]::Regular }
	$font = [System.Drawing.Font]::new("Segoe UI", $size, $style, [System.Drawing.GraphicsUnit]::Point)
	$brush = [System.Drawing.SolidBrush]::new((ColorFromHex $color))
	$format = [System.Drawing.StringFormat]::new()
	$format.Alignment = [System.Drawing.StringAlignment]::$align
	$format.LineAlignment = [System.Drawing.StringAlignment]::Center
	$g.DrawString($text, $font, $brush, [System.Drawing.RectangleF]::new($x, $y, $w, $h), $format)
	$format.Dispose()
	$brush.Dispose()
	$font.Dispose()
}

function DrawButton($g, $x, $y, $w, $h, $text, $down) {
	$fill = if ($down) { "26353a" } else { "171d22" }
	$stroke = if ($down) { "f2d05c" } else { "49d8e8" }
	$color = if ($down) { "ffffff" } else { "dce7ea" }
	FillRound $g ([System.Drawing.Rectangle]::new($x + 1, $y + 1, $w - 2, $h - 2)) 7 $fill $stroke
	DrawText $g $text $x $y $w $h 8 $color $true "Center"
}

$bg = [System.Drawing.Bitmap]::new(760, 720)
$g = [System.Drawing.Graphics]::FromImage($bg)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
$g.Clear((ColorFromHex "202030"))

FillRound $g ([System.Drawing.Rectangle]::new(0, 0, 700, 140)) 10 "10161b" "2b3940"
DrawText $g "X-ARK DECK" 18 8 160 22 11 "49d8e8" $true
DrawText $g "MediaPlayer X-Ark" 540 8 140 22 8 "7d9298" $false "Far"
FillRound $g ([System.Drawing.Rectangle]::new(18, 38, 256, 70)) 8 "071014" "24343a"
FillRound $g ([System.Drawing.Rectangle]::new(296, 38, 210, 30)) 6 "071014" "1f2b31"
FillRound $g ([System.Drawing.Rectangle]::new(296, 82, 210, 12)) 5 "071014" "24343a"
FillRound $g ([System.Drawing.Rectangle]::new(526, 38, 20, 70)) 5 "071014" "24343a"
$pen = [System.Drawing.Pen]::new((ColorFromHex "49d8e8"), 2)
for ($i = 0; $i -lt 28; $i++) {
	$x = 32 + ($i * 8)
	$h = 12 + (($i * 13) % 45)
	$g.DrawLine($pen, $x, 96, $x, 96 - $h)
}
$pen.Dispose()

FillRound $g ([System.Drawing.Rectangle]::new(0, 180, 390, 220)) 10 "10161b" "2b3940"
DrawText $g "QUEUE" 18 188 90 22 10 "49d8e8" $true
FillRound $g ([System.Drawing.Rectangle]::new(18, 218, 354, 160)) 6 "071014" "1f2b31"

FillRound $g ([System.Drawing.Rectangle]::new(0, 430, 480, 540)) 10 "10161b" "2b3940"
DrawText $g "FILE DETAILS" 18 438 160 22 10 "49d8e8" $true
FillRound $g ([System.Drawing.Rectangle]::new(22, 478, 128, 128)) 8 "071014" "24343a"

FillRound $g ([System.Drawing.Rectangle]::new(420, 180, 420, 92)) 10 "10161b" "2b3940"
DrawText $g "COMPACT DECK" 438 188 180 22 10 "49d8e8" $true

$g.Dispose()
$bg.Save((Join-Path $buildDir "background.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$bg.Dispose()

$buttons = [System.Drawing.Bitmap]::new(520, 240)
$g = [System.Drawing.Graphics]::FromImage($buttons)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
$g.Clear([System.Drawing.Color]::Transparent)
$names = @("PREV","-10","PLAY","STOP","+10","NEXT","RAND","LOOP","OPEN","QUEUE","SET","CLOSE","CD","MINI","SAVE","DEL","UP","DOWN","CLEAR","PAUSE")
for ($i = 0; $i -lt $names.Count; $i++) {
	$x = ($i % 5) * 96
	$y = [Math]::Floor($i / 5) * 52
	DrawButton $g $x $y 86 20 $names[$i] $false
	DrawButton $g $x ($y + 24) 86 20 $names[$i] $true
}
DrawButton $g 0 216 16 16 "S" $false
DrawButton $g 24 216 12 20 "S" $false
$g.Dispose()
$buttons.Save((Join-Path $buildDir "buttons.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$buttons.Dispose()

function BtnJson($name, $lx, $ly, $idx, $disabled = $false) {
	$x = ($idx % 5) * 96
	$y = [Math]::Floor($idx / 5) * 52
	return '"' + $name + '":{"location":{"x":' + $lx + ',"y":' + $ly + ',"w":0,"h":0},"up":{"imageKey":"button","x":' + $x + ',"y":' + $y + ',"w":86,"h":20},"down":{"imageKey":"button","x":' + $x + ',"y":' + ($y + 24) + ',"w":86,"h":20},"isDisabled":' + ($disabled.ToString().ToLowerInvariant()) + '}'
}

$mainButtons = @(
	(BtnJson "BtnBack" 560 34 0),
	(BtnJson "BtnSeekBack" 560 58 1),
	(BtnJson "BtnPlay" 560 82 2),
	(BtnJson "BtnPause" 560 82 19),
	(BtnJson "BtnStop" 652 82 3),
	(BtnJson "BtnSeekForward" 652 58 4),
	(BtnJson "BtnNext" 652 34 5),
	(BtnJson "BtnRandom" 18 114 6),
	(BtnJson "BtnLoop" 110 114 7),
	(BtnJson "BtnOpen" 202 114 8),
	(BtnJson "BtnPlaylist" 294 114 9),
	(BtnJson "BtnSetting" 386 114 10),
	(BtnJson "BtnClose" 606 8 11),
	(BtnJson "BtnCD" 478 114 12),
	(BtnJson "BtnMinisize" 606 114 13)
) -join ","

$playlistButtons = @(
	(BtnJson "PBtnOpen" 18 188 8),
	(BtnJson "PBtnSave" 110 188 14),
	(BtnJson "PBtnRemove" 202 188 15),
	(BtnJson "PBtnUp" 242 188 16),
	(BtnJson "PBtnDown" 286 188 17),
	(BtnJson "PBtnClose" 334 188 11),
	(BtnJson "PBtnClear" 334 188 18 $true)
) -join ","

$miniButtons = @(
	(BtnJson "BtnBack" 438 220 0),
	(BtnJson "BtnPlay" 530 220 2),
	(BtnJson "BtnPause" 530 220 19),
	(BtnJson "BtnStop" 622 220 3),
	(BtnJson "BtnNext" 714 220 5),
	(BtnJson "BtnClose" 746 188 11)
) -join ","

$json = @"
{
  "version": "2.1",
  "meta": { "name": "X-Ark Deck", "author": "MediaPlayer X-Ark", "description": "A deck-style skin built independently from bbbs layout." },
  "settings": { "transparentKey": "202030" },
  "images": { "background": "background.png", "button": "buttons.png" },
  "mainForm": {
    "src": { "imageKey": "background", "x": 0, "y": 0, "w": 700, "h": 140 },
    "location": { "x": 0, "y": 0, "w": 700, "h": 140 },
    "buttons": { $mainButtons },
    "sliders": {
      "SldVolume": { "src": { "imageKey": "button", "x": 24, "y": 216, "w": 12, "h": 20 }, "location": { "x": 526, "y": 38, "w": 20, "h": 70 }, "min": 0, "max": 200, "orientation": "vertical" },
      "SldTrack": { "src": { "imageKey": "button", "x": 0, "y": 216, "w": 16, "h": 16 }, "location": { "x": 296, "y": 82, "w": 210, "h": 16 }, "min": 0, "max": 100, "orientation": "horizontal" },
      "SldPan": { "src": { "imageKey": "button", "x": 0, "y": 216, "w": 16, "h": 16 }, "location": { "x": 296, "y": 100, "w": 210, "h": 16 }, "min": -10, "max": 10, "orientation": "horizontal" }
    },
    "spectrum": { "src": { "imageKey": "background", "x": 18, "y": 38, "w": 256, "h": 70 }, "location": { "x": 18, "y": 38, "w": 256, "h": 70 }, "color": "071014", "waveColorL": "49d8e8", "waveColorR": "f2d05c" },
    "waveArea": { "target": "trackbar", "mode": "mix", "exponent": 2.0, "location": { "x": 0, "y": 0, "w": 0, "h": 0 }, "colorMix": "49d8e8", "colorUnplayed": "24343a", "colorPlayed": "f2d05c" },
    "pictures": { "picCover": { "location": { "x": 296, "y": 38, "w": 72, "h": 72 }, "borderColor": "49d8e8", "borderWidth": 1, "cornerRadius": 10 } },
    "labels": {
      "LabelTitle": { "location": { "x": 382, "y": 38, "w": 124, "h": 30 }, "font": "Yu Gothic UI", "size": 12, "bold": true, "foreColor": "dce7ea", "interval": 100, "scrollEnable": true },
      "LabelTime": { "location": { "x": 382, "y": 76, "w": 124, "h": 22 }, "font": "Segoe UI", "size": 12, "bold": true, "foreColor": "f2d05c", "interval": 0, "scrollEnable": false, "align": "right" }
    }
  },
  "subForms": {
    "PlayListForm": {
      "src": { "imageKey": "background", "x": 0, "y": 180, "w": 390, "h": 220 },
      "location": { "x": 0, "y": 0, "w": 390, "h": 220 },
      "offset": { "x": 704, "y": 0, "w": 0, "h": 0 },
      "buttons": { $playlistButtons },
      "grids": { "PlayListGrid": { "location": { "x": 18, "y": 218, "w": 354, "h": 160 }, "backColor": "071014", "foreColor": "dce7ea" } },
      "magnetic": true
    },
    "MiniPlayerForm": {
      "src": { "imageKey": "background", "x": 420, "y": 180, "w": 420, "h": 92 },
      "location": { "x": 0, "y": 0, "w": 420, "h": 92 },
      "buttons": { $miniButtons },
      "sliders": {
        "SldTrack": { "src": { "imageKey": "button", "x": 0, "y": 216, "w": 16, "h": 16 }, "location": { "x": 438, "y": 246, "w": 238, "h": 16 }, "min": 0, "max": 100, "orientation": "horizontal" },
        "SldVolume": { "src": { "imageKey": "button", "x": 0, "y": 216, "w": 16, "h": 16 }, "location": { "x": 688, "y": 246, "w": 110, "h": 16 }, "min": 0, "max": 200, "orientation": "horizontal" }
      },
      "labels": { "LabelTitle": { "location": { "x": 438, "y": 188, "w": 290, "h": 22 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "dce7ea", "interval": 100, "scrollEnable": true } }
    },
    "FileInfoForm": {
      "src": { "imageKey": "background", "x": 0, "y": 430, "w": 480, "h": 540 },
      "location": { "x": 0, "y": 0, "w": 480, "h": 540 },
      "backColor": "10161b",
      "foreColor": "dce7ea",
      "font": "Yu Gothic UI",
      "fontSize": 9,
      "pictures": { "picCover": { "location": { "x": 22, "y": 48, "w": 128, "h": 128 }, "borderColor": "49d8e8", "borderWidth": 1, "cornerRadius": 12 } },
      "labels": {
        "lblTitleKey": { "location": { "x": 176, "y": 52, "w": 92, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblTitleVal": { "location": { "x": 272, "y": 52, "w": 188, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblArtistKey": { "location": { "x": 176, "y": 76, "w": 92, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblArtistVal": { "location": { "x": 272, "y": 76, "w": 188, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblAlbumKey": { "location": { "x": 176, "y": 100, "w": 92, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblAlbumVal": { "location": { "x": 272, "y": 100, "w": 188, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblYearKey": { "location": { "x": 176, "y": 124, "w": 92, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblYearVal": { "location": { "x": 272, "y": 124, "w": 188, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblTrackKey": { "location": { "x": 176, "y": 148, "w": 92, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblTrackVal": { "location": { "x": 272, "y": 148, "w": 188, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblFileNameKey": { "location": { "x": 22, "y": 204, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblFileNameVal": { "location": { "x": 136, "y": 204, "w": 324, "h": 42 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblFormatKey": { "location": { "x": 22, "y": 264, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblFormatVal": { "location": { "x": 136, "y": 264, "w": 324, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblBitKey": { "location": { "x": 22, "y": 288, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblBitVal": { "location": { "x": 136, "y": 288, "w": 324, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblLengthKey": { "location": { "x": 22, "y": 312, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblLengthVal": { "location": { "x": 136, "y": 312, "w": 324, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblSampleRateKey": { "location": { "x": 22, "y": 336, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblSampleRateVal": { "location": { "x": 136, "y": 336, "w": 324, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" },
        "lblChannelKey": { "location": { "x": 22, "y": 360, "w": 108, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "7d9298" },
        "lblChannelVal": { "location": { "x": 136, "y": 360, "w": 324, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "dce7ea" }
      }
    }
  }
}
"@

Set-Content -Path (Join-Path $buildDir "skin.json") -Value $json -Encoding UTF8
$xsk = Join-Path $skinDir "xark-deck.xsk"
if (Test-Path $xsk) { Remove-Item -LiteralPath $xsk -Force }
[IO.Compression.ZipFile]::CreateFromDirectory($buildDir, $xsk)
