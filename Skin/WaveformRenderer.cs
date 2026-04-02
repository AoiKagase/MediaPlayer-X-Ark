using MediaPlayer_X_Ark.Engine.Render;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Vortice.DCommon;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using GdiColor = System.Drawing.Color;
using GdiPixelFormat = System.Drawing.Imaging.PixelFormat;
using D2DPixelFormat = Vortice.DCommon.PixelFormat;

namespace MediaPlayer_X_Ark.Skin
{
	/// <summary>
	/// 波形ピーク列（WaveformL/R）からシークバー用ビットマップを生成する。
	/// 表示モード・色はすべて外部から注入する。
	/// 内部描画は Direct2D（DC レンダーターゲット）を使用し、DrawLine ループを PathGeometry に集約する。
	/// </summary>
	public static class WaveformRenderer
	{
		public enum WaveformMode
		{
			/// <summary>L+R の最大値を1本で表示</summary>
			Mix,
			/// <summary>上半分L・下半分R</summary>
			Stereo,
			/// <summary>LとRを同じ領域に重ねて表示</summary>
			Overlay,
		}

		public class WaveformColors
		{
			public GdiColor Played   { get; set; } = GdiColor.FromArgb(100, 100, 100);
			public GdiColor Unplayed { get; set; } = GdiColor.FromArgb(50, 50, 50);
			public GdiColor ColorL   { get; set; } = GdiColor.FromArgb(0, 200, 100);
			public GdiColor ColorR   { get; set; } = GdiColor.FromArgb(0, 100, 200);
			public GdiColor ColorMix { get; set; } = GdiColor.FromArgb(0, 180, 120);
		}

		/// <summary>
		/// D2DContext.Dispose() 時に呼ぶこと。
		/// </summary>
		public static void DisposeResources()
		{
			// DC レンダーターゲットは毎回生成・破棄するためここでは何もしない
		}

		// ─────────────────────────────────────────────────────────────────────
		// 公開 API
		// ─────────────────────────────────────────────────────────────────────

		/// <summary>
		/// 波形ビットマップを生成する。
		/// </summary>
		public static Bitmap Render(
			float[] peaksL,
			float[] peaksR,
			int width,
			int height,
			float playedRatio,
			WaveformMode mode = WaveformMode.Mix,
			WaveformColors colors = null,
			float abStart = -1f,
			float abEnd = -1f)
		{
			colors ??= new WaveformColors();

			// D2D が使えない場合は GDI+ フォールバック
			if (D2DContext.Factory == null)
				return RenderGdi(peaksL, peaksR, width, height, playedRatio, mode, colors, abStart, abEnd);

			return RenderD2D(peaksL, peaksR, width, height, playedRatio, mode, colors, abStart, abEnd);
		}

		// ─────────────────────────────────────────────────────────────────────
		// Direct2D 実装
		// ─────────────────────────────────────────────────────────────────────

		private static Bitmap RenderD2D(
			float[] peaksL,
			float[] peaksR,
			int width,
			int height,
			float playedRatio,
			WaveformMode mode,
			WaveformColors colors,
			float abStart,
			float abEnd)
		{
			// WIC ビットマップへ直接描画することで GDI のアルファ破壊を回避する
			using var wicBitmap = D2DContext.WIC.CreateBitmap(
				(uint)width, (uint)height,
				Vortice.WIC.PixelFormat.Format32bppPBGRA,
				Vortice.WIC.BitmapCreateCacheOption.CacheOnLoad);

			bool began = false;
			ID2D1RenderTarget rt = null;
			try
			{
				rt = D2DContext.Factory.CreateWicBitmapRenderTarget(
					wicBitmap,
					new RenderTargetProperties(
						new D2DPixelFormat(
							Vortice.DXGI.Format.B8G8R8A8_UNorm,
							AlphaMode.Premultiplied)));

				rt.BeginDraw();
				began = true;

				rt.Clear(new Color4(0f, 0f, 0f, 0f));

				int sampleCount = peaksL?.Length ?? 0;
				if (sampleCount > 0)
					DrawWaveformD2D(rt, peaksL, peaksR, width, height, sampleCount,
						playedRatio, mode, colors, abStart, abEnd);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"WaveformRenderer D2D error: {ex.Message}");
			}
			finally
			{
				if (began)
				{
					try { rt?.EndDraw(); }
					catch (Exception ex) { Debug.WriteLine($"EndDraw failed: {ex.Message}"); }
				}
				rt?.Dispose();
			}

			// WIC ビットマップのピクセルを GDI+ Bitmap へコピー（アルファ保持）
			var result = new Bitmap(width, height, GdiPixelFormat.Format32bppPArgb);
			var bmpData = result.LockBits(
				new System.Drawing.Rectangle(0, 0, width, height),
				System.Drawing.Imaging.ImageLockMode.WriteOnly,
				GdiPixelFormat.Format32bppPArgb);
			try
			{
				wicBitmap.CopyPixels(
					(uint)Math.Abs(bmpData.Stride),
					(uint)(Math.Abs(bmpData.Stride) * height),
					bmpData.Scan0);
			}
			finally
			{
				result.UnlockBits(bmpData);
			}
			return result;
		}

		private static void DrawWaveformD2D(
			ID2D1RenderTarget rt,
			float[] peaksL,
			float[] peaksR,
			int width,
			int height,
			int sampleCount,
			float playedRatio,
			WaveformMode mode,
			WaveformColors colors,
			float abStart,
			float abEnd)
		{
			int playedX  = (int)(width * playedRatio);
			int abStartX = abStart >= 0 ? (int)(width * abStart) : -1;
			int abEndX   = abEnd   >= 0 ? (int)(width * abEnd)   : -1;

			// 色グループを事前計算（3グループ × チャンネル数）
			// グループ: 0=Normal, 1=Played, 2=ABRange
			GdiColor[] colL = {
				colors.ColorL,
				Darken(colors.ColorL, 0.5f),
				Blend(colors.ColorL, GdiColor.Red, 0.4f),
			};
			GdiColor[] colR = {
				colors.ColorR,
				Darken(colors.ColorR, 0.5f),
				Blend(colors.ColorR, GdiColor.Red, 0.4f),
			};
			GdiColor[] colM = {
				colors.ColorMix,
				Darken(colors.ColorMix, 0.5f),
				Blend(colors.ColorMix, GdiColor.Red, 0.4f),
			};

			// グループ別にジオメトリを構築して一括描画
			switch (mode)
			{
				case WaveformMode.Mix:
					DrawGeometryGroups(rt, width, height, sampleCount,
						peaksL, peaksR, playedX, abStartX, abEndX,
						colM, 3, DrawMixLine);
					break;

				case WaveformMode.Stereo:
					DrawGeometryGroups(rt, width, height, sampleCount,
						peaksL, peaksR, playedX, abStartX, abEndX,
						colL, 3, DrawStereoTopLine);
					DrawGeometryGroups(rt, width, height, sampleCount,
						peaksL, peaksR, playedX, abStartX, abEndX,
						colR, 3, DrawStereoBottomLine);
					break;

				case WaveformMode.Overlay:
					DrawGeometryGroupsOverlay(rt, width, height, sampleCount,
						peaksL, peaksR, playedX, abStartX, abEndX, colL, colR);
					break;
			}
		}

		// ── ジオメトリ構築・描画ヘルパー ─────────────────────────────────────

		/// <summary>
		/// 3色グループ（Normal / Played / ABRange）に分けてジオメトリを構築し描画する。
		/// </summary>
		private static void DrawGeometryGroups(
			ID2D1RenderTarget rt,
			int width, int height, int sampleCount,
			float[] peaksL, float[] peaksR,
			int playedX, int abStartX, int abEndX,
			GdiColor[] groupColors, int groupCount,
			Action<ID2D1GeometrySink, int, float, float, int> addLine)
		{
			using var paths  = new PathGeometryGroup(D2DContext.Factory, groupCount);

			for (int x = 0; x < width; x++)
			{
				int sIdx = Math.Min((int)((float)x / width * sampleCount), sampleCount - 1);
				float pL = peaksL[sIdx];
				float pR = peaksR != null ? peaksR[sIdx] : pL;

				int g = GroupIndex(x, playedX, abStartX, abEndX);
				addLine(paths.Sinks[g], x, pL, pR, height);
			}

			paths.CloseAll();

			for (int i = 0; i < groupCount; i++)
			{
				using var brush = rt.CreateSolidColorBrush(ToColor4(groupColors[i]));
				rt.DrawGeometry(paths.Geometries[i], brush, 1f);
			}
		}

		/// <summary>Overlay モード専用（半透明2チャンネルを重ねて描画）</summary>
		private static void DrawGeometryGroupsOverlay(
			ID2D1RenderTarget rt,
			int width, int height, int sampleCount,
			float[] peaksL, float[] peaksR,
			int playedX, int abStartX, int abEndX,
			GdiColor[] colL, GdiColor[] colR)
		{
			const int groups = 3;
			using var pathsL = new PathGeometryGroup(D2DContext.Factory, groups);
			using var pathsR = new PathGeometryGroup(D2DContext.Factory, groups);

			for (int x = 0; x < width; x++)
			{
				int sIdx = Math.Min((int)((float)x / width * sampleCount), sampleCount - 1);
				float pL = peaksL[sIdx];
				float pR = peaksR != null ? peaksR[sIdx] : pL;
				int g = GroupIndex(x, playedX, abStartX, abEndX);
				DrawMixLine(pathsL.Sinks[g], x, pL, pR: 0f, height);
				DrawMixLine(pathsR.Sinks[g], x, pR, pR: 0f, height);
			}

			pathsL.CloseAll();
			pathsR.CloseAll();

			for (int i = 0; i < groups; i++)
			{
				var cL = GdiColor.FromArgb(180, colL[i]);
				var cR = GdiColor.FromArgb(180, colR[i]);
				using var brushL = rt.CreateSolidColorBrush(ToColor4(cL));
				using var brushR = rt.CreateSolidColorBrush(ToColor4(cR));
				rt.DrawGeometry(pathsL.Geometries[i], brushL, 1f);
				rt.DrawGeometry(pathsR.Geometries[i], brushR, 1f);
			}
		}

		// ── 各モードの線セグメント追加 ───────────────────────────────────────

		private static void DrawMixLine(ID2D1GeometrySink sink, int x, float pL, float pR, int height)
		{
			float peak = Math.Max(pL, pR);
			float half = height / 2f;
			float barH = peak * half;
			sink.BeginFigure(new Vector2(x + 0.5f, half - barH), FigureBegin.Hollow);
			sink.AddLine(new Vector2(x + 0.5f, half + barH));
			sink.EndFigure(FigureEnd.Open);
		}

		private static void DrawStereoTopLine(ID2D1GeometrySink sink, int x, float pL, float pR, int height)
		{
			float half = height / 2f;
			float barH = pL * half;
			sink.BeginFigure(new Vector2(x + 0.5f, half - barH), FigureBegin.Hollow);
			sink.AddLine(new Vector2(x + 0.5f, half));
			sink.EndFigure(FigureEnd.Open);
		}

		private static void DrawStereoBottomLine(ID2D1GeometrySink sink, int x, float pL, float pR, int height)
		{
			float half = height / 2f;
			float barH = pR * half;
			sink.BeginFigure(new Vector2(x + 0.5f, half), FigureBegin.Hollow);
			sink.AddLine(new Vector2(x + 0.5f, half + barH));
			sink.EndFigure(FigureEnd.Open);
		}

		// ── ユーティリティ ───────────────────────────────────────────────────

		/// <summary>色グループインデックス: 0=Normal, 1=Played, 2=ABRange</summary>
		private static int GroupIndex(int x, int playedX, int abStartX, int abEndX)
		{
			if (abStartX >= 0 && abEndX >= 0 && x >= abStartX && x <= abEndX)
				return 2;
			if (x < playedX)
				return 1;
			return 0;
		}

		private static Color4 ToColor4(GdiColor c)
			=> new Color4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

		// ─────────────────────────────────────────────────────────────────────
		// GDI+ フォールバック（D2D 未初期化時）
		// ─────────────────────────────────────────────────────────────────────

		private static Bitmap RenderGdi(
			float[] peaksL,
			float[] peaksR,
			int width,
			int height,
			float playedRatio,
			WaveformMode mode,
			WaveformColors colors,
			float abStart,
			float abEnd)
		{
			var bmp = new Bitmap(width, height, GdiPixelFormat.Format32bppArgb);
			using var g = Graphics.FromImage(bmp);
			g.Clear(GdiColor.Transparent);

			int sampleCount = peaksL?.Length ?? 0;
			if (sampleCount == 0)
				return bmp;

			int playedX  = (int)(width * playedRatio);
			int abStartX = abStart >= 0 ? (int)(width * abStart) : -1;
			int abEndX   = abEnd   >= 0 ? (int)(width * abEnd)   : -1;

			for (int x = 0; x < width; x++)
			{
				int sIdx = Math.Min((int)((float)x / width * sampleCount), sampleCount - 1);
				float pL = peaksL[sIdx];
				float pR = peaksR != null ? peaksR[sIdx] : pL;

				bool isPlayed  = x < playedX;
				bool isAbRange = abStartX >= 0 && abEndX >= 0 && x >= abStartX && x <= abEndX;

				GdiColor baseColorL = isAbRange ? Blend(colors.ColorL, GdiColor.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorL, 0.5f) : colors.ColorL;
				GdiColor baseColorR = isAbRange ? Blend(colors.ColorR, GdiColor.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorR, 0.5f) : colors.ColorR;
				GdiColor baseColorM = isAbRange ? Blend(colors.ColorMix, GdiColor.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorMix, 0.5f) : colors.ColorMix;

				switch (mode)
				{
					case WaveformMode.Mix:
						GdiDrawBar(g, x, height, Math.Max(pL, pR), baseColorM);
						break;
					case WaveformMode.Stereo:
						GdiDrawBarTop(g, x, height, pL, baseColorL);
						GdiDrawBarBottom(g, x, height, pR, baseColorR);
						break;
					case WaveformMode.Overlay:
						GdiDrawBar(g, x, height, pL, GdiColor.FromArgb(180, baseColorL));
						GdiDrawBar(g, x, height, pR, GdiColor.FromArgb(180, baseColorR));
						break;
				}
			}

			return bmp;
		}

		private static void GdiDrawBar(Graphics g, int x, int height, float peak, GdiColor color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			using var pen = new Pen(color);
			g.DrawLine(pen, x, half - barH, x, half + barH);
		}

		private static void GdiDrawBarTop(Graphics g, int x, int height, float peak, GdiColor color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			using var pen = new Pen(color);
			g.DrawLine(pen, x, half - barH, x, half);
		}

		private static void GdiDrawBarBottom(Graphics g, int x, int height, float peak, GdiColor color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			using var pen = new Pen(color);
			g.DrawLine(pen, x, half, x, half + barH);
		}

		private static GdiColor Darken(GdiColor c, float factor)
			=> GdiColor.FromArgb(c.A,
				(int)(c.R * factor),
				(int)(c.G * factor),
				(int)(c.B * factor));

		private static GdiColor Blend(GdiColor a, GdiColor b, float t)
			=> GdiColor.FromArgb(
				(int)(a.A + (b.A - a.A) * t),
				(int)(a.R + (b.R - a.R) * t),
				(int)(a.G + (b.G - a.G) * t),
				(int)(a.B + (b.B - a.B) * t));

		// ─────────────────────────────────────────────────────────────────────
		// PathGeometryGroup ヘルパー（using で確実に破棄）
		// ─────────────────────────────────────────────────────────────────────

		private sealed class PathGeometryGroup : IDisposable
		{
			public readonly ID2D1PathGeometry[] Geometries;
			public readonly ID2D1GeometrySink[] Sinks;
			private bool _disposed;

			public PathGeometryGroup(ID2D1Factory1 factory, int count)
			{
				Geometries = new ID2D1PathGeometry[count];
				Sinks      = new ID2D1GeometrySink[count];
				for (int i = 0; i < count; i++)
				{
					Geometries[i] = factory.CreatePathGeometry();
					Sinks[i]      = Geometries[i].Open();
				}
			}

			public void CloseAll()
			{
				foreach (var s in Sinks)
					s.Close();
			}

			public void Dispose()
			{
				if (_disposed)
					return;
				_disposed = true;
				foreach (var s in Sinks)      s?.Dispose();
				foreach (var g in Geometries) g?.Dispose();
			}
		}
	}
}
