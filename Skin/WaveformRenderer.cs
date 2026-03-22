using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MediaPlayer_X_Ark.Skin
{
	/// <summary>
	/// 波形ピーク列（WaveformL/R）からシークバー用ビットマップを生成する。
	/// 表示モード・色はすべて外部から注入する。
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
			public Color Played { get; set; } = Color.FromArgb(100, 100, 100);
			public Color Unplayed { get; set; } = Color.FromArgb(50, 50, 50);
			public Color ColorL { get; set; } = Color.FromArgb(0, 200, 100);
			public Color ColorR { get; set; } = Color.FromArgb(0, 100, 200);
			public Color ColorMix { get; set; } = Color.FromArgb(0, 180, 120);
		}

		/// <summary>
		/// 波形ビットマップを生成する。
		/// </summary>
		/// <param name="peaksL">Lチャンネルのピーク列（0.0〜1.0）</param>
		/// <param name="peaksR">Rチャンネルのピーク列（0.0〜1.0）</param>
		/// <param name="width">ビットマップ幅（シークバーの幅）</param>
		/// <param name="height">ビットマップ高さ</param>
		/// <param name="playedRatio">再生済み割合（0.0〜1.0）</param>
		/// <param name="mode">表示モード</param>
		/// <param name="colors">カラー設定</param>
		/// <param name="abStart">ABリピートA点（0.0〜1.0、未設定は-1）</param>
		/// <param name="abEnd">ABリピートB点（0.0〜1.0、未設定は-1）</param>
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

			var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using var g = Graphics.FromImage(bmp);
			g.Clear(Color.Transparent);

			int sampleCount = peaksL?.Length ?? 0;
			if (sampleCount == 0) return bmp;

			int playedX = (int)(width * playedRatio);
			int abStartX = abStart >= 0 ? (int)(width * abStart) : -1;
			int abEndX = abEnd >= 0 ? (int)(width * abEnd) : -1;

			for (int x = 0; x < width; x++)
			{
				// このピクセルに対応するサンプルインデックス
				int sIdx = Math.Min((int)((float)x / width * sampleCount),
					sampleCount - 1);

				float pL = peaksL[sIdx];
				float pR = peaksR != null ? peaksR[sIdx] : pL;

				// 再生済み/未再生/ABリピート範囲の色決定
				bool isPlayed = x < playedX;
				bool isAbRange = abStartX >= 0 && abEndX >= 0
								  && x >= abStartX && x <= abEndX;

				Color baseColorL = isAbRange
					? Blend(colors.ColorL, Color.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorL, 0.5f) : colors.ColorL;

				Color baseColorR = isAbRange
					? Blend(colors.ColorR, Color.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorR, 0.5f) : colors.ColorR;

				Color baseColorM = isAbRange
					? Blend(colors.ColorMix, Color.Red, 0.4f)
					: isPlayed ? Darken(colors.ColorMix, 0.5f) : colors.ColorMix;

				switch (mode)
				{
					case WaveformMode.Mix:
						DrawBar(g, x, height, Math.Max(pL, pR), baseColorM);
						break;

					case WaveformMode.Stereo:
						DrawBarTop(g, x, height, pL, baseColorL);
						DrawBarBottom(g, x, height, pR, baseColorR);
						break;

					case WaveformMode.Overlay:
						DrawBar(g, x, height, pL, Color.FromArgb(180, baseColorL));
						DrawBar(g, x, height, pR, Color.FromArgb(180, baseColorR));
						break;
				}
			}

			return bmp;
		}

		// ── 描画ヘルパー ───────────────────────────────────────────────────

		/// <summary>中央から上下に伸びるバーを描画（Mix / Overlay）</summary>
		private static void DrawBar(Graphics g, int x, int height,
			float peak, Color color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			int top = half - barH;
			int bottom = half + barH;
			using var pen = new Pen(color);
			g.DrawLine(pen, x, top, x, bottom);
		}

		/// <summary>上半分のみにバーを描画（Stereo L）</summary>
		private static void DrawBarTop(Graphics g, int x, int height,
			float peak, Color color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			using var pen = new Pen(color);
			g.DrawLine(pen, x, half - barH, x, half);
		}

		/// <summary>下半分のみにバーを描画（Stereo R）</summary>
		private static void DrawBarBottom(Graphics g, int x, int height,
			float peak, Color color)
		{
			int half = height / 2;
			int barH = (int)(peak * half);
			using var pen = new Pen(color);
			g.DrawLine(pen, x, half, x, half + barH);
		}

		private static Color Darken(Color c, float factor)
			=> Color.FromArgb(c.A,
				(int)(c.R * factor),
				(int)(c.G * factor),
				(int)(c.B * factor));

		private static Color Blend(Color a, Color b, float t)
			=> Color.FromArgb(
				(int)(a.A + (b.A - a.A) * t),
				(int)(a.R + (b.R - a.R) * t),
				(int)(a.G + (b.G - a.G) * t),
				(int)(a.B + (b.B - a.B) * t));
	}
}