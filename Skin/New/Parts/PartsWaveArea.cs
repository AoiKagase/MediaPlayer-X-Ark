using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	/// <summary>
	/// 音声全体波形の表示に関するプロパティを定義するクラス。
	/// </summary>
	public class PartsWaveArea : AbstractParts
	{
		/// <summary>
		/// トラックバーへ描画するか、指定エリアへ描画するかを定義するプロパティ。
		/// "trackbar" or "area"
		/// </summary>
		[JsonPropertyName("target")] public string Target { get; set; } = "trackbar";
		/// <summary>
		/// mix: 左右のチャンネルを混ぜて表示するモード。
		/// stereo: 左右のチャンネルを別々に表示するモード。
		/// overlay: 左右のチャンネルを重ねて表示するモード。
		/// </summary>
		[JsonPropertyName("mode")] public string Mode { get; set; } = "mix";
		/// <summary>
		/// Exponent: 波形の振幅を調整するための指数。
		/// 値が大きいほど、波形の振幅が強調される。
		/// </summary>
		[JsonPropertyName("exponent")] public float Exponent { get; set; } = 2.5f;
		/// <summary>
		/// ColorL: 左チャンネルの波形の色を定義するプロパティ。
		/// ColorR: 右チャンネルの波形の色を定義するプロパティ。
		/// ColorMix: 左右のチャンネルを混ぜて表示するモードで使用される、混ぜた波形の色を定義するプロパティ。
		/// ColorPlayed: 再生済みの部分の波形の色を定義するプロパティ。
		/// ColorUnplayed: 未再生の部分の波形の色を定義するプロパティ。
		/// </summary>
		[JsonPropertyName("colorL")] public string ColorL { get; set; } = "00CC66";
		[JsonPropertyName("colorR")] public string ColorR { get; set; } = "0066CC";
		[JsonPropertyName("colorMix")] public string ColorMix { get; set; } = "00AA88";
		[JsonPropertyName("colorPlayed")] public string ColorPlayed { get; set; } = "555555";
		[JsonPropertyName("colorUnplayed")] public string ColorUnplayed { get; set; } = "333333";

		/// <summary>
		/// target="area" の場合のみ使用
		/// Location: 波形を描画するエリアの位置とサイズを定義するプロパティ。
		/// </summary>
	}
}
