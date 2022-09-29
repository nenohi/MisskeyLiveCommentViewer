using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace MisskeyLiveCommentViewer
{
	public partial class MisskeyLiveCommentViewer : Form
	{
		private CommentScreen CommentScrean;
		private Misskey misskey = new Misskey();
		private ImageList ImageList = new ImageList() { ImageSize=new Size(50,50)};
		private Bouyomichan bouyomichan = new Bouyomichan();
		private ListViewItem listViewItemtemp = new ListViewItem();
		private string UserID { set; get; }
		private DispatcherTimer timer;
		private int errorcnt=0;
		public MisskeyLiveCommentViewer()
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
            button1.Enabled = false;
            button2.Enabled = false;
            pictureBox1.Enabled = false;
            pictureBox2.Enabled = false;

        }

		private void ScreenDisplay_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ShowCommentWindow.Checked) return;


            if (ScreenDisplay.SelectedItem.GetType() == typeof(Screen))
			{
				if(CommentScrean.AllowsTransparency)
				{
                    CommentScrean.Height = ((Screen)ScreenDisplay.SelectedItem).Bounds.Height;
                    CommentScrean.Width = ((Screen)ScreenDisplay.SelectedItem).Bounds.Width;
                    CommentScrean.Top = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.Y;
                    CommentScrean.Left = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.X;
				}
				else
				{
					WindowReOpen((Screen)ScreenDisplay.SelectedItem, true);
                }
			}
			else
			{
				WindowReOpen((Screen)ScreenDisplay.Items[1], false);
            }
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
                if (ScreenDisplay.SelectedItem.GetType() == typeof(Screen))
				{
                    if (CommentScrean.AllowsTransparency)
                    {
                        CommentScrean.Height = ((Screen)ScreenDisplay.SelectedItem).Bounds.Height;
                        CommentScrean.Width = ((Screen)ScreenDisplay.SelectedItem).Bounds.Width;
                        CommentScrean.Top = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.Y;
                        CommentScrean.Left = ((Screen)ScreenDisplay.SelectedItem).Bounds.Location.X;
					}
					else
					{
						WindowReOpen((Screen)ScreenDisplay.SelectedItem, true);
					}
                }
				else
				{
					WindowReOpen((Screen)ScreenDisplay.Items[1], false);
                }
            }
			else
			{
				CommentScrean.Close();
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
			UserID = id;
			misskey.ConnectAsync();
			ActiveUserTimer();
			ConnectButton.Text = "Connected";
			ConnectButton.Enabled = false;
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
			if (!ImageList.Images.Keys.Contains(json.body.body.user.username))
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
		private void Bouyomichan_CheckedChanged(object sender, EventArgs e)
		{
			if (Bouyomichan.Checked)
			{
				bouyomichan.Start();
			}
		}

		private System.Windows.Media.Brush DrawColorToBrush(System.Drawing.Color color)
		{
			var mediacolor = System.Windows.Media.Color.FromArgb(color.A,color.R, color.G, color.B);
            return new System.Windows.Media.SolidColorBrush(mediacolor);
        }
		/// <summary>
		/// CommentScreanを最初期化します
		/// </summary>
		/// <param name="screen">初期化先モニタ</param>
		/// <param name="flag">透過するかどうか
		/// true:透過
		/// false:無透過
		/// </param>
		private void WindowReOpen(Screen screen,bool flag)
		{
            CommentScrean?.Close();
            CommentScrean = new CommentScreen();
			if (flag)
			{
				CommentScrean.WindowState = System.Windows.WindowState.Normal;
				CommentScrean.WindowStyle = System.Windows.WindowStyle.None; 
				CommentScrean.Height = screen.Bounds.Height;
                CommentScrean.Width = screen.Bounds.Width;
                CommentScrean.Top = screen.Bounds.Location.Y;
                CommentScrean.Left = screen.Bounds.Location.X;
            }
			else
			{
				CommentScrean.WindowState = System.Windows.WindowState.Normal;
				CommentScrean.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                CommentScrean.TextColor = DrawColorToBrush(pictureBox1.BackColor);
                CommentScrean.Background = DrawColorToBrush(pictureBox2.BackColor);
            }
            CommentScrean.ChanegAllowsTransparency(flag);
            CommentScrean.Visibility = System.Windows.Visibility.Visible;
			CommentScrean.Topmost = flag;
			CommentScrean.ShowInTaskbar = !flag;
            button1.Enabled = !flag;
            button2.Enabled = !flag;
            pictureBox1.Enabled = !flag;
            pictureBox2.Enabled = !flag;

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

		private void button2_Click(object sender, EventArgs e)
		{
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var mediacolor = System.Windows.Media.Color.FromArgb(colorDialog1.Color.A, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                CommentScrean.Background = new System.Windows.Media.SolidColorBrush(mediacolor);
                pictureBox2.BackColor = colorDialog1.Color;
            }
        }

		private void ActiveUserTimer()
		{
			if(timer != null)
			{
				if (timer.IsEnabled)
				{
					timer.Stop();
				}
            }
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(5000) };
			timer.Tick += Timer_Tick;
			timer.Start();
        }

		private void Timer_Tick(object sender, EventArgs e)
		{
            ActiveUserCount.Text = "Active Users:" + GetActiveUser().ToString();
			if (!ConnectButton.Enabled)
			{
				if (!misskey.WebSocketConnectState)
				{
					errorcnt++;
				}
				if (errorcnt > 5)
				{
					errorcnt = 0;
                    ConnectButton.Enabled = true;
                    ConnectButton.Text = "Connect";
                }
			}
        }
		private int GetActiveUser()
		{
			int activeusers=0;
            WebRequest request = WebRequest.Create("https://livenow-6xpu3met.arkjp.net/?id="+UserID);
            Stream response_stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response_stream);
            var obj_from_json = JObject.Parse(reader.ReadToEnd());
			activeusers = (int)obj_from_json["count"];
            return activeusers;
		}

		private void MisskeyID_TextChanged(object sender, EventArgs e)
		{
			ConnectButton.Enabled = true;
			ConnectButton.Text = "Connect";
		}
	}
}