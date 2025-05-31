using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000014 RID: 20
	public partial class PopupNewProfile : Window
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00010074 File Offset: 0x0000E274
		public PopupNewProfile(CommonFunction.Profile_Type type, string current_macro_name)
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
				this.title_TextBlock.Text = Startup.langRd["MUI_New_Hotkey_Profile"].ToString();
				this.current_profile_path = CommonFunction.hotkey_profile_path;
				for (int i = 1; i < 256; i++)
				{
					string text = CommonFunction.profile_content_name + i;
					if (!Directory.Exists(Path.Combine(this.current_profile_path, text)))
					{
						this.name_textBox.Text = text;
						this.name_textBox.Focus();
						this.name_textBox.SelectAll();
						break;
					}
				}
			}
			else if (CommonFunction.Profile_Type.pLighting == this.g_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_New_Lighting_Profile"].ToString();
				this.current_profile_path = CommonFunction.lighting_profile_path;
				for (int j = 1; j < 256; j++)
				{
					string text2 = CommonFunction.profile_content_name + j;
					if (!Directory.Exists(Path.Combine(this.current_profile_path, text2)))
					{
						this.name_textBox.Text = text2;
						this.name_textBox.Focus();
						this.name_textBox.SelectAll();
						break;
					}
				}
			}
			else if (CommonFunction.Profile_Type.pMacro == this.g_type)
			{
				if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile"))
				{
					this.current_profile_name = Registry.GetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile");
				}
				this.current_macro_content_path = CommonFunction.macro_profile_path.Replace(CommonFunction.macro_profile_path_replace_name, this.current_profile_name);
				this.title_TextBlock.Text = Startup.langRd["MUI_New_Macro"].ToString();
				for (int k = 1; k < 256; k++)
				{
					string text3 = CommonFunction.macro_content_name + k + ".xml";
					if (!File.Exists(Path.Combine(this.current_macro_content_path, text3)))
					{
						this.name_textBox.Text = CommonFunction.macro_content_name + k;
						this.name_textBox.Focus();
						this.name_textBox.SelectAll();
						break;
					}
				}
			}
			else if (CommonFunction.Profile_Type.pRename_Macro == this.g_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Rename_Title"].ToString();
				this.g_old_name = current_macro_name;
				this.name_textBox.Text = current_macro_name;
				this.name_textBox.Focus();
				this.name_textBox.SelectAll();
			}
			TextCompositionManager.AddPreviewTextInputStartHandler(this.name_textBox, new TextCompositionEventHandler(this.name_textBox_PreviewTextInput));
			TextCompositionManager.AddPreviewTextInputUpdateHandler(this.name_textBox, new TextCompositionEventHandler(this.name_textBox_PreviewTextInput));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0001038A File Offset: 0x0000E58A
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000103A0 File Offset: 0x0000E5A0
		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			string text = this.name_textBox.Text.Trim(CommonFunction.all_whitespaces);
			this.name_textBox.Text = this.name_textBox.Text.Trim(CommonFunction.all_whitespaces);
			if (this.g_type == CommonFunction.Profile_Type.pHotkey)
			{
				if (Directory.Exists(this.current_profile_path + this.name_textBox.Text))
				{
					new Attention(CommonFunction.Attention_Type.aProfile_Same_Name, "")
					{
						Owner = this
					}.ShowDialog();
					this.name_textBox.Focus();
					this.name_textBox.SelectAll();
					return;
				}
			}
			else if (CommonFunction.Profile_Type.pLighting == this.g_type)
			{
				if (Directory.Exists(this.current_profile_path + this.name_textBox.Text))
				{
					new Attention(CommonFunction.Attention_Type.aProfile_Same_Name, "")
					{
						Owner = this
					}.ShowDialog();
					this.name_textBox.Focus();
					this.name_textBox.SelectAll();
					return;
				}
			}
			else if ((CommonFunction.Profile_Type.pMacro == this.g_type || CommonFunction.Profile_Type.pRename_Macro == this.g_type) && File.Exists(Path.Combine(this.current_macro_content_path, this.name_textBox.Text + ".xml")))
			{
				new Attention(CommonFunction.Attention_Type.aMacro_Same_Name, "")
				{
					Owner = this
				}.ShowDialog();
				this.name_textBox.Focus();
				this.name_textBox.SelectAll();
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				base.DialogResult = new bool?(false);
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00010524 File Offset: 0x0000E724
		private void cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00010534 File Offset: 0x0000E734
		private void name_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.TextComposition.CompositionText.Length > 0 || e.Text.Length > 0)
			{
				foreach (char c in Path.GetInvalidFileNameChars())
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

		// Token: 0x0600015E RID: 350 RVA: 0x000105B7 File Offset: 0x0000E7B7
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000105C5 File Offset: 0x0000E7C5
		public string GetNewPorfileName()
		{
			return this.name_textBox.Text;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000105D4 File Offset: 0x0000E7D4
		private void name_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = this.name_textBox.Text;
			if (this.name_textBox.Text.Length > 0)
			{
				foreach (char c in Path.GetInvalidFileNameChars())
				{
					text = text.Replace(c.ToString(), "");
				}
			}
			if (text != this.name_textBox.Text)
			{
				this.name_textBox.Text = text;
			}
			this.OK_Button.IsEnabled = this.name_textBox.Text.Length != 0;
			this.OK_Button.IsEnabled = !string.IsNullOrEmpty(this.name_textBox.Text.Trim(CommonFunction.all_whitespaces));
			if (CommonFunction.Profile_Type.pRename_Macro == this.g_type && this.g_old_name == this.name_textBox.Text.Trim(CommonFunction.all_whitespaces))
			{
				this.OK_Button.IsEnabled = false;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000106CC File Offset: 0x0000E8CC
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

		// Token: 0x04000199 RID: 409
		private CommonFunction.Profile_Type g_type;

		// Token: 0x0400019A RID: 410
		private string current_profile_path = "";

		// Token: 0x0400019B RID: 411
		private string current_macro_content_path = "";

		// Token: 0x0400019C RID: 412
		public string current_profile_name = "";

		// Token: 0x0400019D RID: 413
		private string g_old_name = "";
	}
}
