using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x02000022 RID: 34
	public partial class PsLightingDynamicUI : UserControl
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001C4 RID: 452 RVA: 0x00013B10 File Offset: 0x00011D10
		// (remove) Token: 0x060001C5 RID: 453 RVA: 0x00013B48 File Offset: 0x00011D48
		public event PsLightingDynamicUI.PsDynamicRadioButtionEventHandler DynamicRadioButtionSelectionChangedEvent;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001C6 RID: 454 RVA: 0x00013B80 File Offset: 0x00011D80
		// (remove) Token: 0x060001C7 RID: 455 RVA: 0x00013BB8 File Offset: 0x00011DB8
		public event PsLightingDynamicUI.PsWriteDynamicSettingSelectionChangedEventHandler WriteDynamicSettingSelectionChangedEvent;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001C8 RID: 456 RVA: 0x00013BF0 File Offset: 0x00011DF0
		// (remove) Token: 0x060001C9 RID: 457 RVA: 0x00013C28 File Offset: 0x00011E28
		public event PsLightingDynamicUI.PsWriteDynamicSettingRoutedPropertyChangedEventHandler WriteDynamicSettingRoutedPropertyChangedEvent;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060001CA RID: 458 RVA: 0x00013C60 File Offset: 0x00011E60
		// (remove) Token: 0x060001CB RID: 459 RVA: 0x00013C98 File Offset: 0x00011E98
		public event PsLightingDynamicUI.PsReadDynamicSettingSelectionChangedEventHandler ReadDynamicSettingSelectionChangedEvent;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060001CC RID: 460 RVA: 0x00013CD0 File Offset: 0x00011ED0
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x00013D08 File Offset: 0x00011F08
		public event PsLightingDynamicUI.PsSetHWDynamicSettingSelectionChangedEventHandler SetHWDynamicSettingSelectionChangedEvent;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060001CE RID: 462 RVA: 0x00013D40 File Offset: 0x00011F40
		// (remove) Token: 0x060001CF RID: 463 RVA: 0x00013D78 File Offset: 0x00011F78
		public event PsLightingDynamicUI.PsSetHWDynamicSettingRoutedPropertyChangedEventHandler SetHWDynamicSettingRoutedPropertyChangedEvent;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060001D0 RID: 464 RVA: 0x00013DB0 File Offset: 0x00011FB0
		// (remove) Token: 0x060001D1 RID: 465 RVA: 0x00013DE8 File Offset: 0x00011FE8
		public event PsLightingDynamicUI.PsHidecolorpanelEventHandler HidecolorpanelEvent;

		// Token: 0x060001D2 RID: 466 RVA: 0x00013E20 File Offset: 0x00012020
		public PsLightingDynamicUI()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			DynamicRadioButtionText dynamicRadioButtionText = new DynamicRadioButtionText();
			dynamicRadioButtionText._DynamicRadioButtionText = Startup.langRd["MUI_Breathing"].ToString();
			dynamicRadioButtionText._DynamicRadioButtionGridRowIndex = 0.0;
			dynamicRadioButtionText._DynamicRadioButtionGridColumnIndex = 0.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText);
			DynamicRadioButtionText dynamicRadioButtionText2 = new DynamicRadioButtionText();
			dynamicRadioButtionText2._DynamicRadioButtionText = Startup.langRd["MUI_Marquee"].ToString();
			dynamicRadioButtionText2._DynamicRadioButtionGridRowIndex = 1.0;
			dynamicRadioButtionText2._DynamicRadioButtionGridColumnIndex = 0.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText2);
			DynamicRadioButtionText dynamicRadioButtionText3 = new DynamicRadioButtionText();
			dynamicRadioButtionText3._DynamicRadioButtionText = Startup.langRd["MUI_Circular_Marquee"].ToString();
			dynamicRadioButtionText3._DynamicRadioButtionGridRowIndex = 2.0;
			dynamicRadioButtionText3._DynamicRadioButtionGridColumnIndex = 0.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText3);
			DynamicRadioButtionText dynamicRadioButtionText4 = new DynamicRadioButtionText();
			dynamicRadioButtionText4._DynamicRadioButtionText = Startup.langRd["MUI_Colorful_Marquee"].ToString();
			dynamicRadioButtionText4._DynamicRadioButtionGridRowIndex = 3.0;
			dynamicRadioButtionText4._DynamicRadioButtionGridColumnIndex = 0.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText4);
			DynamicRadioButtionText dynamicRadioButtionText5 = new DynamicRadioButtionText();
			dynamicRadioButtionText5._DynamicRadioButtionText = Startup.langRd["MUI_Press_To_Light"].ToString();
			dynamicRadioButtionText5._DynamicRadioButtionGridRowIndex = 0.0;
			dynamicRadioButtionText5._DynamicRadioButtionGridColumnIndex = 1.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText5);
			DynamicRadioButtionText dynamicRadioButtionText6 = new DynamicRadioButtionText();
			dynamicRadioButtionText6._DynamicRadioButtionText = Startup.langRd["MUI_Hedge"].ToString();
			dynamicRadioButtionText6._DynamicRadioButtionGridRowIndex = 1.0;
			dynamicRadioButtionText6._DynamicRadioButtionGridColumnIndex = 1.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText6);
			DynamicRadioButtionText dynamicRadioButtionText7 = new DynamicRadioButtionText();
			dynamicRadioButtionText7._DynamicRadioButtionText = Startup.langRd["MUI_Rotate"].ToString();
			dynamicRadioButtionText7._DynamicRadioButtionGridRowIndex = 2.0;
			dynamicRadioButtionText7._DynamicRadioButtionGridColumnIndex = 1.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText7);
			DynamicRadioButtionText dynamicRadioButtionText8 = new DynamicRadioButtionText();
			dynamicRadioButtionText8._DynamicRadioButtionText = Startup.langRd["MUI_Wave"].ToString();
			dynamicRadioButtionText8._DynamicRadioButtionGridRowIndex = 3.0;
			dynamicRadioButtionText8._DynamicRadioButtionGridColumnIndex = 1.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText8);
			DynamicRadioButtionText dynamicRadioButtionText9 = new DynamicRadioButtionText();
			dynamicRadioButtionText9._DynamicRadioButtionText = Startup.langRd["MUI_Neon"].ToString();
			dynamicRadioButtionText9._DynamicRadioButtionGridRowIndex = 0.0;
			dynamicRadioButtionText9._DynamicRadioButtionGridColumnIndex = 2.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText9);
			DynamicRadioButtionText dynamicRadioButtionText10 = new DynamicRadioButtionText();
			dynamicRadioButtionText10._DynamicRadioButtionText = Startup.langRd["MUI_Ripple"].ToString();
			dynamicRadioButtionText10._DynamicRadioButtionGridRowIndex = 1.0;
			dynamicRadioButtionText10._DynamicRadioButtionGridColumnIndex = 2.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText10);
			DynamicRadioButtionText dynamicRadioButtionText11 = new DynamicRadioButtionText();
			dynamicRadioButtionText11._DynamicRadioButtionText = Startup.langRd["MUI_Rain_Drop"].ToString();
			dynamicRadioButtionText11._DynamicRadioButtionGridRowIndex = 2.0;
			dynamicRadioButtionText11._DynamicRadioButtionGridColumnIndex = 2.0;
			this.DynamicRadioButtionListView.Items.Add(dynamicRadioButtionText11);
			this.DynamicRadioButtionListView.SelectionChanged += this.RadioButtion_SelectionChangedEvent;
			this.Speed_ScrollBar.ValueChanged += this.DynamicSpeedScrollBar_ValueChanged;
			Direction direction = new Direction();
			direction._DynamicDirection = "M 16.4998,16.4998L 23.4996,24.4998L 30.4996,16.4998L 25.5005,18.4043C 25.5516,15.1346 28.218,12.4998 31.4999,12.4998C 34.8136,12.4998 37.4997,15.1861 37.4997,18.4998C 37.4997,21.8137 34.8136,24.4998 31.4999,24.4998L 31.4999,24.4998L 27.4997,28.4998L 31.4998,28.4998L 31.4999,28.4998C 37.0226,28.4998 41.4998,24.0227 41.4998,18.4998C 41.4998,12.977 37.0226,8.49985 31.4998,8.49985C 26.0085,8.49985 21.5512,12.9258 21.5002,18.4048L 16.4998,16.4998 Z ";
			direction._DynamicDirectionGridColumnIndex = 0.0;
			this.Direction_Rotate.Items.Add(direction);
			Direction direction2 = new Direction();
			direction2._DynamicDirection = "M 43.5,16.4999L 36.5002,24.4999L 29.5002,16.4999L 34.4993,18.4043C 34.4482,15.1347 31.7817,12.4999 28.4999,12.4999C 25.1862,12.4999 22.5001,15.1862 22.5001,18.4999C 22.5001,21.8138 25.1862,24.4999 28.4999,24.4999L 28.4999,24.4999L 32.5001,28.4999L 28.5,28.4999L 28.4999,28.4999C 22.9772,28.4998 18.5,24.0227 18.5,18.4999C 18.5,12.9771 22.9772,8.49991 28.5,8.49991C 33.9912,8.49991 38.4486,12.9258 38.4996,18.4049L 43.5,16.4999 Z ";
			direction2._DynamicDirectionGridColumnIndex = 1.0;
			this.Direction_Rotate.Items.Add(direction2);
			this.Direction_Rotate.Visibility = Visibility.Hidden;
			Direction direction3 = new Direction();
			direction3._DynamicDirection = "M 33.4998,16.4999L 33.5,20.5L 25.4999,20.4999L 28.5,26.4999L 16.5,18.4999L 28.5,10.4999L 25.4999,16.4999L 33.4998,16.4999 Z M 35.4999,16.5L 39.4999,16.5L 39.4999,20.5L 35.4999,20.5L 35.4999,16.5 Z M 41.4999,16.5L 43.4999,16.5L 43.4999,20.5L 41.4999,20.5L 41.4999,16.5 Z ";
			direction3._DynamicDirectionGridColumnIndex = 0.0;
			this.Direction_4_way.Items.Add(direction3);
			Direction direction4 = new Direction();
			direction4._DynamicDirection = "M 27.5,20.5L 27.4998,16.4998L 35.5,16.4999L 32.4998,10.5L 44.4998,18.5L 32.4998,26.5L 35.5,20.5L 27.5,20.5 Z M 25.4999,20.4998L 21.4999,20.4998L 21.4999,16.4998L 25.4999,16.4998L 25.4999,20.4998 Z M 19.4998,20.4998L 17.4998,20.4998L 17.4998,16.4998L 19.4998,16.4998L 19.4998,20.4998 Z ";
			direction4._DynamicDirectionGridColumnIndex = 1.0;
			this.Direction_4_way.Items.Add(direction4);
			Direction direction5 = new Direction();
			direction5._DynamicDirection = "M 32.5,22.4999L 28.4999,22.5001L 28.5,14.4999L 22.5,17.5001L 30.5,5.50006L 38.5,17.5001L 32.5,14.4999L 32.5,22.4999 Z M 32.4998,24.5L 32.4998,28.5L 28.4998,28.5L 28.4998,24.5L 32.4998,24.5 Z M 32.4999,30.5001L 32.4999,32.5001L 28.4999,32.5001L 28.4999,30.5001L 32.4999,30.5001 Z ";
			direction5._DynamicDirectionGridColumnIndex = 2.0;
			this.Direction_4_way.Items.Add(direction5);
			Direction direction6 = new Direction();
			direction6._DynamicDirection = "M 28.5051,15.4971L 32.5051,15.4969L 32.5051,23.4971L 38.5051,20.4969L 30.5051,32.4969L 22.5051,20.4969L 28.5051,23.4971L 28.5051,15.4971 Z M 28.5052,13.497L 28.5052,9.49701L 32.5052,9.49701L 32.5052,13.497L 28.5052,13.497 Z M 28.5051,7.49701L 28.5051,5.49701L 32.5051,5.49701L 32.5051,7.49701L 28.5051,7.49701 Z ";
			direction6._DynamicDirectionGridColumnIndex = 3.0;
			this.Direction_4_way.Items.Add(direction6);
			this.Direction_4_way.Visibility = Visibility.Hidden;
			this.Direction_Rotate.SelectionChanged += this.Direction_Rotate_SelectionChangedEvent;
			this.Direction_4_way.SelectionChanged += this.Direction_4_way_SelectionChangedEvent;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000143B0 File Offset: 0x000125B0
		public int GetCurrentSelectedIndex()
		{
			return this.CurrentSelectedIndex;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000143B8 File Offset: 0x000125B8
		public int[] GetCurrentHWEffect()
		{
			if (this.CurrentSelectedIndex.Equals(0))
			{
				this.HWEffect = 2;
			}
			else if (this.CurrentSelectedIndex.Equals(1))
			{
				this.HWEffect = 5;
			}
			else if (this.CurrentSelectedIndex.Equals(2))
			{
				this.HWEffect = 11;
			}
			else if (this.CurrentSelectedIndex.Equals(3))
			{
				this.HWEffect = 9;
			}
			else if (this.CurrentSelectedIndex.Equals(4))
			{
				this.HWEffect = 4;
			}
			else if (this.CurrentSelectedIndex.Equals(5))
			{
				this.HWEffect = 12;
			}
			else if (this.CurrentSelectedIndex.Equals(6))
			{
				this.HWEffect = 13;
			}
			else if (this.CurrentSelectedIndex.Equals(7))
			{
				this.HWEffect = 3;
			}
			else if (this.CurrentSelectedIndex.Equals(8))
			{
				this.HWEffect = 8;
			}
			else if (this.CurrentSelectedIndex.Equals(9))
			{
				this.HWEffect = 6;
			}
			else if (this.CurrentSelectedIndex.Equals(10))
			{
				this.HWEffect = 10;
			}
			int[] array = new int[3];
			array[0] = this.HWEffect;
			array[1] = (int)this.Speed_ScrollBar.Value;
			if (this.CurrentSelectedIndex.Equals(6))
			{
				array[2] = ((this.Direction_Rotate.SelectedIndex == 0) ? 1 : 2);
			}
			else if (this.CurrentSelectedIndex.Equals(7))
			{
				switch (this.Direction_4_way.SelectedIndex)
				{
				case 0:
					array[2] = 2;
					break;
				case 1:
					array[2] = 1;
					break;
				case 2:
					array[2] = 4;
					break;
				case 3:
					array[2] = 3;
					break;
				}
			}
			else
			{
				array[2] = 0;
			}
			return array;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00014568 File Offset: 0x00012768
		public void SetDynamicRadioButtionMode(int InitialDynamicRadioButtion, int speed, int Direction)
		{
			this.InitialRadioButtion = true;
			this.CurrentSelectedIndex = InitialDynamicRadioButtion;
			this.direction = Direction;
			this.Speed_ScrollBar.Value = (double)speed;
			if (Direction > 4)
			{
				this.Direction_Rotate.SelectedIndex = Direction - 5;
			}
			else
			{
				this.Direction_4_way.SelectedIndex = Direction - 1;
			}
			this.DynamicRadioButtionListView.SelectedIndex = this.CurrentSelectedIndex;
			if (this.CurrentSelectedIndex.Equals(0))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(1))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(2))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(3))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(true));
			}
			else if (this.CurrentSelectedIndex.Equals(4))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(5))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(6))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(7))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(true));
			}
			else if (this.CurrentSelectedIndex.Equals(8))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(true));
			}
			else if (this.CurrentSelectedIndex.Equals(9))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			else if (this.CurrentSelectedIndex.Equals(10))
			{
				this.HidecolorpanelEvent(new UserControlWithHidecolorpanel(false));
			}
			this.InitialRadioButtion = false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00014754 File Offset: 0x00012954
		public void SetDynamicRadioButtionMode(int speed, int Direction)
		{
			this.InitialRadioButtion = true;
			this.direction = Direction;
			this.Speed_ScrollBar.Value = (double)speed;
			if (Direction > 4)
			{
				this.Direction_Rotate.SelectedIndex = Direction - 5;
			}
			else
			{
				this.Direction_4_way.SelectedIndex = Direction - 1;
			}
			this.InitialRadioButtion = false;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000147A8 File Offset: 0x000129A8
		private void SetHWDynamicSetting(int index, int HWindex, SelectionChangedEventArgs e)
		{
			int[] array = new int[3];
			array[0] = HWindex;
			array[1] = (int)this.Speed_ScrollBar.Value;
			if (index.Equals(6))
			{
				array[2] = ((this.Direction_Rotate.SelectedIndex == 0) ? 1 : 2);
			}
			else if (index.Equals(7))
			{
				switch (this.Direction_4_way.SelectedIndex)
				{
				case 0:
					array[2] = 2;
					break;
				case 1:
					array[2] = 1;
					break;
				case 2:
					array[2] = 4;
					break;
				case 3:
					array[2] = 3;
					break;
				}
			}
			else
			{
				array[2] = 0;
			}
			this.SetHWDynamicSettingSelectionChangedEvent(this, new SelectionChangedEventArgsWithSetHWDynamicSetting(e.RoutedEvent, e.RemovedItems, e.AddedItems, array));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00014860 File Offset: 0x00012A60
		private void SetHWDynamicSetting(int index, int HWindex, RoutedPropertyChangedEventArgs<double> e)
		{
			int[] array = new int[3];
			array[0] = HWindex;
			array[1] = (int)this.Speed_ScrollBar.Value;
			if (index.Equals(6))
			{
				array[2] = ((this.Direction_Rotate.SelectedIndex == 0) ? 1 : 2);
			}
			else if (index.Equals(7))
			{
				switch (this.Direction_4_way.SelectedIndex)
				{
				case 0:
					array[2] = 2;
					break;
				case 1:
					array[2] = 1;
					break;
				case 2:
					array[2] = 4;
					break;
				case 3:
					array[2] = 3;
					break;
				}
			}
			else
			{
				array[2] = 0;
			}
			this.SetHWDynamicSettingRoutedPropertyChangedEvent(this, new RoutedPropertyChangedEventArgsWithSetHWDynamicSetting(e.OldValue, e.NewValue, e.RoutedEvent, array));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00014918 File Offset: 0x00012B18
		private void WriteDynamicSetting(int index, SelectionChangedEventArgs e)
		{
			if (this.InitialRadioButtion)
			{
				return;
			}
			int[] array = new int[3];
			array[0] = index;
			array[1] = (int)this.Speed_ScrollBar.Value;
			if (index.Equals(6))
			{
				array[2] = this.Direction_Rotate.SelectedIndex + 5;
			}
			else if (index.Equals(7))
			{
				array[2] = this.Direction_4_way.SelectedIndex + 1;
			}
			else
			{
				array[2] = 0;
			}
			this.WriteDynamicSettingSelectionChangedEvent(this, new SelectionChangedEventArgsWithWriteDynamicSetting(e.RoutedEvent, e.RemovedItems, e.AddedItems, array));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000149A8 File Offset: 0x00012BA8
		private void WriteDynamicSetting(int index, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.InitialRadioButtion)
			{
				return;
			}
			int[] array = new int[3];
			array[0] = index;
			array[1] = (int)this.Speed_ScrollBar.Value;
			if (index.Equals(6))
			{
				array[2] = this.Direction_Rotate.SelectedIndex + 5;
			}
			else if (index.Equals(7))
			{
				array[2] = this.Direction_4_way.SelectedIndex + 1;
			}
			else
			{
				array[2] = 0;
			}
			this.WriteDynamicSettingRoutedPropertyChangedEvent(this, new RoutedPropertyChangedEventArgsWithWriteDynamicSetting(e.OldValue, e.NewValue, e.RoutedEvent, array));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00014A38 File Offset: 0x00012C38
		private void ReadDynamicSetting(int index, SelectionChangedEventArgs e)
		{
			if (this.InitialRadioButtion)
			{
				return;
			}
			this.ReadDynamicSettingSelectionChangedEvent(this, new SelectionChangedEventArgsWithReadDynamicSetting(e.RoutedEvent, e.RemovedItems, e.AddedItems, index));
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00014A67 File Offset: 0x00012C67
		private void Direction_4_way_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
		{
			this.SetHWDynamicSetting(this.CurrentSelectedIndex, 3, e);
			this.WriteDynamicSetting(this.CurrentSelectedIndex, e);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00014A84 File Offset: 0x00012C84
		private void Direction_Rotate_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
		{
			this.SetHWDynamicSetting(this.CurrentSelectedIndex, 13, e);
			this.WriteDynamicSetting(this.CurrentSelectedIndex, e);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00014AA4 File Offset: 0x00012CA4
		private void ScrollBar_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			this.is_drag = false;
			Slider slider = sender as Slider;
			double num = slider.Value;
			this.DynamicSpeedScrollBar_ValueChanged(sender, new RoutedPropertyChangedEventArgs<double>(num, num));
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00014AD5 File Offset: 0x00012CD5
		private void ScrollBar_DragStarted(object sender, DragStartedEventArgs e)
		{
			this.is_drag = true;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00014AE0 File Offset: 0x00012CE0
		private void DynamicSpeedScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.is_drag)
			{
				return;
			}
			if (this.CurrentSelectedIndex.Equals(0))
			{
				this.HWEffect = 2;
			}
			else if (this.CurrentSelectedIndex.Equals(1))
			{
				this.HWEffect = 5;
			}
			else if (this.CurrentSelectedIndex.Equals(2))
			{
				this.HWEffect = 11;
			}
			else if (this.CurrentSelectedIndex.Equals(3))
			{
				this.HWEffect = 9;
			}
			else if (this.CurrentSelectedIndex.Equals(4))
			{
				this.HWEffect = 4;
			}
			else if (this.CurrentSelectedIndex.Equals(5))
			{
				this.HWEffect = 12;
			}
			else if (this.CurrentSelectedIndex.Equals(6))
			{
				this.HWEffect = 13;
			}
			else if (this.CurrentSelectedIndex.Equals(7))
			{
				this.HWEffect = 3;
			}
			else if (this.CurrentSelectedIndex.Equals(8))
			{
				this.HWEffect = 8;
			}
			else if (this.CurrentSelectedIndex.Equals(9))
			{
				this.HWEffect = 6;
			}
			else if (this.CurrentSelectedIndex.Equals(10))
			{
				this.HWEffect = 10;
			}
			this.SetHWDynamicSetting(this.CurrentSelectedIndex, this.HWEffect, e);
			this.WriteDynamicSetting(this.CurrentSelectedIndex, e);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00014C28 File Offset: 0x00012E28
		private void RadioButtion_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
		{
			this.CurrentSelectedIndex = this.DynamicRadioButtionListView.SelectedIndex;
			this.ReadDynamicSetting(this.CurrentSelectedIndex, e);
			if (this.CurrentSelectedIndex.Equals(0))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 2;
			}
			else if (this.CurrentSelectedIndex.Equals(1))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 5;
			}
			else if (this.CurrentSelectedIndex.Equals(2))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 11;
			}
			else if (this.CurrentSelectedIndex.Equals(3))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, true));
				this.HWEffect = 9;
			}
			else if (this.CurrentSelectedIndex.Equals(4))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 4;
			}
			else if (this.CurrentSelectedIndex.Equals(5))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 12;
			}
			else if (this.CurrentSelectedIndex.Equals(6))
			{
				this.DirectionName.Visibility = Visibility.Visible;
				this.Direction_Rotate.Visibility = Visibility.Visible;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 13;
			}
			else if (this.CurrentSelectedIndex.Equals(7))
			{
				this.DirectionName.Visibility = Visibility.Visible;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Visible;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, true));
				this.HWEffect = 3;
			}
			else if (this.CurrentSelectedIndex.Equals(8))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, true));
				this.HWEffect = 8;
			}
			else if (this.CurrentSelectedIndex.Equals(9))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 6;
			}
			else if (this.CurrentSelectedIndex.Equals(10))
			{
				this.DirectionName.Visibility = Visibility.Hidden;
				this.Direction_Rotate.Visibility = Visibility.Hidden;
				this.Direction_4_way.Visibility = Visibility.Hidden;
				this.DynamicRadioButtionSelectionChangedEvent(this, new SelectionChangedEventArgsWithDynamicRadioButtion(e.RoutedEvent, e.RemovedItems, e.AddedItems, false));
				this.HWEffect = 10;
			}
			this.SetHWDynamicSetting(this.CurrentSelectedIndex, this.HWEffect, e);
		}

		// Token: 0x04000207 RID: 519
		private bool InitialRadioButtion;

		// Token: 0x04000208 RID: 520
		private int CurrentSelectedIndex;

		// Token: 0x04000209 RID: 521
		private int direction;

		// Token: 0x0400020A RID: 522
		private int HWEffect;

		// Token: 0x0400020B RID: 523
		private bool is_drag;

		// Token: 0x02000023 RID: 35
		public enum DynamicRadioButtionEnum
		{
			// Token: 0x04000213 RID: 531
			Breathing,
			// Token: 0x04000214 RID: 532
			Marquee,
			// Token: 0x04000215 RID: 533
			Circular_Marquee,
			// Token: 0x04000216 RID: 534
			Colorful_Marquee,
			// Token: 0x04000217 RID: 535
			Press_To_Light,
			// Token: 0x04000218 RID: 536
			Hedge,
			// Token: 0x04000219 RID: 537
			Rotate,
			// Token: 0x0400021A RID: 538
			Wave,
			// Token: 0x0400021B RID: 539
			Neon,
			// Token: 0x0400021C RID: 540
			Ripple,
			// Token: 0x0400021D RID: 541
			Rain_Drop
		}

		// Token: 0x02000024 RID: 36
		public enum HWEffectEnum
		{
			// Token: 0x0400021F RID: 543
			Breathing = 2,
			// Token: 0x04000220 RID: 544
			Wave,
			// Token: 0x04000221 RID: 545
			Marquee = 5,
			// Token: 0x04000222 RID: 546
			Ripple,
			// Token: 0x04000223 RID: 547
			Press_To_Light = 4,
			// Token: 0x04000224 RID: 548
			Neon = 8,
			// Token: 0x04000225 RID: 549
			Colorful_Marquee,
			// Token: 0x04000226 RID: 550
			Rain_Drop,
			// Token: 0x04000227 RID: 551
			Circular_Marquee,
			// Token: 0x04000228 RID: 552
			Hedge,
			// Token: 0x04000229 RID: 553
			Rotate
		}

		// Token: 0x02000025 RID: 37
		public enum DirectionValue
		{
			// Token: 0x0400022B RID: 555
			NA,
			// Token: 0x0400022C RID: 556
			Right,
			// Token: 0x0400022D RID: 557
			Left,
			// Token: 0x0400022E RID: 558
			Down,
			// Token: 0x0400022F RID: 559
			Up,
			// Token: 0x04000230 RID: 560
			Rotate_left,
			// Token: 0x04000231 RID: 561
			Rotate_right
		}

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x060001E5 RID: 485
		public delegate void PsDynamicRadioButtionEventHandler(object sender, SelectionChangedEventArgsWithDynamicRadioButtion e);

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x060001E9 RID: 489
		public delegate void PsWriteDynamicSettingSelectionChangedEventHandler(object sender, SelectionChangedEventArgsWithWriteDynamicSetting e);

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x060001ED RID: 493
		public delegate void PsWriteDynamicSettingRoutedPropertyChangedEventHandler(object sender, RoutedPropertyChangedEventArgsWithWriteDynamicSetting e);

		// Token: 0x02000029 RID: 41
		// (Invoke) Token: 0x060001F1 RID: 497
		public delegate void PsReadDynamicSettingSelectionChangedEventHandler(object sender, SelectionChangedEventArgsWithReadDynamicSetting e);

		// Token: 0x0200002A RID: 42
		// (Invoke) Token: 0x060001F5 RID: 501
		public delegate void PsSetHWDynamicSettingSelectionChangedEventHandler(object sender, SelectionChangedEventArgsWithSetHWDynamicSetting e);

		// Token: 0x0200002B RID: 43
		// (Invoke) Token: 0x060001F9 RID: 505
		public delegate void PsSetHWDynamicSettingRoutedPropertyChangedEventHandler(object sender, RoutedPropertyChangedEventArgsWithSetHWDynamicSetting e);

		// Token: 0x0200002C RID: 44
		// (Invoke) Token: 0x060001FD RID: 509
		public delegate void PsHidecolorpanelEventHandler(UserControlWithHidecolorpanel e);
	}
}
