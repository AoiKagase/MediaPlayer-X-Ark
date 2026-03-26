using MediaPlayer_X_Ark.Skin.New.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New
{
	public class SubFormDef : AbstractParts
	{
		/// <summary>
		/// Offset: オフセットを表すプロパティ。
		/// サブフォームの位置を定義するために使用される。
		/// magneticがtrueの場合、サブフォームはメインフォームに吸着するため、
		/// オフセットはサブフォームがメインフォームからどれだけ離れて配置されるかを定義するために使用される。
		/// width/heightは未使用(Locationで定義されるため)。
		/// </summary>
		[JsonPropertyName("offset")] public Location Offset { get; set; }
		/// <summary>
		/// Buttons: ボタンを表すプロパティ。
		/// スキンのサブフォームに配置されるボタンの定義を格納するために使用される。
		/// </summary>
		[JsonPropertyName("buttons")] public Dictionary<string, PartsButtons> Buttons { get; set; }
		/// <summary>
		/// Labels: ラベルを表すプロパティ。
		/// スキンのサブフォームに配置されるラベルの定義を格納するために使用される。
		/// </summary>
		[JsonPropertyName("labels")] public Dictionary<string, PartsTextArea> Labels { get; set; }
		/// <summary>
		/// Grids: グリッドを表すプロパティ。
		/// </summary>
		[JsonPropertyName("grids")] public Dictionary<string, PartsGrids> Grids { get; set; }
		[JsonPropertyName("pictures")] public Dictionary<string, PartsPictureArea> Pictures { get; set; }
		/// <summary>
		/// Magnetic: 磁石のようにサブフォームがメインフォームに吸着するかどうかを定義するプロパティ。
		/// </summary>
		[JsonPropertyName("magnetic")] public bool Magnetic { get; set; }

        [JsonPropertyName("backColor")] public string BackColor { get; set; }
        [JsonPropertyName("foreColor")] public string ForeColor { get; set; }
        [JsonPropertyName("font")] public string Font { get; set; }
        [JsonPropertyName("fontSize")] public int FontSize { get; set; }
    }
}
