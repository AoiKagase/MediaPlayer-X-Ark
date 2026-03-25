using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	/// <summary>
	/// グリッドの画像と位置を定義するために使用されるプロパティ。
	/// </summary>
	public class PartsGrids : AbstractParts
	{
		/// <summary>
		/// 背景色と前景色を表すプロパティ。
		/// グリッドの背景色と前景色を定義するために使用される。
		/// 尚、srcが指定されている場合は、グリッドの色は画像に従うため、この項目は未使用となる。
		/// </summary>
		[JsonPropertyName("backColor")] public string BackColor { get; set; }
		/// <summary>
		/// グリッドの前景色を表すプロパティ。グリッドの色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("foreColor")] public string ForeColor { get; set; }
		/// <summary>
		/// 罫線の色を表すプロパティ。グリッドの罫線の色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("lineColor")] public string LineColor { get; set; }
		/// <summary>
		/// ヘッダーの背景色を表すプロパティ。グリッドのヘッダーの背景色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("headerBackColor")] public string HeaderBackColor { get; set; }
		/// <summary>
		/// ヘッダーの前景色を表すプロパティ。グリッドのヘッダーの前景色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("headerForeColor")] public string HeaderForeColor { get; set; }
		/// <summary>
		/// ヘッダーの罫線の色を表すプロパティ。グリッドのヘッダーの罫線の色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("headerLineColor")] public string HeaderLineColor { get; set; }
	}
}
