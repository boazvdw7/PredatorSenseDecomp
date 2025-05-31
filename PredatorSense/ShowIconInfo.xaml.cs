using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PredatorSense
{
	// Token: 0x02000037 RID: 55
	public partial class ShowIconInfo : Window
	{
		// Token: 0x0600024D RID: 589 RVA: 0x000182DE File Offset: 0x000164DE
		public ShowIconInfo()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00018318 File Offset: 0x00016518
		public void IconInfo(double mainX, double mainY)
		{
			base.Left = mainX + 637.0;
			base.Top = mainY + 54.0;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0001833C File Offset: 0x0001653C
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.Method1.Text = Startup.langRd["MUI_Method_Number"].ToString().Replace("%Number%", "1");
			this.Method2.Text = Startup.langRd["MUI_Method_Number"].ToString().Replace("%Number%", "2");
			this.Method1_Info2_up.Text = " " + Startup.langRd["MUI_Method1_Info2"].ToString();
			if (Startup.langRd["MUI_Method1_Info1"].ToString().IndexOf("%icon%") == 0)
			{
				this.SelectRec_R.Visibility = Visibility.Collapsed;
				this.SelectRec_L.Visibility = Visibility.Visible;
				this.Method1_Info1.Text = Startup.langRd["MUI_Method1_Info1"].ToString().Replace("%icon%", "");
			}
			else
			{
				this.SelectRec_R.Visibility = Visibility.Visible;
				this.SelectRec_L.Visibility = Visibility.Collapsed;
				this.Method1_Info1.Text = Startup.langRd["MUI_Method1_Info1"].ToString().Replace("%icon%", "");
			}
			if (this.SPText.ActualWidth - (this.SelectRec_L.ActualWidth + this.Method1_Info1.ActualWidth) < this.Method1_Info2_up.ActualWidth)
			{
				this.Method1_Info2_up.Visibility = Visibility.Collapsed;
				this.Method1_Info2_down.Visibility = Visibility.Visible;
				return;
			}
			this.Method1_Info2_up.Visibility = Visibility.Visible;
			this.Method1_Info2_down.Visibility = Visibility.Collapsed;
		}
	}
}
