using MediaPlayer_X_Ark.Engine.Config;
using MediaPlayer_X_Ark.Engine.Player;
using MediaPlayer_X_Ark.Forms.Options;
using MediaPlayer_X_Ark.Forms.Options.Effects;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Forms
{
	public partial class OptionsForm : Form
	{
		private float _runtimeLayoutScale = 1f;

		private IPlayerEngine _engine;
		private IConfigService _config;
		private MainForm _mainForm;

		private TreeView _treeMenu;
		private Panel _contentPanel;

		private Dictionary<string, OptionsControlBase> _controls
			= new Dictionary<string, OptionsControlBase>();

		public OptionsForm(MainForm mainForm, PlayerController engine, IConfigService config)
		{
			InitializeComponent();

			_engine = engine.Engine;
			_config = config;
			_mainForm = mainForm;
			this.Owner = mainForm;
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			// アプリ内の他フォームより前面に固定
			Win32API.SetWindowPos(this.Handle, Win32API.HWND_TOP, 0, 0, 0, 0,
				Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE | Win32API.SWP_NOACTIVATE);
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			BuildLayout();
			ApplyRuntimeLayoutScaleIfNeeded();
			BuildTreeMenu();
			RegisterControls();
			SelectTab("GENERAL");
		}
		private void BuildLayout()
		{
			// TreeView（左）
			_treeMenu = new TreeView
			{
				Width = 200,
				Dock = DockStyle.Left,
				BorderStyle = BorderStyle.None,
				Font = new Font("Yu Gothic UI", 9f),
			};
			_treeMenu.AfterSelect += TreeMenu_AfterSelect;

			// コンテンツPanel（右）
			_contentPanel = new Panel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(8),
			};

			// Splitter
			var splitter = new Splitter
			{
				Dock = DockStyle.Left,
				Width = 1,
				BackColor = SystemColors.ControlDark,
			};

			Controls.Add(_contentPanel);
			Controls.Add(splitter);
			Controls.Add(_treeMenu);
		}

		private void ApplyRuntimeLayoutScaleIfNeeded()
		{
			float scale = DeviceDpi > 0 ? DeviceDpi / 96f : 1f;
			if (Math.Abs(scale - _runtimeLayoutScale) < 0.001f)
				return;

			float delta = scale / _runtimeLayoutScale;
			SuspendLayout();
			foreach (Control control in Controls)
				control.Scale(new SizeF(delta, delta));

			_treeMenu.Width = ScaleLength(_treeMenu.Width, delta);
			_contentPanel.Padding = ScalePadding(_contentPanel.Padding, delta);
			_runtimeLayoutScale = scale;
			ResumeLayout(true);
		}

		private static Padding ScalePadding(Padding padding, float scale)
			=> new Padding(
				ScaleLength(padding.Left, scale),
				ScaleLength(padding.Top, scale),
				ScaleLength(padding.Right, scale),
				ScaleLength(padding.Bottom, scale));

		private static int ScaleLength(int value, float scale)
		{
			if (value <= 0)
				return value;

			return Math.Max(1, (int)Math.Round(value * scale, MidpointRounding.AwayFromZero));
		}

		private void BuildTreeMenu()
		{
			var nodeGeneral = new TreeNode("一般設定") { Name = "GENERAL" };
			var nodeDisplay = new TreeNode("表示設定") { Name = "DISPLAY" };
			var nodeOutput = new TreeNode("出力設定") { Name = "OUTPUT" };
			var nodeGEQ = new TreeNode("Graphic Equalizer") { Name = "GEQ" };
			var nodePitch = new TreeNode("Pitch / Freq / Speed") { Name = "PITCH" };
			var nodeDistortion = new TreeNode("Distortion") { Name = "DISTORTION" };
			var nodeChorus = new TreeNode("Chorus") { Name = "CHORUS" };
			var nodeEcho = new TreeNode("Echo") { Name = "ECHO" };
			var nodeFlanger = new TreeNode("Flanger") { Name = "FLANGER" };
			var nodeHighpass = new TreeNode("Highpass") { Name = "HIGHPASS" };
			var nodeLowpass = new TreeNode("Lowpass") { Name = "LOWPASS" };
			var nodeCompressor = new TreeNode("Compressor") { Name = "COMPRESSOR" };
			var nodeReverb = new TreeNode("Reverb") { Name = "REVERB" };
			var nodeNormalize = new TreeNode("Normalize") { Name = "NORMALIZE" };
			var nodeSkin = new TreeNode("スキン") { Name = "SKIN" };
			var nodeCddb = new TreeNode("CDDB") { Name = "CDDB" };
			var nodeExtensions = new TreeNode("関連付け") { Name = "EXTENSIONS" };
			var nodeAbout = new TreeNode("About") { Name = "ABOUT" };
			var nodeCrossfade = new TreeNode("再生設定") { Name = "PLAYBACK" };

			var nodeEffects = new TreeNode("エフェクト", new TreeNode[]
			{
				nodeGEQ, nodePitch, nodeDistortion, nodeChorus,
				nodeEcho, nodeFlanger, nodeHighpass, nodeLowpass,
				nodeCompressor, nodeReverb, nodeNormalize,
			})
			{ Name = "EFFECTS" };
            var nodePlugins = new TreeNode("プラグイン") { Name = "PLUGINS" };
            var nodeOther = new TreeNode("その他", new TreeNode[]
			{
				nodeCddb, nodeExtensions, nodeAbout
			})
			{ Name = "OTHER" };

			_treeMenu.Nodes.AddRange(new TreeNode[]
			{
				nodeGeneral, nodeCrossfade,  nodeDisplay, nodeOutput,
				nodeEffects, nodeSkin, nodePlugins, nodeOther
            });

			_treeMenu.ExpandAll();
			_treeMenu.SelectedNode = nodeGeneral;
		}
		private void RegisterControls()
		{
			// ★順次追加していく
			_controls["GENERAL"] = new GeneralSettingsControl(_engine, _config, _mainForm);
			_controls["PLAYBACK"] = new PlaybackSettingsControl(_engine, _config);
			_controls["DISPLAY"] = new DisplaySettingsControl(_engine, _config, _mainForm);
			_controls["OUTPUT"] = new OutputSettingsControl(_engine, _config, _mainForm);
			_controls["SKIN"] = new SkinSettingsControl(_engine, _config, _mainForm);
			_controls["GEQ"] = new GEQControl(_engine, _config);
			_controls["DISTORTION"] = new Options.Effects.DistortionControl(_engine, _config);
			_controls["CHORUS"] = new Options.Effects.ChorusControl(_engine, _config);
			_controls["ECHO"] = new Options.Effects.EchoControl(_engine, _config);
			_controls["FLANGER"] = new Options.Effects.FlangerControl(_engine, _config);
			_controls["HIGHPASS"] = new Options.Effects.HighpassControl(_engine, _config);
			_controls["LOWPASS"] = new Options.Effects.LowpassControl(_engine, _config);
			_controls["COMPRESSOR"] = new Options.Effects.CompressorControl(_engine, _config);
			_controls["REVERB"] = new Options.Effects.ReverbControl(_engine, _config);
			_controls["NORMALIZE"] = new Options.Effects.NormalizeControl(_engine, _config);
			_controls["PITCH"] = new Options.Effects.PitchControl(_engine, _config);
			_controls["CDDB"] = new CddbSettingsControl(_engine, _config);
			_controls["EXTENSIONS"] = new ExtensionsControl(_engine, _config);
            _controls["PLUGINS"] = new PluginsControl(_engine, _config);
            _controls["ABOUT"] = new AboutControl(_engine, _config);
			// ...
		}

		private void TreeMenu_AfterSelect(object sender, TreeViewEventArgs e)
		{
			SelectTab(e.Node.Name);
		}

		public void SelectTab(string tabName)
		{
			if (!_controls.TryGetValue(tabName, out var control)) return;

			_contentPanel.Controls.Clear();
			_contentPanel.Controls.Add(control);
			control.LoadSettings();
		}

		private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
