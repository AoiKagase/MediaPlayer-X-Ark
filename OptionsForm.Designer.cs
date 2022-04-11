namespace MediaPlayer_X_Ark
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DistortionLevelValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DistortionLevel = new UI.Knob();
            this.CheckDistortion = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(367, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 537);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DistortionLevelValue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DistortionLevel);
            this.groupBox1.Controls.Add(this.CheckDistortion);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 94);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // DistortionLevelValue
            // 
            this.DistortionLevelValue.AutoSize = true;
            this.DistortionLevelValue.Location = new System.Drawing.Point(29, 76);
            this.DistortionLevelValue.Name = "DistortionLevelValue";
            this.DistortionLevelValue.Size = new System.Drawing.Size(34, 15);
            this.DistortionLevelValue.TabIndex = 2;
            this.DistortionLevelValue.Text = "Level";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Level";
            // 
            // DistortionLevel
            // 
            this.DistortionLevel.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.DistortionLevel.HasTicks = true;
            this.DistortionLevel.KnobColor = System.Drawing.SystemColors.Control;
            this.DistortionLevel.LargeChange = 1;
            this.DistortionLevel.Location = new System.Drawing.Point(29, 37);
            this.DistortionLevel.Maximum = 10;
            this.DistortionLevel.Name = "DistortionLevel";
            this.DistortionLevel.PointerColor = System.Drawing.SystemColors.ControlText;
            this.DistortionLevel.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.DistortionLevel.PointerOffset = 4;
            this.DistortionLevel.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.DistortionLevel.PointerWidth = 2;
            this.DistortionLevel.Size = new System.Drawing.Size(36, 36);
            this.DistortionLevel.TabIndex = 1;
            this.DistortionLevel.Text = "Level";
            this.DistortionLevel.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.DistortionLevel.ValueChanged += new System.EventHandler(this.DistortionLevel_ValueChanged);
            // 
            // CheckDistortion
            // 
            this.CheckDistortion.AutoSize = true;
            this.CheckDistortion.Location = new System.Drawing.Point(6, 0);
            this.CheckDistortion.Name = "CheckDistortion";
            this.CheckDistortion.Size = new System.Drawing.Size(78, 19);
            this.CheckDistortion.TabIndex = 0;
            this.CheckDistortion.Text = "Distortion";
            this.CheckDistortion.UseVisualStyleBackColor = true;
            this.CheckDistortion.CheckedChanged += new System.EventHandler(this.CheckDistortion_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel1);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private UI.Knob DistortionLevel;
        private System.Windows.Forms.CheckBox CheckDistortion;
        private System.Windows.Forms.Label DistortionLevelValue;
        private System.Windows.Forms.Label label1;
    }
}