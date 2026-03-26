using System.ComponentModel;

namespace MediaPlayer_X_Ark.Engine.Effector
{
    /// <summary>
    /// 全エフェクターが実装する共通インターフェース。
    /// FMOD依存を含まない。
    /// </summary>
    public interface IEffector : INotifyPropertyChanged
    {
        bool Enabled { get; set; }
        FMOD.RESULT Switch(bool sw);
        void SetDefault();
    }
}