using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class AbstractParts
	{
		/// <summary>
		/// 画像と位置を表すプロパティ。スキンの画像から特定の部分を切り取るために使用される。
		/// </summary>
		[JsonPropertyName("src")] public SpriteRect Src { get; set; }
		/// <summary>
		/// 位置とサイズを表すプロパティ。X座標、Y座標、幅、高さを表すプロパティ。
		/// </summary>
		[JsonPropertyName("location")] public Location Location { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("isDisabled")] public bool IsDisabled { get; set; }
	}
}
