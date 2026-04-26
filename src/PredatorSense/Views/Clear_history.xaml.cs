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
	// Token: 0x02000043 RID: 67
	public partial class Clear_history : Window
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0001FE54 File Offset: 0x0001E054
		public Clear_history()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(true);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0001FEB5 File Offset: 0x0001E0B5
		private void close_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0001FEC3 File Offset: 0x0001E0C3
		private void ok_Button_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0001FED1 File Offset: 0x0001E0D1
		private void remind_CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "MacroHistoryRemind", 1U);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0001FEE3 File Offset: 0x0001E0E3
		private void remind_CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "MacroHistoryRemind", 0U);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0001FEF8 File Offset: 0x0001E0F8
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
