using System;

namespace MediaPlayer_X_Ark.Engine.Player
{
    public class PlayerErrorEventArgs : EventArgs
    {
        public string Function { get; }
        public string Message { get; }
        public int ErrorCode { get; }  // FMOD.RESULT を int に変換して隠蔽

        public PlayerErrorEventArgs(string function, string message, int code)
        {
            Function = function;
            Message = message;
            ErrorCode = code;
        }

        public bool IsOk => ErrorCode == 0;  // FMOD.RESULT.OK == 0

        public override string ToString()
            => IsOk ? "" : $"{Function} - {Message}";
    }
}