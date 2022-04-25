using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MisskeyCommentViewer
{
	public partial class MisskeyCommentViewer : Form
	{
		private CommentScreen CommentScrean;
		private Misskey misskey = new Misskey();
		private ImageList ImageList = new ImageList();
		private Bouyomichan bouyomichan = new Bouyomichan();
		private ListViewItem listViewItemtemp = new ListViewItem();
		public MisskeyCommentViewer()
		{
			InitializeComponent();
			ScreenDisplay.DisplayMember = "DeviceName";
			foreach (System.Windows.Forms.Screen s in Screen.AllScreens)
			{
				if (s != null)
				{
					ScreenDisplay.Items.Add(s);
				}
			}
			CommentScrean = new CommentScreen();
			CommentScrean.Visibility = System.Windows.Visibility.Hidden;

			listView1.Columns.Add("", 50, HorizontalAlignment.Center);
			listView1.Columns.Add("userid", 70, HorizontalAlignment.Left);
			listView1.Columns.Add("comment", 500, HorizontalAlignment.Left);

			listView1.SmallImageList = ImageList;
			listView1.Update();

		}

		private void ScreenDisplay_SelectedIndexChanged(object sender, EventArgs e)
		{
			CommentScrean.Height = ((Screen)ScreenDisplay.SelectedItem).Bounds.Height;
			CommentScrean.Width = ((Screen)ScreenDisplay.SelectedItem).Bounds.Width;
			CommentScrean.Top = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.Y;
			CommentScrean.Left = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.X;
		}

		private void ShowCommentWindow_CheckedChanged(object sender, EventArgs e)
		{
			if (ShowCommentWindow.Checked)
			{
				CommentScrean.Visibility = System.Windows.Visibility.Visible;
			}
			else
			{
				CommentScrean.Visibility = System.Windows.Visibility.Hidden;
			}
		}

		private void ConnectButton_Click(object sender, EventArgs e)
		{
			misskey.ReceiveLiveComment += Misskey_ReceiveLiveComment;
			misskey.livetag = "ml" + MisskeyID.Text.ToLower();
			misskey.ConnectAsync();

		}
		Image createThumbnail(Image image, int w, int h)
		{
			Bitmap canvas = new Bitmap(w, h);

			Graphics g = Graphics.FromImage(canvas);
			g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

			float fw = (float)w / (float)image.Width;
			float fh = (float)h / (float)image.Height;

			float scale = Math.Min(fw, fh);
			fw = image.Width * scale;
			fh = image.Height * scale;

			g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
			g.Dispose();

			return canvas;
		}
		private async void Misskey_ReceiveLiveComment(object sender, EventArgs e)
		{
			if (ShowCommentWindow.Checked)
			{
				CommentScrean.Dispatcher.Invoke(() =>
				{
					CommentScrean.Addtext(sender);
				});
			}
			var json = JsonConvert.DeserializeObject<MisskeyReceiveObj>(sender.ToString());
			await ViewImageURL(json.body.body.user.avatarUrl, json.body.body.user.username);
			string Text = json.body.body.text;
			string regex = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
			foreach (string tag in json.body.body.tags)
			{
				Text = Text.Replace("#" + tag, "");
			}
			Text = Regex.Replace(Text, regex, "");
			Text = Text.Trim();
			string[] item = { json.body.body.user.username, json.body.body.user.name, json.body.body.user.username, Text };
			listViewItemtemp = new ListViewItem();
			listViewItemtemp.Text = "";
			listViewItemtemp.ImageKey = json.body.body.user.username;
			listViewItemtemp.SubItems.Add(json.body.body.user.name);
			listViewItemtemp.SubItems.Add(Text);

			if (Bouyomichan.Checked)
			{
				bouyomichan.Speak(Text);
			}

			if (InvokeRequired)
			{
				Invoke(new ListAddDelgate(ListAdd));
			}
			else
			{
				ListAdd();
			}
		}
		delegate void ListAddDelgate();
		public void ListAdd()
		{
			if (listViewItemtemp != null)
			{
				listView1.SmallImageList = ImageList;
				listView1.Items.Insert(0, listViewItemtemp);
				listView1.Update();
				listViewItemtemp = null;
			}
		}
		public async Task ViewImageURL(string url, string username)
		{
			using (var web = new System.Net.Http.HttpClient())
			{
				var bytes = await web.GetByteArrayAsync(url).ConfigureAwait(false);
				using (var stream = new System.IO.MemoryStream(bytes))
				{
					var bitmapimage = new BitmapImage();
					bitmapimage.BeginInit();
					bitmapimage.StreamSource = stream;
					bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapimage.EndInit();
					bitmapimage.Freeze();
					Image bitmap = Bitmap.FromStream(stream);
					ImageList.Images.Add(username, bitmap);
					bitmap.Dispose();
				}
			}
		}

		private void MisskeyID_Leave(object sender, EventArgs e)
		{
			misskey.livetag = MisskeyID.Text;
		}

		private void Bouyomichan_CheckedChanged(object sender, EventArgs e)
		{
			if (Bouyomichan.Checked)
			{
				bouyomichan.Start();
			}
		}
	}
}
