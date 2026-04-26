using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PredatorSense
{
	// Token: 0x0200000B RID: 11
	public partial class More_color_picker : Window
	{
		// Token: 0x060000BE RID: 190
		[DllImport("gdi32.dll")]
		internal static extern int GetDeviceCaps(IntPtr hdc, int Index);

		// Token: 0x060000BF RID: 191
		[DllImport("user32.dll")]
		internal static extern IntPtr GetDC(IntPtr Hwnd);

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00008D0D File Offset: 0x00006F0D
		internal static double GetDpiX
		{
			get
			{
				return (double)More_color_picker.GetDeviceCaps(More_color_picker.GetDC(IntPtr.Zero), 88);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00008D21 File Offset: 0x00006F21
		internal static double GetDpiY
		{
			get
			{
				return (double)More_color_picker.GetDeviceCaps(More_color_picker.GetDC(IntPtr.Zero), 90);
			}
		}

		// Token: 0x060000C2 RID: 194
		[DllImport("user32.dll")]
		private static extern bool ClipCursor(ref global::System.Drawing.Rectangle rect);

		// Token: 0x060000C3 RID: 195
		[DllImport("user32.dll")]
		private static extern bool ClipCursor(IntPtr rect);

		// Token: 0x060000C4 RID: 196
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		// Token: 0x060000C5 RID: 197 RVA: 0x00008D38 File Offset: 0x00006F38
		private BitmapSource BitmapToBitmapSource(Bitmap bitmap)
		{
			IntPtr hbitmap = bitmap.GetHbitmap();
			BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			More_color_picker.DeleteObject(hbitmap);
			return bitmapSource;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00008D6A File Offset: 0x00006F6A
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00008D72 File Offset: 0x00006F72
		public global::System.Drawing.Color SelectedColor
		{
			get
			{
				return this.selectedColor;
			}
			private set
			{
				if (this.selectedColor != value)
				{
					this.selectedColor = value;
					this.draw_light_index++;
					this.draw_light_bitmap(this.selectedColor, this.draw_light_index);
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00008DA9 File Offset: 0x00006FA9
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00008DB1 File Offset: 0x00006FB1
		public global::System.Drawing.Color ChangeColor
		{
			get
			{
				return this.changeColor;
			}
			private set
			{
				this.changeColor = value;
				this.show_color_index++;
				this.draw_show_color_bitmap(value, this.show_color_index);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00008DD8 File Offset: 0x00006FD8
		public More_color_picker(byte r, byte g, byte b)
		{
			int[] array = new int[3];
			this.backup_number = array;
			byte[] array2 = new byte[3];
			this.outside_color = array2;
			//base..ctor();
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new global::System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			this.show_color_index = 0;
			this.draw_light_index = 0;
			this.color_r_Label.Content = Startup.langRd["MUI_Red_R"].ToString() + " : ";
			this.color_g_Label.Content = Startup.langRd["MUI_Green_G"].ToString() + " : ";
			this.color_b_Label.Content = Startup.langRd["MUI_Blue_B"].ToString() + " : ";
			this.outside_color[0] = r;
			this.outside_color[1] = g;
			this.outside_color[2] = b;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00008F08 File Offset: 0x00007108
		private void mainWindow_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00008F20 File Offset: 0x00007120
		public static More_color_picker.ColorRGB HSL2RGB(double h, double sl, double l)
		{
			double num = l;
			double num2 = l;
			double num3 = l;
			double num4 = ((l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl));
			if (num4 > 0.0)
			{
				double num5 = l + l - num4;
				double num6 = (num4 - num5) / num4;
				h *= 6.0;
				int num7 = (int)h;
				double num8 = h - (double)num7;
				double num9 = num4 * num6 * num8;
				double num10 = num5 + num9;
				double num11 = num4 - num9;
				switch (num7)
				{
				case 0:
					num = num4;
					num2 = num10;
					num3 = num5;
					break;
				case 1:
					num = num11;
					num2 = num4;
					num3 = num5;
					break;
				case 2:
					num = num5;
					num2 = num4;
					num3 = num10;
					break;
				case 3:
					num = num5;
					num2 = num11;
					num3 = num4;
					break;
				case 4:
					num = num10;
					num2 = num5;
					num3 = num4;
					break;
				case 5:
					num = num4;
					num2 = num5;
					num3 = num11;
					break;
				}
			}
			More_color_picker.ColorRGB colorRGB;
			colorRGB.R = Convert.ToByte(num * 255.0);
			colorRGB.G = Convert.ToByte(num2 * 255.0);
			colorRGB.B = Convert.ToByte(num3 * 255.0);
			return colorRGB;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009048 File Offset: 0x00007248
		public static void RGB2HSL(More_color_picker.ColorRGB rgb, out double h, out double s, out double l)
		{
			double num = (double)rgb.R / 255.0;
			double num2 = (double)rgb.G / 255.0;
			double num3 = (double)rgb.B / 255.0;
			h = 0.0;
			s = 0.0;
			l = 0.0;
			double num4 = Math.Max(num, num2);
			num4 = Math.Max(num4, num3);
			double num5 = Math.Min(num, num2);
			num5 = Math.Min(num5, num3);
			l = (num5 + num4) / 2.0;
			if (l <= 0.0)
			{
				return;
			}
			double num6 = num4 - num5;
			s = num6;
			if (s > 0.0)
			{
				s /= ((l <= 0.5) ? (num4 + num5) : (2.0 - num4 - num5));
				double num7 = (num4 - num) / num6;
				double num8 = (num4 - num2) / num6;
				double num9 = (num4 - num3) / num6;
				if (num == num4)
				{
					h = ((num2 == num5) ? (5.0 + num9) : (1.0 - num8));
				}
				else if (num2 == num4)
				{
					h = ((num3 == num5) ? (1.0 + num7) : (3.0 - num9));
				}
				else
				{
					h = ((num == num5) ? (3.0 + num8) : (5.0 - num7));
				}
				h /= 6.0;
				h = ((h == 1.0) ? 0.0 : h);
				return;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000091E4 File Offset: 0x000073E4
		private void UpdateColor(bool text_input, int pos_x, int pos_y)
		{
			byte[] array = new byte[4];
			if (!text_input)
			{
				int num = (int)Mouse.GetPosition(this.image_Canvas).X;
				int num2 = (int)Mouse.GetPosition(this.image_Canvas).Y;
				if (num < 0 || num2 < 0 || (double)num > this.Color_image.Width - 1.0 || (double)num2 > this.Color_image.Height - 1.0)
				{
					return;
				}
				CroppedBitmap croppedBitmap = new CroppedBitmap(this.Color_image.Source as BitmapSource, new Int32Rect(num, num2, 1, 1));
				croppedBitmap.CopyPixels(array, 4, 0);
				this.color_sight_Rectangle.SetValue(Canvas.LeftProperty, Mouse.GetPosition(this.image_Canvas).X - this.color_sight_Rectangle.Width / 2.0);
				this.color_sight_Rectangle.SetValue(Canvas.TopProperty, Mouse.GetPosition(this.image_Canvas).Y - this.color_sight_Rectangle.Width / 2.0);
			}
			else
			{
				int num3 = ((pos_x < 0) ? 0 : pos_x);
				int num4 = ((pos_x > (int)(this.Color_image.Width - 1.0)) ? ((int)(this.Color_image.Width - 1.0)) : pos_x);
				int num5 = ((pos_y < 0) ? 0 : pos_y);
				int num6 = ((pos_y > (int)(this.Color_image.Height - 1.0)) ? ((int)(this.Color_image.Height - 1.0)) : pos_y);
				CroppedBitmap croppedBitmap2 = new CroppedBitmap(this.Color_image.Source as BitmapSource, new Int32Rect(num4, num6, 1, 1));
				croppedBitmap2.CopyPixels(array, 4, 0);
			}
			this.SelectedColor = global::System.Drawing.Color.FromArgb((int)array[2], (int)array[1], (int)array[0]);
			global::System.Drawing.Color color = this.transfer_light_rgb(global::System.Drawing.Color.FromArgb((int)array[2], (int)array[1], (int)array[0]));
			if (!text_input)
			{
				this.All_Color_Textbox_Change(color, true);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000093F0 File Offset: 0x000075F0
		private double PointsToPixels(double wpfPoints, More_color_picker.LengthDirection direction)
		{
			if (direction == More_color_picker.LengthDirection.Horizontal)
			{
				return wpfPoints * (double)Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.WorkArea.Width;
			}
			return wpfPoints * (double)Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.WorkArea.Height;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009448 File Offset: 0x00007648
		private void image_Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			More_color_picker.ClipCursor(IntPtr.Zero);
			global::System.Windows.Point point = this.image_Canvas.PointToScreen(new global::System.Windows.Point(0.0, 0.0));
			global::System.Drawing.Rectangle rectangle = new global::System.Drawing.Rectangle((int)point.X + 1, (int)point.Y + 1, (int)(point.X + this.PointsToPixels(this.image_Canvas.Width, More_color_picker.LengthDirection.Horizontal)), (int)(point.Y + this.PointsToPixels(this.image_Canvas.Height, More_color_picker.LengthDirection.Vertical)));
			More_color_picker.ClipCursor(ref rectangle);
			this.IsMouseDown = true;
			this.color_sight_Rectangle.Focus();
			this.UpdateColor(false, 0, 0);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000094FD File Offset: 0x000076FD
		private void image_Canvas_LeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.IsMouseDown = false;
			More_color_picker.ClipCursor(IntPtr.Zero);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009511 File Offset: 0x00007711
		private void image_Canvas_MouseMove(object sender, global::System.Windows.Input.MouseEventArgs e)
		{
			if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				if (this.IsMouseDown)
				{
					this.IsMouseDown = false;
					More_color_picker.ClipCursor(IntPtr.Zero);
				}
				return;
			}
			if (this.IsMouseDown)
			{
				this.UpdateColor(false, 0, 0);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009547 File Offset: 0x00007747
		private void image_Canvas_MouseLeave(object sender, global::System.Windows.Input.MouseEventArgs e)
		{
			this.IsMouseDown = false;
			More_color_picker.ClipCursor(IntPtr.Zero);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000955C File Offset: 0x0000775C
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Bitmap bitmap = new Bitmap(240, 220);
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					double num = (double)i / (double)bitmap.Width;
					double num2 = (double)j / (double)(bitmap.Height - 1);
					global::System.Drawing.Color color = More_color_picker.HSL2RGB(num, num2, 0.5);
					bitmap.SetPixel(i, bitmap.Height - 1 - j, color);
				}
			}
			this.Color_image.Source = this.BitmapToBitmapSource(bitmap);
			Thumb thumb = (this.color_Slider.Template.FindName("PART_Track", this.color_Slider) as Track).Thumb;
			thumb.MouseEnter += this.thumb_MouseEnter;
			this.color_Slider.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.color_Slider_LostFocus), true);
			this.color_sight_Rectangle.Focusable = true;
			global::System.Drawing.Color color2 = global::System.Drawing.Color.FromArgb((int)this.outside_color[0], (int)this.outside_color[1], (int)this.outside_color[2]);
			this.All_Color_Textbox_Change(color2, false);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000967C File Offset: 0x0000787C
		private double over_range_check(double origin, double limit)
		{
			double num = origin;
			if (origin > limit)
			{
				num = limit;
			}
			else if (origin < 0.0)
			{
				num = 0.0;
			}
			return num;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000096AC File Offset: 0x000078AC
		private void Color_TextBox_PreviewKeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					e.Handled = true;
				}
				else if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
				{
					if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
					{
						e.Handled = true;
					}
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00009744 File Offset: 0x00007944
		private void Color_TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			global::System.Windows.Controls.TextBox textBox = sender as global::System.Windows.Controls.TextBox;
			if (textBox.Text == "")
			{
				textBox.Text = "0";
				return;
			}
			int num = 0;
			if (!int.TryParse(textBox.Text, out num))
			{
				textBox.Text = this.backup_number[Convert.ToInt32(textBox.Tag.ToString())].ToString();
				return;
			}
			this.backup_number[Convert.ToInt32(textBox.Tag.ToString())] = num;
			if (num > 255)
			{
				textBox.Text = "255";
				return;
			}
			if (num < 0)
			{
				textBox.Text = "0";
				return;
			}
			if (this.color_r_TextBox.Text != "" && this.color_g_TextBox.Text != "" && this.color_b_TextBox.Text != "" && this.immediate_change)
			{
				this.slider_lock = true;
				int num2 = Convert.ToInt32(this.color_r_TextBox.Text);
				int num3 = Convert.ToInt32(this.color_g_TextBox.Text);
				int num4 = Convert.ToInt32(this.color_b_TextBox.Text);
				if (num2 >= 0 && num2 <= 255 && num3 >= 0 && num3 <= 255 && num4 >= 0 && num4 <= 255)
				{
					this.ChangeColor = global::System.Drawing.Color.FromArgb(num2, num3, num4);
					if (this.cursor_move)
					{
						More_color_picker.ColorRGB colorRGB = new More_color_picker.ColorRGB(this.ChangeColor);
						double num5 = 0.0;
						double num6 = 0.0;
						double num7 = 0.0;
						More_color_picker.RGB2HSL(colorRGB, out num5, out num6, out num7);
						this.color_Slider.Value = num7 * this.color_Slider.Maximum;
						double num8 = num5 * this.Color_image.Width;
						double num9 = this.Color_image.Height - num6 * (this.Color_image.Height - 1.0);
						this.color_sight_Rectangle.SetValue(Canvas.LeftProperty, num8 - this.color_sight_Rectangle.Width / 2.0);
						this.color_sight_Rectangle.SetValue(Canvas.TopProperty, num9 - this.color_sight_Rectangle.Width / 2.0);
						this.UpdateColor(true, Convert.ToInt32(num8), Convert.ToInt32(num9));
					}
				}
				this.slider_lock = false;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00009A78 File Offset: 0x00007C78
		private void draw_show_color_bitmap(global::System.Drawing.Color select_color, int index_number)
		{
			Bitmap show_color_bitmap = new Bitmap((int)this.show_color_Image.Width, (int)this.show_color_Image.Height);
			ThreadStart threadStart = delegate
			{
				for (int i = 0; i < show_color_bitmap.Height; i++)
				{
					for (int j = 0; j < show_color_bitmap.Width; j++)
					{
						show_color_bitmap.SetPixel(j, i, select_color);
					}
				}
				this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
				{
					this.show_color_Image.Source = this.BitmapToBitmapSource(show_color_bitmap);
					this.show_color_index = 0;
				}));
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00009BE8 File Offset: 0x00007DE8
		private void draw_light_bitmap(global::System.Drawing.Color select_color, int index_number)
		{
			Bitmap light_bitmap = new Bitmap((int)this.Color_light_image.Width, (int)this.Color_light_image.Height);
			ThreadStart threadStart = delegate
			{
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				More_color_picker.ColorRGB colorRGB = new More_color_picker.ColorRGB(select_color);
				More_color_picker.RGB2HSL(colorRGB, out num, out num2, out num3);
				for (int i = 0; i < light_bitmap.Height; i++)
				{
					double num4 = (double)i / (double)(light_bitmap.Height - 1);
					global::System.Drawing.Color color = More_color_picker.HSL2RGB(num, num2, num4);
					for (int j = 0; j < light_bitmap.Width; j++)
					{
						light_bitmap.SetPixel(j, light_bitmap.Height - 1 - i, color);
					}
				}
				this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
				{
					this.Color_light_image.Source = this.BitmapToBitmapSource(light_bitmap);
					this.draw_light_index = 0;
				}));
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00009C44 File Offset: 0x00007E44
		private void thumb_MouseEnter(object sender, global::System.Windows.Input.MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
			{
				MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
				mouseButtonEventArgs.RoutedEvent = UIElement.MouseLeftButtonDownEvent;
				(sender as Thumb).RaiseEvent(mouseButtonEventArgs);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00009C94 File Offset: 0x00007E94
		private void color_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (!this.slider_lock)
			{
				this.cursor_move = false;
				this.color_Slider.Focus();
				double num = e.NewValue / this.color_Slider.Maximum;
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				More_color_picker.RGB2HSL(new More_color_picker.ColorRGB(this.SelectedColor), out num3, out num4, out num2);
				More_color_picker.ColorRGB colorRGB = new More_color_picker.ColorRGB(More_color_picker.HSL2RGB(num3, num4, num));
				this.All_Color_Textbox_Change(colorRGB, false);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00009D25 File Offset: 0x00007F25
		private void color_Slider_LostFocus(object sender, RoutedEventArgs e)
		{
			this.cursor_move = true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00009D30 File Offset: 0x00007F30
		public global::System.Drawing.Color transfer_light_rgb(global::System.Drawing.Color temp_color)
		{
			double num = this.color_Slider.Value / this.color_Slider.Maximum;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			More_color_picker.RGB2HSL(new More_color_picker.ColorRGB(temp_color), out num3, out num4, out num2);
			return temp_color = new More_color_picker.ColorRGB(More_color_picker.HSL2RGB(num3, num4, num));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00009DA0 File Offset: 0x00007FA0
		private void Color_light_image_Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.cursor_move = false;
			this.color_Slider.Focus();
			int num = (int)(this.Color_image.Height - 1.0) - (int)Mouse.GetPosition(this.Color_light_image_Canvas).Y;
			this.color_Slider.Value = Convert.ToDouble(num);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009DFD File Offset: 0x00007FFD
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00009E0B File Offset: 0x0000800B
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00009E19 File Offset: 0x00008019
		private void color_Slider_PreviewKeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			e.Handled = true;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00009E24 File Offset: 0x00008024
		private void All_Color_Textbox_Change(global::System.Drawing.Color color_, bool cursor_move_change_)
		{
			if (cursor_move_change_)
			{
				this.cursor_move = false;
			}
			int num = 0;
			if (this.color_b_TextBox.Text != color_.B.ToString())
			{
				num = 2;
			}
			else if (this.color_g_TextBox.Text != color_.G.ToString())
			{
				num = 1;
			}
			else if (this.color_r_TextBox.Text != color_.R.ToString())
			{
				num = 0;
			}
			this.immediate_change = false;
			for (int i = 0; i < 3; i++)
			{
				if (i == num)
				{
					this.immediate_change = true;
				}
				if (i == 0)
				{
					this.color_r_TextBox.Text = color_.R.ToString();
				}
				else if (i == 1)
				{
					this.color_g_TextBox.Text = color_.G.ToString();
				}
				else if (i == 2)
				{
					this.color_b_TextBox.Text = color_.B.ToString();
				}
			}
			if (cursor_move_change_)
			{
				this.cursor_move = true;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00009F34 File Offset: 0x00008134
		private void AdjustWindowSizeAndPos(bool owner_center)
		{
			double width = SystemParameters.WorkArea.Width;
			double height = SystemParameters.WorkArea.Height;
			double num = base.Width / base.Height;
			if (base.Height > height)
			{
				base.Height = height;
				base.Width = base.Height * num;
			}
			if (!owner_center)
			{
				base.Left = width / 2.0 - base.Width / 2.0;
				base.Top = height / 2.0 - base.Height / 2.0;
			}
		}

		// Token: 0x040000CA RID: 202
		private const int LOGPIXELSX = 88;

		// Token: 0x040000CB RID: 203
		private const int LOGPIXELSY = 90;

		// Token: 0x040000CC RID: 204
		private bool IsMouseDown;

		// Token: 0x040000CD RID: 205
		private global::System.Drawing.Color selectedColor = global::System.Drawing.Color.Transparent;

		// Token: 0x040000CE RID: 206
		private global::System.Drawing.Color changeColor = global::System.Drawing.Color.Transparent;

		// Token: 0x040000CF RID: 207
		private int show_color_index;

		// Token: 0x040000D0 RID: 208
		private int draw_light_index;

		// Token: 0x040000D1 RID: 209
		private bool cursor_move = true;

		// Token: 0x040000D2 RID: 210
		private bool slider_lock;

		// Token: 0x040000D3 RID: 211
		private bool immediate_change = true;

		// Token: 0x040000D4 RID: 212
		private int[] backup_number;

		// Token: 0x040000D5 RID: 213
		private byte[] outside_color;

		// Token: 0x0200000C RID: 12
		public struct ColorRGB
		{
			// Token: 0x060000E6 RID: 230 RVA: 0x0000A30D File Offset: 0x0000850D
			public ColorRGB(global::System.Drawing.Color value)
			{
				this.R = value.R;
				this.G = value.G;
				this.B = value.B;
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x0000A338 File Offset: 0x00008538
			public static implicit operator global::System.Drawing.Color(More_color_picker.ColorRGB rgb)
			{
				return global::System.Drawing.Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B);
			}

			// Token: 0x060000E8 RID: 232 RVA: 0x0000A361 File Offset: 0x00008561
			public static explicit operator More_color_picker.ColorRGB(global::System.Drawing.Color c)
			{
				return new More_color_picker.ColorRGB(c);
			}

			// Token: 0x040000EA RID: 234
			public byte R;

			// Token: 0x040000EB RID: 235
			public byte G;

			// Token: 0x040000EC RID: 236
			public byte B;
		}

		// Token: 0x0200000D RID: 13
		public enum LengthDirection
		{
			// Token: 0x040000EE RID: 238
			Vertical,
			// Token: 0x040000EF RID: 239
			Horizontal
		}
	}
}
