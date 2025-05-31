using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PredatorSense
{
	// Token: 0x02000038 RID: 56
	public partial class ShowPresetKeys : Window, IComponentConnector
	{

        // Add this method to the `ShowPresetKeys` class
        public void UncheckAllPresetKeysMenu()
        {
            this.PresetKeysMenu_WASD.IsChecked = new bool?(false);
            this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
            this.PresetKeysMenu_Numberrows.IsChecked = new bool?(false);
            this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
        }
        // Token: 0x06000252 RID: 594 RVA: 0x000185E8 File Offset: 0x000167E8
        //public ShowPresetKeys()
        //{
        //	this.InitializeComponent();
        //	if (Startup._TTFont)
        //	{
        //		base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
        //	}
        //	base.Resources = Startup.styled;
        //	this.PresetKeysMenu_All.Click += this.PresetKeysMenu_All_Checked;
        //	this.PresetKeysMenu_WASD.Click += this.PresetKeysMenu_WASD_Checked;
        //	this.PresetKeysMenu_Arrowkeys.Click += this.PresetKeysMenu_Arrowkeys_Checked;
        //	this.PresetKeysMenu_Numberrows.Click += this.PresetKeysMenu_Numberrows_Checked;
        //	this.PresetKeysMenu_Fnkeys.Click += this.PresetKeysMenu_Fnkeys_Checked;
        //}

        // Token: 0x06000253 RID: 595 RVA: 0x000187BE File Offset: 0x000169BE
        public void PresetKeys(double mainX, double mainY, ListBox ListBox)
		{
			this.keyListBox = ListBox;
			this.UpdatePresetKeysMenu();
			base.Left = mainX + 673.0;
			base.Top = mainY + 60.0;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000187F0 File Offset: 0x000169F0
		//public void UncheckAllPresetKeysMenu()
		//{
		//	this.PresetKeysMenu_WASD.IsChecked = new bool?(false);
		//	this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
		//	this.PresetKeysMenu_Numberrows.IsChecked = new bool?(false);
		//	this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
		//}

		// Token: 0x06000255 RID: 597 RVA: 0x00018844 File Offset: 0x00016A44
		private void UpdatePresetKeysMenu()
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
			this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
			this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
			this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
			if (this.item35.IsSelected && this.item51.IsSelected && this.item52.IsSelected && this.item53.IsSelected)
			{
				this.PresetKeysMenu_WASD.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_WASD.IsChecked = new bool?(false);
			}
			this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
			this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
			this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
			this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
			this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
			this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
			this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
			this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
			this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
			this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
			this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
			if (this.item75.IsSelected && this.item84.IsSelected && this.item85.IsSelected && this.item86.IsSelected)
			{
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
			}
			if (this.PresetKeysMenu_Arrowkeys.IsChecked == true && this.item3.IsSelected && this.item4.IsSelected && this.item5.IsSelected && this.item6.IsSelected && this.item7.IsSelected && this.item8.IsSelected && this.item77.IsSelected)
			{
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
			}
			this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
			this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
			this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
			this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
			this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
			this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
			this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
			this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
			this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
			this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
			if (this.item17.IsSelected && this.item18.IsSelected && this.item19.IsSelected && this.item20.IsSelected && this.item21.IsSelected && this.item22.IsSelected && this.item23.IsSelected && this.item24.IsSelected && this.item25.IsSelected && this.item26.IsSelected)
			{
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(false);
			}
			if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
			{
				this.PresetKeysMenu_All.IsChecked = new bool?(true);
				return;
			}
			this.PresetKeysMenu_All.IsChecked = new bool?(false);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00018C70 File Offset: 0x00016E70
		private void PresetKeysMenu_All_Checked(object sender, RoutedEventArgs e)
		{
			if (this.PresetKeysMenu_All.IsChecked == true)
			{
				this.keyListBox.SelectAll();
				this.PresetKeysMenu_WASD.IsChecked = new bool?(true);
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(true);
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
				return;
			}
			this.keyListBox.SelectedItems.Clear();
			this.UncheckAllPresetKeysMenu();
		}

        // Token: 0x06000257 RID: 599 RVA: 0x00018D04 File Offset: 0x00016F04
        private void PresetKeysMenu_WASD_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_WASD.IsChecked == true)
			{
				this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
				this.item35.IsSelected = true;
				this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
				this.item51.IsSelected = true;
				this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
				this.item52.IsSelected = true;
				this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
				this.item53.IsSelected = true;
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
					return;
				}
			}
			else
			{
				this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
				this.item35.IsSelected = false;
				this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
				this.item51.IsSelected = false;
				this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
				this.item52.IsSelected = false;
				this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
				this.item53.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00018E84 File Offset: 0x00017084
		private void PresetKeysMenu_Arrowkeys_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Arrowkeys.IsChecked == true)
			{
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = true;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = true;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = true;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = true;
				if (this.item3.IsSelected && this.item4.IsSelected && this.item5.IsSelected && this.item6.IsSelected && this.item7.IsSelected && this.item8.IsSelected && this.item77.IsSelected)
				{
					this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
				}
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
					return;
				}
			}
			else
			{
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = false;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = false;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = false;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00019080 File Offset: 0x00017280
		private void PresetKeysMenu_Numberrows_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Numberrows.IsChecked == true)
			{
				this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
				this.item17.IsSelected = true;
				this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
				this.item18.IsSelected = true;
				this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
				this.item19.IsSelected = true;
				this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
				this.item20.IsSelected = true;
				this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
				this.item21.IsSelected = true;
				this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
				this.item22.IsSelected = true;
				this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
				this.item23.IsSelected = true;
				this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
				this.item24.IsSelected = true;
				this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
				this.item25.IsSelected = true;
				this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
				this.item26.IsSelected = true;
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
					return;
				}
			}
			else
			{
				this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
				this.item17.IsSelected = false;
				this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
				this.item18.IsSelected = false;
				this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
				this.item19.IsSelected = false;
				this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
				this.item20.IsSelected = false;
				this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
				this.item21.IsSelected = false;
				this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
				this.item22.IsSelected = false;
				this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
				this.item23.IsSelected = false;
				this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
				this.item24.IsSelected = false;
				this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
				this.item25.IsSelected = false;
				this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
				this.item26.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00019374 File Offset: 0x00017574
		private void PresetKeysMenu_Fnkeys_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Fnkeys.IsChecked == true)
			{
				this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
				this.item3.IsSelected = true;
				this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
				this.item4.IsSelected = true;
				this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
				this.item5.IsSelected = true;
				this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
				this.item6.IsSelected = true;
				this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
				this.item7.IsSelected = true;
				this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
				this.item8.IsSelected = true;
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = true;
				this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
				this.item77.IsSelected = true;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = true;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = true;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = true;
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
					return;
				}
			}
			else
			{
				this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
				this.item3.IsSelected = false;
				this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
				this.item4.IsSelected = false;
				this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
				this.item5.IsSelected = false;
				this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
				this.item6.IsSelected = false;
				this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
				this.item7.IsSelected = false;
				this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
				this.item8.IsSelected = false;
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = false;
				this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
				this.item77.IsSelected = false;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = false;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = false;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = false;
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000196BC File Offset: 0x000178BC
		//[DebuggerNonUserCode]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//public void InitializeComponent()
		//{
		//	if (this._contentLoaded)
		//	{
		//		return;
		//	}
		//	this._contentLoaded = true;
		//	Uri uri = new Uri("/PredatorSense;component/showpresetkeys%20.xaml", UriKind.Relative);
		//	Application.LoadComponent(this, uri);
		//}

		//// Token: 0x0600025C RID: 604 RVA: 0x000196EC File Offset: 0x000178EC
		//[EditorBrowsable(EditorBrowsableState.Never)]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//[DebuggerNonUserCode]
		//void IComponentConnector.Connect(int connectionId, object target)
		//{
		//	switch (connectionId)
		//	{
		//		case 1:
		//			this.PresetKeysMenu_All = (CheckBox)target;
		//			return;
		//		case 2:
		//			this.PresetKeysMenu_WASD = (CheckBox)target;
		//			return;
		//		case 3:
		//			this.PresetKeysMenu_Arrowkeys = (CheckBox)target;
		//			return;
		//		case 4:
		//			this.PresetKeysMenu_Numberrows = (CheckBox)target;
		//			return;
		//		case 5:
		//			this.PresetKeysMenu_Fnkeys = (CheckBox)target;
		//			return;
		//		default:
		//			this._contentLoaded = true;
		//			return;
		//	}
		//}

		// Token: 0x04000288 RID: 648
		private ListBox keyListBox = new ListBox();

		// Token: 0x04000289 RID: 649
		private ListBoxItem item35 = new ListBoxItem();

		// Token: 0x0400028A RID: 650
		private ListBoxItem item51 = new ListBoxItem();

		// Token: 0x0400028B RID: 651
		private ListBoxItem item52 = new ListBoxItem();

		// Token: 0x0400028C RID: 652
		private ListBoxItem item53 = new ListBoxItem();

		// Token: 0x0400028D RID: 653
		private ListBoxItem item75 = new ListBoxItem();

		// Token: 0x0400028E RID: 654
		private ListBoxItem item84 = new ListBoxItem();

		// Token: 0x0400028F RID: 655
		private ListBoxItem item85 = new ListBoxItem();

		// Token: 0x04000290 RID: 656
		private ListBoxItem item86 = new ListBoxItem();

		// Token: 0x04000291 RID: 657
		private ListBoxItem item17 = new ListBoxItem();

		// Token: 0x04000292 RID: 658
		private ListBoxItem item18 = new ListBoxItem();

		// Token: 0x04000293 RID: 659
		private ListBoxItem item19 = new ListBoxItem();

		// Token: 0x04000294 RID: 660
		private ListBoxItem item20 = new ListBoxItem();

		// Token: 0x04000295 RID: 661
		private ListBoxItem item21 = new ListBoxItem();

		// Token: 0x04000296 RID: 662
		private ListBoxItem item22 = new ListBoxItem();

		// Token: 0x04000297 RID: 663
		private ListBoxItem item23 = new ListBoxItem();

		// Token: 0x04000298 RID: 664
		private ListBoxItem item24 = new ListBoxItem();

		// Token: 0x04000299 RID: 665
		private ListBoxItem item25 = new ListBoxItem();

		// Token: 0x0400029A RID: 666
		private ListBoxItem item26 = new ListBoxItem();

		// Token: 0x0400029B RID: 667
		private ListBoxItem item3 = new ListBoxItem();

		// Token: 0x0400029C RID: 668
		private ListBoxItem item4 = new ListBoxItem();

		// Token: 0x0400029D RID: 669
		private ListBoxItem item5 = new ListBoxItem();

		// Token: 0x0400029E RID: 670
		private ListBoxItem item6 = new ListBoxItem();

		// Token: 0x0400029F RID: 671
		private ListBoxItem item7 = new ListBoxItem();

		// Token: 0x040002A0 RID: 672
		private ListBoxItem item8 = new ListBoxItem();

		// Token: 0x040002A1 RID: 673
		private ListBoxItem item77 = new ListBoxItem();

		// Token: 0x040002A2 RID: 674
		//internal CheckBox PresetKeysMenu_All;

		// Token: 0x040002A3 RID: 675
		//internal CheckBox PresetKeysMenu_WASD;

		// Token: 0x040002A4 RID: 676
		//internal CheckBox PresetKeysMenu_Arrowkeys;

		// Token: 0x040002A5 RID: 677
		//internal CheckBox PresetKeysMenu_Numberrows;

		// Token: 0x040002A6 RID: 678
		//internal CheckBox PresetKeysMenu_Fnkeys;

		// Token: 0x040002A7 RID: 679
		//private bool _contentLoaded;
	}
}
