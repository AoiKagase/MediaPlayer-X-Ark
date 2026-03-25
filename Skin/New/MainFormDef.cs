using MediaPlayer_X_Ark.Skin.New.Parts;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediaPlayer_X_Ark.Skin.New
{
	public class MainFormDef : AbstractParts
	{
		/// <summary>
		/// Location: メインフォームの位置とサイズを定義するために使用されるプロパティ。
		/// メインフォームなのでX,Yは0,0で固定されるが、幅と高さはスキンの画像に合わせて指定する必要がある。
		/// </summary>
		// [JsonPropertyName("location")] public Parts.Location Location { get; set; }

		/// <summary>
		/// Buttons: メインフォームに配置されるボタンを定義するために使用されるプロパティ。
		/// </summary>
		[JsonPropertyName("buttons")] public Dictionary<string, PartsButtons> Buttons { get; set; }
		/// <summary>
		/// Labels: メインフォームに配置されるテキストエリアを定義するために使用されるプロパティ。
		/// </summary>
		[JsonPropertyName("labels")] public Dictionary<string, PartsTextArea> Labels { get; set; }
		/// <summary>
		/// Sliders: メインフォームに配置されるスライダーを定義するために使用されるプロパティ。 
		/// </summary>
		[JsonPropertyName("sliders")] public Dictionary<string, PartsSliders> Sliders { get; set; }
		/// <summary>
		/// Spectrum: メインフォームに配置されるスペクトラムを定義するために使用されるプロパティ。
		/// </summary>
		[JsonPropertyName("spectrum")] public PartsSpectrum Spectrum { get; set; }
		/// <summary>
		/// WaveArea: メインフォームに配置される波形エリアを定義するために使用されるプロパティ。
		/// </summary>
		[JsonPropertyName("wavearea")] public PartsSpectrum WaveArea { get; set; } = null;
	}
}
