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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("出力設定");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Graphic Equalizer");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Pitch / Freq / Speed");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Distortion");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Chorus");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Echo");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Flanger");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Highpass");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Lowpass");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Compressor");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Reverb");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("エフェクト", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("スキン");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("その他");
            this.TreeMenu = new System.Windows.Forms.TreeView();
            this.tabControlEffects = new System.Windows.Forms.TabControl();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.cmbSpeaker = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.cmbSampling = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.cmbSampleRate = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.cmbOutput = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.tabGEqualizer = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label56 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.cmbEqPreset = new System.Windows.Forms.ComboBox();
            this.CheckGEQ = new System.Windows.Forms.CheckBox();
            this.PictGEQGraph = new System.Windows.Forms.PictureBox();
            this.TrkGEQ60 = new ColorSlider.ColorSlider();
            this.TrkGEQ32 = new ColorSlider.ColorSlider();
            this.TrkGEQ125 = new ColorSlider.ColorSlider();
            this.TrkGEQ250 = new ColorSlider.ColorSlider();
            this.TrkGEQ500 = new ColorSlider.ColorSlider();
            this.TrkGEQ1K = new ColorSlider.ColorSlider();
            this.TrkGEQ2K = new ColorSlider.ColorSlider();
            this.TrkGEQ4K = new ColorSlider.ColorSlider();
            this.TrkGEQ8K = new ColorSlider.ColorSlider();
            this.TrkGEQ16K = new ColorSlider.ColorSlider();
            this.TrkGEQ20K = new ColorSlider.ColorSlider();
            this.TrkGEQ22K = new ColorSlider.ColorSlider();
            this.tabPitch = new System.Windows.Forms.TabPage();
            this.GroupSpeed = new System.Windows.Forms.GroupBox();
            this.lblValSpeed = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.KnobSpeed = new UI.Knob();
            this.CheckSpeed = new System.Windows.Forms.CheckBox();
            this.GroupFrequency = new System.Windows.Forms.GroupBox();
            this.lblValFrequency = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.KnobFrequency = new UI.Knob();
            this.CheckFrequency = new System.Windows.Forms.CheckBox();
            this.GroupPitchShift = new System.Windows.Forms.GroupBox();
            this.lblValPitchFFT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KnobPitchFFT = new UI.Knob();
            this.lblValPitchPitch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.KnobPitchPitch = new UI.Knob();
            this.CheckPitch = new System.Windows.Forms.CheckBox();
            this.tabDistortion = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CheckDistortion = new System.Windows.Forms.CheckBox();
            this.lblValDistortionLevel = new System.Windows.Forms.TextBox();
            this.KnobDistortionLevel = new UI.Knob();
            this.label1 = new System.Windows.Forms.Label();
            this.tabChorus = new System.Windows.Forms.TabPage();
            this.GroupChorus = new System.Windows.Forms.GroupBox();
            this.lblValChorusDepth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.KnobChorusDepth = new UI.Knob();
            this.lblValChorusRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.KnobChorusRate = new UI.Knob();
            this.lblValChorusMix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.KnobChorusMix = new UI.Knob();
            this.CheckChorus = new System.Windows.Forms.CheckBox();
            this.tabEcho = new System.Windows.Forms.TabPage();
            this.GroupEcho = new System.Windows.Forms.GroupBox();
            this.lblValEchoWet = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.KnobEchoWet = new UI.Knob();
            this.lblValEchoDry = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.KnobEchoDry = new UI.Knob();
            this.lblValEchoFeedback = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.KnobEchoFeedback = new UI.Knob();
            this.lblValEchoDelay = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.KnobEchoDelay = new UI.Knob();
            this.CheckEcho = new System.Windows.Forms.CheckBox();
            this.tabFlanger = new System.Windows.Forms.TabPage();
            this.GroupFlanger = new System.Windows.Forms.GroupBox();
            this.lblValFlangerDepth = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.KnobFlangerDepth = new UI.Knob();
            this.lblValFlangerRate = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.KnobFlangerRate = new UI.Knob();
            this.lblValFlangerMix = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.KnobFlangerMix = new UI.Knob();
            this.CheckFlanger = new System.Windows.Forms.CheckBox();
            this.tabHightpass = new System.Windows.Forms.TabPage();
            this.GroupHighpass = new System.Windows.Forms.GroupBox();
            this.lblValHighpassResonance = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.KnobHighpassResonance = new UI.Knob();
            this.lblValHighpassCutoff = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.KnobHighpassCutoff = new UI.Knob();
            this.CheckHighpass = new System.Windows.Forms.CheckBox();
            this.tabLowpass = new System.Windows.Forms.TabPage();
            this.GroupLowpass = new System.Windows.Forms.GroupBox();
            this.lblValLowpassResonance = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.KnobLowpassResonance = new UI.Knob();
            this.lblValLowpassCutoff = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.KnobLowpassCutoff = new UI.Knob();
            this.CheckLowpass = new System.Windows.Forms.CheckBox();
            this.tabCompressor = new System.Windows.Forms.TabPage();
            this.GroupCompressor = new System.Windows.Forms.GroupBox();
            this.CheckCompLinked = new System.Windows.Forms.CheckBox();
            this.lblValCompGain = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.KnobCompGain = new UI.Knob();
            this.lblValCompRelease = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.KnobCompRelease = new UI.Knob();
            this.lblValCompAttack = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.KnobCompAttack = new UI.Knob();
            this.lblValCompRatio = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.KnobCompRatio = new UI.Knob();
            this.lblValCompThreshold = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.KnobCompThreshold = new UI.Knob();
            this.CheckCompressor = new System.Windows.Forms.CheckBox();
            this.tabReverb = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.knob13 = new UI.Knob();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.knob12 = new UI.Knob();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.knob11 = new UI.Knob();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.knob10 = new UI.Knob();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.knob9 = new UI.Knob();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.knob8 = new UI.Knob();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.knob7 = new UI.Knob();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.knob6 = new UI.Knob();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.knob1 = new UI.Knob();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.knob2 = new UI.Knob();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.knob3 = new UI.Knob();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.knob4 = new UI.Knob();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.knob5 = new UI.Knob();
            this.CheckReverb = new System.Windows.Forms.CheckBox();
            this.tabControlEffects.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.tabGEqualizer.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictGEQGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ125)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ250)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ500)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ1K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ2K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ4K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ8K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ16K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ20K)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ22K)).BeginInit();
            this.tabPitch.SuspendLayout();
            this.GroupSpeed.SuspendLayout();
            this.GroupFrequency.SuspendLayout();
            this.GroupPitchShift.SuspendLayout();
            this.tabDistortion.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabChorus.SuspendLayout();
            this.GroupChorus.SuspendLayout();
            this.tabEcho.SuspendLayout();
            this.GroupEcho.SuspendLayout();
            this.tabFlanger.SuspendLayout();
            this.GroupFlanger.SuspendLayout();
            this.tabHightpass.SuspendLayout();
            this.GroupHighpass.SuspendLayout();
            this.tabLowpass.SuspendLayout();
            this.GroupLowpass.SuspendLayout();
            this.tabCompressor.SuspendLayout();
            this.GroupCompressor.SuspendLayout();
            this.tabReverb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeMenu
            // 
            this.TreeMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.TreeMenu.Location = new System.Drawing.Point(0, 0);
            this.TreeMenu.Name = "TreeMenu";
            treeNode1.Name = "OUTPUT";
            treeNode1.Text = "出力設定";
            treeNode2.Name = "GEQ";
            treeNode2.Text = "Graphic Equalizer";
            treeNode3.Name = "PITCH";
            treeNode3.Text = "Pitch / Freq / Speed";
            treeNode4.Name = "DISTORTION";
            treeNode4.Text = "Distortion";
            treeNode5.Name = "CHORUS";
            treeNode5.Text = "Chorus";
            treeNode6.Name = "ECHO";
            treeNode6.Text = "Echo";
            treeNode7.Name = "FLANGER";
            treeNode7.Text = "Flanger";
            treeNode8.Name = "HIGHPASS";
            treeNode8.Text = "Highpass";
            treeNode9.Name = "LOWPASS";
            treeNode9.Text = "Lowpass";
            treeNode10.Name = "COMPRESSOR";
            treeNode10.Text = "Compressor";
            treeNode11.Name = "REVERB";
            treeNode11.Text = "Reverb";
            treeNode12.Name = "EFFECTS";
            treeNode12.Text = "エフェクト";
            treeNode13.Name = "SKIN";
            treeNode13.Text = "スキン";
            treeNode14.Name = "OTHER";
            treeNode14.Text = "その他";
            this.TreeMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode12,
            treeNode13,
            treeNode14});
            this.TreeMenu.Size = new System.Drawing.Size(199, 419);
            this.TreeMenu.TabIndex = 3;
            this.TreeMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeMenu_AfterSelect);
            // 
            // tabControlEffects
            // 
            this.tabControlEffects.Controls.Add(this.tabSetting);
            this.tabControlEffects.Controls.Add(this.tabGEqualizer);
            this.tabControlEffects.Controls.Add(this.tabPitch);
            this.tabControlEffects.Controls.Add(this.tabDistortion);
            this.tabControlEffects.Controls.Add(this.tabChorus);
            this.tabControlEffects.Controls.Add(this.tabEcho);
            this.tabControlEffects.Controls.Add(this.tabFlanger);
            this.tabControlEffects.Controls.Add(this.tabHightpass);
            this.tabControlEffects.Controls.Add(this.tabLowpass);
            this.tabControlEffects.Controls.Add(this.tabCompressor);
            this.tabControlEffects.Controls.Add(this.tabReverb);
            this.tabControlEffects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEffects.Location = new System.Drawing.Point(199, 0);
            this.tabControlEffects.Name = "tabControlEffects";
            this.tabControlEffects.SelectedIndex = 0;
            this.tabControlEffects.Size = new System.Drawing.Size(582, 419);
            this.tabControlEffects.TabIndex = 4;
            // 
            // tabSetting
            // 
            this.tabSetting.BackColor = System.Drawing.Color.Transparent;
            this.tabSetting.Controls.Add(this.BtnUpdate);
            this.tabSetting.Controls.Add(this.cmbSpeaker);
            this.tabSetting.Controls.Add(this.label43);
            this.tabSetting.Controls.Add(this.cmbSampling);
            this.tabSetting.Controls.Add(this.label42);
            this.tabSetting.Controls.Add(this.cmbFormat);
            this.tabSetting.Controls.Add(this.label41);
            this.tabSetting.Controls.Add(this.cmbSampleRate);
            this.tabSetting.Controls.Add(this.label40);
            this.tabSetting.Controls.Add(this.cmbDevice);
            this.tabSetting.Controls.Add(this.label38);
            this.tabSetting.Controls.Add(this.cmbOutput);
            this.tabSetting.Controls.Add(this.label36);
            this.tabSetting.Location = new System.Drawing.Point(4, 24);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Size = new System.Drawing.Size(574, 391);
            this.tabSetting.TabIndex = 10;
            this.tabSetting.Text = "出力設定";
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(272, 204);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(75, 23);
            this.BtnUpdate.TabIndex = 25;
            this.BtnUpdate.Text = "適用";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // cmbSpeaker
            // 
            this.cmbSpeaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpeaker.FormattingEnabled = true;
            this.cmbSpeaker.Items.AddRange(new object[] {
            "デフォルト",
            "モノラル",
            "ステレオ",
            "4.0",
            "5.0",
            "5.1",
            "7.1",
            "7.1.4"});
            this.cmbSpeaker.Location = new System.Drawing.Point(105, 159);
            this.cmbSpeaker.Name = "cmbSpeaker";
            this.cmbSpeaker.Size = new System.Drawing.Size(242, 23);
            this.cmbSpeaker.TabIndex = 24;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(24, 162);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(75, 15);
            this.label43.TabIndex = 23;
            this.label43.Text = "スピーカーモード";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSampling
            // 
            this.cmbSampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSampling.FormattingEnabled = true;
            this.cmbSampling.Items.AddRange(new object[] {
            "補完無し",
            "リニア補完",
            "キュービック補完",
            "５ポイントスプライン補完"});
            this.cmbSampling.Location = new System.Drawing.Point(105, 130);
            this.cmbSampling.Name = "cmbSampling";
            this.cmbSampling.Size = new System.Drawing.Size(242, 23);
            this.cmbSampling.TabIndex = 22;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(13, 133);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(86, 15);
            this.label42.TabIndex = 21;
            this.label42.Text = "サンプリングモード";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFormat
            // 
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            "8bit integer PCM",
            "16bit integer PCM",
            "24bit integer PCM",
            "32bit integer PCM",
            "32bit floating point PCM"});
            this.cmbFormat.Location = new System.Drawing.Point(105, 101);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(242, 23);
            this.cmbFormat.TabIndex = 20;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(43, 104);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(56, 15);
            this.label41.TabIndex = 19;
            this.label41.Text = "フォーマット";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSampleRate
            // 
            this.cmbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSampleRate.FormattingEnabled = true;
            this.cmbSampleRate.Items.AddRange(new object[] {
            "192000",
            "96000",
            "88200",
            "48000",
            "44100",
            "32000",
            "22050",
            "16000",
            "11025",
            "8000"});
            this.cmbSampleRate.Location = new System.Drawing.Point(105, 72);
            this.cmbSampleRate.Name = "cmbSampleRate";
            this.cmbSampleRate.Size = new System.Drawing.Size(242, 23);
            this.cmbSampleRate.TabIndex = 18;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(29, 75);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(70, 15);
            this.label40.TabIndex = 17;
            this.label40.Text = "サンプルレート";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(105, 43);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(242, 23);
            this.cmbDevice.TabIndex = 16;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(55, 46);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(44, 15);
            this.label38.TabIndex = 15;
            this.label38.Text = "デバイス";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbOutput
            // 
            this.cmbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutput.FormattingEnabled = true;
            this.cmbOutput.Items.AddRange(new object[] {
            "自動検出",
            "Windows Audio Session API",
            "Low latency ASIO 2.0",
            "Windows Sonic"});
            this.cmbOutput.Location = new System.Drawing.Point(105, 14);
            this.cmbOutput.Name = "cmbOutput";
            this.cmbOutput.Size = new System.Drawing.Size(242, 23);
            this.cmbOutput.TabIndex = 14;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(44, 17);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(55, 15);
            this.label36.TabIndex = 13;
            this.label36.Text = "出力方式";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabGEqualizer
            // 
            this.tabGEqualizer.BackColor = System.Drawing.Color.Transparent;
            this.tabGEqualizer.Controls.Add(this.groupBox3);
            this.tabGEqualizer.Location = new System.Drawing.Point(4, 24);
            this.tabGEqualizer.Name = "tabGEqualizer";
            this.tabGEqualizer.Size = new System.Drawing.Size(574, 391);
            this.tabGEqualizer.TabIndex = 9;
            this.tabGEqualizer.Text = "Graphic Equalizer";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label56);
            this.groupBox3.Controls.Add(this.label55);
            this.groupBox3.Controls.Add(this.label54);
            this.groupBox3.Controls.Add(this.label53);
            this.groupBox3.Controls.Add(this.label52);
            this.groupBox3.Controls.Add(this.label51);
            this.groupBox3.Controls.Add(this.label50);
            this.groupBox3.Controls.Add(this.label49);
            this.groupBox3.Controls.Add(this.label48);
            this.groupBox3.Controls.Add(this.label47);
            this.groupBox3.Controls.Add(this.label46);
            this.groupBox3.Controls.Add(this.label45);
            this.groupBox3.Controls.Add(this.label44);
            this.groupBox3.Controls.Add(this.cmbEqPreset);
            this.groupBox3.Controls.Add(this.CheckGEQ);
            this.groupBox3.Controls.Add(this.PictGEQGraph);
            this.groupBox3.Controls.Add(this.TrkGEQ60);
            this.groupBox3.Controls.Add(this.TrkGEQ32);
            this.groupBox3.Controls.Add(this.TrkGEQ125);
            this.groupBox3.Controls.Add(this.TrkGEQ250);
            this.groupBox3.Controls.Add(this.TrkGEQ500);
            this.groupBox3.Controls.Add(this.TrkGEQ1K);
            this.groupBox3.Controls.Add(this.TrkGEQ2K);
            this.groupBox3.Controls.Add(this.TrkGEQ4K);
            this.groupBox3.Controls.Add(this.TrkGEQ8K);
            this.groupBox3.Controls.Add(this.TrkGEQ16K);
            this.groupBox3.Controls.Add(this.TrkGEQ20K);
            this.groupBox3.Controls.Add(this.TrkGEQ22K);
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(563, 377);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(518, 348);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(26, 15);
            this.label56.TabIndex = 28;
            this.label56.Text = "22K";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(469, 348);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(26, 15);
            this.label55.TabIndex = 28;
            this.label55.Text = "20K";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(424, 348);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(26, 15);
            this.label54.TabIndex = 28;
            this.label54.Text = "16K";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(385, 348);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(20, 15);
            this.label53.TabIndex = 28;
            this.label53.Text = "8K";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(343, 348);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(20, 15);
            this.label52.TabIndex = 28;
            this.label52.Text = "4K";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(295, 348);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(20, 15);
            this.label51.TabIndex = 28;
            this.label51.Text = "2K";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(250, 348);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(20, 15);
            this.label50.TabIndex = 28;
            this.label50.Text = "1K";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(200, 348);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(25, 15);
            this.label49.TabIndex = 28;
            this.label49.Text = "500";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(155, 348);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(25, 15);
            this.label48.TabIndex = 28;
            this.label48.Text = "250";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(110, 348);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(25, 15);
            this.label47.TabIndex = 28;
            this.label47.Text = "125";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(73, 348);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(19, 15);
            this.label46.TabIndex = 28;
            this.label46.Text = "60";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(26, 348);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(19, 15);
            this.label45.TabIndex = 28;
            this.label45.Text = "32";
            // 
            // label44
            // 
            this.label44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label44.Location = new System.Drawing.Point(23, 274);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(521, 1);
            this.label44.TabIndex = 15;
            // 
            // cmbEqPreset
            // 
            this.cmbEqPreset.FormattingEnabled = true;
            this.cmbEqPreset.Items.AddRange(new object[] {
            "Normal",
            "Rock",
            "Pop",
            "Bass Boost",
            "Trable Boost",
            "Total Boost",
            "Total Reduce",
            "Custom"});
            this.cmbEqPreset.Location = new System.Drawing.Point(6, 25);
            this.cmbEqPreset.Name = "cmbEqPreset";
            this.cmbEqPreset.Size = new System.Drawing.Size(121, 23);
            this.cmbEqPreset.TabIndex = 14;
            this.cmbEqPreset.SelectedIndexChanged += new System.EventHandler(this.cmbEqPreset_SelectedIndexChanged);
            // 
            // CheckGEQ
            // 
            this.CheckGEQ.AutoSize = true;
            this.CheckGEQ.BackColor = System.Drawing.SystemColors.Control;
            this.CheckGEQ.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckGEQ.Location = new System.Drawing.Point(6, 0);
            this.CheckGEQ.Name = "CheckGEQ";
            this.CheckGEQ.Size = new System.Drawing.Size(117, 19);
            this.CheckGEQ.TabIndex = 13;
            this.CheckGEQ.Text = "Graphic Equalizer";
            this.CheckGEQ.UseVisualStyleBackColor = false;
            this.CheckGEQ.CheckedChanged += new System.EventHandler(this.CheckGEQ_CheckedChanged);
            // 
            // PictGEQGraph
            // 
            this.PictGEQGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictGEQGraph.Location = new System.Drawing.Point(6, 54);
            this.PictGEQGraph.Name = "PictGEQGraph";
            this.PictGEQGraph.Size = new System.Drawing.Size(551, 137);
            this.PictGEQGraph.TabIndex = 0;
            this.PictGEQGraph.TabStop = false;
            // 
            // TrkGEQ60
            // 
            this.TrkGEQ60.AutoSize = false;
            this.TrkGEQ60.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ60.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ60.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ60.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ60.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ60.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ60.Location = new System.Drawing.Point(51, 197);
            this.TrkGEQ60.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ60.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ60.Name = "TrkGEQ60";
            this.TrkGEQ60.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ60.Padding = 10;
            this.TrkGEQ60.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ60.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ60.ShowDivisionsText = false;
            this.TrkGEQ60.ShowSmallScale = false;
            this.TrkGEQ60.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ60.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ60.TabIndex = 17;
            this.TrkGEQ60.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ60.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ60.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ60.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ60.TickAdd = 0F;
            this.TrkGEQ60.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ60.TickDivide = 10F;
            this.TrkGEQ60.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ60.ValueChanged += new System.EventHandler(this.TrkGEQ60_ValueChanged);
            // 
            // TrkGEQ32
            // 
            this.TrkGEQ32.AutoSize = false;
            this.TrkGEQ32.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ32.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ32.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ32.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ32.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ32.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ32.Location = new System.Drawing.Point(6, 197);
            this.TrkGEQ32.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ32.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ32.Name = "TrkGEQ32";
            this.TrkGEQ32.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ32.Padding = 10;
            this.TrkGEQ32.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ32.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ32.ShowDivisionsText = true;
            this.TrkGEQ32.ShowSmallScale = false;
            this.TrkGEQ32.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ32.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ32.TabIndex = 16;
            this.TrkGEQ32.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ32.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ32.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ32.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ32.TickAdd = 0F;
            this.TrkGEQ32.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ32.TickDivide = 10F;
            this.TrkGEQ32.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ32.ValueChanged += new System.EventHandler(this.TrkGEQ32_ValueChanged);
            // 
            // TrkGEQ125
            // 
            this.TrkGEQ125.AutoSize = false;
            this.TrkGEQ125.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ125.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ125.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ125.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ125.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ125.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ125.Location = new System.Drawing.Point(96, 197);
            this.TrkGEQ125.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ125.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ125.Name = "TrkGEQ125";
            this.TrkGEQ125.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ125.Padding = 10;
            this.TrkGEQ125.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ125.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ125.ShowDivisionsText = false;
            this.TrkGEQ125.ShowSmallScale = false;
            this.TrkGEQ125.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ125.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ125.TabIndex = 18;
            this.TrkGEQ125.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ125.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ125.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ125.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ125.TickAdd = 0F;
            this.TrkGEQ125.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ125.TickDivide = 10F;
            this.TrkGEQ125.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ125.ValueChanged += new System.EventHandler(this.TrkGEQ125_ValueChanged);
            // 
            // TrkGEQ250
            // 
            this.TrkGEQ250.AutoSize = false;
            this.TrkGEQ250.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ250.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ250.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ250.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ250.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ250.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ250.Location = new System.Drawing.Point(141, 197);
            this.TrkGEQ250.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ250.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ250.Name = "TrkGEQ250";
            this.TrkGEQ250.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ250.Padding = 10;
            this.TrkGEQ250.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ250.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ250.ShowDivisionsText = false;
            this.TrkGEQ250.ShowSmallScale = false;
            this.TrkGEQ250.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ250.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ250.TabIndex = 19;
            this.TrkGEQ250.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ250.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ250.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ250.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ250.TickAdd = 0F;
            this.TrkGEQ250.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ250.TickDivide = 10F;
            this.TrkGEQ250.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ250.ValueChanged += new System.EventHandler(this.TrkGEQ250_ValueChanged);
            // 
            // TrkGEQ500
            // 
            this.TrkGEQ500.AutoSize = false;
            this.TrkGEQ500.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ500.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ500.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ500.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ500.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ500.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ500.Location = new System.Drawing.Point(186, 197);
            this.TrkGEQ500.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ500.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ500.Name = "TrkGEQ500";
            this.TrkGEQ500.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ500.Padding = 10;
            this.TrkGEQ500.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ500.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ500.ShowDivisionsText = false;
            this.TrkGEQ500.ShowSmallScale = false;
            this.TrkGEQ500.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ500.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ500.TabIndex = 20;
            this.TrkGEQ500.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ500.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ500.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ500.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ500.TickAdd = 0F;
            this.TrkGEQ500.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ500.TickDivide = 10F;
            this.TrkGEQ500.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ500.ValueChanged += new System.EventHandler(this.TrkGEQ500_ValueChanged);
            // 
            // TrkGEQ1K
            // 
            this.TrkGEQ1K.AutoSize = false;
            this.TrkGEQ1K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ1K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ1K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ1K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ1K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ1K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ1K.Location = new System.Drawing.Point(231, 197);
            this.TrkGEQ1K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ1K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ1K.Name = "TrkGEQ1K";
            this.TrkGEQ1K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ1K.Padding = 10;
            this.TrkGEQ1K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ1K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ1K.ShowDivisionsText = false;
            this.TrkGEQ1K.ShowSmallScale = false;
            this.TrkGEQ1K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ1K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ1K.TabIndex = 21;
            this.TrkGEQ1K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ1K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ1K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ1K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ1K.TickAdd = 0F;
            this.TrkGEQ1K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ1K.TickDivide = 10F;
            this.TrkGEQ1K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ1K.ValueChanged += new System.EventHandler(this.TrkGEQ1K_ValueChanged);
            // 
            // TrkGEQ2K
            // 
            this.TrkGEQ2K.AutoSize = false;
            this.TrkGEQ2K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ2K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ2K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ2K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ2K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ2K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ2K.Location = new System.Drawing.Point(276, 197);
            this.TrkGEQ2K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ2K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ2K.Name = "TrkGEQ2K";
            this.TrkGEQ2K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ2K.Padding = 10;
            this.TrkGEQ2K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ2K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ2K.ShowDivisionsText = false;
            this.TrkGEQ2K.ShowSmallScale = false;
            this.TrkGEQ2K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ2K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ2K.TabIndex = 22;
            this.TrkGEQ2K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ2K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ2K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ2K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ2K.TickAdd = 0F;
            this.TrkGEQ2K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ2K.TickDivide = 10F;
            this.TrkGEQ2K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ2K.ValueChanged += new System.EventHandler(this.TrkGEQ2K_ValueChanged);
            // 
            // TrkGEQ4K
            // 
            this.TrkGEQ4K.AutoSize = false;
            this.TrkGEQ4K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ4K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ4K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ4K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ4K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ4K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ4K.Location = new System.Drawing.Point(321, 197);
            this.TrkGEQ4K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ4K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ4K.Name = "TrkGEQ4K";
            this.TrkGEQ4K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ4K.Padding = 10;
            this.TrkGEQ4K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ4K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ4K.ShowDivisionsText = false;
            this.TrkGEQ4K.ShowSmallScale = false;
            this.TrkGEQ4K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ4K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ4K.TabIndex = 23;
            this.TrkGEQ4K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ4K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ4K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ4K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ4K.TickAdd = 0F;
            this.TrkGEQ4K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ4K.TickDivide = 10F;
            this.TrkGEQ4K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ4K.ValueChanged += new System.EventHandler(this.TrkGEQ4K_ValueChanged);
            // 
            // TrkGEQ8K
            // 
            this.TrkGEQ8K.AutoSize = false;
            this.TrkGEQ8K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ8K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ8K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ8K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ8K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ8K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ8K.Location = new System.Drawing.Point(366, 197);
            this.TrkGEQ8K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ8K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ8K.Name = "TrkGEQ8K";
            this.TrkGEQ8K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ8K.Padding = 10;
            this.TrkGEQ8K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ8K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ8K.ShowDivisionsText = false;
            this.TrkGEQ8K.ShowSmallScale = false;
            this.TrkGEQ8K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ8K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ8K.TabIndex = 24;
            this.TrkGEQ8K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ8K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ8K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ8K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ8K.TickAdd = 0F;
            this.TrkGEQ8K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ8K.TickDivide = 10F;
            this.TrkGEQ8K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ8K.ValueChanged += new System.EventHandler(this.TrkGEQ8K_ValueChanged);
            // 
            // TrkGEQ16K
            // 
            this.TrkGEQ16K.AutoSize = false;
            this.TrkGEQ16K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ16K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ16K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ16K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ16K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ16K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ16K.Location = new System.Drawing.Point(411, 197);
            this.TrkGEQ16K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ16K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ16K.Name = "TrkGEQ16K";
            this.TrkGEQ16K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ16K.Padding = 10;
            this.TrkGEQ16K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ16K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ16K.ShowDivisionsText = false;
            this.TrkGEQ16K.ShowSmallScale = false;
            this.TrkGEQ16K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ16K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ16K.TabIndex = 25;
            this.TrkGEQ16K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ16K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ16K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ16K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ16K.TickAdd = 0F;
            this.TrkGEQ16K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ16K.TickDivide = 10F;
            this.TrkGEQ16K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ16K.ValueChanged += new System.EventHandler(this.TrkGEQ16K_ValueChanged);
            // 
            // TrkGEQ20K
            // 
            this.TrkGEQ20K.AutoSize = false;
            this.TrkGEQ20K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ20K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ20K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ20K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ20K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ20K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ20K.Location = new System.Drawing.Point(456, 197);
            this.TrkGEQ20K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ20K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ20K.Name = "TrkGEQ20K";
            this.TrkGEQ20K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ20K.Padding = 10;
            this.TrkGEQ20K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ20K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ20K.ShowDivisionsText = false;
            this.TrkGEQ20K.ShowSmallScale = false;
            this.TrkGEQ20K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ20K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ20K.TabIndex = 26;
            this.TrkGEQ20K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ20K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ20K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ20K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ20K.TickAdd = 0F;
            this.TrkGEQ20K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ20K.TickDivide = 10F;
            this.TrkGEQ20K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ20K.ValueChanged += new System.EventHandler(this.TrkGEQ20K_ValueChanged);
            // 
            // TrkGEQ22K
            // 
            this.TrkGEQ22K.AutoSize = false;
            this.TrkGEQ22K.BackColor = System.Drawing.Color.Transparent;
            this.TrkGEQ22K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ22K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.TrkGEQ22K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            this.TrkGEQ22K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.Font = new System.Drawing.Font("Yu Gothic UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TrkGEQ22K.LargeChange = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ22K.Location = new System.Drawing.Point(501, 197);
            this.TrkGEQ22K.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TrkGEQ22K.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.TrkGEQ22K.Name = "TrkGEQ22K";
            this.TrkGEQ22K.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrkGEQ22K.Padding = 10;
            this.TrkGEQ22K.ScaleDivisions = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TrkGEQ22K.ScaleSubDivisions = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TrkGEQ22K.ShowDivisionsText = true;
            this.TrkGEQ22K.ShowSmallScale = false;
            this.TrkGEQ22K.Size = new System.Drawing.Size(61, 154);
            this.TrkGEQ22K.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TrkGEQ22K.TabIndex = 27;
            this.TrkGEQ22K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ22K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            this.TrkGEQ22K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.TrkGEQ22K.ThumbSize = new System.Drawing.Size(16, 8);
            this.TrkGEQ22K.TickAdd = 0F;
            this.TrkGEQ22K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrkGEQ22K.TickDivide = 10F;
            this.TrkGEQ22K.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TrkGEQ22K.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TrkGEQ22K.ValueChanged += new System.EventHandler(this.TrkGEQ22K_ValueChanged);
            // 
            // tabPitch
            // 
            this.tabPitch.BackColor = System.Drawing.Color.Transparent;
            this.tabPitch.Controls.Add(this.GroupSpeed);
            this.tabPitch.Controls.Add(this.GroupFrequency);
            this.tabPitch.Controls.Add(this.GroupPitchShift);
            this.tabPitch.Location = new System.Drawing.Point(4, 24);
            this.tabPitch.Name = "tabPitch";
            this.tabPitch.Size = new System.Drawing.Size(574, 391);
            this.tabPitch.TabIndex = 8;
            this.tabPitch.Text = "Pitch/Freq/Speed";
            // 
            // GroupSpeed
            // 
            this.GroupSpeed.Controls.Add(this.lblValSpeed);
            this.GroupSpeed.Controls.Add(this.label6);
            this.GroupSpeed.Controls.Add(this.KnobSpeed);
            this.GroupSpeed.Controls.Add(this.CheckSpeed);
            this.GroupSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupSpeed.Location = new System.Drawing.Point(264, 3);
            this.GroupSpeed.Name = "GroupSpeed";
            this.GroupSpeed.Size = new System.Drawing.Size(100, 150);
            this.GroupSpeed.TabIndex = 16;
            this.GroupSpeed.TabStop = false;
            // 
            // lblValSpeed
            // 
            this.lblValSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValSpeed.Location = new System.Drawing.Point(6, 111);
            this.lblValSpeed.Name = "lblValSpeed";
            this.lblValSpeed.ReadOnly = true;
            this.lblValSpeed.Size = new System.Drawing.Size(64, 16);
            this.lblValSpeed.TabIndex = 2;
            this.lblValSpeed.Text = "0.0";
            this.lblValSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Speed";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobSpeed
            // 
            this.KnobSpeed.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobSpeed.BorderWidth = 2;
            this.KnobSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobSpeed.HasTicks = true;
            this.KnobSpeed.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobSpeed.LargeChange = 10;
            this.KnobSpeed.Location = new System.Drawing.Point(6, 41);
            this.KnobSpeed.Minimum = -100;
            this.KnobSpeed.Name = "KnobSpeed";
            this.KnobSpeed.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobSpeed.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobSpeed.PointerOffset = 4;
            this.KnobSpeed.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobSpeed.PointerWidth = 2;
            this.KnobSpeed.Size = new System.Drawing.Size(64, 64);
            this.KnobSpeed.TabIndex = 1;
            this.KnobSpeed.Text = "Level";
            this.KnobSpeed.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckSpeed
            // 
            this.CheckSpeed.AutoSize = true;
            this.CheckSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.CheckSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckSpeed.Location = new System.Drawing.Point(6, 3);
            this.CheckSpeed.Name = "CheckSpeed";
            this.CheckSpeed.Size = new System.Drawing.Size(58, 19);
            this.CheckSpeed.TabIndex = 0;
            this.CheckSpeed.Text = "Speed";
            this.CheckSpeed.UseVisualStyleBackColor = false;
            this.CheckSpeed.CheckedChanged += new System.EventHandler(this.CheckSpeed_CheckedChanged);
            // 
            // GroupFrequency
            // 
            this.GroupFrequency.Controls.Add(this.lblValFrequency);
            this.GroupFrequency.Controls.Add(this.label8);
            this.GroupFrequency.Controls.Add(this.KnobFrequency);
            this.GroupFrequency.Controls.Add(this.CheckFrequency);
            this.GroupFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupFrequency.Location = new System.Drawing.Point(158, 3);
            this.GroupFrequency.Name = "GroupFrequency";
            this.GroupFrequency.Size = new System.Drawing.Size(100, 150);
            this.GroupFrequency.TabIndex = 15;
            this.GroupFrequency.TabStop = false;
            // 
            // lblValFrequency
            // 
            this.lblValFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValFrequency.Location = new System.Drawing.Point(6, 114);
            this.lblValFrequency.Name = "lblValFrequency";
            this.lblValFrequency.ReadOnly = true;
            this.lblValFrequency.Size = new System.Drawing.Size(64, 16);
            this.lblValFrequency.TabIndex = 2;
            this.lblValFrequency.Text = "0.0";
            this.lblValFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(6, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "Frequency";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFrequency
            // 
            this.KnobFrequency.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobFrequency.BorderWidth = 2;
            this.KnobFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobFrequency.HasTicks = true;
            this.KnobFrequency.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobFrequency.LargeChange = 10;
            this.KnobFrequency.Location = new System.Drawing.Point(6, 44);
            this.KnobFrequency.Minimum = -100;
            this.KnobFrequency.Name = "KnobFrequency";
            this.KnobFrequency.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobFrequency.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFrequency.PointerOffset = 4;
            this.KnobFrequency.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFrequency.PointerWidth = 2;
            this.KnobFrequency.Size = new System.Drawing.Size(64, 64);
            this.KnobFrequency.TabIndex = 1;
            this.KnobFrequency.Text = "Level";
            this.KnobFrequency.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckFrequency
            // 
            this.CheckFrequency.AutoSize = true;
            this.CheckFrequency.BackColor = System.Drawing.SystemColors.Control;
            this.CheckFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckFrequency.Location = new System.Drawing.Point(6, 3);
            this.CheckFrequency.Name = "CheckFrequency";
            this.CheckFrequency.Size = new System.Drawing.Size(80, 19);
            this.CheckFrequency.TabIndex = 0;
            this.CheckFrequency.Text = "Frequency";
            this.CheckFrequency.UseVisualStyleBackColor = false;
            this.CheckFrequency.CheckedChanged += new System.EventHandler(this.CheckFrequency_CheckedChanged);
            // 
            // GroupPitchShift
            // 
            this.GroupPitchShift.Controls.Add(this.lblValPitchFFT);
            this.GroupPitchShift.Controls.Add(this.label2);
            this.GroupPitchShift.Controls.Add(this.KnobPitchFFT);
            this.GroupPitchShift.Controls.Add(this.lblValPitchPitch);
            this.GroupPitchShift.Controls.Add(this.label4);
            this.GroupPitchShift.Controls.Add(this.KnobPitchPitch);
            this.GroupPitchShift.Controls.Add(this.CheckPitch);
            this.GroupPitchShift.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupPitchShift.Location = new System.Drawing.Point(3, 3);
            this.GroupPitchShift.Name = "GroupPitchShift";
            this.GroupPitchShift.Size = new System.Drawing.Size(149, 150);
            this.GroupPitchShift.TabIndex = 14;
            this.GroupPitchShift.TabStop = false;
            // 
            // lblValPitchFFT
            // 
            this.lblValPitchFFT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValPitchFFT.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValPitchFFT.Location = new System.Drawing.Point(77, 114);
            this.lblValPitchFFT.Name = "lblValPitchFFT";
            this.lblValPitchFFT.ReadOnly = true;
            this.lblValPitchFFT.Size = new System.Drawing.Size(64, 16);
            this.lblValPitchFFT.TabIndex = 5;
            this.lblValPitchFFT.Text = "0.0";
            this.lblValPitchFFT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(76, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "FFT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobPitchFFT
            // 
            this.KnobPitchFFT.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobPitchFFT.BorderWidth = 2;
            this.KnobPitchFFT.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobPitchFFT.HasTicks = true;
            this.KnobPitchFFT.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobPitchFFT.LargeChange = 1;
            this.KnobPitchFFT.Location = new System.Drawing.Point(77, 44);
            this.KnobPitchFFT.Maximum = 4;
            this.KnobPitchFFT.Name = "KnobPitchFFT";
            this.KnobPitchFFT.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobPitchFFT.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobPitchFFT.PointerOffset = 4;
            this.KnobPitchFFT.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobPitchFFT.PointerWidth = 2;
            this.KnobPitchFFT.Size = new System.Drawing.Size(64, 64);
            this.KnobPitchFFT.TabIndex = 3;
            this.KnobPitchFFT.Text = "Level";
            this.KnobPitchFFT.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValPitchPitch
            // 
            this.lblValPitchPitch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValPitchPitch.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValPitchPitch.Location = new System.Drawing.Point(6, 114);
            this.lblValPitchPitch.Name = "lblValPitchPitch";
            this.lblValPitchPitch.Size = new System.Drawing.Size(64, 16);
            this.lblValPitchPitch.TabIndex = 2;
            this.lblValPitchPitch.Text = "0.0";
            this.lblValPitchPitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Pitch";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobPitchPitch
            // 
            this.KnobPitchPitch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobPitchPitch.BorderWidth = 2;
            this.KnobPitchPitch.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobPitchPitch.HasTicks = true;
            this.KnobPitchPitch.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobPitchPitch.LargeChange = 1;
            this.KnobPitchPitch.Location = new System.Drawing.Point(6, 44);
            this.KnobPitchPitch.Maximum = 150;
            this.KnobPitchPitch.Name = "KnobPitchPitch";
            this.KnobPitchPitch.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobPitchPitch.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobPitchPitch.PointerOffset = 4;
            this.KnobPitchPitch.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobPitchPitch.PointerWidth = 2;
            this.KnobPitchPitch.Size = new System.Drawing.Size(64, 64);
            this.KnobPitchPitch.TabIndex = 1;
            this.KnobPitchPitch.Text = "Level";
            this.KnobPitchPitch.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckPitch
            // 
            this.CheckPitch.AutoSize = true;
            this.CheckPitch.BackColor = System.Drawing.SystemColors.Control;
            this.CheckPitch.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckPitch.Location = new System.Drawing.Point(6, 3);
            this.CheckPitch.Name = "CheckPitch";
            this.CheckPitch.Size = new System.Drawing.Size(77, 19);
            this.CheckPitch.TabIndex = 0;
            this.CheckPitch.Text = "PitchShift";
            this.CheckPitch.UseVisualStyleBackColor = false;
            this.CheckPitch.CheckedChanged += new System.EventHandler(this.CheckPitch_CheckedChanged);
            // 
            // tabDistortion
            // 
            this.tabDistortion.BackColor = System.Drawing.Color.Transparent;
            this.tabDistortion.Controls.Add(this.groupBox2);
            this.tabDistortion.Location = new System.Drawing.Point(4, 24);
            this.tabDistortion.Name = "tabDistortion";
            this.tabDistortion.Padding = new System.Windows.Forms.Padding(3);
            this.tabDistortion.Size = new System.Drawing.Size(574, 391);
            this.tabDistortion.TabIndex = 0;
            this.tabDistortion.Text = "Distortion";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CheckDistortion);
            this.groupBox2.Controls.Add(this.lblValDistortionLevel);
            this.groupBox2.Controls.Add(this.KnobDistortionLevel);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(103, 142);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // CheckDistortion
            // 
            this.CheckDistortion.AutoSize = true;
            this.CheckDistortion.BackColor = System.Drawing.SystemColors.Control;
            this.CheckDistortion.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckDistortion.Location = new System.Drawing.Point(6, 0);
            this.CheckDistortion.Name = "CheckDistortion";
            this.CheckDistortion.Size = new System.Drawing.Size(78, 19);
            this.CheckDistortion.TabIndex = 5;
            this.CheckDistortion.Text = "Distortion";
            this.CheckDistortion.UseVisualStyleBackColor = false;
            this.CheckDistortion.CheckedChanged += new System.EventHandler(this.CheckDistortion_CheckedChanged);
            // 
            // lblValDistortionLevel
            // 
            this.lblValDistortionLevel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValDistortionLevel.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValDistortionLevel.Location = new System.Drawing.Point(20, 110);
            this.lblValDistortionLevel.Name = "lblValDistortionLevel";
            this.lblValDistortionLevel.Size = new System.Drawing.Size(64, 16);
            this.lblValDistortionLevel.TabIndex = 11;
            this.lblValDistortionLevel.Text = "0.0";
            this.lblValDistortionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // KnobDistortionLevel
            // 
            this.KnobDistortionLevel.BackColor = System.Drawing.SystemColors.Control;
            this.KnobDistortionLevel.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobDistortionLevel.BorderWidth = 2;
            this.KnobDistortionLevel.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobDistortionLevel.HasTicks = true;
            this.KnobDistortionLevel.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobDistortionLevel.LargeChange = 5;
            this.KnobDistortionLevel.Location = new System.Drawing.Point(20, 38);
            this.KnobDistortionLevel.Name = "KnobDistortionLevel";
            this.KnobDistortionLevel.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobDistortionLevel.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobDistortionLevel.PointerOffset = 4;
            this.KnobDistortionLevel.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobDistortionLevel.PointerWidth = 2;
            this.KnobDistortionLevel.Size = new System.Drawing.Size(64, 64);
            this.KnobDistortionLevel.TabIndex = 9;
            this.KnobDistortionLevel.Text = "Level";
            this.KnobDistortionLevel.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Level";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabChorus
            // 
            this.tabChorus.BackColor = System.Drawing.Color.Transparent;
            this.tabChorus.Controls.Add(this.GroupChorus);
            this.tabChorus.Location = new System.Drawing.Point(4, 24);
            this.tabChorus.Name = "tabChorus";
            this.tabChorus.Padding = new System.Windows.Forms.Padding(3);
            this.tabChorus.Size = new System.Drawing.Size(574, 391);
            this.tabChorus.TabIndex = 1;
            this.tabChorus.Text = "Chorus";
            // 
            // GroupChorus
            // 
            this.GroupChorus.Controls.Add(this.lblValChorusDepth);
            this.GroupChorus.Controls.Add(this.label7);
            this.GroupChorus.Controls.Add(this.KnobChorusDepth);
            this.GroupChorus.Controls.Add(this.lblValChorusRate);
            this.GroupChorus.Controls.Add(this.label5);
            this.GroupChorus.Controls.Add(this.KnobChorusRate);
            this.GroupChorus.Controls.Add(this.lblValChorusMix);
            this.GroupChorus.Controls.Add(this.label3);
            this.GroupChorus.Controls.Add(this.KnobChorusMix);
            this.GroupChorus.Controls.Add(this.CheckChorus);
            this.GroupChorus.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupChorus.Location = new System.Drawing.Point(3, 3);
            this.GroupChorus.Name = "GroupChorus";
            this.GroupChorus.Size = new System.Drawing.Size(223, 152);
            this.GroupChorus.TabIndex = 4;
            this.GroupChorus.TabStop = false;
            // 
            // lblValChorusDepth
            // 
            this.lblValChorusDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValChorusDepth.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValChorusDepth.Location = new System.Drawing.Point(146, 117);
            this.lblValChorusDepth.Name = "lblValChorusDepth";
            this.lblValChorusDepth.Size = new System.Drawing.Size(64, 16);
            this.lblValChorusDepth.TabIndex = 8;
            this.lblValChorusDepth.Text = "0.0";
            this.lblValChorusDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(146, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "Depth";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusDepth
            // 
            this.KnobChorusDepth.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobChorusDepth.BorderWidth = 2;
            this.KnobChorusDepth.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobChorusDepth.HasTicks = true;
            this.KnobChorusDepth.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobChorusDepth.LargeChange = 5;
            this.KnobChorusDepth.Location = new System.Drawing.Point(146, 47);
            this.KnobChorusDepth.Name = "KnobChorusDepth";
            this.KnobChorusDepth.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobChorusDepth.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusDepth.PointerOffset = 4;
            this.KnobChorusDepth.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusDepth.PointerWidth = 2;
            this.KnobChorusDepth.Size = new System.Drawing.Size(64, 64);
            this.KnobChorusDepth.TabIndex = 6;
            this.KnobChorusDepth.Text = "Level";
            this.KnobChorusDepth.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValChorusRate
            // 
            this.lblValChorusRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValChorusRate.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValChorusRate.Location = new System.Drawing.Point(76, 117);
            this.lblValChorusRate.Name = "lblValChorusRate";
            this.lblValChorusRate.Size = new System.Drawing.Size(64, 16);
            this.lblValChorusRate.TabIndex = 5;
            this.lblValChorusRate.Text = "0.0";
            this.lblValChorusRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(76, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Rate";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusRate
            // 
            this.KnobChorusRate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobChorusRate.BorderWidth = 2;
            this.KnobChorusRate.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobChorusRate.HasTicks = true;
            this.KnobChorusRate.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobChorusRate.LargeChange = 1;
            this.KnobChorusRate.Location = new System.Drawing.Point(76, 47);
            this.KnobChorusRate.Maximum = 20;
            this.KnobChorusRate.Name = "KnobChorusRate";
            this.KnobChorusRate.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobChorusRate.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusRate.PointerOffset = 4;
            this.KnobChorusRate.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusRate.PointerWidth = 2;
            this.KnobChorusRate.Size = new System.Drawing.Size(64, 64);
            this.KnobChorusRate.TabIndex = 3;
            this.KnobChorusRate.Text = "Level";
            this.KnobChorusRate.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValChorusMix
            // 
            this.lblValChorusMix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValChorusMix.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValChorusMix.Location = new System.Drawing.Point(6, 117);
            this.lblValChorusMix.Name = "lblValChorusMix";
            this.lblValChorusMix.Size = new System.Drawing.Size(64, 16);
            this.lblValChorusMix.TabIndex = 2;
            this.lblValChorusMix.Text = "0.0";
            this.lblValChorusMix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mix";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusMix
            // 
            this.KnobChorusMix.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobChorusMix.BorderWidth = 2;
            this.KnobChorusMix.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobChorusMix.HasTicks = true;
            this.KnobChorusMix.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobChorusMix.LargeChange = 5;
            this.KnobChorusMix.Location = new System.Drawing.Point(6, 47);
            this.KnobChorusMix.Name = "KnobChorusMix";
            this.KnobChorusMix.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobChorusMix.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusMix.PointerOffset = 4;
            this.KnobChorusMix.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobChorusMix.PointerWidth = 2;
            this.KnobChorusMix.Size = new System.Drawing.Size(64, 64);
            this.KnobChorusMix.TabIndex = 1;
            this.KnobChorusMix.Text = "Level";
            this.KnobChorusMix.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckChorus
            // 
            this.CheckChorus.AutoSize = true;
            this.CheckChorus.BackColor = System.Drawing.SystemColors.Control;
            this.CheckChorus.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckChorus.Location = new System.Drawing.Point(3, 3);
            this.CheckChorus.Name = "CheckChorus";
            this.CheckChorus.Size = new System.Drawing.Size(63, 19);
            this.CheckChorus.TabIndex = 0;
            this.CheckChorus.Text = "Chorus";
            this.CheckChorus.UseVisualStyleBackColor = false;
            this.CheckChorus.CheckedChanged += new System.EventHandler(this.CheckChorus_CheckedChanged);
            // 
            // tabEcho
            // 
            this.tabEcho.BackColor = System.Drawing.Color.Transparent;
            this.tabEcho.Controls.Add(this.GroupEcho);
            this.tabEcho.Location = new System.Drawing.Point(4, 24);
            this.tabEcho.Name = "tabEcho";
            this.tabEcho.Size = new System.Drawing.Size(574, 391);
            this.tabEcho.TabIndex = 2;
            this.tabEcho.Text = "Echo";
            // 
            // GroupEcho
            // 
            this.GroupEcho.BackColor = System.Drawing.Color.Transparent;
            this.GroupEcho.Controls.Add(this.lblValEchoWet);
            this.GroupEcho.Controls.Add(this.label15);
            this.GroupEcho.Controls.Add(this.KnobEchoWet);
            this.GroupEcho.Controls.Add(this.lblValEchoDry);
            this.GroupEcho.Controls.Add(this.label9);
            this.GroupEcho.Controls.Add(this.KnobEchoDry);
            this.GroupEcho.Controls.Add(this.lblValEchoFeedback);
            this.GroupEcho.Controls.Add(this.label11);
            this.GroupEcho.Controls.Add(this.KnobEchoFeedback);
            this.GroupEcho.Controls.Add(this.lblValEchoDelay);
            this.GroupEcho.Controls.Add(this.label13);
            this.GroupEcho.Controls.Add(this.KnobEchoDelay);
            this.GroupEcho.Controls.Add(this.CheckEcho);
            this.GroupEcho.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupEcho.Location = new System.Drawing.Point(3, 3);
            this.GroupEcho.Name = "GroupEcho";
            this.GroupEcho.Size = new System.Drawing.Size(297, 152);
            this.GroupEcho.TabIndex = 10;
            this.GroupEcho.TabStop = false;
            // 
            // lblValEchoWet
            // 
            this.lblValEchoWet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValEchoWet.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValEchoWet.Location = new System.Drawing.Point(216, 114);
            this.lblValEchoWet.Name = "lblValEchoWet";
            this.lblValEchoWet.Size = new System.Drawing.Size(64, 16);
            this.lblValEchoWet.TabIndex = 11;
            this.lblValEchoWet.Text = "0.0";
            this.lblValEchoWet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(216, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 16);
            this.label15.TabIndex = 10;
            this.label15.Text = "Wet";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoWet
            // 
            this.KnobEchoWet.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobEchoWet.BorderWidth = 2;
            this.KnobEchoWet.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobEchoWet.HasTicks = true;
            this.KnobEchoWet.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobEchoWet.LargeChange = 5;
            this.KnobEchoWet.Location = new System.Drawing.Point(216, 44);
            this.KnobEchoWet.Maximum = 10;
            this.KnobEchoWet.Minimum = -80;
            this.KnobEchoWet.Name = "KnobEchoWet";
            this.KnobEchoWet.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobEchoWet.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoWet.PointerOffset = 4;
            this.KnobEchoWet.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoWet.PointerWidth = 2;
            this.KnobEchoWet.Size = new System.Drawing.Size(64, 64);
            this.KnobEchoWet.TabIndex = 9;
            this.KnobEchoWet.Text = "Level";
            this.KnobEchoWet.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValEchoDry
            // 
            this.lblValEchoDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValEchoDry.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValEchoDry.Location = new System.Drawing.Point(146, 114);
            this.lblValEchoDry.Name = "lblValEchoDry";
            this.lblValEchoDry.Size = new System.Drawing.Size(64, 16);
            this.lblValEchoDry.TabIndex = 8;
            this.lblValEchoDry.Text = "0.0";
            this.lblValEchoDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(146, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 7;
            this.label9.Text = "Dry";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoDry
            // 
            this.KnobEchoDry.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobEchoDry.BorderWidth = 2;
            this.KnobEchoDry.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobEchoDry.HasTicks = true;
            this.KnobEchoDry.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobEchoDry.LargeChange = 5;
            this.KnobEchoDry.Location = new System.Drawing.Point(146, 44);
            this.KnobEchoDry.Maximum = 10;
            this.KnobEchoDry.Minimum = -80;
            this.KnobEchoDry.Name = "KnobEchoDry";
            this.KnobEchoDry.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobEchoDry.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoDry.PointerOffset = 4;
            this.KnobEchoDry.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoDry.PointerWidth = 2;
            this.KnobEchoDry.Size = new System.Drawing.Size(64, 64);
            this.KnobEchoDry.TabIndex = 6;
            this.KnobEchoDry.Text = "Level";
            this.KnobEchoDry.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValEchoFeedback
            // 
            this.lblValEchoFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValEchoFeedback.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValEchoFeedback.Location = new System.Drawing.Point(76, 114);
            this.lblValEchoFeedback.Name = "lblValEchoFeedback";
            this.lblValEchoFeedback.Size = new System.Drawing.Size(64, 16);
            this.lblValEchoFeedback.TabIndex = 5;
            this.lblValEchoFeedback.Text = "0.0";
            this.lblValEchoFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(76, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 16);
            this.label11.TabIndex = 4;
            this.label11.Text = "Feedback";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoFeedback
            // 
            this.KnobEchoFeedback.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobEchoFeedback.BorderWidth = 2;
            this.KnobEchoFeedback.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobEchoFeedback.HasTicks = true;
            this.KnobEchoFeedback.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobEchoFeedback.LargeChange = 5;
            this.KnobEchoFeedback.Location = new System.Drawing.Point(76, 44);
            this.KnobEchoFeedback.Name = "KnobEchoFeedback";
            this.KnobEchoFeedback.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobEchoFeedback.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoFeedback.PointerOffset = 4;
            this.KnobEchoFeedback.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoFeedback.PointerWidth = 2;
            this.KnobEchoFeedback.Size = new System.Drawing.Size(64, 64);
            this.KnobEchoFeedback.TabIndex = 3;
            this.KnobEchoFeedback.Text = "Level";
            this.KnobEchoFeedback.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValEchoDelay
            // 
            this.lblValEchoDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValEchoDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValEchoDelay.Location = new System.Drawing.Point(6, 114);
            this.lblValEchoDelay.Name = "lblValEchoDelay";
            this.lblValEchoDelay.Size = new System.Drawing.Size(64, 16);
            this.lblValEchoDelay.TabIndex = 2;
            this.lblValEchoDelay.Text = "0.0";
            this.lblValEchoDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(6, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 16);
            this.label13.TabIndex = 2;
            this.label13.Text = "Delay";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoDelay
            // 
            this.KnobEchoDelay.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobEchoDelay.BorderWidth = 2;
            this.KnobEchoDelay.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobEchoDelay.HasTicks = true;
            this.KnobEchoDelay.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobEchoDelay.LargeChange = 100;
            this.KnobEchoDelay.Location = new System.Drawing.Point(6, 44);
            this.KnobEchoDelay.Maximum = 5000;
            this.KnobEchoDelay.Minimum = 1;
            this.KnobEchoDelay.Name = "KnobEchoDelay";
            this.KnobEchoDelay.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobEchoDelay.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoDelay.PointerOffset = 4;
            this.KnobEchoDelay.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobEchoDelay.PointerWidth = 2;
            this.KnobEchoDelay.Size = new System.Drawing.Size(64, 64);
            this.KnobEchoDelay.TabIndex = 1;
            this.KnobEchoDelay.Text = "Level";
            this.KnobEchoDelay.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckEcho
            // 
            this.CheckEcho.AutoSize = true;
            this.CheckEcho.BackColor = System.Drawing.SystemColors.Control;
            this.CheckEcho.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckEcho.Location = new System.Drawing.Point(6, 3);
            this.CheckEcho.Name = "CheckEcho";
            this.CheckEcho.Size = new System.Drawing.Size(52, 19);
            this.CheckEcho.TabIndex = 0;
            this.CheckEcho.Text = "Echo";
            this.CheckEcho.UseVisualStyleBackColor = false;
            this.CheckEcho.CheckedChanged += new System.EventHandler(this.CheckEcho_CheckedChanged);
            // 
            // tabFlanger
            // 
            this.tabFlanger.BackColor = System.Drawing.Color.Transparent;
            this.tabFlanger.Controls.Add(this.GroupFlanger);
            this.tabFlanger.Location = new System.Drawing.Point(4, 24);
            this.tabFlanger.Name = "tabFlanger";
            this.tabFlanger.Size = new System.Drawing.Size(574, 391);
            this.tabFlanger.TabIndex = 3;
            this.tabFlanger.Text = "Flanger";
            // 
            // GroupFlanger
            // 
            this.GroupFlanger.Controls.Add(this.lblValFlangerDepth);
            this.GroupFlanger.Controls.Add(this.label33);
            this.GroupFlanger.Controls.Add(this.KnobFlangerDepth);
            this.GroupFlanger.Controls.Add(this.lblValFlangerRate);
            this.GroupFlanger.Controls.Add(this.label35);
            this.GroupFlanger.Controls.Add(this.KnobFlangerRate);
            this.GroupFlanger.Controls.Add(this.lblValFlangerMix);
            this.GroupFlanger.Controls.Add(this.label37);
            this.GroupFlanger.Controls.Add(this.KnobFlangerMix);
            this.GroupFlanger.Controls.Add(this.CheckFlanger);
            this.GroupFlanger.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupFlanger.Location = new System.Drawing.Point(3, 3);
            this.GroupFlanger.Name = "GroupFlanger";
            this.GroupFlanger.Size = new System.Drawing.Size(225, 155);
            this.GroupFlanger.TabIndex = 10;
            this.GroupFlanger.TabStop = false;
            // 
            // lblValFlangerDepth
            // 
            this.lblValFlangerDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValFlangerDepth.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValFlangerDepth.Location = new System.Drawing.Point(146, 114);
            this.lblValFlangerDepth.Name = "lblValFlangerDepth";
            this.lblValFlangerDepth.Size = new System.Drawing.Size(64, 16);
            this.lblValFlangerDepth.TabIndex = 8;
            this.lblValFlangerDepth.Text = "0.0";
            this.lblValFlangerDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label33.Location = new System.Drawing.Point(146, 25);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(64, 16);
            this.label33.TabIndex = 7;
            this.label33.Text = "Depth";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerDepth
            // 
            this.KnobFlangerDepth.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobFlangerDepth.BorderWidth = 2;
            this.KnobFlangerDepth.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobFlangerDepth.HasTicks = true;
            this.KnobFlangerDepth.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobFlangerDepth.LargeChange = 5;
            this.KnobFlangerDepth.Location = new System.Drawing.Point(146, 44);
            this.KnobFlangerDepth.Minimum = 1;
            this.KnobFlangerDepth.Name = "KnobFlangerDepth";
            this.KnobFlangerDepth.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobFlangerDepth.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerDepth.PointerOffset = 4;
            this.KnobFlangerDepth.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerDepth.PointerWidth = 2;
            this.KnobFlangerDepth.Size = new System.Drawing.Size(64, 64);
            this.KnobFlangerDepth.TabIndex = 6;
            this.KnobFlangerDepth.Text = "Level";
            this.KnobFlangerDepth.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValFlangerRate
            // 
            this.lblValFlangerRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValFlangerRate.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValFlangerRate.Location = new System.Drawing.Point(76, 114);
            this.lblValFlangerRate.Name = "lblValFlangerRate";
            this.lblValFlangerRate.Size = new System.Drawing.Size(64, 16);
            this.lblValFlangerRate.TabIndex = 5;
            this.lblValFlangerRate.Text = "0.0";
            this.lblValFlangerRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label35.Location = new System.Drawing.Point(76, 25);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(64, 16);
            this.label35.TabIndex = 4;
            this.label35.Text = "Rate";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerRate
            // 
            this.KnobFlangerRate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobFlangerRate.BorderWidth = 2;
            this.KnobFlangerRate.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobFlangerRate.HasTicks = true;
            this.KnobFlangerRate.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobFlangerRate.LargeChange = 1;
            this.KnobFlangerRate.Location = new System.Drawing.Point(76, 44);
            this.KnobFlangerRate.Maximum = 20;
            this.KnobFlangerRate.Name = "KnobFlangerRate";
            this.KnobFlangerRate.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobFlangerRate.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerRate.PointerOffset = 4;
            this.KnobFlangerRate.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerRate.PointerWidth = 2;
            this.KnobFlangerRate.Size = new System.Drawing.Size(64, 64);
            this.KnobFlangerRate.TabIndex = 3;
            this.KnobFlangerRate.Text = "Level";
            this.KnobFlangerRate.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValFlangerMix
            // 
            this.lblValFlangerMix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValFlangerMix.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValFlangerMix.Location = new System.Drawing.Point(6, 114);
            this.lblValFlangerMix.Name = "lblValFlangerMix";
            this.lblValFlangerMix.Size = new System.Drawing.Size(64, 16);
            this.lblValFlangerMix.TabIndex = 2;
            this.lblValFlangerMix.Text = "0.0";
            this.lblValFlangerMix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label37.Location = new System.Drawing.Point(6, 25);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(64, 16);
            this.label37.TabIndex = 2;
            this.label37.Text = "Mix";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerMix
            // 
            this.KnobFlangerMix.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobFlangerMix.BorderWidth = 2;
            this.KnobFlangerMix.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobFlangerMix.HasTicks = true;
            this.KnobFlangerMix.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobFlangerMix.LargeChange = 5;
            this.KnobFlangerMix.Location = new System.Drawing.Point(6, 44);
            this.KnobFlangerMix.Name = "KnobFlangerMix";
            this.KnobFlangerMix.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobFlangerMix.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerMix.PointerOffset = 4;
            this.KnobFlangerMix.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobFlangerMix.PointerWidth = 2;
            this.KnobFlangerMix.Size = new System.Drawing.Size(64, 64);
            this.KnobFlangerMix.TabIndex = 1;
            this.KnobFlangerMix.Text = "Level";
            this.KnobFlangerMix.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckFlanger
            // 
            this.CheckFlanger.AutoSize = true;
            this.CheckFlanger.BackColor = System.Drawing.SystemColors.Control;
            this.CheckFlanger.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckFlanger.Location = new System.Drawing.Point(6, 3);
            this.CheckFlanger.Name = "CheckFlanger";
            this.CheckFlanger.Size = new System.Drawing.Size(65, 19);
            this.CheckFlanger.TabIndex = 0;
            this.CheckFlanger.Text = "Flanger";
            this.CheckFlanger.UseVisualStyleBackColor = false;
            this.CheckFlanger.CheckedChanged += new System.EventHandler(this.CheckFlanger_CheckedChanged);
            // 
            // tabHightpass
            // 
            this.tabHightpass.BackColor = System.Drawing.Color.Transparent;
            this.tabHightpass.Controls.Add(this.GroupHighpass);
            this.tabHightpass.Location = new System.Drawing.Point(4, 24);
            this.tabHightpass.Name = "tabHightpass";
            this.tabHightpass.Size = new System.Drawing.Size(574, 391);
            this.tabHightpass.TabIndex = 4;
            this.tabHightpass.Text = "Highpass";
            // 
            // GroupHighpass
            // 
            this.GroupHighpass.Controls.Add(this.lblValHighpassResonance);
            this.GroupHighpass.Controls.Add(this.label21);
            this.GroupHighpass.Controls.Add(this.KnobHighpassResonance);
            this.GroupHighpass.Controls.Add(this.lblValHighpassCutoff);
            this.GroupHighpass.Controls.Add(this.label23);
            this.GroupHighpass.Controls.Add(this.KnobHighpassCutoff);
            this.GroupHighpass.Controls.Add(this.CheckHighpass);
            this.GroupHighpass.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupHighpass.Location = new System.Drawing.Point(3, 3);
            this.GroupHighpass.Name = "GroupHighpass";
            this.GroupHighpass.Size = new System.Drawing.Size(163, 148);
            this.GroupHighpass.TabIndex = 13;
            this.GroupHighpass.TabStop = false;
            // 
            // lblValHighpassResonance
            // 
            this.lblValHighpassResonance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValHighpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValHighpassResonance.Location = new System.Drawing.Point(76, 114);
            this.lblValHighpassResonance.Name = "lblValHighpassResonance";
            this.lblValHighpassResonance.Size = new System.Drawing.Size(64, 16);
            this.lblValHighpassResonance.TabIndex = 5;
            this.lblValHighpassResonance.Text = "0.0";
            this.lblValHighpassResonance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label21.Location = new System.Drawing.Point(76, 25);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(64, 16);
            this.label21.TabIndex = 4;
            this.label21.Text = "Resonance";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobHighpassResonance
            // 
            this.KnobHighpassResonance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobHighpassResonance.BorderWidth = 2;
            this.KnobHighpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobHighpassResonance.HasTicks = true;
            this.KnobHighpassResonance.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobHighpassResonance.LargeChange = 5;
            this.KnobHighpassResonance.Location = new System.Drawing.Point(76, 44);
            this.KnobHighpassResonance.Name = "KnobHighpassResonance";
            this.KnobHighpassResonance.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobHighpassResonance.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobHighpassResonance.PointerOffset = 4;
            this.KnobHighpassResonance.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobHighpassResonance.PointerWidth = 2;
            this.KnobHighpassResonance.Size = new System.Drawing.Size(64, 64);
            this.KnobHighpassResonance.TabIndex = 3;
            this.KnobHighpassResonance.Text = "Level";
            this.KnobHighpassResonance.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValHighpassCutoff
            // 
            this.lblValHighpassCutoff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValHighpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValHighpassCutoff.Location = new System.Drawing.Point(6, 114);
            this.lblValHighpassCutoff.Name = "lblValHighpassCutoff";
            this.lblValHighpassCutoff.Size = new System.Drawing.Size(64, 16);
            this.lblValHighpassCutoff.TabIndex = 2;
            this.lblValHighpassCutoff.Text = "0.0";
            this.lblValHighpassCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label23.Location = new System.Drawing.Point(6, 25);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(64, 16);
            this.label23.TabIndex = 2;
            this.label23.Text = "Cutoff";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobHighpassCutoff
            // 
            this.KnobHighpassCutoff.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobHighpassCutoff.BorderWidth = 2;
            this.KnobHighpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobHighpassCutoff.HasTicks = true;
            this.KnobHighpassCutoff.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobHighpassCutoff.LargeChange = 500;
            this.KnobHighpassCutoff.Location = new System.Drawing.Point(6, 44);
            this.KnobHighpassCutoff.Maximum = 22000;
            this.KnobHighpassCutoff.Minimum = 1;
            this.KnobHighpassCutoff.Name = "KnobHighpassCutoff";
            this.KnobHighpassCutoff.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobHighpassCutoff.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobHighpassCutoff.PointerOffset = 4;
            this.KnobHighpassCutoff.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobHighpassCutoff.PointerWidth = 2;
            this.KnobHighpassCutoff.Size = new System.Drawing.Size(64, 64);
            this.KnobHighpassCutoff.TabIndex = 1;
            this.KnobHighpassCutoff.Text = "Level";
            this.KnobHighpassCutoff.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckHighpass
            // 
            this.CheckHighpass.AutoSize = true;
            this.CheckHighpass.BackColor = System.Drawing.SystemColors.Control;
            this.CheckHighpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckHighpass.Location = new System.Drawing.Point(6, 3);
            this.CheckHighpass.Name = "CheckHighpass";
            this.CheckHighpass.Size = new System.Drawing.Size(75, 19);
            this.CheckHighpass.TabIndex = 0;
            this.CheckHighpass.Text = "Highpass";
            this.CheckHighpass.UseVisualStyleBackColor = false;
            this.CheckHighpass.CheckedChanged += new System.EventHandler(this.CheckHighpass_CheckedChanged);
            // 
            // tabLowpass
            // 
            this.tabLowpass.BackColor = System.Drawing.Color.Transparent;
            this.tabLowpass.Controls.Add(this.GroupLowpass);
            this.tabLowpass.Location = new System.Drawing.Point(4, 24);
            this.tabLowpass.Name = "tabLowpass";
            this.tabLowpass.Size = new System.Drawing.Size(574, 391);
            this.tabLowpass.TabIndex = 5;
            this.tabLowpass.Text = "Lowpass";
            // 
            // GroupLowpass
            // 
            this.GroupLowpass.Controls.Add(this.lblValLowpassResonance);
            this.GroupLowpass.Controls.Add(this.label17);
            this.GroupLowpass.Controls.Add(this.KnobLowpassResonance);
            this.GroupLowpass.Controls.Add(this.lblValLowpassCutoff);
            this.GroupLowpass.Controls.Add(this.label19);
            this.GroupLowpass.Controls.Add(this.KnobLowpassCutoff);
            this.GroupLowpass.Controls.Add(this.CheckLowpass);
            this.GroupLowpass.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupLowpass.Location = new System.Drawing.Point(3, 3);
            this.GroupLowpass.Name = "GroupLowpass";
            this.GroupLowpass.Size = new System.Drawing.Size(158, 145);
            this.GroupLowpass.TabIndex = 14;
            this.GroupLowpass.TabStop = false;
            // 
            // lblValLowpassResonance
            // 
            this.lblValLowpassResonance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValLowpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValLowpassResonance.Location = new System.Drawing.Point(76, 113);
            this.lblValLowpassResonance.Name = "lblValLowpassResonance";
            this.lblValLowpassResonance.Size = new System.Drawing.Size(64, 16);
            this.lblValLowpassResonance.TabIndex = 5;
            this.lblValLowpassResonance.Text = "0.0";
            this.lblValLowpassResonance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label17.Location = new System.Drawing.Point(76, 24);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(64, 16);
            this.label17.TabIndex = 4;
            this.label17.Text = "Resonance";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobLowpassResonance
            // 
            this.KnobLowpassResonance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobLowpassResonance.BorderWidth = 2;
            this.KnobLowpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobLowpassResonance.HasTicks = true;
            this.KnobLowpassResonance.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobLowpassResonance.LargeChange = 5;
            this.KnobLowpassResonance.Location = new System.Drawing.Point(76, 43);
            this.KnobLowpassResonance.Name = "KnobLowpassResonance";
            this.KnobLowpassResonance.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobLowpassResonance.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobLowpassResonance.PointerOffset = 4;
            this.KnobLowpassResonance.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobLowpassResonance.PointerWidth = 2;
            this.KnobLowpassResonance.Size = new System.Drawing.Size(64, 64);
            this.KnobLowpassResonance.TabIndex = 3;
            this.KnobLowpassResonance.Text = "Level";
            this.KnobLowpassResonance.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValLowpassCutoff
            // 
            this.lblValLowpassCutoff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValLowpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValLowpassCutoff.Location = new System.Drawing.Point(6, 113);
            this.lblValLowpassCutoff.Name = "lblValLowpassCutoff";
            this.lblValLowpassCutoff.Size = new System.Drawing.Size(64, 16);
            this.lblValLowpassCutoff.TabIndex = 2;
            this.lblValLowpassCutoff.Text = "0.0";
            this.lblValLowpassCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label19.Location = new System.Drawing.Point(6, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 16);
            this.label19.TabIndex = 2;
            this.label19.Text = "Cutoff";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobLowpassCutoff
            // 
            this.KnobLowpassCutoff.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobLowpassCutoff.BorderWidth = 2;
            this.KnobLowpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobLowpassCutoff.HasTicks = true;
            this.KnobLowpassCutoff.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobLowpassCutoff.LargeChange = 500;
            this.KnobLowpassCutoff.Location = new System.Drawing.Point(6, 43);
            this.KnobLowpassCutoff.Maximum = 22000;
            this.KnobLowpassCutoff.Minimum = 1;
            this.KnobLowpassCutoff.Name = "KnobLowpassCutoff";
            this.KnobLowpassCutoff.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobLowpassCutoff.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobLowpassCutoff.PointerOffset = 4;
            this.KnobLowpassCutoff.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobLowpassCutoff.PointerWidth = 2;
            this.KnobLowpassCutoff.Size = new System.Drawing.Size(64, 64);
            this.KnobLowpassCutoff.TabIndex = 1;
            this.KnobLowpassCutoff.Text = "Level";
            this.KnobLowpassCutoff.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckLowpass
            // 
            this.CheckLowpass.AutoSize = true;
            this.CheckLowpass.BackColor = System.Drawing.SystemColors.Control;
            this.CheckLowpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckLowpass.Location = new System.Drawing.Point(6, 3);
            this.CheckLowpass.Name = "CheckLowpass";
            this.CheckLowpass.Size = new System.Drawing.Size(71, 19);
            this.CheckLowpass.TabIndex = 0;
            this.CheckLowpass.Text = "Lowpass";
            this.CheckLowpass.UseVisualStyleBackColor = false;
            this.CheckLowpass.CheckedChanged += new System.EventHandler(this.CheckLowpass_CheckedChanged);
            // 
            // tabCompressor
            // 
            this.tabCompressor.BackColor = System.Drawing.Color.Transparent;
            this.tabCompressor.Controls.Add(this.GroupCompressor);
            this.tabCompressor.Location = new System.Drawing.Point(4, 24);
            this.tabCompressor.Name = "tabCompressor";
            this.tabCompressor.Size = new System.Drawing.Size(574, 391);
            this.tabCompressor.TabIndex = 6;
            this.tabCompressor.Text = "Compressor";
            // 
            // GroupCompressor
            // 
            this.GroupCompressor.Controls.Add(this.CheckCompLinked);
            this.GroupCompressor.Controls.Add(this.lblValCompGain);
            this.GroupCompressor.Controls.Add(this.label39);
            this.GroupCompressor.Controls.Add(this.KnobCompGain);
            this.GroupCompressor.Controls.Add(this.lblValCompRelease);
            this.GroupCompressor.Controls.Add(this.label25);
            this.GroupCompressor.Controls.Add(this.KnobCompRelease);
            this.GroupCompressor.Controls.Add(this.lblValCompAttack);
            this.GroupCompressor.Controls.Add(this.label27);
            this.GroupCompressor.Controls.Add(this.KnobCompAttack);
            this.GroupCompressor.Controls.Add(this.lblValCompRatio);
            this.GroupCompressor.Controls.Add(this.label29);
            this.GroupCompressor.Controls.Add(this.KnobCompRatio);
            this.GroupCompressor.Controls.Add(this.lblValCompThreshold);
            this.GroupCompressor.Controls.Add(this.label31);
            this.GroupCompressor.Controls.Add(this.KnobCompThreshold);
            this.GroupCompressor.Controls.Add(this.CheckCompressor);
            this.GroupCompressor.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GroupCompressor.Location = new System.Drawing.Point(3, 3);
            this.GroupCompressor.Name = "GroupCompressor";
            this.GroupCompressor.Size = new System.Drawing.Size(368, 160);
            this.GroupCompressor.TabIndex = 13;
            this.GroupCompressor.TabStop = false;
            // 
            // CheckCompLinked
            // 
            this.CheckCompLinked.AutoSize = true;
            this.CheckCompLinked.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckCompLinked.Location = new System.Drawing.Point(6, 133);
            this.CheckCompLinked.Name = "CheckCompLinked";
            this.CheckCompLinked.Size = new System.Drawing.Size(61, 19);
            this.CheckCompLinked.TabIndex = 16;
            this.CheckCompLinked.Text = "Linked";
            this.CheckCompLinked.UseVisualStyleBackColor = true;
            this.CheckCompLinked.Visible = false;
            // 
            // lblValCompGain
            // 
            this.lblValCompGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValCompGain.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValCompGain.Location = new System.Drawing.Point(286, 111);
            this.lblValCompGain.Name = "lblValCompGain";
            this.lblValCompGain.Size = new System.Drawing.Size(64, 16);
            this.lblValCompGain.TabIndex = 14;
            this.lblValCompGain.Text = "0.0";
            this.lblValCompGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label39.Location = new System.Drawing.Point(286, 22);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(64, 16);
            this.label39.TabIndex = 13;
            this.label39.Text = "Gain";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompGain
            // 
            this.KnobCompGain.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobCompGain.BorderWidth = 2;
            this.KnobCompGain.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobCompGain.HasTicks = true;
            this.KnobCompGain.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobCompGain.LargeChange = 1;
            this.KnobCompGain.Location = new System.Drawing.Point(286, 41);
            this.KnobCompGain.Maximum = 30;
            this.KnobCompGain.Minimum = -30;
            this.KnobCompGain.Name = "KnobCompGain";
            this.KnobCompGain.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobCompGain.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompGain.PointerOffset = 4;
            this.KnobCompGain.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompGain.PointerWidth = 2;
            this.KnobCompGain.Size = new System.Drawing.Size(64, 64);
            this.KnobCompGain.TabIndex = 12;
            this.KnobCompGain.Text = "Level";
            this.KnobCompGain.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValCompRelease
            // 
            this.lblValCompRelease.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValCompRelease.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValCompRelease.Location = new System.Drawing.Point(216, 111);
            this.lblValCompRelease.Name = "lblValCompRelease";
            this.lblValCompRelease.Size = new System.Drawing.Size(64, 16);
            this.lblValCompRelease.TabIndex = 11;
            this.lblValCompRelease.Text = "0.0";
            this.lblValCompRelease.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label25.Location = new System.Drawing.Point(216, 22);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(64, 16);
            this.label25.TabIndex = 10;
            this.label25.Text = "Release";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompRelease
            // 
            this.KnobCompRelease.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobCompRelease.BorderWidth = 2;
            this.KnobCompRelease.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobCompRelease.HasTicks = true;
            this.KnobCompRelease.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobCompRelease.LargeChange = 100;
            this.KnobCompRelease.Location = new System.Drawing.Point(216, 41);
            this.KnobCompRelease.Maximum = 5000;
            this.KnobCompRelease.Minimum = 10;
            this.KnobCompRelease.Name = "KnobCompRelease";
            this.KnobCompRelease.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobCompRelease.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompRelease.PointerOffset = 4;
            this.KnobCompRelease.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompRelease.PointerWidth = 2;
            this.KnobCompRelease.Size = new System.Drawing.Size(64, 64);
            this.KnobCompRelease.TabIndex = 9;
            this.KnobCompRelease.Text = "Level";
            this.KnobCompRelease.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValCompAttack
            // 
            this.lblValCompAttack.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValCompAttack.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValCompAttack.Location = new System.Drawing.Point(146, 111);
            this.lblValCompAttack.Name = "lblValCompAttack";
            this.lblValCompAttack.Size = new System.Drawing.Size(64, 16);
            this.lblValCompAttack.TabIndex = 8;
            this.lblValCompAttack.Text = "0.0";
            this.lblValCompAttack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label27.Location = new System.Drawing.Point(146, 22);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(64, 16);
            this.label27.TabIndex = 7;
            this.label27.Text = "Attack";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompAttack
            // 
            this.KnobCompAttack.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobCompAttack.BorderWidth = 2;
            this.KnobCompAttack.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobCompAttack.HasTicks = true;
            this.KnobCompAttack.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobCompAttack.LargeChange = 100;
            this.KnobCompAttack.Location = new System.Drawing.Point(146, 41);
            this.KnobCompAttack.Maximum = 5000;
            this.KnobCompAttack.Minimum = 1;
            this.KnobCompAttack.Name = "KnobCompAttack";
            this.KnobCompAttack.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobCompAttack.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompAttack.PointerOffset = 4;
            this.KnobCompAttack.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompAttack.PointerWidth = 2;
            this.KnobCompAttack.Size = new System.Drawing.Size(64, 64);
            this.KnobCompAttack.TabIndex = 6;
            this.KnobCompAttack.Text = "Level";
            this.KnobCompAttack.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValCompRatio
            // 
            this.lblValCompRatio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValCompRatio.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValCompRatio.Location = new System.Drawing.Point(76, 111);
            this.lblValCompRatio.Name = "lblValCompRatio";
            this.lblValCompRatio.Size = new System.Drawing.Size(64, 16);
            this.lblValCompRatio.TabIndex = 5;
            this.lblValCompRatio.Text = "0.0";
            this.lblValCompRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label29.Location = new System.Drawing.Point(76, 22);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(64, 16);
            this.label29.TabIndex = 4;
            this.label29.Text = "Ratio";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompRatio
            // 
            this.KnobCompRatio.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobCompRatio.BorderWidth = 2;
            this.KnobCompRatio.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobCompRatio.HasTicks = true;
            this.KnobCompRatio.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobCompRatio.LargeChange = 1;
            this.KnobCompRatio.Location = new System.Drawing.Point(76, 41);
            this.KnobCompRatio.Maximum = 50;
            this.KnobCompRatio.Minimum = 1;
            this.KnobCompRatio.Name = "KnobCompRatio";
            this.KnobCompRatio.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobCompRatio.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompRatio.PointerOffset = 4;
            this.KnobCompRatio.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompRatio.PointerWidth = 2;
            this.KnobCompRatio.Size = new System.Drawing.Size(64, 64);
            this.KnobCompRatio.TabIndex = 3;
            this.KnobCompRatio.Text = "Level";
            this.KnobCompRatio.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // lblValCompThreshold
            // 
            this.lblValCompThreshold.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblValCompThreshold.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValCompThreshold.Location = new System.Drawing.Point(6, 111);
            this.lblValCompThreshold.Name = "lblValCompThreshold";
            this.lblValCompThreshold.Size = new System.Drawing.Size(64, 16);
            this.lblValCompThreshold.TabIndex = 2;
            this.lblValCompThreshold.Text = "0.0";
            this.lblValCompThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label31.Location = new System.Drawing.Point(6, 22);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(64, 16);
            this.label31.TabIndex = 2;
            this.label31.Text = "Threshold";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompThreshold
            // 
            this.KnobCompThreshold.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.KnobCompThreshold.BorderWidth = 2;
            this.KnobCompThreshold.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KnobCompThreshold.HasTicks = true;
            this.KnobCompThreshold.KnobColor = System.Drawing.SystemColors.Control;
            this.KnobCompThreshold.LargeChange = 1;
            this.KnobCompThreshold.Location = new System.Drawing.Point(6, 41);
            this.KnobCompThreshold.Maximum = 0;
            this.KnobCompThreshold.Minimum = -60;
            this.KnobCompThreshold.Name = "KnobCompThreshold";
            this.KnobCompThreshold.PointerColor = System.Drawing.SystemColors.ControlText;
            this.KnobCompThreshold.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompThreshold.PointerOffset = 4;
            this.KnobCompThreshold.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.KnobCompThreshold.PointerWidth = 2;
            this.KnobCompThreshold.Size = new System.Drawing.Size(64, 64);
            this.KnobCompThreshold.TabIndex = 1;
            this.KnobCompThreshold.Text = "Level";
            this.KnobCompThreshold.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckCompressor
            // 
            this.CheckCompressor.AutoSize = true;
            this.CheckCompressor.BackColor = System.Drawing.SystemColors.Control;
            this.CheckCompressor.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckCompressor.Location = new System.Drawing.Point(6, 0);
            this.CheckCompressor.Name = "CheckCompressor";
            this.CheckCompressor.Size = new System.Drawing.Size(88, 19);
            this.CheckCompressor.TabIndex = 0;
            this.CheckCompressor.Text = "Compressor";
            this.CheckCompressor.UseVisualStyleBackColor = false;
            this.CheckCompressor.CheckedChanged += new System.EventHandler(this.CheckCompressor_CheckedChanged);
            // 
            // tabReverb
            // 
            this.tabReverb.BackColor = System.Drawing.Color.Transparent;
            this.tabReverb.Controls.Add(this.groupBox1);
            this.tabReverb.Location = new System.Drawing.Point(4, 24);
            this.tabReverb.Name = "tabReverb";
            this.tabReverb.Size = new System.Drawing.Size(574, 391);
            this.tabReverb.TabIndex = 7;
            this.tabReverb.Text = "Reverb";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox13);
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Controls.Add(this.knob13);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.knob12);
            this.groupBox1.Controls.Add(this.textBox11);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.knob11);
            this.groupBox1.Controls.Add(this.textBox10);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.knob10);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.knob9);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.knob8);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.knob7);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.knob6);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.knob1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.knob2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.knob3);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.knob4);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.knob5);
            this.groupBox1.Controls.Add(this.CheckReverb);
            this.groupBox1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 256);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // textBox13
            // 
            this.textBox13.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox13.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox13.Location = new System.Drawing.Point(426, 222);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(64, 16);
            this.textBox13.TabIndex = 38;
            this.textBox13.Text = "0.0";
            this.textBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label34.Location = new System.Drawing.Point(426, 133);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(64, 16);
            this.label34.TabIndex = 37;
            this.label34.Text = "DryLevel";
            this.label34.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob13
            // 
            this.knob13.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob13.BorderWidth = 2;
            this.knob13.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob13.HasTicks = true;
            this.knob13.KnobColor = System.Drawing.SystemColors.Control;
            this.knob13.LargeChange = 1;
            this.knob13.Location = new System.Drawing.Point(426, 152);
            this.knob13.Maximum = 30;
            this.knob13.Minimum = -30;
            this.knob13.Name = "knob13";
            this.knob13.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob13.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob13.PointerOffset = 4;
            this.knob13.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob13.PointerWidth = 2;
            this.knob13.Size = new System.Drawing.Size(64, 64);
            this.knob13.TabIndex = 36;
            this.knob13.Text = "Level";
            this.knob13.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox12
            // 
            this.textBox12.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox12.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox12.Location = new System.Drawing.Point(356, 222);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(64, 16);
            this.textBox12.TabIndex = 35;
            this.textBox12.Text = "0.0";
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label32.Location = new System.Drawing.Point(356, 133);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(64, 16);
            this.label32.TabIndex = 34;
            this.label32.Text = "WetLevel";
            this.label32.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob12
            // 
            this.knob12.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob12.BorderWidth = 2;
            this.knob12.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob12.HasTicks = true;
            this.knob12.KnobColor = System.Drawing.SystemColors.Control;
            this.knob12.LargeChange = 1;
            this.knob12.Location = new System.Drawing.Point(356, 152);
            this.knob12.Maximum = 30;
            this.knob12.Minimum = -30;
            this.knob12.Name = "knob12";
            this.knob12.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob12.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob12.PointerOffset = 4;
            this.knob12.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob12.PointerWidth = 2;
            this.knob12.Size = new System.Drawing.Size(64, 64);
            this.knob12.TabIndex = 33;
            this.knob12.Text = "Level";
            this.knob12.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox11
            // 
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox11.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox11.Location = new System.Drawing.Point(286, 222);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(64, 16);
            this.textBox11.TabIndex = 32;
            this.textBox11.Text = "0.0";
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label30.Location = new System.Drawing.Point(286, 133);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(64, 16);
            this.label30.TabIndex = 31;
            this.label30.Text = "EarlyLateMix";
            this.label30.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob11
            // 
            this.knob11.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob11.BorderWidth = 2;
            this.knob11.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob11.HasTicks = true;
            this.knob11.KnobColor = System.Drawing.SystemColors.Control;
            this.knob11.LargeChange = 1;
            this.knob11.Location = new System.Drawing.Point(286, 152);
            this.knob11.Maximum = 30;
            this.knob11.Minimum = -30;
            this.knob11.Name = "knob11";
            this.knob11.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob11.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob11.PointerOffset = 4;
            this.knob11.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob11.PointerWidth = 2;
            this.knob11.Size = new System.Drawing.Size(64, 64);
            this.knob11.TabIndex = 30;
            this.knob11.Text = "Level";
            this.knob11.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox10
            // 
            this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox10.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox10.Location = new System.Drawing.Point(216, 222);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(64, 16);
            this.textBox10.TabIndex = 29;
            this.textBox10.Text = "0.0";
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label28.Location = new System.Drawing.Point(216, 133);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(64, 16);
            this.label28.TabIndex = 28;
            this.label28.Text = "HighCut";
            this.label28.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob10
            // 
            this.knob10.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob10.BorderWidth = 2;
            this.knob10.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob10.HasTicks = true;
            this.knob10.KnobColor = System.Drawing.SystemColors.Control;
            this.knob10.LargeChange = 1;
            this.knob10.Location = new System.Drawing.Point(216, 152);
            this.knob10.Maximum = 30;
            this.knob10.Minimum = -30;
            this.knob10.Name = "knob10";
            this.knob10.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob10.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob10.PointerOffset = 4;
            this.knob10.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob10.PointerWidth = 2;
            this.knob10.Size = new System.Drawing.Size(64, 64);
            this.knob10.TabIndex = 27;
            this.knob10.Text = "Level";
            this.knob10.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox9
            // 
            this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox9.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox9.Location = new System.Drawing.Point(146, 222);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(64, 16);
            this.textBox9.TabIndex = 26;
            this.textBox9.Text = "0.0";
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label26.Location = new System.Drawing.Point(146, 133);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(64, 16);
            this.label26.TabIndex = 25;
            this.label26.Text = "LwShlfGain";
            this.label26.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob9
            // 
            this.knob9.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob9.BorderWidth = 2;
            this.knob9.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob9.HasTicks = true;
            this.knob9.KnobColor = System.Drawing.SystemColors.Control;
            this.knob9.LargeChange = 1;
            this.knob9.Location = new System.Drawing.Point(146, 152);
            this.knob9.Maximum = 30;
            this.knob9.Minimum = -30;
            this.knob9.Name = "knob9";
            this.knob9.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob9.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob9.PointerOffset = 4;
            this.knob9.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob9.PointerWidth = 2;
            this.knob9.Size = new System.Drawing.Size(64, 64);
            this.knob9.TabIndex = 24;
            this.knob9.Text = "Level";
            this.knob9.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox8
            // 
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox8.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox8.Location = new System.Drawing.Point(76, 222);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(64, 16);
            this.textBox8.TabIndex = 23;
            this.textBox8.Text = "0.0";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label24.Location = new System.Drawing.Point(76, 133);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 16);
            this.label24.TabIndex = 22;
            this.label24.Text = "LowShlfFq";
            this.label24.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob8
            // 
            this.knob8.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob8.BorderWidth = 2;
            this.knob8.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob8.HasTicks = true;
            this.knob8.KnobColor = System.Drawing.SystemColors.Control;
            this.knob8.LargeChange = 1;
            this.knob8.Location = new System.Drawing.Point(76, 152);
            this.knob8.Maximum = 30;
            this.knob8.Minimum = -30;
            this.knob8.Name = "knob8";
            this.knob8.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob8.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob8.PointerOffset = 4;
            this.knob8.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob8.PointerWidth = 2;
            this.knob8.Size = new System.Drawing.Size(64, 64);
            this.knob8.TabIndex = 21;
            this.knob8.Text = "Level";
            this.knob8.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox7
            // 
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox7.Location = new System.Drawing.Point(6, 222);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(64, 16);
            this.textBox7.TabIndex = 20;
            this.textBox7.Text = "0.0";
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label22.Location = new System.Drawing.Point(6, 133);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 16);
            this.label22.TabIndex = 19;
            this.label22.Text = "Diffusion";
            this.label22.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob7
            // 
            this.knob7.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob7.BorderWidth = 2;
            this.knob7.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob7.HasTicks = true;
            this.knob7.KnobColor = System.Drawing.SystemColors.Control;
            this.knob7.LargeChange = 1;
            this.knob7.Location = new System.Drawing.Point(6, 152);
            this.knob7.Maximum = 30;
            this.knob7.Minimum = -30;
            this.knob7.Name = "knob7";
            this.knob7.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob7.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob7.PointerOffset = 4;
            this.knob7.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob7.PointerWidth = 2;
            this.knob7.Size = new System.Drawing.Size(64, 64);
            this.knob7.TabIndex = 18;
            this.knob7.Text = "Level";
            this.knob7.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox6.Location = new System.Drawing.Point(356, 114);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(64, 16);
            this.textBox6.TabIndex = 17;
            this.textBox6.Text = "0.0";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label20.Location = new System.Drawing.Point(356, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 16);
            this.label20.TabIndex = 16;
            this.label20.Text = "Diffusion";
            this.label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob6
            // 
            this.knob6.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob6.BorderWidth = 2;
            this.knob6.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob6.HasTicks = true;
            this.knob6.KnobColor = System.Drawing.SystemColors.Control;
            this.knob6.LargeChange = 1;
            this.knob6.Location = new System.Drawing.Point(356, 44);
            this.knob6.Maximum = 30;
            this.knob6.Minimum = -30;
            this.knob6.Name = "knob6";
            this.knob6.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob6.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob6.PointerOffset = 4;
            this.knob6.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob6.PointerWidth = 2;
            this.knob6.Size = new System.Drawing.Size(64, 64);
            this.knob6.TabIndex = 15;
            this.knob6.Text = "Level";
            this.knob6.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(286, 114);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(64, 16);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "0.0";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(286, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 16);
            this.label10.TabIndex = 13;
            this.label10.Text = "HFDcRatio";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob1
            // 
            this.knob1.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob1.BorderWidth = 2;
            this.knob1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob1.HasTicks = true;
            this.knob1.KnobColor = System.Drawing.SystemColors.Control;
            this.knob1.LargeChange = 1;
            this.knob1.Location = new System.Drawing.Point(286, 44);
            this.knob1.Maximum = 30;
            this.knob1.Minimum = -30;
            this.knob1.Name = "knob1";
            this.knob1.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob1.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob1.PointerOffset = 4;
            this.knob1.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob1.PointerWidth = 2;
            this.knob1.Size = new System.Drawing.Size(64, 64);
            this.knob1.TabIndex = 12;
            this.knob1.Text = "Level";
            this.knob1.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox2.Location = new System.Drawing.Point(216, 114);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(64, 16);
            this.textBox2.TabIndex = 11;
            this.textBox2.Text = "0.0";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(216, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 16);
            this.label12.TabIndex = 10;
            this.label12.Text = "HFRef";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob2
            // 
            this.knob2.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob2.BorderWidth = 2;
            this.knob2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob2.HasTicks = true;
            this.knob2.KnobColor = System.Drawing.SystemColors.Control;
            this.knob2.LargeChange = 100;
            this.knob2.Location = new System.Drawing.Point(216, 44);
            this.knob2.Maximum = 5000;
            this.knob2.Minimum = 10;
            this.knob2.Name = "knob2";
            this.knob2.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob2.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob2.PointerOffset = 4;
            this.knob2.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob2.PointerWidth = 2;
            this.knob2.Size = new System.Drawing.Size(64, 64);
            this.knob2.TabIndex = 9;
            this.knob2.Text = "Level";
            this.knob2.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox3.Location = new System.Drawing.Point(146, 114);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(64, 16);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = "0.0";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(146, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 16);
            this.label14.TabIndex = 7;
            this.label14.Text = "LateDelay";
            this.label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob3
            // 
            this.knob3.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob3.BorderWidth = 2;
            this.knob3.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob3.HasTicks = true;
            this.knob3.KnobColor = System.Drawing.SystemColors.Control;
            this.knob3.LargeChange = 100;
            this.knob3.Location = new System.Drawing.Point(146, 44);
            this.knob3.Maximum = 5000;
            this.knob3.Minimum = 1;
            this.knob3.Name = "knob3";
            this.knob3.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob3.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob3.PointerOffset = 4;
            this.knob3.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob3.PointerWidth = 2;
            this.knob3.Size = new System.Drawing.Size(64, 64);
            this.knob3.TabIndex = 6;
            this.knob3.Text = "Level";
            this.knob3.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox4.Location = new System.Drawing.Point(76, 114);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(64, 16);
            this.textBox4.TabIndex = 5;
            this.textBox4.Text = "0.0";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(76, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 16);
            this.label16.TabIndex = 4;
            this.label16.Text = "EarlyDelay";
            this.label16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob4
            // 
            this.knob4.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob4.BorderWidth = 2;
            this.knob4.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob4.HasTicks = true;
            this.knob4.KnobColor = System.Drawing.SystemColors.Control;
            this.knob4.LargeChange = 1;
            this.knob4.Location = new System.Drawing.Point(76, 44);
            this.knob4.Maximum = 50;
            this.knob4.Minimum = 1;
            this.knob4.Name = "knob4";
            this.knob4.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob4.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob4.PointerOffset = 4;
            this.knob4.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob4.PointerWidth = 2;
            this.knob4.Size = new System.Drawing.Size(64, 64);
            this.knob4.TabIndex = 3;
            this.knob4.Text = "Level";
            this.knob4.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox5.Location = new System.Drawing.Point(6, 114);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(64, 16);
            this.textBox5.TabIndex = 2;
            this.textBox5.Text = "0.0";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label18.Location = new System.Drawing.Point(6, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 16);
            this.label18.TabIndex = 2;
            this.label18.Text = "DecayTime";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // knob5
            // 
            this.knob5.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.knob5.BorderWidth = 2;
            this.knob5.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.knob5.HasTicks = true;
            this.knob5.KnobColor = System.Drawing.SystemColors.Control;
            this.knob5.LargeChange = 1;
            this.knob5.Location = new System.Drawing.Point(6, 44);
            this.knob5.Maximum = 0;
            this.knob5.Minimum = -60;
            this.knob5.Name = "knob5";
            this.knob5.PointerColor = System.Drawing.SystemColors.ControlText;
            this.knob5.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob5.PointerOffset = 4;
            this.knob5.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            this.knob5.PointerWidth = 2;
            this.knob5.Size = new System.Drawing.Size(64, 64);
            this.knob5.TabIndex = 1;
            this.knob5.Text = "Level";
            this.knob5.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            // 
            // CheckReverb
            // 
            this.CheckReverb.AutoSize = true;
            this.CheckReverb.BackColor = System.Drawing.SystemColors.Control;
            this.CheckReverb.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckReverb.Location = new System.Drawing.Point(6, 2);
            this.CheckReverb.Name = "CheckReverb";
            this.CheckReverb.Size = new System.Drawing.Size(62, 19);
            this.CheckReverb.TabIndex = 0;
            this.CheckReverb.Text = "Reverb";
            this.CheckReverb.UseVisualStyleBackColor = false;
            this.CheckReverb.CheckedChanged += new System.EventHandler(this.CheckReverb_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 419);
            this.Controls.Add(this.tabControlEffects);
            this.Controls.Add(this.TreeMenu);
            this.DoubleBuffered = true;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tabControlEffects.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.tabSetting.PerformLayout();
            this.tabGEqualizer.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictGEQGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ125)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ250)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ500)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ1K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ2K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ4K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ8K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ16K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ20K)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrkGEQ22K)).EndInit();
            this.tabPitch.ResumeLayout(false);
            this.GroupSpeed.ResumeLayout(false);
            this.GroupSpeed.PerformLayout();
            this.GroupFrequency.ResumeLayout(false);
            this.GroupFrequency.PerformLayout();
            this.GroupPitchShift.ResumeLayout(false);
            this.GroupPitchShift.PerformLayout();
            this.tabDistortion.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabChorus.ResumeLayout(false);
            this.GroupChorus.ResumeLayout(false);
            this.GroupChorus.PerformLayout();
            this.tabEcho.ResumeLayout(false);
            this.GroupEcho.ResumeLayout(false);
            this.GroupEcho.PerformLayout();
            this.tabFlanger.ResumeLayout(false);
            this.GroupFlanger.ResumeLayout(false);
            this.GroupFlanger.PerformLayout();
            this.tabHightpass.ResumeLayout(false);
            this.GroupHighpass.ResumeLayout(false);
            this.GroupHighpass.PerformLayout();
            this.tabLowpass.ResumeLayout(false);
            this.GroupLowpass.ResumeLayout(false);
            this.GroupLowpass.PerformLayout();
            this.tabCompressor.ResumeLayout(false);
            this.GroupCompressor.ResumeLayout(false);
            this.GroupCompressor.PerformLayout();
            this.tabReverb.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView TreeMenu;
        private System.Windows.Forms.TabControl tabControlEffects;
        private System.Windows.Forms.TabPage tabDistortion;
        private System.Windows.Forms.TabPage tabChorus;
        private System.Windows.Forms.GroupBox GroupChorus;
        private System.Windows.Forms.TextBox lblValChorusDepth;
        private System.Windows.Forms.Label label7;
        private UI.Knob KnobChorusDepth;
        private System.Windows.Forms.TextBox lblValChorusRate;
        private System.Windows.Forms.Label label5;
        private UI.Knob KnobChorusRate;
        private System.Windows.Forms.TextBox lblValChorusMix;
        private System.Windows.Forms.Label label3;
        private UI.Knob KnobChorusMix;
        private System.Windows.Forms.CheckBox CheckChorus;
        private System.Windows.Forms.TabPage tabEcho;
        private System.Windows.Forms.TabPage tabFlanger;
        private System.Windows.Forms.TabPage tabHightpass;
        private System.Windows.Forms.TabPage tabLowpass;
        private System.Windows.Forms.TabPage tabCompressor;
        private System.Windows.Forms.TabPage tabReverb;
        private System.Windows.Forms.TabPage tabPitch;
        private System.Windows.Forms.GroupBox GroupSpeed;
        private System.Windows.Forms.TextBox lblValSpeed;
        private System.Windows.Forms.Label label6;
        private UI.Knob KnobSpeed;
        private System.Windows.Forms.CheckBox CheckSpeed;
        private System.Windows.Forms.GroupBox GroupFrequency;
        private System.Windows.Forms.TextBox lblValFrequency;
        private System.Windows.Forms.Label label8;
        private UI.Knob KnobFrequency;
        private System.Windows.Forms.CheckBox CheckFrequency;
        private System.Windows.Forms.GroupBox GroupPitchShift;
        private System.Windows.Forms.TextBox lblValPitchFFT;
        private System.Windows.Forms.Label label2;
        private UI.Knob KnobPitchFFT;
        private System.Windows.Forms.TextBox lblValPitchPitch;
        private System.Windows.Forms.Label label4;
        private UI.Knob KnobPitchPitch;
        private System.Windows.Forms.CheckBox CheckPitch;
        private System.Windows.Forms.GroupBox GroupEcho;
        private System.Windows.Forms.TextBox lblValEchoWet;
        private System.Windows.Forms.Label label15;
        private UI.Knob KnobEchoWet;
        private System.Windows.Forms.TextBox lblValEchoDry;
        private System.Windows.Forms.Label label9;
        private UI.Knob KnobEchoDry;
        private System.Windows.Forms.TextBox lblValEchoFeedback;
        private System.Windows.Forms.Label label11;
        private UI.Knob KnobEchoFeedback;
        private System.Windows.Forms.TextBox lblValEchoDelay;
        private System.Windows.Forms.Label label13;
        private UI.Knob KnobEchoDelay;
        private System.Windows.Forms.CheckBox CheckEcho;
        private System.Windows.Forms.GroupBox GroupFlanger;
        private System.Windows.Forms.TextBox lblValFlangerDepth;
        private System.Windows.Forms.Label label33;
        private UI.Knob KnobFlangerDepth;
        private System.Windows.Forms.TextBox lblValFlangerRate;
        private System.Windows.Forms.Label label35;
        private UI.Knob KnobFlangerRate;
        private System.Windows.Forms.TextBox lblValFlangerMix;
        private System.Windows.Forms.Label label37;
        private UI.Knob KnobFlangerMix;
        private System.Windows.Forms.CheckBox CheckFlanger;
        private System.Windows.Forms.GroupBox GroupHighpass;
        private System.Windows.Forms.TextBox lblValHighpassResonance;
        private System.Windows.Forms.Label label21;
        private UI.Knob KnobHighpassResonance;
        private System.Windows.Forms.TextBox lblValHighpassCutoff;
        private System.Windows.Forms.Label label23;
        private UI.Knob KnobHighpassCutoff;
        private System.Windows.Forms.CheckBox CheckHighpass;
        private System.Windows.Forms.GroupBox GroupLowpass;
        private System.Windows.Forms.TextBox lblValLowpassResonance;
        private System.Windows.Forms.Label label17;
        private UI.Knob KnobLowpassResonance;
        private System.Windows.Forms.TextBox lblValLowpassCutoff;
        private System.Windows.Forms.Label label19;
        private UI.Knob KnobLowpassCutoff;
        private System.Windows.Forms.CheckBox CheckLowpass;
        private System.Windows.Forms.GroupBox GroupCompressor;
        private System.Windows.Forms.CheckBox CheckCompLinked;
        private System.Windows.Forms.TextBox lblValCompGain;
        private System.Windows.Forms.Label label39;
        private UI.Knob KnobCompGain;
        private System.Windows.Forms.TextBox lblValCompRelease;
        private System.Windows.Forms.Label label25;
        private UI.Knob KnobCompRelease;
        private System.Windows.Forms.TextBox lblValCompAttack;
        private System.Windows.Forms.Label label27;
        private UI.Knob KnobCompAttack;
        private System.Windows.Forms.TextBox lblValCompRatio;
        private System.Windows.Forms.Label label29;
        private UI.Knob KnobCompRatio;
        private System.Windows.Forms.TextBox lblValCompThreshold;
        private System.Windows.Forms.Label label31;
        private UI.Knob KnobCompThreshold;
        private System.Windows.Forms.CheckBox CheckCompressor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Label label34;
        private UI.Knob knob13;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label32;
        private UI.Knob knob12;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label30;
        private UI.Knob knob11;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label28;
        private UI.Knob knob10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label26;
        private UI.Knob knob9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label24;
        private UI.Knob knob8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label22;
        private UI.Knob knob7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label20;
        private UI.Knob knob6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private UI.Knob knob1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label12;
        private UI.Knob knob2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label14;
        private UI.Knob knob3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label16;
        private UI.Knob knob4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label18;
        private UI.Knob knob5;
        private System.Windows.Forms.CheckBox CheckReverb;
        private System.Windows.Forms.TabPage tabGEqualizer;
        private System.Windows.Forms.TextBox lblValDistortionLevel;
        private System.Windows.Forms.Label label1;
        private UI.Knob KnobDistortionLevel;
        private System.Windows.Forms.CheckBox CheckDistortion;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.ComboBox cmbSpeaker;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.ComboBox cmbSampling;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox cmbFormat;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.ComboBox cmbSampleRate;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox cmbOutput;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.PictureBox PictGEQGraph;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox CheckGEQ;
        private System.Windows.Forms.ComboBox cmbEqPreset;
        private System.Windows.Forms.Label label44;
        private ColorSlider.ColorSlider TrkGEQ32;
        private ColorSlider.ColorSlider TrkGEQ22K;
        private ColorSlider.ColorSlider TrkGEQ20K;
        private ColorSlider.ColorSlider TrkGEQ60;
        private ColorSlider.ColorSlider TrkGEQ125;
        private ColorSlider.ColorSlider TrkGEQ250;
        private ColorSlider.ColorSlider TrkGEQ500;
        private ColorSlider.ColorSlider TrkGEQ1K;
        private ColorSlider.ColorSlider TrkGEQ2K;
        private ColorSlider.ColorSlider TrkGEQ4K;
        private ColorSlider.ColorSlider TrkGEQ8K;
        private ColorSlider.ColorSlider TrkGEQ16K;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
    }
}