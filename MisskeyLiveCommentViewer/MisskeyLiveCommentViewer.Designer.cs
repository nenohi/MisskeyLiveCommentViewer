namespace MisskeyLiveCommentViewer
{
	partial class MisskeyLiveCommentViewer
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
            this.Bouyomichan = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ActiveUserCount = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
            this.listView1.Size = new System.Drawing.Size(620, 400);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectButton.Location = new System.Drawing.Point(713, 361);
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
            this.ShowCommentWindow.Size = new System.Drawing.Size(145, 16);
            this.ShowCommentWindow.TabIndex = 2;
            this.ShowCommentWindow.Text = "Show Comment Window";
            this.ShowCommentWindow.UseVisualStyleBackColor = true;
            this.ShowCommentWindow.CheckedChanged += new System.EventHandler(this.ShowCommentWindow_CheckedChanged);
            // 
            // ScreenDisplay
            // 
            this.ScreenDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScreenDisplay.FormattingEnabled = true;
            this.ScreenDisplay.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScreenDisplay.Items.AddRange(new object[] {
            "OBS"});
            this.ScreenDisplay.Location = new System.Drawing.Point(648, 34);
            this.ScreenDisplay.Name = "ScreenDisplay";
            this.ScreenDisplay.Size = new System.Drawing.Size(140, 20);
            this.ScreenDisplay.TabIndex = 3;
            this.ScreenDisplay.SelectedIndexChanged += new System.EventHandler(this.ScreenDisplay_SelectedIndexChanged);
            // 
            // MisskeyID
            // 
            this.MisskeyID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MisskeyID.Location = new System.Drawing.Point(648, 336);
            this.MisskeyID.Name = "MisskeyID";
            this.MisskeyID.Size = new System.Drawing.Size(140, 19);
            this.MisskeyID.TabIndex = 4;
            this.MisskeyID.TextChanged += new System.EventHandler(this.MisskeyID_TextChanged);
            // 
            // Bouyomichan
            // 
            this.Bouyomichan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bouyomichan.AutoSize = true;
            this.Bouyomichan.Location = new System.Drawing.Point(648, 60);
            this.Bouyomichan.Name = "Bouyomichan";
            this.Bouyomichan.Size = new System.Drawing.Size(92, 16);
            this.Bouyomichan.TabIndex = 5;
            this.Bouyomichan.Text = "Bouyomichan";
            this.Bouyomichan.UseVisualStyleBackColor = true;
            this.Bouyomichan.CheckedChanged += new System.EventHandler(this.Bouyomichan_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(705, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "ChangeColor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(648, 94);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 23);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(646, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "UserID";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Location = new System.Drawing.Point(648, 135);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(51, 23);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(705, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "ChangeColor";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(646, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "TextColor";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(646, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Background Color";
            // 
            // ActiveUserCount
            // 
            this.ActiveUserCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveUserCount.AutoSize = true;
            this.ActiveUserCount.Location = new System.Drawing.Point(646, 400);
            this.ActiveUserCount.Name = "ActiveUserCount";
            this.ActiveUserCount.Size = new System.Drawing.Size(74, 12);
            this.ActiveUserCount.TabIndex = 8;
            this.ActiveUserCount.Text = "Active Users:";
            // 
            // LoginButton
            // 
            this.LoginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginButton.Location = new System.Drawing.Point(718, 418);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 11;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(637, 418);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 12;
            this.SendButton.Text = "投稿";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // SendTextBox
            // 
            this.SendTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendTextBox.Enabled = false;
            this.SendTextBox.Location = new System.Drawing.Point(12, 420);
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.Size = new System.Drawing.Size(620, 19);
            this.SendTextBox.TabIndex = 13;
            this.SendTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTextBox_KeyDown);
            // 
            // MisskeyLiveCommentViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SendTextBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ActiveUserCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Bouyomichan);
            this.Controls.Add(this.MisskeyID);
            this.Controls.Add(this.ScreenDisplay);
            this.Controls.Add(this.ShowCommentWindow);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.listView1);
            this.Name = "MisskeyLiveCommentViewer";
            this.Text = "MisskeyCommentViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MisskeyLiveCommentViewer_FormClosing);
            this.Load += new System.EventHandler(this.MisskeyLiveCommentViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button ConnectButton;
		private System.Windows.Forms.CheckBox ShowCommentWindow;
		private System.Windows.Forms.ComboBox ScreenDisplay;
		private System.Windows.Forms.TextBox MisskeyID;
		private System.Windows.Forms.CheckBox Bouyomichan;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ActiveUserCount;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox SendTextBox;
    }
}

