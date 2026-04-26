using System;
using System.CodeDom.Compiler;
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
	// Token: 0x02000017 RID: 23
	public partial class Popup_import : Window
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00011344 File Offset: 0x0000F544
		public Popup_import(string conflic_name, string src_path, string profile_root_path, CommonFunction.Profile_Type type)
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			this.g_type = type;
			this.current_profile_path = profile_root_path;
			this.profile_name_TextBlock.Text = conflic_name;
			this.g_src_path = src_path;
			try
			{
				for (int i = 1; i < 256; i++)
				{
					string text = string.Concat(new object[] { conflic_name, " (", i, ")" });
					string text2 = global::System.IO.Path.Combine(profile_root_path, text);
					if (!Directory.Exists(text2))
					{
						this.keep_content_TextBlock.Text = Startup.langRd["MUI_Import_Rename_Info"].ToString().Replace("%Name%", text);
						this.g_new_name = text;
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00011470 File Offset: 0x0000F670
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00011485 File Offset: 0x0000F685
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00011494 File Offset: 0x0000F694
		private void replace_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.choice_flag = 1;
				if (!File.Exists(this.g_src_path))
				{
					this.g_result = false;
					base.DialogResult = new bool?(false);
				}
				else
				{
					Task<int> task = CommonFunction.delete_folder(this.current_profile_path + this.profile_name_TextBlock.Text);
					int result = task.Result;
					if (!Popup_import.ExtractZip(this.g_src_path, this.current_profile_path + this.profile_name_TextBlock.Text, "acer3345678"))
					{
						this.g_result = false;
						base.DialogResult = new bool?(false);
					}
					else
					{
						if (this.g_type == CommonFunction.Profile_Type.pHotkey && !Directory.Exists(this.current_profile_path + this.profile_name_TextBlock.Text + "\\MacroKeyPool"))
						{
							Directory.CreateDirectory(this.current_profile_path + this.profile_name_TextBlock.Text + "\\MacroKeyPool");
						}
						base.DialogResult = new bool?(true);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001159C File Offset: 0x0000F79C
		private void skip_Button_Click(object sender, RoutedEventArgs e)
		{
			this.choice_flag = -1;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000115B4 File Offset: 0x0000F7B4
		private void keep_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.choice_flag = 2;
				string text = this.current_profile_path + this.g_new_name;
				if (!Popup_import.ExtractZip(this.g_src_path, text, "acer3345678"))
				{
					this.g_result = false;
					base.DialogResult = new bool?(false);
				}
				else
				{
					if (this.g_type == CommonFunction.Profile_Type.pHotkey && !Directory.Exists(text + "\\MacroKeyPool"))
					{
						Directory.CreateDirectory(text + "\\MacroKeyPool");
					}
					base.DialogResult = new bool?(true);
				}
			}
			catch (Exception)
			{
				this.g_result = false;
				base.DialogResult = new bool?(false);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00011664 File Offset: 0x0000F864
		public static void DeleteFolder(string directoryPath)
		{
			try
			{
				foreach (string text in Directory.GetFileSystemEntries(directoryPath))
				{
					if (File.Exists(text))
					{
						FileInfo fileInfo = new FileInfo(text);
						if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						File.Delete(text);
					}
					else
					{
						Popup_import.DeleteFolder(text);
					}
				}
				Directory.Delete(directoryPath);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000116EC File Offset: 0x0000F8EC
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

		// Token: 0x06000180 RID: 384 RVA: 0x000117E4 File Offset: 0x0000F9E4
		public bool get_result()
		{
			return this.g_result;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000117EC File Offset: 0x0000F9EC
		public int get_flag()
		{
			return this.choice_flag;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000117F4 File Offset: 0x0000F9F4
		public string get_newName()
		{
			if (this.choice_flag == 1)
			{
				return this.profile_name_TextBlock.Text;
			}
			if (this.choice_flag == 2)
			{
				return this.g_new_name;
			}
			return string.Empty;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00011820 File Offset: 0x0000FA20
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

		// Token: 0x040001C6 RID: 454
		private string g_src_path = "";

		// Token: 0x040001C7 RID: 455
		private string current_profile_path = "";

		// Token: 0x040001C8 RID: 456
		private string g_new_name = "";

		// Token: 0x040001C9 RID: 457
		private int choice_flag;

		// Token: 0x040001CA RID: 458
		private bool g_result = true;

		// Token: 0x040001CB RID: 459
		private CommonFunction.Profile_Type g_type;
	}
}
