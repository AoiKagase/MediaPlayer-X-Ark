namespace MediaPlayer_X_Ark
{
    partial class ScrollLabel
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
            this.components = new System.ComponentModel.Container();
            this.ScrollTime = new System.Windows.Forms.Timer(this.components);
            this.Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ScrollTime
            // 
            this.ScrollTime.Enabled = true;
            this.ScrollTime.Tick += new System.EventHandler(this.ScrollTime_Tick);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Location = new System.Drawing.Point(42, 32);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(38, 15);
            this.Label.TabIndex = 0;
            this.Label.Text = "label1";
            // 
            // ScrollLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label);
            this.DoubleBuffered = true;
            this.Name = "ScrollLabel";
            this.Load += new System.EventHandler(this.ScrollLabel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer ScrollTime;
        private System.Windows.Forms.Label Label;
    }
}
