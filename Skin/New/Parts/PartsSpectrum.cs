using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class PartsSpectrum : AbstractParts
	{
		/// <summary>
		/// 色を表すプロパティ。スペクトラムの色を定義するために使用される。
		/// SRCが未指定の場合は、スペクトラム全体がこの色で塗りつぶされる。
		/// SRCが指定されている場合は、この項目は未使用で、スペクトラムの色は画像に従う。
		/// </summary>
		[JsonPropertyName("color")] public string Color { get; set; }
		/// <summary>ウェーブラインL色（RRGGBB形式）。未指定時はデフォルト（Lime）。</summary>
		[JsonPropertyName("waveColorL")] public string WaveColorL { get; set; }
		/// <summary>ウェーブラインR色（RRGGBB形式）。未指定時はデフォルト（Cyan）。</summary>
		[JsonPropertyName("waveColorR")] public string WaveColorR { get; set; }
	}
}
