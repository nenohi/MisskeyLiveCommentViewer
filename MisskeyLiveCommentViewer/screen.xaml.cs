using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MisskeyLiveCommentViewer
{
	/// <summary>
	/// screan.xaml の相互作用ロジック
	/// </summary>
	public partial class CommentScreen : Window
	{
		//Window window = new Window();

		int[] uetexthi = new int[25];
		int[] nakatexthi = new int[25];
		int[] shitatexthi = new int[25];
		int[] uetextcnt = new int[25];
		int[] nakatextcnt = new int[25];
		int[] shitatextcnt = new int[25];
		public System.Windows.Media.Brush TextColor = System.Windows.Media.Brushes.White;

		#region DependencyProperties

		#region AltF4Cancel

		public bool AltF4Cancel
		{
			get { return (bool)GetValue(AltF4CancelProperty); }
			set { SetValue(AltF4CancelProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AltF4Cancel.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AltF4CancelProperty =
			DependencyProperty.Register(
				"AltF4Cancel",
				typeof(bool),
				typeof(CommentScreen),
				new PropertyMetadata(true));

		#endregion

		#region ShowSystemMenu

		public bool ShowSystemMenu
		{
			get { return (bool)GetValue(ShowSystemMenuProperty); }
			set { SetValue(ShowSystemMenuProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ShowSystemMenu.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShowSystemMenuProperty =
			DependencyProperty.Register(
				"ShowSystemMenu",
				typeof(bool),
				typeof(CommentScreen),
				new PropertyMetadata(
					false,
					ShowSystemMenuPropertyChanged));

		private static void ShowSystemMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

			if (d is CommentScreen window)
			{

				window.SetShowSystemMenu((bool)e.NewValue);

			}

		}

		#endregion

		#region ClickThrough

		public bool ClickThrough
		{
			get { return (bool)GetValue(ClickThroughProperty); }
			set { SetValue(ClickThroughProperty, value); }
		}


		// Using a DependencyProperty as the backing store for ClickThrough.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ClickThroughProperty =
			DependencyProperty.Register(
				"ClickThrough",
				typeof(bool),
				typeof(CommentScreen),
				new PropertyMetadata(
					true,
					ClickThroughPropertyChanged));

		private static void ClickThroughPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

			if (d is CommentScreen window)
			{

				window.SetClickThrough((bool)e.NewValue);

			}

		}

		#endregion

		#endregion

		#region const values

		private const int GWL_STYLE = (-16); // ウィンドウスタイル
		private const int GWL_EXSTYLE = (-20); // 拡張ウィンドウスタイル

		//private const int WS_SYSMENU = 0x00080000; // システムメニュを表示する
		private const int WS_EX_TRANSPARENT = 0x00000020; // 透過ウィンドウスタイル

		private const int WM_SYSKEYDOWN = 0x0104; // Alt + 任意のキー の入力

		private const int VK_F4 = 0x73;

		#endregion

		#region Win32Apis

		[DllImport("user32")]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

		#endregion


		protected override void OnSourceInitialized(EventArgs e)
		{

			//システムメニュを非表示
			this.SetShowSystemMenu(this.ShowSystemMenu);

			//クリックをスルー
			this.SetClickThrough(this.ClickThrough);

			//Alt + F4 を無効化
			var handle = new WindowInteropHelper(this).Handle;
			var hwndSource = HwndSource.FromHwnd(handle);
			hwndSource.AddHook(WndProc);
			base.OnSourceInitialized(e);
		}

		protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr IParam, ref bool handled)
		{

			//Alt + F4 が入力されたら
			if (msg == WM_SYSKEYDOWN && wParam.ToInt32() == VK_F4)
			{

				if (this.AltF4Cancel)
				{

					//処理済みにセットする
					//(Windowは閉じられなくなる)
					handled = true;

				}

			}

			return IntPtr.Zero;

		}

		/// <summary>
		/// システムメニュの表示を切り替える
		/// </summary>
		/// <param name="value"><see langword="true"/> = 表示, <see langword="false"/> = 非表示</param>
		protected void SetShowSystemMenu(bool value)
		{

			try
			{

				var handle = new WindowInteropHelper(this).Handle;

				int windowStyle = GetWindowLong(handle, GWL_STYLE);

				if (value)
				{
					//  windowStyle |= WS_SYSMENU; //フラグの追加
				}
				else
				{
					//    windowStyle &= ~WS_SYSMENU; //フラグを消す
				}

				SetWindowLong(handle, GWL_STYLE, windowStyle);

			}
			catch
			{

			}

		}

		/// <summary>
		/// クリックスルーの設定
		/// </summary>
		/// <param name="value"><see langword="true"/> = クリックをスルー, <see langword="false"/>=クリックを捉える</param>
		protected void SetClickThrough(bool value)
		{

			try
			{

				var handle = new WindowInteropHelper(this).Handle;

				int extendStyle = GetWindowLong(handle, GWL_EXSTYLE);

				if (value)
				{
					extendStyle |= WS_EX_TRANSPARENT; //フラグの追加
				}
				else
				{
					extendStyle &= ~WS_EX_TRANSPARENT; //フラグを消す
				}

				SetWindowLong(handle, GWL_EXSTYLE, extendStyle);

			}
			catch
			{

			}

		}



		public CommentScreen()
		{
			InitializeComponent();
		}

		public void Addtext(object msg)
		{
			int lcount = 0;
			string regex = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
			double wid = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			var message = JsonConvert.DeserializeObject<MisskeyReceiveObj>(msg.ToString());
			Byte[] tb = System.Text.Encoding.Default.GetBytes(message.body.body.text);
			//string str = System.Text.Encoding.GetEncoding(932).GetString(message.Byt);
			//string str1 = System.Text.Encoding.UTF8.GetString(message.Byt);
			string Text = message.body.body.text;
			Text = Regex.Replace(Text,@"(#[a-z|A-Z]*)","");
			Text = Regex.Replace(Text, regex, "");
			Text = Text.Trim();


			int fontsz = 75;
			int textspeed = 7000;
			double textsize;
			int area = 0;
			//wid = 1920;
			int ttxetsize = 0;
			int ttxetsize1 = 0;
			Boolean findtext = true;
			switch (area)
			{
				case 1:
					for (int i = 0; i < uetexthi.Length && findtext; i++)
					{
						if (uetextcnt[i] == 0)
						{
							uetextcnt[i] = 1;
							uetexthi[i] = fontsz;
							lcount = i;
							findtext = false;
						}
						else
						{
							ttxetsize += uetexthi[i];
						}

					}
					break;

				case 2:
					for (int i = 0; i < nakatexthi.Length && findtext; i++)
					{
						if (i == 0 || i % 2 == 0)
						{
							if (nakatextcnt[i] == 0)
							{
								nakatextcnt[i] = 1;
								nakatexthi[i] = fontsz;
								lcount = i;
								findtext = false;
							}
							else
							{
								ttxetsize1 -= nakatexthi[i];
							}
						}
						else if (i % 2 == 1)
						{
							if (nakatextcnt[i] == 0)
							{
								nakatextcnt[i] = 1;
								nakatexthi[i] = fontsz;
								lcount = i;
								findtext = false;
							}
							else
							{
								ttxetsize += nakatexthi[i];
							}
						}
					}
					if (lcount % 2 == 1)
					{
						int hei = (int)SystemParameters.PrimaryScreenHeight / 2;
						ttxetsize = hei + ttxetsize1;

					}
					else
					{
						int hei = (int)SystemParameters.PrimaryScreenHeight / 2;
						ttxetsize = hei + ttxetsize;
					}

					ttxetsize -= fontsz;
					break;
				case 3:
					for (int i = 0; i < shitatexthi.Length && findtext; i++)
					{
						if (shitatextcnt[i] == 0)
						{
							shitatextcnt[i] = 1;
							shitatexthi[i] = fontsz;
							lcount = i;
							findtext = false;
						}
						else
						{
							ttxetsize -= shitatexthi[i];
						}

					}
					ttxetsize = (int)SystemParameters.PrimaryScreenHeight + ttxetsize - fontsz;
					break;
				default:
					break;
			}
			Font font = new Font("Arial", fontsz);
			Bitmap bitmap = new Bitmap(500, 500);
			System.Drawing.Graphics g = Graphics.FromImage(bitmap);
			SizeF sizeF = g.MeasureString(Text, font);
			textsize = sizeF.Width;
			g.Dispose();
			bitmap.Dispose();
			var converter = new System.Windows.Media.BrushConverter();
			var brush = TextColor;

			Label label1 = new Label
			{
				Content = Text,
				Margin = new Thickness(wid, ttxetsize, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				FontSize = fontsz,
				Foreground = brush,
			};
			Label source = (Label)label1;

			grid1.Children.Add(label1);
			grid1.UpdateLayout();
			TranslateTransform transform = label1.RenderTransform as TranslateTransform;
			if (transform == null)
			{
				source.RenderTransformOrigin = label1.PointFromScreen(new System.Windows.Point());
				transform = new TranslateTransform();
				source.RenderTransform = transform;
			}

			transform.BeginAnimation(TranslateTransform.XProperty,
			new DoubleAnimation(-(textsize + wid), TimeSpan.FromMilliseconds(textspeed)),
			HandoffBehavior.Compose);


			//label1.Width = Auto;
			DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(textspeed) };
			timer.Start();
			timer.Tick += (s, args) =>
			{
				// タイマーの停止
				timer.Stop();
				grid1.Children.Remove(label1);
				//textcnt[lcount] = 0;

				// 以下に待機後の処理を書く
			};
			DispatcherTimer timer12 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(textspeed / 4) };
			timer12.Start();
			timer12.Tick += (s, args) =>
			{
				// タイマーの停止
				timer12.Stop();
				//grid1.Children.Remove(label1);
				switch (area)
				{
					case 1:
						uetextcnt[lcount] = 0;
						break;
					case 2:
						nakatextcnt[lcount] = 0;
						break;
					case 3:
						shitatextcnt[lcount] = 0;
						break;
					default:
						break;
				}

				// 以下に待機後の処理を書く
			};

			//Console.WriteLine(label1);
			//Console.WriteLine(label1);

		}
		public void ChanegAllowsTransparency(bool flag)
		{
			this.AllowsTransparency = flag;
		}
	}
}
