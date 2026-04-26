using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PredatorSense
{
	// Token: 0x02000004 RID: 4
	public partial class dataItem : UserControl
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000013 RID: 19 RVA: 0x0000269C File Offset: 0x0000089C
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x000026D4 File Offset: 0x000008D4
		public event EventHandler ExecuteMethod;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000015 RID: 21 RVA: 0x0000270C File Offset: 0x0000090C
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x00002744 File Offset: 0x00000944
		public event EventHandler ExecuteMethod_callBackreWrite;

		// Token: 0x06000017 RID: 23 RVA: 0x00002779 File Offset: 0x00000979
		protected virtual void OnExecuteMethod()
		{
			if (this.ExecuteMethod != null)
			{
				this.ExecuteMethod(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002794 File Offset: 0x00000994
		protected virtual void rewriteXML()
		{
			if (this.ExecuteMethod_callBackreWrite != null)
			{
				this.ExecuteMethod_callBackreWrite(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027B0 File Offset: 0x000009B0
		public dataItem()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.g_ms_text = Startup.langRd["MUI_MS"].ToString();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002830 File Offset: 0x00000A30
		public dataItem(int time)
		{
			this.InitializeComponent();
			try
			{
				if (Startup._TTFont)
				{
					base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
				}
				this.g_ms_text = Startup.langRd["MUI_MS"].ToString();
				this.keyin.Visibility = Visibility.Visible;
				this.keyValue.Visibility = Visibility.Collapsed;
				base.AllowDrop = false;
				this.textState.Text = string.Empty;
				this.dataState.Tag = "delay";
				this.ms.Content = this.g_ms_text;
				this.keyin.Text = "";
				this.keyin.MaxLength = 8;
				this.keyin.Width = 85.0;
				this.keyin.PreviewKeyDown += this.keyin_PreviewKeyDown;
				this.keyin.Focusable = true;
				this.keyin.LostFocus += this.keyin_LostFocus;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002974 File Offset: 0x00000B74
		private void deleteItem_click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.deleteFunc = true;
				this.OnExecuteMethod();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000029A4 File Offset: 0x00000BA4
		public void setState(string state, string value)
		{
			try
			{
				this.dataState.Tag = state;
				if (state != null)
				{
					if (state == "down")
					{
						this.downG.Visibility = Visibility.Visible;
						this.textState.Text = string.Empty;
						goto IL_0085;
					}
					if (state == "up")
					{
						this.upG.Visibility = Visibility.Visible;
						this.textState.Text = string.Empty;
						goto IL_0085;
					}
				}
				this.time.Visibility = Visibility.Visible;
				this.textState.Text = string.Empty;
				IL_0085:
				this.keyValue.Text = value;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002A58 File Offset: 0x00000C58
		public void setState(string state, string value, string virsualKey)
		{
			try
			{
				this.dataState.Tag = state;
				if (state != null)
				{
					if (state == "down")
					{
						this.downG.Visibility = Visibility.Visible;
						this.textState.Text = Startup.langRd["MUI_Key_Down"].ToString();
						goto IL_00C3;
					}
					if (state == "up")
					{
						this.upG.Visibility = Visibility.Visible;
						this.textState.Text = Startup.langRd["MUI_Key_up"].ToString();
						goto IL_00C3;
					}
				}
				this.time.Visibility = Visibility.Visible;
				this.textState.Text = Startup.langRd["MUI_Time_Delay"].ToString();
				this.ms.Content = this.g_ms_text;
				IL_00C3:
				this.virsualKeyName = virsualKey;
				this.keyValue.Text = value;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B50 File Offset: 0x00000D50
		public void setDeleteBTag(object ins)
		{
			try
			{
				this.deleteB.Tag = ins;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B80 File Offset: 0x00000D80
		public object getTag()
		{
			object obj;
			try
			{
				obj = this.deleteB.Tag;
			}
			catch (Exception)
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public string getstate()
		{
			string text;
			try
			{
				text = (string)this.dataState.Tag;
			}
			catch (Exception)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public string getVirsualKey()
		{
			string empty;
			try
			{
				empty = this.virsualKeyName;
			}
			catch (Exception)
			{
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C20 File Offset: 0x00000E20
		public void setVirsualKey(string keyname)
		{
			try
			{
				this.virsualKeyName = keyname;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C4C File Offset: 0x00000E4C
		public void setSelected(bool sel)
		{
			try
			{
				if (this.selected != sel)
				{
					this.selected = sel;
					if (this.selected)
					{
						this.deleteB.Visibility = Visibility.Visible;
						base.Background = new SolidColorBrush
						{
							Color = CommonFunction.ColorFromString("#191919")
						};
					}
					else
					{
						this.deleteB.Visibility = Visibility.Collapsed;
						base.Background = new SolidColorBrush
						{
							Color = Colors.Transparent
						};
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CD8 File Offset: 0x00000ED8
		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			try
			{
				if (!this.selected)
				{
					this.deleteB.Visibility = Visibility.Visible;
					base.Background = new SolidColorBrush
					{
						Color = CommonFunction.ColorFromString("#404040")
					};
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D30 File Offset: 0x00000F30
		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			try
			{
				if (!this.selected)
				{
					this.deleteB.Visibility = Visibility.Collapsed;
					base.Background = new SolidColorBrush
					{
						Color = Colors.Transparent
					};
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D80 File Offset: 0x00000F80
		private void UserControl_MouseMove(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D82 File Offset: 0x00000F82
		public void showLine()
		{
			this.dropLine.Visibility = Visibility.Visible;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D90 File Offset: 0x00000F90
		public void unshowLine()
		{
			this.dropLine.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DA0 File Offset: 0x00000FA0
		private void keyin_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			try
			{
				this.keyin.SelectAll();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002DD0 File Offset: 0x00000FD0
		private void keyin_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.keyin.Text == string.Empty)
				{
					this.keyValue.Text = this.oldValue;
					this.virsualKeyName = this.oldValue;
				}
				else
				{
					this.keyValue.Text = int.Parse(this.keyin.Text).ToString();
					this.virsualKeyName = int.Parse(this.keyin.Text).ToString();
				}
				this.textState.Text = Startup.langRd["MUI_Time_Delay"].ToString();
				this.ms.Content = this.g_ms_text;
				this.time.Visibility = Visibility.Visible;
				this.keyin.Visibility = Visibility.Collapsed;
				this.keyValue.Visibility = Visibility.Visible;
				base.AllowDrop = true;
				this.rewriteXML();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002ECC File Offset: 0x000010CC
		private void keyin_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					this.keyin.Visibility = Visibility.Collapsed;
				}
				if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
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

		// Token: 0x0600002C RID: 44 RVA: 0x00002F68 File Offset: 0x00001168
		public void setBackground(byte o, byte r, byte g, byte b)
		{
			try
			{
				base.Background = new SolidColorBrush
				{
					Color = Color.FromArgb(o, r, g, b)
				};
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002FA8 File Offset: 0x000011A8
		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
			{
				try
				{
					if (this.dataState.Tag.Equals("delay") && this.keyin.Visibility != Visibility.Visible)
					{
						this.oldValue = this.keyValue.Text;
						this.OnExecuteMethod();
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x04000011 RID: 17
		private bool selected;

		// Token: 0x04000012 RID: 18
		public string oldValue = string.Empty;

		// Token: 0x04000013 RID: 19
		public bool deleteFunc;

		// Token: 0x04000014 RID: 20
		private string virsualKeyName = string.Empty;

		// Token: 0x04000015 RID: 21
		private string g_ms_text = "ms";
	}
}
