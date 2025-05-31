using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Win32;

namespace PredatorSense
{
	// Token: 0x0200006B RID: 107
	public partial class Popup_Keyassignment : Window
	{
		// Token: 0x06000352 RID: 850
		[DllImport("User32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000353 RID: 851
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hwnd, uint wMsg, UIntPtr wParam, IntPtr lParam);

		// Token: 0x06000354 RID: 852 RVA: 0x00028694 File Offset: 0x00026894
		public Popup_Keyassignment(Button key_button, string current_hotkey_profile)
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			this.key_content = (HotkeyProfileXML.KeyContent)key_button.Tag;
			this.show_hotkey_Button.Template = key_button.Template;
			this.show_hotkey_Button.Background = key_button.Background;
			this.new_name = "";
			this.new_name_list = new List<string>();
			this.hotkey_profile_name = current_hotkey_profile;
			this.none_ComboBoxItem.Content = "(" + Startup.langRd["MUI_None"].ToString() + ")";
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0002878A File Offset: 0x0002698A
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0002879F File Offset: 0x0002699F
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000287B0 File Offset: 0x000269B0
		private void key_assignment_Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((this.key_assignment_Combox.SelectedIndex >= 0 && this.option_Combox.SelectedIndex >= 0) || this.key_assignment_Combox.SelectedIndex == 6)
			{
				this.OK_Button.IsEnabled = true;
			}
			else
			{
				this.OK_Button.IsEnabled = false;
			}
			string text = "";
			string text2 = "";
			this.option_Label.Content = Startup.langRd["MUI_Options"].ToString();
			this.path_TextBox.Visibility = Visibility.Hidden;
			this.browse_Button.Visibility = Visibility.Hidden;
			this.option_Combox.Visibility = Visibility.Visible;
			this.option_Label.Visibility = Visibility.Visible;
			this.option_Combox.Text = "";
			if (this.key_assignment_Combox.SelectedIndex == 0)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 0, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "0", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 1, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "1", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 2, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "2", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 3, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "3", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 1)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 0, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "0", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 1, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "1", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 2, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "2", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 3, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "3", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 2)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 0, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "0", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 1, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "1", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 2, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "2", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 3, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "3", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 3)
			{
				this.option_Combox.Visibility = Visibility.Hidden;
				this.option_Label.Content = Startup.langRd["MUI_Path"].ToString();
				this.path_TextBox.Visibility = Visibility.Visible;
				this.browse_Button.Visibility = Visibility.Visible;
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 4)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 0, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "0", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 1, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "1", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				CommonFunction.Get_type_and_option_name(this.key_assignment_Combox.SelectedIndex, 2, ref text, ref text2);
				this.option_Combox.Items.Add(this.add_option_comboxitems(text2, "2", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 5)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				this.initial_macro_profile_combobox(true);
				return;
			}
			if (this.key_assignment_Combox.SelectedIndex == 6)
			{
				this.option_Combox.IsEnabled = true;
				this.option_Combox.Items.Clear();
				this.option_Combox.Visibility = Visibility.Hidden;
				this.option_Label.Visibility = Visibility.Hidden;
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00028FE4 File Offset: 0x000271E4
		private ComboBoxItem add_option_comboxitems(string name, string tag, string bg_color_hex, Thickness thickness, string border_color_hex, HorizontalAlignment horizon)
		{
			return new ComboBoxItem
			{
				Content = name,
				Style = (Style)base.Resources["ComboBoxItemStyle"],
				Tag = tag,
				Background = new SolidColorBrush(CommonFunction.ColorFromString(bg_color_hex)),
				BorderThickness = thickness,
				BorderBrush = new SolidColorBrush(CommonFunction.ColorFromString(border_color_hex)),
				HorizontalContentAlignment = horizon
			};
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00029054 File Offset: 0x00027254
		private void cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00029064 File Offset: 0x00027264
		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			this.key_content.type = this.key_assignment_Combox.SelectedIndex;
			if (this.key_assignment_Combox.SelectedIndex == 3)
			{
				this.key_content.option = 0;
				this.key_content.userDefineName = this.path_TextBox.Text;
			}
			else if (this.key_assignment_Combox.SelectedIndex == 5)
			{
				this.key_content.option = 0;
				this.key_content.userDefineName = this.option_Combox.Text;
			}
			else if (this.key_assignment_Combox.SelectedIndex == 6)
			{
				this.key_content.option = 0;
				this.key_content.userDefineName = "";
			}
			else
			{
				this.key_content.option = this.option_Combox.SelectedIndex;
				this.key_content.userDefineName = "";
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0002914C File Offset: 0x0002734C
		public HotkeyProfileXML.KeyContent Get_select_item_result()
		{
			return this.key_content;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00029154 File Offset: 0x00027354
		private void option_Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((this.key_assignment_Combox.SelectedIndex >= 0 && this.option_Combox.SelectedIndex >= 0) || this.key_assignment_Combox.SelectedIndex == 6)
			{
				this.OK_Button.IsEnabled = true;
			}
			else
			{
				this.OK_Button.IsEnabled = false;
			}
			if (this.option_Combox.SelectedIndex == 0 && this.key_assignment_Combox.SelectedIndex == 5)
			{
				base.Opacity = 0.0;
				PopupNewProfile popupNewProfile = new PopupNewProfile(CommonFunction.Profile_Type.pMacro, "");
				popupNewProfile.Owner = this;
				popupNewProfile.ShowDialog();
				base.Opacity = 1.0;
				if (popupNewProfile.DialogResult.Value)
				{
					this.new_name = popupNewProfile.GetNewPorfileName();
					this.option_Combox.Items.Add(this.add_option_comboxitems(this.new_name, "0", "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left));
					this.option_Combox.SelectedIndex = this.option_Combox.Items.Count - 1;
					IntPtr intPtr = Popup_Keyassignment.FindWindow(null, "PredatorSense ");
					if (intPtr != IntPtr.Zero)
					{
						Popup_Keyassignment.PostMessage(intPtr, 32773U, new UIntPtr(0U), new IntPtr(0));
					}
					this.new_name_list.Add(this.new_name);
					return;
				}
				this.option_Combox.SelectedIndex = -1;
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000292DF File Offset: 0x000274DF
		public bool Get_type_isnone()
		{
			return this.key_assignment_Combox.SelectedIndex == 6;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000292F4 File Offset: 0x000274F4
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.key_assignment_Combox.SelectedIndex = this.key_content.type;
			if (this.key_content.type == 5)
			{
				if (this.key_content.userDefineName != "")
				{
					for (int i = 1; i < this.option_Combox.Items.Count; i++)
					{
						ComboBoxItem comboBoxItem = (ComboBoxItem)this.option_Combox.Items[i];
						if (this.key_content.userDefineName == comboBoxItem.Content.ToString())
						{
							this.option_Combox.SelectedIndex = i;
							return;
						}
					}
					return;
				}
			}
			else
			{
				if (this.key_content.type == 3)
				{
					this.path_TextBox.Text = this.key_content.userDefineName;
					return;
				}
				if (this.key_content.type != 6)
				{
					this.option_Combox.SelectedIndex = this.key_content.option;
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000293E8 File Offset: 0x000275E8
		private void browse_Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Files (*.*)|*.*";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			openFileDialog.Title = Startup.langRd["MUI_Open"].ToString();
			if (openFileDialog.ShowDialog() == true)
			{
				this.path_TextBox.Text = openFileDialog.FileName;
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00029464 File Offset: 0x00027664
		private void path_TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (textBox.Text != "")
			{
				this.OK_Button.IsEnabled = true;
				return;
			}
			this.OK_Button.IsEnabled = false;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000294A4 File Offset: 0x000276A4
		private void initial_macro_profile_combobox(bool retext)
		{
			this.option_Combox.Items.Clear();
			int num = 0;
			if (retext)
			{
				this.option_Combox.Text = CommonFunction.select_macro_name;
			}
			ComboBoxItem comboBoxItem = this.add_option_comboxitems("+ " + Startup.langRd["MUI_New_Macro"].ToString(), num.ToString(), "#CCCCCC", new Thickness(0.0, 0.0, 0.0, 2.0), "#191919", HorizontalAlignment.Center);
			this.option_Combox.Items.Add(comboBoxItem);
			DirectoryInfo directoryInfo = new DirectoryInfo(CommonFunction.macro_profile_path.Replace(CommonFunction.macro_profile_path_replace_name, this.hotkey_profile_name));
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				num++;
				comboBoxItem = this.add_option_comboxitems(Path.GetFileNameWithoutExtension(fileInfo.FullName), num.ToString(), "#DADADA", new Thickness(0.0, 0.0, 0.0, 1.0), "#999999", HorizontalAlignment.Left);
				comboBoxItem.Focusable = true;
				this.option_Combox.Items.Add(comboBoxItem);
			}
			if (!this.option_Combox.Items.IsEmpty)
			{
				this.option_Combox.SelectedIndex = -1;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00029610 File Offset: 0x00027810
		public bool IsNewMacro()
		{
			if (this.new_name_list.Count > 0)
			{
				for (int i = 0; i < this.new_name_list.Count; i++)
				{
					if (this.option_Combox.Text == this.new_name_list[i].ToString())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00029667 File Offset: 0x00027867
		public string GetNewName()
		{
			return this.option_Combox.Text;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00029674 File Offset: 0x00027874
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

		// Token: 0x0400043F RID: 1087
		private HotkeyProfileXML.KeyContent key_content = new HotkeyProfileXML.KeyContent();

		// Token: 0x04000440 RID: 1088
		private string hotkey_profile_name = "";

		// Token: 0x04000441 RID: 1089
		private string new_name = "";

		// Token: 0x04000442 RID: 1090
		private List<string> new_name_list = new List<string>();
	}
}
