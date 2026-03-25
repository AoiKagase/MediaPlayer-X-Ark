using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin.New.Parts
{
	/// <summary>
	/// ボタンの定義を表すクラス。
	/// スキンのJSONファイルで、各ボタンの位置、サイズ、画像、状態などを定義するために使用される。
	/// </summary>
	public class PartsButtons : AbstractParts
	{
		/// <summary>
		/// "ButtonsName" : {
		///		"location": {x, y, w, h},
		///		"up" : {
		///			"image": "image_key",
		///			"src": {x, y, w, h},
		///		},
		///		"down":{
		///			"image": "image_key",
		///			"src": {x, y, w, h},
		///		},
		///		"enabled": true/false
		/// }
		/// 
		/// <summary>
		/// ボタンの状態を表すプロパティ。通常は、"up"、"down"、"hover"、"disabled"などの状態が定義される。
		/// UP: ボタンが通常の状態で表示されるときの画像と位置を定義するために使用される。
		/// </summary>
		[JsonPropertyName("up")] public SpriteRect Up { get; set; }
		/// <summary>
		/// DOWN: ボタンが押されたときの画像と位置を定義するために使用される。
		/// </summary>
		[JsonPropertyName("down")] public SpriteRect Down { get; set; }
		/// <summary>
		/// HOVER: ボタンにマウスカーソルが乗ったときの画像と位置を定義するために使用される。
		/// </summary>
		[JsonPropertyName("hover")] public SpriteRect Hover { get; set; } = null;
		/// <summary>
		/// DISABLED: ボタンが無効な状態のときの画像と位置を定義するために使用される。
		/// </summary>
		[JsonPropertyName("disabled")] public SpriteRect Disabled { get; set; } = null;
		/// <summary>
		/// Optional: ボタンの追加の状態を定義するために使用される。
		/// 例えば、特定の機能が有効なときの画像と位置を定義するために使用される。
		/// </summary>
		[JsonPropertyName("optional")] public SpriteRect Optional { get; set; } = null;
	}
}
