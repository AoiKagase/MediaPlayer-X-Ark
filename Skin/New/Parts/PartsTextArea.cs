using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	public class PartsTextArea : AbstractParts
	{
		/// <summary>
		/// テキストエリアのフォント、サイズ、スタイル、色を表すプロパティ。
		/// </summary>
		[JsonPropertyName("font")] public string Font { get; set; }
		/// <summary>
		/// フォントサイズ
		/// </summary>
		[JsonPropertyName("size")] public int Size { get; set; }
		/// <summary>
		/// 太字かどうかを表すプロパティ。
		/// </summary>
		[JsonPropertyName("bold")] public bool Bold { get; set; }
		/// <summary>
		/// 斜体かどうかを表すプロパティ。
		/// </summary>
		[JsonPropertyName("italic")] public bool Italic { get; set; }
		/// <summary>
		/// テキストの前景色を表すプロパティ。テキストの色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("foreColor")] public string ForeColor { get; set; }
		/// <summary>
		/// テキストの背景色を表すプロパティ。テキストエリアの背景色を定義するために使用される。
		/// </summary>
		[JsonPropertyName("backColor")] public string BackColor { get; set; }
		/// <summary>
		/// backColor の互換表記。
		/// </summary>
		[JsonPropertyName("BackGroundColor")] public string BackGroundColor { get; set; }
		/// <summary>
		/// テキストの横方向の配置基準を表すプロパティ。
		/// left / center / right を指定する。
		/// 未指定時は left。
		/// </summary>
		[JsonPropertyName("align")] public string Align { get; set; }
		/// <summary>
		/// テキストエリアがスクロール可能かどうかを表すプロパティ。
		/// テキストがエリアのサイズを超える場合に、自動スクロールするかどうかを指定するために使用される。
		/// </summary>
		[JsonPropertyName("scrollEnable")] public bool ScrollEnable { get; set; }
		/// <summary>
		/// テキストエリアのスクロール方向を表すプロパティ。
		/// 0: 上から下へスクロール
		/// 1: 下から上へスクロール
		/// 2: 左から右へスクロール
		/// 3: 右から左へスクロール
		/// defaultは3で、右から左へスクロールすることを示す。
		/// スクロールが有効な場合に、テキストがどの方向にスクロールするかを指定するために使用される。
		/// </summary>
		[JsonPropertyName("scrollVector")] public int ScrollVector { get; set; } = 3;
		/// <summary>
		/// テキストエリアのスクロール間隔を表すプロパティ。
		/// スクロールが有効な場合に、テキストがどのくらいの速さでスクロールするかを指定するために使用される。
		/// </summary>
		[JsonPropertyName("interval")] public int Interval { get; set; }
		/// <summary>
		/// テキストエリアに表示されるテキストを表すプロパティ。
		/// テキストエリアに表示される内容を定義するために使用される。
		/// {0}を使用して、動的にテキストを挿入することができる。例えば、{0}を現在の曲名に置き換えることができる。
		/// 上記を指定しない場合は指定されたテキストがそのまま表示される。
		/// </summary>
		[JsonPropertyName("additionalValue")] public string AdditionalValue { get; set; }
	}
}
