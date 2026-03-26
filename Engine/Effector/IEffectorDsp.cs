namespace MediaPlayer_X_Ark.Engine.Effector
{
    /// <summary>
    /// FMODのDSP操作を抽象化するブリッジインターフェース。
    /// AbstractEffectorBase はこのインターフェース経由でのみDSP操作を行う。
    /// </summary>
    public interface IEffectorDsp
    {
        bool IsValid { get; }
        bool Bypass { get; set; }

        FMOD.RESULT GetParameterFloat(int index, out float value);
        FMOD.RESULT SetParameterFloat(int index, float value);
        FMOD.RESULT SetParameterBool(int index, bool value);
        FMOD.RESULT SetParameterData(int index, byte[] value);

        void Release();
    }
}