using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000013 RID: 19
	public partial class OverclockPage : UserControl
	{
		// Token: 0x0600013E RID: 318 RVA: 0x0000EE44 File Offset: 0x0000D044
		public OverclockPage()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			base.Loaded += this.Window_Loaded;
			this.GPUModeNormal.Click += this.GPUModeNormal_Click;
			this.GPUModeFaster.Click += this.GPUModeFaster_Click;
			this.GPUModeTurbo.Click += this.GPUModeTurbo_Click;
			this.CPU_Frequency.Text = "--";
			this.CPU_Usage.Text = Startup.langRd["MUI_Loading"].ToString() + "  --%";
			this.GPU1_Frequency.Text = "--";
			this.GPU1_Usage.Text = Startup.langRd["MUI_Loading"].ToString() + "  --%";
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
			this.WaringOverlocking.Visibility = Visibility.Hidden;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public void SetCPUMode(int InitialCPUMode, int cpuNormal_maxMHz, int cpuFaster_maxMHz, int cpuTurbo_maxMHz)
		{
			if (cpuNormal_maxMHz != 0)
			{
				this.CpuNormal_maxMHz = cpuNormal_maxMHz;
			}
			if (cpuFaster_maxMHz != 0)
			{
				this.CpuFaster_maxMHz = cpuFaster_maxMHz;
			}
			if (cpuTurbo_maxMHz != 0)
			{
				this.CpuTurbo_maxMHz = cpuTurbo_maxMHz;
			}
			if (InitialCPUMode.Equals(0))
			{
				this.BeforeUnpluggedCPUMode = 0;
				if (this.CpuNormal_maxMHz == 0)
				{
					this.current_cpu_maxMHz = 2000;
				}
				else
				{
					this.current_cpu_maxMHz = this.CpuNormal_maxMHz;
				}
				this.current_cpu_maxangle = 144;
				return;
			}
			if (InitialCPUMode.Equals(1))
			{
				if (this.CpuFaster_maxMHz == 0)
				{
					this.current_cpu_maxMHz = 2500;
				}
				else
				{
					this.current_cpu_maxMHz = this.CpuFaster_maxMHz;
				}
				this.current_cpu_maxangle = 192;
				this.BeforeUnpluggedCPUMode = 1;
				if (!this.ACPluggedStatus)
				{
					return;
				}
			}
			else if (InitialCPUMode.Equals(2))
			{
				if (this.CpuTurbo_maxMHz == 0)
				{
					this.current_cpu_maxMHz = 3000;
				}
				else
				{
					this.current_cpu_maxMHz = this.CpuTurbo_maxMHz;
				}
				this.current_cpu_maxangle = 240;
				this.BeforeUnpluggedCPUMode = 2;
				bool acpluggedStatus = this.ACPluggedStatus;
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000F084 File Offset: 0x0000D284
		public void SetGPUMode(int InitialGPUMode, int gpuNormal_maxMHz, int gpuFaster_maxMHz, int gpuTurbo_maxMHz)
		{
			if (gpuNormal_maxMHz != 0)
			{
				this.CpuNormal_maxMHz = gpuNormal_maxMHz;
			}
			if (gpuFaster_maxMHz != 0)
			{
				this.CpuFaster_maxMHz = gpuFaster_maxMHz;
			}
			if (gpuTurbo_maxMHz != 0)
			{
				this.CpuTurbo_maxMHz = gpuTurbo_maxMHz;
			}
			if (InitialGPUMode.Equals(0))
			{
				this.GPUModeNormal.IsChecked = new bool?(true);
				this.GPUModeFaster.IsChecked = new bool?(false);
				this.GPUModeTurbo.IsChecked = new bool?(false);
				this.BeforeUnpluggedGPUMode = 0;
				this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Visible;
				this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
				this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
				if (this.GpuNormal_maxMHz == 0)
				{
					this.current_gpu_maxMHz = 2000;
				}
				else
				{
					this.current_gpu_maxMHz = this.GpuNormal_maxMHz;
				}
				if (this.GPU2Normal_maxMHz == 0)
				{
					this.current_GPU2_maxMHz = 2000;
				}
				else
				{
					this.current_GPU2_maxMHz = this.GPU2Normal_maxMHz;
				}
				this.current_gpu_maxangle = 144;
				return;
			}
			if (!InitialGPUMode.Equals(1))
			{
				if (InitialGPUMode.Equals(2))
				{
					if (this.GpuTurbo_maxMHz == 0)
					{
						this.current_gpu_maxMHz = 3000;
					}
					else
					{
						this.current_gpu_maxMHz = this.GpuTurbo_maxMHz;
					}
					if (this.GPU2Turbo_maxMHz == 0)
					{
						this.current_GPU2_maxMHz = 3000;
					}
					else
					{
						this.current_GPU2_maxMHz = this.GPU2Turbo_maxMHz;
					}
					this.current_gpu_maxangle = 240;
					this.BeforeUnpluggedGPUMode = 2;
					if (!this.ACPluggedStatus)
					{
						return;
					}
					this.GPUModeNormal.IsChecked = new bool?(false);
					this.GPUModeFaster.IsChecked = new bool?(false);
					this.GPUModeTurbo.IsChecked = new bool?(true);
					this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Hidden;
					this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
					this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Visible;
				}
				return;
			}
			if (this.GpuFaster_maxMHz == 0)
			{
				this.current_gpu_maxMHz = 2500;
			}
			else
			{
				this.current_gpu_maxMHz = this.GpuFaster_maxMHz;
			}
			if (this.GPU2Faster_maxMHz == 0)
			{
				this.current_GPU2_maxMHz = 2500;
			}
			else
			{
				this.current_GPU2_maxMHz = this.GPU2Faster_maxMHz;
			}
			this.current_gpu_maxangle = 192;
			this.BeforeUnpluggedGPUMode = 1;
			if (!this.ACPluggedStatus)
			{
				return;
			}
			this.GPUModeNormal.IsChecked = new bool?(false);
			this.GPUModeFaster.IsChecked = new bool?(true);
			this.GPUModeTurbo.IsChecked = new bool?(false);
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Visible;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000F2F0 File Offset: 0x0000D4F0
		public void SetGPUMaxMHz(int GPUmaxMHz)
		{
			if (this.GPUModeNormal.IsChecked == true)
			{
				this.GpuNormal_maxMHz = GPUmaxMHz;
				if (this.GpuNormal_maxMHz == 0)
				{
					this.current_gpu_maxMHz = 2000;
				}
				else
				{
					this.current_gpu_maxMHz = this.GpuNormal_maxMHz;
				}
				this.current_gpu_maxangle = 144;
				return;
			}
			if (this.GPUModeFaster.IsChecked == true)
			{
				this.GpuFaster_maxMHz = GPUmaxMHz;
				if (this.GpuFaster_maxMHz == 0)
				{
					this.current_gpu_maxMHz = 2500;
				}
				else
				{
					this.current_gpu_maxMHz = this.GpuFaster_maxMHz;
				}
				this.current_gpu_maxangle = 192;
				return;
			}
			if (this.GPUModeTurbo.IsChecked == true)
			{
				this.GpuTurbo_maxMHz = GPUmaxMHz;
				if (this.GpuTurbo_maxMHz == 0)
				{
					this.current_gpu_maxMHz = 3000;
				}
				else
				{
					this.current_gpu_maxMHz = this.GpuTurbo_maxMHz;
				}
				this.current_gpu_maxangle = 240;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000F3FC File Offset: 0x0000D5FC
		public int GetGPUMode()
		{
			if (this.GPUModeNormal.IsChecked == true)
			{
				return 0;
			}
			if (this.GPUModeFaster.IsChecked == true)
			{
				return 1;
			}
			if (this.GPUModeTurbo.IsChecked == true)
			{
				return 2;
			}
			return -1;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000F473 File Offset: 0x0000D673
		public void PushData_CPU(params float[] data)
		{
			if (!this.Init_indicatorAnimation)
			{
				return;
			}
			this.PushData_CPU(DateTime.Now, data);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000F48A File Offset: 0x0000D68A
		public void PushData_GPU1(params float[] data)
		{
			if (!this.Init_indicatorAnimation)
			{
				return;
			}
			this.PushData_GPU1(DateTime.Now, data);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000F4A4 File Offset: 0x0000D6A4
		public void PushData_CPU(DateTime time, params float[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException();
			}
			if (this.current_cpu_maxMHz == 0 || this.current_cpu_maxangle == 0)
			{
				throw new ArgumentNullException();
			}
			this.CPU_Frequency.Text = data[0].ToString();
			this.CPU_Usage.Text = Startup.langRd["MUI_Loading"].ToString() + "  " + data[1].ToString() + "%";
			this.last_cpu_angle = this.current_cpu_angle;
			this.current_cpu_angle = (int)(data[0] / (float)this.current_cpu_maxMHz * (float)this.current_cpu_maxangle);
			if (this.current_cpu_angle > this.current_cpu_maxangle)
			{
				this.current_cpu_angle = this.current_cpu_maxangle;
			}
			DoubleAnimation doubleAnimation = new DoubleAnimation((double)this.last_cpu_angle, (double)this.current_cpu_angle, new Duration(TimeSpan.FromSeconds(1.0)));
			this.CPU_oc_indicator.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000F59C File Offset: 0x0000D79C
		public void PushData_GPU1(DateTime time, params float[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException();
			}
			if (this.current_gpu_maxMHz == 0 || this.current_gpu_maxangle == 0)
			{
				throw new ArgumentNullException();
			}
			this.GPU1_Frequency.Text = data[0].ToString();
			this.GPU1_Usage.Text = Startup.langRd["MUI_Loading"].ToString() + "  " + data[1].ToString() + "%";
			this.last_gpu1_angle = this.current_gpu1_angle;
			this.current_gpu1_angle = (int)(data[0] / (float)this.current_gpu_maxMHz * (float)this.current_gpu_maxangle);
			if (this.current_gpu1_angle > this.current_gpu_maxangle)
			{
				this.current_gpu1_angle = this.current_gpu_maxangle;
			}
			DoubleAnimation doubleAnimation = new DoubleAnimation((double)this.last_gpu1_angle, (double)this.current_gpu1_angle, new Duration(TimeSpan.FromSeconds(1.0)));
			this.GPU1_oc_indicator.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000F694 File Offset: 0x0000D894
		public void Init_oc_indicatorAnimation()
		{
			if (this.Init_indicatorAnimation)
			{
				return;
			}
			DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 240.0, new Duration(TimeSpan.FromSeconds(0.7)));
			doubleAnimation.AutoReverse = true;
			this.CPU_oc_indicator.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
			this.GPU1_oc_indicator.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
			this.last_cpu_angle = this.current_cpu_angle;
			this.current_cpu_angle = 0;
			this.last_gpu1_angle = this.current_gpu1_angle;
			this.current_gpu1_angle = 0;
			this.last_gpu2_angle = this.current_gpu2_angle;
			this.current_gpu2_angle = 0;
			this.Init_indicatorAnimation = true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000F740 File Offset: 0x0000D940
		public void Disabled_OverClocking_ACUnPlugged()
		{
			this.ACPluggedStatus = false;
			this.WaringOverlocking.Visibility = Visibility.Visible;
			this.BeforeUnpluggedGPUMode = this.GetGPUMode();
			this.GPUModeNormal.IsChecked = new bool?(true);
			this.GPUModeFaster.IsChecked = new bool?(false);
			this.GPUModeFaster.IsEnabled = false;
			this.GPUModeTurbo.IsChecked = new bool?(false);
			this.GPUModeTurbo.IsEnabled = false;
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Visible;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		public void Disabled_OverClocking_ACPlugged()
		{
			this.ACPluggedStatus = true;
			this.WaringOverlocking.Visibility = Visibility.Visible;
			this.BeforeUnpluggedGPUMode = this.GetGPUMode();
			this.GPUModeNormal.IsChecked = new bool?(true);
			this.GPUModeFaster.IsChecked = new bool?(false);
			this.GPUModeFaster.IsEnabled = false;
			this.GPUModeTurbo.IsChecked = new bool?(false);
			this.GPUModeTurbo.IsEnabled = false;
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Visible;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000F878 File Offset: 0x0000DA78
		public void Enabled_OverClocking_ACPlugged()
		{
			this.ACPluggedStatus = true;
			this.WaringOverlocking.Visibility = Visibility.Hidden;
			this.SetCPUMode(this.BeforeUnpluggedCPUMode, this.CpuNormal_maxMHz, this.CpuFaster_maxMHz, this.CpuTurbo_maxMHz);
			this.SetGPUMode(this.BeforeUnpluggedGPUMode, this.GpuNormal_maxMHz, this.GpuFaster_maxMHz, this.GpuTurbo_maxMHz);
			this.GPUModeFaster.IsEnabled = true;
			this.GPUModeTurbo.IsEnabled = true;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000F8EC File Offset: 0x0000DAEC
		public void GPUModeNormal_Click(object sender, RoutedEventArgs e)
		{
			if (this.GPUModeNormal.IsChecked == false)
			{
				this.GPUModeNormal.IsChecked = new bool?(true);
				return;
			}
			if (this.GpuNormal_maxMHz == 0)
			{
				this.current_gpu_maxMHz = 2000;
			}
			else
			{
				this.current_gpu_maxMHz = this.GpuNormal_maxMHz;
			}
			if (this.GPU2Normal_maxMHz == 0)
			{
				this.current_GPU2_maxMHz = 2000;
			}
			else
			{
				this.current_GPU2_maxMHz = this.GPU2Normal_maxMHz;
			}
			this.current_gpu_maxangle = 144;
			this.GPUModeFaster.IsChecked = new bool?(false);
			this.GPUModeTurbo.IsChecked = new bool?(false);
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Visible;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
			this.set_gpu_oc_level_and_get_max_frequency(CommonFunction.Overclock_Mode_Type.Normal);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", 0U);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
		public void GPUModeFaster_Click(object sender, RoutedEventArgs e)
		{
			if (this.GPUModeFaster.IsChecked == false)
			{
				this.GPUModeFaster.IsChecked = new bool?(true);
				return;
			}
			if (this.GpuFaster_maxMHz == 0)
			{
				this.current_gpu_maxMHz = 2500;
			}
			else
			{
				this.current_gpu_maxMHz = this.GpuFaster_maxMHz;
			}
			if (this.GPU2Faster_maxMHz == 0)
			{
				this.current_GPU2_maxMHz = 2500;
			}
			else
			{
				this.current_GPU2_maxMHz = this.GPU2Faster_maxMHz;
			}
			this.current_gpu_maxangle = 192;
			this.GPUModeNormal.IsChecked = new bool?(false);
			this.GPUModeTurbo.IsChecked = new bool?(false);
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Visible;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Hidden;
			this.set_gpu_oc_level_and_get_max_frequency(CommonFunction.Overclock_Mode_Type.Faster);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", 1U);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000FAC4 File Offset: 0x0000DCC4
		public void GPUModeTurbo_Click(object sender, RoutedEventArgs e)
		{
			if (this.GPUModeTurbo.IsChecked == false)
			{
				this.GPUModeTurbo.IsChecked = new bool?(true);
				return;
			}
			if (this.GpuTurbo_maxMHz == 0)
			{
				this.current_gpu_maxMHz = 3000;
			}
			else
			{
				this.current_gpu_maxMHz = this.GpuTurbo_maxMHz;
			}
			if (this.GPU2Turbo_maxMHz == 0)
			{
				this.current_GPU2_maxMHz = 3000;
			}
			else
			{
				this.current_GPU2_maxMHz = this.GPU2Turbo_maxMHz;
			}
			this.current_gpu_maxangle = 240;
			this.GPUModeNormal.IsChecked = new bool?(false);
			this.GPUModeFaster.IsChecked = new bool?(false);
			this.GPU1_img_oc_dashboard_normal.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_faster.Visibility = Visibility.Hidden;
			this.GPU1_img_oc_dashboard_turbo.Visibility = Visibility.Visible;
			this.set_gpu_oc_level_and_get_max_frequency(CommonFunction.Overclock_Mode_Type.Turbo);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", 2U);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000FBB0 File Offset: 0x0000DDB0
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			List<TextBlock> list = this.FindVisualChild<TextBlock>(this.GPUModeNormal);
			TextBlock textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Normal"].ToString();
			list = this.FindVisualChild<TextBlock>(this.GPUModeFaster);
			textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Faster"].ToString();
			list = this.FindVisualChild<TextBlock>(this.GPUModeTurbo);
			textBlock = list[0];
			textBlock.Text = Startup.langRd["MUI_Turbo"].ToString();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		private void Common_Popup_Closed(object sender, EventArgs e)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Closed(sender, e);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000FC70 File Offset: 0x0000DE70
		private void Common_Popup_Opened(object sender, EventArgs e)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Opened(sender, e);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000FC94 File Offset: 0x0000DE94
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

		// Token: 0x06000152 RID: 338 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		private void set_gpu_oc_level_and_get_max_frequency(CommonFunction.Overclock_Mode_Type level)
		{
			this.set_home_oc_combobox_index(false, level);
			this.lock_gpu_oc_button(false);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_gpu_oc_level(level).GetAwaiter();
				Thread.Sleep(1000);
				List<int> max_gpu_freq = CommonFunction.get_gpu_frequency(CommonFunction.Frequency_Mode.fMax).GetAwaiter().GetResult();
				Thread.Sleep(1000);
				this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
				{
					this.SetGPUMaxMHz(max_gpu_freq[0]);
					this.lock_gpu_oc_button(true);
				}));
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000FE98 File Offset: 0x0000E098
		public void get_gpu_max_frequency()
		{
			ThreadStart threadStart = delegate
			{
				List<int> max_gpu_freq = CommonFunction.get_gpu_frequency(CommonFunction.Frequency_Mode.fMax).GetAwaiter().GetResult();
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
				{
					this.SetGPUMaxMHz(max_gpu_freq[0]);
				}));
			};
			new Thread(threadStart).Start();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000FEBD File Offset: 0x0000E0BD
		private void lock_gpu_oc_button(bool flag)
		{
			this.GPUModeNormal.IsEnabled = flag;
			this.GPUModeFaster.IsEnabled = flag;
			this.GPUModeTurbo.IsEnabled = flag;
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000FEEF File Offset: 0x0000E0EF
		private void set_home_oc_combobox_index(bool iscpu, CommonFunction.Overclock_Mode_Type type)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
		}

		// Token: 0x0400016F RID: 367
		private int last_cpu_angle;

		// Token: 0x04000170 RID: 368
		private int current_cpu_angle;

		// Token: 0x04000171 RID: 369
		private int last_gpu1_angle;

		// Token: 0x04000172 RID: 370
		private int current_gpu1_angle;

		// Token: 0x04000173 RID: 371
		private int last_gpu2_angle;

		// Token: 0x04000174 RID: 372
		private int current_gpu2_angle;

		// Token: 0x04000175 RID: 373
		private int current_cpu_maxMHz;

		// Token: 0x04000176 RID: 374
		private int current_cpu_maxangle;

		// Token: 0x04000177 RID: 375
		private int current_gpu_maxMHz;

		// Token: 0x04000178 RID: 376
		private int current_gpu_maxangle;

		// Token: 0x04000179 RID: 377
		private bool Init_indicatorAnimation;

		// Token: 0x0400017A RID: 378
		public int BeforeUnpluggedCPUMode;

		// Token: 0x0400017B RID: 379
		public int BeforeUnpluggedGPUMode;

		// Token: 0x0400017C RID: 380
		private bool ACPluggedStatus = true;

		// Token: 0x0400017D RID: 381
		private int CpuNormal_maxMHz;

		// Token: 0x0400017E RID: 382
		private int CpuFaster_maxMHz;

		// Token: 0x0400017F RID: 383
		private int CpuTurbo_maxMHz;

		// Token: 0x04000180 RID: 384
		private int GpuNormal_maxMHz;

		// Token: 0x04000181 RID: 385
		private int GpuFaster_maxMHz;

		// Token: 0x04000182 RID: 386
		private int GpuTurbo_maxMHz;

		// Token: 0x04000183 RID: 387
		private int current_GPU2_maxMHz;

		// Token: 0x04000184 RID: 388
		private int GPU2Normal_maxMHz;

		// Token: 0x04000185 RID: 389
		private int GPU2Faster_maxMHz;

		// Token: 0x04000186 RID: 390
		private int GPU2Turbo_maxMHz;
	}
}
