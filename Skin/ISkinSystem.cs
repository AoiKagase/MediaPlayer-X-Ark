using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_X_Ark.Skin
{
    public interface ISkinSystem
    {
        FormComponents MainForm { get; }
        SpectrumComponents ImgSpectrum { get; }
        WaveformComponents Waveform { get; }
        Dictionary<string, ButtonComponents> Buttons { get; }
        Dictionary<string, SliderComponents> Sliders { get; }
        Dictionary<string, LabelComponents> Labels { get; }

        // ★フォームも辞書に
        Dictionary<string, FormComponents> Forms { get; }
        Dictionary<string, GridComponents> Grids { get; }
        // ★フォームごとのボタンも辞書で管理
        Dictionary<string, Dictionary<string, ButtonComponents>> FormButtons { get; }

        FormComponents this[string formName] { get; }
        Dictionary<string, ButtonComponents> GetFormButtons(string formName);
    }
}

