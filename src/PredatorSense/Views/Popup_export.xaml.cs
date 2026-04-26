using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using ICSharpCode.SharpZipLib.Zip;

namespace PredatorSense
{
	// Token: 0x02000015 RID: 21
	public partial class Popup_export : Window
	{
		// Token: 0x06000164 RID: 356 RVA: 0x000108D0 File Offset: 0x0000EAD0
		public Popup_export(List<string> conflic_item, string des_path, string profile_root_path, CommonFunction.Profile_Type type)
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			this.g_type = type;
			if (this.g_type == CommonFunction.Profile_Type.pHotkey)
			{
				this.extension_name = ".apsh";
			}
			else if (CommonFunction.Profile_Type.pLighting == this.g_type)
			{
				this.extension_name = ".apsl";
			}
			this.current_profile_path = profile_root_path;
			this.g_conflic_item = conflic_item;
			this.g_des_path = des_path;
			this.next();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00010994 File Offset: 0x0000EB94
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000109A9 File Offset: 0x0000EBA9
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000109B8 File Offset: 0x0000EBB8
		private void next()
		{
			if (this.preview_index == this.g_conflic_item.Count)
			{
				base.DialogResult = new bool?(true);
				return;
			}
			this.profile_name_TextBlock.Text = this.g_conflic_item[this.preview_index];
			for (int i = 1; i < 256; i++)
			{
				string text = string.Concat(new object[]
				{
					this.profile_name_TextBlock.Text,
					" (",
					i,
					")",
					this.extension_name
				});
				string text2 = global::System.IO.Path.Combine(this.g_des_path, text);
				if (!File.Exists(text2))
				{
					this.keep_content_TextBlock.Text = Startup.langRd["MUI_Eeport_Rename_Info"].ToString().Replace("%Name%", text);
					this.g_new_name = text;
					break;
				}
			}
			if (this.g_conflic_item.Count - this.preview_index - 1 == 0)
			{
				this.check_conflic_StackPanel.Visibility = Visibility.Hidden;
			}
			if (this.g_conflic_item.Count - this.preview_index - 1 == 1)
			{
				this.do_this_content_TextBlock.Text = Startup.langRd["MUI_Conflict_Info"].ToString();
				return;
			}
			this.do_this_content_TextBlock.Text = Startup.langRd["MUI_Conflicts_Info"].ToString().Replace("%N%", (this.g_conflic_item.Count - this.preview_index - 1).ToString());
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00010B3C File Offset: 0x0000ED3C
		private void replace_Button_Click(object sender, RoutedEventArgs e)
		{
			if (this.do_this_CheckBox.IsChecked == true)
			{
				while (this.preview_index < this.g_conflic_item.Count)
				{
					Popup_export.ZipDir(this.current_profile_path + this.g_conflic_item[this.preview_index], this.g_des_path + "\\" + this.g_conflic_item[this.preview_index] + this.extension_name, "acer3345678", false);
					this.preview_index++;
					this.next();
				}
				return;
			}
			Popup_export.ZipDir(this.current_profile_path + this.g_conflic_item[this.preview_index], this.g_des_path + "\\" + this.g_conflic_item[this.preview_index] + this.extension_name, "acer3345678", false);
			this.preview_index++;
			this.next();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00010C48 File Offset: 0x0000EE48
		private void skip_Button_Click(object sender, RoutedEventArgs e)
		{
			if (this.do_this_CheckBox.IsChecked == true)
			{
				base.DialogResult = new bool?(true);
				return;
			}
			this.preview_index++;
			this.next();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00010C98 File Offset: 0x0000EE98
		private void keep_Button_Click(object sender, RoutedEventArgs e)
		{
			if (this.do_this_CheckBox.IsChecked == true)
			{
				while (this.preview_index < this.g_conflic_item.Count)
				{
					Popup_export.ZipDir(this.current_profile_path + this.g_conflic_item[this.preview_index], this.g_des_path + "\\" + this.g_new_name, "acer3345678", false);
					this.preview_index++;
					this.next();
				}
				return;
			}
			Popup_export.ZipDir(this.current_profile_path + this.g_conflic_item[this.preview_index], this.g_des_path + "\\" + this.g_new_name, "acer3345678", false);
			this.preview_index++;
			this.next();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00010D80 File Offset: 0x0000EF80
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

		// Token: 0x0600016C RID: 364 RVA: 0x00010E44 File Offset: 0x0000F044
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

		// Token: 0x040001A8 RID: 424
		private List<string> g_conflic_item;

		// Token: 0x040001A9 RID: 425
		private string g_des_path = "";

		// Token: 0x040001AA RID: 426
		private string g_new_name = "";

		// Token: 0x040001AB RID: 427
		private int preview_index;

		// Token: 0x040001AC RID: 428
		private string current_profile_path = "";

		// Token: 0x040001AD RID: 429
		private CommonFunction.Profile_Type g_type;

		// Token: 0x040001AE RID: 430
		private string extension_name = "";
	}
}
