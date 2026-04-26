using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000018 RID: 24
	public partial class ProfileManager : Window
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00011A38 File Offset: 0x0000FC38
		public ProfileManager(CommonFunction.Profile_Type type)
		{
			this.InitializeComponent();
			this.g_type = type;
			if (this.g_type == CommonFunction.Profile_Type.pHotkey)
			{
				this.Title_TextBlock.Text = Startup.langRd["MUI_Hotkey_Profile_Manager"].ToString();
				this.current_profile_path = CommonFunction.hotkey_profile_path;
				if (global::TsDotNetLib.Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile"))
				{
					this.current_profile_name = global::TsDotNetLib.Registry.GetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile");
				}
				this.extension_name = ".apsh";
				this.filter_name = "apsh file (*.apsh)|*.apsh";
			}
			else if (CommonFunction.Profile_Type.pLighting == this.g_type)
			{
				this.Title_TextBlock.Text = Startup.langRd["MUI_Lighting_Profile_Manager"].ToString();
				this.current_profile_path = CommonFunction.lighting_profile_path;
				if (global::TsDotNetLib.Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\LightSetting", "LightingProfile"))
				{
					this.current_profile_name = global::TsDotNetLib.Registry.GetStringLM("SOFTWARE\\OEM\\PredatorSense\\LightSetting", "LightingProfile");
				}
				this.extension_name = ".apsl";
				this.filter_name = "apsl file (*.apsl)|*.apsl";
			}
			this.current_profile_content_path = global::System.IO.Path.Combine(this.current_profile_path, this.current_profile_name);
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00011BCA File Offset: 0x0000FDCA
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00011BED File Offset: 0x0000FDED
		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00011BFC File Offset: 0x0000FDFC
		private void profile_rename(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.new_profile_click)
				{
					this.profile_add_Button.IsEnabled = false;
				}
				ListBoxItem listBoxItem = (ListBoxItem)sender;
				if (!listBoxItem.Content.Equals("Default"))
				{
					int num = this.profile_ListBox.Items.IndexOf(listBoxItem);
					this.profile_ListBox.SelectedIndex = num;
					listBoxItem.Visibility = Visibility.Collapsed;
					ListBoxItem listBoxItem2 = new ListBoxItem();
					listBoxItem2.ContentTemplate = (DataTemplate)base.Resources["ListBoxItemTemplateTextbox"];
					listBoxItem2.Content = listBoxItem.Content.ToString();
					listBoxItem2.Foreground = Brushes.White;
					listBoxItem2.Tag = listBoxItem;
					this.tempRemoveItem = listBoxItem2;
					this.profile_ListBox.Items.Insert(num, listBoxItem2);
					new Thread(new ParameterizedThreadStart(this.ListBoxItemDelayFocus)).Start(listBoxItem2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00011DEC File Offset: 0x0000FFEC
		private void ListBoxItemDelayFocus(object data)
		{
			try
			{
				Thread.Sleep(100);
				ListBoxItem listbox = (ListBoxItem)data;
				if (base.Dispatcher.CheckAccess())
				{
					listbox.Focus();
				}
				else
				{
					base.Dispatcher.Invoke(delegate
					{
						listbox.Focus();
						ContentPresenter contentPresenter = this.FindVisualChildForListbox<ContentPresenter>(listbox);
						global::System.Windows.Controls.TextBox textBox = (global::System.Windows.Controls.TextBox)listbox.ContentTemplate.FindName("editName", contentPresenter);
						textBox.LostFocus += this.profileItem_LostFocus;
						textBox.GotFocus += this.profileItem_GotFocus;
						textBox.KeyDown += this.testBox_KeyDown;
						TextCompositionManager.AddPreviewTextInputStartHandler(textBox, new TextCompositionEventHandler(this.PreviewTextInput_CheckInvalidCharacters));
						TextCompositionManager.AddPreviewTextInputUpdateHandler(textBox, new TextCompositionEventHandler(this.PreviewTextInput_CheckInvalidCharacters));
						textBox.TextChanged += this.TextChanged_CheckInvalidCharacters;
						textBox.Focus();
					});
				}
				base.Dispatcher.Invoke(delegate
				{
					if (this.new_profile_click)
					{
						this.profile_add_Button.IsEnabled = true;
						this.new_profile_click = false;
					}
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00011E80 File Offset: 0x00010080
		private void TextChanged_CheckInvalidCharacters(object sender, TextChangedEventArgs e)
		{
			global::System.Windows.Controls.TextBox textBox = sender as global::System.Windows.Controls.TextBox;
			string text = textBox.Text;
			if (textBox.Text.Length > 0)
			{
				foreach (char c in global::System.IO.Path.GetInvalidFileNameChars())
				{
					text = text.Replace(c.ToString(), "");
				}
			}
			if (text != textBox.Text)
			{
				textBox.Text = text;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00011EF0 File Offset: 0x000100F0
		private void PreviewTextInput_CheckInvalidCharacters(object sender, TextCompositionEventArgs e)
		{
			try
			{
				if (e.TextComposition.CompositionText.Length > 0 || e.Text.Length > 0)
				{
					foreach (char c in global::System.IO.Path.GetInvalidFileNameChars())
					{
						if (c.ToString() == e.Text || c.ToString() == e.TextComposition.CompositionText)
						{
							e.Handled = true;
							return;
						}
					}
					e.Handled = false;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00011F88 File Offset: 0x00010188
		private void testBox_KeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					this.profile_ListBox.Focusable = true;
					this.profile_ListBox.Focus();
					this.profile_ListBox.Focusable = false;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00011FD8 File Offset: 0x000101D8
		private void profileItem_GotFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				global::System.Windows.Controls.TextBox textBox = (global::System.Windows.Controls.TextBox)sender;
				textBox.SelectAll();
				Keyboard.Focus(textBox);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00012010 File Offset: 0x00010210
		private void reWriteProfileName(string new_name)
		{
			try
			{
				if (this.g_type == CommonFunction.Profile_Type.pHotkey)
				{
					try
					{
						Directory.Move(CommonFunction.hotkey_profile_path + this.current_profile_name, CommonFunction.hotkey_profile_path + new_name);
					}
					catch (Exception)
					{
						CommonFunction.exception_move_folder(CommonFunction.hotkey_profile_path + this.current_profile_name, CommonFunction.hotkey_profile_path + new_name, new_name);
					}
					global::TsDotNetLib.Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile", new_name);
				}
				else if (CommonFunction.Profile_Type.pLighting == this.g_type)
				{
					try
					{
						Directory.Move(CommonFunction.lighting_profile_path + this.current_profile_name, CommonFunction.lighting_profile_path + new_name);
					}
					catch (Exception)
					{
						CommonFunction.exception_move_folder(CommonFunction.lighting_profile_path + this.current_profile_name, CommonFunction.lighting_profile_path + new_name, new_name);
					}
					global::TsDotNetLib.Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\LightSetting", "LightingProfile", new_name);
				}
				this.current_profile_name = new_name;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00012110 File Offset: 0x00010310
		private void profileItem_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				global::System.Windows.Controls.TextBox textBox = (global::System.Windows.Controls.TextBox)sender;
				ListBoxItem listBoxItem = (ListBoxItem)textBox.Tag;
				textBox.Text = textBox.Text.Trim(CommonFunction.all_whitespaces);
				if (textBox.Text.Trim() == "")
				{
					this.profile_ListBox.Items.Remove(this.tempRemoveItem);
					listBoxItem.Visibility = Visibility.Visible;
				}
				else if (Directory.Exists(this.current_profile_path + textBox.Text))
				{
					if (listBoxItem.Content.ToString() != textBox.Text.ToString())
					{
						base.Opacity = 0.0;
						new Attention(CommonFunction.Attention_Type.aProfile_Same_Name, "")
						{
							Owner = this
						}.ShowDialog();
						base.Opacity = 1.0;
						this.profile_ListBox.Items.Remove(this.tempRemoveItem);
						listBoxItem.Visibility = Visibility.Visible;
						this.profile_rename(listBoxItem, null);
					}
					else
					{
						this.profile_ListBox.Items.Remove(this.tempRemoveItem);
						listBoxItem.Visibility = Visibility.Visible;
					}
				}
				else
				{
					listBoxItem.Content.ToString();
					listBoxItem.Content = textBox.Text;
					listBoxItem.Tag = this.current_profile_path + textBox.Text;
					this.profile_ListBox.Items.Remove(this.tempRemoveItem);
					listBoxItem.Visibility = Visibility.Visible;
					this.reWriteProfileName(listBoxItem.Content.ToString());
					listBoxItem.IsSelected = false;
					listBoxItem.IsSelected = true;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000122C4 File Offset: 0x000104C4
		private childItem FindVisualChildForListbox<childItem>(DependencyObject obj) where childItem : DependencyObject
		{
			childItem childItem2;
			try
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(obj, i);
					if (child != null && child is childItem)
					{
						return (childItem)((object)child);
					}
					childItem childItem1 = this.FindVisualChildForListbox<childItem>(child);
					if (childItem1 != null)
					{
						return childItem1;
					}
				}
				childItem2 = default(childItem);
			}
			catch (Exception)
			{
				childItem2 = default(childItem);
			}
			return childItem2;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00012340 File Offset: 0x00010540
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.profile_ListBox.Items.Clear();
			List<string> list = new List<string>(Directory.GetDirectories(this.current_profile_path));
			this.load_default_listboxitem();
			foreach (string text in list)
			{
				if (!(text.Substring(text.LastIndexOf("\\") + 1) == CommonFunction.default_content_name))
				{
					ListBoxItem listBoxItem = new ListBoxItem();
					listBoxItem.Content = text.Substring(text.LastIndexOf("\\") + 1);
					listBoxItem.Tag = text;
					listBoxItem.Selected += this.selectedProfileItemFocus;
					if (!(listBoxItem.Content.ToString() == CommonFunction.default_content_name))
					{
						listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(this.profile_rename);
					}
					listBoxItem.Style = (Style)base.Resources["profile_listboxitem_Style"];
					listBoxItem.ContentTemplate = (DataTemplate)base.Resources["profile_listboxitem_DataTemplate"];
					this.profile_ListBox.Items.Add(listBoxItem);
					if (listBoxItem.Content.Equals(this.current_profile_name))
					{
						this.profile_ListBox.SelectedIndex = this.profile_ListBox.Items.IndexOf(listBoxItem);
						this.profile_ListBox.ScrollIntoView(listBoxItem);
					}
					if (listBoxItem.Content.Equals(this.current_profile_name))
					{
						this.current_profile_index = this.profile_ListBox.Items.IndexOf(listBoxItem);
					}
				}
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000124F0 File Offset: 0x000106F0
		private void selectedProfileItemFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000124F4 File Offset: 0x000106F4
		private void load_default_listboxitem()
		{
			ListBoxItem listBoxItem = new ListBoxItem();
			listBoxItem.Content = CommonFunction.default_content_name;
			listBoxItem.Selected += this.selectedProfileItemFocus;
			if (!(listBoxItem.Content.ToString() == CommonFunction.default_content_name))
			{
				listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(this.profile_rename);
			}
			listBoxItem.Style = (Style)base.Resources["profile_listboxitem_Style"];
			listBoxItem.ContentTemplate = (DataTemplate)base.Resources["profile_listboxitem_DataTemplate"];
			this.profile_ListBox.Items.Add(listBoxItem);
			if (listBoxItem.Content.Equals(this.current_profile_name))
			{
				this.profile_ListBox.SelectedIndex = this.profile_ListBox.Items.IndexOf(listBoxItem);
				this.profile_ListBox.ScrollIntoView(listBoxItem);
			}
			if (listBoxItem.Content.Equals(this.current_profile_name))
			{
				this.current_profile_index = this.profile_ListBox.Items.IndexOf(listBoxItem);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000125FA File Offset: 0x000107FA
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main_Grid.Focus();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00012608 File Offset: 0x00010808
		private void profile_add_Button_Click(object sender, RoutedEventArgs e)
		{
			string text = "";
			string text2 = "";
			bool flag = true;
			this.new_profile_click = true;
			for (int i = 1; i < 256; i++)
			{
				text = CommonFunction.profile_content_name + i.ToString();
				text2 = global::System.IO.Path.Combine(this.current_profile_path, text);
				if (!Directory.Exists(text2))
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				if (this.g_type == CommonFunction.Profile_Type.pHotkey)
				{
					HotkeyProfileXML.ResetDefaultHotkeyProfile(text);
				}
				else if (CommonFunction.Profile_Type.pLighting == this.g_type)
				{
					LightingProfileXML.ResetDefaultLightProfile(text);
				}
			}
			ListBoxItem listBoxItem = new ListBoxItem();
			listBoxItem.Content = text;
			listBoxItem.Tag = text2;
			listBoxItem.Selected += this.selectedProfileItemFocus;
			if (listBoxItem.Content.ToString() != CommonFunction.default_content_name)
			{
				listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(this.profile_rename);
			}
			listBoxItem.Style = (Style)base.Resources["profile_listboxitem_Style"];
			listBoxItem.ContentTemplate = (DataTemplate)base.Resources["profile_listboxitem_DataTemplate"];
			this.profile_ListBox.Items.Add(listBoxItem);
			this.profile_ListBox.SelectedItem = listBoxItem;
			this.profile_ListBox.ScrollIntoView(listBoxItem);
			listBoxItem.Focus();
			this.profile_rename(listBoxItem, null);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00012752 File Offset: 0x00010952
		private void profile_more_Button_Click(object sender, RoutedEventArgs e)
		{
			if (!this.more_Popup.IsOpen)
			{
				this.more_Popup.IsOpen = true;
				return;
			}
			this.more_Popup.IsOpen = false;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0001277C File Offset: 0x0001097C
		private void profile_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.profile_ListBox.SelectedIndex == 0)
			{
				this.profile_delete_Button.IsEnabled = false;
			}
			else
			{
				this.profile_delete_Button.IsEnabled = true;
			}
			if (this.profile_ListBox.SelectedIndex != -1)
			{
				ListBoxItem listBoxItem = new ListBoxItem();
				listBoxItem = (ListBoxItem)this.profile_ListBox.Items[this.profile_ListBox.SelectedIndex];
				this.current_profile_name = listBoxItem.Content.ToString();
				this.current_profile_content_path = global::System.IO.Path.Combine(this.current_profile_path, this.current_profile_name);
				this.current_profile_index = this.profile_ListBox.SelectedIndex;
				if (this.g_type == CommonFunction.Profile_Type.pHotkey)
				{
					global::TsDotNetLib.Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile", this.current_profile_name);
				}
				else if (CommonFunction.Profile_Type.pLighting == this.g_type)
				{
					global::TsDotNetLib.Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\LightSetting", "LightingProfile", this.current_profile_name);
				}
				this.rewrite_root_name();
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00012864 File Offset: 0x00010A64
		private void rewrite_root_name()
		{
			if (File.Exists(this.current_profile_content_path + "\\Main.xml"))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.current_profile_content_path + "\\Main.xml");
				XmlNode xmlNode = xmlDocument.SelectSingleNode("ROOT");
				XmlElement xmlElement = (XmlElement)xmlNode;
				xmlElement.SetAttribute("name", this.current_profile_name);
				try
				{
					xmlDocument.Save(this.current_profile_content_path + "\\Main.xml");
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(this.current_profile_content_path + "\\Main.xml");
					int result = task.Result;
					xmlDocument.Save(this.current_profile_content_path + "\\Main.xml");
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0001292C File Offset: 0x00010B2C
		private void more_Popup_Closed(object sender, EventArgs e)
		{
			if (this.Mask_Rectangle.Visibility == Visibility.Visible)
			{
				this.Mask_Rectangle.Visibility = Visibility.Hidden;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00012948 File Offset: 0x00010B48
		private void duplicate_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string text = string.Empty;
				string text2 = string.Empty;
				for (int i = 1; i < 256; i++)
				{
					text = string.Concat(new object[] { this.current_profile_name, " (", i, ")" });
					text2 = global::System.IO.Path.Combine(this.current_profile_path, text);
					if (!Directory.Exists(text2))
					{
						ProfileManager.DirectoryCopy(this.current_profile_content_path, text2, true);
						ListBoxItem listBoxItem = new ListBoxItem();
						listBoxItem.Content = text;
						listBoxItem.Tag = text2;
						listBoxItem.Selected += this.selectedProfileItemFocus;
						if (!(listBoxItem.Content.ToString() == CommonFunction.default_content_name))
						{
							listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(this.profile_rename);
						}
						listBoxItem.Style = (Style)base.Resources["profile_listboxitem_Style"];
						listBoxItem.ContentTemplate = (DataTemplate)base.Resources["profile_listboxitem_DataTemplate"];
						this.profile_ListBox.Items.Insert(this.current_profile_index + 1, listBoxItem);
						this.profile_ListBox.ScrollIntoView(listBoxItem);
						this.profile_ListBox.SelectedItem = listBoxItem;
						this.profile_ListBox.ScrollIntoView(listBoxItem);
						break;
					}
				}
				this.more_Popup.IsOpen = false;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00012AC4 File Offset: 0x00010CC4
		private void duplicate_action(string source_file_path, string target_file_path, bool copy_sub_dirs)
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00012AC8 File Offset: 0x00010CC8
		private static void DirectoryCopy(string source_file_path, string target_file_path, bool copy_sub_dirs)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(source_file_path);
				if (directoryInfo.Exists)
				{
					DirectoryInfo[] directories = directoryInfo.GetDirectories();
					if (!Directory.Exists(target_file_path))
					{
						Directory.CreateDirectory(target_file_path);
					}
					FileInfo[] files = directoryInfo.GetFiles();
					foreach (FileInfo fileInfo in files)
					{
						string text = global::System.IO.Path.Combine(target_file_path, fileInfo.Name);
						FileInfo fileInfo2 = new FileInfo(text);
						if (fileInfo2.Exists)
						{
							fileInfo2.IsReadOnly = false;
						}
						fileInfo.CopyTo(text, true);
					}
					if (copy_sub_dirs)
					{
						foreach (DirectoryInfo directoryInfo2 in directories)
						{
							string text2 = global::System.IO.Path.Combine(target_file_path, directoryInfo2.Name);
							ProfileManager.DirectoryCopy(directoryInfo2.FullName, text2, copy_sub_dirs);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00012BA8 File Offset: 0x00010DA8
		private void import_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
				openFileDialog.Filter = this.filter_name;
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Title = Startup.langRd["MUI_Import_Title"].ToString();
				if (openFileDialog.ShowDialog() == true)
				{
					bool flag = true;
					string text = global::System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					text = text.Trim(CommonFunction.all_whitespaces);
					new List<string>(Directory.GetDirectories(this.current_profile_path));
					int num = 0;
					if (Directory.Exists(this.current_profile_path + text))
					{
						base.Opacity = 0.0;
						Popup_import popup_import = new Popup_import(text, openFileDialog.FileName, this.current_profile_path, this.g_type);
						popup_import.Owner = this;
						popup_import.ShowDialog();
						base.Opacity = 1.0;
						text = popup_import.get_newName();
						flag = popup_import.get_result();
						num = popup_import.get_flag();
					}
					else if (ProfileManager.ExtractZip(openFileDialog.FileName, this.current_profile_path + text, "acer3345678"))
					{
						if (this.g_type == CommonFunction.Profile_Type.pHotkey && !Directory.Exists(this.current_profile_path + text + "\\MacroKeyPool"))
						{
							Directory.CreateDirectory(this.current_profile_path + text + "\\MacroKeyPool");
						}
					}
					else
					{
						flag = false;
					}
					if (!flag)
					{
						base.Opacity = 0.0;
						new Attention(CommonFunction.Attention_Type.aImport_Failed, this.current_profile_name)
						{
							Owner = this
						}.ShowDialog();
						base.Opacity = 1.0;
					}
					else if (num != -1)
					{
						this.profile_ListBox.Items.Clear();
						this.load_default_listboxitem();
						List<string> list = new List<string>(Directory.GetDirectories(this.current_profile_path));
						int num2 = -1;
						foreach (string text2 in list)
						{
							if (!(text2.Substring(text2.LastIndexOf("\\") + 1) == CommonFunction.default_content_name))
							{
								ListBoxItem listBoxItem = new ListBoxItem();
								listBoxItem.Content = text2.Substring(text2.LastIndexOf("\\") + 1);
								listBoxItem.Tag = text2;
								listBoxItem.Selected += this.selectedProfileItemFocus;
								if (!(listBoxItem.Content.ToString() == CommonFunction.default_content_name))
								{
									listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(this.profile_rename);
								}
								listBoxItem.Style = (Style)base.Resources["profile_listboxitem_Style"];
								listBoxItem.ContentTemplate = (DataTemplate)base.Resources["profile_listboxitem_DataTemplate"];
								this.profile_ListBox.Items.Add(listBoxItem);
								if (listBoxItem.Content.Equals(text))
								{
									this.profile_ListBox.SelectedIndex = this.profile_ListBox.Items.IndexOf(listBoxItem);
									this.profile_ListBox.ScrollIntoView(listBoxItem);
								}
								if (listBoxItem.Content.Equals(this.current_profile_name))
								{
									num2 = this.profile_ListBox.Items.IndexOf(listBoxItem);
								}
							}
						}
						if (this.profile_ListBox.SelectedIndex == -1)
						{
							this.profile_ListBox.SelectedIndex = num2;
						}
						ListBoxItem listBoxItem2 = (ListBoxItem)this.profile_ListBox.Items[this.profile_ListBox.SelectedIndex];
						List<string> list2 = new List<string>(Directory.GetDirectories(this.current_profile_path));
						foreach (string text3 in list2)
						{
							ComboBoxItem comboBoxItem = new ComboBoxItem();
							comboBoxItem.Content = text3.Substring(text3.LastIndexOf("\\") + 1);
							comboBoxItem.Tag = text3;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00012FE8 File Offset: 0x000111E8
		private void export_Button_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
			saveFileDialog.Filter = this.filter_name;
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.Title = Startup.langRd["MUI_Export"].ToString();
			saveFileDialog.FileName = this.current_profile_name;
			saveFileDialog.OverwritePrompt = false;
			bool? flag = saveFileDialog.ShowDialog();
			string text = string.Empty;
			string text2 = string.Empty;
			text2 = global::System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
			text = global::System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
			text = text.Trim(CommonFunction.all_whitespaces) + this.extension_name;
			saveFileDialog.FileName = global::System.IO.Path.Combine(text2, text);
			if (flag == true)
			{
				if (!Directory.Exists(this.current_profile_content_path))
				{
					return;
				}
				double num = (double)CommonFunction.GetDirectorySize(this.current_profile_content_path) / 1048576.0;
				string pathRoot = global::System.IO.Path.GetPathRoot(saveFileDialog.FileName);
				double num2 = CommonFunction.Get_Free_Size(pathRoot);
				if (num >= num2)
				{
					base.Opacity = 0.0;
					new Attention(CommonFunction.Attention_Type.aEnough_Storage, this.current_profile_name)
					{
						Owner = this
					}.ShowDialog();
					base.Opacity = 1.0;
					return;
				}
				List<string> list = new List<string>();
				if (File.Exists(saveFileDialog.FileName))
				{
					list.Add(this.current_profile_name);
				}
				if (list.Count != 0)
				{
					base.Opacity = 0.0;
					new Popup_export(list, global::System.IO.Path.GetDirectoryName(saveFileDialog.FileName), this.current_profile_path, this.g_type)
					{
						Owner = this
					}.ShowDialog();
					base.Opacity = 1.0;
					return;
				}
				ProfileManager.ZipDir(this.current_profile_content_path, saveFileDialog.FileName, "acer3345678", true);
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000131BC File Offset: 0x000113BC
		private void export_all_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.ShowDialog();
				if (!folderBrowserDialog.SelectedPath.Equals(""))
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>(Directory.GetDirectories(this.current_profile_path));
					foreach (string text in list2)
					{
						string text2 = text.Substring(text.LastIndexOf("\\") + 1);
						if (!Directory.Exists(this.current_profile_path + text2))
						{
							return;
						}
						double num = (double)CommonFunction.GetDirectorySize(this.current_profile_path + text2) / 1048576.0;
						string pathRoot = global::System.IO.Path.GetPathRoot(folderBrowserDialog.SelectedPath);
						double num2 = CommonFunction.Get_Free_Size(pathRoot);
						if (num >= num2)
						{
							base.Opacity = 0.0;
							new Attention(CommonFunction.Attention_Type.aEnough_Storage, this.current_profile_name)
							{
								Owner = this
							}.ShowDialog();
							base.Opacity = 1.0;
							return;
						}
						if (File.Exists(folderBrowserDialog.SelectedPath + "\\" + text2 + this.extension_name))
						{
							list.Add(text2);
						}
						else
						{
							ProfileManager.ZipDir(this.current_profile_path + text2, folderBrowserDialog.SelectedPath + "\\" + text2 + this.extension_name, "acer3345678", false);
						}
					}
					if (list.Count != 0)
					{
						base.Opacity = 0.0;
						new Popup_export(list, folderBrowserDialog.SelectedPath, this.current_profile_path, this.g_type)
						{
							Owner = this
						}.ShowDialog();
						base.Opacity = 1.0;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000133C4 File Offset: 0x000115C4
		private static bool ZipDir(string SourceDir, string TargetFile, string Password, bool BackupOldFile)
		{
			bool flag;
			try
			{
				FastZip fastZip = new FastZip();
				fastZip.EntryFactory = new ZipEntryFactory
				{
					IsUnicodeText = true
				};
				if (!Directory.Exists(SourceDir))
				{
					flag = false;
				}
				else
				{
					if (BackupOldFile && File.Exists(TargetFile))
					{
						File.Copy(TargetFile, TargetFile + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".back");
						try
						{
							File.Delete(TargetFile);
						}
						catch (Exception)
						{
							Task<int> task = CommonFunction.delete_file(TargetFile);
							int result = task.Result;
						}
					}
					if (!string.IsNullOrEmpty(Password))
					{
						fastZip.Password = Password;
					}
					fastZip.CreateZip(TargetFile, SourceDir, true, "");
					flag = true;
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00013488 File Offset: 0x00011688
		private static bool ExtractZip(string SourceFile, string TargetDir, string Password)
		{
			FastZip fastZip = new FastZip();
			fastZip.EntryFactory = new ZipEntryFactory
			{
				IsUnicodeText = true
			};
			bool flag = false;
			bool flag2;
			try
			{
				if (!File.Exists(SourceFile))
				{
					flag2 = false;
				}
				else
				{
					ZipFile zipFile = new ZipFile(SourceFile);
					string empty = string.Empty;
					foreach (object obj in zipFile)
					{
						ZipEntry zipEntry = (ZipEntry)obj;
						string fileName = global::System.IO.Path.GetFileName(zipEntry.Name);
						if (fileName == "Main.xml")
						{
							flag = true;
						}
					}
					if (!flag)
					{
						flag2 = false;
					}
					else
					{
						if (Password != "")
						{
							fastZip.Password = Password;
						}
						Directory.CreateDirectory(TargetDir);
						fastZip.ExtractZip(SourceFile, TargetDir, "");
						flag2 = true;
					}
				}
			}
			catch
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00013580 File Offset: 0x00011780
		private void profile_delete_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Attention attention = new Attention(CommonFunction.Attention_Type.aDelete_Profile, this.current_profile_name);
				this.main_mask_Rectangle.Visibility = Visibility.Visible;
				attention.Owner = this;
				attention.ShowDialog();
				this.main_mask_Rectangle.Visibility = Visibility.Hidden;
				if (attention.DialogResult.Value)
				{
					int selectedIndex = this.profile_ListBox.SelectedIndex;
					string text = this.current_profile_content_path;
					DirectoryInfo directoryInfo = new DirectoryInfo(text);
					if (directoryInfo.Exists)
					{
						Task<int> task = CommonFunction.delete_folder(text);
						int result = task.Result;
						this.profile_ListBox.Items.RemoveAt(selectedIndex);
					}
					if (selectedIndex == 0)
					{
						this.profile_ListBox.SelectedIndex = 0;
					}
					else
					{
						this.profile_ListBox.SelectedIndex = selectedIndex - 1;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0001364C File Offset: 0x0001184C
		private void more_Popup_Opened(object sender, EventArgs e)
		{
			this.Mask_Rectangle.Visibility = Visibility.Visible;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0001365C File Offset: 0x0001185C
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

		// Token: 0x040001D7 RID: 471
		private ListBoxItem tempRemoveItem = new ListBoxItem();

		// Token: 0x040001D8 RID: 472
		private string current_profile_path = "";

		// Token: 0x040001D9 RID: 473
		private string current_profile_content_path = "";

		// Token: 0x040001DA RID: 474
		public int current_profile_index = -1;

		// Token: 0x040001DB RID: 475
		public string current_profile_name = "";

		// Token: 0x040001DC RID: 476
		private CommonFunction.Profile_Type g_type;

		// Token: 0x040001DD RID: 477
		private bool new_profile_click;

		// Token: 0x040001DE RID: 478
		private string extension_name = "";

		// Token: 0x040001DF RID: 479
		private string filter_name = "";
	}
}
