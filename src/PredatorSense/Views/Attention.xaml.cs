using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x02000002 RID: 2
	public partial class Attention : Window
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Attention(CommonFunction.Attention_Type attention_type, string name)
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			if (attention_type == CommonFunction.Attention_Type.aDelete_Profile)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Delete_Profile_Title"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Delete_Profile_Info"].ToString().Replace("%Name%", name);
				return;
			}
			if (CommonFunction.Attention_Type.aDelete_Macro == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Delete_Macro_Title"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Delete_Macro_Info"].ToString().Replace("%Name%", name);
				return;
			}
			if (CommonFunction.Attention_Type.aEnough_Storage == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Export_Fail_Title"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Export_Fail_Info"].ToString();
				this.show_OK_Button();
				return;
			}
			if (CommonFunction.Attention_Type.aImport_Failed == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Import_Fail_Title"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Import_Fail_Info"].ToString();
				this.show_OK_Button();
				return;
			}
			if (CommonFunction.Attention_Type.aMacro_Record_Max == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Attention"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Over_Max"].ToString();
				this.show_OK_Button();
				return;
			}
			if (CommonFunction.Attention_Type.aProfile_Same_Name == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Attention"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Same_Name_List_Attention"].ToString();
				TextBlock textBlock = this.content_TextBlock;
				textBlock.Text = textBlock.Text + Environment.NewLine + Startup.langRd["MUI_Enter_Another_Name"].ToString();
				this.show_OK_Button();
				return;
			}
			if (CommonFunction.Attention_Type.aMacro_Same_Name == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Attention"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Macro_Same_Info"].ToString();
				TextBlock textBlock2 = this.content_TextBlock;
				textBlock2.Text = textBlock2.Text + Environment.NewLine + Startup.langRd["MUI_Enter_Another_Name"].ToString();
				this.show_OK_Button();
				return;
			}
			if (CommonFunction.Attention_Type.aGroup_Same_Name == attention_type)
			{
				this.title_TextBlock.Text = Startup.langRd["MUI_Attention"].ToString();
				this.content_TextBlock.Text = Startup.langRd["MUI_Group_Same_Info"].ToString();
				TextBlock textBlock3 = this.content_TextBlock;
				textBlock3.Text = textBlock3.Text + Environment.NewLine + Startup.langRd["MUI_Enter_Another_Name"].ToString();
				this.show_OK_Button();
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002379 File Offset: 0x00000579
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000238E File Offset: 0x0000058E
		private void yes_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000239C File Offset: 0x0000059C
		private void no_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023AA File Offset: 0x000005AA
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023B8 File Offset: 0x000005B8
		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023C6 File Offset: 0x000005C6
		private void show_OK_Button()
		{
			this.yes_Button.Visibility = Visibility.Hidden;
			this.no_Button.Visibility = Visibility.Hidden;
			this.OK_Button.Visibility = Visibility.Visible;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023EC File Offset: 0x000005EC
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
	}
}
