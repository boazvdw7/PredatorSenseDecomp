using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x0200006A RID: 106
	public partial class MainWindow : Window
	{
		// Token: 0x0600032D RID: 813 RVA: 0x00026E40 File Offset: 0x00025040
		public MainWindow()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(false);
			string text = Assembly.GetExecutingAssembly().FullName.Split(new char[] { ',' })[0];
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\Log", text))
			{
				string text2 = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("PROGRAMDATA"),
					"\\OEM\\PredatorSense\\Log\\",
					Environment.UserName,
					"_",
					text,
					".log"
				});
				this._log = new Log(text2, text);
			}
			this.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
			this.FanControl_Page = new FanControlPage();
			this.FanControl_Page.Width = 1152.0;
			this.FanControl_Page.Height = 312.0;
			this.FanControl_Page.HorizontalAlignment = HorizontalAlignment.Left;
			this.FanControl_Page.VerticalAlignment = VerticalAlignment.Top;
			Grid.SetRow(this.FanControl_Page, 0);
			Grid.SetColumn(this.FanControl_Page, 0);
			this.Content_Grid.Children.Add(this.FanControl_Page);
			this.Monitoring_Page = new MonitoringPage();
			this.Monitoring_Page.Width = 1152.0;
			this.Monitoring_Page.Height = 312.0;
			this.Monitoring_Page.HorizontalAlignment = HorizontalAlignment.Left;
			this.Monitoring_Page.VerticalAlignment = VerticalAlignment.Top;
			Grid.SetRow(this.Monitoring_Page, 1);
			Grid.SetColumn(this.Monitoring_Page, 0);
			this.Content_Grid.Children.Add(this.Monitoring_Page);
			if (File.Exists(this.NVIDIA_Experience_file))
			{
				this.gfe_Button.Visibility = Visibility.Visible;
				return;
			}
			if (File.Exists(this.NVIDIA_Experience_filex86))
			{
				this.gfe_Button.Visibility = Visibility.Visible;
				return;
			}
			this.gfe_Button.Visibility = Visibility.Hidden;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000271A7 File Offset: 0x000253A7
		private void Setting_Button_Click(object sender, RoutedEventArgs e)
		{
			if (!this.setting_Popup.IsOpen)
			{
				this.setting_Popup.IsOpen = true;
				return;
			}
			this.setting_Popup.IsOpen = false;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000271CF File Offset: 0x000253CF
		public void setting_Popup_Closed(object sender, EventArgs e)
		{
			if (this.Mask_Rectangle.Visibility == Visibility.Visible)
			{
				this.Mask_Rectangle.Visibility = Visibility.Hidden;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000271EC File Offset: 0x000253EC
		private void Close_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Application.Current.Shutdown();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00027218 File Offset: 0x00025418
		private void Minimize_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				base.WindowState = WindowState.Minimized;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00027244 File Offset: 0x00025444
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0002725C File Offset: 0x0002545C
		private void mainWindow_SourceInitialized(object sender, EventArgs e)
		{
			try
			{
				new WindowInteropHelper(this);
				HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
				hwndSource.AddHook(new HwndSourceHook(this.WndProc));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000272A4 File Offset: 0x000254A4
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 32772)
			{
				this.load_advance_settings();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000272B9 File Offset: 0x000254B9
		private void stickykey_Checkbox_Checked(object sender, RoutedEventArgs e)
		{
			this.SetStickyKey(true);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "StickyKey", 1U);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000272D2 File Offset: 0x000254D2
		private void stickykey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
		{
			this.SetStickyKey(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "StickyKey", 0U);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000274EC File Offset: 0x000256EC
		private async void SetStickyKey(bool status)
		{
			try
			{
				NamedPipeClientStream client = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				client.Connect();
				Task<int> tsk = new Task<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(client, 2, new object[] { Convert.ToUInt32(status) });
					client.WaitForPipeDrain();
					return 0;
				});
				tsk.Start();
				await tsk;
				client.Close();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00027730 File Offset: 0x00025930
		private async void SetWinMenuKey(bool status)
		{
			try
			{
				NamedPipeClientStream client = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				client.Connect();
				Task<int> tsk = new Task<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(client, 9, new object[] { Convert.ToUInt32(status) });
					client.WaitForPipeDrain();
					return 0;
				});
				tsk.Start();
				await tsk;
				client.Close();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00027774 File Offset: 0x00025974
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "load_advance_settings");
			this.load_advance_settings();
			Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "initial_fan_page");
			this.initial_fan_page();
			this.tokenSource = new CancellationTokenSource();
			this.pollingMainWindowsStatus(this.tokenSource.Token);
			this.initial_flag = false;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00027838 File Offset: 0x00025A38
		private void pollingMainWindowsStatus(CancellationToken token)
		{
			try
			{
				this.temperature_and_usage_pollingTask = Task.Factory.StartNew(delegate
				{
					do
					{
						this.get_temperature_frequency_usage_info_data();
						Thread.Sleep(5000);
					}
					while (!token.IsCancellationRequested);
				}, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
				this.fan_speed_pollingTask = Task.Factory.StartNew(delegate
				{
					do
					{
						this.get_fan_speed_info_data();
						Thread.Sleep(5000);
					}
					while (!token.IsCancellationRequested);
				}, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00027A90 File Offset: 0x00025C90
		private async void PollingStop()
		{
			try
			{
				if (this.tokenSource != null)
				{
					this.tokenSource.Cancel();
					await this.temperature_and_usage_pollingTask;
					await this.fan_speed_pollingTask;
					this.tokenSource.Dispose();
					this.tokenSource = null;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00027ACC File Offset: 0x00025CCC
		private void initial_fan_page()
		{
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode"))
			{
				int num = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode", 0U);
				int num2 = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanPercentage", 0U);
				int num3 = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanPercentage", 0U);
				bool flag = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanCustomAuto", 0U) == 1;
				bool flag2 = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanCustomAuto", 0U) == 1;
				bool flag3 = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CoolBoostMode", 0U) == 1;
				this.FanControl_Page.PassShowCoolBoosterStatusicon(flag3);
				this.FanControl_Page.SetFanMode(num);
				this.FanControl_Page.SetCustomFanSpeedMode(new int[]
				{
					num2 / 10,
					num3 / 10
				});
				this.FanControl_Page.SetCustomFanAutoMode(new bool[] { flag, flag2 });
				this.main_fan_mode_switch(num);
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00027CD0 File Offset: 0x00025ED0
		private void get_temperature_frequency_usage_info_data()
        {
            // Variables extracted from the original display class
            bool gpu_enable_flag = false;
            int[] temperature_data_pool = new int[] { -1, -1, -1, -1 };
            bool[] ret_pool = new bool[4];
            var gpu_usage_data = CommonFunction.get_gpu_usage_loading().GetAwaiter().GetResult();

            int result = CommonFunction.get_gpu_coproc_status().GetAwaiter().GetResult();
            gpu_enable_flag = result == 1;

            if (CommonFunction.get_wmi_system_health_info(ref temperature_data_pool[0], CommonFunction.System_Health_Information_Index.sCPU_Temperature))
            {
                ret_pool[0] = true;
            }
            if (CommonFunction.get_wmi_system_health_info(ref temperature_data_pool[2], CommonFunction.System_Health_Information_Index.sGPU1_Temperature) && gpu_usage_data.Count > 0)
            {
                ret_pool[2] = true;
            }

            base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                if (ret_pool[0])
                {
                    int num = (int)this.cpuCounter.NextValue();
                    this.Monitoring_Page.PushData_CPU(new float[]
                    {
                        (float)temperature_data_pool[0],
                        (float)num
                    });
                }
                if (gpu_enable_flag && ret_pool[2])
                {
                    if (temperature_data_pool[2] > 0)
                    {
                        this.Monitoring_Page.PushData_GPU(new float[]
                        {
                            (float)temperature_data_pool[2],
                            gpu_usage_data[0]
                        });
                    }
                    else
                    {
                        gpu_enable_flag = false;
                    }
                }
                if (!gpu_enable_flag)
                {
                    this.Monitoring_Page.PushData_GPU(new float[2]);
                }
                this.Monitoring_Page.update_discrete_gpu_state(gpu_enable_flag);
            }));
        }

		// Token: 0x0600033E RID: 830 RVA: 0x00027E24 File Offset: 0x00026024
		private void get_fan_speed_info_data()
        {
            // Prepare data and result arrays
            int[] data_pool = new int[] { -1, -1, -1, -1, -1 };
            bool[] ret_pool = new bool[5];

            // Query CPU fan speed
            if (CommonFunction.get_wmi_system_health_info(ref data_pool[0], CommonFunction.System_Health_Information_Index.sCPU_Fan_Speed))
            {
                ret_pool[0] = true;
            }
            // Query GPU fan speed
            if (CommonFunction.get_wmi_system_health_info(ref data_pool[3], CommonFunction.System_Health_Information_Index.sGPU_Fan_Speed))
            {
                ret_pool[3] = true;
            }

            // Update UI on dispatcher
            base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                if (ret_pool[0])
                {
                    this.FanControl_Page.PushData_CPU((double)data_pool[0]);
                }
                if (ret_pool[3])
                {
                    this.FanControl_Page.PushData_GPU1((double)data_pool[3]);
                }
            }));
        }
		

		// Token: 0x0600033F RID: 831 RVA: 0x00027EB8 File Offset: 0x000260B8
		private void load_advance_settings()
		{
			if (Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "StickyKey", 0U) == 0)
			{
				this.stickykey_Checkbox.IsChecked = new bool?(false);
			}
			else
			{
				this.stickykey_Checkbox.IsChecked = new bool?(true);
			}
			if (Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "Degree", 0U) == 0)
			{
				this.C_RadioButton.IsChecked = new bool?(true);
			}
			else
			{
				this.F_RadioButton.IsChecked = new bool?(true);
			}
			if (Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "WinAndMenuKey", 0U) == 0)
			{
				this.winmenukey_Checkbox.IsChecked = new bool?(false);
				return;
			}
			this.winmenukey_Checkbox.IsChecked = new bool?(true);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00027F66 File Offset: 0x00026166
		public void setting_Popup_Opened(object sender, EventArgs e)
		{
			this.Mask_Rectangle.Visibility = Visibility.Visible;
			this.Mask_Rectangle.MouseLeftButtonDown += this.Mask_Rectangle_MouseLeftButtonDown;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00027F8B File Offset: 0x0002618B
		private void remove_textbox_focus()
		{
			this.Main_Grid.Focusable = true;
			this.Main_Grid.Focus();
			this.Main_Grid.Focusable = false;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00027FB1 File Offset: 0x000261B1
		private void mainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.MainTitle_Rectangle.Focusable = true;
			this.MainTitle_Rectangle.Focus();
			this.MainTitle_Rectangle.Focusable = false;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00027FD8 File Offset: 0x000261D8
		private void F_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			if (!this.initial_flag)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "Degree", 1U);
				this.Monitoring_Page.update_title_text(CommonFunction.Degree_Type.dFahrenheit);
			}
			this.advance_F_TextBlock.Foreground = new SolidColorBrush(CommonFunction.ColorFromString("#E60000"));
			this.advance_C_TextBlock.Foreground = new SolidColorBrush(CommonFunction.ColorFromString("#808080"));
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00028040 File Offset: 0x00026240
		private void C_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			if (!this.initial_flag)
			{
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "Degree", 0U);
				this.Monitoring_Page.update_title_text(CommonFunction.Degree_Type.dCelsius);
			}
			this.advance_F_TextBlock.Foreground = new SolidColorBrush(CommonFunction.ColorFromString("#808080"));
			this.advance_C_TextBlock.Foreground = new SolidColorBrush(CommonFunction.ColorFromString("#E60000"));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000280A8 File Offset: 0x000262A8
		private void Main_Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				try
				{
					base.DragMove();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000280D8 File Offset: 0x000262D8
		public void main_fan_mode_switch(int mode)
		{
			if (mode == 0)
			{
				this.FanControl_Page.SetFanMode(0);
				this.FanControl_Page.FanModeAuto_Click(null, null);
				return;
			}
			if (mode == 1)
			{
				this.FanControl_Page.SetFanMode(1);
				this.FanControl_Page.FanModeMax_Click(null, null);
				return;
			}
			if (mode == 2)
			{
				this.FanControl_Page.SetFanMode(2);
				this.FanControl_Page.FanModeCustom_Click(null, null);
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0002813D File Offset: 0x0002633D
		private void winmenukey_Checkbox_Checked(object sender, RoutedEventArgs e)
		{
			CommonFunction.set_win_menu_key_status(true);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "WinAndMenuKey", 1U);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00028155 File Offset: 0x00026355
		private void winmenukey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
		{
			CommonFunction.set_win_menu_key_status(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "WinAndMenuKey", 0U);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00028170 File Offset: 0x00026370
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

		// Token: 0x0600034A RID: 842 RVA: 0x0002820D File Offset: 0x0002640D
		private void mainWindow_Closing(object sender, CancelEventArgs e)
		{
			this.PollingStop();
			Environment.Exit(0);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0002821B File Offset: 0x0002641B
		private void Mask_Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.Mask_Rectangle.MouseLeftButtonDown -= this.Mask_Rectangle_MouseLeftButtonDown;
			this.Mask_Rectangle.Visibility = Visibility.Hidden;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00028240 File Offset: 0x00026440
		private void gfe_Button_MouseEnter(object sender, MouseEventArgs e)
		{
			this.gfe_Button.Opacity = 0.9;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00028256 File Offset: 0x00026456
		private void gfe_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			this.gfe_Button.Opacity = 0.6;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0002826C File Offset: 0x0002646C
		private void gfe_Button_Initialized(object sender, EventArgs e)
		{
			this.gfe_Button.Opacity = 0.6;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00028284 File Offset: 0x00026484
		private void gfe_Button_Click(object sender, RoutedEventArgs e)
		{
			this.gfe_Button.Opacity = 1.0;
			try
			{
				if (File.Exists(this.NVIDIA_Experience_file))
				{
					new Process
					{
						StartInfo = new ProcessStartInfo("C:\\Program Files\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe")
					}.Start();
				}
				else if (File.Exists(this.NVIDIA_Experience_filex86))
				{
					new Process
					{
						StartInfo = new ProcessStartInfo("C:\\Program Files (x86)\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe")
					}.Start();
				}
				else
				{
					this.gfe_Button.Visibility = Visibility.Hidden;
				}
			}
			catch (Exception)
			{
				this.gfe_Button.Visibility = Visibility.Hidden;
			}
		}

		// Token: 0x0400041E RID: 1054
		private const string LOG_REGISTRY_PATH = "SOFTWARE\\OEM\\PredatorSense\\Log";

		// Token: 0x0400041F RID: 1055
		private Log _log;

		// Token: 0x04000420 RID: 1056
		private string[] basic_color_table = new string[] { "#FF0000", "#FFA000", "#FFFF00", "#64FF00", "#00FFFF", "#00B4FF", "#304FFE", "#FF3CFF", "#AA00FF", "#FFFFFF" };

		// Token: 0x04000421 RID: 1057
		private string[] g_recent_color = new string[] { "", "", "", "", "", "", "", "", "", "" };

		// Token: 0x04000422 RID: 1058
		private string[] g_lighting_recent_color = new string[] { "", "", "", "", "", "", "", "", "", "" };

		// Token: 0x04000423 RID: 1059
		public bool initial_flag = true;

		// Token: 0x04000424 RID: 1060
		private Task temperature_and_usage_pollingTask;

		// Token: 0x04000425 RID: 1061
		private Task fan_speed_pollingTask;

		// Token: 0x04000426 RID: 1062
		private CancellationTokenSource tokenSource;

		// Token: 0x04000427 RID: 1063
		private PerformanceCounter cpuCounter;

		// Token: 0x04000428 RID: 1064
		private FanControlPage FanControl_Page;

		// Token: 0x04000429 RID: 1065
		private MonitoringPage Monitoring_Page;

		// Token: 0x0400042A RID: 1066
		public string NVIDIA_Experience_filex86 = "C:\\Program Files (x86)\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe";

		// Token: 0x0400042B RID: 1067
		public string NVIDIA_Experience_file = "C:\\Program Files\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe";
	}
}