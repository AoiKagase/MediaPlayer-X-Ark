using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class PartsSliders : AbstractParts
	{
		/// <summary>
		/// MinとMax: スライダーの最小値と最大値を定義するために使用されるプロパティ。
		/// 内部定義するため、不要かもしれないが、スライダーの範囲を指定するために使用される。
		/// </summary>
		[JsonPropertyName("min")] public int Min { get; set; }
		[JsonPropertyName("max")] public int Max { get; set; }
		/// <summary>
		/// Orientation: スライダーの向きを定義するために使用されるプロパティ。
		/// Horizontal: スライダーが水平に配置されることを示す値。
		/// Vertical: スライダーが垂直に配置されることを示す値。
		/// </summary>
		[JsonPropertyName("orientation")] public string Orientation { get; set; }
	}
}
