using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark
{
    public partial class CustomSlider : UserControl
    {
        public event EventHandler<EventArgs> ValueChanged;

        /// <summary>
        /// スライダー値が変更された場合に発生します
        /// </summary>
        /// <param name="e"></param>
        [Browsable(true)]
        [Description("スライダー値が変更されるときに発生するイベントです")]
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            EventHandler<EventArgs> eventHandler = ValueChanged;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        public Image SliderImage
        {
            get
            {
                return this.Slider.Image;
            }
            set
            {
                this.Slider.Image = value;
                if (value != null)
                {
                    this.Slider.Width = this.Slider.Image.Width;
                    this.Slider.Height = this.Slider.Image.Height;
                }
            }
        }

        public int Maximum { get; set; }
        public int Minimum { get; set; }
        public Orientation Orientation { get; set; }

        private int _value;

        public int Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                ValueChangeSliderPosition();

                OnCheckedChanged(EventArgs.Empty);
            }
        }
        public CustomSlider()
        {
            InitializeComponent();
        }

        private void ValueChangeSliderPosition()
        {
            if (this.Orientation == Orientation.Horizontal)
            {
                this.Slider.Top = 0;
                this.Slider.Left = (int)((float)(this.Width - this.Slider.Width) / (Maximum - Minimum) * (this._value - Minimum));
            }
            else
            {
                this.Slider.Left = 0;
                this.Slider.Top = this.Height - (int)((float)(this.Height - this.Slider.Height) / (Maximum - Minimum) * (this._value - Minimum));
            }
        }

        private void CustomSlider_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        private Point mousePoint;
        private void Bar_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Bar_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        private void Bar_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                int position;
                if (Orientation == Orientation.Horizontal)
                {
                    position = this.Slider.Left + e.X - mousePoint.X;
                    if (position < 0)
                        this.Slider.Left = 0;
                    else if (position > (this.Width - this.Slider.Width))
                        this.Slider.Left = (this.Width - this.Slider.Width);
                    else
                        this.Slider.Left = position;
                    this.Slider.Top = 0;
                }
                else
                {
                    position = this.Slider.Top + e.Y - mousePoint.Y;
                    if (position < 0)
                        this.Slider.Top = 0;
                    else if (position > (this.Height - this.Slider.Height))
                        this.Slider.Top = (this.Height - this.Slider.Height);
                    else
                        this.Slider.Top = position;
                    this.Slider.Left = 0;
                }
            }
        }
    }
}
