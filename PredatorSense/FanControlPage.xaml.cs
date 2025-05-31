using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000006 RID: 6
	public partial class FanControlPage : UserControl
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00004C18 File Offset: 0x00002E18
		public FanControlPage()
		{
			this.InitializeComponent();
			this.CPU_textblock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_textblock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_FanRate.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU1_FanRate.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_RPM.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_RPM.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CoolBoost.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			base.Loaded += this.Window_Loaded;
			this.ShowCoolBoosterInformationicon.Click += this.ShowCoolBoosterInformationicon_Click;
			this.FanModeAuto.Click += this.FanModeAuto_Click;
			this.FanModeMax.Click += this.FanModeMax_Click;
			this.FanModeCustom.Click += this.FanModeCustom_Click;
			this.CPU_ScrollBar.Visibility = Visibility.Hidden;
			this.CPU_Auto.Visibility = Visibility.Hidden;
			this.GPU1_ScrollBar.Visibility = Visibility.Hidden;
			this.GPU1_Auto.Visibility = Visibility.Hidden;
			this.CPU_Auto.Click += this.FanModeCustomAutoCPU_Click;
			this.GPU1_Auto.Click += this.FanModeCustomAutoGPU1_Click;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004E04 File Offset: 0x00003004
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Auto"].ToString();
			list = this.FindVisualChild<TextBlock>(this.FanModeMax);
			textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Max"].ToString();
			list = this.FindVisualChild<TextBlock>(this.FanModeCustom);
			textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Custom"].ToString();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004EA0 File Offset: 0x000030A0
		public void SetCustomFanAutoMode(params bool[] CustomFanAutoMode)
		{
			this.CPU_Auto.IsChecked = new bool?(CustomFanAutoMode[0]);
			this.GPU1_Auto.IsChecked = new bool?(CustomFanAutoMode[1]);
			if (this.CPU_Auto.IsChecked == true)
			{
				this.CPU_ScrollBar.IsEnabled = false;
			}
			else
			{
				this.CPU_ScrollBar.IsEnabled = true;
			}
			if (this.GPU1_Auto.IsChecked == true)
			{
				this.GPU1_ScrollBar.IsEnabled = false;
				return;
			}
			this.GPU1_ScrollBar.IsEnabled = true;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004F48 File Offset: 0x00003148
		public void SetCustomFanSpeedMode(params int[] CustomFanSpeedMode)
		{
			this.CPU_ScrollBar.Value = (double)CustomFanSpeedMode[0];
			this.GPU1_ScrollBar.Value = (double)CustomFanSpeedMode[1];
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004F68 File Offset: 0x00003168
		public bool[] GetCustomFanAutoMode()
		{
			bool[] array = new bool[4];
			array[0] = this.CPU_Auto.IsChecked.Value;
			array[1] = this.GPU1_Auto.IsChecked.Value;
			return array;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004FAC File Offset: 0x000031AC
		public int[] GetCustomFanSpeedMode()
		{
			int[] array = new int[4];
			array[0] = (int)this.CPU_ScrollBar.Value;
			array[1] = (int)this.GPU1_ScrollBar.Value;
			return array;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004FE0 File Offset: 0x000031E0
		public void SetFanMode(int InitialFanMode)
		{
			if (InitialFanMode.Equals(0))
			{
				this.FanModeAuto.IsChecked = new bool?(true);
				this.FanModeMax.IsChecked = new bool?(false);
				this.FanModeCustom.IsChecked = new bool?(false);
				this.CPU_ScrollBar.Visibility = Visibility.Hidden;
				this.CPU_Auto.Visibility = Visibility.Hidden;
				this.GPU1_ScrollBar.Visibility = Visibility.Hidden;
				this.GPU1_Auto.Visibility = Visibility.Hidden;
				return;
			}
			if (InitialFanMode.Equals(1))
			{
				this.FanModeAuto.IsChecked = new bool?(false);
				this.FanModeMax.IsChecked = new bool?(true);
				this.FanModeCustom.IsChecked = new bool?(false);
				this.CPU_ScrollBar.Visibility = Visibility.Hidden;
				this.CPU_Auto.Visibility = Visibility.Hidden;
				this.GPU1_ScrollBar.Visibility = Visibility.Hidden;
				this.GPU1_Auto.Visibility = Visibility.Hidden;
				return;
			}
			if (InitialFanMode.Equals(2))
			{
				this.FanModeAuto.IsChecked = new bool?(false);
				this.FanModeMax.IsChecked = new bool?(false);
				this.FanModeCustom.IsChecked = new bool?(true);
				this.CPU_ScrollBar.Visibility = Visibility.Visible;
				this.CPU_Auto.Visibility = Visibility.Visible;
				this.GPU1_ScrollBar.Visibility = Visibility.Visible;
				this.GPU1_Auto.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005138 File Offset: 0x00003338
		public int GetFanMode()
		{
			if (this.FanModeAuto.IsChecked == true)
			{
				return 0;
			}
			if (this.FanModeMax.IsChecked == true)
			{
				return 1;
			}
			if (this.FanModeCustom.IsChecked == true)
			{
				return 2;
			}
			return -1;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000051AF File Offset: 0x000033AF
		public void PassShowCoolBoosterStatusicon(bool CoolBoosterStatus)
		{
			if (CoolBoosterStatus)
			{
				this.ShowCoolBoosterStatusicon.IsChecked = new bool?(true);
				return;
			}
			this.ShowCoolBoosterStatusicon.IsChecked = new bool?(false);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000051D7 File Offset: 0x000033D7
		public void HideFanControlUIInformationPopup()
		{
			if (this.ShowCoolBoosterInformationPopup.IsOpen)
			{
				this.ShowCoolBoosterInformationPopup.IsOpen = false;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000051F4 File Offset: 0x000033F4
		public void PushData_CPU(double data)
		{
			if (data.Equals(double.NaN))
			{
				throw new ArgumentNullException();
			}
			this.CPU_FanRate.Text = data.ToString();
			int num = 11 - (int)(data / 450.0);
			if (data > 4500.0)
			{
				num = 2;
			}
			if (data == 0.0)
			{
				DoubleAnimation doubleAnimation = new DoubleAnimation((double)this.lastAngle_CPU, (double)this.lastAngle_CPU, new Duration(TimeSpan.FromSeconds(0.0)));
				doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
				this.CPU_fan.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
				return;
			}
			this.lastAngle_CPU = (int)this.CPU_fan.Angle;
			DoubleAnimation doubleAnimation2 = new DoubleAnimation((double)this.lastAngle_CPU, (double)(this.lastAngle_CPU + 360), new Duration(TimeSpan.FromSeconds((double)num)));
			doubleAnimation2.RepeatBehavior = RepeatBehavior.Forever;
			this.CPU_fan.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation2);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000052F0 File Offset: 0x000034F0
		public void PushData_GPU1(double data)
		{
			if (data.Equals(double.NaN))
			{
				throw new ArgumentNullException();
			}
			this.GPU1_FanRate.Text = data.ToString();
			int num = 11 - (int)(data / 450.0);
			if (data > 4500.0)
			{
				num = 2;
			}
			if (data == 0.0)
			{
				DoubleAnimation doubleAnimation = new DoubleAnimation((double)this.lastAngle_GPU1, (double)this.lastAngle_GPU1, new Duration(TimeSpan.FromSeconds(0.0)));
				doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
				this.GPU1_fan.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
				return;
			}
			this.lastAngle_GPU1 = (int)this.GPU1_fan.Angle;
			DoubleAnimation doubleAnimation2 = new DoubleAnimation((double)this.lastAngle_GPU1, (double)(this.lastAngle_GPU1 + 360), new Duration(TimeSpan.FromSeconds((double)num)));
			doubleAnimation2.RepeatBehavior = RepeatBehavior.Forever;
			this.GPU1_fan.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation2);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000053F4 File Offset: 0x000035F4
		public void FanModeAuto_Click(object sender, RoutedEventArgs e)
		{
			if (this.FanModeAuto.IsChecked == false)
			{
				this.FanModeAuto.IsChecked = new bool?(true);
				return;
			}
			this.FanModeAuto.IsChecked = new bool?(true);
			this.FanModeMax.IsChecked = new bool?(false);
			this.FanModeCustom.IsChecked = new bool?(false);
			this.CPU_ScrollBar.Visibility = Visibility.Hidden;
			this.CPU_Auto.Visibility = Visibility.Hidden;
			this.GPU1_ScrollBar.Visibility = Visibility.Hidden;
			this.GPU1_Auto.Visibility = Visibility.Hidden;
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode", 0U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Auto);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000054DC File Offset: 0x000036DC
		public void FanModeMax_Click(object sender, RoutedEventArgs e)
		{
			if (this.FanModeMax.IsChecked == false)
			{
				this.FanModeMax.IsChecked = new bool?(true);
				return;
			}
			this.FanModeAuto.IsChecked = new bool?(false);
			this.FanModeMax.IsChecked = new bool?(true);
			this.FanModeCustom.IsChecked = new bool?(false);
			this.CPU_ScrollBar.Visibility = Visibility.Hidden;
			this.CPU_Auto.Visibility = Visibility.Hidden;
			this.GPU1_ScrollBar.Visibility = Visibility.Hidden;
			this.GPU1_Auto.Visibility = Visibility.Hidden;
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode", 1U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Max);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000055D4 File Offset: 0x000037D4
		public void FanModeCustom_Click(object sender, RoutedEventArgs e)
		{
			if (this.FanModeCustom.IsChecked == false)
			{
				this.FanModeCustom.IsChecked = new bool?(true);
				return;
			}
			this.FanModeAuto.IsChecked = new bool?(false);
			this.FanModeMax.IsChecked = new bool?(false);
			this.FanModeCustom.IsChecked = new bool?(true);
			this.CPU_ScrollBar.Visibility = Visibility.Visible;
			this.CPU_Auto.Visibility = Visibility.Visible;
			this.GPU1_ScrollBar.Visibility = Visibility.Visible;
			this.GPU1_Auto.Visibility = Visibility.Visible;
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode", 2U);
			List<bool> all_auto_state = new List<bool>();
			List<ulong> all_percentage_state = new List<ulong>();
			all_auto_state.Add(this.CPU_Auto.IsChecked.Value);
			all_auto_state.Add(this.GPU1_Auto.IsChecked.Value);
			all_percentage_state.Add((ulong)this.CPU_ScrollBar.Value * 10UL);
			all_percentage_state.Add((ulong)this.GPU1_ScrollBar.Value * 10UL);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_all_custom_fan_state(all_auto_state, all_percentage_state);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000572E File Offset: 0x0000392E
		private void ShowCoolBoosterInformationicon_Click(object sender, RoutedEventArgs e)
		{
			if (this.ShowCoolBoosterInformationPopup.IsOpen)
			{
				this.ShowCoolBoosterInformationPopup.IsOpen = false;
				return;
			}
			this.ShowCoolBoosterInformationPopup.IsOpen = true;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005758 File Offset: 0x00003958
		private void FanModeCustomAutoCPU_Click(object sender, RoutedEventArgs e)
		{
			if (this.CPU_Auto.IsChecked == true)
			{
				this.CPU_ScrollBar.IsEnabled = false;
				return;
			}
			this.CPU_ScrollBar.IsEnabled = true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000057A0 File Offset: 0x000039A0
		private void FanModeCustomAutoGPU1_Click(object sender, RoutedEventArgs e)
		{
			if (this.GPU1_Auto.IsChecked == true)
			{
				this.GPU1_ScrollBar.IsEnabled = false;
				return;
			}
			this.GPU1_ScrollBar.IsEnabled = true;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000057F0 File Offset: 0x000039F0
		private void ShowCoolBoosterStatusicon_Checked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CoolBoostMode", 1U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_coolboost_state(true);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005840 File Offset: 0x00003A40
		private void ShowCoolBoosterStatusicon_Unchecked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CoolBoostMode", 0U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_coolboost_state(false);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000058A8 File Offset: 0x00003AA8
		private void Custom_Auto_Checked_Unchecked(object sender, RoutedEventArgs e)
		{
			if (this.check_main_initial_flag())
			{
				return;
			}
			CheckBox checkBox = sender as CheckBox;
			Slider slider = (Slider)base.FindName(checkBox.Name.Replace("_Auto", "_ScrollBar"));
			ulong slider_value = (ulong)slider.Value * 10UL;
			CommonFunction.Fan_Group_Type current_group_type = CommonFunction.Fan_Group_Type.fCPU;
			if (Convert.ToInt32(checkBox.Tag) == 0)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanCustomAuto", checkBox.IsChecked.Value ? 1U : 0U);
				current_group_type = CommonFunction.Fan_Group_Type.fCPU;
			}
			else if (Convert.ToInt32(checkBox.Tag) == 1)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanCustomAuto", checkBox.IsChecked.Value ? 1U : 0U);
				current_group_type = CommonFunction.Fan_Group_Type.fGPU;
			}
			bool state = checkBox.IsChecked.Value;
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_single_custom_fan_state(state, slider_value, current_group_type);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000059A4 File Offset: 0x00003BA4
		private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.is_drag)
			{
				return;
			}
			if (this.check_main_initial_flag())
			{
				return;
			}
			Slider slider = sender as Slider;
			uint num = (uint)slider.Value * 10U;
			this.Do_fan_custom_ScrollBar_value_change(Convert.ToInt32(slider.Tag), num);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000059E8 File Offset: 0x00003BE8
		private void Common_Popup_Closed(object sender, EventArgs e)
		{
			if (Startup._support_OC)
			{
				OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
				oc_MainWindow.setting_Popup_Closed(sender, e);
				return;
			}
			OC_MainWindow oc_MainWindow2 = (OC_MainWindow)Window.GetWindow(this);
			oc_MainWindow2.setting_Popup_Closed(sender, e);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005A28 File Offset: 0x00003C28
		private void Common_Popup_Opened(object sender, EventArgs e)
		{
			if (Startup._support_OC)
			{
				OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
				oc_MainWindow.setting_Popup_Opened(sender, e);
				return;
			}
			OC_MainWindow oc_MainWindow2 = (OC_MainWindow)Window.GetWindow(this);
			oc_MainWindow2.setting_Popup_Opened(sender, e);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005A68 File Offset: 0x00003C68
		private List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
		{
			List<T> list4;
			try
			{
				List<T> list = new List<T>();
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(obj, i);
					if (child != null && child is T)
					{
						list.Add((T)((object)child));
						List<T> list2 = this.FindVisualChild<T>(child);
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
					else
					{
						List<T> list3 = this.FindVisualChild<T>(child);
						if (list3 != null)
						{
							list.AddRange(list3);
						}
					}
				}
				list4 = list;
			}
			catch (Exception)
			{
				list4 = null;
			}
			return list4;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005AF4 File Offset: 0x00003CF4
		private void ScrollBar_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			Slider slider = sender as Slider;
			uint num = (uint)slider.Value * 10U;
			this.Do_fan_custom_ScrollBar_value_change(Convert.ToInt32(slider.Tag), num);
			this.is_drag = false;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005B2C File Offset: 0x00003D2C
		private void ScrollBar_DragStarted(object sender, DragStartedEventArgs e)
		{
			this.is_drag = true;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005B38 File Offset: 0x00003D38
		private void Do_fan_custom_ScrollBar_value_change(int fan_type, uint slider_value)
		{
			CommonFunction.Fan_Group_Type fan_Group_Type = CommonFunction.Fan_Group_Type.fCPU;
			if (fan_type == 0)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanPercentage", slider_value);
				fan_Group_Type = CommonFunction.Fan_Group_Type.fCPU;
			}
			else if (fan_type == 1)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanPercentage", slider_value);
				fan_Group_Type = CommonFunction.Fan_Group_Type.fGPU;
			}
			CommonFunction.set_single_custom_fan_speed((ulong)slider_value, fan_Group_Type);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005B80 File Offset: 0x00003D80
		private bool check_main_initial_flag()
		{
			bool flag;
			if (Startup._support_OC)
			{
				OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
				flag = oc_MainWindow.initial_flag;
			}
			else
			{
				OC_MainWindow oc_MainWindow2 = (OC_MainWindow)Window.GetWindow(this);
				flag = oc_MainWindow2.initial_flag;
			}
			return flag;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005BC0 File Offset: 0x00003DC0
		public void Support_OC_Style()
		{
			this.fan_control_Grid.RowDefinitions[0].Height = new GridLength(66.0);
			this.CPU_speed_control_Grid.RowDefinitions[0].Height = new GridLength(139.0);
			this.CPU_information_Grid.RowDefinitions[0].Height = new GridLength(48.0);
			this.GPU_information_Grid.RowDefinitions[0].Height = new GridLength(48.0);
			this.GPU_speed_control_Grid.RowDefinitions[0].Height = new GridLength(139.0);
		}

		// Token: 0x0400004F RID: 79
		public bool initial_flag;

		// Token: 0x04000050 RID: 80
		private bool is_drag;

		// Token: 0x04000051 RID: 81
		private int lastAngle_CPU;

		// Token: 0x04000052 RID: 82
		private int lastAngle_GPU1;
	}
}
