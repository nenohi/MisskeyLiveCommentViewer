using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisskeyCommentViewer
{
	public partial class MisskeyCommentViewer : Form
	{
		private CommentScreen CommentScrean;
		private Misskey misskey = new Misskey();
		private ImageList ImageList = new ImageList() { ImageSize=new Size(50,50)};
		private Bouyomichan bouyomichan = new Bouyomichan();
		private ListViewItem listViewItemtemp = new ListViewItem();
		public MisskeyCommentViewer()
		{
			InitializeComponent();
			ScreenDisplay.DisplayMember = "DeviceName";
			foreach (Screen s in Screen.AllScreens)
			{
				if (s != null)
				{
					ScreenDisplay.Items.Add(s);
				}
			}
			CommentScrean = new CommentScreen();
			CommentScrean.Visibility = System.Windows.Visibility.Hidden;

			listView1.Columns.Add("icon", 50, HorizontalAlignment.Center);
			listView1.Columns.Add("userid", 70, HorizontalAlignment.Left);
			listView1.Columns.Add("comment", 490, HorizontalAlignment.Left);

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
			if(ScreenDisplay.SelectedItem == null)
			{
				ShowCommentWindow.Checked = false;
				return;
			}
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
			string misskeyidtext = MisskeyID.Text.ToLower();
			string id="";
			Uri uri;
			bool isurl = Uri.TryCreate(misskeyidtext, UriKind.Absolute,out uri);
			if (isurl)
			{
				if(uri.Host == "live.misskey.io")
				{
					id = uri.Segments.Last().Replace("@","");
                }
				else
				{

				}
			}else if (misskeyidtext.Contains("@"))
			{
				id = misskeyidtext.Replace("@", "");
			}
			else
			{
				id = misskeyidtext;
			}
			misskey.ReceiveLiveComment -= Misskey_ReceiveLiveComment;
			misskey.ReceiveLiveComment += Misskey_ReceiveLiveComment;
			misskey.livetag = "ml" + id;
			misskey.ConnectAsync();
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
			if (checkBox1.Checked && !ImageList.Images.Keys.Contains(json.body.body.user.username))
			{
				await ViewImageURL(json.body.body.user.avatarUrl, json.body.body.user.username);
			}
			string Text = json.body.body.text;
			string regex = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
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
				string bouyomi = Regex.Replace(Text, @"(#[a-z|A-Z]*)", "");
                bouyomichan.Speak(bouyomi);
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
				listView1.Items.Add(listViewItemtemp);
				listView1.Update();
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                listViewItemtemp = null;
			}
		}
		public Task ViewImageURL(string url, string username)
		{
			try
            {
				var iurl = new Uri(url);
				var iurlSegments = iurl.Segments[iurl.Segments.Length - 1].Split('.');
				var filetype = iurlSegments[iurlSegments.Length - 1].ToLower();
				if (filetype == "webp")
				{
                    using (var web = new WebClient())
                    {
						Image image = null;
                        var bytes = web.DownloadData(new Uri(url));
                        using (Stream stream = new MemoryStream(bytes))
                        {
                            ImageProcessor.Plugins.WebP.Imaging.Formats.WebPFormat format = new ImageProcessor.Plugins.WebP.Imaging.Formats.WebPFormat();
							image = format.Load(stream);
							
							ImageList.Images.Add(username,image);
                        }
						if (image == null) image.Dispose();
                    }
                }
				else
				{
					using (var web = new WebClient())
					{
						var bytes = web.DownloadData(new Uri(url));
						using (Stream stream = new MemoryStream(bytes))
						{
							Image img = Image.FromStream(stream);
							ImageList.Images.Add(username, img);
							img.Dispose();
						}
					}
				}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

			return Task.CompletedTask;
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

		private void button1_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				var mediacolor = System.Windows.Media.Color.FromArgb(colorDialog1.Color.A, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
				CommentScrean.TextColor = new System.Windows.Media.SolidColorBrush(mediacolor);
				pictureBox1.BackColor = colorDialog1.Color;
			}
		}
    }
}