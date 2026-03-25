using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class SpriteRect
	{
		/// <summary>
		/// 画像キー。スキンの画像リストから対応する画像を取得するために使用される。
		/// </summary>
		[JsonPropertyName("imageKey")] public string ImageKey { get; set; }
		/// <summary>
		/// X座標、Y座標、幅、高さを表すプロパティ。スキンの画像から特定の部分を切り取るために使用される。
		/// </summary>
		[JsonPropertyName("x")] public int X { get; set; }
		[JsonPropertyName("y")] public int Y { get; set; }
		[JsonPropertyName("w")] public int W { get; set; }
		[JsonPropertyName("h")] public int H { get; set; }
		/// <summary>
		/// Rectangle構造体に変換するためのメソッド。
		/// X、Y、W、Hの値を使用して、新しいRectangleオブジェクトを作成し、返す。
		/// </summary>
		/// <returns></returns>
		public Rectangle ToRectangle() => new Rectangle(X, Y, W, H);
	}
}
