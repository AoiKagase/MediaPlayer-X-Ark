using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class Location
	{
		/// <summary>
		/// 座標とサイズを表すプロパティ。X座標、Y座標、幅、高さを表すプロパティ。
		/// スキンの位置とサイズを定義するために使用される。
		/// スキンの要素が画面上のどこに配置され、どのくらいのスペースを占めるかを指定するために使用される。
		/// </summary>
		[JsonPropertyName("x")] public int X { get; set; }
		[JsonPropertyName("y")] public int Y { get; set; }
		[JsonPropertyName("w")] public int W { get; set; }
		[JsonPropertyName("h")] public int H { get; set; }
	}
}
