namespace MediaPlayer_X_Ark.Skin
{
	public interface ISkinSystem
	{
		FormComponents MainForm { get; }
		SpectrumComponents ImgSpectrum { get; }
		ButtonComponents BtnOpen { get; }
		ButtonComponents BtnClose { get; }
		ButtonComponents BtnPlay { get; }
		ButtonComponents BtnStop { get; }
		ButtonComponents BtnBack { get; }
		ButtonComponents BtnSeekBack { get; }
		ButtonComponents BtnPause { get; }
		ButtonComponents BtnSeekForward { get; }
		ButtonComponents BtnNext { get; }
		ButtonComponents BtnRandom { get; }
		ButtonComponents BtnLoop { get; }
		ButtonComponents BtnSetting { get; }
		ButtonComponents BtnPlaylist { get; }
		ButtonComponents BtnMinisize { get; }
		ButtonComponents BtnCD { get; }
		SliderComponents SldVolume { get; }
		SliderComponents SldPan { get; }
		SliderComponents SldTrack { get; }
		GraphicComponents LabelTitle { get; }
		GraphicComponents LabelTime { get; }
		FormComponents PlayListForm { get; }
		PListGrid PlayListGrid { get; }
		ButtonComponents PBtnOpen { get; }
		ButtonComponents PBtnSave { get; }
		ButtonComponents PBtnRemove { get; }
		ButtonComponents PBtnUp { get; }
		ButtonComponents PBtnDown { get; }
		ButtonComponents PBtnClose { get; }
		ButtonComponents PBtnClear { get; }
	}
}