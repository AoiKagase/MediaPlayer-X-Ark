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
	public partial class UrlInputForm : Form
	{
		private Label lblMessage;
		private TextBox txtUrl;
		private Button btnOK;
		private Button btnCancel;

		public string Url => txtUrl.Text.Trim();

		public UrlInputForm()
		{
			// フォーム設定
			this.Text = "URL Open";
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Width = 420;
			this.Height = 130;
			this.AcceptButton = btnOK;
			this.CancelButton = btnCancel;

			// ラベル
			lblMessage = new Label
			{
				Text = "再生するURLを入力してください",
				Location = new System.Drawing.Point(12, 12),
				Size = new System.Drawing.Size(380, 15),
			};

			// テキストボックス
			txtUrl = new TextBox
			{
				Text = "https://",
				Location = new System.Drawing.Point(12, 35),
				Size = new System.Drawing.Size(380, 23),
			};

			// OKボタン
			btnOK = new Button
			{
				Text = "OK",
				DialogResult = DialogResult.OK,
				Location = new System.Drawing.Point(236, 65),
				Size = new System.Drawing.Size(75, 23),
			};

			// キャンセルボタン
			btnCancel = new Button
			{
				Text = "キャンセル",
				DialogResult = DialogResult.Cancel,
				Location = new System.Drawing.Point(317, 65),
				Size = new System.Drawing.Size(75, 23),
			};

			// AcceptButton/CancelButton をボタン生成後に設定
			this.AcceptButton = btnOK;
			this.CancelButton = btnCancel;

			this.Controls.AddRange(new Control[]
			{
				lblMessage, txtUrl, btnOK, btnCancel
			});
		}
	}
}
