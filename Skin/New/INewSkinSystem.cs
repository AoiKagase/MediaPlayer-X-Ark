using System.Collections.Generic;
using MediaPlayer_X_Ark.Skin.New.Parts;

namespace MediaPlayer_X_Ark.Skin.New
{
	public interface INewSkinSystem
	{
        /// <summary>
        /// MainFormComponents: メインフォームのコンポーネントを表すプロパティ。
        /// </summary>
        FormComponents MainForm { get; }
        /// <summary>
        /// FormKey, SpectrumComponentsのペアを格納するDictionary。
        /// </summary>
        SpectrumComponents Spectrum { get; }
        /// <summary>
        /// FormKey, WaveformComponentsのペアを格納するDictionary。
        /// </summary>
        WaveformComponents WaveForm { get; }
        /// <summary>
        /// SliderName, SliderComponentsのペアを格納するDictionary。
        /// </summary>
        Dictionary<string, SliderComponents> Sliders { get; }
        /// <summary>
        /// FormKey, FormComponentsのペアを格納するDictionary。
        /// サブフォームの定義を保持するために使用される。
        /// </summary>
        Dictionary<string, FormComponents> SubForms { get; }
		/// <summary>
		/// FormKey, ButtonName, ButtonComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, Dictionary<string, ButtonComponents>> Buttons { get; }
		/// <summary>
		/// FormKey, LabelName, LabelComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, Dictionary<string, LabelComponents>> Labels { get; }
		/// <summary>
		/// FormKey, GridName, GridComponentsのペアを格納するDictionary。
		/// </summary>
		Dictionary<string, Dictionary<string, GridComponents>> Grids { get; }
		Dictionary<string, Dictionary<string, PictureComponents>> Pictures { get; }

	}
}