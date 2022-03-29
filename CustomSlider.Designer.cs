namespace MediaPlayer_X_Ark
{
    partial class CustomSlider
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.Slider = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Slider)).BeginInit();
            this.SuspendLayout();
            // 
            // Slider
            // 
            this.Slider.Location = new System.Drawing.Point(0, 0);
            this.Slider.Name = "Slider";
            this.Slider.Size = new System.Drawing.Size(100, 50);
            this.Slider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Slider.TabIndex = 1;
            this.Slider.TabStop = false;
            this.Slider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseDown);
            this.Slider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseMove);
            this.Slider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseUp);
            // 
            // CustomSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.Slider);
            this.DoubleBuffered = true;
            this.Name = "CustomSlider";
            this.Size = new System.Drawing.Size(154, 76);
            this.Load += new System.EventHandler(this.CustomSlider_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Slider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Slider;
    }
}
