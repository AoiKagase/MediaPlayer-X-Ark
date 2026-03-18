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
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("出力設定");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Graphic Equalizer");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Pitch / Freq / Speed");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Distortion");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Chorus");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Echo");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Flanger");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Highpass");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Lowpass");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Compressor");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Reverb");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("エフェクト", new System.Windows.Forms.TreeNode[] { treeNode17, treeNode18, treeNode19, treeNode20, treeNode21, treeNode22, treeNode23, treeNode24, treeNode25, treeNode26 });
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("スキン");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("About");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("その他", new System.Windows.Forms.TreeNode[] { treeNode29 });
            TreeMenu = new System.Windows.Forms.TreeView();
            tabControlEffects = new System.Windows.Forms.TabControl();
            tabSetting = new System.Windows.Forms.TabPage();
            lblOutputNote = new System.Windows.Forms.Label();
            BtnUpdate = new System.Windows.Forms.Button();
            cmbDevice = new System.Windows.Forms.ComboBox();
            label38 = new System.Windows.Forms.Label();
            cmbOutput = new System.Windows.Forms.ComboBox();
            label36 = new System.Windows.Forms.Label();
            tabGEqualizer = new System.Windows.Forms.TabPage();
            GroupGEQ = new System.Windows.Forms.GroupBox();
            label56 = new System.Windows.Forms.Label();
            label55 = new System.Windows.Forms.Label();
            label54 = new System.Windows.Forms.Label();
            label53 = new System.Windows.Forms.Label();
            label52 = new System.Windows.Forms.Label();
            label51 = new System.Windows.Forms.Label();
            label50 = new System.Windows.Forms.Label();
            label49 = new System.Windows.Forms.Label();
            label48 = new System.Windows.Forms.Label();
            label47 = new System.Windows.Forms.Label();
            label46 = new System.Windows.Forms.Label();
            label45 = new System.Windows.Forms.Label();
            label44 = new System.Windows.Forms.Label();
            cmbEqPreset = new System.Windows.Forms.ComboBox();
            CheckGEQ = new System.Windows.Forms.CheckBox();
            PictGEQGraph = new System.Windows.Forms.PictureBox();
            TrkGEQ60 = new ColorSlider.ColorSlider();
            TrkGEQ32 = new ColorSlider.ColorSlider();
            TrkGEQ125 = new ColorSlider.ColorSlider();
            TrkGEQ250 = new ColorSlider.ColorSlider();
            TrkGEQ500 = new ColorSlider.ColorSlider();
            TrkGEQ1K = new ColorSlider.ColorSlider();
            TrkGEQ2K = new ColorSlider.ColorSlider();
            TrkGEQ4K = new ColorSlider.ColorSlider();
            TrkGEQ8K = new ColorSlider.ColorSlider();
            TrkGEQ16K = new ColorSlider.ColorSlider();
            TrkGEQ20K = new ColorSlider.ColorSlider();
            TrkGEQ22K = new ColorSlider.ColorSlider();
            btnGEQPresetSave = new System.Windows.Forms.Button();
            btnGEQPresetDelete = new System.Windows.Forms.Button();
            tabPitch = new System.Windows.Forms.TabPage();
            GroupSpeed = new System.Windows.Forms.GroupBox();
            lblValSpeed = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            KnobSpeed = new UI.Knob();
            CheckSpeed = new System.Windows.Forms.CheckBox();
            GroupFrequency = new System.Windows.Forms.GroupBox();
            lblValFrequency = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            KnobFrequency = new UI.Knob();
            CheckFrequency = new System.Windows.Forms.CheckBox();
            GroupPitchShift = new System.Windows.Forms.GroupBox();
            lblValPitchFFT = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            KnobPitchFFT = new UI.Knob();
            lblValPitchPitch = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            KnobPitchPitch = new UI.Knob();
            CheckPitch = new System.Windows.Forms.CheckBox();
            cmbPitchPreset = new System.Windows.Forms.ComboBox();
            btnPitchPresetSave = new System.Windows.Forms.Button();
            btnPitchPresetDelete = new System.Windows.Forms.Button();
            tabDistortion = new System.Windows.Forms.TabPage();
            GroupDistortion = new System.Windows.Forms.GroupBox();
            CheckDistortion = new System.Windows.Forms.CheckBox();
            lblValDistortionLevel = new System.Windows.Forms.TextBox();
            KnobDistortionLevel = new UI.Knob();
            label1 = new System.Windows.Forms.Label();
            cmbDistortionPreset = new System.Windows.Forms.ComboBox();
            btnDistortionPresetSave = new System.Windows.Forms.Button();
            btnDistortionPresetDelete = new System.Windows.Forms.Button();
            tabChorus = new System.Windows.Forms.TabPage();
            GroupChorus = new System.Windows.Forms.GroupBox();
            lblValChorusDepth = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            KnobChorusDepth = new UI.Knob();
            lblValChorusRate = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            KnobChorusRate = new UI.Knob();
            lblValChorusMix = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            KnobChorusMix = new UI.Knob();
            CheckChorus = new System.Windows.Forms.CheckBox();
            cmbChorusPreset = new System.Windows.Forms.ComboBox();
            btnChorusPresetSave = new System.Windows.Forms.Button();
            btnChorusPresetDelete = new System.Windows.Forms.Button();
            tabEcho = new System.Windows.Forms.TabPage();
            GroupEcho = new System.Windows.Forms.GroupBox();
            lblValEchoWet = new System.Windows.Forms.TextBox();
            label15 = new System.Windows.Forms.Label();
            KnobEchoWet = new UI.Knob();
            lblValEchoDry = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            KnobEchoDry = new UI.Knob();
            lblValEchoFeedback = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            KnobEchoFeedback = new UI.Knob();
            lblValEchoDelay = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            KnobEchoDelay = new UI.Knob();
            CheckEcho = new System.Windows.Forms.CheckBox();
            cmbEchoPreset = new System.Windows.Forms.ComboBox();
            btnEchoPresetSave = new System.Windows.Forms.Button();
            btnEchoPresetDelete = new System.Windows.Forms.Button();
            tabFlanger = new System.Windows.Forms.TabPage();
            GroupFlanger = new System.Windows.Forms.GroupBox();
            lblValFlangerDepth = new System.Windows.Forms.TextBox();
            label33 = new System.Windows.Forms.Label();
            KnobFlangerDepth = new UI.Knob();
            lblValFlangerRate = new System.Windows.Forms.TextBox();
            label35 = new System.Windows.Forms.Label();
            KnobFlangerRate = new UI.Knob();
            lblValFlangerMix = new System.Windows.Forms.TextBox();
            label37 = new System.Windows.Forms.Label();
            KnobFlangerMix = new UI.Knob();
            CheckFlanger = new System.Windows.Forms.CheckBox();
            cmbFlangerPreset = new System.Windows.Forms.ComboBox();
            btnFlangerPresetSave = new System.Windows.Forms.Button();
            btnFlangerPresetDelete = new System.Windows.Forms.Button();
            tabHightpass = new System.Windows.Forms.TabPage();
            GroupHighpass = new System.Windows.Forms.GroupBox();
            lblValHighpassResonance = new System.Windows.Forms.TextBox();
            label21 = new System.Windows.Forms.Label();
            KnobHighpassResonance = new UI.Knob();
            lblValHighpassCutoff = new System.Windows.Forms.TextBox();
            label23 = new System.Windows.Forms.Label();
            KnobHighpassCutoff = new UI.Knob();
            CheckHighpass = new System.Windows.Forms.CheckBox();
            cmbHighpassPreset = new System.Windows.Forms.ComboBox();
            btnHighpassPresetSave = new System.Windows.Forms.Button();
            btnHighpassPresetDelete = new System.Windows.Forms.Button();
            tabLowpass = new System.Windows.Forms.TabPage();
            GroupLowpass = new System.Windows.Forms.GroupBox();
            lblValLowpassResonance = new System.Windows.Forms.TextBox();
            label17 = new System.Windows.Forms.Label();
            KnobLowpassResonance = new UI.Knob();
            lblValLowpassCutoff = new System.Windows.Forms.TextBox();
            label19 = new System.Windows.Forms.Label();
            KnobLowpassCutoff = new UI.Knob();
            CheckLowpass = new System.Windows.Forms.CheckBox();
            cmbLowpassPreset = new System.Windows.Forms.ComboBox();
            btnLowpassPresetSave = new System.Windows.Forms.Button();
            btnLowpassPresetDelete = new System.Windows.Forms.Button();
            tabCompressor = new System.Windows.Forms.TabPage();
            GroupCompressor = new System.Windows.Forms.GroupBox();
            CheckCompLinked = new System.Windows.Forms.CheckBox();
            lblValCompGain = new System.Windows.Forms.TextBox();
            label39 = new System.Windows.Forms.Label();
            KnobCompGain = new UI.Knob();
            lblValCompRelease = new System.Windows.Forms.TextBox();
            label25 = new System.Windows.Forms.Label();
            KnobCompRelease = new UI.Knob();
            lblValCompAttack = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            KnobCompAttack = new UI.Knob();
            lblValCompRatio = new System.Windows.Forms.TextBox();
            label29 = new System.Windows.Forms.Label();
            KnobCompRatio = new UI.Knob();
            lblValCompThreshold = new System.Windows.Forms.TextBox();
            label31 = new System.Windows.Forms.Label();
            KnobCompThreshold = new UI.Knob();
            CheckCompressor = new System.Windows.Forms.CheckBox();
            cmbCompressorPreset = new System.Windows.Forms.ComboBox();
            btnCompressorPresetSave = new System.Windows.Forms.Button();
            btnCompressorPresetDelete = new System.Windows.Forms.Button();
            tabReverb = new System.Windows.Forms.TabPage();
            GroupReverb = new System.Windows.Forms.GroupBox();
            CheckReverb = new System.Windows.Forms.CheckBox();
            lblReverbDecayTime = new System.Windows.Forms.Label();
            lblReverbEarlyDelay = new System.Windows.Forms.Label();
            lblReverbLateDelay = new System.Windows.Forms.Label();
            lblReverbHFRef = new System.Windows.Forms.Label();
            lblReverbHFDcRatio = new System.Windows.Forms.Label();
            lblReverbDiffusion = new System.Windows.Forms.Label();
            lblReverbDensity = new System.Windows.Forms.Label();
            lblValReverbDecayTime = new System.Windows.Forms.TextBox();
            lblValReverbEarlyDelay = new System.Windows.Forms.TextBox();
            lblValReverbLateDelay = new System.Windows.Forms.TextBox();
            lblValReverbHFRef = new System.Windows.Forms.TextBox();
            lblValReverbHFDcRatio = new System.Windows.Forms.TextBox();
            lblValReverbDiffusion = new System.Windows.Forms.TextBox();
            lblValReverbDensity = new System.Windows.Forms.TextBox();
            KnobReverbDecayTime = new UI.Knob();
            KnobReverbEarlyDelay = new UI.Knob();
            KnobReverbLateDelay = new UI.Knob();
            KnobReverbHFRef = new UI.Knob();
            KnobReverbHFDcRatio = new UI.Knob();
            KnobReverbDiffusion = new UI.Knob();
            KnobReverbDensity = new UI.Knob();
            lblReverbDivider = new System.Windows.Forms.Label();
            lblReverbLowShelfFreq = new System.Windows.Forms.Label();
            lblReverbLowShelfGain = new System.Windows.Forms.Label();
            lblReverbHighCut = new System.Windows.Forms.Label();
            lblReverbEarlyLate = new System.Windows.Forms.Label();
            lblReverbWet = new System.Windows.Forms.Label();
            lblReverbDry = new System.Windows.Forms.Label();
            lblValReverbLowShelfFreq = new System.Windows.Forms.TextBox();
            lblValReverbLowShelfGain = new System.Windows.Forms.TextBox();
            lblValReverbHighCut = new System.Windows.Forms.TextBox();
            lblValReverbEarlyLate = new System.Windows.Forms.TextBox();
            lblValReverbWet = new System.Windows.Forms.TextBox();
            lblValReverbDry = new System.Windows.Forms.TextBox();
            KnobReverbLowShelfFrequency = new UI.Knob();
            KnobReverbLowshelfGain = new UI.Knob();
            KnobReverbHighCut = new UI.Knob();
            KnobReverbEarlyLate = new UI.Knob();
            KnobReverbWet = new UI.Knob();
            KnobReverbDry = new UI.Knob();
            cmbReverbPreset = new System.Windows.Forms.ComboBox();
            btnReverbPresetSave = new System.Windows.Forms.Button();
            btnReverbPresetDelete = new System.Windows.Forms.Button();
            tabSkin = new System.Windows.Forms.TabPage();
            lblSkinPath = new System.Windows.Forms.Label();
            txtSkinPath = new System.Windows.Forms.TextBox();
            lblSkinName = new System.Windows.Forms.Label();
            lblSkinAuthorLabel = new System.Windows.Forms.Label();
            lblSkinAuthor = new System.Windows.Forms.Label();
            lblSkinDescLabel = new System.Windows.Forms.Label();
            lblSkinDesc = new System.Windows.Forms.Label();
            BtnSkinBrowse = new System.Windows.Forms.Button();
            PictSkinPreview = new System.Windows.Forms.PictureBox();
            BtnSkinApply = new System.Windows.Forms.Button();
            tabAbout = new System.Windows.Forms.TabPage();
            lblAboutAppName = new System.Windows.Forms.Label();
            lblAboutVersion = new System.Windows.Forms.Label();
            lblAboutCopyright = new System.Windows.Forms.Label();
            lblAboutCompany = new System.Windows.Forms.Label();
            lnkAboutGitHub = new System.Windows.Forms.LinkLabel();
            textBox13 = new System.Windows.Forms.TextBox();
            label34 = new System.Windows.Forms.Label();
            textBox12 = new System.Windows.Forms.TextBox();
            label32 = new System.Windows.Forms.Label();
            textBox11 = new System.Windows.Forms.TextBox();
            label30 = new System.Windows.Forms.Label();
            textBox10 = new System.Windows.Forms.TextBox();
            label28 = new System.Windows.Forms.Label();
            textBox9 = new System.Windows.Forms.TextBox();
            label26 = new System.Windows.Forms.Label();
            textBox8 = new System.Windows.Forms.TextBox();
            label24 = new System.Windows.Forms.Label();
            textBox7 = new System.Windows.Forms.TextBox();
            label22 = new System.Windows.Forms.Label();
            textBox6 = new System.Windows.Forms.TextBox();
            label20 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            textBox3 = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            textBox4 = new System.Windows.Forms.TextBox();
            label16 = new System.Windows.Forms.Label();
            textBox5 = new System.Windows.Forms.TextBox();
            label18 = new System.Windows.Forms.Label();
            tabControlEffects.SuspendLayout();
            tabSetting.SuspendLayout();
            tabGEqualizer.SuspendLayout();
            GroupGEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictGEQGraph).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ60).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ32).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ125).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ250).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ500).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ1K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ2K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ4K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ8K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ16K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ20K).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ22K).BeginInit();
            tabPitch.SuspendLayout();
            GroupSpeed.SuspendLayout();
            GroupFrequency.SuspendLayout();
            GroupPitchShift.SuspendLayout();
            tabDistortion.SuspendLayout();
            GroupDistortion.SuspendLayout();
            tabChorus.SuspendLayout();
            GroupChorus.SuspendLayout();
            tabEcho.SuspendLayout();
            GroupEcho.SuspendLayout();
            tabFlanger.SuspendLayout();
            GroupFlanger.SuspendLayout();
            tabHightpass.SuspendLayout();
            GroupHighpass.SuspendLayout();
            tabLowpass.SuspendLayout();
            GroupLowpass.SuspendLayout();
            tabCompressor.SuspendLayout();
            GroupCompressor.SuspendLayout();
            tabReverb.SuspendLayout();
            GroupReverb.SuspendLayout();
            tabSkin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictSkinPreview).BeginInit();
            tabAbout.SuspendLayout();
            SuspendLayout();
            // 
            // TreeMenu
            // 
            TreeMenu.Dock = System.Windows.Forms.DockStyle.Left;
            TreeMenu.Location = new System.Drawing.Point(0, 0);
            TreeMenu.Name = "TreeMenu";
            treeNode16.Name = "OUTPUT";
            treeNode16.Text = "出力設定";
            treeNode17.Name = "GEQ";
            treeNode17.Text = "Graphic Equalizer";
            treeNode18.Name = "PITCH";
            treeNode18.Text = "Pitch / Freq / Speed";
            treeNode19.Name = "DISTORTION";
            treeNode19.Text = "Distortion";
            treeNode20.Name = "CHORUS";
            treeNode20.Text = "Chorus";
            treeNode21.Name = "ECHO";
            treeNode21.Text = "Echo";
            treeNode22.Name = "FLANGER";
            treeNode22.Text = "Flanger";
            treeNode23.Name = "HIGHPASS";
            treeNode23.Text = "Highpass";
            treeNode24.Name = "LOWPASS";
            treeNode24.Text = "Lowpass";
            treeNode25.Name = "COMPRESSOR";
            treeNode25.Text = "Compressor";
            treeNode26.Name = "REVERB";
            treeNode26.Text = "Reverb";
            treeNode27.Name = "EFFECTS";
            treeNode27.Text = "エフェクト";
            treeNode28.Name = "SKIN";
            treeNode28.Text = "スキン";
            treeNode29.Name = "ABOUT";
            treeNode29.Text = "About";
            treeNode30.Name = "OTHER";
            treeNode30.Text = "その他";
            TreeMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode16, treeNode27, treeNode28, treeNode30 });
            TreeMenu.Size = new System.Drawing.Size(199, 420);
            TreeMenu.TabIndex = 3;
            TreeMenu.AfterSelect += TreeMenu_AfterSelect;
            // 
            // tabControlEffects
            // 
            tabControlEffects.Controls.Add(tabSetting);
            tabControlEffects.Controls.Add(tabGEqualizer);
            tabControlEffects.Controls.Add(tabPitch);
            tabControlEffects.Controls.Add(tabDistortion);
            tabControlEffects.Controls.Add(tabChorus);
            tabControlEffects.Controls.Add(tabEcho);
            tabControlEffects.Controls.Add(tabFlanger);
            tabControlEffects.Controls.Add(tabHightpass);
            tabControlEffects.Controls.Add(tabLowpass);
            tabControlEffects.Controls.Add(tabCompressor);
            tabControlEffects.Controls.Add(tabReverb);
            tabControlEffects.Controls.Add(tabSkin);
            tabControlEffects.Controls.Add(tabAbout);
            tabControlEffects.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlEffects.Location = new System.Drawing.Point(199, 0);
            tabControlEffects.Name = "tabControlEffects";
            tabControlEffects.SelectedIndex = 0;
            tabControlEffects.Size = new System.Drawing.Size(582, 420);
            tabControlEffects.TabIndex = 4;
            // 
            // tabSetting
            // 
            tabSetting.BackColor = System.Drawing.Color.Transparent;
            tabSetting.Controls.Add(lblOutputNote);
            tabSetting.Controls.Add(BtnUpdate);
            tabSetting.Controls.Add(cmbDevice);
            tabSetting.Controls.Add(label38);
            tabSetting.Controls.Add(cmbOutput);
            tabSetting.Controls.Add(label36);
            tabSetting.Location = new System.Drawing.Point(4, 24);
            tabSetting.Name = "tabSetting";
            tabSetting.Size = new System.Drawing.Size(574, 392);
            tabSetting.TabIndex = 10;
            tabSetting.Text = "出力設定";
            // 
            // lblOutputNote
            // 
            lblOutputNote.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblOutputNote.AutoEllipsis = true;
            lblOutputNote.Location = new System.Drawing.Point(353, 17);
            lblOutputNote.Name = "lblOutputNote";
            lblOutputNote.Size = new System.Drawing.Size(196, 44);
            lblOutputNote.TabIndex = 26;
            // 
            // BtnUpdate
            // 
            BtnUpdate.Location = new System.Drawing.Point(272, 204);
            BtnUpdate.Name = "BtnUpdate";
            BtnUpdate.Size = new System.Drawing.Size(75, 23);
            BtnUpdate.TabIndex = 25;
            BtnUpdate.Text = "適用";
            BtnUpdate.UseVisualStyleBackColor = true;
            BtnUpdate.Click += BtnUpdate_Click;
            // 
            // cmbDevice
            // 
            cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDevice.FormattingEnabled = true;
            cmbDevice.Location = new System.Drawing.Point(105, 43);
            cmbDevice.Name = "cmbDevice";
            cmbDevice.Size = new System.Drawing.Size(242, 23);
            cmbDevice.TabIndex = 16;
            cmbDevice.SelectedIndexChanged += cmbDevice_SelectedIndexChanged;
            // 
            // label38
            // 
            label38.AutoSize = true;
            label38.Location = new System.Drawing.Point(55, 46);
            label38.Name = "label38";
            label38.Size = new System.Drawing.Size(44, 15);
            label38.TabIndex = 15;
            label38.Text = "デバイス";
            label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbOutput
            // 
            cmbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbOutput.FormattingEnabled = true;
            cmbOutput.Items.AddRange(new object[] { "自動検出", "Windows Audio Session API", "Low latency ASIO 2.0", "Windows Sonic" });
            cmbOutput.Location = new System.Drawing.Point(105, 14);
            cmbOutput.Name = "cmbOutput";
            cmbOutput.Size = new System.Drawing.Size(242, 23);
            cmbOutput.TabIndex = 14;
            cmbOutput.SelectedIndexChanged += cmbOutput_SelectedIndexChanged;
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new System.Drawing.Point(44, 17);
            label36.Name = "label36";
            label36.Size = new System.Drawing.Size(55, 15);
            label36.TabIndex = 13;
            label36.Text = "出力方式";
            label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabGEqualizer
            // 
            tabGEqualizer.BackColor = System.Drawing.Color.Transparent;
            tabGEqualizer.Controls.Add(GroupGEQ);
            tabGEqualizer.Location = new System.Drawing.Point(4, 24);
            tabGEqualizer.Name = "tabGEqualizer";
            tabGEqualizer.Size = new System.Drawing.Size(574, 392);
            tabGEqualizer.TabIndex = 9;
            tabGEqualizer.Text = "Graphic Equalizer";
            // 
            // GroupGEQ
            // 
            GroupGEQ.Controls.Add(label56);
            GroupGEQ.Controls.Add(label55);
            GroupGEQ.Controls.Add(label54);
            GroupGEQ.Controls.Add(label53);
            GroupGEQ.Controls.Add(label52);
            GroupGEQ.Controls.Add(label51);
            GroupGEQ.Controls.Add(label50);
            GroupGEQ.Controls.Add(label49);
            GroupGEQ.Controls.Add(label48);
            GroupGEQ.Controls.Add(label47);
            GroupGEQ.Controls.Add(label46);
            GroupGEQ.Controls.Add(label45);
            GroupGEQ.Controls.Add(label44);
            GroupGEQ.Controls.Add(cmbEqPreset);
            GroupGEQ.Controls.Add(CheckGEQ);
            GroupGEQ.Controls.Add(PictGEQGraph);
            GroupGEQ.Controls.Add(TrkGEQ60);
            GroupGEQ.Controls.Add(TrkGEQ32);
            GroupGEQ.Controls.Add(TrkGEQ125);
            GroupGEQ.Controls.Add(TrkGEQ250);
            GroupGEQ.Controls.Add(TrkGEQ500);
            GroupGEQ.Controls.Add(TrkGEQ1K);
            GroupGEQ.Controls.Add(TrkGEQ2K);
            GroupGEQ.Controls.Add(TrkGEQ4K);
            GroupGEQ.Controls.Add(TrkGEQ8K);
            GroupGEQ.Controls.Add(TrkGEQ16K);
            GroupGEQ.Controls.Add(TrkGEQ20K);
            GroupGEQ.Controls.Add(TrkGEQ22K);
            GroupGEQ.Controls.Add(btnGEQPresetSave);
            GroupGEQ.Controls.Add(btnGEQPresetDelete);
            GroupGEQ.Location = new System.Drawing.Point(3, 6);
            GroupGEQ.Name = "GroupGEQ";
            GroupGEQ.Size = new System.Drawing.Size(563, 377);
            GroupGEQ.TabIndex = 13;
            GroupGEQ.TabStop = false;
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Location = new System.Drawing.Point(518, 348);
            label56.Name = "label56";
            label56.Size = new System.Drawing.Size(26, 15);
            label56.TabIndex = 28;
            label56.Text = "22K";
            // 
            // label55
            // 
            label55.AutoSize = true;
            label55.Location = new System.Drawing.Point(469, 348);
            label55.Name = "label55";
            label55.Size = new System.Drawing.Size(26, 15);
            label55.TabIndex = 28;
            label55.Text = "20K";
            // 
            // label54
            // 
            label54.AutoSize = true;
            label54.Location = new System.Drawing.Point(424, 348);
            label54.Name = "label54";
            label54.Size = new System.Drawing.Size(26, 15);
            label54.TabIndex = 28;
            label54.Text = "16K";
            // 
            // label53
            // 
            label53.AutoSize = true;
            label53.Location = new System.Drawing.Point(385, 348);
            label53.Name = "label53";
            label53.Size = new System.Drawing.Size(20, 15);
            label53.TabIndex = 28;
            label53.Text = "8K";
            // 
            // label52
            // 
            label52.AutoSize = true;
            label52.Location = new System.Drawing.Point(343, 348);
            label52.Name = "label52";
            label52.Size = new System.Drawing.Size(20, 15);
            label52.TabIndex = 28;
            label52.Text = "4K";
            // 
            // label51
            // 
            label51.AutoSize = true;
            label51.Location = new System.Drawing.Point(295, 348);
            label51.Name = "label51";
            label51.Size = new System.Drawing.Size(20, 15);
            label51.TabIndex = 28;
            label51.Text = "2K";
            // 
            // label50
            // 
            label50.AutoSize = true;
            label50.Location = new System.Drawing.Point(250, 348);
            label50.Name = "label50";
            label50.Size = new System.Drawing.Size(20, 15);
            label50.TabIndex = 28;
            label50.Text = "1K";
            // 
            // label49
            // 
            label49.AutoSize = true;
            label49.Location = new System.Drawing.Point(200, 348);
            label49.Name = "label49";
            label49.Size = new System.Drawing.Size(25, 15);
            label49.TabIndex = 28;
            label49.Text = "500";
            // 
            // label48
            // 
            label48.AutoSize = true;
            label48.Location = new System.Drawing.Point(155, 348);
            label48.Name = "label48";
            label48.Size = new System.Drawing.Size(25, 15);
            label48.TabIndex = 28;
            label48.Text = "250";
            // 
            // label47
            // 
            label47.AutoSize = true;
            label47.Location = new System.Drawing.Point(110, 348);
            label47.Name = "label47";
            label47.Size = new System.Drawing.Size(25, 15);
            label47.TabIndex = 28;
            label47.Text = "125";
            // 
            // label46
            // 
            label46.AutoSize = true;
            label46.Location = new System.Drawing.Point(73, 348);
            label46.Name = "label46";
            label46.Size = new System.Drawing.Size(19, 15);
            label46.TabIndex = 28;
            label46.Text = "60";
            // 
            // label45
            // 
            label45.AutoSize = true;
            label45.Location = new System.Drawing.Point(26, 348);
            label45.Name = "label45";
            label45.Size = new System.Drawing.Size(19, 15);
            label45.TabIndex = 28;
            label45.Text = "32";
            // 
            // label44
            // 
            label44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label44.Location = new System.Drawing.Point(23, 274);
            label44.Name = "label44";
            label44.Size = new System.Drawing.Size(521, 1);
            label44.TabIndex = 15;
            // 
            // cmbEqPreset
            // 
            cmbEqPreset.FormattingEnabled = true;
            cmbEqPreset.Items.AddRange(new object[] { "Normal", "Rock", "Pop", "Bass Boost", "Trable Boost", "Total Boost", "Total Reduce" });
            cmbEqPreset.Location = new System.Drawing.Point(6, 25);
            cmbEqPreset.Name = "cmbEqPreset";
            cmbEqPreset.Size = new System.Drawing.Size(121, 23);
            cmbEqPreset.TabIndex = 14;
            cmbEqPreset.SelectedIndexChanged += cmbEqPreset_SelectedIndexChanged;
            // 
            // CheckGEQ
            // 
            CheckGEQ.AutoSize = true;
            CheckGEQ.BackColor = System.Drawing.SystemColors.Control;
            CheckGEQ.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckGEQ.Location = new System.Drawing.Point(6, 0);
            CheckGEQ.Name = "CheckGEQ";
            CheckGEQ.Size = new System.Drawing.Size(117, 19);
            CheckGEQ.TabIndex = 13;
            CheckGEQ.Text = "Graphic Equalizer";
            CheckGEQ.UseVisualStyleBackColor = false;
            CheckGEQ.CheckedChanged += CheckGEQ_CheckedChanged;
            // 
            // PictGEQGraph
            // 
            PictGEQGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            PictGEQGraph.Location = new System.Drawing.Point(6, 54);
            PictGEQGraph.Name = "PictGEQGraph";
            PictGEQGraph.Size = new System.Drawing.Size(551, 137);
            PictGEQGraph.TabIndex = 0;
            PictGEQGraph.TabStop = false;
            // 
            // TrkGEQ60
            // 
            TrkGEQ60.AutoSize = false;
            TrkGEQ60.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ60.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ60.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ60.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ60.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ60.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ60.Location = new System.Drawing.Point(51, 197);
            TrkGEQ60.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ60.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ60.Name = "TrkGEQ60";
            TrkGEQ60.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ60.Padding = 10;
            TrkGEQ60.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ60.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ60.ShowDivisionsText = false;
            TrkGEQ60.ShowSmallScale = false;
            TrkGEQ60.Size = new System.Drawing.Size(61, 154);
            TrkGEQ60.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ60.TabIndex = 17;
            TrkGEQ60.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ60.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ60.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ60.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ60.TickAdd = 0F;
            TrkGEQ60.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ60.TickDivide = 10F;
            TrkGEQ60.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ60.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ32
            // 
            TrkGEQ32.AutoSize = false;
            TrkGEQ32.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ32.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ32.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ32.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ32.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ32.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ32.Location = new System.Drawing.Point(6, 197);
            TrkGEQ32.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ32.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ32.Name = "TrkGEQ32";
            TrkGEQ32.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ32.Padding = 10;
            TrkGEQ32.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ32.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ32.ShowDivisionsText = true;
            TrkGEQ32.ShowSmallScale = false;
            TrkGEQ32.Size = new System.Drawing.Size(61, 154);
            TrkGEQ32.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ32.TabIndex = 16;
            TrkGEQ32.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ32.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ32.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ32.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ32.TickAdd = 0F;
            TrkGEQ32.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ32.TickDivide = 10F;
            TrkGEQ32.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ32.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ125
            // 
            TrkGEQ125.AutoSize = false;
            TrkGEQ125.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ125.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ125.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ125.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ125.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ125.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ125.Location = new System.Drawing.Point(96, 197);
            TrkGEQ125.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ125.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ125.Name = "TrkGEQ125";
            TrkGEQ125.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ125.Padding = 10;
            TrkGEQ125.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ125.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ125.ShowDivisionsText = false;
            TrkGEQ125.ShowSmallScale = false;
            TrkGEQ125.Size = new System.Drawing.Size(61, 154);
            TrkGEQ125.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ125.TabIndex = 18;
            TrkGEQ125.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ125.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ125.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ125.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ125.TickAdd = 0F;
            TrkGEQ125.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ125.TickDivide = 10F;
            TrkGEQ125.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ125.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ250
            // 
            TrkGEQ250.AutoSize = false;
            TrkGEQ250.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ250.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ250.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ250.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ250.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ250.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ250.Location = new System.Drawing.Point(141, 197);
            TrkGEQ250.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ250.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ250.Name = "TrkGEQ250";
            TrkGEQ250.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ250.Padding = 10;
            TrkGEQ250.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ250.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ250.ShowDivisionsText = false;
            TrkGEQ250.ShowSmallScale = false;
            TrkGEQ250.Size = new System.Drawing.Size(61, 154);
            TrkGEQ250.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ250.TabIndex = 19;
            TrkGEQ250.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ250.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ250.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ250.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ250.TickAdd = 0F;
            TrkGEQ250.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ250.TickDivide = 10F;
            TrkGEQ250.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ250.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ500
            // 
            TrkGEQ500.AutoSize = false;
            TrkGEQ500.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ500.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ500.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ500.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ500.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ500.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ500.Location = new System.Drawing.Point(186, 197);
            TrkGEQ500.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ500.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ500.Name = "TrkGEQ500";
            TrkGEQ500.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ500.Padding = 10;
            TrkGEQ500.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ500.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ500.ShowDivisionsText = false;
            TrkGEQ500.ShowSmallScale = false;
            TrkGEQ500.Size = new System.Drawing.Size(61, 154);
            TrkGEQ500.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ500.TabIndex = 20;
            TrkGEQ500.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ500.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ500.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ500.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ500.TickAdd = 0F;
            TrkGEQ500.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ500.TickDivide = 10F;
            TrkGEQ500.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ500.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ1K
            // 
            TrkGEQ1K.AutoSize = false;
            TrkGEQ1K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ1K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ1K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ1K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ1K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ1K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ1K.Location = new System.Drawing.Point(231, 197);
            TrkGEQ1K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ1K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ1K.Name = "TrkGEQ1K";
            TrkGEQ1K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ1K.Padding = 10;
            TrkGEQ1K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ1K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ1K.ShowDivisionsText = false;
            TrkGEQ1K.ShowSmallScale = false;
            TrkGEQ1K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ1K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ1K.TabIndex = 21;
            TrkGEQ1K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ1K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ1K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ1K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ1K.TickAdd = 0F;
            TrkGEQ1K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ1K.TickDivide = 10F;
            TrkGEQ1K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ1K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ2K
            // 
            TrkGEQ2K.AutoSize = false;
            TrkGEQ2K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ2K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ2K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ2K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ2K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ2K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ2K.Location = new System.Drawing.Point(276, 197);
            TrkGEQ2K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ2K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ2K.Name = "TrkGEQ2K";
            TrkGEQ2K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ2K.Padding = 10;
            TrkGEQ2K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ2K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ2K.ShowDivisionsText = false;
            TrkGEQ2K.ShowSmallScale = false;
            TrkGEQ2K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ2K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ2K.TabIndex = 22;
            TrkGEQ2K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ2K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ2K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ2K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ2K.TickAdd = 0F;
            TrkGEQ2K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ2K.TickDivide = 10F;
            TrkGEQ2K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ2K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ4K
            // 
            TrkGEQ4K.AutoSize = false;
            TrkGEQ4K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ4K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ4K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ4K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ4K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ4K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ4K.Location = new System.Drawing.Point(321, 197);
            TrkGEQ4K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ4K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ4K.Name = "TrkGEQ4K";
            TrkGEQ4K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ4K.Padding = 10;
            TrkGEQ4K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ4K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ4K.ShowDivisionsText = false;
            TrkGEQ4K.ShowSmallScale = false;
            TrkGEQ4K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ4K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ4K.TabIndex = 23;
            TrkGEQ4K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ4K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ4K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ4K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ4K.TickAdd = 0F;
            TrkGEQ4K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ4K.TickDivide = 10F;
            TrkGEQ4K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ4K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ8K
            // 
            TrkGEQ8K.AutoSize = false;
            TrkGEQ8K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ8K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ8K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ8K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ8K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ8K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ8K.Location = new System.Drawing.Point(366, 197);
            TrkGEQ8K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ8K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ8K.Name = "TrkGEQ8K";
            TrkGEQ8K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ8K.Padding = 10;
            TrkGEQ8K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ8K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ8K.ShowDivisionsText = false;
            TrkGEQ8K.ShowSmallScale = false;
            TrkGEQ8K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ8K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ8K.TabIndex = 24;
            TrkGEQ8K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ8K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ8K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ8K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ8K.TickAdd = 0F;
            TrkGEQ8K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ8K.TickDivide = 10F;
            TrkGEQ8K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ8K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ16K
            // 
            TrkGEQ16K.AutoSize = false;
            TrkGEQ16K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ16K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ16K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ16K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ16K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ16K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ16K.Location = new System.Drawing.Point(411, 197);
            TrkGEQ16K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ16K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ16K.Name = "TrkGEQ16K";
            TrkGEQ16K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ16K.Padding = 10;
            TrkGEQ16K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ16K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ16K.ShowDivisionsText = false;
            TrkGEQ16K.ShowSmallScale = false;
            TrkGEQ16K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ16K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ16K.TabIndex = 25;
            TrkGEQ16K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ16K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ16K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ16K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ16K.TickAdd = 0F;
            TrkGEQ16K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ16K.TickDivide = 10F;
            TrkGEQ16K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ16K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ20K
            // 
            TrkGEQ20K.AutoSize = false;
            TrkGEQ20K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ20K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ20K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ20K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ20K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ20K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ20K.Location = new System.Drawing.Point(456, 197);
            TrkGEQ20K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ20K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ20K.Name = "TrkGEQ20K";
            TrkGEQ20K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ20K.Padding = 10;
            TrkGEQ20K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ20K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ20K.ShowDivisionsText = false;
            TrkGEQ20K.ShowSmallScale = false;
            TrkGEQ20K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ20K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ20K.TabIndex = 26;
            TrkGEQ20K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ20K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ20K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ20K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ20K.TickAdd = 0F;
            TrkGEQ20K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ20K.TickDivide = 10F;
            TrkGEQ20K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ20K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // TrkGEQ22K
            // 
            TrkGEQ22K.AutoSize = false;
            TrkGEQ22K.BackColor = System.Drawing.Color.Transparent;
            TrkGEQ22K.BarInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ22K.BarPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.BarPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            TrkGEQ22K.ElapsedInnerColor = System.Drawing.SystemColors.ControlLight;
            TrkGEQ22K.ElapsedPenColorBottom = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.ElapsedPenColorTop = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.Font = new System.Drawing.Font("Yu Gothic UI", 6F);
            TrkGEQ22K.LargeChange = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ22K.Location = new System.Drawing.Point(501, 197);
            TrkGEQ22K.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            TrkGEQ22K.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            TrkGEQ22K.Name = "TrkGEQ22K";
            TrkGEQ22K.Orientation = System.Windows.Forms.Orientation.Vertical;
            TrkGEQ22K.Padding = 10;
            TrkGEQ22K.ScaleDivisions = new decimal(new int[] { 10, 0, 0, 0 });
            TrkGEQ22K.ScaleSubDivisions = new decimal(new int[] { 5, 0, 0, 0 });
            TrkGEQ22K.ShowDivisionsText = true;
            TrkGEQ22K.ShowSmallScale = false;
            TrkGEQ22K.Size = new System.Drawing.Size(61, 154);
            TrkGEQ22K.SmallChange = new decimal(new int[] { 1, 0, 0, 0 });
            TrkGEQ22K.TabIndex = 27;
            TrkGEQ22K.ThumbInnerColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.ThumbOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ22K.ThumbPenColor = System.Drawing.SystemColors.ControlDark;
            TrkGEQ22K.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            TrkGEQ22K.ThumbSize = new System.Drawing.Size(16, 8);
            TrkGEQ22K.TickAdd = 0F;
            TrkGEQ22K.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            TrkGEQ22K.TickDivide = 10F;
            TrkGEQ22K.TickStyle = System.Windows.Forms.TickStyle.Both;
            TrkGEQ22K.Value = new decimal(new int[] { 0, 0, 0, 0 });
            TrkGEQ22K.ValueChanged += EqTrackBar_ValueChanged;
            // 
            // btnGEQPresetSave
            // 
            btnGEQPresetSave.Location = new System.Drawing.Point(133, 25);
            btnGEQPresetSave.Name = "btnGEQPresetSave";
            btnGEQPresetSave.Size = new System.Drawing.Size(50, 23);
            btnGEQPresetSave.TabIndex = 29;
            btnGEQPresetSave.Text = "保存";
            btnGEQPresetSave.Click += BtnGEQPresetSave_Click;
            // 
            // btnGEQPresetDelete
            // 
            btnGEQPresetDelete.Location = new System.Drawing.Point(189, 25);
            btnGEQPresetDelete.Name = "btnGEQPresetDelete";
            btnGEQPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnGEQPresetDelete.TabIndex = 30;
            btnGEQPresetDelete.Text = "削除";
            btnGEQPresetDelete.Click += BtnGEQPresetDelete_Click;
            // 
            // tabPitch
            // 
            tabPitch.BackColor = System.Drawing.Color.Transparent;
            tabPitch.Controls.Add(GroupSpeed);
            tabPitch.Controls.Add(GroupFrequency);
            tabPitch.Controls.Add(GroupPitchShift);
            tabPitch.Location = new System.Drawing.Point(4, 24);
            tabPitch.Name = "tabPitch";
            tabPitch.Size = new System.Drawing.Size(574, 392);
            tabPitch.TabIndex = 8;
            tabPitch.Text = "Pitch/Freq/Speed";
            // 
            // GroupSpeed
            // 
            GroupSpeed.Controls.Add(lblValSpeed);
            GroupSpeed.Controls.Add(label6);
            GroupSpeed.Controls.Add(KnobSpeed);
            GroupSpeed.Controls.Add(CheckSpeed);
            GroupSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            GroupSpeed.Location = new System.Drawing.Point(264, 3);
            GroupSpeed.Name = "GroupSpeed";
            GroupSpeed.Size = new System.Drawing.Size(100, 150);
            GroupSpeed.TabIndex = 16;
            GroupSpeed.TabStop = false;
            // 
            // lblValSpeed
            // 
            lblValSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValSpeed.Location = new System.Drawing.Point(6, 111);
            lblValSpeed.Name = "lblValSpeed";
            lblValSpeed.ReadOnly = true;
            lblValSpeed.Size = new System.Drawing.Size(64, 16);
            lblValSpeed.TabIndex = 2;
            lblValSpeed.Text = "0.0";
            lblValSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label6.Location = new System.Drawing.Point(6, 22);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(64, 16);
            label6.TabIndex = 2;
            label6.Text = "Speed";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobSpeed
            // 
            KnobSpeed.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobSpeed.BorderWidth = 2;
            KnobSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobSpeed.HasTicks = true;
            KnobSpeed.KnobColor = System.Drawing.SystemColors.Control;
            KnobSpeed.LargeChange = 10;
            KnobSpeed.Location = new System.Drawing.Point(6, 41);
            KnobSpeed.Minimum = -100;
            KnobSpeed.Name = "KnobSpeed";
            KnobSpeed.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobSpeed.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobSpeed.PointerOffset = 4;
            KnobSpeed.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobSpeed.PointerWidth = 2;
            KnobSpeed.Size = new System.Drawing.Size(55, 55);
            KnobSpeed.TabIndex = 1;
            KnobSpeed.Text = "Level";
            KnobSpeed.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobSpeed.ValueChanged += KnobSpeed_ValueChanged;
            // 
            // CheckSpeed
            // 
            CheckSpeed.AutoSize = true;
            CheckSpeed.BackColor = System.Drawing.SystemColors.Control;
            CheckSpeed.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckSpeed.Location = new System.Drawing.Point(6, 3);
            CheckSpeed.Name = "CheckSpeed";
            CheckSpeed.Size = new System.Drawing.Size(58, 19);
            CheckSpeed.TabIndex = 0;
            CheckSpeed.Text = "Speed";
            CheckSpeed.UseVisualStyleBackColor = false;
            CheckSpeed.CheckedChanged += CheckSpeed_CheckedChanged;
            // 
            // GroupFrequency
            // 
            GroupFrequency.Controls.Add(lblValFrequency);
            GroupFrequency.Controls.Add(label8);
            GroupFrequency.Controls.Add(KnobFrequency);
            GroupFrequency.Controls.Add(CheckFrequency);
            GroupFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            GroupFrequency.Location = new System.Drawing.Point(158, 3);
            GroupFrequency.Name = "GroupFrequency";
            GroupFrequency.Size = new System.Drawing.Size(100, 150);
            GroupFrequency.TabIndex = 15;
            GroupFrequency.TabStop = false;
            // 
            // lblValFrequency
            // 
            lblValFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValFrequency.Location = new System.Drawing.Point(6, 114);
            lblValFrequency.Name = "lblValFrequency";
            lblValFrequency.ReadOnly = true;
            lblValFrequency.Size = new System.Drawing.Size(64, 16);
            lblValFrequency.TabIndex = 2;
            lblValFrequency.Text = "0.0";
            lblValFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            label8.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label8.Location = new System.Drawing.Point(6, 25);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(64, 16);
            label8.TabIndex = 2;
            label8.Text = "Frequency";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFrequency
            // 
            KnobFrequency.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFrequency.BorderWidth = 2;
            KnobFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobFrequency.HasTicks = true;
            KnobFrequency.KnobColor = System.Drawing.SystemColors.Control;
            KnobFrequency.LargeChange = 10;
            KnobFrequency.Location = new System.Drawing.Point(6, 44);
            KnobFrequency.Minimum = -100;
            KnobFrequency.Name = "KnobFrequency";
            KnobFrequency.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobFrequency.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFrequency.PointerOffset = 4;
            KnobFrequency.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFrequency.PointerWidth = 2;
            KnobFrequency.Size = new System.Drawing.Size(55, 55);
            KnobFrequency.TabIndex = 1;
            KnobFrequency.Text = "Level";
            KnobFrequency.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFrequency.ValueChanged += KnobFrequency_ValueChanged;
            // 
            // CheckFrequency
            // 
            CheckFrequency.AutoSize = true;
            CheckFrequency.BackColor = System.Drawing.SystemColors.Control;
            CheckFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckFrequency.Location = new System.Drawing.Point(6, 3);
            CheckFrequency.Name = "CheckFrequency";
            CheckFrequency.Size = new System.Drawing.Size(80, 19);
            CheckFrequency.TabIndex = 0;
            CheckFrequency.Text = "Frequency";
            CheckFrequency.UseVisualStyleBackColor = false;
            CheckFrequency.CheckedChanged += CheckFrequency_CheckedChanged;
            // 
            // GroupPitchShift
            // 
            GroupPitchShift.Controls.Add(lblValPitchFFT);
            GroupPitchShift.Controls.Add(label2);
            GroupPitchShift.Controls.Add(KnobPitchFFT);
            GroupPitchShift.Controls.Add(lblValPitchPitch);
            GroupPitchShift.Controls.Add(label4);
            GroupPitchShift.Controls.Add(KnobPitchPitch);
            GroupPitchShift.Controls.Add(CheckPitch);
            GroupPitchShift.Controls.Add(cmbPitchPreset);
            GroupPitchShift.Controls.Add(btnPitchPresetSave);
            GroupPitchShift.Controls.Add(btnPitchPresetDelete);
            GroupPitchShift.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            GroupPitchShift.Location = new System.Drawing.Point(3, 3);
            GroupPitchShift.Name = "GroupPitchShift";
            GroupPitchShift.Size = new System.Drawing.Size(149, 150);
            GroupPitchShift.TabIndex = 14;
            GroupPitchShift.TabStop = false;
            // 
            // lblValPitchFFT
            // 
            lblValPitchFFT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValPitchFFT.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValPitchFFT.Location = new System.Drawing.Point(77, 114);
            lblValPitchFFT.Name = "lblValPitchFFT";
            lblValPitchFFT.ReadOnly = true;
            lblValPitchFFT.Size = new System.Drawing.Size(64, 16);
            lblValPitchFFT.TabIndex = 5;
            lblValPitchFFT.Text = "0.0";
            lblValPitchFFT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label2.Location = new System.Drawing.Point(76, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 16);
            label2.TabIndex = 4;
            label2.Text = "FFT";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobPitchFFT
            // 
            KnobPitchFFT.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobPitchFFT.BorderWidth = 2;
            KnobPitchFFT.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobPitchFFT.HasTicks = true;
            KnobPitchFFT.KnobColor = System.Drawing.SystemColors.Control;
            KnobPitchFFT.LargeChange = 1;
            KnobPitchFFT.Location = new System.Drawing.Point(77, 44);
            KnobPitchFFT.Maximum = 4;
            KnobPitchFFT.Name = "KnobPitchFFT";
            KnobPitchFFT.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobPitchFFT.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobPitchFFT.PointerOffset = 4;
            KnobPitchFFT.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobPitchFFT.PointerWidth = 2;
            KnobPitchFFT.Size = new System.Drawing.Size(55, 55);
            KnobPitchFFT.TabIndex = 3;
            KnobPitchFFT.Text = "Level";
            KnobPitchFFT.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobPitchFFT.ValueChanged += KnobPitchFFT_ValueChanged;
            // 
            // lblValPitchPitch
            // 
            lblValPitchPitch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValPitchPitch.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValPitchPitch.Location = new System.Drawing.Point(6, 114);
            lblValPitchPitch.Name = "lblValPitchPitch";
            lblValPitchPitch.Size = new System.Drawing.Size(64, 16);
            lblValPitchPitch.TabIndex = 2;
            lblValPitchPitch.Text = "0.0";
            lblValPitchPitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label4.Location = new System.Drawing.Point(6, 25);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(64, 16);
            label4.TabIndex = 2;
            label4.Text = "Pitch";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobPitchPitch
            // 
            KnobPitchPitch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobPitchPitch.BorderWidth = 2;
            KnobPitchPitch.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobPitchPitch.HasTicks = true;
            KnobPitchPitch.KnobColor = System.Drawing.SystemColors.Control;
            KnobPitchPitch.LargeChange = 1;
            KnobPitchPitch.Location = new System.Drawing.Point(6, 44);
            KnobPitchPitch.Maximum = 150;
            KnobPitchPitch.Name = "KnobPitchPitch";
            KnobPitchPitch.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobPitchPitch.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobPitchPitch.PointerOffset = 4;
            KnobPitchPitch.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobPitchPitch.PointerWidth = 2;
            KnobPitchPitch.Size = new System.Drawing.Size(55, 55);
            KnobPitchPitch.TabIndex = 1;
            KnobPitchPitch.Text = "Level";
            KnobPitchPitch.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobPitchPitch.ValueChanged += KnobPitchPitch_ValueChanged;
            // 
            // CheckPitch
            // 
            CheckPitch.AutoSize = true;
            CheckPitch.BackColor = System.Drawing.SystemColors.Control;
            CheckPitch.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckPitch.Location = new System.Drawing.Point(6, 3);
            CheckPitch.Name = "CheckPitch";
            CheckPitch.Size = new System.Drawing.Size(77, 19);
            CheckPitch.TabIndex = 0;
            CheckPitch.Text = "PitchShift";
            CheckPitch.UseVisualStyleBackColor = false;
            CheckPitch.CheckedChanged += CheckPitch_CheckedChanged;
            // 
            // cmbPitchPreset
            // 
            cmbPitchPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPitchPreset.Location = new System.Drawing.Point(6, 130);
            cmbPitchPreset.Name = "cmbPitchPreset";
            cmbPitchPreset.Size = new System.Drawing.Size(150, 29);
            cmbPitchPreset.TabIndex = 6;
            cmbPitchPreset.SelectedIndexChanged += cmbPitchPreset_SelectedIndexChanged;
            // 
            // btnPitchPresetSave
            // 
            btnPitchPresetSave.Location = new System.Drawing.Point(162, 130);
            btnPitchPresetSave.Name = "btnPitchPresetSave";
            btnPitchPresetSave.Size = new System.Drawing.Size(50, 23);
            btnPitchPresetSave.TabIndex = 7;
            btnPitchPresetSave.Text = "保存";
            btnPitchPresetSave.Click += BtnPitchPresetSave_Click;
            // 
            // btnPitchPresetDelete
            // 
            btnPitchPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnPitchPresetDelete.Name = "btnPitchPresetDelete";
            btnPitchPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnPitchPresetDelete.TabIndex = 8;
            btnPitchPresetDelete.Text = "削除";
            btnPitchPresetDelete.Click += BtnPitchPresetDelete_Click;
            // 
            // tabDistortion
            // 
            tabDistortion.BackColor = System.Drawing.Color.Transparent;
            tabDistortion.Controls.Add(GroupDistortion);
            tabDistortion.Location = new System.Drawing.Point(4, 24);
            tabDistortion.Name = "tabDistortion";
            tabDistortion.Padding = new System.Windows.Forms.Padding(3);
            tabDistortion.Size = new System.Drawing.Size(574, 392);
            tabDistortion.TabIndex = 0;
            tabDistortion.Text = "Distortion";
            // 
            // GroupDistortion
            // 
            GroupDistortion.Controls.Add(CheckDistortion);
            GroupDistortion.Controls.Add(lblValDistortionLevel);
            GroupDistortion.Controls.Add(KnobDistortionLevel);
            GroupDistortion.Controls.Add(label1);
            GroupDistortion.Controls.Add(cmbDistortionPreset);
            GroupDistortion.Controls.Add(btnDistortionPresetSave);
            GroupDistortion.Controls.Add(btnDistortionPresetDelete);
            GroupDistortion.Location = new System.Drawing.Point(6, 6);
            GroupDistortion.Name = "GroupDistortion";
            GroupDistortion.Size = new System.Drawing.Size(560, 378);
            GroupDistortion.TabIndex = 12;
            GroupDistortion.TabStop = false;
            // 
            // CheckDistortion
            // 
            CheckDistortion.AutoSize = true;
            CheckDistortion.BackColor = System.Drawing.SystemColors.Control;
            CheckDistortion.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckDistortion.Location = new System.Drawing.Point(6, 0);
            CheckDistortion.Name = "CheckDistortion";
            CheckDistortion.Size = new System.Drawing.Size(78, 19);
            CheckDistortion.TabIndex = 5;
            CheckDistortion.Text = "Distortion";
            CheckDistortion.UseVisualStyleBackColor = false;
            CheckDistortion.CheckedChanged += CheckDistortion_CheckedChanged;
            // 
            // lblValDistortionLevel
            // 
            lblValDistortionLevel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValDistortionLevel.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValDistortionLevel.Location = new System.Drawing.Point(29, 149);
            lblValDistortionLevel.Name = "lblValDistortionLevel";
            lblValDistortionLevel.Size = new System.Drawing.Size(64, 16);
            lblValDistortionLevel.TabIndex = 11;
            lblValDistortionLevel.Text = "0.0";
            lblValDistortionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // KnobDistortionLevel
            // 
            KnobDistortionLevel.BackColor = System.Drawing.SystemColors.Control;
            KnobDistortionLevel.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobDistortionLevel.BorderWidth = 2;
            KnobDistortionLevel.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobDistortionLevel.HasTicks = true;
            KnobDistortionLevel.KnobColor = System.Drawing.SystemColors.Control;
            KnobDistortionLevel.LargeChange = 5;
            KnobDistortionLevel.Location = new System.Drawing.Point(29, 77);
            KnobDistortionLevel.Name = "KnobDistortionLevel";
            KnobDistortionLevel.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobDistortionLevel.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobDistortionLevel.PointerOffset = 4;
            KnobDistortionLevel.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobDistortionLevel.PointerWidth = 2;
            KnobDistortionLevel.Size = new System.Drawing.Size(55, 55);
            KnobDistortionLevel.TabIndex = 9;
            KnobDistortionLevel.Text = "Level";
            KnobDistortionLevel.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobDistortionLevel.ValueChanged += KnobDistortionLevel_ValueChanged;
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label1.Location = new System.Drawing.Point(29, 60);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 16);
            label1.TabIndex = 8;
            label1.Text = "Level";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbDistortionPreset
            // 
            cmbDistortionPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDistortionPreset.Location = new System.Drawing.Point(6, 25);
            cmbDistortionPreset.Name = "cmbDistortionPreset";
            cmbDistortionPreset.Size = new System.Drawing.Size(150, 23);
            cmbDistortionPreset.TabIndex = 12;
            cmbDistortionPreset.SelectedIndexChanged += cmbDistortionPreset_SelectedIndexChanged;
            // 
            // btnDistortionPresetSave
            // 
            btnDistortionPresetSave.Location = new System.Drawing.Point(162, 25);
            btnDistortionPresetSave.Name = "btnDistortionPresetSave";
            btnDistortionPresetSave.Size = new System.Drawing.Size(50, 23);
            btnDistortionPresetSave.TabIndex = 13;
            btnDistortionPresetSave.Text = "保存";
            btnDistortionPresetSave.Click += BtnEchoPresetSave_Click;
            // 
            // btnDistortionPresetDelete
            // 
            btnDistortionPresetDelete.Location = new System.Drawing.Point(218, 25);
            btnDistortionPresetDelete.Name = "btnDistortionPresetDelete";
            btnDistortionPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnDistortionPresetDelete.TabIndex = 14;
            btnDistortionPresetDelete.Text = "削除";
            btnDistortionPresetDelete.Click += BtnDistortionPresetDelete_Click;
            // 
            // tabChorus
            // 
            tabChorus.BackColor = System.Drawing.Color.Transparent;
            tabChorus.Controls.Add(GroupChorus);
            tabChorus.Location = new System.Drawing.Point(4, 24);
            tabChorus.Name = "tabChorus";
            tabChorus.Padding = new System.Windows.Forms.Padding(3);
            tabChorus.Size = new System.Drawing.Size(574, 392);
            tabChorus.TabIndex = 1;
            tabChorus.Text = "Chorus";
            // 
            // GroupChorus
            // 
            GroupChorus.Controls.Add(lblValChorusDepth);
            GroupChorus.Controls.Add(label7);
            GroupChorus.Controls.Add(KnobChorusDepth);
            GroupChorus.Controls.Add(lblValChorusRate);
            GroupChorus.Controls.Add(label5);
            GroupChorus.Controls.Add(KnobChorusRate);
            GroupChorus.Controls.Add(lblValChorusMix);
            GroupChorus.Controls.Add(label3);
            GroupChorus.Controls.Add(KnobChorusMix);
            GroupChorus.Controls.Add(CheckChorus);
            GroupChorus.Controls.Add(cmbChorusPreset);
            GroupChorus.Controls.Add(btnChorusPresetSave);
            GroupChorus.Controls.Add(btnChorusPresetDelete);
            GroupChorus.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupChorus.Location = new System.Drawing.Point(6, 3);
            GroupChorus.Name = "GroupChorus";
            GroupChorus.Size = new System.Drawing.Size(560, 381);
            GroupChorus.TabIndex = 4;
            GroupChorus.TabStop = false;
            // 
            // lblValChorusDepth
            // 
            lblValChorusDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValChorusDepth.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValChorusDepth.Location = new System.Drawing.Point(145, 260);
            lblValChorusDepth.Name = "lblValChorusDepth";
            lblValChorusDepth.Size = new System.Drawing.Size(64, 16);
            lblValChorusDepth.TabIndex = 8;
            lblValChorusDepth.Text = "0.0";
            lblValChorusDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            label7.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label7.Location = new System.Drawing.Point(145, 168);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(64, 16);
            label7.TabIndex = 7;
            label7.Text = "Depth";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusDepth
            // 
            KnobChorusDepth.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusDepth.BorderWidth = 2;
            KnobChorusDepth.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobChorusDepth.HasTicks = true;
            KnobChorusDepth.KnobColor = System.Drawing.SystemColors.Control;
            KnobChorusDepth.LargeChange = 5;
            KnobChorusDepth.Location = new System.Drawing.Point(145, 190);
            KnobChorusDepth.Name = "KnobChorusDepth";
            KnobChorusDepth.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobChorusDepth.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusDepth.PointerOffset = 4;
            KnobChorusDepth.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusDepth.PointerWidth = 2;
            KnobChorusDepth.Size = new System.Drawing.Size(55, 55);
            KnobChorusDepth.TabIndex = 6;
            KnobChorusDepth.Text = "Level";
            KnobChorusDepth.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusDepth.ValueChanged += KnobChorusDepth_ValueChanged;
            // 
            // lblValChorusRate
            // 
            lblValChorusRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValChorusRate.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValChorusRate.Location = new System.Drawing.Point(75, 260);
            lblValChorusRate.Name = "lblValChorusRate";
            lblValChorusRate.Size = new System.Drawing.Size(64, 16);
            lblValChorusRate.TabIndex = 5;
            lblValChorusRate.Text = "0.0";
            lblValChorusRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            label5.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label5.Location = new System.Drawing.Point(75, 168);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(64, 16);
            label5.TabIndex = 4;
            label5.Text = "Rate";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusRate
            // 
            KnobChorusRate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusRate.BorderWidth = 2;
            KnobChorusRate.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobChorusRate.HasTicks = true;
            KnobChorusRate.KnobColor = System.Drawing.SystemColors.Control;
            KnobChorusRate.LargeChange = 1;
            KnobChorusRate.Location = new System.Drawing.Point(75, 190);
            KnobChorusRate.Maximum = 20;
            KnobChorusRate.Name = "KnobChorusRate";
            KnobChorusRate.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobChorusRate.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusRate.PointerOffset = 4;
            KnobChorusRate.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusRate.PointerWidth = 2;
            KnobChorusRate.Size = new System.Drawing.Size(55, 55);
            KnobChorusRate.TabIndex = 3;
            KnobChorusRate.Text = "Level";
            KnobChorusRate.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusRate.ValueChanged += KnobChorusRate_ValueChanged;
            // 
            // lblValChorusMix
            // 
            lblValChorusMix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValChorusMix.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValChorusMix.Location = new System.Drawing.Point(5, 260);
            lblValChorusMix.Name = "lblValChorusMix";
            lblValChorusMix.Size = new System.Drawing.Size(64, 16);
            lblValChorusMix.TabIndex = 2;
            lblValChorusMix.Text = "0.0";
            lblValChorusMix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label3.Location = new System.Drawing.Point(5, 168);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 16);
            label3.TabIndex = 2;
            label3.Text = "Mix";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobChorusMix
            // 
            KnobChorusMix.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusMix.BorderWidth = 2;
            KnobChorusMix.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobChorusMix.HasTicks = true;
            KnobChorusMix.KnobColor = System.Drawing.SystemColors.Control;
            KnobChorusMix.LargeChange = 5;
            KnobChorusMix.Location = new System.Drawing.Point(5, 190);
            KnobChorusMix.Name = "KnobChorusMix";
            KnobChorusMix.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobChorusMix.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusMix.PointerOffset = 4;
            KnobChorusMix.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobChorusMix.PointerWidth = 2;
            KnobChorusMix.Size = new System.Drawing.Size(55, 55);
            KnobChorusMix.TabIndex = 1;
            KnobChorusMix.Text = "Level";
            KnobChorusMix.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobChorusMix.ValueChanged += KnobChorusMix_ValueChanged;
            // 
            // CheckChorus
            // 
            CheckChorus.AutoSize = true;
            CheckChorus.BackColor = System.Drawing.SystemColors.Control;
            CheckChorus.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckChorus.Location = new System.Drawing.Point(6, 3);
            CheckChorus.Name = "CheckChorus";
            CheckChorus.Size = new System.Drawing.Size(63, 19);
            CheckChorus.TabIndex = 0;
            CheckChorus.Text = "Chorus";
            CheckChorus.UseVisualStyleBackColor = false;
            CheckChorus.CheckedChanged += CheckChorus_CheckedChanged;
            // 
            // cmbChorusPreset
            // 
            cmbChorusPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbChorusPreset.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            cmbChorusPreset.Location = new System.Drawing.Point(6, 28);
            cmbChorusPreset.Name = "cmbChorusPreset";
            cmbChorusPreset.Size = new System.Drawing.Size(150, 23);
            cmbChorusPreset.TabIndex = 9;
            cmbChorusPreset.SelectedIndexChanged += cmbChorusPreset_SelectedIndexChanged;
            // 
            // btnChorusPresetSave
            // 
            btnChorusPresetSave.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            btnChorusPresetSave.Location = new System.Drawing.Point(162, 28);
            btnChorusPresetSave.Name = "btnChorusPresetSave";
            btnChorusPresetSave.Size = new System.Drawing.Size(50, 23);
            btnChorusPresetSave.TabIndex = 10;
            btnChorusPresetSave.Text = "保存";
            btnChorusPresetSave.Click += BtnChorusPresetSave_Click;
            // 
            // btnChorusPresetDelete
            // 
            btnChorusPresetDelete.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            btnChorusPresetDelete.Location = new System.Drawing.Point(218, 28);
            btnChorusPresetDelete.Name = "btnChorusPresetDelete";
            btnChorusPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnChorusPresetDelete.TabIndex = 11;
            btnChorusPresetDelete.Text = "削除";
            btnChorusPresetDelete.Click += BtnChorusPresetDelete_Click;
            // 
            // tabEcho
            // 
            tabEcho.BackColor = System.Drawing.Color.Transparent;
            tabEcho.Controls.Add(GroupEcho);
            tabEcho.Location = new System.Drawing.Point(4, 24);
            tabEcho.Name = "tabEcho";
            tabEcho.Size = new System.Drawing.Size(574, 392);
            tabEcho.TabIndex = 2;
            tabEcho.Text = "Echo";
            // 
            // GroupEcho
            // 
            GroupEcho.BackColor = System.Drawing.Color.Transparent;
            GroupEcho.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            GroupEcho.Controls.Add(lblValEchoWet);
            GroupEcho.Controls.Add(label15);
            GroupEcho.Controls.Add(KnobEchoWet);
            GroupEcho.Controls.Add(lblValEchoDry);
            GroupEcho.Controls.Add(label9);
            GroupEcho.Controls.Add(KnobEchoDry);
            GroupEcho.Controls.Add(lblValEchoFeedback);
            GroupEcho.Controls.Add(label11);
            GroupEcho.Controls.Add(KnobEchoFeedback);
            GroupEcho.Controls.Add(lblValEchoDelay);
            GroupEcho.Controls.Add(label13);
            GroupEcho.Controls.Add(KnobEchoDelay);
            GroupEcho.Controls.Add(CheckEcho);
            GroupEcho.Controls.Add(cmbEchoPreset);
            GroupEcho.Controls.Add(btnEchoPresetSave);
            GroupEcho.Controls.Add(btnEchoPresetDelete);
            GroupEcho.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupEcho.Location = new System.Drawing.Point(3, 3);
            GroupEcho.Name = "GroupEcho";
            GroupEcho.Size = new System.Drawing.Size(563, 386);
            GroupEcho.TabIndex = 10;
            GroupEcho.TabStop = false;
            // 
            // lblValEchoWet
            // 
            lblValEchoWet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValEchoWet.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValEchoWet.Location = new System.Drawing.Point(228, 123);
            lblValEchoWet.Name = "lblValEchoWet";
            lblValEchoWet.Size = new System.Drawing.Size(64, 16);
            lblValEchoWet.TabIndex = 11;
            lblValEchoWet.Text = "0.0";
            lblValEchoWet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            label15.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label15.Location = new System.Drawing.Point(228, 34);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(64, 16);
            label15.TabIndex = 10;
            label15.Text = "Wet";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoWet
            // 
            KnobEchoWet.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoWet.BorderWidth = 2;
            KnobEchoWet.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobEchoWet.HasTicks = true;
            KnobEchoWet.KnobColor = System.Drawing.SystemColors.Control;
            KnobEchoWet.LargeChange = 5;
            KnobEchoWet.Location = new System.Drawing.Point(228, 53);
            KnobEchoWet.Maximum = 10;
            KnobEchoWet.Minimum = -80;
            KnobEchoWet.Name = "KnobEchoWet";
            KnobEchoWet.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobEchoWet.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoWet.PointerOffset = 4;
            KnobEchoWet.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoWet.PointerWidth = 2;
            KnobEchoWet.Size = new System.Drawing.Size(55, 55);
            KnobEchoWet.TabIndex = 9;
            KnobEchoWet.Text = "Level";
            KnobEchoWet.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoWet.ValueChanged += KnobEchoWet_ValueChanged;
            // 
            // lblValEchoDry
            // 
            lblValEchoDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValEchoDry.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValEchoDry.Location = new System.Drawing.Point(158, 123);
            lblValEchoDry.Name = "lblValEchoDry";
            lblValEchoDry.Size = new System.Drawing.Size(64, 16);
            lblValEchoDry.TabIndex = 8;
            lblValEchoDry.Text = "0.0";
            lblValEchoDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            label9.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label9.Location = new System.Drawing.Point(158, 34);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(64, 16);
            label9.TabIndex = 7;
            label9.Text = "Dry";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoDry
            // 
            KnobEchoDry.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoDry.BorderWidth = 2;
            KnobEchoDry.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobEchoDry.HasTicks = true;
            KnobEchoDry.KnobColor = System.Drawing.SystemColors.Control;
            KnobEchoDry.LargeChange = 5;
            KnobEchoDry.Location = new System.Drawing.Point(158, 53);
            KnobEchoDry.Maximum = 10;
            KnobEchoDry.Minimum = -80;
            KnobEchoDry.Name = "KnobEchoDry";
            KnobEchoDry.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobEchoDry.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoDry.PointerOffset = 4;
            KnobEchoDry.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoDry.PointerWidth = 2;
            KnobEchoDry.Size = new System.Drawing.Size(55, 55);
            KnobEchoDry.TabIndex = 6;
            KnobEchoDry.Text = "Level";
            KnobEchoDry.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoDry.ValueChanged += KnobEchoDry_ValueChanged;
            // 
            // lblValEchoFeedback
            // 
            lblValEchoFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValEchoFeedback.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValEchoFeedback.Location = new System.Drawing.Point(88, 123);
            lblValEchoFeedback.Name = "lblValEchoFeedback";
            lblValEchoFeedback.Size = new System.Drawing.Size(64, 16);
            lblValEchoFeedback.TabIndex = 5;
            lblValEchoFeedback.Text = "0.0";
            lblValEchoFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            label11.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label11.Location = new System.Drawing.Point(88, 34);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(64, 16);
            label11.TabIndex = 4;
            label11.Text = "Feedback";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoFeedback
            // 
            KnobEchoFeedback.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoFeedback.BorderWidth = 2;
            KnobEchoFeedback.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobEchoFeedback.HasTicks = true;
            KnobEchoFeedback.KnobColor = System.Drawing.SystemColors.Control;
            KnobEchoFeedback.LargeChange = 5;
            KnobEchoFeedback.Location = new System.Drawing.Point(88, 53);
            KnobEchoFeedback.Name = "KnobEchoFeedback";
            KnobEchoFeedback.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobEchoFeedback.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoFeedback.PointerOffset = 4;
            KnobEchoFeedback.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoFeedback.PointerWidth = 2;
            KnobEchoFeedback.Size = new System.Drawing.Size(55, 55);
            KnobEchoFeedback.TabIndex = 3;
            KnobEchoFeedback.Text = "Level";
            KnobEchoFeedback.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoFeedback.ValueChanged += KnobEchoFeedback_ValueChanged;
            // 
            // lblValEchoDelay
            // 
            lblValEchoDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValEchoDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValEchoDelay.Location = new System.Drawing.Point(18, 123);
            lblValEchoDelay.Name = "lblValEchoDelay";
            lblValEchoDelay.Size = new System.Drawing.Size(64, 16);
            lblValEchoDelay.TabIndex = 2;
            lblValEchoDelay.Text = "0.0";
            lblValEchoDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            label13.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label13.Location = new System.Drawing.Point(18, 34);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(64, 16);
            label13.TabIndex = 2;
            label13.Text = "Delay";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobEchoDelay
            // 
            KnobEchoDelay.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoDelay.BorderWidth = 2;
            KnobEchoDelay.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobEchoDelay.HasTicks = true;
            KnobEchoDelay.KnobColor = System.Drawing.SystemColors.Control;
            KnobEchoDelay.LargeChange = 100;
            KnobEchoDelay.Location = new System.Drawing.Point(18, 53);
            KnobEchoDelay.Maximum = 5000;
            KnobEchoDelay.Minimum = 1;
            KnobEchoDelay.Name = "KnobEchoDelay";
            KnobEchoDelay.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobEchoDelay.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoDelay.PointerOffset = 4;
            KnobEchoDelay.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobEchoDelay.PointerWidth = 2;
            KnobEchoDelay.Size = new System.Drawing.Size(55, 55);
            KnobEchoDelay.TabIndex = 1;
            KnobEchoDelay.Text = "Level";
            KnobEchoDelay.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobEchoDelay.ValueChanged += KnobEchoDelay_ValueChanged;
            // 
            // CheckEcho
            // 
            CheckEcho.AutoSize = true;
            CheckEcho.BackColor = System.Drawing.SystemColors.Control;
            CheckEcho.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckEcho.Location = new System.Drawing.Point(6, 0);
            CheckEcho.Name = "CheckEcho";
            CheckEcho.Size = new System.Drawing.Size(52, 19);
            CheckEcho.TabIndex = 0;
            CheckEcho.Text = "Echo";
            CheckEcho.UseVisualStyleBackColor = false;
            CheckEcho.CheckedChanged += CheckEcho_CheckedChanged;
            // 
            // cmbEchoPreset
            // 
            cmbEchoPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbEchoPreset.Location = new System.Drawing.Point(18, 139);
            cmbEchoPreset.Name = "cmbEchoPreset";
            cmbEchoPreset.Size = new System.Drawing.Size(150, 23);
            cmbEchoPreset.TabIndex = 12;
            cmbEchoPreset.SelectedIndexChanged += cmbEchoPreset_SelectedIndexChanged;
            // 
            // btnEchoPresetSave
            // 
            btnEchoPresetSave.Location = new System.Drawing.Point(174, 139);
            btnEchoPresetSave.Name = "btnEchoPresetSave";
            btnEchoPresetSave.Size = new System.Drawing.Size(50, 23);
            btnEchoPresetSave.TabIndex = 13;
            btnEchoPresetSave.Text = "保存";
            btnEchoPresetSave.Click += BtnEchoPresetSave_Click;
            // 
            // btnEchoPresetDelete
            // 
            btnEchoPresetDelete.Location = new System.Drawing.Point(230, 139);
            btnEchoPresetDelete.Name = "btnEchoPresetDelete";
            btnEchoPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnEchoPresetDelete.TabIndex = 14;
            btnEchoPresetDelete.Text = "削除";
            btnEchoPresetDelete.Click += BtnEchoPresetDelete_Click;
            // 
            // tabFlanger
            // 
            tabFlanger.BackColor = System.Drawing.Color.Transparent;
            tabFlanger.Controls.Add(GroupFlanger);
            tabFlanger.Location = new System.Drawing.Point(4, 24);
            tabFlanger.Name = "tabFlanger";
            tabFlanger.Size = new System.Drawing.Size(574, 392);
            tabFlanger.TabIndex = 3;
            tabFlanger.Text = "Flanger";
            // 
            // GroupFlanger
            // 
            GroupFlanger.Controls.Add(lblValFlangerDepth);
            GroupFlanger.Controls.Add(label33);
            GroupFlanger.Controls.Add(KnobFlangerDepth);
            GroupFlanger.Controls.Add(lblValFlangerRate);
            GroupFlanger.Controls.Add(label35);
            GroupFlanger.Controls.Add(KnobFlangerRate);
            GroupFlanger.Controls.Add(lblValFlangerMix);
            GroupFlanger.Controls.Add(label37);
            GroupFlanger.Controls.Add(KnobFlangerMix);
            GroupFlanger.Controls.Add(CheckFlanger);
            GroupFlanger.Controls.Add(cmbFlangerPreset);
            GroupFlanger.Controls.Add(btnFlangerPresetSave);
            GroupFlanger.Controls.Add(btnFlangerPresetDelete);
            GroupFlanger.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupFlanger.Location = new System.Drawing.Point(3, 3);
            GroupFlanger.Name = "GroupFlanger";
            GroupFlanger.Size = new System.Drawing.Size(225, 155);
            GroupFlanger.TabIndex = 10;
            GroupFlanger.TabStop = false;
            // 
            // lblValFlangerDepth
            // 
            lblValFlangerDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValFlangerDepth.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValFlangerDepth.Location = new System.Drawing.Point(146, 114);
            lblValFlangerDepth.Name = "lblValFlangerDepth";
            lblValFlangerDepth.Size = new System.Drawing.Size(64, 16);
            lblValFlangerDepth.TabIndex = 8;
            lblValFlangerDepth.Text = "0.0";
            lblValFlangerDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label33
            // 
            label33.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label33.Location = new System.Drawing.Point(146, 25);
            label33.Name = "label33";
            label33.Size = new System.Drawing.Size(64, 16);
            label33.TabIndex = 7;
            label33.Text = "Depth";
            label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerDepth
            // 
            KnobFlangerDepth.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerDepth.BorderWidth = 2;
            KnobFlangerDepth.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobFlangerDepth.HasTicks = true;
            KnobFlangerDepth.KnobColor = System.Drawing.SystemColors.Control;
            KnobFlangerDepth.LargeChange = 5;
            KnobFlangerDepth.Location = new System.Drawing.Point(146, 44);
            KnobFlangerDepth.Minimum = 1;
            KnobFlangerDepth.Name = "KnobFlangerDepth";
            KnobFlangerDepth.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobFlangerDepth.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerDepth.PointerOffset = 4;
            KnobFlangerDepth.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerDepth.PointerWidth = 2;
            KnobFlangerDepth.Size = new System.Drawing.Size(55, 55);
            KnobFlangerDepth.TabIndex = 6;
            KnobFlangerDepth.Text = "Level";
            KnobFlangerDepth.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerDepth.ValueChanged += KnobFlangerDepth_ValueChanged;
            // 
            // lblValFlangerRate
            // 
            lblValFlangerRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValFlangerRate.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValFlangerRate.Location = new System.Drawing.Point(76, 114);
            lblValFlangerRate.Name = "lblValFlangerRate";
            lblValFlangerRate.Size = new System.Drawing.Size(64, 16);
            lblValFlangerRate.TabIndex = 5;
            lblValFlangerRate.Text = "0.0";
            lblValFlangerRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label35
            // 
            label35.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label35.Location = new System.Drawing.Point(76, 25);
            label35.Name = "label35";
            label35.Size = new System.Drawing.Size(64, 16);
            label35.TabIndex = 4;
            label35.Text = "Rate";
            label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerRate
            // 
            KnobFlangerRate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerRate.BorderWidth = 2;
            KnobFlangerRate.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobFlangerRate.HasTicks = true;
            KnobFlangerRate.KnobColor = System.Drawing.SystemColors.Control;
            KnobFlangerRate.LargeChange = 1;
            KnobFlangerRate.Location = new System.Drawing.Point(76, 44);
            KnobFlangerRate.Maximum = 20;
            KnobFlangerRate.Name = "KnobFlangerRate";
            KnobFlangerRate.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobFlangerRate.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerRate.PointerOffset = 4;
            KnobFlangerRate.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerRate.PointerWidth = 2;
            KnobFlangerRate.Size = new System.Drawing.Size(55, 55);
            KnobFlangerRate.TabIndex = 3;
            KnobFlangerRate.Text = "Level";
            KnobFlangerRate.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerRate.ValueChanged += KnobFlangerRate_ValueChanged;
            // 
            // lblValFlangerMix
            // 
            lblValFlangerMix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValFlangerMix.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValFlangerMix.Location = new System.Drawing.Point(6, 114);
            lblValFlangerMix.Name = "lblValFlangerMix";
            lblValFlangerMix.Size = new System.Drawing.Size(64, 16);
            lblValFlangerMix.TabIndex = 2;
            lblValFlangerMix.Text = "0.0";
            lblValFlangerMix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label37
            // 
            label37.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label37.Location = new System.Drawing.Point(6, 25);
            label37.Name = "label37";
            label37.Size = new System.Drawing.Size(64, 16);
            label37.TabIndex = 2;
            label37.Text = "Mix";
            label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobFlangerMix
            // 
            KnobFlangerMix.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerMix.BorderWidth = 2;
            KnobFlangerMix.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobFlangerMix.HasTicks = true;
            KnobFlangerMix.KnobColor = System.Drawing.SystemColors.Control;
            KnobFlangerMix.LargeChange = 5;
            KnobFlangerMix.Location = new System.Drawing.Point(6, 44);
            KnobFlangerMix.Name = "KnobFlangerMix";
            KnobFlangerMix.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobFlangerMix.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerMix.PointerOffset = 4;
            KnobFlangerMix.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobFlangerMix.PointerWidth = 2;
            KnobFlangerMix.Size = new System.Drawing.Size(55, 55);
            KnobFlangerMix.TabIndex = 1;
            KnobFlangerMix.Text = "Level";
            KnobFlangerMix.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobFlangerMix.ValueChanged += KnobFlangerMix_ValueChanged;
            // 
            // CheckFlanger
            // 
            CheckFlanger.AutoSize = true;
            CheckFlanger.BackColor = System.Drawing.SystemColors.Control;
            CheckFlanger.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckFlanger.Location = new System.Drawing.Point(6, 3);
            CheckFlanger.Name = "CheckFlanger";
            CheckFlanger.Size = new System.Drawing.Size(65, 19);
            CheckFlanger.TabIndex = 0;
            CheckFlanger.Text = "Flanger";
            CheckFlanger.UseVisualStyleBackColor = false;
            CheckFlanger.CheckedChanged += CheckFlanger_CheckedChanged;
            // 
            // cmbFlangerPreset
            // 
            cmbFlangerPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFlangerPreset.Location = new System.Drawing.Point(6, 130);
            cmbFlangerPreset.Name = "cmbFlangerPreset";
            cmbFlangerPreset.Size = new System.Drawing.Size(150, 23);
            cmbFlangerPreset.TabIndex = 9;
            cmbFlangerPreset.SelectedIndexChanged += cmbFlangerPreset_SelectedIndexChanged;
            // 
            // btnFlangerPresetSave
            // 
            btnFlangerPresetSave.Location = new System.Drawing.Point(162, 130);
            btnFlangerPresetSave.Name = "btnFlangerPresetSave";
            btnFlangerPresetSave.Size = new System.Drawing.Size(50, 23);
            btnFlangerPresetSave.TabIndex = 10;
            btnFlangerPresetSave.Text = "保存";
            btnFlangerPresetSave.Click += BtnFlangerPresetSave_Click;
            // 
            // btnFlangerPresetDelete
            // 
            btnFlangerPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnFlangerPresetDelete.Name = "btnFlangerPresetDelete";
            btnFlangerPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnFlangerPresetDelete.TabIndex = 11;
            btnFlangerPresetDelete.Text = "削除";
            btnFlangerPresetDelete.Click += BtnFlangerPresetDelete_Click;
            // 
            // tabHightpass
            // 
            tabHightpass.BackColor = System.Drawing.Color.Transparent;
            tabHightpass.Controls.Add(GroupHighpass);
            tabHightpass.Location = new System.Drawing.Point(4, 24);
            tabHightpass.Name = "tabHightpass";
            tabHightpass.Size = new System.Drawing.Size(574, 392);
            tabHightpass.TabIndex = 4;
            tabHightpass.Text = "Highpass";
            // 
            // GroupHighpass
            // 
            GroupHighpass.Controls.Add(lblValHighpassResonance);
            GroupHighpass.Controls.Add(label21);
            GroupHighpass.Controls.Add(KnobHighpassResonance);
            GroupHighpass.Controls.Add(lblValHighpassCutoff);
            GroupHighpass.Controls.Add(label23);
            GroupHighpass.Controls.Add(KnobHighpassCutoff);
            GroupHighpass.Controls.Add(CheckHighpass);
            GroupHighpass.Controls.Add(cmbHighpassPreset);
            GroupHighpass.Controls.Add(btnHighpassPresetSave);
            GroupHighpass.Controls.Add(btnHighpassPresetDelete);
            GroupHighpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupHighpass.Location = new System.Drawing.Point(3, 3);
            GroupHighpass.Name = "GroupHighpass";
            GroupHighpass.Size = new System.Drawing.Size(163, 148);
            GroupHighpass.TabIndex = 13;
            GroupHighpass.TabStop = false;
            // 
            // lblValHighpassResonance
            // 
            lblValHighpassResonance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValHighpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValHighpassResonance.Location = new System.Drawing.Point(76, 114);
            lblValHighpassResonance.Name = "lblValHighpassResonance";
            lblValHighpassResonance.Size = new System.Drawing.Size(64, 16);
            lblValHighpassResonance.TabIndex = 5;
            lblValHighpassResonance.Text = "0.0";
            lblValHighpassResonance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            label21.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label21.Location = new System.Drawing.Point(76, 25);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(64, 16);
            label21.TabIndex = 4;
            label21.Text = "Resonance";
            label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobHighpassResonance
            // 
            KnobHighpassResonance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobHighpassResonance.BorderWidth = 2;
            KnobHighpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobHighpassResonance.HasTicks = true;
            KnobHighpassResonance.KnobColor = System.Drawing.SystemColors.Control;
            KnobHighpassResonance.LargeChange = 5;
            KnobHighpassResonance.Location = new System.Drawing.Point(76, 44);
            KnobHighpassResonance.Name = "KnobHighpassResonance";
            KnobHighpassResonance.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobHighpassResonance.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobHighpassResonance.PointerOffset = 4;
            KnobHighpassResonance.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobHighpassResonance.PointerWidth = 2;
            KnobHighpassResonance.Size = new System.Drawing.Size(55, 55);
            KnobHighpassResonance.TabIndex = 3;
            KnobHighpassResonance.Text = "Level";
            KnobHighpassResonance.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobHighpassResonance.ValueChanged += KnobHighpassResonance_ValueChanged;
            // 
            // lblValHighpassCutoff
            // 
            lblValHighpassCutoff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValHighpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValHighpassCutoff.Location = new System.Drawing.Point(6, 114);
            lblValHighpassCutoff.Name = "lblValHighpassCutoff";
            lblValHighpassCutoff.Size = new System.Drawing.Size(64, 16);
            lblValHighpassCutoff.TabIndex = 2;
            lblValHighpassCutoff.Text = "0.0";
            lblValHighpassCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            label23.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label23.Location = new System.Drawing.Point(6, 25);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(64, 16);
            label23.TabIndex = 2;
            label23.Text = "Cutoff";
            label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobHighpassCutoff
            // 
            KnobHighpassCutoff.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobHighpassCutoff.BorderWidth = 2;
            KnobHighpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobHighpassCutoff.HasTicks = true;
            KnobHighpassCutoff.KnobColor = System.Drawing.SystemColors.Control;
            KnobHighpassCutoff.LargeChange = 500;
            KnobHighpassCutoff.Location = new System.Drawing.Point(6, 44);
            KnobHighpassCutoff.Maximum = 22000;
            KnobHighpassCutoff.Minimum = 1;
            KnobHighpassCutoff.Name = "KnobHighpassCutoff";
            KnobHighpassCutoff.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobHighpassCutoff.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobHighpassCutoff.PointerOffset = 4;
            KnobHighpassCutoff.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobHighpassCutoff.PointerWidth = 2;
            KnobHighpassCutoff.Size = new System.Drawing.Size(55, 55);
            KnobHighpassCutoff.TabIndex = 1;
            KnobHighpassCutoff.Text = "Level";
            KnobHighpassCutoff.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobHighpassCutoff.ValueChanged += KnobHighpassCutoff_ValueChanged;
            // 
            // CheckHighpass
            // 
            CheckHighpass.AutoSize = true;
            CheckHighpass.BackColor = System.Drawing.SystemColors.Control;
            CheckHighpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckHighpass.Location = new System.Drawing.Point(6, 3);
            CheckHighpass.Name = "CheckHighpass";
            CheckHighpass.Size = new System.Drawing.Size(75, 19);
            CheckHighpass.TabIndex = 0;
            CheckHighpass.Text = "Highpass";
            CheckHighpass.UseVisualStyleBackColor = false;
            CheckHighpass.CheckedChanged += CheckHighpass_CheckedChanged;
            // 
            // cmbHighpassPreset
            // 
            cmbHighpassPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbHighpassPreset.Location = new System.Drawing.Point(6, 130);
            cmbHighpassPreset.Name = "cmbHighpassPreset";
            cmbHighpassPreset.Size = new System.Drawing.Size(150, 23);
            cmbHighpassPreset.TabIndex = 6;
            cmbHighpassPreset.SelectedIndexChanged += cmbHighpassPreset_SelectedIndexChanged;
            // 
            // btnHighpassPresetSave
            // 
            btnHighpassPresetSave.Location = new System.Drawing.Point(162, 130);
            btnHighpassPresetSave.Name = "btnHighpassPresetSave";
            btnHighpassPresetSave.Size = new System.Drawing.Size(50, 23);
            btnHighpassPresetSave.TabIndex = 7;
            btnHighpassPresetSave.Text = "保存";
            btnHighpassPresetSave.Click += BtnHighpassPresetSave_Click;
            // 
            // btnHighpassPresetDelete
            // 
            btnHighpassPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnHighpassPresetDelete.Name = "btnHighpassPresetDelete";
            btnHighpassPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnHighpassPresetDelete.TabIndex = 8;
            btnHighpassPresetDelete.Text = "削除";
            btnHighpassPresetDelete.Click += BtnHighpassPresetDelete_Click;
            // 
            // tabLowpass
            // 
            tabLowpass.BackColor = System.Drawing.Color.Transparent;
            tabLowpass.Controls.Add(GroupLowpass);
            tabLowpass.Location = new System.Drawing.Point(4, 24);
            tabLowpass.Name = "tabLowpass";
            tabLowpass.Size = new System.Drawing.Size(574, 392);
            tabLowpass.TabIndex = 5;
            tabLowpass.Text = "Lowpass";
            // 
            // GroupLowpass
            // 
            GroupLowpass.Controls.Add(lblValLowpassResonance);
            GroupLowpass.Controls.Add(label17);
            GroupLowpass.Controls.Add(KnobLowpassResonance);
            GroupLowpass.Controls.Add(lblValLowpassCutoff);
            GroupLowpass.Controls.Add(label19);
            GroupLowpass.Controls.Add(KnobLowpassCutoff);
            GroupLowpass.Controls.Add(CheckLowpass);
            GroupLowpass.Controls.Add(cmbLowpassPreset);
            GroupLowpass.Controls.Add(btnLowpassPresetSave);
            GroupLowpass.Controls.Add(btnLowpassPresetDelete);
            GroupLowpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupLowpass.Location = new System.Drawing.Point(3, 3);
            GroupLowpass.Name = "GroupLowpass";
            GroupLowpass.Size = new System.Drawing.Size(158, 145);
            GroupLowpass.TabIndex = 14;
            GroupLowpass.TabStop = false;
            // 
            // lblValLowpassResonance
            // 
            lblValLowpassResonance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValLowpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValLowpassResonance.Location = new System.Drawing.Point(76, 113);
            lblValLowpassResonance.Name = "lblValLowpassResonance";
            lblValLowpassResonance.Size = new System.Drawing.Size(64, 16);
            lblValLowpassResonance.TabIndex = 5;
            lblValLowpassResonance.Text = "0.0";
            lblValLowpassResonance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            label17.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label17.Location = new System.Drawing.Point(76, 24);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(64, 16);
            label17.TabIndex = 4;
            label17.Text = "Resonance";
            label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobLowpassResonance
            // 
            KnobLowpassResonance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobLowpassResonance.BorderWidth = 2;
            KnobLowpassResonance.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobLowpassResonance.HasTicks = true;
            KnobLowpassResonance.KnobColor = System.Drawing.SystemColors.Control;
            KnobLowpassResonance.LargeChange = 5;
            KnobLowpassResonance.Location = new System.Drawing.Point(76, 43);
            KnobLowpassResonance.Name = "KnobLowpassResonance";
            KnobLowpassResonance.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobLowpassResonance.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobLowpassResonance.PointerOffset = 4;
            KnobLowpassResonance.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobLowpassResonance.PointerWidth = 2;
            KnobLowpassResonance.Size = new System.Drawing.Size(55, 55);
            KnobLowpassResonance.TabIndex = 3;
            KnobLowpassResonance.Text = "Level";
            KnobLowpassResonance.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobLowpassResonance.ValueChanged += KnobLowpassResonance_ValueChanged;
            // 
            // lblValLowpassCutoff
            // 
            lblValLowpassCutoff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValLowpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValLowpassCutoff.Location = new System.Drawing.Point(6, 113);
            lblValLowpassCutoff.Name = "lblValLowpassCutoff";
            lblValLowpassCutoff.Size = new System.Drawing.Size(64, 16);
            lblValLowpassCutoff.TabIndex = 2;
            lblValLowpassCutoff.Text = "0.0";
            lblValLowpassCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            label19.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label19.Location = new System.Drawing.Point(6, 24);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(64, 16);
            label19.TabIndex = 2;
            label19.Text = "Cutoff";
            label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobLowpassCutoff
            // 
            KnobLowpassCutoff.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobLowpassCutoff.BorderWidth = 2;
            KnobLowpassCutoff.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobLowpassCutoff.HasTicks = true;
            KnobLowpassCutoff.KnobColor = System.Drawing.SystemColors.Control;
            KnobLowpassCutoff.LargeChange = 500;
            KnobLowpassCutoff.Location = new System.Drawing.Point(6, 43);
            KnobLowpassCutoff.Maximum = 22000;
            KnobLowpassCutoff.Minimum = 1;
            KnobLowpassCutoff.Name = "KnobLowpassCutoff";
            KnobLowpassCutoff.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobLowpassCutoff.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobLowpassCutoff.PointerOffset = 4;
            KnobLowpassCutoff.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobLowpassCutoff.PointerWidth = 2;
            KnobLowpassCutoff.Size = new System.Drawing.Size(55, 55);
            KnobLowpassCutoff.TabIndex = 1;
            KnobLowpassCutoff.Text = "Level";
            KnobLowpassCutoff.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobLowpassCutoff.ValueChanged += KnobLowpassCutoff_ValueChanged;
            // 
            // CheckLowpass
            // 
            CheckLowpass.AutoSize = true;
            CheckLowpass.BackColor = System.Drawing.SystemColors.Control;
            CheckLowpass.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckLowpass.Location = new System.Drawing.Point(6, 3);
            CheckLowpass.Name = "CheckLowpass";
            CheckLowpass.Size = new System.Drawing.Size(71, 19);
            CheckLowpass.TabIndex = 0;
            CheckLowpass.Text = "Lowpass";
            CheckLowpass.UseVisualStyleBackColor = false;
            CheckLowpass.CheckedChanged += CheckLowpass_CheckedChanged;
            // 
            // cmbLowpassPreset
            // 
            cmbLowpassPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbLowpassPreset.Location = new System.Drawing.Point(6, 130);
            cmbLowpassPreset.Name = "cmbLowpassPreset";
            cmbLowpassPreset.Size = new System.Drawing.Size(150, 23);
            cmbLowpassPreset.TabIndex = 6;
            cmbLowpassPreset.SelectedIndexChanged += cmbLowpassPreset_SelectedIndexChanged;
            // 
            // btnLowpassPresetSave
            // 
            btnLowpassPresetSave.Location = new System.Drawing.Point(162, 130);
            btnLowpassPresetSave.Name = "btnLowpassPresetSave";
            btnLowpassPresetSave.Size = new System.Drawing.Size(50, 23);
            btnLowpassPresetSave.TabIndex = 7;
            btnLowpassPresetSave.Text = "保存";
            btnLowpassPresetSave.Click += BtnLowpassPresetSave_Click;
            // 
            // btnLowpassPresetDelete
            // 
            btnLowpassPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnLowpassPresetDelete.Name = "btnLowpassPresetDelete";
            btnLowpassPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnLowpassPresetDelete.TabIndex = 8;
            btnLowpassPresetDelete.Text = "削除";
            btnLowpassPresetDelete.Click += BtnLowpassPresetDelete_Click;
            // 
            // tabCompressor
            // 
            tabCompressor.BackColor = System.Drawing.Color.Transparent;
            tabCompressor.Controls.Add(GroupCompressor);
            tabCompressor.Location = new System.Drawing.Point(4, 24);
            tabCompressor.Name = "tabCompressor";
            tabCompressor.Size = new System.Drawing.Size(574, 392);
            tabCompressor.TabIndex = 6;
            tabCompressor.Text = "Compressor";
            // 
            // GroupCompressor
            // 
            GroupCompressor.Controls.Add(CheckCompLinked);
            GroupCompressor.Controls.Add(lblValCompGain);
            GroupCompressor.Controls.Add(label39);
            GroupCompressor.Controls.Add(KnobCompGain);
            GroupCompressor.Controls.Add(lblValCompRelease);
            GroupCompressor.Controls.Add(label25);
            GroupCompressor.Controls.Add(KnobCompRelease);
            GroupCompressor.Controls.Add(lblValCompAttack);
            GroupCompressor.Controls.Add(label27);
            GroupCompressor.Controls.Add(KnobCompAttack);
            GroupCompressor.Controls.Add(lblValCompRatio);
            GroupCompressor.Controls.Add(label29);
            GroupCompressor.Controls.Add(KnobCompRatio);
            GroupCompressor.Controls.Add(lblValCompThreshold);
            GroupCompressor.Controls.Add(label31);
            GroupCompressor.Controls.Add(KnobCompThreshold);
            GroupCompressor.Controls.Add(CheckCompressor);
            GroupCompressor.Controls.Add(cmbCompressorPreset);
            GroupCompressor.Controls.Add(btnCompressorPresetSave);
            GroupCompressor.Controls.Add(btnCompressorPresetDelete);
            GroupCompressor.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupCompressor.Location = new System.Drawing.Point(3, 3);
            GroupCompressor.Name = "GroupCompressor";
            GroupCompressor.Size = new System.Drawing.Size(368, 160);
            GroupCompressor.TabIndex = 13;
            GroupCompressor.TabStop = false;
            // 
            // CheckCompLinked
            // 
            CheckCompLinked.AutoSize = true;
            CheckCompLinked.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckCompLinked.Location = new System.Drawing.Point(6, 133);
            CheckCompLinked.Name = "CheckCompLinked";
            CheckCompLinked.Size = new System.Drawing.Size(61, 19);
            CheckCompLinked.TabIndex = 16;
            CheckCompLinked.Text = "Linked";
            CheckCompLinked.UseVisualStyleBackColor = true;
            CheckCompLinked.Visible = false;
            CheckCompLinked.CheckedChanged += CheckCompLinked_CheckedChanged;
            // 
            // lblValCompGain
            // 
            lblValCompGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValCompGain.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValCompGain.Location = new System.Drawing.Point(286, 111);
            lblValCompGain.Name = "lblValCompGain";
            lblValCompGain.Size = new System.Drawing.Size(64, 16);
            lblValCompGain.TabIndex = 14;
            lblValCompGain.Text = "0.0";
            lblValCompGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label39
            // 
            label39.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label39.Location = new System.Drawing.Point(286, 22);
            label39.Name = "label39";
            label39.Size = new System.Drawing.Size(64, 16);
            label39.TabIndex = 13;
            label39.Text = "Gain";
            label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompGain
            // 
            KnobCompGain.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompGain.BorderWidth = 2;
            KnobCompGain.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobCompGain.HasTicks = true;
            KnobCompGain.KnobColor = System.Drawing.SystemColors.Control;
            KnobCompGain.LargeChange = 1;
            KnobCompGain.Location = new System.Drawing.Point(286, 41);
            KnobCompGain.Maximum = 30;
            KnobCompGain.Minimum = -30;
            KnobCompGain.Name = "KnobCompGain";
            KnobCompGain.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobCompGain.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompGain.PointerOffset = 4;
            KnobCompGain.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompGain.PointerWidth = 2;
            KnobCompGain.Size = new System.Drawing.Size(55, 55);
            KnobCompGain.TabIndex = 12;
            KnobCompGain.Text = "Level";
            KnobCompGain.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompGain.ValueChanged += KnobCompGain_ValueChanged;
            // 
            // lblValCompRelease
            // 
            lblValCompRelease.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValCompRelease.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValCompRelease.Location = new System.Drawing.Point(216, 111);
            lblValCompRelease.Name = "lblValCompRelease";
            lblValCompRelease.Size = new System.Drawing.Size(64, 16);
            lblValCompRelease.TabIndex = 11;
            lblValCompRelease.Text = "0.0";
            lblValCompRelease.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label25
            // 
            label25.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label25.Location = new System.Drawing.Point(216, 22);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(64, 16);
            label25.TabIndex = 10;
            label25.Text = "Release";
            label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompRelease
            // 
            KnobCompRelease.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompRelease.BorderWidth = 2;
            KnobCompRelease.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobCompRelease.HasTicks = true;
            KnobCompRelease.KnobColor = System.Drawing.SystemColors.Control;
            KnobCompRelease.LargeChange = 100;
            KnobCompRelease.Location = new System.Drawing.Point(216, 41);
            KnobCompRelease.Maximum = 5000;
            KnobCompRelease.Minimum = 10;
            KnobCompRelease.Name = "KnobCompRelease";
            KnobCompRelease.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobCompRelease.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompRelease.PointerOffset = 4;
            KnobCompRelease.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompRelease.PointerWidth = 2;
            KnobCompRelease.Size = new System.Drawing.Size(55, 55);
            KnobCompRelease.TabIndex = 9;
            KnobCompRelease.Text = "Level";
            KnobCompRelease.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompRelease.ValueChanged += KnobCompRelease_ValueChanged;
            // 
            // lblValCompAttack
            // 
            lblValCompAttack.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValCompAttack.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValCompAttack.Location = new System.Drawing.Point(146, 111);
            lblValCompAttack.Name = "lblValCompAttack";
            lblValCompAttack.Size = new System.Drawing.Size(64, 16);
            lblValCompAttack.TabIndex = 8;
            lblValCompAttack.Text = "0.0";
            lblValCompAttack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label27
            // 
            label27.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label27.Location = new System.Drawing.Point(146, 22);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(64, 16);
            label27.TabIndex = 7;
            label27.Text = "Attack";
            label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompAttack
            // 
            KnobCompAttack.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompAttack.BorderWidth = 2;
            KnobCompAttack.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobCompAttack.HasTicks = true;
            KnobCompAttack.KnobColor = System.Drawing.SystemColors.Control;
            KnobCompAttack.LargeChange = 100;
            KnobCompAttack.Location = new System.Drawing.Point(146, 41);
            KnobCompAttack.Maximum = 5000;
            KnobCompAttack.Minimum = 1;
            KnobCompAttack.Name = "KnobCompAttack";
            KnobCompAttack.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobCompAttack.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompAttack.PointerOffset = 4;
            KnobCompAttack.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompAttack.PointerWidth = 2;
            KnobCompAttack.Size = new System.Drawing.Size(55, 55);
            KnobCompAttack.TabIndex = 6;
            KnobCompAttack.Text = "Level";
            KnobCompAttack.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompAttack.ValueChanged += KnobCompAttack_ValueChanged;
            // 
            // lblValCompRatio
            // 
            lblValCompRatio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValCompRatio.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValCompRatio.Location = new System.Drawing.Point(76, 111);
            lblValCompRatio.Name = "lblValCompRatio";
            lblValCompRatio.Size = new System.Drawing.Size(64, 16);
            lblValCompRatio.TabIndex = 5;
            lblValCompRatio.Text = "0.0";
            lblValCompRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label29
            // 
            label29.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label29.Location = new System.Drawing.Point(76, 22);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(64, 16);
            label29.TabIndex = 4;
            label29.Text = "Ratio";
            label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompRatio
            // 
            KnobCompRatio.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompRatio.BorderWidth = 2;
            KnobCompRatio.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobCompRatio.HasTicks = true;
            KnobCompRatio.KnobColor = System.Drawing.SystemColors.Control;
            KnobCompRatio.LargeChange = 1;
            KnobCompRatio.Location = new System.Drawing.Point(76, 41);
            KnobCompRatio.Maximum = 50;
            KnobCompRatio.Minimum = 1;
            KnobCompRatio.Name = "KnobCompRatio";
            KnobCompRatio.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobCompRatio.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompRatio.PointerOffset = 4;
            KnobCompRatio.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompRatio.PointerWidth = 2;
            KnobCompRatio.Size = new System.Drawing.Size(55, 55);
            KnobCompRatio.TabIndex = 3;
            KnobCompRatio.Text = "Level";
            KnobCompRatio.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompRatio.ValueChanged += KnobCompRatio_ValueChanged;
            // 
            // lblValCompThreshold
            // 
            lblValCompThreshold.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValCompThreshold.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValCompThreshold.Location = new System.Drawing.Point(6, 111);
            lblValCompThreshold.Name = "lblValCompThreshold";
            lblValCompThreshold.Size = new System.Drawing.Size(64, 16);
            lblValCompThreshold.TabIndex = 2;
            lblValCompThreshold.Text = "0.0";
            lblValCompThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label31
            // 
            label31.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label31.Location = new System.Drawing.Point(6, 22);
            label31.Name = "label31";
            label31.Size = new System.Drawing.Size(64, 16);
            label31.TabIndex = 2;
            label31.Text = "Threshold";
            label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KnobCompThreshold
            // 
            KnobCompThreshold.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompThreshold.BorderWidth = 2;
            KnobCompThreshold.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobCompThreshold.HasTicks = true;
            KnobCompThreshold.KnobColor = System.Drawing.SystemColors.Control;
            KnobCompThreshold.LargeChange = 1;
            KnobCompThreshold.Location = new System.Drawing.Point(6, 41);
            KnobCompThreshold.Maximum = 0;
            KnobCompThreshold.Minimum = -60;
            KnobCompThreshold.Name = "KnobCompThreshold";
            KnobCompThreshold.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobCompThreshold.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompThreshold.PointerOffset = 4;
            KnobCompThreshold.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobCompThreshold.PointerWidth = 2;
            KnobCompThreshold.Size = new System.Drawing.Size(55, 55);
            KnobCompThreshold.TabIndex = 1;
            KnobCompThreshold.Text = "Level";
            KnobCompThreshold.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobCompThreshold.ValueChanged += KnobCompThreshold_ValueChanged;
            // 
            // CheckCompressor
            // 
            CheckCompressor.AutoSize = true;
            CheckCompressor.BackColor = System.Drawing.SystemColors.Control;
            CheckCompressor.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckCompressor.Location = new System.Drawing.Point(6, 0);
            CheckCompressor.Name = "CheckCompressor";
            CheckCompressor.Size = new System.Drawing.Size(88, 19);
            CheckCompressor.TabIndex = 0;
            CheckCompressor.Text = "Compressor";
            CheckCompressor.UseVisualStyleBackColor = false;
            CheckCompressor.CheckedChanged += CheckCompressor_CheckedChanged;
            // 
            // cmbCompressorPreset
            // 
            cmbCompressorPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCompressorPreset.Location = new System.Drawing.Point(6, 130);
            cmbCompressorPreset.Name = "cmbCompressorPreset";
            cmbCompressorPreset.Size = new System.Drawing.Size(150, 23);
            cmbCompressorPreset.TabIndex = 17;
            cmbCompressorPreset.SelectedIndexChanged += cmbCompressorPreset_SelectedIndexChanged;
            // 
            // btnCompressorPresetSave
            // 
            btnCompressorPresetSave.Location = new System.Drawing.Point(162, 130);
            btnCompressorPresetSave.Name = "btnCompressorPresetSave";
            btnCompressorPresetSave.Size = new System.Drawing.Size(50, 23);
            btnCompressorPresetSave.TabIndex = 18;
            btnCompressorPresetSave.Text = "保存";
            btnCompressorPresetSave.Click += BtnCompressorPresetSave_Click;
            // 
            // btnCompressorPresetDelete
            // 
            btnCompressorPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnCompressorPresetDelete.Name = "btnCompressorPresetDelete";
            btnCompressorPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnCompressorPresetDelete.TabIndex = 19;
            btnCompressorPresetDelete.Text = "削除";
            btnCompressorPresetDelete.Click += BtnCompressorPresetDelete_Click;
            // 
            // tabReverb
            // 
            tabReverb.BackColor = System.Drawing.Color.Transparent;
            tabReverb.Controls.Add(GroupReverb);
            tabReverb.Location = new System.Drawing.Point(4, 24);
            tabReverb.Name = "tabReverb";
            tabReverb.Size = new System.Drawing.Size(574, 392);
            tabReverb.TabIndex = 7;
            tabReverb.Text = "Reverb";
            // 
            // GroupReverb
            // 
            GroupReverb.Controls.Add(CheckReverb);
            GroupReverb.Controls.Add(lblReverbDecayTime);
            GroupReverb.Controls.Add(lblReverbEarlyDelay);
            GroupReverb.Controls.Add(lblReverbLateDelay);
            GroupReverb.Controls.Add(lblReverbHFRef);
            GroupReverb.Controls.Add(lblReverbHFDcRatio);
            GroupReverb.Controls.Add(lblReverbDiffusion);
            GroupReverb.Controls.Add(lblReverbDensity);
            GroupReverb.Controls.Add(lblValReverbDecayTime);
            GroupReverb.Controls.Add(lblValReverbEarlyDelay);
            GroupReverb.Controls.Add(lblValReverbLateDelay);
            GroupReverb.Controls.Add(lblValReverbHFRef);
            GroupReverb.Controls.Add(lblValReverbHFDcRatio);
            GroupReverb.Controls.Add(lblValReverbDiffusion);
            GroupReverb.Controls.Add(lblValReverbDensity);
            GroupReverb.Controls.Add(KnobReverbDecayTime);
            GroupReverb.Controls.Add(KnobReverbEarlyDelay);
            GroupReverb.Controls.Add(KnobReverbLateDelay);
            GroupReverb.Controls.Add(KnobReverbHFRef);
            GroupReverb.Controls.Add(KnobReverbHFDcRatio);
            GroupReverb.Controls.Add(KnobReverbDiffusion);
            GroupReverb.Controls.Add(KnobReverbDensity);
            GroupReverb.Controls.Add(lblReverbDivider);
            GroupReverb.Controls.Add(lblReverbLowShelfFreq);
            GroupReverb.Controls.Add(lblReverbLowShelfGain);
            GroupReverb.Controls.Add(lblReverbHighCut);
            GroupReverb.Controls.Add(lblReverbEarlyLate);
            GroupReverb.Controls.Add(lblReverbWet);
            GroupReverb.Controls.Add(lblReverbDry);
            GroupReverb.Controls.Add(lblValReverbLowShelfFreq);
            GroupReverb.Controls.Add(lblValReverbLowShelfGain);
            GroupReverb.Controls.Add(lblValReverbHighCut);
            GroupReverb.Controls.Add(lblValReverbEarlyLate);
            GroupReverb.Controls.Add(lblValReverbWet);
            GroupReverb.Controls.Add(lblValReverbDry);
            GroupReverb.Controls.Add(KnobReverbLowShelfFrequency);
            GroupReverb.Controls.Add(KnobReverbLowshelfGain);
            GroupReverb.Controls.Add(KnobReverbHighCut);
            GroupReverb.Controls.Add(KnobReverbEarlyLate);
            GroupReverb.Controls.Add(KnobReverbWet);
            GroupReverb.Controls.Add(KnobReverbDry);
            GroupReverb.Controls.Add(cmbReverbPreset);
            GroupReverb.Controls.Add(btnReverbPresetSave);
            GroupReverb.Controls.Add(btnReverbPresetDelete);
            GroupReverb.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            GroupReverb.Location = new System.Drawing.Point(3, 3);
            GroupReverb.Name = "GroupReverb";
            GroupReverb.Size = new System.Drawing.Size(552, 285);
            GroupReverb.TabIndex = 18;
            GroupReverb.TabStop = false;
            // 
            // CheckReverb
            // 
            CheckReverb.AutoSize = true;
            CheckReverb.BackColor = System.Drawing.SystemColors.Control;
            CheckReverb.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            CheckReverb.Location = new System.Drawing.Point(6, 2);
            CheckReverb.Name = "CheckReverb";
            CheckReverb.Size = new System.Drawing.Size(62, 19);
            CheckReverb.TabIndex = 0;
            CheckReverb.Text = "Reverb";
            CheckReverb.UseVisualStyleBackColor = false;
            CheckReverb.CheckedChanged += CheckReverb_CheckedChanged;
            // 
            // lblReverbDecayTime
            // 
            lblReverbDecayTime.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbDecayTime.Location = new System.Drawing.Point(9, 22);
            lblReverbDecayTime.Name = "lblReverbDecayTime";
            lblReverbDecayTime.Size = new System.Drawing.Size(72, 30);
            lblReverbDecayTime.TabIndex = 1;
            lblReverbDecayTime.Text = "Decay Time";
            lblReverbDecayTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbEarlyDelay
            // 
            lblReverbEarlyDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbEarlyDelay.Location = new System.Drawing.Point(87, 22);
            lblReverbEarlyDelay.Name = "lblReverbEarlyDelay";
            lblReverbEarlyDelay.Size = new System.Drawing.Size(72, 30);
            lblReverbEarlyDelay.TabIndex = 2;
            lblReverbEarlyDelay.Text = "Early Delay";
            lblReverbEarlyDelay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbLateDelay
            // 
            lblReverbLateDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbLateDelay.Location = new System.Drawing.Point(165, 22);
            lblReverbLateDelay.Name = "lblReverbLateDelay";
            lblReverbLateDelay.Size = new System.Drawing.Size(72, 30);
            lblReverbLateDelay.TabIndex = 3;
            lblReverbLateDelay.Text = "Late Delay";
            lblReverbLateDelay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbHFRef
            // 
            lblReverbHFRef.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbHFRef.Location = new System.Drawing.Point(243, 22);
            lblReverbHFRef.Name = "lblReverbHFRef";
            lblReverbHFRef.Size = new System.Drawing.Size(72, 30);
            lblReverbHFRef.TabIndex = 4;
            lblReverbHFRef.Text = "HF Reference";
            lblReverbHFRef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbHFDcRatio
            // 
            lblReverbHFDcRatio.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbHFDcRatio.Location = new System.Drawing.Point(321, 22);
            lblReverbHFDcRatio.Name = "lblReverbHFDcRatio";
            lblReverbHFDcRatio.Size = new System.Drawing.Size(72, 30);
            lblReverbHFDcRatio.TabIndex = 5;
            lblReverbHFDcRatio.Text = "HF Decay\nRatio";
            lblReverbHFDcRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbDiffusion
            // 
            lblReverbDiffusion.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbDiffusion.Location = new System.Drawing.Point(399, 22);
            lblReverbDiffusion.Name = "lblReverbDiffusion";
            lblReverbDiffusion.Size = new System.Drawing.Size(72, 30);
            lblReverbDiffusion.TabIndex = 6;
            lblReverbDiffusion.Text = "Diffusion";
            lblReverbDiffusion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbDensity
            // 
            lblReverbDensity.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbDensity.Location = new System.Drawing.Point(477, 22);
            lblReverbDensity.Name = "lblReverbDensity";
            lblReverbDensity.Size = new System.Drawing.Size(72, 30);
            lblReverbDensity.TabIndex = 7;
            lblReverbDensity.Text = "Density";
            lblReverbDensity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValReverbDecayTime
            // 
            lblValReverbDecayTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbDecayTime.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbDecayTime.Location = new System.Drawing.Point(14, 120);
            lblValReverbDecayTime.Name = "lblValReverbDecayTime";
            lblValReverbDecayTime.ReadOnly = true;
            lblValReverbDecayTime.Size = new System.Drawing.Size(62, 16);
            lblValReverbDecayTime.TabIndex = 8;
            lblValReverbDecayTime.Text = "1500";
            lblValReverbDecayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbEarlyDelay
            // 
            lblValReverbEarlyDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbEarlyDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbEarlyDelay.Location = new System.Drawing.Point(92, 120);
            lblValReverbEarlyDelay.Name = "lblValReverbEarlyDelay";
            lblValReverbEarlyDelay.ReadOnly = true;
            lblValReverbEarlyDelay.Size = new System.Drawing.Size(62, 16);
            lblValReverbEarlyDelay.TabIndex = 9;
            lblValReverbEarlyDelay.Text = "20";
            lblValReverbEarlyDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbLateDelay
            // 
            lblValReverbLateDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbLateDelay.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbLateDelay.Location = new System.Drawing.Point(170, 120);
            lblValReverbLateDelay.Name = "lblValReverbLateDelay";
            lblValReverbLateDelay.ReadOnly = true;
            lblValReverbLateDelay.Size = new System.Drawing.Size(62, 16);
            lblValReverbLateDelay.TabIndex = 10;
            lblValReverbLateDelay.Text = "40";
            lblValReverbLateDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbHFRef
            // 
            lblValReverbHFRef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbHFRef.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbHFRef.Location = new System.Drawing.Point(248, 120);
            lblValReverbHFRef.Name = "lblValReverbHFRef";
            lblValReverbHFRef.ReadOnly = true;
            lblValReverbHFRef.Size = new System.Drawing.Size(62, 16);
            lblValReverbHFRef.TabIndex = 11;
            lblValReverbHFRef.Text = "5000";
            lblValReverbHFRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbHFDcRatio
            // 
            lblValReverbHFDcRatio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbHFDcRatio.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbHFDcRatio.Location = new System.Drawing.Point(326, 120);
            lblValReverbHFDcRatio.Name = "lblValReverbHFDcRatio";
            lblValReverbHFDcRatio.ReadOnly = true;
            lblValReverbHFDcRatio.Size = new System.Drawing.Size(62, 16);
            lblValReverbHFDcRatio.TabIndex = 12;
            lblValReverbHFDcRatio.Text = "50";
            lblValReverbHFDcRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbDiffusion
            // 
            lblValReverbDiffusion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbDiffusion.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbDiffusion.Location = new System.Drawing.Point(404, 120);
            lblValReverbDiffusion.Name = "lblValReverbDiffusion";
            lblValReverbDiffusion.ReadOnly = true;
            lblValReverbDiffusion.Size = new System.Drawing.Size(62, 16);
            lblValReverbDiffusion.TabIndex = 13;
            lblValReverbDiffusion.Text = "50";
            lblValReverbDiffusion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbDensity
            // 
            lblValReverbDensity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbDensity.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbDensity.Location = new System.Drawing.Point(482, 120);
            lblValReverbDensity.Name = "lblValReverbDensity";
            lblValReverbDensity.ReadOnly = true;
            lblValReverbDensity.Size = new System.Drawing.Size(62, 16);
            lblValReverbDensity.TabIndex = 14;
            lblValReverbDensity.Text = "50";
            lblValReverbDensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // KnobReverbDecayTime
            // 
            KnobReverbDecayTime.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDecayTime.BorderWidth = 2;
            KnobReverbDecayTime.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbDecayTime.HasTicks = true;
            KnobReverbDecayTime.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbDecayTime.LargeChange = 500;
            KnobReverbDecayTime.Location = new System.Drawing.Point(14, 55);
            KnobReverbDecayTime.Maximum = 20000;
            KnobReverbDecayTime.Minimum = 100;
            KnobReverbDecayTime.Name = "KnobReverbDecayTime";
            KnobReverbDecayTime.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbDecayTime.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDecayTime.PointerOffset = 4;
            KnobReverbDecayTime.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDecayTime.PointerWidth = 2;
            KnobReverbDecayTime.Size = new System.Drawing.Size(62, 62);
            KnobReverbDecayTime.TabIndex = 1;
            KnobReverbDecayTime.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDecayTime.Value = 1500;
            KnobReverbDecayTime.ValueChanged += KnobReverbDecayTime_ValueChanged;
            // 
            // KnobReverbEarlyDelay
            // 
            KnobReverbEarlyDelay.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbEarlyDelay.BorderWidth = 2;
            KnobReverbEarlyDelay.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbEarlyDelay.HasTicks = true;
            KnobReverbEarlyDelay.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbEarlyDelay.LargeChange = 10;
            KnobReverbEarlyDelay.Location = new System.Drawing.Point(92, 55);
            KnobReverbEarlyDelay.Maximum = 300;
            KnobReverbEarlyDelay.Name = "KnobReverbEarlyDelay";
            KnobReverbEarlyDelay.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbEarlyDelay.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbEarlyDelay.PointerOffset = 4;
            KnobReverbEarlyDelay.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbEarlyDelay.PointerWidth = 2;
            KnobReverbEarlyDelay.Size = new System.Drawing.Size(62, 62);
            KnobReverbEarlyDelay.TabIndex = 2;
            KnobReverbEarlyDelay.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbEarlyDelay.Value = 20;
            KnobReverbEarlyDelay.ValueChanged += KnobReverbEarlyDelay_ValueChanged;
            // 
            // KnobReverbLateDelay
            // 
            KnobReverbLateDelay.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLateDelay.BorderWidth = 2;
            KnobReverbLateDelay.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbLateDelay.HasTicks = true;
            KnobReverbLateDelay.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbLateDelay.LargeChange = 5;
            KnobReverbLateDelay.Location = new System.Drawing.Point(170, 55);
            KnobReverbLateDelay.Name = "KnobReverbLateDelay";
            KnobReverbLateDelay.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbLateDelay.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLateDelay.PointerOffset = 4;
            KnobReverbLateDelay.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLateDelay.PointerWidth = 2;
            KnobReverbLateDelay.Size = new System.Drawing.Size(62, 62);
            KnobReverbLateDelay.TabIndex = 3;
            KnobReverbLateDelay.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLateDelay.Value = 40;
            KnobReverbLateDelay.ValueChanged += KnobReverbLateDelay_ValueChanged;
            // 
            // KnobReverbHFRef
            // 
            KnobReverbHFRef.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHFRef.BorderWidth = 2;
            KnobReverbHFRef.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbHFRef.HasTicks = true;
            KnobReverbHFRef.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbHFRef.LargeChange = 1000;
            KnobReverbHFRef.Location = new System.Drawing.Point(248, 55);
            KnobReverbHFRef.Maximum = 20000;
            KnobReverbHFRef.Minimum = 20;
            KnobReverbHFRef.Name = "KnobReverbHFRef";
            KnobReverbHFRef.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbHFRef.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHFRef.PointerOffset = 4;
            KnobReverbHFRef.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHFRef.PointerWidth = 2;
            KnobReverbHFRef.Size = new System.Drawing.Size(62, 62);
            KnobReverbHFRef.TabIndex = 4;
            KnobReverbHFRef.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHFRef.Value = 5000;
            KnobReverbHFRef.ValueChanged += KnobReverbHFRef_ValueChanged;
            // 
            // KnobReverbHFDcRatio
            // 
            KnobReverbHFDcRatio.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHFDcRatio.BorderWidth = 2;
            KnobReverbHFDcRatio.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbHFDcRatio.HasTicks = true;
            KnobReverbHFDcRatio.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbHFDcRatio.LargeChange = 5;
            KnobReverbHFDcRatio.Location = new System.Drawing.Point(326, 55);
            KnobReverbHFDcRatio.Minimum = 10;
            KnobReverbHFDcRatio.Name = "KnobReverbHFDcRatio";
            KnobReverbHFDcRatio.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbHFDcRatio.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHFDcRatio.PointerOffset = 4;
            KnobReverbHFDcRatio.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHFDcRatio.PointerWidth = 2;
            KnobReverbHFDcRatio.Size = new System.Drawing.Size(62, 62);
            KnobReverbHFDcRatio.TabIndex = 5;
            KnobReverbHFDcRatio.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHFDcRatio.Value = 50;
            KnobReverbHFDcRatio.ValueChanged += KnobReverbHFDcRatio_ValueChanged;
            // 
            // KnobReverbDiffusion
            // 
            KnobReverbDiffusion.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDiffusion.BorderWidth = 2;
            KnobReverbDiffusion.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbDiffusion.HasTicks = true;
            KnobReverbDiffusion.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbDiffusion.LargeChange = 5;
            KnobReverbDiffusion.Location = new System.Drawing.Point(404, 55);
            KnobReverbDiffusion.Minimum = 10;
            KnobReverbDiffusion.Name = "KnobReverbDiffusion";
            KnobReverbDiffusion.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbDiffusion.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDiffusion.PointerOffset = 4;
            KnobReverbDiffusion.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDiffusion.PointerWidth = 2;
            KnobReverbDiffusion.Size = new System.Drawing.Size(62, 62);
            KnobReverbDiffusion.TabIndex = 6;
            KnobReverbDiffusion.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDiffusion.Value = 50;
            KnobReverbDiffusion.ValueChanged += KnobReverbDiffusion_ValueChanged;
            // 
            // KnobReverbDensity
            // 
            KnobReverbDensity.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDensity.BorderWidth = 2;
            KnobReverbDensity.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbDensity.HasTicks = true;
            KnobReverbDensity.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbDensity.LargeChange = 5;
            KnobReverbDensity.Location = new System.Drawing.Point(482, 55);
            KnobReverbDensity.Minimum = 10;
            KnobReverbDensity.Name = "KnobReverbDensity";
            KnobReverbDensity.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbDensity.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDensity.PointerOffset = 4;
            KnobReverbDensity.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDensity.PointerWidth = 2;
            KnobReverbDensity.Size = new System.Drawing.Size(62, 62);
            KnobReverbDensity.TabIndex = 7;
            KnobReverbDensity.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDensity.Value = 50;
            KnobReverbDensity.ValueChanged += KnobReverbDensity_ValueChanged;
            // 
            // lblReverbDivider
            // 
            lblReverbDivider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblReverbDivider.Location = new System.Drawing.Point(6, 142);
            lblReverbDivider.Name = "lblReverbDivider";
            lblReverbDivider.Size = new System.Drawing.Size(540, 1);
            lblReverbDivider.TabIndex = 99;
            // 
            // lblReverbLowShelfFreq
            // 
            lblReverbLowShelfFreq.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbLowShelfFreq.Location = new System.Drawing.Point(45, 157);
            lblReverbLowShelfFreq.Name = "lblReverbLowShelfFreq";
            lblReverbLowShelfFreq.Size = new System.Drawing.Size(72, 30);
            lblReverbLowShelfFreq.TabIndex = 100;
            lblReverbLowShelfFreq.Text = "Low Shelf\nFreq";
            lblReverbLowShelfFreq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbLowShelfGain
            // 
            lblReverbLowShelfGain.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbLowShelfGain.Location = new System.Drawing.Point(123, 157);
            lblReverbLowShelfGain.Name = "lblReverbLowShelfGain";
            lblReverbLowShelfGain.Size = new System.Drawing.Size(72, 30);
            lblReverbLowShelfGain.TabIndex = 101;
            lblReverbLowShelfGain.Text = "Low Shelf\nGain";
            lblReverbLowShelfGain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbHighCut
            // 
            lblReverbHighCut.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbHighCut.Location = new System.Drawing.Point(201, 157);
            lblReverbHighCut.Name = "lblReverbHighCut";
            lblReverbHighCut.Size = new System.Drawing.Size(72, 30);
            lblReverbHighCut.TabIndex = 102;
            lblReverbHighCut.Text = "High Cut";
            lblReverbHighCut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbEarlyLate
            // 
            lblReverbEarlyLate.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbEarlyLate.Location = new System.Drawing.Point(279, 157);
            lblReverbEarlyLate.Name = "lblReverbEarlyLate";
            lblReverbEarlyLate.Size = new System.Drawing.Size(72, 30);
            lblReverbEarlyLate.TabIndex = 103;
            lblReverbEarlyLate.Text = "Early/Late\nMix";
            lblReverbEarlyLate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbWet
            // 
            lblReverbWet.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbWet.Location = new System.Drawing.Point(357, 157);
            lblReverbWet.Name = "lblReverbWet";
            lblReverbWet.Size = new System.Drawing.Size(72, 30);
            lblReverbWet.TabIndex = 104;
            lblReverbWet.Text = "Wet Level";
            lblReverbWet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReverbDry
            // 
            lblReverbDry.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblReverbDry.Location = new System.Drawing.Point(435, 157);
            lblReverbDry.Name = "lblReverbDry";
            lblReverbDry.Size = new System.Drawing.Size(72, 30);
            lblReverbDry.TabIndex = 105;
            lblReverbDry.Text = "Dry Level";
            lblReverbDry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValReverbLowShelfFreq
            // 
            lblValReverbLowShelfFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbLowShelfFreq.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbLowShelfFreq.Location = new System.Drawing.Point(50, 255);
            lblValReverbLowShelfFreq.Name = "lblValReverbLowShelfFreq";
            lblValReverbLowShelfFreq.ReadOnly = true;
            lblValReverbLowShelfFreq.Size = new System.Drawing.Size(62, 16);
            lblValReverbLowShelfFreq.TabIndex = 106;
            lblValReverbLowShelfFreq.Text = "250";
            lblValReverbLowShelfFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbLowShelfGain
            // 
            lblValReverbLowShelfGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbLowShelfGain.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbLowShelfGain.Location = new System.Drawing.Point(128, 255);
            lblValReverbLowShelfGain.Name = "lblValReverbLowShelfGain";
            lblValReverbLowShelfGain.ReadOnly = true;
            lblValReverbLowShelfGain.Size = new System.Drawing.Size(62, 16);
            lblValReverbLowShelfGain.TabIndex = 107;
            lblValReverbLowShelfGain.Text = "0";
            lblValReverbLowShelfGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbHighCut
            // 
            lblValReverbHighCut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbHighCut.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbHighCut.Location = new System.Drawing.Point(206, 255);
            lblValReverbHighCut.Name = "lblValReverbHighCut";
            lblValReverbHighCut.ReadOnly = true;
            lblValReverbHighCut.Size = new System.Drawing.Size(62, 16);
            lblValReverbHighCut.TabIndex = 108;
            lblValReverbHighCut.Text = "20000";
            lblValReverbHighCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbEarlyLate
            // 
            lblValReverbEarlyLate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbEarlyLate.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbEarlyLate.Location = new System.Drawing.Point(284, 255);
            lblValReverbEarlyLate.Name = "lblValReverbEarlyLate";
            lblValReverbEarlyLate.ReadOnly = true;
            lblValReverbEarlyLate.Size = new System.Drawing.Size(62, 16);
            lblValReverbEarlyLate.TabIndex = 109;
            lblValReverbEarlyLate.Text = "50";
            lblValReverbEarlyLate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbWet
            // 
            lblValReverbWet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbWet.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbWet.Location = new System.Drawing.Point(362, 255);
            lblValReverbWet.Name = "lblValReverbWet";
            lblValReverbWet.ReadOnly = true;
            lblValReverbWet.Size = new System.Drawing.Size(62, 16);
            lblValReverbWet.TabIndex = 110;
            lblValReverbWet.Text = "-6";
            lblValReverbWet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValReverbDry
            // 
            lblValReverbDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblValReverbDry.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblValReverbDry.Location = new System.Drawing.Point(440, 255);
            lblValReverbDry.Name = "lblValReverbDry";
            lblValReverbDry.ReadOnly = true;
            lblValReverbDry.Size = new System.Drawing.Size(62, 16);
            lblValReverbDry.TabIndex = 111;
            lblValReverbDry.Text = "0";
            lblValReverbDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // KnobReverbLowShelfFrequency
            // 
            KnobReverbLowShelfFrequency.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLowShelfFrequency.BorderWidth = 2;
            KnobReverbLowShelfFrequency.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbLowShelfFrequency.HasTicks = true;
            KnobReverbLowShelfFrequency.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbLowShelfFrequency.LargeChange = 50;
            KnobReverbLowShelfFrequency.Location = new System.Drawing.Point(50, 190);
            KnobReverbLowShelfFrequency.Maximum = 1000;
            KnobReverbLowShelfFrequency.Minimum = 20;
            KnobReverbLowShelfFrequency.Name = "KnobReverbLowShelfFrequency";
            KnobReverbLowShelfFrequency.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbLowShelfFrequency.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLowShelfFrequency.PointerOffset = 4;
            KnobReverbLowShelfFrequency.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLowShelfFrequency.PointerWidth = 2;
            KnobReverbLowShelfFrequency.Size = new System.Drawing.Size(62, 62);
            KnobReverbLowShelfFrequency.TabIndex = 8;
            KnobReverbLowShelfFrequency.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLowShelfFrequency.Value = 250;
            KnobReverbLowShelfFrequency.ValueChanged += KnobReverbLowShelfFrequency_ValueChanged;
            // 
            // KnobReverbLowshelfGain
            // 
            KnobReverbLowshelfGain.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLowshelfGain.BorderWidth = 2;
            KnobReverbLowshelfGain.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbLowshelfGain.HasTicks = true;
            KnobReverbLowshelfGain.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbLowshelfGain.LargeChange = 2;
            KnobReverbLowshelfGain.Location = new System.Drawing.Point(128, 190);
            KnobReverbLowshelfGain.Maximum = 12;
            KnobReverbLowshelfGain.Minimum = -36;
            KnobReverbLowshelfGain.Name = "KnobReverbLowshelfGain";
            KnobReverbLowshelfGain.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbLowshelfGain.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLowshelfGain.PointerOffset = 4;
            KnobReverbLowshelfGain.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbLowshelfGain.PointerWidth = 2;
            KnobReverbLowshelfGain.Size = new System.Drawing.Size(62, 62);
            KnobReverbLowshelfGain.TabIndex = 9;
            KnobReverbLowshelfGain.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbLowshelfGain.ValueChanged += KnobReverbLowshelfGain_ValueChanged;
            // 
            // KnobReverbHighCut
            // 
            KnobReverbHighCut.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHighCut.BorderWidth = 2;
            KnobReverbHighCut.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbHighCut.HasTicks = true;
            KnobReverbHighCut.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbHighCut.LargeChange = 500;
            KnobReverbHighCut.Location = new System.Drawing.Point(206, 190);
            KnobReverbHighCut.Maximum = 20000;
            KnobReverbHighCut.Minimum = 20;
            KnobReverbHighCut.Name = "KnobReverbHighCut";
            KnobReverbHighCut.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbHighCut.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHighCut.PointerOffset = 4;
            KnobReverbHighCut.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbHighCut.PointerWidth = 2;
            KnobReverbHighCut.Size = new System.Drawing.Size(62, 62);
            KnobReverbHighCut.TabIndex = 10;
            KnobReverbHighCut.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbHighCut.Value = 20000;
            KnobReverbHighCut.ValueChanged += KnobReverbHighCut_ValueChanged;
            // 
            // KnobReverbEarlyLate
            // 
            KnobReverbEarlyLate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbEarlyLate.BorderWidth = 2;
            KnobReverbEarlyLate.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbEarlyLate.HasTicks = true;
            KnobReverbEarlyLate.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbEarlyLate.LargeChange = 5;
            KnobReverbEarlyLate.Location = new System.Drawing.Point(284, 190);
            KnobReverbEarlyLate.Name = "KnobReverbEarlyLate";
            KnobReverbEarlyLate.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbEarlyLate.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbEarlyLate.PointerOffset = 4;
            KnobReverbEarlyLate.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbEarlyLate.PointerWidth = 2;
            KnobReverbEarlyLate.Size = new System.Drawing.Size(62, 62);
            KnobReverbEarlyLate.TabIndex = 11;
            KnobReverbEarlyLate.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbEarlyLate.Value = 50;
            KnobReverbEarlyLate.ValueChanged += KnobReverbEarlyLate_ValueChanged;
            // 
            // KnobReverbWet
            // 
            KnobReverbWet.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbWet.BorderWidth = 2;
            KnobReverbWet.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbWet.HasTicks = true;
            KnobReverbWet.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbWet.LargeChange = 5;
            KnobReverbWet.Location = new System.Drawing.Point(362, 190);
            KnobReverbWet.Maximum = 20;
            KnobReverbWet.Minimum = -80;
            KnobReverbWet.Name = "KnobReverbWet";
            KnobReverbWet.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbWet.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbWet.PointerOffset = 4;
            KnobReverbWet.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbWet.PointerWidth = 2;
            KnobReverbWet.Size = new System.Drawing.Size(62, 62);
            KnobReverbWet.TabIndex = 12;
            KnobReverbWet.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbWet.Value = -6;
            KnobReverbWet.ValueChanged += KnobReverbWet_ValueChanged;
            // 
            // KnobReverbDry
            // 
            KnobReverbDry.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDry.BorderWidth = 2;
            KnobReverbDry.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            KnobReverbDry.HasTicks = true;
            KnobReverbDry.KnobColor = System.Drawing.SystemColors.Control;
            KnobReverbDry.LargeChange = 5;
            KnobReverbDry.Location = new System.Drawing.Point(440, 190);
            KnobReverbDry.Maximum = 20;
            KnobReverbDry.Minimum = -80;
            KnobReverbDry.Name = "KnobReverbDry";
            KnobReverbDry.PointerColor = System.Drawing.SystemColors.ControlText;
            KnobReverbDry.PointerEndCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDry.PointerOffset = 4;
            KnobReverbDry.PointerStartCap = System.Drawing.Drawing2D.LineCap.Flat;
            KnobReverbDry.PointerWidth = 2;
            KnobReverbDry.Size = new System.Drawing.Size(62, 62);
            KnobReverbDry.TabIndex = 13;
            KnobReverbDry.TickColor = System.Drawing.SystemColors.ControlDarkDark;
            KnobReverbDry.ValueChanged += KnobReverbDry_ValueChanged;
            // 
            // cmbReverbPreset
            // 
            cmbReverbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbReverbPreset.Location = new System.Drawing.Point(6, 130);
            cmbReverbPreset.Name = "cmbReverbPreset";
            cmbReverbPreset.Size = new System.Drawing.Size(150, 23);
            cmbReverbPreset.TabIndex = 112;
            cmbReverbPreset.SelectedIndexChanged += cmbReverbPreset_SelectedIndexChanged;
            // 
            // btnReverbPresetSave
            // 
            btnReverbPresetSave.Location = new System.Drawing.Point(162, 130);
            btnReverbPresetSave.Name = "btnReverbPresetSave";
            btnReverbPresetSave.Size = new System.Drawing.Size(50, 23);
            btnReverbPresetSave.TabIndex = 113;
            btnReverbPresetSave.Text = "保存";
            btnReverbPresetSave.Click += BtnReverbPresetSave_Click;
            // 
            // btnReverbPresetDelete
            // 
            btnReverbPresetDelete.Location = new System.Drawing.Point(218, 130);
            btnReverbPresetDelete.Name = "btnReverbPresetDelete";
            btnReverbPresetDelete.Size = new System.Drawing.Size(50, 23);
            btnReverbPresetDelete.TabIndex = 114;
            btnReverbPresetDelete.Text = "削除";
            btnReverbPresetDelete.Click += BtnReverbPresetDelete_Click;
            // 
            // tabSkin
            // 
            tabSkin.BackColor = System.Drawing.Color.Transparent;
            tabSkin.Controls.Add(lblSkinPath);
            tabSkin.Controls.Add(txtSkinPath);
            tabSkin.Controls.Add(lblSkinName);
            tabSkin.Controls.Add(lblSkinAuthorLabel);
            tabSkin.Controls.Add(lblSkinAuthor);
            tabSkin.Controls.Add(lblSkinDescLabel);
            tabSkin.Controls.Add(lblSkinDesc);
            tabSkin.Controls.Add(BtnSkinBrowse);
            tabSkin.Controls.Add(PictSkinPreview);
            tabSkin.Controls.Add(BtnSkinApply);
            tabSkin.Location = new System.Drawing.Point(4, 24);
            tabSkin.Name = "tabSkin";
            tabSkin.Size = new System.Drawing.Size(574, 392);
            tabSkin.TabIndex = 12;
            tabSkin.Text = "スキン";
            // 
            // lblSkinPath
            // 
            lblSkinPath.AutoSize = true;
            lblSkinPath.Location = new System.Drawing.Point(12, 14);
            lblSkinPath.Name = "lblSkinPath";
            lblSkinPath.Size = new System.Drawing.Size(71, 15);
            lblSkinPath.TabIndex = 0;
            lblSkinPath.Text = "スキンファイル:";
            // 
            // txtSkinPath
            // 
            txtSkinPath.Location = new System.Drawing.Point(12, 32);
            txtSkinPath.Name = "txtSkinPath";
            txtSkinPath.ReadOnly = true;
            txtSkinPath.Size = new System.Drawing.Size(450, 23);
            txtSkinPath.TabIndex = 0;
            // 
            // lblSkinName
            // 
            lblSkinName.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Bold);
            lblSkinName.Location = new System.Drawing.Point(12, 266);
            lblSkinName.Name = "lblSkinName";
            lblSkinName.Size = new System.Drawing.Size(440, 25);
            lblSkinName.TabIndex = 1;
            // 
            // lblSkinAuthorLabel
            // 
            lblSkinAuthorLabel.AutoSize = true;
            lblSkinAuthorLabel.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblSkinAuthorLabel.Location = new System.Drawing.Point(12, 307);
            lblSkinAuthorLabel.Name = "lblSkinAuthorLabel";
            lblSkinAuthorLabel.Size = new System.Drawing.Size(47, 15);
            lblSkinAuthorLabel.TabIndex = 2;
            lblSkinAuthorLabel.Text = "Author:";
            // 
            // lblSkinAuthor
            // 
            lblSkinAuthor.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblSkinAuthor.Location = new System.Drawing.Point(70, 307);
            lblSkinAuthor.Name = "lblSkinAuthor";
            lblSkinAuthor.Size = new System.Drawing.Size(380, 18);
            lblSkinAuthor.TabIndex = 3;
            // 
            // lblSkinDescLabel
            // 
            lblSkinDescLabel.AutoSize = true;
            lblSkinDescLabel.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblSkinDescLabel.Location = new System.Drawing.Point(12, 328);
            lblSkinDescLabel.Name = "lblSkinDescLabel";
            lblSkinDescLabel.Size = new System.Drawing.Size(70, 15);
            lblSkinDescLabel.TabIndex = 4;
            lblSkinDescLabel.Text = "Description:";
            // 
            // lblSkinDesc
            // 
            lblSkinDesc.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblSkinDesc.Location = new System.Drawing.Point(90, 328);
            lblSkinDesc.Name = "lblSkinDesc";
            lblSkinDesc.Size = new System.Drawing.Size(460, 18);
            lblSkinDesc.TabIndex = 5;
            // 
            // BtnSkinBrowse
            // 
            BtnSkinBrowse.Location = new System.Drawing.Point(470, 31);
            BtnSkinBrowse.Name = "BtnSkinBrowse";
            BtnSkinBrowse.Size = new System.Drawing.Size(90, 25);
            BtnSkinBrowse.TabIndex = 1;
            BtnSkinBrowse.Text = "参照...";
            BtnSkinBrowse.UseVisualStyleBackColor = true;
            BtnSkinBrowse.Click += BtnSkinBrowse_Click;
            // 
            // PictSkinPreview
            // 
            PictSkinPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            PictSkinPreview.Location = new System.Drawing.Point(12, 65);
            PictSkinPreview.Name = "PictSkinPreview";
            PictSkinPreview.Size = new System.Drawing.Size(548, 198);
            PictSkinPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            PictSkinPreview.TabIndex = 2;
            PictSkinPreview.TabStop = false;
            // 
            // BtnSkinApply
            // 
            BtnSkinApply.Location = new System.Drawing.Point(471, 358);
            BtnSkinApply.Name = "BtnSkinApply";
            BtnSkinApply.Size = new System.Drawing.Size(90, 25);
            BtnSkinApply.TabIndex = 2;
            BtnSkinApply.Text = "適用";
            BtnSkinApply.UseVisualStyleBackColor = true;
            BtnSkinApply.Click += BtnSkinApply_Click;
            // 
            // tabAbout
            // 
            tabAbout.BackColor = System.Drawing.Color.Transparent;
            tabAbout.Controls.Add(lblAboutAppName);
            tabAbout.Controls.Add(lblAboutVersion);
            tabAbout.Controls.Add(lblAboutCopyright);
            tabAbout.Controls.Add(lblAboutCompany);
            tabAbout.Controls.Add(lnkAboutGitHub);
            tabAbout.Location = new System.Drawing.Point(4, 24);
            tabAbout.Name = "tabAbout";
            tabAbout.Size = new System.Drawing.Size(574, 392);
            tabAbout.TabIndex = 13;
            tabAbout.Text = "About";
            // 
            // lblAboutAppName
            // 
            lblAboutAppName.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            lblAboutAppName.Location = new System.Drawing.Point(20, 40);
            lblAboutAppName.Name = "lblAboutAppName";
            lblAboutAppName.Size = new System.Drawing.Size(534, 36);
            lblAboutAppName.TabIndex = 0;
            lblAboutAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAboutVersion
            // 
            lblAboutVersion.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            lblAboutVersion.Location = new System.Drawing.Point(20, 85);
            lblAboutVersion.Name = "lblAboutVersion";
            lblAboutVersion.Size = new System.Drawing.Size(534, 24);
            lblAboutVersion.TabIndex = 1;
            lblAboutVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAboutCopyright
            // 
            lblAboutCopyright.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblAboutCopyright.Location = new System.Drawing.Point(20, 118);
            lblAboutCopyright.Name = "lblAboutCopyright";
            lblAboutCopyright.Size = new System.Drawing.Size(534, 20);
            lblAboutCopyright.TabIndex = 2;
            lblAboutCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAboutCompany
            // 
            lblAboutCompany.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lblAboutCompany.Location = new System.Drawing.Point(20, 140);
            lblAboutCompany.Name = "lblAboutCompany";
            lblAboutCompany.Size = new System.Drawing.Size(534, 20);
            lblAboutCompany.TabIndex = 3;
            lblAboutCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkAboutGitHub
            // 
            lnkAboutGitHub.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            lnkAboutGitHub.Location = new System.Drawing.Point(20, 170);
            lnkAboutGitHub.Name = "lnkAboutGitHub";
            lnkAboutGitHub.Size = new System.Drawing.Size(534, 20);
            lnkAboutGitHub.TabIndex = 4;
            lnkAboutGitHub.TabStop = true;
            lnkAboutGitHub.Text = "https://github.com/AoiKagase/MediaPlayer-X-Ark";
            lnkAboutGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lnkAboutGitHub.LinkClicked += lnkAboutGitHub_LinkClicked;
            // 
            // textBox13
            // 
            textBox13.Location = new System.Drawing.Point(0, 0);
            textBox13.Name = "textBox13";
            textBox13.Size = new System.Drawing.Size(100, 23);
            textBox13.TabIndex = 0;
            // 
            // label34
            // 
            label34.Location = new System.Drawing.Point(0, 0);
            label34.Name = "label34";
            label34.Size = new System.Drawing.Size(100, 23);
            label34.TabIndex = 0;
            // 
            // textBox12
            // 
            textBox12.Location = new System.Drawing.Point(0, 0);
            textBox12.Name = "textBox12";
            textBox12.Size = new System.Drawing.Size(100, 23);
            textBox12.TabIndex = 0;
            // 
            // label32
            // 
            label32.Location = new System.Drawing.Point(0, 0);
            label32.Name = "label32";
            label32.Size = new System.Drawing.Size(100, 23);
            label32.TabIndex = 0;
            // 
            // textBox11
            // 
            textBox11.Location = new System.Drawing.Point(0, 0);
            textBox11.Name = "textBox11";
            textBox11.Size = new System.Drawing.Size(100, 23);
            textBox11.TabIndex = 0;
            // 
            // label30
            // 
            label30.Location = new System.Drawing.Point(0, 0);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(100, 23);
            label30.TabIndex = 0;
            // 
            // textBox10
            // 
            textBox10.Location = new System.Drawing.Point(0, 0);
            textBox10.Name = "textBox10";
            textBox10.Size = new System.Drawing.Size(100, 23);
            textBox10.TabIndex = 0;
            // 
            // label28
            // 
            label28.Location = new System.Drawing.Point(0, 0);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(100, 23);
            label28.TabIndex = 0;
            // 
            // textBox9
            // 
            textBox9.Location = new System.Drawing.Point(0, 0);
            textBox9.Name = "textBox9";
            textBox9.Size = new System.Drawing.Size(100, 23);
            textBox9.TabIndex = 0;
            // 
            // label26
            // 
            label26.Location = new System.Drawing.Point(0, 0);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(100, 23);
            label26.TabIndex = 0;
            // 
            // textBox8
            // 
            textBox8.Location = new System.Drawing.Point(0, 0);
            textBox8.Name = "textBox8";
            textBox8.Size = new System.Drawing.Size(100, 23);
            textBox8.TabIndex = 0;
            // 
            // label24
            // 
            label24.Location = new System.Drawing.Point(0, 0);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(100, 23);
            label24.TabIndex = 0;
            // 
            // textBox7
            // 
            textBox7.Location = new System.Drawing.Point(0, 0);
            textBox7.Name = "textBox7";
            textBox7.Size = new System.Drawing.Size(100, 23);
            textBox7.TabIndex = 0;
            // 
            // label22
            // 
            label22.Location = new System.Drawing.Point(0, 0);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(100, 23);
            label22.TabIndex = 0;
            // 
            // textBox6
            // 
            textBox6.Location = new System.Drawing.Point(0, 0);
            textBox6.Name = "textBox6";
            textBox6.Size = new System.Drawing.Size(100, 23);
            textBox6.TabIndex = 0;
            // 
            // label20
            // 
            label20.Location = new System.Drawing.Point(0, 0);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(100, 23);
            label20.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(0, 0);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(100, 23);
            textBox1.TabIndex = 0;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(0, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(100, 23);
            label10.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(0, 0);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(100, 23);
            textBox2.TabIndex = 0;
            // 
            // label12
            // 
            label12.Location = new System.Drawing.Point(0, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(100, 23);
            label12.TabIndex = 0;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(0, 0);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(100, 23);
            textBox3.TabIndex = 0;
            // 
            // label14
            // 
            label14.Location = new System.Drawing.Point(0, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(100, 23);
            label14.TabIndex = 0;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(0, 0);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(100, 23);
            textBox4.TabIndex = 0;
            // 
            // label16
            // 
            label16.Location = new System.Drawing.Point(0, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(100, 23);
            label16.TabIndex = 0;
            // 
            // textBox5
            // 
            textBox5.Location = new System.Drawing.Point(0, 0);
            textBox5.Name = "textBox5";
            textBox5.Size = new System.Drawing.Size(100, 23);
            textBox5.TabIndex = 0;
            // 
            // label18
            // 
            label18.Location = new System.Drawing.Point(0, 0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(100, 23);
            label18.TabIndex = 0;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(781, 420);
            Controls.Add(tabControlEffects);
            Controls.Add(TreeMenu);
            DoubleBuffered = true;
            Name = "OptionsForm";
            Text = "Options";
            FormClosing += OptionsForm_FormClosing;
            Load += OptionsForm_Load;
            tabControlEffects.ResumeLayout(false);
            tabSetting.ResumeLayout(false);
            tabSetting.PerformLayout();
            tabGEqualizer.ResumeLayout(false);
            GroupGEQ.ResumeLayout(false);
            GroupGEQ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictGEQGraph).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ60).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ32).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ125).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ250).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ500).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ1K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ2K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ4K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ8K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ16K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ20K).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrkGEQ22K).EndInit();
            tabPitch.ResumeLayout(false);
            GroupSpeed.ResumeLayout(false);
            GroupSpeed.PerformLayout();
            GroupFrequency.ResumeLayout(false);
            GroupFrequency.PerformLayout();
            GroupPitchShift.ResumeLayout(false);
            GroupPitchShift.PerformLayout();
            tabDistortion.ResumeLayout(false);
            GroupDistortion.ResumeLayout(false);
            GroupDistortion.PerformLayout();
            tabChorus.ResumeLayout(false);
            GroupChorus.ResumeLayout(false);
            GroupChorus.PerformLayout();
            tabEcho.ResumeLayout(false);
            GroupEcho.ResumeLayout(false);
            GroupEcho.PerformLayout();
            tabFlanger.ResumeLayout(false);
            GroupFlanger.ResumeLayout(false);
            GroupFlanger.PerformLayout();
            tabHightpass.ResumeLayout(false);
            GroupHighpass.ResumeLayout(false);
            GroupHighpass.PerformLayout();
            tabLowpass.ResumeLayout(false);
            GroupLowpass.ResumeLayout(false);
            GroupLowpass.PerformLayout();
            tabCompressor.ResumeLayout(false);
            GroupCompressor.ResumeLayout(false);
            GroupCompressor.PerformLayout();
            tabReverb.ResumeLayout(false);
            GroupReverb.ResumeLayout(false);
            GroupReverb.PerformLayout();
            tabSkin.ResumeLayout(false);
            tabSkin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictSkinPreview).EndInit();
            tabAbout.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabSkin;
		private System.Windows.Forms.Label lblSkinPath;
		private System.Windows.Forms.TextBox txtSkinPath;
		private System.Windows.Forms.Button BtnSkinBrowse;
		private System.Windows.Forms.PictureBox PictSkinPreview;
		private System.Windows.Forms.Button BtnSkinApply; 
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.Label label51;
		private System.Windows.Forms.Label label52;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.Label label54;
		private System.Windows.Forms.Label label55;
		private System.Windows.Forms.Label label56;

		private System.Windows.Forms.TreeView TreeMenu;
        private System.Windows.Forms.TabControl tabControlEffects;
        private System.Windows.Forms.TabPage tabDistortion;
        private System.Windows.Forms.TabPage tabChorus;
        private System.Windows.Forms.GroupBox GroupChorus;
        private System.Windows.Forms.TextBox lblValChorusDepth;
        private UI.Knob KnobChorusDepth;
        private System.Windows.Forms.TextBox lblValChorusRate;
		private UI.Knob KnobChorusRate;
        private System.Windows.Forms.TextBox lblValChorusMix;
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
        private UI.Knob KnobSpeed;
        private System.Windows.Forms.CheckBox CheckSpeed;
        private System.Windows.Forms.GroupBox GroupFrequency;
        private System.Windows.Forms.TextBox lblValFrequency;
        private UI.Knob KnobFrequency;
        private System.Windows.Forms.CheckBox CheckFrequency;
        private System.Windows.Forms.GroupBox GroupPitchShift;
        private System.Windows.Forms.TextBox lblValPitchFFT;
        private UI.Knob KnobPitchFFT;
        private System.Windows.Forms.TextBox lblValPitchPitch;
		private UI.Knob KnobPitchPitch;
        private System.Windows.Forms.CheckBox CheckPitch;
        private System.Windows.Forms.GroupBox GroupEcho;
        private System.Windows.Forms.TextBox lblValEchoWet;
        private UI.Knob KnobEchoWet;
        private System.Windows.Forms.TextBox lblValEchoDry;
		private UI.Knob KnobEchoDry;
        private System.Windows.Forms.TextBox lblValEchoFeedback;
        private UI.Knob KnobEchoFeedback;
        private System.Windows.Forms.TextBox lblValEchoDelay;
		private UI.Knob KnobEchoDelay;
        private System.Windows.Forms.CheckBox CheckEcho;
        private System.Windows.Forms.GroupBox GroupFlanger;
        private System.Windows.Forms.TextBox lblValFlangerDepth;
		private UI.Knob KnobFlangerDepth;
        private System.Windows.Forms.TextBox lblValFlangerRate;
		private UI.Knob KnobFlangerRate;
        private System.Windows.Forms.TextBox lblValFlangerMix;
		private UI.Knob KnobFlangerMix;
        private System.Windows.Forms.CheckBox CheckFlanger;
        private System.Windows.Forms.GroupBox GroupHighpass;
        private System.Windows.Forms.TextBox lblValHighpassResonance;
		private UI.Knob KnobHighpassResonance;
        private System.Windows.Forms.TextBox lblValHighpassCutoff;
		private UI.Knob KnobHighpassCutoff;
        private System.Windows.Forms.CheckBox CheckHighpass;
        private System.Windows.Forms.GroupBox GroupLowpass;
        private System.Windows.Forms.TextBox lblValLowpassResonance;
		private UI.Knob KnobLowpassResonance;
        private System.Windows.Forms.TextBox lblValLowpassCutoff;
		private UI.Knob KnobLowpassCutoff;
        private System.Windows.Forms.CheckBox CheckLowpass;
        private System.Windows.Forms.GroupBox GroupCompressor;
        private System.Windows.Forms.CheckBox CheckCompLinked;
        private System.Windows.Forms.TextBox lblValCompGain;
		private UI.Knob KnobCompGain;
        private System.Windows.Forms.TextBox lblValCompRelease;
        private UI.Knob KnobCompRelease;
        private System.Windows.Forms.TextBox lblValCompAttack;
        private UI.Knob KnobCompAttack;
        private System.Windows.Forms.TextBox lblValCompRatio;
        private UI.Knob KnobCompRatio;
        private System.Windows.Forms.TextBox lblValCompThreshold;
        private UI.Knob KnobCompThreshold;
        private System.Windows.Forms.CheckBox CheckCompressor;
        private System.Windows.Forms.GroupBox GroupReverb;
        private System.Windows.Forms.TextBox textBox13;
        private UI.Knob KnobReverbDry;
        private System.Windows.Forms.TextBox textBox12;
        private UI.Knob KnobReverbWet;
        private System.Windows.Forms.TextBox textBox11;
        private UI.Knob KnobReverbEarlyLate;
        private System.Windows.Forms.TextBox textBox10;
        private UI.Knob KnobReverbHighCut;
        private System.Windows.Forms.TextBox textBox9;
        private UI.Knob KnobReverbLowshelfGain;
        private System.Windows.Forms.TextBox textBox8;
        private UI.Knob KnobReverbLowShelfFrequency;
        private System.Windows.Forms.TextBox textBox7;
        private UI.Knob KnobReverbDensity;
        private System.Windows.Forms.TextBox textBox6;
        private UI.Knob KnobReverbDiffusion;
        private System.Windows.Forms.TextBox textBox1;
        private UI.Knob KnobReverbHFDcRatio;
        private System.Windows.Forms.TextBox textBox2;
        private UI.Knob KnobReverbHFRef;
        private System.Windows.Forms.TextBox textBox3;
        private UI.Knob KnobReverbLateDelay;
        private System.Windows.Forms.TextBox textBox4;
        private UI.Knob KnobReverbEarlyDelay;
        private System.Windows.Forms.TextBox textBox5;
        private UI.Knob KnobReverbDecayTime;
        private System.Windows.Forms.CheckBox CheckReverb;
        private System.Windows.Forms.TabPage tabGEqualizer;
        private System.Windows.Forms.TextBox lblValDistortionLevel;
        private UI.Knob KnobDistortionLevel;
        private System.Windows.Forms.CheckBox CheckDistortion;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.ComboBox cmbOutput;
        private System.Windows.Forms.PictureBox PictGEQGraph;
        private System.Windows.Forms.GroupBox GroupDistortion;
        private System.Windows.Forms.GroupBox GroupGEQ;
        private System.Windows.Forms.CheckBox CheckGEQ;
        private System.Windows.Forms.ComboBox cmbEqPreset;
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
        private System.Windows.Forms.Button btnGEQPresetSave;
		private System.Windows.Forms.Button btnGEQPresetDelete;

		private System.Windows.Forms.Label lblOutputNote;
		// Reverb Labels（旧 label10〜label34 の Reverb 関連）
		private System.Windows.Forms.Label lblReverbDecayTime;
		private System.Windows.Forms.Label lblReverbEarlyDelay;
		private System.Windows.Forms.Label lblReverbLateDelay;
		private System.Windows.Forms.Label lblReverbHFRef;
		private System.Windows.Forms.Label lblReverbHFDcRatio;
		private System.Windows.Forms.Label lblReverbDiffusion;
		private System.Windows.Forms.Label lblReverbDensity;
		private System.Windows.Forms.Label lblReverbLowShelfFreq;
		private System.Windows.Forms.Label lblReverbLowShelfGain;
		private System.Windows.Forms.Label lblReverbHighCut;
		private System.Windows.Forms.Label lblReverbEarlyLate;
		private System.Windows.Forms.Label lblReverbWet;
		private System.Windows.Forms.Label lblReverbDry;
		private System.Windows.Forms.Label lblReverbDivider;

		// Reverb Values（旧 textBox1〜textBox13）
		private System.Windows.Forms.TextBox lblValReverbDecayTime;
		private System.Windows.Forms.TextBox lblValReverbEarlyDelay;
		private System.Windows.Forms.TextBox lblValReverbLateDelay;
		private System.Windows.Forms.TextBox lblValReverbHFRef;
		private System.Windows.Forms.TextBox lblValReverbHFDcRatio;
		private System.Windows.Forms.TextBox lblValReverbDiffusion;
		private System.Windows.Forms.TextBox lblValReverbDensity;
		private System.Windows.Forms.TextBox lblValReverbLowShelfFreq;
		private System.Windows.Forms.TextBox lblValReverbLowShelfGain;
		private System.Windows.Forms.TextBox lblValReverbHighCut;
		private System.Windows.Forms.TextBox lblValReverbEarlyLate;
		private System.Windows.Forms.TextBox lblValReverbWet;
		private System.Windows.Forms.TextBox lblValReverbDry;

		private System.Windows.Forms.Label lblSkinName;
		private System.Windows.Forms.Label lblSkinAuthorLabel;
		private System.Windows.Forms.Label lblSkinAuthor;
		private System.Windows.Forms.Label lblSkinDescLabel;
		private System.Windows.Forms.Label lblSkinDesc;

        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Label lblAboutAppName;
        private System.Windows.Forms.Label lblAboutVersion;
        private System.Windows.Forms.Label lblAboutCopyright;
        private System.Windows.Forms.Label lblAboutCompany;
        private System.Windows.Forms.LinkLabel lnkAboutGitHub;

        private System.Windows.Forms.ComboBox cmbChorusPreset;
        private System.Windows.Forms.Button btnChorusPresetSave;
        private System.Windows.Forms.Button btnChorusPresetDelete;

        private System.Windows.Forms.ComboBox cmbEchoPreset;
        private System.Windows.Forms.Button btnEchoPresetSave;
        private System.Windows.Forms.Button btnEchoPresetDelete;

        private System.Windows.Forms.ComboBox cmbDistortionPreset;
        private System.Windows.Forms.Button btnDistortionPresetSave;
        private System.Windows.Forms.Button btnDistortionPresetDelete;

        private System.Windows.Forms.ComboBox cmbFlangerPreset;
        private System.Windows.Forms.Button btnFlangerPresetSave;
        private System.Windows.Forms.Button btnFlangerPresetDelete;

        private System.Windows.Forms.ComboBox cmbHighpassPreset;
        private System.Windows.Forms.Button btnHighpassPresetSave;
        private System.Windows.Forms.Button btnHighpassPresetDelete;

        private System.Windows.Forms.ComboBox cmbLowpassPreset;
        private System.Windows.Forms.Button btnLowpassPresetSave;
        private System.Windows.Forms.Button btnLowpassPresetDelete;

        private System.Windows.Forms.ComboBox cmbCompressorPreset;
        private System.Windows.Forms.Button btnCompressorPresetSave;
        private System.Windows.Forms.Button btnCompressorPresetDelete;

        private System.Windows.Forms.ComboBox cmbReverbPreset;
        private System.Windows.Forms.Button btnReverbPresetSave;
        private System.Windows.Forms.Button btnReverbPresetDelete;

        private System.Windows.Forms.ComboBox cmbPitchPreset;
        private System.Windows.Forms.Button btnPitchPresetSave;
        private System.Windows.Forms.Button btnPitchPresetDelete;
    }
}