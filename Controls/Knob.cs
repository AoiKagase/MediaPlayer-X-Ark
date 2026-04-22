using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Engine.Render;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
// 曖昧参照を解消するエイリアス
using Color4     = Vortice.Mathematics.Color4;
using RectangleF = System.Drawing.RectangleF;

namespace UI
{
	/// <summary>
	/// Represents a knob control
	/// </summary>
	[Description("Represents a knob control")]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	public class Knob : Control
	{
		static readonly object _ValueChangedKey = new object();
		static readonly object _LargeChangeChangedKey = new object();
		static readonly object _MinimumChangedKey = new object();
		static readonly object _MaximumChangedKey = new object();
		static readonly object _BorderColorChangedKey = new object();
		static readonly object _KnobColorChangedKey = new object();
		static readonly object _PointerColorChangedKey = new object();
		static readonly object _PointerWidthChangedKey = new object();
		static readonly object _BorderWidthChangedKey = new object();
		static readonly object _PointerStartCapChangedKey = new object();
		static readonly object _PointerEndCapChangedKey = new object();
		static readonly object _MinimumAngleChangedKey = new object();
		static readonly object _MaximumAngleChangedKey = new object();
		static readonly object _HasTicksChangedKey = new object();
		static readonly object _TickHeightChangedKey = new object();
		static readonly object _TickWidthChangedKey = new object();
		static readonly object _TickColorChangedKey = new object();
		static readonly object _PointerOffsetChangedKey = new object();
		int _largeChange = 20;
		int _minimum = 0;
		int _maximum = 100;
		int _value=0;
		Color _borderColor = SystemColors.ControlDarkDark;
		Color _knobColor = SystemColors.Control;
		Color _pointerColor = SystemColors.ControlText;
		Color _tickColor = SystemColors.ControlDarkDark;
		int _pointerWidth = 1;
		int _pointerOffset = 0;
		int _borderWidth = 1;
		int _minimumAngle = 30;
		int _maximumAngle = 330;
		LineCap _pointerStartCap=LineCap.Round;
		LineCap _pointerEndCap=LineCap.Triangle;
		bool _dragging = false;
		Point _dragHit;
		bool _hasTicks = false;
		int _tickHeight = 2;
		int _tickWidth = 1;
		int[] _tickPositions;

		string _parameterName = "";
		string _unit = "";
		float _scale = 1f;

		/// <summary>
		/// パラメータの表示名（例："Decay Time"）
		/// </summary>
		[Description("Parameter display name")]
		[Category("Behavior")]
		[DefaultValue("")]
		public string ParameterName
		{
			get { return _parameterName; }
			set { _parameterName = value; }
		}

		/// <summary>
		/// 単位（例："ms", "dB", "%", ""）
		/// </summary>
		[Description("Unit of the parameter (e.g. ms, dB, %)")]
		[Category("Behavior")]
		[DefaultValue("")]
		public string Unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		/// <summary>
		/// スケール係数。Knob内部値 / Scale = FMOD実値。
		/// 例：Distortion(0.0～1.0)なら Scale=100、それ以外は Scale=1
		/// </summary>
		[Description("Scale factor. Knob internal value / Scale = actual value.")]
		[Category("Behavior")]
		[DefaultValue(1f)]
		public float Scales
		{
			get { return _scale; }
			set { _scale = value > 0 ? value : 1f; }
		}
		/// <summary>
		/// Creates a new instance of the control
		/// </summary>
		public Knob()
		{
			SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			UpdateStyles();
			TabStop = true;
			// D2D HwndRenderTarget が直接 HWND に描画するため DoubleBuffer (GDI BitBlt) は無効にする
			DoubleBuffered = false;
			_RecomputeTicks();
		}

		#region D2D

		private ID2D1HwndRenderTarget _renderTarget;
		private ID2D1StrokeStyle _pointerStrokeStyle;
		private LineCap _cachedStartCap;
		private LineCap _cachedEndCap;

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (D2DContext.Factory == null) return;
			CreateRenderTarget();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeDeviceResources();
			base.OnHandleDestroyed(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) { }

		protected override void WndProc(ref Message m)
		{
			const int WM_ERASEBKGND = 0x0014;
			if (m.Msg == WM_ERASEBKGND) { m.Result = IntPtr.Zero; return; }
			base.WndProc(ref m);
		}

		private void CreateRenderTarget()
		{
			if (!IsHandleCreated || D2DContext.Factory == null) return;
			DisposeDeviceResources();
			var pixelSize = GetClientPixelSize();
			_renderTarget = D2DContext.Factory.CreateHwndRenderTarget(
				new RenderTargetProperties(),
				new HwndRenderTargetProperties
				{
					Hwnd           = Handle,
					PixelSize      = pixelSize,
					PresentOptions = PresentOptions.None,
				});
		}

		private void DisposeDeviceResources()
		{
			_pointerStrokeStyle?.Dispose(); _pointerStrokeStyle = null;
			_renderTarget?.Dispose();       _renderTarget       = null;
		}

		private ID2D1StrokeStyle GetPointerStrokeStyle()
		{
			if (_pointerStrokeStyle == null
				|| _cachedStartCap != _pointerStartCap
				|| _cachedEndCap   != _pointerEndCap)
			{
				_pointerStrokeStyle?.Dispose();
				_pointerStrokeStyle = D2DContext.Factory.CreateStrokeStyle(new StrokeStyleProperties
				{
					StartCap = ToCapStyle(_pointerStartCap),
					EndCap   = ToCapStyle(_pointerEndCap),
				});
				_cachedStartCap = _pointerStartCap;
				_cachedEndCap   = _pointerEndCap;
			}
			return _pointerStrokeStyle;
		}

		private static CapStyle ToCapStyle(LineCap cap) => cap switch
		{
			LineCap.Round    => CapStyle.Round,
			LineCap.Square   => CapStyle.Square,
			LineCap.Triangle => CapStyle.Triangle,
			_                => CapStyle.Flat,
		};

		private static Color4 ToColor4(Color c)
			=> new Color4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

		#endregion
		/// <summary>
		/// Indicates the value of the control
		/// </summary>
		[Description("Indicates the value of the control")]
		[Category("Behavior")]
		[DefaultValue(0)]
		public int Value {
			get { return _value; }
			set {
				if (_value != value)
				{
					_value = value;
					Invalidate();
					OnValueChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of Value changes
		/// </summary>
		[Description("Raised with the value of Value changes")]
		[Category("Behavior")]
		public event EventHandler ValueChanged {
			add { Events.AddHandler(_ValueChangedKey, value); }
			remove { Events.RemoveHandler(_ValueChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of Value changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnValueChanged(EventArgs args)
		{
			(Events[_ValueChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the amount the control increments when the large modifiers are used
		/// </summary>
		[Description("Indicates the amount the control increments when the large modifiers are used")]
		[Category("Behavior")]
		[DefaultValue(20)]
		public int LargeChange {
			get { return _largeChange; }
			set {
				if (_largeChange != value)
				{
					_largeChange = value;
					_RecomputeTicks();
					Invalidate();
					OnLargeChangeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of LargeChange changes
		/// </summary>
		[Description("Raised with the value of LargeChange changes")]
		[Category("Behavior")]
		public event EventHandler LargeChangeChanged {
			add { Events.AddHandler(_LargeChangeChangedKey, value); }
			remove { Events.RemoveHandler(_LargeChangeChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of LargeChange changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnLargeChangeChanged(EventArgs args)
		{
			(Events[_LargeChangeChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the minimum value for the control
		/// </summary>
		[Description("Indicates the minimum value for the control")]
		[Category("Behavior")]
		[DefaultValue(0)]
		public int Minimum {
			get { return _minimum; }
			set {
				if (_minimum != value)
				{
					_minimum = value;
					_RecomputeTicks();
					Invalidate();
					OnMinimumChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of Minimum changes
		/// </summary>
		[Description("Raised with the value of Minimum changes")]
		[Category("Behavior")]
		public event EventHandler MinimumChanged {
			add { Events.AddHandler(_MinimumChangedKey, value); }
			remove { Events.RemoveHandler(_MinimumChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of Minimum changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnMinimumChanged(EventArgs args)
		{
			(Events[_MinimumChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the maximum value for the control
		/// </summary>
		[Description("Indicates the maximum value for the control")]
		[Category("Behavior")]
		[DefaultValue(100)]
		public int Maximum {
			get { return _maximum; }
			set {
				if (_maximum != value)
				{
					_maximum = value;
					_RecomputeTicks();
					Invalidate();
					OnMaximumChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of Maximum changes
		/// </summary>
		[Description("Raised with the value of Maximum changes")]
		[Category("Behavior")]
		public event EventHandler MaximumChanged {
			add { Events.AddHandler(_MaximumChangedKey, value); }
			remove { Events.RemoveHandler(_MaximumChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of Maximum changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnMaximumChanged(EventArgs args)
		{
			(Events[_MaximumChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the border color
		/// </summary>
		[Description("Indicates the border color of the control")]
		[Category("Appearance")]
		public Color BorderColor {
			get { return _borderColor; }
			set {
				if (value != _borderColor)
				{
					_borderColor = value;
					Invalidate();
					OnBorderColorChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised when the value of BorderColor changes
		/// </summary>
		[Description("Raised when the value of BorderColor changes")]
		[Category("Behavior")]
		public event EventHandler BorderColorChanged {
			add { Events.AddHandler(_BorderColorChangedKey, value); }
			remove { Events.RemoveHandler(_BorderColorChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of BorderColor changes
		/// </summary>
		/// <param name="args">The event args (not used)</param>
		protected virtual void OnBorderColorChanged(EventArgs args)
		{
			(Events[_BorderColorChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the knob color
		/// </summary>
		[Description("Indicates the knob color of the control")]
		[Category("Appearance")]
		public Color KnobColor {
			get { return _knobColor; }
			set {
				if (value != _knobColor)
				{
					_knobColor = value;
					Invalidate();
					OnKnobColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised when the value of KnobColor changes
		/// </summary>
		[Description("Raised when the value of KnobColor changes")]
		[Category("Behavior")]
		public event EventHandler KnobColorChanged {
			add { Events.AddHandler(_KnobColorChangedKey, value); }
			remove { Events.RemoveHandler(_KnobColorChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of KnobColor changes
		/// </summary>
		/// <param name="args">The event args (not used)</param>
		protected virtual void OnKnobColorChanged(EventArgs args)
		{
			(Events[_KnobColorChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the pointer color
		/// </summary>
		[Description("Indicates the pointer color of the control")]
		[Category("Appearance")]
		public Color PointerColor {
			get { return _pointerColor; }
			set {
				if (value != _pointerColor)
				{
					_pointerColor = value;
					Invalidate();
					OnPointerColorChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised when the value of PointerColor changes
		/// </summary>
		[Description("Raised when the value of PointerColor changes")]
		[Category("Behavior")]
		public event EventHandler PointerColorChanged {
			add { Events.AddHandler(_PointerColorChangedKey, value); }
			remove { Events.RemoveHandler(_PointerColorChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of PointerColor changes
		/// </summary>
		/// <param name="args">The event args (not used)</param>
		protected virtual void OnPointerColorChanged(EventArgs args)
		{
			(Events[_PointerColorChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the pointer width of the control
		/// </summary>
		[Description("Indicates the pointer width of the control")]
		[Category("Appearance")]
		[DefaultValue(1)]
		public int PointerWidth {
			get { return _pointerWidth; }
			set {
				if (_pointerWidth != value)
				{
					_pointerWidth = value;
					Invalidate();
					OnPointerWidthChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of PointerWidth changes
		/// </summary>
		[Description("Raised with the value of PointerWidth changes")]
		[Category("Behavior")]
		public event EventHandler PointerWidthChanged {
			add { Events.AddHandler(_PointerWidthChangedKey, value); }
			remove { Events.RemoveHandler(_PointerWidthChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of PointerWidth changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnPointerWidthChanged(EventArgs args)
		{
			(Events[_PointerWidthChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the pointer offset for the control
		/// </summary>
		[Description("Indicates the pointer offset for the control")]
		[Category("Appearance")]
		[DefaultValue(0)]
		public int PointerOffset {
			get { return _pointerOffset; }
			set {
				if (_pointerOffset != value)
				{
					_pointerOffset = value;
					Invalidate();
					OnPointerOffsetChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of PointerOffset changes
		/// </summary>
		[Description("Raised with the value of PointerOffset changes")]
		[Category("Behavior")]
		public event EventHandler PointerOffsetChanged {
			add { Events.AddHandler(_PointerOffsetChangedKey, value); }
			remove { Events.RemoveHandler(_PointerOffsetChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of PointerOffset changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnPointerOffsetChanged(EventArgs args)
		{
			(Events[_PointerOffsetChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the border width of the control
		/// </summary>
		[Description("Indicates the border width of the control")]
		[Category("Appearance")]
		[DefaultValue(1)]
		public int BorderWidth {
			get { return _borderWidth; }
			set {
				if (_borderWidth != value)
				{
					_borderWidth = value;
					Invalidate();
					OnBorderWidthChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of BorderWidth changes
		/// </summary>
		[Description("Raised with the borderWidth of BorderWidth changes")]
		[Category("Behavior")]
		public event EventHandler BorderWidthChanged {
			add { Events.AddHandler(_BorderWidthChangedKey, value); }
			remove { Events.RemoveHandler(_BorderWidthChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of BorderWidth changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnBorderWidthChanged(EventArgs args)
		{
			(Events[_BorderWidthChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the pointer start line cap of the control
		/// </summary>
		[Description("Indicates the pointer start line cap of the control")]
		[Category("Appearance")]
		[DefaultValue(LineCap.Round)]
		public LineCap PointerStartCap {
			get { return _pointerStartCap; }
			set {
				if (_pointerStartCap != value)
				{
					_pointerStartCap = value;
					Invalidate();
					OnPointerStartCapChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of PointerStartCap changes
		/// </summary>
		[Description("Raised with the value of PointerStartCap changes")]
		[Category("Behavior")]
		public event EventHandler PointerStartCapChanged {
			add { Events.AddHandler(_PointerStartCapChangedKey, value); }
			remove { Events.RemoveHandler(_PointerStartCapChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of PointerStartCap changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnPointerStartCapChanged(EventArgs args)
		{
			(Events[_PointerStartCapChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the pointer end line cap of the control
		/// </summary>
		[Description("Indicates the pointer end line cap of the control")]
		[Category("Appearance")]
		[DefaultValue(LineCap.Triangle)]
		public LineCap PointerEndCap {
			get { return _pointerEndCap; }
			set {
				if (_pointerEndCap != value)
				{
					_pointerEndCap = value;
					Invalidate();
					OnPointerEndCapChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of PointerEndCap changes
		/// </summary>
		[Description("Raised with the value of PointerEndCap changes")]
		[Category("Behavior")]
		public event EventHandler PointerEndCapChanged {
			add { Events.AddHandler(_PointerEndCapChangedKey, value); }
			remove { Events.RemoveHandler(_PointerEndCapChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of PointerEndCap changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnPointerEndCapChanged(EventArgs args)
		{
			(Events[_PointerEndCapChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the minimum value for the control
		/// </summary>
		[Description("Indicates the minimum angle for the control")]
		[Category("Appearance")]
		[DefaultValue(30)]
		public int MinimumAngle {
			get { return _minimumAngle; }
			set {
				if (_minimumAngle != value)
				{
					_minimumAngle = value;
					Invalidate();
					OnMinimumAngleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of MinimumAngle changes
		/// </summary>
		[Description("Raised with the value of MinimumAngle changes")]
		[Category("Behavior")]
		public event EventHandler MinimumAngleChanged {
			add { Events.AddHandler(_MinimumAngleChangedKey, value); }
			remove { Events.RemoveHandler(_MinimumAngleChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of MinimumAngle changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnMinimumAngleChanged(EventArgs args)
		{
			(Events[_MinimumAngleChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the maximum angle for the control
		/// </summary>
		[Description("Indicates the maximum angle for the control")]
		[Category("Appearance")]
		[DefaultValue(330)]
		public int MaximumAngle {
			get { return _maximumAngle; }
			set {
				if (_maximumAngle != value)
				{
					_maximumAngle = value;
					Invalidate();
					OnMaximumAngleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of MaximumAngle changes
		/// </summary>
		[Description("Raised with the value of MaximumAngle changes")]
		[Category("Behavior")]
		public event EventHandler MaximumAngleChanged {
			add { Events.AddHandler(_MaximumAngleChangedKey, value); }
			remove { Events.RemoveHandler(_MaximumAngleChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of MaximumAngle changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnMaximumAngleChanged(EventArgs args)
		{
			(Events[_MaximumAngleChangedKey] as EventHandler)?.Invoke(this, args);
		}
		internal bool TicksVisible {
			get { return _hasTicks && 0<_tickHeight && 0<_tickWidth; }
		}
		/// <summary>
		/// Indicates whether or not the control displays tick marks
		/// </summary>
		[Description("Indicates whether or not the control displays tick marks")]
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool HasTicks {
			get { return _hasTicks; }
			set {
				if (_hasTicks != value)
				{
					_hasTicks = value;
					Invalidate();
					OnHasTicksChanged(EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Raised with the value of HasTicks changes
		/// </summary>
		[Description("Raised with the value of HasTicks changes")]
		[Category("Behavior")]
		public event EventHandler HasTicksChanged {
			add { Events.AddHandler(_HasTicksChangedKey, value); }
			remove { Events.RemoveHandler(_HasTicksChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of HasTicks changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnHasTicksChanged(EventArgs args)
		{
			(Events[_HasTicksChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the height of the tick marks
		/// </summary>
		[Description("Indicates the height of the tick marks")]
		[Category("Appearance")]
		[DefaultValue(2)]
		public int TickHeight {
			get { return _tickHeight; }
			set {
				if (_tickHeight != value)
				{
					_tickHeight = value;
					Invalidate();
					OnTickHeightChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of TickHeight changes
		/// </summary>
		[Description("Raised with the value of TickHeight changes")]
		[Category("Behavior")]
		public event EventHandler TickHeightChanged {
			add { Events.AddHandler(_TickHeightChangedKey, value); }
			remove { Events.RemoveHandler(_TickHeightChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of TickHeight changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnTickHeightChanged(EventArgs args)
		{
			(Events[_TickHeightChangedKey] as EventHandler)?.Invoke(this, args);
		}
		/// <summary>
		/// Indicates the width of the tick marks
		/// </summary>
		[Description("Indicates the width of the tick marks")]
		[Category("Appearance")]
		[DefaultValue(1)]
		public int TickWidth {
			get { return _tickWidth; }
			set {
				if (_tickWidth != value)
				{
					_tickWidth = value;
					Invalidate();
					OnTickWidthChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of TickWidth changes
		/// </summary>
		[Description("Raised with the value of TickWidth changes")]
		[Category("Behavior")]
		public event EventHandler TickWidthChanged {
			add { Events.AddHandler(_TickWidthChangedKey, value); }
			remove { Events.RemoveHandler(_TickWidthChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of TickWidth changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnTickWidthChanged(EventArgs args)
		{
			(Events[_TickWidthChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Indicates the color of the tick marks
		/// </summary>
		[Description("Indicates the color of the tick marks")]
		[Category("Appearance")]
		public Color TickColor {
			get { return _tickColor; }
			set {
				if (_tickColor != value)
				{
					_tickColor = value;
					Invalidate();
					OnTickColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised with the value of TickColor changes
		/// </summary>
		[Description("Raised with the value of TickColor changes")]
		[Category("Behavior")]
		public event EventHandler TickColorChanged {
			add { Events.AddHandler(_TickColorChangedKey, value); }
			remove { Events.RemoveHandler(_TickColorChangedKey, value); }
		}
		/// <summary>
		/// Called when the value of TickColor changes
		/// </summary>
		/// <param name="args">The event args to use</param>
		protected virtual void OnTickColorChanged(EventArgs args)
		{
			(Events[_TickColorChangedKey] as EventHandler)?.Invoke(this, args);
		}

		/// <summary>
		/// Called when the control needs to be painted
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnPaint(PaintEventArgs args)
		{
			if (_renderTarget == null)
			{
				if (IsHandleCreated && D2DContext.Factory != null)
					CreateRenderTarget();
				if (_renderTarget == null) return;
			}

			// 角度計算（GDI+ 版と同一ロジック）
			float knobMinAngle = _minimumAngle;
			float knobMaxAngle = _maximumAngle;
			if (knobMinAngle < 0)  knobMinAngle = 360 + knobMinAngle;
			if (knobMaxAngle <= 0) knobMaxAngle = 360 + knobMaxAngle;

			double offset = 0.0;
			int min = Minimum, max = Maximum;
			var knobRange  = knobMaxAngle - knobMinAngle;
			double valueRange = max - min;
			double valueRatio = knobRange / valueRange;
			if (0 > min) offset = -min;

			var knobRect = ClientRectangle;
			knobRect.Inflate(-1, -1);
			var orr = knobRect;
			if (TicksVisible)
				knobRect.Inflate(new Size(-_tickHeight - 2, -_tickHeight - 2));

			var size   = (float)Math.Min(knobRect.Width - 4, knobRect.Height - 4);
			knobRect.X += 2;
			knobRect.Y += 2;
			var radius = size / 2f;
			var origin = new Vector2(knobRect.Left + radius, knobRect.Top + radius);

			double q     = ((Value + offset) * valueRatio) + knobMinAngle;
			double angle = q + 90d;
			if (angle > 360.0) angle -= 360.0;
			double angrad = angle * (Math.PI / 180d);
			double adj    = 1;
			if (_pointerEndCap != LineCap.NoAnchor) adj += _pointerWidth / 2d;
			var x1 = (float)(origin.X + (_pointerOffset - adj) * Math.Cos(angrad));
			var y1 = (float)(origin.Y + (_pointerOffset - adj) * Math.Sin(angrad));
			var x2 = (float)(origin.X + (radius - adj) * Math.Cos(angrad));
			var y2 = (float)(origin.Y + (radius - adj) * Math.Sin(angrad));

			var rt = _renderTarget;
			rt.BeginDraw();
			try
			{
				rt.Transform = Matrix3x2.CreateScale(GetRenderScaleX(), GetRenderScaleY());
				// 背景
				using (var backBrush = rt.CreateSolidColorBrush(ToColor4(BackColor)))
					rt.FillRectangle(
						new Vortice.Mathematics.Rect(orr.Left - 1, orr.Top - 1, orr.Width + 2, orr.Height + 2),
						backBrush);

				// ボーダー（円）
				var borderR       = radius - _borderWidth / 2f;
				var borderEllipse = new Ellipse(origin, borderR, borderR);
				using (var borderBrush = rt.CreateSolidColorBrush(ToColor4(_borderColor)))
					rt.DrawEllipse(borderEllipse, borderBrush, _borderWidth);

				// ノブ塗りつぶし
				var knobR       = radius - _borderWidth;
				var knobEllipse = new Ellipse(origin, knobR, knobR);
				using (var bgBrush = rt.CreateSolidColorBrush(ToColor4(_knobColor)))
					rt.FillEllipse(knobEllipse, bgBrush);

				// 現在値テキスト（ドラッグ調整中のみ表示）
				if (_dragging)
				{
					string valueText = _scale == 1f
						? Value.ToString()
						: (Value / _scale).ToString("0.##");
					float fontSize = Math.Max(6f, size / 5f) * 96f / 72f;
					using var tf = D2DContext.DWrite.CreateTextFormat(
						Font.Name,
						FontWeight.Normal, Vortice.DirectWrite.FontStyle.Normal, FontStretch.Normal,
						fontSize);
					using var layout = D2DContext.DWrite.CreateTextLayout(valueText, tf, float.MaxValue, float.MaxValue);
					var metrics = layout.Metrics;
					using (var textBrush = rt.CreateSolidColorBrush(ToColor4(ForeColor)))
						rt.DrawTextLayout(
							new Vector2(origin.X - metrics.Width / 2f, origin.Y - metrics.Height / 2f),
							layout, textBrush);
				}

				// ポインター
				using (var pointerBrush = rt.CreateSolidColorBrush(ToColor4(_pointerColor)))
					rt.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2),
						pointerBrush, _pointerWidth, GetPointerStrokeStyle());

				// 目盛り
				if (TicksVisible)
				{
					using var tickBrush = rt.CreateSolidColorBrush(ToColor4(_tickColor));
					for (var i = 0; i < _tickPositions.Length; ++i)
					{
						angle = ((_tickPositions[i] + offset) * valueRatio) + knobMinAngle + 90d;
						if (angle > 360.0) angle -= 360.0;
						angrad = angle * (Math.PI / 180d);
						var tx1 = origin.X + (radius + 2)               * (float)Math.Cos(angrad);
						var ty1 = origin.Y + (radius + 2)               * (float)Math.Sin(angrad);
						var tx2 = origin.X + (radius + _tickHeight + 2) * (float)Math.Cos(angrad);
						var ty2 = origin.Y + (radius + _tickHeight + 2) * (float)Math.Sin(angrad);
						rt.DrawLine(new Vector2(tx1, ty1), new Vector2(tx2, ty2), tickBrush, _tickWidth);
					}
				}

				// フォーカス矩形
				if (Focused)
				{
					float fw = Math.Min(Width, Height);
					using var focusBrush = rt.CreateSolidColorBrush(new Color4(0f, 0f, 0f, 1f));
					using var focusStyle = D2DContext.Factory.CreateStrokeStyle(
						new StrokeStyleProperties { DashStyle = Vortice.Direct2D1.DashStyle.Dot });
					rt.DrawRectangle(new Vortice.Mathematics.Rect(0, 0, fw, fw), focusBrush, 1f, focusStyle);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Knob draw error: {ex.Message}");
			}
			finally
			{
				rt.Transform = Matrix3x2.Identity;
				try { rt.EndDraw(); }
				catch
				{
					DisposeDeviceResources();
					if (IsHandleCreated && !IsDisposed && D2DContext.Factory != null)
						CreateRenderTarget();
					BeginInvoke((Action)Invalidate);
				}
			}
		}
		
		/// <summary>
		/// Called when a mouse button is pressed
		/// </summary>
		/// <param name="args"></param>
		protected override void OnMouseDown(MouseEventArgs args)
		{
			Focus();
			if (MouseButtons.Left == (args.Button & MouseButtons.Left)) {
				var knobRect = ClientRectangle;
				// adjust the client rect so it doesn't overhang
				knobRect.Inflate(-1, -1);
				if (TicksVisible)
					knobRect.Inflate(-_tickHeight-2, -_tickHeight-2);
				var size = (float)Math.Min(knobRect.Width - 4, knobRect.Height - 4);
				knobRect.X += 2;
				knobRect.Y += 2;
				var radius = size / 2f;
				var origin = new PointF(knobRect.Left + radius, knobRect.Top + radius);
				if (radius > _GetLineDistance(origin, new PointF(args.X, args.Y)))
				{
					_dragHit = args.Location;
					_dragging = true;
				}
			}
			base.OnMouseDown(args);
		}
		/// <summary>
		/// Called when a mouse button is released
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnMouseUp(MouseEventArgs args)
		{
			_dragging = false;
			Invalidate();
			base.OnMouseUp(args);
		}
		/// <summary>
		/// Called when a mouse button is moved
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnMouseMove(MouseEventArgs args)
		{
			// TODO: Improve Ctrl+Drag
			if (_dragging)
			{
				int opos = Value;
				int pos = opos;
				var delta = _dragHit.Y - args.Location.Y;

				// Ctrl: 微調整（1単位）、通常: LargeChange単位
				if (Keys.Control != (ModifierKeys & Keys.Control))
					delta *= LargeChange;

				pos += delta;
				if (pos < Minimum) pos = Minimum;
				if (pos > Maximum) pos = Maximum;
				if (pos != opos)
				{
					Value = pos;
					_dragHit = args.Location;
				}
			}
			base.OnMouseMove(args);
		}
		/// <summary>
		/// Called when the mouse wheel is scrolled
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnMouseWheel(MouseEventArgs args)
		{
			int pos;
			int step = LargeChange; // LargeChange単位で変化

			if (args.Delta > 0)
			{
				pos = Math.Min(Value + step, Maximum);
				Value = pos;
			}
			else if (args.Delta < 0)
			{
				pos = Math.Max(Value - step, Minimum);
				Value = pos;
			}
			base.OnMouseWheel(args);
		}
		/// <summary>
		/// Called when a key is pressed
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnKeyDown(KeyEventArgs args)
		{
			Focus();
			if(Keys.PageDown==(args.KeyCode & Keys.PageDown))
			{
				var v = Value;
				var i = 0;
				for(;i<_tickPositions.Length;i++)
				{
					var t = _tickPositions[i];
					if (t >= v)
						break;
				}
				if (1 > i)
					i = 1;
				Value = _tickPositions[i - 1];
			}
			if (Keys.PageUp == (args.KeyCode & Keys.PageUp))
			{
				var v = Value;
				var i = 0;
				for (; i < _tickPositions.Length; i++)
				{
					var t = _tickPositions[i];
					if (t > v)
						break;
				}
				if (_tickPositions.Length <= i)
					i = _tickPositions.Length - 1;
				Value = _tickPositions[i];
			}

			if (Keys.Home == (args.KeyCode & Keys.Home))
			{
				Value = Minimum;
			}
			if (Keys.End== (args.KeyCode & Keys.End))
			{
				Value = Maximum;
			}
			base.OnKeyDown(args);
		}
		/// <summary>
		/// Called when a command key is pressed
		/// </summary>
		/// <param name="msg">The message</param>
		/// <param name="keyData">The command key(s)</param>
		/// <returns>True if handled, otherwise false</returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Focus();
			int pos;
			var handled = false;

			// BUG: Right arrow doesn't seem to be working!
			if (Keys.Up == (keyData & Keys.Up) || Keys.Right == (keyData & Keys.Right))
			{
				pos = Value+1;
				if (pos < Maximum)
				{
					Value = pos;
				}
				else
					Value = Maximum;
				handled = true;
			}
			
			if (Keys.Down == (keyData & Keys.Down) || Keys.Left == (keyData & Keys.Left))
			{
				pos = Value-1;
				if (pos > Minimum)
				{
					Value = pos;
				}
				else
					Value = Minimum;
				handled = true;
			}
			if (handled)
				return true;
			return base.ProcessCmdKey(ref msg, keyData);
		}
		/// <summary>
		/// Called when the control is resized
		/// </summary>
		/// <param name="args">The event arguments</param>
		protected override void OnResize(EventArgs args)
		{
			if (_renderTarget != null && Width > 0 && Height > 0)
				_renderTarget.Resize(GetClientPixelSize());
			Invalidate();
			base.OnResize(args);
		}
		/// <summary>
		/// Called when the control's size changes
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnSizeChanged(EventArgs args)
		{
			SuspendLayout();
			int edge = Math.Max(1, Math.Min(Width, Height));
			if (Width != edge || Height != edge)
				Size = new Size(edge, edge);
			ResumeLayout(true);
			base.OnSizeChanged(args);
		}

		private float GetRenderScaleX()
		{
			int width = Math.Max(1, Width);
			return GetClientPixelSize().Width / (float)width;
		}

		private float GetRenderScaleY()
		{
			int height = Math.Max(1, Height);
			return GetClientPixelSize().Height / (float)height;
		}

		private Vortice.Mathematics.SizeI GetClientPixelSize()
		{
			NativeMethods.GetClientRect(Handle, out var rect);
			int width = Math.Max(1, rect.Right - rect.Left);
			int height = Math.Max(1, rect.Bottom - rect.Top);
			return new Vortice.Mathematics.SizeI(width, height);
		}

		private static class NativeMethods
		{
			[System.Runtime.InteropServices.DllImport("user32.dll")]
			[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
			public static extern bool GetClientRect(IntPtr hWnd, out NativeRect lpRect);
		}

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
		private struct NativeRect
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		/// <summary>
		/// Called when the control receives focus
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnEnter(EventArgs args)
		{
			Invalidate();
			base.OnEnter(args);
		}
		/// <summary>
		/// Called when the control loses focus
		/// </summary>
		/// <param name="args">The event args</param>
		protected override void OnLeave(EventArgs args)
		{
			Invalidate();
			base.OnLeave(args);
		}
		
		static RectangleF _GetCircleRect(float x, float y, float r)
		{
			return new RectangleF(x - r, y - r, r * 2, r * 2);
		}
		static float _GetLineDistance(PointF p1, PointF p2)
		{
			var xdist = p1.X - p2.X;
			var ydist = p1.Y - p2.Y;
			return (float)Math.Sqrt(xdist * xdist + ydist * ydist);
		}
		void _RecomputeTicks()
		{
			var tickCount = (int)Math.Ceiling((double)((Maximum - Minimum + 1) / _largeChange));
			var ticks = new int[tickCount+1];
			ticks[0] = Minimum;
			var t = Minimum;
			for(var i = 1;i<ticks.Length;i++)
			{
				t += _largeChange;
				t = Math.Min(t, Maximum);
				ticks[i] = t;
			}
			_tickPositions = ticks;
		}

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			if (MouseButtons.Left == (args.Button & MouseButtons.Left))
			{
				using (var form = new KnobInputForm(
					Value,
					Minimum,
					Maximum,
					_parameterName,
					_unit,
					_scale))
				{
					var screenPos = PointToScreen(new Point(Width, 0));
					form.StartPosition = FormStartPosition.Manual;
					form.Location = screenPos;

					if (form.ShowDialog() == DialogResult.OK)
						Value = form.InputValue;
				}
			}
			base.OnMouseDoubleClick(args);
		}
	}
}
