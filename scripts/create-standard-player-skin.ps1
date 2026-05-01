Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot
$buildDir = Join-Path $repo "Resources\Skins\Standard-Player"
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

function SavePng($name, $w, $h, $draw) {
	$bmp = [System.Drawing.Bitmap]::new($w, $h)
	$g = [System.Drawing.Graphics]::FromImage($bmp)
	$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
	$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
	$g.Clear((C "ff00ff"))
	& $draw $g
	$g.Dispose()
	$bmp.Save((Join-Path $buildDir $name), [System.Drawing.Imaging.ImageFormat]::Png)
	$bmp.Dispose()
}

SavePng "background.png" 720 220 {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(0, 0, 720, 220)) "f3f5f7" "b8c0c8" 12
	FillRound $g ([System.Drawing.Rectangle]::new(18, 18, 152, 152)) "dfe5ea" "b8c0c8" 8
	FillRound $g ([System.Drawing.Rectangle]::new(190, 26, 330, 42)) "ffffff" "d2d8de" 7
	FillRound $g ([System.Drawing.Rectangle]::new(190, 86, 330, 56)) "ffffff" "d2d8de" 7
	FillRound $g ([System.Drawing.Rectangle]::new(540, 26, 24, 116)) "ffffff" "d2d8de" 8
	FillRound $g ([System.Drawing.Rectangle]::new(190, 166, 354, 12)) "dfe5ea" "c6cdd4" 6
	Text $g "Media Player" 202 34 230 24 11 "303840" $true
	Text $g "00:00 / 00:00" 386 34 118 24 10 "607080" $false "Far"
	$p = [System.Drawing.Pen]::new((C "5b8def"), 2)
	for ($i = 0; $i -lt 28; $i++) {
		$x = 210 + ($i * 10)
		$h = 8 + (($i * 11) % 34)
		$g.DrawLine($p, $x, 130, $x, 130 - $h)
	}
	$p.Dispose()
}

SavePng "playlist-background.png" 420 260 {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(0, 0, 420, 260)) "f3f5f7" "b8c0c8" 12
	Text $g "Playlist" 18 10 160 24 11 "303840" $true
	FillRound $g ([System.Drawing.Rectangle]::new(18, 46, 384, 194)) "ffffff" "d2d8de" 7
}

SavePng "mini-background.png" 360 72 {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(0, 0, 360, 72)) "f3f5f7" "b8c0c8" 12
	FillRound $g ([System.Drawing.Rectangle]::new(12, 12, 160, 22)) "ffffff" "d2d8de" 7
}

SavePng "fileinfo-background.png" 460 520 {
	param($g)
	FillRound $g ([System.Drawing.Rectangle]::new(0, 0, 460, 520)) "f3f5f7" "b8c0c8" 12
	Text $g "File Information" 18 10 220 24 11 "303840" $true
	FillRound $g ([System.Drawing.Rectangle]::new(22, 50, 128, 128)) "dfe5ea" "b8c0c8" 8
	FillRound $g ([System.Drawing.Rectangle]::new(170, 50, 266, 128)) "ffffff" "d2d8de" 8
	FillRound $g ([System.Drawing.Rectangle]::new(22, 210, 414, 278)) "ffffff" "d2d8de" 8
}

function DrawButton($g, $idx, $down, $kind) {
	$x = ($idx % 6) * 54
	$y = [Math]::Floor($idx / 6) * 56 + ($(if ($down) { 26 } else { 0 }))
	$fill = if ($down) { "d8e5ff" } else { "ffffff" }
	$stroke = if ($down) { "356fd6" } else { "aeb8c2" }
	$fg = if ($down) { "1f4fae" } else { "303840" }
	FillRound $g ([System.Drawing.Rectangle]::new($x + 1, $y + 1, 44, 22)) $fill $stroke 7
	$b = [System.Drawing.SolidBrush]::new((C $fg))
	$p = [System.Drawing.Pen]::new((C $fg), 2)
	switch ($kind) {
		"prev" { $g.FillPolygon($b,@([Drawing.Point]::new($x+27,$y+6),[Drawing.Point]::new($x+17,$y+12),[Drawing.Point]::new($x+27,$y+18))); $g.DrawLine($p,$x+15,$y+6,$x+15,$y+18) }
		"back" { $g.FillPolygon($b,@([Drawing.Point]::new($x+28,$y+6),[Drawing.Point]::new($x+16,$y+12),[Drawing.Point]::new($x+28,$y+18))) }
		"play" { $g.FillPolygon($b,@([Drawing.Point]::new($x+18,$y+6),[Drawing.Point]::new($x+18,$y+18),[Drawing.Point]::new($x+30,$y+12))) }
		"pause" { $g.FillRectangle($b,$x+17,$y+7,5,11); $g.FillRectangle($b,$x+26,$y+7,5,11) }
		"stop" { $g.FillRectangle($b,$x+18,$y+7,12,12) }
		"fwd" { $g.FillPolygon($b,@([Drawing.Point]::new($x+16,$y+6),[Drawing.Point]::new($x+28,$y+12),[Drawing.Point]::new($x+16,$y+18))) }
		"next" { $g.FillPolygon($b,@([Drawing.Point]::new($x+16,$y+6),[Drawing.Point]::new($x+26,$y+12),[Drawing.Point]::new($x+16,$y+18))); $g.DrawLine($p,$x+29,$y+6,$x+29,$y+18) }
		"random" { $g.DrawLine($p,$x+14,$y+9,$x+31,$y+9); $g.DrawLine($p,$x+14,$y+16,$x+31,$y+16); $g.DrawLine($p,$x+28,$y+6,$x+33,$y+9); $g.DrawLine($p,$x+28,$y+13,$x+33,$y+16) }
		"loop" { $g.DrawArc($p,$x+13,$y+7,20,11,20,300); $g.FillPolygon($b,@([Drawing.Point]::new($x+31,$y+10),[Drawing.Point]::new($x+36,$y+12),[Drawing.Point]::new($x+31,$y+15))) }
		"open" { $g.DrawRectangle($p,$x+14,$y+10,18,9); $g.DrawLine($p,$x+14,$y+10,$x+20,$y+6); $g.DrawLine($p,$x+20,$y+6,$x+32,$y+6) }
		"list" { for($i=0;$i -lt 3;$i++){ $g.FillRectangle($b,$x+14,$y+7+$i*5,4,2); $g.DrawLine($p,$x+21,$y+8+$i*5,$x+33,$y+8+$i*5) } }
		"gear" { $g.DrawEllipse($p,$x+16,$y+6,14,14); $g.FillEllipse($b,$x+21,$y+11,4,4) }
		"close" { $g.DrawLine($p,$x+17,$y+7,$x+30,$y+18); $g.DrawLine($p,$x+30,$y+7,$x+17,$y+18) }
		"cd" { $g.DrawEllipse($p,$x+15,$y+5,16,16); $g.FillEllipse($b,$x+21,$y+11,4,4) }
		"mini" { $g.DrawRectangle($p,$x+14,$y+7,18,11); $g.DrawLine($p,$x+20,$y+16,$x+32,$y+16) }
		"save" { $g.DrawRectangle($p,$x+15,$y+6,16,14); $g.FillRectangle($b,$x+20,$y+7,8,4) }
		"trash" { $g.DrawRectangle($p,$x+17,$y+8,12,11); $g.DrawLine($p,$x+16,$y+6,$x+30,$y+6) }
		"up" { $g.FillPolygon($b,@([Drawing.Point]::new($x+23,$y+6),[Drawing.Point]::new($x+15,$y+17),[Drawing.Point]::new($x+31,$y+17))) }
		"down" { $g.FillPolygon($b,@([Drawing.Point]::new($x+15,$y+7),[Drawing.Point]::new($x+31,$y+7),[Drawing.Point]::new($x+23,$y+18))) }
		"clear" { $g.DrawRectangle($p,$x+15,$y+8,16,10); $g.DrawLine($p,$x+18,$y+10,$x+29,$y+16) }
	}
	$b.Dispose(); $p.Dispose()
}

$buttons = [System.Drawing.Bitmap]::new(324, 248)
$g = [System.Drawing.Graphics]::FromImage($buttons)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.Clear([System.Drawing.Color]::Transparent)
$icons = @("prev","back","play","stop","fwd","next","random","loop","open","list","gear","close","cd","mini","save","trash","up","down","clear","pause")
for ($i = 0; $i -lt $icons.Count; $i++) {
	DrawButton $g $i $false $icons[$i]
	DrawButton $g $i $true $icons[$i]
}
FillRound $g ([System.Drawing.Rectangle]::new(0, 228, 16, 16)) "ffffff" "aeb8c2" 5
FillRound $g ([System.Drawing.Rectangle]::new(24, 224, 12, 22)) "ffffff" "aeb8c2" 5
$g.Dispose()
$buttons.Save((Join-Path $buildDir "buttons.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$buttons.Dispose()

function B($name, $lx, $ly, $idx, $disabled = $false) {
	$x = ($idx % 6) * 54
	$y = [Math]::Floor($idx / 6) * 56
	'"' + $name + '":{"location":{"x":' + $lx + ',"y":' + $ly + ',"w":0,"h":0},"up":{"imageKey":"buttons","x":' + $x + ',"y":' + $y + ',"w":44,"h":22},"down":{"imageKey":"buttons","x":' + $x + ',"y":' + ($y + 26) + ',"w":44,"h":22},"isDisabled":' + $disabled.ToString().ToLowerInvariant() + '}'
}

$mainButtons = @(
	(B "BtnBack" 208 184 0), (B "BtnSeekBack" 258 184 1), (B "BtnPlay" 308 184 2),
	(B "BtnPause" 308 184 19), (B "BtnStop" 358 184 3), (B "BtnSeekForward" 408 184 4),
	(B "BtnNext" 458 184 5), (B "BtnRandom" 18 184 6), (B "BtnLoop" 68 184 7),
	(B "BtnOpen" 560 48 8), (B "BtnPlaylist" 610 48 9), (B "BtnSetting" 560 84 10),
	(B "BtnClose" 666 12 11), (B "BtnCD" 610 84 12), (B "BtnMinisize" 560 120 13)
) -join ","
$plButtons = @(
	(B "PBtnOpen" 18 12 8), (B "PBtnSave" 68 12 14), (B "PBtnRemove" 118 12 15),
	(B "PBtnUp" 268 12 16), (B "PBtnDown" 318 12 17), (B "PBtnClose" 358 12 11),
	(B "PBtnClear" 358 12 18 $true)
) -join ","
$miniButtons = @(
	(B "BtnBack" 184 38 0), (B "BtnPlay" 226 38 2), (B "BtnPause" 226 38 19),
	(B "BtnStop" 268 38 3), (B "BtnNext" 310 38 5), (B "BtnClose" 308 8 11)
) -join ","

$json = @"
{
  "version": "2.1",
  "meta": { "name": "Standard Player", "author": "MediaPlayer X-Ark", "description": "General media player layout with light neutral controls." },
  "settings": { "transparentKey": "ff00ff" },
  "images": {
    "background": "background.png",
    "playlistBackground": "playlist-background.png",
    "miniBackground": "mini-background.png",
    "fileInfoBackground": "fileinfo-background.png",
    "buttons": "buttons.png"
  },
  "mainForm": {
    "src": { "imageKey": "background", "x": 0, "y": 0, "w": 720, "h": 220 },
    "location": { "x": 0, "y": 0, "w": 720, "h": 220 },
    "buttons": { $mainButtons },
    "sliders": {
      "SldVolume": { "src": { "imageKey": "buttons", "x": 24, "y": 224, "w": 12, "h": 22 }, "location": { "x": 540, "y": 26, "w": 24, "h": 116 }, "min": 0, "max": 200, "orientation": "vertical" },
      "SldTrack": { "src": { "imageKey": "buttons", "x": 0, "y": 228, "w": 16, "h": 16 }, "location": { "x": 190, "y": 166, "w": 354, "h": 16 }, "min": 0, "max": 100, "orientation": "horizontal" },
      "SldPan": { "src": { "imageKey": "buttons", "x": 0, "y": 228, "w": 16, "h": 16 }, "location": { "x": 18, "y": 166, "w": 152, "h": 16 }, "min": -10, "max": 10, "orientation": "horizontal" }
    },
    "spectrum": { "src": { "imageKey": "background", "x": 190, "y": 86, "w": 330, "h": 56 }, "location": { "x": 190, "y": 86, "w": 330, "h": 56 }, "color": "ffffff", "waveColorL": "5b8def", "waveColorR": "48a868" },
    "waveArea": { "target": "trackbar", "mode": "mix", "exponent": 2.0, "location": { "x": 0, "y": 0, "w": 0, "h": 0 }, "colorMix": "5b8def", "colorUnplayed": "c6cdd4", "colorPlayed": "48a868" },
    "labels": {
      "LabelTitle": { "location": { "x": 202, "y": 34, "w": 230, "h": 24 }, "font": "Yu Gothic UI", "size": 11, "bold": true, "foreColor": "303840", "interval": 100, "scrollEnable": true },
      "LabelTime": { "location": { "x": 386, "y": 34, "w": 118, "h": 24 }, "font": "Segoe UI", "size": 10, "bold": true, "foreColor": "607080", "interval": 0, "scrollEnable": false, "align": "right" }
    }
  },
  "subForms": {
    "PlayListForm": {
      "src": { "imageKey": "playlistBackground", "x": 0, "y": 0, "w": 420, "h": 260 },
      "location": { "x": 0, "y": 0, "w": 420, "h": 260 },
      "offset": { "x": 724, "y": 0, "w": 0, "h": 0 },
      "buttons": { $plButtons },
      "grids": { "PlayListGrid": { "location": { "x": 18, "y": 46, "w": 384, "h": 194 }, "backColor": "ffffff", "foreColor": "303840" } },
      "magnetic": true
    },
    "MiniPlayerForm": {
      "src": { "imageKey": "miniBackground", "x": 0, "y": 0, "w": 360, "h": 72 },
      "location": { "x": 0, "y": 0, "w": 360, "h": 72 },
      "buttons": { $miniButtons },
      "sliders": {
        "SldTrack": { "src": { "imageKey": "buttons", "x": 0, "y": 228, "w": 16, "h": 16 }, "location": { "x": 12, "y": 46, "w": 140, "h": 16 }, "min": 0, "max": 100, "orientation": "horizontal" },
        "SldVolume": { "src": { "imageKey": "buttons", "x": 0, "y": 228, "w": 16, "h": 16 }, "location": { "x": 160, "y": 46, "w": 70, "h": 16 }, "min": 0, "max": 200, "orientation": "horizontal" }
      },
      "labels": { "LabelTitle": { "location": { "x": 16, "y": 14, "w": 152, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "303840", "interval": 100, "scrollEnable": true } }
    },
    "FileInfoForm": {
      "src": { "imageKey": "fileInfoBackground", "x": 0, "y": 0, "w": 460, "h": 520 },
      "location": { "x": 0, "y": 0, "w": 460, "h": 520 },
      "backColor": "f3f5f7", "foreColor": "303840", "font": "Yu Gothic UI", "fontSize": 9,
      "pictures": { "picCover": { "location": { "x": 22, "y": 50, "w": 128, "h": 128 }, "borderColor": "b8c0c8", "borderWidth": 1 } },
      "labels": {
        "lblTitleKey": { "location": { "x": 178, "y": 56, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblTitleVal": { "location": { "x": 268, "y": 56, "w": 154, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblArtistKey": { "location": { "x": 178, "y": 80, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblArtistVal": { "location": { "x": 268, "y": 80, "w": 154, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblAlbumKey": { "location": { "x": 178, "y": 104, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblAlbumVal": { "location": { "x": 268, "y": 104, "w": 154, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblYearKey": { "location": { "x": 178, "y": 128, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblYearVal": { "location": { "x": 268, "y": 128, "w": 154, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblTrackKey": { "location": { "x": 178, "y": 152, "w": 86, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblTrackVal": { "location": { "x": 268, "y": 152, "w": 154, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblFileNameKey": { "location": { "x": 34, "y": 226, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblFileNameVal": { "location": { "x": 144, "y": 226, "w": 278, "h": 42 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblFormatKey": { "location": { "x": 34, "y": 286, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblFormatVal": { "location": { "x": 144, "y": 286, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblBitKey": { "location": { "x": 34, "y": 310, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblBitVal": { "location": { "x": 144, "y": 310, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblLengthKey": { "location": { "x": 34, "y": 334, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblLengthVal": { "location": { "x": 144, "y": 334, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblSampleRateKey": { "location": { "x": 34, "y": 358, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblSampleRateVal": { "location": { "x": 144, "y": 358, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" },
        "lblChannelKey": { "location": { "x": 34, "y": 382, "w": 106, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "bold": true, "foreColor": "607080" },
        "lblChannelVal": { "location": { "x": 144, "y": 382, "w": 278, "h": 18 }, "font": "Yu Gothic UI", "size": 9, "foreColor": "303840" }
      }
    }
  }
}
"@

Set-Content -Path (Join-Path $buildDir "skin.json") -Value $json -Encoding UTF8
$xsk = Join-Path $skinDir "standard-player.xsk"
if (Test-Path $xsk) { Remove-Item -LiteralPath $xsk -Force }
[IO.Compression.ZipFile]::CreateFromDirectory($buildDir, $xsk)
