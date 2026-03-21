using MediaPlayer_X_Ark.Engine;
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
		private IPlayerEngine _engine;
		private IConfigService _config;
		private MainForm _mainForm;

		private TreeView _treeMenu;
		private Panel _contentPanel;

		private Dictionary<string, OptionsControlBase> _controls
			= new Dictionary<string, OptionsControlBase>();

		public OptionsForm(IPlayerEngine engine, IConfigService config, MainForm mainForm)
		{
			InitializeComponent();

			_engine = engine;
			_config = config;
			_mainForm = mainForm;
			this.Owner = mainForm;
		}
		private void OptionsForm_Load(object sender, EventArgs e)
		{
			BuildLayout();
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
			var nodeSkin = new TreeNode("スキン") { Name = "SKIN" };
			var nodeCddb = new TreeNode("CDDB") { Name = "CDDB" };
			var nodeExtensions = new TreeNode("関連付け") { Name = "EXTENSIONS" };
			var nodeAbout = new TreeNode("About") { Name = "ABOUT" };

			var nodeEffects = new TreeNode("エフェクト", new TreeNode[]
			{
				nodeGEQ, nodePitch, nodeDistortion, nodeChorus,
				nodeEcho, nodeFlanger, nodeHighpass, nodeLowpass,
				nodeCompressor, nodeReverb
			})
			{ Name = "EFFECTS" };

			var nodeOther = new TreeNode("その他", new TreeNode[]
			{
				nodeCddb, nodeExtensions, nodeAbout
			})
			{ Name = "OTHER" };

			_treeMenu.Nodes.AddRange(new TreeNode[]
			{
				nodeGeneral, nodeDisplay, nodeOutput,
				nodeEffects, nodeSkin, nodeOther
			});

			_treeMenu.ExpandAll();
		}
		private void RegisterControls()
		{
			// ★順次追加していく
			_controls["GENERAL"] = new GeneralSettingsControl(_engine, _config, _mainForm);
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
			_controls["PITCH"] = new Options.Effects.PitchControl(_engine, _config);
			_controls["CDDB"] = new CddbSettingsControl(_engine, _config);
			_controls["EXTENSIONS"] = new ExtensionsControl(_engine, _config);
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
