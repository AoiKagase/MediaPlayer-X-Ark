#region License

/* Copyright (c) 2017 Fabrice Lacharme
 * This code is inspired from Michal Brylka 
 * https://www.codeproject.com/Articles/17395/Owner-drawn-trackbar-slider
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Fabrice Lacharme
 * Email: fabrice.lacharme@gmail.com
 */

#endregion


using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using MediaPlayer_X_Ark.Engine.Render;
using Vortice.DCommon;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.Mathematics;
// 曖昧参照を解消するエイリアス（既存コードは System.Drawing/WinForms 型を優先）
using Color       = System.Drawing.Color;
using Size        = System.Drawing.Size;
using Orientation = System.Windows.Forms.Orientation;
using DashStyle   = Vortice.Direct2D1.DashStyle;
using FontStyle   = Vortice.DirectWrite.FontStyle;

namespace ColorSlider
{
    /* Original code from Michal Brylka on Code Project
    * see https://www.codeproject.com/Articles/17395/Owner-drawn-trackbar-slider
    * ColorSlider is a trackbar control written in C# as a replacement of the trackbar 
    * 
    * CodeProject: https://www.codeproject.com/Tips/1193311/Csharp-Slider-Trackbar-Control-using-Windows-Forms
    * Github: https://github.com/fabricelacharme/ColorSlider
    * 
    * 20/11/17 - version 1.0.O.1
    * 
    * Fixed: erroneous vertical display in case of minimum <> 0 (negative or positive)
    * Modified: DrawColorSlider, OnMouseMove
    * 
    * Added: Ticks display transformations
    * - TickAdd: allow to add a fixed value to the graduations: 
    *       usage: transform K = °C + 273,15, or °F = 1,8°C + 32   K = (°F + 459,67) / 1,8
    * - TickDivide: allow to diveide by a fixed value the graduations 
    *       usage: divide by 1000 => display graduations in kilograms when in gram
    *       
    *       
    * 10/12/17 - version 1.0.0.2
    * Added ForeColor property to graduations text color
    * 
    * 29/11/2019 - Version 1.0.0.3
    * Scale accept decimal negative values (-0.5 to 5.5 for ex)
    * 
    */

    /// <summary>
    /// Encapsulates control that visualy displays certain integer value and allows user to change it within desired range. It imitates <see cref="System.Windows.Forms.TrackBar"/> as far as mouse usage is concerned.
    /// </summary>
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Scroll"), DefaultProperty("BarInnerColor")]
    public partial class ColorSlider : TrackBar
    {
        #region Events

        /// <summary>
        /// Fires when Slider position has changed
        /// </summary>
        [Description("Event fires when the Value property changes")]
        [Category("Action")]
        public new event EventHandler ValueChanged;

        /// <summary>
        /// Fires when user scrolls the Slider
        /// </summary>
        [Description("Event fires when the Slider position is changed")]
        [Category("Behavior")]
        public new event ScrollEventHandler Scroll;

        #endregion


        #region D2D resources

        private ID2D1HwndRenderTarget _renderTarget;
        private ID2D1StrokeStyle      _dotStrokeStyle;
        private IDWriteTextFormat     _tickTextFormat;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (D2DContext.Factory == null) return;
            CreateRenderTarget();
            CreateDeviceResources();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeDeviceResources();
            base.OnHandleDestroyed(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_renderTarget != null && Width > 0 && Height > 0)
            {
                _renderTarget.Resize(new Vortice.Mathematics.SizeI(Width, Height));
                Invalidate();
            }
        }

        // D2D が背景も rt.Clear() で描画するため GDI の背景塗りは不要
        protected override void OnPaintBackground(PaintEventArgs e) { }

        protected override void WndProc(ref Message m)
        {
            const int WM_ERASEBKGND = 0x0014;
            if (m.Msg == WM_ERASEBKGND)
            {
                m.Result = IntPtr.Zero;
                return;
            }
            base.WndProc(ref m);
        }

        private void CreateRenderTarget()
        {
            if (!IsHandleCreated || D2DContext.Factory == null) return;
            DisposeDeviceResources();
            _renderTarget = D2DContext.Factory.CreateHwndRenderTarget(
                new RenderTargetProperties(),
                new HwndRenderTargetProperties
                {
                    Hwnd        = Handle,
                    PixelSize   = new Vortice.Mathematics.SizeI(Math.Max(1, Width), Math.Max(1, Height)),
                    PresentOptions = PresentOptions.None,
                });
        }

        private void CreateDeviceResources()
        {
            if (_renderTarget == null) return;
            _dotStrokeStyle = D2DContext.Factory.CreateStrokeStyle(
                new StrokeStyleProperties { DashStyle = DashStyle.Custom },
                new float[] { 0f, 2f });
            _tickTextFormat = D2DContext.DWrite.CreateTextFormat(
                Font.Name,
                FontWeight.Normal, FontStyle.Normal, FontStretch.Normal,
                Font.Size * 96f / 72f);
        }

        private void DisposeDeviceResources()
        {
            _tickTextFormat?.Dispose(); _tickTextFormat = null;
            _dotStrokeStyle?.Dispose(); _dotStrokeStyle = null;
            _renderTarget?.Dispose();   _renderTarget   = null;
        }

        private static Color4 ToColor4(Color c)
            => new Color4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

        // BackColor が透明の場合、親を辿って実際の背景色を取得する
        private Color GetEffectiveBackColor()
        {
            var c = BackColor;
            Control p = Parent;
            while (c.A == 0 && p != null)
            {
                c = p.BackColor;
                p = p.Parent;
            }
            return c.A == 0 ? SystemColors.Control : c;
        }

        #endregion

        #region Properties

        private Rectangle barRect; //bounding rectangle of bar area
        private Rectangle barHalfRect;
        private Rectangle thumbHalfRect;
        private Rectangle elapsedRect; //bounding rectangle of elapsed area

        // Margin left & right (bottom & Top)
        private int OffsetL = 0;
        private int OffsetR = 0;

        #region thumb

        private Rectangle thumbRect; //bounding rectangle of thumb area
        /// <summary>
        /// Gets the thumb rect. Usefull to determine bounding rectangle when creating custom thumb shape.
        /// </summary>
        /// <value>The thumb rect.</value>
        [Browsable(false)]
        public Rectangle ThumbRect
        {
            get { return thumbRect; }
        }

        private Size _thumbSize = new Size(16, 16);

        /// <summary>
        /// Gets or sets the size of the thumb.
        /// </summary>
        /// <value>The size of the thumb.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is lower than zero or grather than half of appropiate dimension</exception>
        [Description("Set Slider thumb size")]
        [Category("ColorSlider")]
        [DefaultValue(16)]
        public Size ThumbSize
        {
            get { return _thumbSize; }
            set
            {
                int h = value.Height;
                int w = value.Width;
                if (h > 0 && w > 0)
                {
                    _thumbSize = new Size(w, h);
                }
                else
                    throw new ArgumentOutOfRangeException(
                        "TrackSize has to be greather than zero and lower than half of Slider width");                 

                Invalidate();
            }
        }

        private GraphicsPath _thumbCustomShape = null;
        /// <summary>
        /// Gets or sets the thumb custom shape. Use ThumbRect property to determine bounding rectangle.
        /// </summary>
        /// <value>The thumb custom shape. null means default shape</value>
        [Description("Set Slider's thumb's custom shape")]
        [Category("ColorSlider")]
        [Browsable(false)]
        [DefaultValue(typeof(GraphicsPath), "null")]
        public GraphicsPath ThumbCustomShape
        {
            get { return _thumbCustomShape; }
            set
            {
                _thumbCustomShape = value;
                //_thumbSize = (int) (_barOrientation == Orientation.Horizontal ? value.GetBounds().Width : value.GetBounds().Height) + 1;
                _thumbSize = new Size((int)value.GetBounds().Width, (int)value.GetBounds().Height);

                Invalidate();
            }
        }

        private Size _thumbRoundRectSize = new Size(16, 16);
        /// <summary>
        /// Gets or sets the size of the thumb round rectangle edges.
        /// </summary>
        /// <value>The size of the thumb round rectangle edges.</value>
        [Description("Set Slider's thumb round rect size")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Size), "16; 16")]
        public Size ThumbRoundRectSize
        {
            get { return _thumbRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                _thumbRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private Size _borderRoundRectSize = new Size(8, 8);
        /// <summary>
        /// Gets or sets the size of the border round rect.
        /// </summary>
        /// <value>The size of the border round rect.</value>
        [Description("Set Slider's border round rect size")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Size), "8; 8")]
        public Size BorderRoundRectSize
        {
            get { return _borderRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                _borderRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private bool _drawSemitransparentThumb = true;
        /// <summary>
        /// Gets or sets a value indicating whether to draw semitransparent thumb.
        /// </summary>
        /// <value><c>true</c> if semitransparent thumb should be drawn; otherwise, <c>false</c>.</value>
        [Description("Set whether to draw semitransparent thumb")]
        [Category("ColorSlider")]
        [DefaultValue(true)]
        public bool DrawSemitransparentThumb
        {
            get { return _drawSemitransparentThumb; }
            set
            {
                _drawSemitransparentThumb = value;
                Invalidate();
            }
        }

        private Image _thumbImage = null;
        //private Image _thumbImage = Properties.Resources.BTN_Thumb_Blue;
        /// <summary>
        /// Gets or sets the Image used to render the thumb.
        /// </summary>
        /// <value>the thumb Image</value> 
        [Description("Set to use a specific Image for the thumb")]
        [Category("ColorSlider")]
        [DefaultValue(null)]
        public Image ThumbImage
        {
            get { return _thumbImage; }
            set
            {
                if (value != null)
                    _thumbImage = value;
                else
                    _thumbImage = null;
                Invalidate();
            }
        }

        #endregion


        #region Appearance

        private int _padding = 0;
        /// <summary>
        /// Gets or sets the padding (inside margins: left & right or bottom & top)
        /// </summary>
        /// <value>The padding.</value>
        [Description("Set Slider padding (inside margins: left & right or bottom & top)")]
        [Category("ColorSlider")]
        [DefaultValue(0)]
        public new int Padding
        {
            get { return _padding; }
            set
            {
                if (_padding != value)
                {
                    _padding = value;                    
                    OffsetL = OffsetR = _padding;

                    Invalidate();
                }
            }
        }

        private Orientation _barOrientation = Orientation.Horizontal;
        /// <summary>
        /// Gets or sets the orientation of Slider.
        /// </summary>
        /// <value>The orientation.</value>
        [Description("Set Slider orientation")]
        [Category("ColorSlider")]
        [DefaultValue(Orientation.Horizontal)]
        public new Orientation Orientation
        {
            get { return _barOrientation; }
            set
            {
                if (_barOrientation != value)
                {
                    _barOrientation = value;
                    // Switch from horizontal to vertical (design mode)
					// Comment these lines if problems in Run mode
                    if (this.DesignMode)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }

                    Invalidate();
                }
            }
        }

        private bool _drawFocusRectangle = false;
        /// <summary>
        /// Gets or sets a value indicating whether to draw focus rectangle.
        /// </summary>
        /// <value><c>true</c> if focus rectangle should be drawn; otherwise, <c>false</c>.</value>
        [Description("Set whether to draw focus rectangle")]
        [Category("ColorSlider")]
        [DefaultValue(false)]
        public bool DrawFocusRectangle
        {
            get { return _drawFocusRectangle; }
            set
            {
                _drawFocusRectangle = value;
                Invalidate();
            }
        }

        private bool _mouseEffects = true;
        /// <summary>
        /// Gets or sets whether mouse entry and exit actions have impact on how control look.
        /// </summary>
        /// <value><c>true</c> if mouse entry and exit actions have impact on how control look; otherwise, <c>false</c>.</value>
        [Description("Set whether mouse entry and exit actions have impact on how control look")]
        [Category("ColorSlider")]
        [DefaultValue(true)]
        public bool MouseEffects
        {
            get { return _mouseEffects; }
            set
            {
                _mouseEffects = value;
                Invalidate();
            }
        }

        #endregion


        #region values

        private decimal _trackerValue = 30;
        /// <summary>
        /// Gets or sets the value of Slider.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is outside appropriate range (min, max)</exception>
        [Description("Set Slider value")]
        [Category("ColorSlider")]
        [DefaultValue(30)]
        public new decimal Value
        {
            get { return _trackerValue; }
            set
            {
                if (value >= _minimum & value <= _maximum)
                {
                    _trackerValue = value;
                    if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
            }
        }

        private decimal _minimum = 0;
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when minimal value is greather than maximal one</exception>
        [Description("Set Slider minimal point")]
        [Category("ColorSlider")]
        [DefaultValue(0)]
        public new decimal Minimum
        {
            get { return _minimum; }
            set
            {
                if (value < _maximum)
                {
                    _minimum = value;
                    if (_trackerValue < _minimum)
                    {
                        _trackerValue = _minimum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Minimal value is greather than maximal one");
            }
        }

        private decimal _maximum = 100;
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when maximal value is lower than minimal one</exception>
        [Description("Set Slider maximal point")]
        [Category("ColorSlider")]
        [DefaultValue(100)]
        public new decimal Maximum
        {
            get { return _maximum; }
            set
            {
                if (value > _minimum)
                {
                    _maximum = value;
                    if (_trackerValue > _maximum)
                    {
                        _trackerValue = _maximum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                //else throw new ArgumentOutOfRangeException("Maximal value is lower than minimal one");
            }
        }

        private decimal _smallChange = 1;
        /// <summary>
        /// Gets or sets trackbar's small change. It affects how to behave when directional keys are pressed
        /// </summary>
        /// <value>The small change value.</value>
        [Description("Set trackbar's small change")]
        [Category("ColorSlider")]
        [DefaultValue(1)]
        public new decimal SmallChange
        {
            get { return _smallChange; }
            set { _smallChange = value; }
        }

        private decimal _largeChange = 5;
        /// <summary>
        /// Gets or sets trackbar's large change. It affects how to behave when PageUp/PageDown keys are pressed
        /// </summary>
        /// <value>The large change value.</value>
        [Description("Set trackbar's large change")]
        [Category("ColorSlider")]
        [DefaultValue(5)]
        public new decimal LargeChange
        {
            get { return _largeChange; }
            set { _largeChange = value; }
        }
           
        private int _mouseWheelBarPartitions = 10;
        /// <summary>
        /// Gets or sets the mouse wheel bar partitions.
        /// </summary>
        /// <value>The mouse wheel bar partitions.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value isn't greather than zero</exception>
        [Description("Set to how many parts is bar divided when using mouse wheel")]
        [Category("ColorSlider")]
        [DefaultValue(10)]
        public int MouseWheelBarPartitions
        {
            get { return _mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                    _mouseWheelBarPartitions = value;
                else throw new ArgumentOutOfRangeException("MouseWheelBarPartitions has to be greather than zero");
            }
        }

        #endregion


        #region colors

        private Color _thumbOuterColor = Color.White;
        /// <summary>
        /// Gets or sets the thumb outer color.
        /// </summary>
        /// <value>The thumb outer color.</value>
        [Description("Sets Slider thumb outer color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "White")]
        public Color ThumbOuterColor
        {
            get { return _thumbOuterColor; }
            set
            {
                _thumbOuterColor = value;
                Invalidate();
            }
        }

        private Color _thumbInnerColor = Color.FromArgb(21, 56, 152);
        /// <summary>
        /// Gets or sets the inner color of the thumb.
        /// </summary>
        /// <value>The inner color of the thumb.</value>
        [Description("Set Slider thumb inner color")]
        [Category("ColorSlider")]        
        public Color ThumbInnerColor
        {
            get { return _thumbInnerColor; }
            set
            {
                _thumbInnerColor = value;
                Invalidate();
            }
        }

        private Color _thumbPenColor = Color.FromArgb(21, 56, 152);
        /// <summary>
        /// Gets or sets the color of the thumb pen.
        /// </summary>
        /// <value>The color of the thumb pen.</value>
        [Description("Set Slider thumb pen color")]
        [Category("ColorSlider")]       
        public Color ThumbPenColor
        {
            get { return _thumbPenColor; }
            set
            {
                _thumbPenColor = value;
                Invalidate();
            }
        }

        private Color _barInnerColor = Color.Black;
        /// <summary>
        /// Gets or sets the inner color of the bar.
        /// </summary>
        /// <value>The inner color of the bar.</value>
        [Description("Set Slider bar inner color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BarInnerColor
        {
            get { return _barInnerColor; }
            set
            {
                _barInnerColor = value;
                Invalidate();
            }
        }

        private Color _elapsedPenColorTop = Color.FromArgb(95, 140, 180);   // bleu clair
        /// <summary>
        /// Gets or sets the top color of the Elapsed
        /// </summary>
        [Description("Gets or sets the top color of the elapsed")]
        [Category("ColorSlider")]
        public Color ElapsedPenColorTop 
        {
            get { return _elapsedPenColorTop; }
            set
            {
                _elapsedPenColorTop = value;
                Invalidate();
            }
        }

        private Color _elapsedPenColorBottom = Color.FromArgb(99, 130, 208);   // bleu très clair
        /// <summary>
        /// Gets or sets the bottom color of the elapsed
        /// </summary>
        [Description("Gets or sets the bottom color of the elapsed")]
        [Category("ColorSlider")]
        public Color ElapsedPenColorBottom 
        {
            get { return _elapsedPenColorBottom; }
            set
            {
                _elapsedPenColorBottom = value;
                Invalidate();
            }
        }

        private Color _barPenColorTop = Color.FromArgb(55, 60, 74);     // gris foncé
        /// <summary>
        /// Gets or sets the top color of the bar
        /// </summary>
        [Description("Gets or sets the top color of the bar")]
        [Category("ColorSlider")]
        public Color BarPenColorTop
        {
            get { return _barPenColorTop; }
            set
            {
                _barPenColorTop = value;
                Invalidate();
            }
        }

        private Color _barPenColorBottom = Color.FromArgb(87, 94, 110);    // gris moyen
        /// <summary>
        /// Gets or sets the bottom color of bar
        /// </summary>
        [Description("Gets or sets the bottom color of the bar")]
        [Category("ColorSlider")]
        public Color BarPenColorBottom
        {
            get { return _barPenColorBottom; }
            set
            {
                _barPenColorBottom = value;
                Invalidate();
            }
        }   

        private Color _elapsedInnerColor = Color.FromArgb(21, 56, 152);
        /// <summary>
        /// Gets or sets the inner color of the elapsed.
        /// </summary>
        /// <value>The inner color of the elapsed.</value>
        [Description("Set Slider's elapsed part inner color")]
        [Category("ColorSlider")]        
        public Color ElapsedInnerColor
        {
            get { return _elapsedInnerColor; }
            set
            {
                _elapsedInnerColor = value;
                Invalidate();
            }
        }

        private Color _tickColor = Color.White;
        /// <summary>
        /// Gets or sets the color of the graduations
        /// </summary>
        [Description("Color of graduations")]
        [Category("ColorSlider")]            
        public Color TickColor
        {
            get { return _tickColor; }
            set
            {
                if (value != _tickColor)
                {
                    _tickColor = value;
                    Invalidate();
                }
            }
        }

        #endregion


        #region divisions

        // For ex: if values are multiples of 50, 
        // values = 0, 50, 100, 150 etc...
        //set TickDivide to 50
        // And ticks will be displayed as 
        // values = 0, 1, 2, 3 etc...
        private float _tickDivide = 0;

        [Description("Gets or sets a value used to divide the graduation")]
        [Category("ColorSlider")]
        public float TickDivide
        {
            get { return _tickDivide; }
            set {
                _tickDivide = value;
                Invalidate();
            }
        }

        private float _tickAdd = 0;
        [Description("Gets or sets a value added to the graduation")]
        [Category("ColorSlider")]
        public float TickAdd
        {
            get { return _tickAdd; }
            set { _tickAdd = value;
                Invalidate();
            }
        }

        private TickStyle _tickStyle = TickStyle.TopLeft;
        /// <summary>
        /// Gets or sets where to display the ticks (None, both top-left, bottom-right)
        /// </summary>
        [Description("Gets or sets where to display the ticks")]
        [Category("ColorSlider")]
        [DefaultValue(TickStyle.TopLeft)]
        public new TickStyle TickStyle
        {
            get { return _tickStyle; }
            set {
                _tickStyle = value;
                Invalidate();
            }
        }

        private decimal _scaleDivisions = 10;
        /// <summary>
        /// How many divisions of maximum?
        /// </summary>
        [Description("Set the number of intervals between minimum and maximum")]
        [Category("ColorSlider")]
        public decimal ScaleDivisions
        {
            get { return _scaleDivisions; }
            set {
                if (value > 0)
                {
                    _scaleDivisions = value;                    
                }
                //else throw new ArgumentOutOfRangeException("TickFreqency must be > 0 and < Maximum");

                Invalidate();
            }
        }

        private decimal _scaleSubDivisions = 5;
        /// <summary>
        /// How many subdivisions for each division
        /// </summary>
        [Description("Set the number of subdivisions between main divisions of graduation.")]
        [Category("ColorSlider")]
        public decimal ScaleSubDivisions
        {
            get { return _scaleSubDivisions; }
            set
            {
                if (value > 0 && _scaleDivisions > 0 && (_maximum - _minimum) / ((value + 1) * _scaleDivisions) > 0)
                { 
                    _scaleSubDivisions = value;
                    
                }
                //else throw new ArgumentOutOfRangeException("TickSubFreqency must be > 0 and < TickFrequency");

                Invalidate();
            }
        }

        private bool _showSmallScale = false;
        /// <summary>
        /// Shows Small Scale marking.
        /// </summary>
        [Description("Show or hide subdivisions of graduations")]
        [Category("ColorSlider")]
        public bool ShowSmallScale
        {
            get { return _showSmallScale; }
            set {

                if (value == true)
                {
                    if (_scaleDivisions > 0 && _scaleSubDivisions > 0 && (_maximum - _minimum) / ((_scaleSubDivisions + 1) * _scaleDivisions) > 0)
                    {
                        _showSmallScale = value;
                        Invalidate();
                    }
                    else
                    {
                        _showSmallScale = false;
                    }
                }
                else
                {
                    _showSmallScale = value;
                    // need to redraw 
                    Invalidate();
                }
            }
        }

        private bool _showDivisionsText = true;
        /// <summary>
        /// Shows Small Scale marking.
        /// </summary>
        [Description("Show or hide text value of graduations")]
        [Category("ColorSlider")]
        public bool ShowDivisionsText
        {
            get { return _showDivisionsText; }
            set { _showDivisionsText = value;
                Invalidate();
            }
        }

        #endregion


        #region Font

        /// <summary>
        /// Get or Sets the Font of the Text being displayed.
        /// </summary>
        [Bindable(true),
        Browsable(true),
        Category("ColorSlider"),
        Description("Get or Sets the Font of the Text being displayed."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        EditorBrowsable(EditorBrowsableState.Always)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                Invalidate();
                OnFontChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Get or Sets the Font of the Text being displayed.
        /// </summary>
        [Bindable(true),
        Browsable(true),
        Category("ColorSlider"),
        Description("Get or Sets the Color of the Text being displayed."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        EditorBrowsable(EditorBrowsableState.Always)]
        public override Color ForeColor {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                Invalidate();
                OnForeColorChanged(EventArgs.Empty);
            }
        }

        #endregion

        #endregion


        #region Color schemas

        //define own color schemas
        private Color[,] aColorSchema = new Color[,]
            {
                {
                    Color.White,                    // thumb outer
                    Color.FromArgb(21, 56, 152),    // thumb inner
                    Color.FromArgb(21, 56, 152),    // thumb pen color
                    
                    Color.Black,                    // bar inner    

                    Color.FromArgb(95, 140, 180),     // slider elapsed top                   
                    Color.FromArgb(99, 130, 208),     // slider elapsed bottom                    

                    Color.FromArgb(55, 60, 74),     // slider remain top                    
                    Color.FromArgb(87, 94, 110),     // slider remain bottom
                                         
                    Color.FromArgb(21, 56, 152)     // elapsed interieur centre
                },
                {
                    Color.White,                    // thumb outer
                    Color.Red,    // thumb inner
                    Color.Red,    // thumb pen color
                    
                    Color.Black,                    // bar inner    

                    Color.LightCoral,     // slider elapsed top                   
                    Color.Salmon,     // slider elapsed bottom
                    

                    Color.FromArgb(55, 60, 74),     // slider remain top                    
                    Color.FromArgb(87, 94, 110),     // slider remain bottom
                                         
                    Color.Red     // gauche interieur centre
                },
                {
                    Color.White,                    // thumb outer
                    Color.Green,    // thumb inner
                    Color.Green,    // thumb pen color
                    
                    Color.Black,                    // bar inner    

                    Color.SpringGreen,     // slider elapsed top                   
                    Color.LightGreen,     // slider elapsed bottom
                    

                    Color.FromArgb(55, 60, 74),     // slider remain top                    
                    Color.FromArgb(87, 94, 110),     // slider remain bottom
                                         
                    Color.Green     // gauche interieur centre
                },
            };

        public enum ColorSchemas
        {
            BlueColors,
            RedColors,
            GreenColors
        }

        private ColorSchemas colorSchema = ColorSchemas.BlueColors;

        /// <summary>
        /// Sets color schema. Color generalization / fast color changing. Has no effect when slider colors are changed manually after schema was applied. 
        /// </summary>
        /// <value>New color schema value</value>
        [Description("Set Slider color schema. Has no effect when slider colors are changed manually after schema was applied.")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(ColorSchemas), "BlueColors")]
        public ColorSchemas ColorSchema
        {
            get { return colorSchema; }
            set
            {
                colorSchema = value;
                byte sn = (byte)value;
                _thumbOuterColor = aColorSchema[sn, 0];
                _thumbInnerColor = aColorSchema[sn, 1];
                _thumbPenColor = aColorSchema[sn, 2];
                
                _barInnerColor = aColorSchema[sn, 3];

                _elapsedPenColorTop = aColorSchema[sn, 4];
                _elapsedPenColorBottom = aColorSchema[sn, 5];

                _barPenColorTop = aColorSchema[sn, 6];
                _barPenColorBottom = aColorSchema[sn, 7];

                _elapsedInnerColor = aColorSchema[sn, 8];

                Invalidate();
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The current value.</param>
        public ColorSlider(decimal min, decimal max, decimal value)
        {
//          base.InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true);
            // D2D HwndRenderTarget が直接 HWND に描画するため、
            // OptimizedDoubleBuffer（GDI バックバッファ→BitBlt）は無効にする。
            // 有効のままだと空の GDI バッファが D2D 描画を上書きしてしまう。
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            
            // Default backcolor
            BackColor = Color.FromArgb(70, 77, 95);
            ForeColor = Color.White;

            // Font
            //this.Font = new Font("Tahoma", 6.75f);
            this.Font = new Font("Microsoft Sans Serif", 6f);

            Minimum = min;
            Maximum = max;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        public ColorSlider() : this(0, 100, 30) { }

        #endregion

        #region Paint

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_renderTarget == null)
            {
                if (IsHandleCreated && D2DContext.Factory != null)
                {
                    CreateRenderTarget();
                    CreateDeviceResources();
                }
                if (_renderTarget == null) return;
            }

            Color[] colors;
            if (!Enabled)
                colors = DesaturateColors(_thumbOuterColor, _thumbInnerColor, _thumbPenColor,
                    _barInnerColor, _elapsedPenColorTop, _elapsedPenColorBottom,
                    _barPenColorTop, _barPenColorBottom, _elapsedInnerColor);
            else if (_mouseEffects && mouseInRegion)
                colors = LightenColors(_thumbOuterColor, _thumbInnerColor, _thumbPenColor,
                    _barInnerColor, _elapsedPenColorTop, _elapsedPenColorBottom,
                    _barPenColorTop, _barPenColorBottom, _elapsedInnerColor);
            else
                colors = new[] { _thumbOuterColor, _thumbInnerColor, _thumbPenColor,
                    _barInnerColor, _elapsedPenColorTop, _elapsedPenColorBottom,
                    _barPenColorTop, _barPenColorBottom, _elapsedInnerColor };

            _renderTarget.BeginDraw();
            try
            {
                DrawColorSlider(colors[0], colors[1], colors[2], colors[3],
                    colors[4], colors[5], colors[6], colors[7], colors[8]);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ColorSlider draw error: {ex.Message}");
            }
            finally
            {
                try { _renderTarget?.EndDraw(); }
                catch
                {
                    // デバイスロスト等 → 即再生成して次フレームで描画
                    DisposeDeviceResources();
                    if (IsHandleCreated && !IsDisposed && D2DContext.Factory != null)
                    {
                        CreateRenderTarget();
                        CreateDeviceResources();
                    }
                    BeginInvoke((Action)Invalidate);
                }
            }
        }

        /// <summary>
        /// Draws the colorslider control using passed colors.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        /// <param name="thumbOuterColorPaint">The thumb outer color paint.</param>
        /// <param name="thumbInnerColorPaint">The thumb inner color paint.</param>
        /// <param name="thumbPenColorPaint">The thumb pen color paint.</param>
        /// <param name="barInnerColorPaint">The bar inner color paint.</param>
        /// <param name="barPenColorPaint">The bar pen color paint.</param>
        /// <param name="elapsedInnerColorPaint">The elapsed inner color paint.</param>
        private void DrawColorSlider(
            Color thumbOuterColorPaint, Color thumbInnerColorPaint, Color thumbPenColorPaint,
            Color barInnerColorPaint,
            Color ElapsedTopPenColorPaint, Color ElapsedBottomPenColorPaint,
            Color barTopPenColorPaint, Color barBottomPenColorPaint,
            Color elapsedInnerColorPaint)
        {
            var rt = _renderTarget;
            try
            {
                rt.Clear(ToColor4(GetEffectiveBackColor()));

                //adjust drawing rects
                barRect = ClientRectangle;

                //set up thumbRect approprietly
                if (_barOrientation == Orientation.Horizontal)
                {
                    #region horizontal
                    if (_thumbImage != null)
                    {
                        decimal TrackX = OffsetL + ((_trackerValue - _minimum) * (ClientRectangle.Width - OffsetL - OffsetR - _thumbImage.Width)) / (_maximum - _minimum);
                        thumbRect = new Rectangle((int)TrackX, ClientRectangle.Height/2 - _thumbImage.Height/2, _thumbImage.Width, _thumbImage.Height);                        
                    }
                    else
                    {
                        decimal TrackX = OffsetL + ((_trackerValue - _minimum) * (ClientRectangle.Width - OffsetL - OffsetR - _thumbSize.Width)) / (_maximum - _minimum);
                        thumbRect = new Rectangle((int)TrackX, barRect.Y + ClientRectangle.Height/2 - _thumbSize.Height/2 , _thumbSize.Width, _thumbSize.Height);                        
                    }                    
                    #endregion
                }
                else
                {
                    #region vertical
                    if (_thumbImage != null)
                    {                        
                        decimal TrackY = OffsetR + ((_maximum - _trackerValue) * (ClientRectangle.Height - OffsetL -OffsetR - _thumbImage.Height)) / (_maximum - _minimum);
                        thumbRect = new Rectangle(ClientRectangle.Width/2 - _thumbImage.Width/2, (int)TrackY, _thumbImage.Width, _thumbImage.Height);
                    }
                    else
                    {                        
                        decimal TrackY = OffsetR + ((_maximum - _trackerValue) * (ClientRectangle.Height - OffsetL - OffsetR - _thumbSize.Height)) / (_maximum - _minimum);
                        thumbRect = new Rectangle(barRect.X + ClientRectangle.Width/2 - _thumbSize.Width/2, (int)TrackY, _thumbSize.Width, _thumbSize.Height);
                    }
                    #endregion
                }

                thumbHalfRect = thumbRect;
                if (_barOrientation == Orientation.Horizontal)
                {
                    #region horizontal

                    barRect.X     = barRect.X + OffsetL;
                    barRect.Width = barRect.Width - OffsetL - OffsetR;

                    barHalfRect        = barRect;
                    barHalfRect.Height /= 2;

                    thumbHalfRect.Height /= 2;
                    elapsedRect        = barRect;
                    elapsedRect.Width  = (thumbRect.Left + _thumbSize.Width / 2) - OffsetL;

                    #endregion
                }
                else
                {
                    #region vertical

                    barRect.Y      = barRect.Y + OffsetR;
                    barRect.Height = barRect.Height - OffsetL - OffsetR;

                    barHalfRect       = barRect;
                    barHalfRect.Width /= 2;

                    thumbHalfRect.Width /= 2;
                    elapsedRect         = barRect;

                    elapsedRect.Height = barRect.Height - (thumbRect.Top + ThumbSize.Height / 2) + OffsetR;
                    elapsedRect.Y      = 1 + thumbRect.Top + ThumbSize.Height / 2 + OffsetR;

                    #endregion
                }
                
                //draw bar

                #region draw inner bar

                using (var br = rt.CreateSolidColorBrush(ToColor4(barInnerColorPaint)))
                {
                    if (_barOrientation == Orientation.Horizontal)
                        rt.DrawLine(new Vector2(barRect.X, barRect.Y + barRect.Height / 2f), new Vector2(barRect.X + barRect.Width, barRect.Y + barRect.Height / 2f), br, 1f);
                    else
                        rt.DrawLine(new Vector2(barRect.X + barRect.Width / 2f, barRect.Y), new Vector2(barRect.X + barRect.Width / 2f, barRect.Y + barRect.Height), br, 1f);
                }
                #endregion

                #region draw elapsed bar

                using (var br = rt.CreateSolidColorBrush(ToColor4(elapsedInnerColorPaint)))
                {
                    if (_barOrientation == Orientation.Horizontal)
                        rt.DrawLine(new Vector2(barRect.X, barRect.Y + barRect.Height / 2f), new Vector2(barRect.X + elapsedRect.Width, barRect.Y + barRect.Height / 2f), br, 1f);
                    else
                        rt.DrawLine(new Vector2(barRect.X + barRect.Width / 2f, barRect.Y + (barRect.Height - elapsedRect.Height)), new Vector2(barRect.X + barRect.Width / 2f, barRect.Y + barRect.Height), br, 1f);
                }
                #endregion

                #region draw external contours

                using (var brET  = rt.CreateSolidColorBrush(ToColor4(ElapsedTopPenColorPaint)))
                using (var brEB  = rt.CreateSolidColorBrush(ToColor4(ElapsedBottomPenColorPaint)))
                using (var brBT  = rt.CreateSolidColorBrush(ToColor4(barTopPenColorPaint)))
                using (var brBB  = rt.CreateSolidColorBrush(ToColor4(barBottomPenColorPaint)))
                {
                    if (_barOrientation == Orientation.Horizontal)
                    {
                        float cy = barRect.Y + barRect.Height / 2f;
                        rt.DrawLine(new Vector2(barRect.X, cy - 1), new Vector2(barRect.X + elapsedRect.Width, cy - 1), brET, 1f);
                        rt.DrawLine(new Vector2(barRect.X, cy + 1), new Vector2(barRect.X + elapsedRect.Width, cy + 1), brEB, 1f);
                        rt.DrawLine(new Vector2(barRect.X + elapsedRect.Width, cy - 1), new Vector2(barRect.X + barRect.Width, cy - 1), brBT, 1f);
                        rt.DrawLine(new Vector2(barRect.X + elapsedRect.Width, cy + 1), new Vector2(barRect.X + barRect.Width, cy + 1), brBB, 1f);
                        rt.DrawLine(new Vector2(barRect.X, cy - 1), new Vector2(barRect.X, cy + 1), brBT, 1f);
                        rt.DrawLine(new Vector2(barRect.X + barRect.Width, cy - 1), new Vector2(barRect.X + barRect.Width, cy + 1), brBB, 1f);
                    }
                    else
                    {
                        float cx = barRect.X + barRect.Width / 2f;
                        float elY = barRect.Y + (barRect.Height - elapsedRect.Height);
                        rt.DrawLine(new Vector2(cx - 1, elY), new Vector2(cx - 1, barRect.Y + barRect.Height), brET, 1f);
                        rt.DrawLine(new Vector2(cx + 1, elY), new Vector2(cx + 1, barRect.Y + barRect.Height), brEB, 1f);
                        rt.DrawLine(new Vector2(cx - 1, barRect.Y), new Vector2(cx - 1, elY), brBT, 1f);
                        rt.DrawLine(new Vector2(cx + 1, barRect.Y), new Vector2(cx + 1, elY), brBB, 1f);
                        rt.DrawLine(new Vector2(cx - 1, barRect.Y), new Vector2(cx + 1, barRect.Y), brBT, 1f);
                        rt.DrawLine(new Vector2(cx - 1, barRect.Y + barRect.Height), new Vector2(cx + 1, barRect.Y + barRect.Height), brBB, 1f);
                    }
                }
                #endregion

                

                #region draw thumb

                Color newthumbOuterColorPaint = thumbOuterColorPaint, newthumbInnerColorPaint = thumbInnerColorPaint;
                if (Capture && _drawSemitransparentThumb)
                {
                    newthumbOuterColorPaint = Color.FromArgb(175, thumbOuterColorPaint);
                    newthumbInnerColorPaint = Color.FromArgb(175, thumbInnerColorPaint);
                }

                // グラデーションブラシ（上→下の2ストップ、TileFlipXY ≒ Mirror）
                Rectangle gradRect = _barOrientation == Orientation.Horizontal ? thumbRect : thumbHalfRect;
                var stops = new GradientStop[]
                {
                    new GradientStop { Color = ToColor4(newthumbOuterColorPaint), Position = 0f },
                    new GradientStop { Color = ToColor4(newthumbInnerColorPaint), Position = 1f },
                };
                using var gradColl = rt.CreateGradientStopCollection(stops, Gamma.StandardRgb, ExtendMode.Mirror);
                using var lgbThumb = rt.CreateLinearGradientBrush(
                    new LinearGradientBrushProperties
                    {
                        StartPoint = new Vector2(gradRect.X, gradRect.Y),
                        EndPoint   = new Vector2(gradRect.X, gradRect.Y + gradRect.Height),
                    },
                    gradColl);

                // サム形状ジオメトリ
                using var thumbGeo = D2DContext.Factory.CreateRoundedRectangleGeometry(
                    new RoundedRectangle
                    {
                        Rect    = new Vortice.Mathematics.Rect(thumbRect.X, thumbRect.Y, thumbRect.Width, thumbRect.Height),
                        RadiusX = _thumbRoundRectSize.Width  / 2f,
                        RadiusY = _thumbRoundRectSize.Height / 2f,
                    });

                if (_thumbCustomShape != null)
                {
                    // カスタム形状：パス座標を Translate して描画
                    var bounds = _thumbCustomShape.GetBounds();
                    float dx = thumbRect.Left - bounds.Left;
                    float dy = thumbRect.Top  - bounds.Top;
                    rt.Transform = Matrix3x2.CreateTranslation(dx, dy);
                }

                rt.FillGeometry(thumbGeo, lgbThumb);

                Color newThumbPenColor = thumbPenColorPaint;
                if (_mouseEffects && (Capture || mouseInThumbRegion))
                    newThumbPenColor = ControlPaint.Dark(newThumbPenColor);

                if (_thumbImage != null)
                {
                    // サム画像：GDI+ ビットマップを D2D Bitmap に変換して描画
                    using var bmp = new Bitmap(_thumbImage);
                    bmp.MakeTransparent(Color.FromArgb(255, 0, 255));
                    using var d2dBmp = CreateD2DBitmapFromGdi(bmp);
                    if (d2dBmp != null)
                    {
                        var srcR = new Vortice.Mathematics.Rect(0, 0, bmp.Width, bmp.Height);
                        var dstR = new Vortice.Mathematics.Rect(thumbRect.X, thumbRect.Y, thumbRect.Width, thumbRect.Height);
                        rt.DrawBitmap(d2dBmp, dstR, 1f, BitmapInterpolationMode.Linear, srcR);
                    }
                }
                else
                {
                    using var thumbBrush = rt.CreateSolidColorBrush(ToColor4(newThumbPenColor));
                    rt.DrawGeometry(thumbGeo, thumbBrush, 1f);
                }

                if (_thumbCustomShape != null)
                    rt.Transform = Matrix3x2.Identity;

                #endregion draw thumb


                #region draw focusing rectangle

                if (Focused && _drawFocusRectangle && _dotStrokeStyle != null)
                {
                    Rectangle r = ClientRectangle;
                    r.Width -= 2; r.Height--; r.X++;
                    using var focusGeo = D2DContext.Factory.CreateRoundedRectangleGeometry(
                        new RoundedRectangle
                        {
                            Rect    = new Vortice.Mathematics.Rect(r.X, r.Y, r.Width, r.Height),
                            RadiusX = _borderRoundRectSize.Width  / 2f,
                            RadiusY = _borderRoundRectSize.Height / 2f,
                        });
                    var focusColor = Color.FromArgb(200, ElapsedTopPenColorPaint);
                    using var focusBrush = rt.CreateSolidColorBrush(ToColor4(focusColor));
                    rt.DrawGeometry(focusGeo, focusBrush, 1f, _dotStrokeStyle);
                }
                #endregion draw focusing rectangle


                #region draw ticks

                if (_tickStyle != TickStyle.None)
                {
                    int nbticks = 1 + (int)(_scaleDivisions * (_scaleSubDivisions + 1));
                    int interval = 0;
                    int start = 0;
                    int W = 0;
                    float rulerValue = 0;
                    int offset = 0;

                    if (_barOrientation == Orientation.Horizontal)
                    {
                        start      = thumbRect.Width / 2;
                        W          = barRect.Width - thumbRect.Width;
                        rulerValue = (float)_minimum;
                        offset     = 2 + thumbRect.Height / 2;
                    }
                    else
                    {
                        start      = thumbRect.Height / 2;
                        W          = barRect.Height - thumbRect.Height;
                        rulerValue = (float)_maximum;
                        offset     = 2 + thumbRect.Width / 2;
                    }

                    int scaleL = 5;
                    int scaleS = 3;
                    int idx    = 0;

                    using var tickBrush = rt.CreateSolidColorBrush(ToColor4(_tickColor));
                    using var textBrush = rt.CreateSolidColorBrush(ToColor4(ForeColor));

                    // テキストフォーマットが未作成なら生成
                    if (_tickTextFormat == null)
                        CreateDeviceResources();

                    string maxStr = String.Format("{0,0:##}", _maximum);
                    float maxW = 0, maxH = 0;
                    if (_tickTextFormat != null)
                    {
                        using var maxLayout = D2DContext.DWrite.CreateTextLayout(maxStr, _tickTextFormat, 9999f, 9999f);
                        maxW = maxLayout.Metrics.Width;
                        maxH = maxLayout.Metrics.Height;
                    }

                    for (int i = 0; i <= _scaleDivisions; i++)
                    {
                        float val = rulerValue;
                        if (_tickDivide != 0) val /= _tickDivide;
                        if (_tickAdd    != 0) val += _tickAdd;
                        string str = String.Format("{0:0.##}", val);

                        float tw = 0, th = 0;
                        IDWriteTextLayout layout = null;
                        if (_tickTextFormat != null && _showDivisionsText)
                        {
                            layout = D2DContext.DWrite.CreateTextLayout(str, _tickTextFormat, 9999f, 9999f);
                            tw = layout.Metrics.Width;
                            th = layout.Metrics.Height;
                        }

                        if (_barOrientation == Orientation.Horizontal)
                        {
                            float cy = barRect.Y + barRect.Height / 2f;
                            float cx = start + barRect.X + interval;

                            if (_showDivisionsText && layout != null)
                            {
                                if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                    rt.DrawTextLayout(new Vector2(cx - tw * 0.5f, cy - th - scaleL - offset), layout, textBrush);
                                if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                    rt.DrawTextLayout(new Vector2(cx - tw * 0.5f, cy + scaleL + offset), layout, textBrush);
                            }

                            if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                rt.DrawLine(new Vector2(cx, cy - scaleL - offset), new Vector2(cx, cy - offset), tickBrush, 1f);
                            if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                rt.DrawLine(new Vector2(cx, cy + offset), new Vector2(cx, cy + scaleL + offset), tickBrush, 1f);

                            rulerValue += (float)((_maximum - _minimum) / _scaleDivisions);

                            if (i < _scaleDivisions && _showSmallScale)
                            {
                                for (int j = 0; j <= _scaleSubDivisions; j++)
                                {
                                    idx++;
                                    interval = idx * W / (nbticks - 1);
                                    float sx = start + barRect.X + interval;
                                    if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                        rt.DrawLine(new Vector2(sx, cy - scaleS - offset), new Vector2(sx, cy - offset), tickBrush, 1f);
                                    if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                        rt.DrawLine(new Vector2(sx, cy + offset), new Vector2(sx, cy + scaleS + offset), tickBrush, 1f);
                                }
                            }
                        }
                        else
                        {
                            float cx2 = barRect.X + barRect.Width / 2f;
                            float sy  = start + barRect.Y + interval;

                            if (_showDivisionsText && layout != null)
                            {
                                if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                    rt.DrawTextLayout(new Vector2(cx2 - scaleL - tw - offset, sy - th * 0.5f), layout, textBrush);
                                if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                    rt.DrawTextLayout(new Vector2(cx2 + scaleL + offset, sy - th * 0.5f), layout, textBrush);
                            }

                            if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                rt.DrawLine(new Vector2(cx2 - scaleL - offset, sy), new Vector2(cx2 - offset, sy), tickBrush, 1f);
                            if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                rt.DrawLine(new Vector2(cx2 + offset, sy), new Vector2(cx2 + scaleL + offset, sy), tickBrush, 1f);

                            rulerValue -= (float)((_maximum - _minimum) / _scaleDivisions);

                            if (i < _scaleDivisions && _showSmallScale)
                            {
                                for (int j = 0; j <= _scaleSubDivisions; j++)
                                {
                                    idx++;
                                    interval = idx * W / (nbticks - 1);
                                    float ssy = start + barRect.Y + interval;
                                    if (_tickStyle == TickStyle.TopLeft || _tickStyle == TickStyle.Both)
                                        rt.DrawLine(new Vector2(cx2 - scaleS - offset, ssy), new Vector2(cx2 - offset, ssy), tickBrush, 1f);
                                    if (_tickStyle == TickStyle.BottomRight || _tickStyle == TickStyle.Both)
                                        rt.DrawLine(new Vector2(cx2 + offset, ssy), new Vector2(cx2 + scaleS + offset, ssy), tickBrush, 1f);
                                }
                            }
                        }

                        layout?.Dispose();

                        if (i < _scaleDivisions || _scaleSubDivisions == 0)
                            interval = (i + 1) * W / (int)_scaleDivisions;
                    }
                }
                #endregion
            }
            catch (Exception Err)
            {
                System.Diagnostics.Debug.WriteLine("DrawColorSlider Error in " + Name + ":" + Err.Message);
            }
        }

        #endregion

        #region Overided events

        private bool mouseInRegion = false;
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInRegion = true;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInRegion = false;
            mouseInThumbRegion = false;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Capture = true;
                if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)_trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
                OnMouseMove(e);
            }
        }

        private bool mouseInThumbRegion = false;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
            
            if (Capture & e.Button == MouseButtons.Left)
            {
                ScrollEventType set = ScrollEventType.ThumbPosition;
                Point pt = e.Location;
                int p = _barOrientation == Orientation.Horizontal ? pt.X : pt.Y;                

                int margin = _thumbSize.Height >> 1;
                p -= margin;
                              
                if (_barOrientation == Orientation.Horizontal)
                {
                    if (_thumbImage != null) 
                    {                        
                        _trackerValue = _minimum  + (p - OffsetL) * (_maximum - _minimum) / (ClientRectangle.Width - OffsetL -OffsetR - _thumbImage.Width);
                    }
                    else
                    {                     
                        _trackerValue = _minimum + (p - OffsetL) * (_maximum - _minimum) / (ClientRectangle.Width - OffsetL - OffsetR - _thumbSize.Width);
                    }
                }
                else
                {
                    if (_thumbImage != null)
                    {                        
                        _trackerValue = _maximum - (p - OffsetR) * (_maximum - _minimum) / (ClientRectangle.Height - OffsetL - OffsetR - _thumbImage.Height);
                    }
                    else
                    {                     
                        _trackerValue = _maximum - (p - OffsetR) * (_maximum - _minimum) / (ClientRectangle.Height - OffsetL - OffsetR - _thumbSize.Height);
                    }
                }

                // Number of divisions
                int nbdiv = (int)(Math.Round(_trackerValue / _smallChange));
                _trackerValue = nbdiv * _smallChange;

                if (_trackerValue <= _minimum)
                {
                    _trackerValue = _minimum;
                    set = ScrollEventType.First;
                }
                else if (_trackerValue >= _maximum)
                {
                    _trackerValue = _maximum;
                    set = ScrollEventType.Last;
                }

                if (Scroll != null) Scroll(this, new ScrollEventArgs(set, (int)_trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
            if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.EndScroll, (int)_trackerValue));
            if (ValueChanged != null) ValueChanged(this, new EventArgs());
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (mouseInRegion)
            {
                decimal v = e.Delta / 120 * (_maximum - _minimum) / _mouseWheelBarPartitions;
                SetProperValue(Value + v);

                // Avoid to send MouseWheel event to the parent container
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Left:
                    SetProperValue(Value - (int)_smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, (int)Value));
                    break;
                case Keys.Up:
                case Keys.Right:
                    SetProperValue(Value + (int)_smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, (int)Value));
                    break;
                case Keys.Home:
                    Value = _minimum;
                    break;
                case Keys.End:
                    Value = _maximum;
                    break;
                case Keys.PageDown:
                    SetProperValue(Value - (int)_largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeDecrement, (int)Value));
                    break;
                case Keys.PageUp:
                    SetProperValue(Value + (int)_largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, (int)Value));
                    break;
            }
            if (Scroll != null && Value == _minimum) Scroll(this, new ScrollEventArgs(ScrollEventType.First, (int)Value));
            if (Scroll != null && Value == _maximum) Scroll(this, new ScrollEventArgs(ScrollEventType.Last, (int)Value));
            Point pt = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
        }

        /// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab | ModifierKeys == Keys.Shift)
                return base.ProcessDialogKey(keyData);
            else
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
        }

        #endregion

        #region Help routines

        private ID2D1Bitmap CreateD2DBitmapFromGdi(Bitmap source)
        {
            if (_renderTarget == null || source == null) return null;
            using var conv = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(conv))
                g.DrawImage(source, new Rectangle(0, 0, conv.Width, conv.Height));
            var bmpData = conv.LockBits(new Rectangle(0, 0, conv.Width, conv.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            try
            {
                return _renderTarget.CreateBitmap(
                    new Vortice.Mathematics.SizeI(conv.Width, conv.Height),
                    bmpData.Scan0, (uint)Math.Abs(bmpData.Stride),
                    new BitmapProperties(new Vortice.DCommon.PixelFormat(
                        Vortice.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)));
            }
            finally { conv.UnlockBits(bmpData); }
        }

        /// <summary>
        /// Creates the round rect path.
        /// </summary>
        /// <param name="rect">The rectangle on which graphics path will be spanned.</param>
        /// <param name="size">The size of rounded rectangle edges.</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundRectPath(Rectangle rect, Size size)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(rect.Left + size.Width / 2, rect.Top, rect.Right - size.Width / 2, rect.Top);
            gp.AddArc(rect.Right - size.Width, rect.Top, size.Width, size.Height, 270, 90);

            gp.AddLine(rect.Right, rect.Top + size.Height / 2, rect.Right, rect.Bottom - size.Width / 2);
            gp.AddArc(rect.Right - size.Width, rect.Bottom - size.Height, size.Width, size.Height, 0, 90);

            gp.AddLine(rect.Right - size.Width / 2, rect.Bottom, rect.Left + size.Width / 2, rect.Bottom);
            gp.AddArc(rect.Left, rect.Bottom - size.Height, size.Width, size.Height, 90, 90);

            gp.AddLine(rect.Left, rect.Bottom - size.Height / 2, rect.Left, rect.Top + size.Height / 2);
            gp.AddArc(rect.Left, rect.Top, size.Width, size.Height, 180, 90);
            return gp;
        }

        /// <summary>
        /// Desaturates colors from given array.
        /// </summary>
        /// <param name="colorsToDesaturate">The colors to be desaturated.</param>
        /// <returns></returns>
        public static Color[] DesaturateColors(params Color[] colorsToDesaturate)
        {
            Color[] colorsToReturn = new Color[colorsToDesaturate.Length];
            for (int i = 0; i < colorsToDesaturate.Length; i++)
            {
                //use NTSC weighted avarage
                int gray =
                    (int)(colorsToDesaturate[i].R * 0.3 + colorsToDesaturate[i].G * 0.6 + colorsToDesaturate[i].B * 0.1);
                colorsToReturn[i] = Color.FromArgb(-0x010101 * (255 - gray) - 1);
            }
            return colorsToReturn;
        }

        /// <summary>
        /// Lightens colors from given array.
        /// </summary>
        /// <param name="colorsToLighten">The colors to lighten.</param>
        /// <returns></returns>
        public static Color[] LightenColors(params Color[] colorsToLighten)
        {
            Color[] colorsToReturn = new Color[colorsToLighten.Length];
            for (int i = 0; i < colorsToLighten.Length; i++)
            {
                colorsToReturn[i] = ControlPaint.Light(colorsToLighten[i]);
            }
            return colorsToReturn;
        }

        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(decimal val)
        {
            if (val < _minimum) Value = _minimum;
            else if (val > _maximum) Value = _maximum;
            else Value = val;
        }

        /// <summary>
        /// Determines whether rectangle contains given point.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <param name="rect">The base rectangle.</param>
        /// <returns>
        /// 	<c>true</c> if rectangle contains given point; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPointInRect(Point pt, Rectangle rect)
        {
            if (pt.X > rect.Left & pt.X < rect.Right & pt.Y > rect.Top & pt.Y < rect.Bottom)
                return true;
            else return false;
        }

        #endregion
    }
}
