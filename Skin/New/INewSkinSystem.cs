using System.Collections.Generic;
using MediaPlayer_X_Ark.Skin.New.Parts;

namespace MediaPlayer_X_Ark.Skin.New
{
	public interface INewSkinSystem
	{
		FormComponents MainForm { get; }
		/// <summary>
		/// FormKey, FormComponentsのペアを格納するDictionary。
		/// サブフォームの定義を保持するために使用される。
		/// </summary>
		Dictionary<string, FormComponents> SubForms { get; }
		/// <summary>
		/// FormKey, ButtonComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, ButtonComponents> Buttons { get; }
		/// <summary>
		/// FormKey, SliderComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, SliderComponents> Sliders { get; }
		/// <summary>
		/// FormKey, SpectrumComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, SpectrumComponents> Spectrums { get; }
		/// <summary>
		/// FormKey, WaveformComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, WaveformComponents> WaveForms { get; }
		/// <summary>
		/// FormKey, LabelComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, LabelComponents> Labels { get; }
		/// <summary>
		/// FormKey, GridComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, GridComponents> Grids { get; }
	}
}