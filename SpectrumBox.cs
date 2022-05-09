using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

using SharpDX.Direct2D1;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MediaPlayer_X_Ark
{
	public partial class SpectrumBox : PictureBox
	{
		//--------------------------------------------------------------//
		//                         DirectX設定                          //
		//--------------------------------------------------------------//
		/// <summary>
		/// Direct3Dのデバイス
		/// </summary>
		public SharpDX.Direct3D11.Device Device { get { return _device; } }
		private SharpDX.Direct3D11.Device _device = null;

		/// <summary>
		/// スワップチェーン
		/// ※デバイスが描いた画像をウィンドウに表示する機能
		/// </summary>
		SwapChain _SwapChain;
		Texture2D _BackBuffer;

		#region Direct2D関連
		/// <summary>
		/// レンダーターゲット2D
		/// </summary>
		public RenderTarget RenderTarget2D { get { return _RenderTarget2D; } }
		private RenderTarget _RenderTarget2D;
		/// <summary>
		/// Direct2Dで描画用のファクトリーオブジェクト
		/// </summary>
		private SharpDX.Direct2D1.Factory _Factory2D;

		/// <summary>
		/// DirectWriteで描画用のファクトリーオブジェクト
		/// </summary>
		private SharpDX.DirectWrite.Factory _FactoryDWrite;
		/// <summary>
		/// 描画ブラシ
		/// </summary>
		private SolidColorBrush _ColorBrush;
		private SharpDX.Color _BackColor;
		/// <summary>
		/// 画像
		/// </summary>
		private string _FileSpectrum;
		private string _FileSnow;
		private string _FileWave;
		private SharpDX.Direct2D1.Bitmap _BitmapSpectrum;
		private SharpDX.Direct2D1.Bitmap _BitmapSnow;
		private SharpDX.Direct2D1.Bitmap _BitmapWave;
		#endregion

		public string BitmapSpectrum
        {
			get
			{
				return _FileSpectrum;
			}
			set
			{
				_BitmapSpectrum = LoadFromFile(_RenderTarget2D, value);
				_FileSpectrum = value;
			}
		}

		public string BitmapSnow
		{
			get { return _FileSnow; }
			set
			{
				_BitmapSnow = LoadFromFile(_RenderTarget2D, value);
				_FileSnow = value;
			}
		}

		public string BitmapWave
        {
			get
			{
				return _FileWave;
			}
			set
			{
				_BitmapWave = LoadFromFile(_RenderTarget2D, value);
				_FileWave = value;
			}
        }
		/// <summary>
		/// 表示対象ハンドル
		/// </summary>
		protected IntPtr DisplayHandle { get { return Handle; } }

		public SpectrumBox()
		{
			InitializeComponent();

			// スタイルの指定
			SetStyle(ControlStyles.AllPaintingInWmPaint |// ちらつき抑える
				ControlStyles.Opaque, true);            // 背景は描画しない

		}

		/// <summary>
		/// DirectXデバイスの初期化
		/// </summary>
		public void Initialize(System.Drawing.Color backColor)
		{
			// スワップチェーン設定
			var desc = new SwapChainDescription()
			{
				// バッファ数
				// ※ダブルバッファリングを行う場合は2を指定
				BufferCount = 1,
				// 描画情報
				ModeDescription = new ModeDescription(ClientSize.Width, ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
				// ウィンドウモードの有効・無効
				IsWindowed = true,
				// 描画対象ハンドル
				OutputHandle = DisplayHandle,
				// マルチサンプル方法の指定
				SampleDescription = new SampleDescription(1, 0),
				// 描画後の表示バッファの扱い方法の指定
				SwapEffect = SwapEffect.Discard,
				// 描画画像の使用方法
				Usage = Usage.RenderTargetOutput
			};

			// デバイスとスワップチェーンを生成
			SharpDX.Direct3D11.Device.CreateWithSwapChain(
				// デバイスの種類
				DriverType.Hardware,
				// ランタイムレイヤーの有効にするリスト
				DeviceCreationFlags.BgraSupport,
				// フィーチャーレベル
				// ※ある程度のハードウェアのレベルを規定して，それぞれのレベルにあわせたプログラムを書ける仕組み
				// ※DirectX の世代を指定
				new[] { SharpDX.Direct3D.FeatureLevel.Level_11_0 },
				// スワップチェーン設定
				desc,
				// 生成した変数を返す
				out _device, out _SwapChain);

			// Windowsの不要なイベントを無効にする
			var factory = _SwapChain.GetParent<SharpDX.DXGI.Factory>();
			factory.MakeWindowAssociation(DisplayHandle, WindowAssociationFlags.IgnoreAll);

			// バックバッファーを保持する
			_BackBuffer = Texture2D.FromSwapChain<Texture2D>(_SwapChain, 0);
			_BackColor = new SharpDX.Color(backColor.R, backColor.G, backColor.B, backColor.A);

			// 2D用の初期化を行う
			InitializeDirect2D();
		}
		#region DirectXデバイス基本初期設定
		/// <summary>
		/// Direct2D 関連の初期化
		/// </summary>
		public void InitializeDirect2D()
		{
			// Direct2Dリソースを作成
			_Factory2D = new SharpDX.Direct2D1.Factory();
			using (var surface = _BackBuffer.QueryInterface<Surface>())
			{
				_RenderTarget2D = new RenderTarget(_Factory2D, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));
			}
			// 非テキストプリミティブのエッジのレンダリング方法を指定
			_RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
			// テキストの描画に使用されるアンチエイリアスモードについて指定
			_RenderTarget2D.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;


			// DirectWriteオブジェクトを生成するために必要なファクトリオブジェクトを生成
			_FactoryDWrite = new SharpDX.DirectWrite.Factory();

			// ブラシを生成
			_ColorBrush = new SolidColorBrush(_RenderTarget2D, SharpDX.Color.Red);
			// RGBAで色を指定する場合は下記のように行う
			//_ColorBrush = new SolidColorBrush(_RenderTarget2D, new SharpDX.Color(255, 255, 255, 255));
		}
		#endregion

		/// <summary>
		/// メインループ処理
		/// </summary>
		public void MainLoop(System.Drawing.Color backColor, int[] mFFT)
		{
			_RenderTarget2D?.BeginDraw();
			// 画面を特定の色(例．灰色)で初期化
			_RenderTarget2D?.Clear(_BackColor);

			// 画像描画
			if (_RenderTarget2D != null)
			{
				// 位置
				var pos = new Vector2(0.0f, 0.0f);
				// サイズ
				var size = _BitmapSpectrum?.Size ?? new Size2F();

				// 画像描画
				_RenderTarget2D.DrawBitmap(_BitmapSpectrum, new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, pos.X + size.Width, pos.Y + size.Height), 1.0f, BitmapInterpolationMode.Linear);
			}

			_RenderTarget2D?.EndDraw();
			_SwapChain?.Present(0, PresentFlags.None);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		/// <summary>
		/// 解放処理
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
		}

		/// <summary>
		/// BitmapからDirectX用のBitmapを生成する
		/// </summary>
		/// <param name="renderTarget">描画先のレンダーターゲットを指定する</param>
		/// <param name="file">読み込むファイルアドレス</param>
		/// <returns>Bitmap形式の画像データ</returns>
		private static SharpDX.Direct2D1.Bitmap LoadFromFile(RenderTarget renderTarget, string file)
		{
			// System.Drawing.Imageを使ってファイルから画像を読み込む
			using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
			{
				// BGRA から RGBA 形式へ変換する
				// 1行のデータサイズを算出
				int stride = bitmap.Width * sizeof(int);
				using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
				{
					// 読み込み元のBitmapをロックする
					var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
					var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

					// 変換処理
					for (int y = 0; y < bitmap.Height; y++)
					{
						int offset = bitmapData.Stride * y;
						for (int x = 0; x < bitmap.Width; x++)
						{
							// 1byteずつデータを読み込む
							byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
							int rgba = R | (G << 8) | (B << 16) | (A << 24);
							tempStream.Write(rgba);
						}
					}
					// 読み込み元のBitmapのロックを解除する
					bitmap.UnlockBits(bitmapData);
					tempStream.Position = 0;

					// 変換したデータからBitmapを生成して返す
					var size = new Size2(bitmap.Width, bitmap.Height);
					var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
					return new SharpDX.Direct2D1.Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
				}
			}
		}
	}
}
