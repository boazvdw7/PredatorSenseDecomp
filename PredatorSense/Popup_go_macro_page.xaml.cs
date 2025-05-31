using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000016 RID: 22
	public partial class Popup_go_macro_page : Window
	{
		// Token: 0x0600016F RID: 367 RVA: 0x0001108C File Offset: 0x0000F28C
		public Popup_go_macro_page(string key_number)
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
			this.content_TextBlock.Text = this.content_TextBlock.Text.Replace("%Num%", key_number);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000110F9 File Offset: 0x0000F2F9
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0001110E File Offset: 0x0000F30E
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0001111C File Offset: 0x0000F31C
		private void ok_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0001112A File Offset: 0x0000F32A
		private void remind_CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "GoMacroEditRemind", 1U);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0001113C File Offset: 0x0000F33C
		private void remind_CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "GoMacroEditRemind", 0U);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00011150 File Offset: 0x0000F350
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
