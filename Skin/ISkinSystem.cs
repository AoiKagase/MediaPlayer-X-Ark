using System.Collections.Generic;

namespace MediaPlayer_X_Ark.Skin
{
	public interface ISkinSystem
	{
		FormComponents MainForm { get; }
		SpectrumComponents ImgSpectrum { get; }
		Dictionary<string, ButtonComponents> Buttons { get; }
		Dictionary<string, SliderComponents> Sliders { get; }
		Dictionary<string, GraphicComponents> Labels { get; }

		// ★フォームも辞書に
		Dictionary<string, FormComponents> Forms { get; }
		Dictionary<string, PListGrid> Grids { get; }
		// ★フォームごとのボタンも辞書で管理
		Dictionary<string, Dictionary<string, ButtonComponents>> FormButtons { get; }

		FormComponents this[string formName] { get; }
		Dictionary<string, ButtonComponents> GetFormButtons(string formName);
	}
}