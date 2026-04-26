using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000005 RID: 5
	public partial class FanControlPage_F : UserControl
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000031D8 File Offset: 0x000013D8
		public FanControlPage_F()
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
			this.setting_auto.Visibility = Visibility.Hidden;
			this.CPU_Auto.Click += this.FanModeCustomAutoCPU_Click;
			this.GPU1_Auto.Click += this.FanModeCustomAutoGPU1_Click;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000033D0 File Offset: 0x000015D0
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.FanModeMax);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Max"].ToString();
			list = this.FindVisualChild<TextBlock>(this.FanModeCustom);
			textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Custom"].ToString();
			if (this.setting_auto.Visibility == Visibility.Visible)
			{
				this.setting_auto.Visibility = Visibility.Hidden;
			}
			else
			{
				this.setting_auto.Visibility = Visibility.Hidden;
			}
			int num = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "AutoFanModeType", 0U);
			if (num == 0)
			{
				list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
				textBlock = list[0];
				textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nNormal";
				this.Normal.FontWeight = FontWeights.UltraBold;
				this.Silence.FontWeight = FontWeights.Normal;
				this.CoolBoost_text.FontWeight = FontWeights.Normal;
				return;
			}
			if (num == 1)
			{
				list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
				textBlock = list[0];
				textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nSilence";
				this.Normal.FontWeight = FontWeights.Normal;
				this.Silence.FontWeight = FontWeights.UltraBold;
				this.CoolBoost_text.FontWeight = FontWeights.Normal;
				return;
			}
			if (num == 2)
			{
				list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
				textBlock = list[0];
				textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nCoolBoost";
				this.Normal.FontWeight = FontWeights.Normal;
				this.Silence.FontWeight = FontWeights.Normal;
				this.CoolBoost_text.FontWeight = FontWeights.UltraBold;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000035BC File Offset: 0x000017BC
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

		// Token: 0x06000033 RID: 51 RVA: 0x00003664 File Offset: 0x00001864
		public void SetCustomFanSpeedMode(params int[] CustomFanSpeedMode)
		{
			this.CPU_ScrollBar.Value = (double)CustomFanSpeedMode[0];
			this.GPU1_ScrollBar.Value = (double)CustomFanSpeedMode[1];
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003684 File Offset: 0x00001884
		public bool[] GetCustomFanAutoMode()
		{
			bool[] array = new bool[4];
			array[0] = this.CPU_Auto.IsChecked.Value;
			array[1] = this.GPU1_Auto.IsChecked.Value;
			return array;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000036C8 File Offset: 0x000018C8
		public int[] GetCustomFanSpeedMode()
		{
			int[] array = new int[4];
			array[0] = (int)this.CPU_ScrollBar.Value;
			array[1] = (int)this.GPU1_ScrollBar.Value;
			return array;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000036FC File Offset: 0x000018FC
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

		// Token: 0x06000037 RID: 55 RVA: 0x00003854 File Offset: 0x00001A54
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

		// Token: 0x06000038 RID: 56 RVA: 0x000038CB File Offset: 0x00001ACB
		public void PassShowCoolBoosterStatusicon(bool CoolBoosterStatus)
		{
			if (CoolBoosterStatus)
			{
				this.ShowCoolBoosterStatusicon.IsChecked = new bool?(true);
				return;
			}
			this.ShowCoolBoosterStatusicon.IsChecked = new bool?(false);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000038F3 File Offset: 0x00001AF3
		public void HideFanControlUIInformationPopup()
		{
			if (this.ShowCoolBoosterInformationPopup.IsOpen)
			{
				this.ShowCoolBoosterInformationPopup.IsOpen = false;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003910 File Offset: 0x00001B10
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

		// Token: 0x0600003B RID: 59 RVA: 0x00003A0C File Offset: 0x00001C0C
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

		// Token: 0x0600003C RID: 60 RVA: 0x00003B20 File Offset: 0x00001D20
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
			if (this.setting_auto.Visibility == Visibility.Hidden)
			{
				this.setting_auto.Visibility = Visibility.Visible;
			}
			else
			{
				this.setting_auto.Visibility = Visibility.Visible;
			}
			int num = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "AutoFanModeType", 0U);
			if (num == 0)
			{
				ThreadStart threadStart = delegate
				{
					CommonFunction.set_auto_normal_state(true);
				};
				new Thread(threadStart).Start();
			}
			else if (num == 1)
			{
				ThreadStart threadStart2 = delegate
				{
					CommonFunction.set_auto_silence_state(true);
				};
				new Thread(threadStart2).Start();
			}
			else if (num == 2)
			{
				ThreadStart threadStart3 = delegate
				{
					CommonFunction.set_auto_coolboost_state(true);
				};
				new Thread(threadStart3).Start();
			}
			if (this.setting_auto.Visibility == Visibility.Hidden)
			{
				this.setting_auto.Visibility = Visibility.Visible;
				return;
			}
			this.setting_auto.Visibility = Visibility.Visible;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003CB8 File Offset: 0x00001EB8
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

		// Token: 0x0600003E RID: 62 RVA: 0x00003DB0 File Offset: 0x00001FB0
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

		// Token: 0x0600003F RID: 63 RVA: 0x00003F0A File Offset: 0x0000210A
		private void ShowCoolBoosterInformationicon_Click(object sender, RoutedEventArgs e)
		{
			if (this.ShowCoolBoosterInformationPopup.IsOpen)
			{
				this.ShowCoolBoosterInformationPopup.IsOpen = false;
				return;
			}
			this.ShowCoolBoosterInformationPopup.IsOpen = true;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003F34 File Offset: 0x00002134
		private void FanModeCustomAutoCPU_Click(object sender, RoutedEventArgs e)
		{
			if (this.CPU_Auto.IsChecked == true)
			{
				this.CPU_ScrollBar.IsEnabled = false;
				return;
			}
			this.CPU_ScrollBar.IsEnabled = true;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003F7C File Offset: 0x0000217C
		private void FanModeCustomAutoGPU1_Click(object sender, RoutedEventArgs e)
		{
			if (this.GPU1_Auto.IsChecked == true)
			{
				this.GPU1_ScrollBar.IsEnabled = false;
				return;
			}
			this.GPU1_ScrollBar.IsEnabled = true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003FCC File Offset: 0x000021CC
		private void ShowCoolBoosterStatusicon_Checked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CoolBoostMode", 1U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_coolboost_state(true);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000401C File Offset: 0x0000221C
		private void ShowCoolBoosterStatusicon_Unchecked(object sender, RoutedEventArgs e)
		{
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CoolBoostMode", 0U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_coolboost_state(false);
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004084 File Offset: 0x00002284
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

		// Token: 0x06000045 RID: 69 RVA: 0x00004180 File Offset: 0x00002380
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

		// Token: 0x06000046 RID: 70 RVA: 0x000041C4 File Offset: 0x000023C4
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

		// Token: 0x06000047 RID: 71 RVA: 0x00004204 File Offset: 0x00002404
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

		// Token: 0x06000048 RID: 72 RVA: 0x00004244 File Offset: 0x00002444
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

		// Token: 0x06000049 RID: 73 RVA: 0x000042D0 File Offset: 0x000024D0
		private void ScrollBar_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			Slider slider = sender as Slider;
			uint num = (uint)slider.Value * 10U;
			this.Do_fan_custom_ScrollBar_value_change(Convert.ToInt32(slider.Tag), num);
			this.is_drag = false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004308 File Offset: 0x00002508
		private void ScrollBar_DragStarted(object sender, DragStartedEventArgs e)
		{
			this.is_drag = true;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004314 File Offset: 0x00002514
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

		// Token: 0x0600004C RID: 76 RVA: 0x0000435C File Offset: 0x0000255C
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

		// Token: 0x0600004D RID: 77 RVA: 0x0000439C File Offset: 0x0000259C
		public void Support_OC_Style()
		{
			this.fan_control_Grid.RowDefinitions[0].Height = new GridLength(66.0);
			this.CPU_speed_control_Grid.RowDefinitions[0].Height = new GridLength(139.0);
			this.CPU_information_Grid.RowDefinitions[0].Height = new GridLength(48.0);
			this.GPU_information_Grid.RowDefinitions[0].Height = new GridLength(48.0);
			this.GPU_speed_control_Grid.RowDefinitions[0].Height = new GridLength(139.0);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004468 File Offset: 0x00002668
		private void Normal_Click(object sender, RoutedEventArgs e)
		{
			this.FanModeAuto.IsChecked = new bool?(true);
			this.FanModeMax.IsChecked = new bool?(false);
			this.FanModeCustom.IsChecked = new bool?(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "AutoFanModeType", 0U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_auto_normal_state(true);
			};
			new Thread(threadStart).Start();
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nNormal";
			this.Normal.FontWeight = FontWeights.UltraBold;
			this.Silence.FontWeight = FontWeights.Normal;
			this.CoolBoost_text.FontWeight = FontWeights.Normal;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004554 File Offset: 0x00002754
		private void Silence_Click(object sender, RoutedEventArgs e)
		{
			this.FanModeAuto.IsChecked = new bool?(true);
			this.FanModeMax.IsChecked = new bool?(false);
			this.FanModeCustom.IsChecked = new bool?(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "AutoFanModeType", 1U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_auto_silence_state(true);
			};
			new Thread(threadStart).Start();
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nSilence";
			this.Silence.FontWeight = FontWeights.UltraBold;
			this.Normal.FontWeight = FontWeights.Normal;
			this.CoolBoost_text.FontWeight = FontWeights.Normal;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004640 File Offset: 0x00002840
		private void CoolBoost_text_Click(object sender, RoutedEventArgs e)
		{
			this.FanModeAuto.IsChecked = new bool?(true);
			this.FanModeMax.IsChecked = new bool?(false);
			this.FanModeCustom.IsChecked = new bool?(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "AutoFanModeType", 2U);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_auto_coolboost_state(true);
			};
			new Thread(threadStart).Start();
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.FanModeAuto);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Auto"].ToString() + "\r\nCoolBoost";
			this.CoolBoost_text.FontWeight = FontWeights.UltraBold;
			this.Silence.FontWeight = FontWeights.Normal;
			this.Normal.FontWeight = FontWeights.Normal;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004722 File Offset: 0x00002922
		private void FanModeAuto_MouseEnter(object sender, MouseEventArgs e)
		{
			if (this.setting_auto.Visibility == Visibility.Hidden)
			{
				this.setting_auto.Visibility = Visibility.Visible;
				return;
			}
			this.setting_auto.Visibility = Visibility.Visible;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000474B File Offset: 0x0000294B
		private void FanModeAuto_MouseDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000474D File Offset: 0x0000294D
		private void FanModeAuto_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.setting_auto.Visibility == Visibility.Visible)
			{
				this.setting_auto.Visibility = Visibility.Hidden;
				return;
			}
			this.setting_auto.Visibility = Visibility.Hidden;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004775 File Offset: 0x00002975
		private void Canvas_MouseEnter(object sender, MouseEventArgs e)
		{
			if (this.setting_auto.Visibility == Visibility.Hidden)
			{
				this.setting_auto.Visibility = Visibility.Visible;
				return;
			}
			this.setting_auto.Visibility = Visibility.Visible;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000479E File Offset: 0x0000299E
		private void Canvas_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.setting_auto.Visibility == Visibility.Visible)
			{
				this.setting_auto.Visibility = Visibility.Hidden;
				return;
			}
			this.setting_auto.Visibility = Visibility.Hidden;
		}

		// Token: 0x04000024 RID: 36
		public bool initial_flag;

		// Token: 0x04000025 RID: 37
		private bool is_drag;

		// Token: 0x04000026 RID: 38
		private int lastAngle_CPU;

		// Token: 0x04000027 RID: 39
		private int lastAngle_GPU1;
	}
}
