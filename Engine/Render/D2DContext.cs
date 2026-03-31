using System;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.WIC;

namespace MediaPlayer_X_Ark.Engine.Render
{
  /// <summary>
  /// Direct2D / DirectWrite / WIC の共有ファクトリを管理するシングルトン。
  /// アプリ起動時に Initialize() を呼び、終了時に Dispose() を呼ぶこと。
  /// </summary>
  internal static class D2DContext
  {
    public static ID2D1Factory1 Factory { get; private set; }
    public static IDWriteFactory DWrite { get; private set; }
    public static IWICImagingFactory WIC { get; private set; }

    private static bool _initialized;

    public static void Initialize()
    {
      if (_initialized) return;

      Factory = D2D1.D2D1CreateFactory<ID2D1Factory1>(Vortice.Direct2D1.FactoryType.MultiThreaded);
      DWrite = Vortice.DirectWrite.DWrite.DWriteCreateFactory<IDWriteFactory>(Vortice.DirectWrite.FactoryType.Shared);
      WIC = new IWICImagingFactory();

      _initialized = true;
    }

    public static void Dispose()
    {
      if (!_initialized) return;

      WIC?.Dispose();
      DWrite?.Dispose();
      Factory?.Dispose();

      WIC = null;
      DWrite = null;
      Factory = null;

      _initialized = false;
    }
  }
}
