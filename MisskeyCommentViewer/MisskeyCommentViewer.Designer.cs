namespace MisskeyCommentViewer
{
	partial class MisskeyCommentViewer
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.listView1 = new System.Windows.Forms.ListView();
			this.ConnectButton = new System.Windows.Forms.Button();
			this.ShowCommentWindow = new System.Windows.Forms.CheckBox();
			this.ScreenDisplay = new System.Windows.Forms.ComboBox();
			this.MisskeyID = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(12, 12);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(620, 426);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// ConnectButton
			// 
			this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectButton.Location = new System.Drawing.Point(713, 415);
			this.ConnectButton.Name = "ConnectButton";
			this.ConnectButton.Size = new System.Drawing.Size(75, 23);
			this.ConnectButton.TabIndex = 1;
			this.ConnectButton.Text = "Connect";
			this.ConnectButton.UseVisualStyleBackColor = true;
			this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
			// 
			// ShowCommentWindow
			// 
			this.ShowCommentWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowCommentWindow.AutoSize = true;
			this.ShowCommentWindow.Location = new System.Drawing.Point(648, 12);
			this.ShowCommentWindow.Name = "ShowCommentWindow";
			this.ShowCommentWindow.Size = new System.Drawing.Size(137, 16);
			this.ShowCommentWindow.TabIndex = 2;
			this.ShowCommentWindow.Text = "Show Comment Windo";
			this.ShowCommentWindow.UseVisualStyleBackColor = true;
			this.ShowCommentWindow.CheckedChanged += new System.EventHandler(this.ShowCommentWindow_CheckedChanged);
			// 
			// ScreenDisplay
			// 
			this.ScreenDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ScreenDisplay.FormattingEnabled = true;
			this.ScreenDisplay.Location = new System.Drawing.Point(648, 61);
			this.ScreenDisplay.Name = "ScreenDisplay";
			this.ScreenDisplay.Size = new System.Drawing.Size(140, 20);
			this.ScreenDisplay.TabIndex = 3;
			this.ScreenDisplay.SelectedIndexChanged += new System.EventHandler(this.ScreenDisplay_SelectedIndexChanged);
			// 
			// MisskeyID
			// 
			this.MisskeyID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.MisskeyID.Location = new System.Drawing.Point(648, 390);
			this.MisskeyID.Name = "MisskeyID";
			this.MisskeyID.Size = new System.Drawing.Size(140, 19);
			this.MisskeyID.TabIndex = 4;
			this.MisskeyID.Leave += new System.EventHandler(this.MisskeyID_Leave);
			// 
			// MisskeyCommentViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.MisskeyID);
			this.Controls.Add(this.ScreenDisplay);
			this.Controls.Add(this.ShowCommentWindow);
			this.Controls.Add(this.ConnectButton);
			this.Controls.Add(this.listView1);
			this.Name = "MisskeyCommentViewer";
			this.Text = "MisskeyCommentViewer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button ConnectButton;
		private System.Windows.Forms.CheckBox ShowCommentWindow;
		private System.Windows.Forms.ComboBox ScreenDisplay;
		private System.Windows.Forms.TextBox MisskeyID;
	}
}

