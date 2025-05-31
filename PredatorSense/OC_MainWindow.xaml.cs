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
	// Token: 0x0200000E RID: 14
	public partial class OC_MainWindow : Window
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0000A36C File Offset: 0x0000856C
		public OC_MainWindow(CommonFunction.AC_Mode_Type ac_mode, bool battery_boost_status)
		{
			this.InitializeComponent();
			this.advance_C_TextBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.advance_F_TextBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
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
			this.current_ac_mode = ac_mode;
			this.g_battery_boost = battery_boost_status;
			this.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
			this.FanControl_Page = new FanControlPage();
			this.FanControl_Page.Width = 1152.0;
			this.FanControl_Page.Height = 312.0;
			this.FanControl_Page.HorizontalAlignment = HorizontalAlignment.Left;
			this.FanControl_Page.VerticalAlignment = VerticalAlignment.Top;
			Grid.SetRow(this.FanControl_Page, 0);
			Grid.SetColumn(this.FanControl_Page, 0);
			this.Content_Grid.Children.Add(this.FanControl_Page);
			this.Monitoring_Page = new OC_MonitoringPage();
			this.Monitoring_Page.Width = 1152.0;
			this.Monitoring_Page.Height = 312.0;
			this.Monitoring_Page.HorizontalAlignment = HorizontalAlignment.Left;
			this.Monitoring_Page.VerticalAlignment = VerticalAlignment.Top;
			if (File.Exists(this.NVIDIA_Experience_file64))
			{
				this.gfe_Button.Visibility = Visibility.Visible;
			}
			else if (File.Exists(this.NVIDIA_Experience_filex86))
			{
				this.gfe_Button.Visibility = Visibility.Visible;
			}
			else
			{
				this.gfe_Button.Visibility = Visibility.Hidden;
			}
			Grid.SetRow(this.Monitoring_Page, 1);
			Grid.SetColumn(this.Monitoring_Page, 0);
			if (CommonFunction.complete_loading)
			{
				this.Monitoring_Page.OC_Grid.Visibility = Visibility.Hidden;
				this.OC_text.Visibility = Visibility.Hidden;
				this.Content_Grid.Children.Add(this.Monitoring_Page);
			}
			else
			{
				this.Content_Grid.Children.Add(this.Monitoring_Page);
			}
			this.stickykey_Checkbox.Checked += this.stickykey_Checkbox_Checked;
			this.stickykey_Checkbox.Unchecked += this.stickykey_Checkbox_Unchecked;
			if (CommonFunction.Get_STICKYK_Status())
			{
				if (CommonFunction.Get_STICKYK_Status().ToString() == "True")
				{
					this.stickykey_Checkbox.IsChecked = new bool?(true);
					return;
				}
			}
			else if (CommonFunction.Get_STICKYK_Status().ToString() == "False")
			{
				this.stickykey_Checkbox.IsChecked = new bool?(false);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000A7EA File Offset: 0x000089EA
		private void Setting_Button_Click(object sender, RoutedEventArgs e)
		{
			if (!this.setting_Popup.IsOpen)
			{
				this.setting_Popup.IsOpen = true;
				return;
			}
			this.setting_Popup.IsOpen = false;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000A812 File Offset: 0x00008A12
		public void setting_Popup_Closed(object sender, EventArgs e)
		{
			if (this.Mask_Rectangle.Visibility == Visibility.Visible)
			{
				this.Mask_Rectangle.Visibility = Visibility.Hidden;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000A830 File Offset: 0x00008A30
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

		// Token: 0x060000ED RID: 237 RVA: 0x0000A85C File Offset: 0x00008A5C
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

		// Token: 0x060000EE RID: 238 RVA: 0x0000A888 File Offset: 0x00008A88
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000A8A0 File Offset: 0x00008AA0
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

		// Token: 0x060000F0 RID: 240 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 32772)
			{
				this.load_advance_settings();
			}
			else if ((long)msg == 32775L)
			{
				Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, string.Concat(new object[]
				{
					"Message: UICMD_PWS_BATTERY_BOOST_CHANGE, wParam: ",
					wParam.ToInt32(),
					" lParam: ",
					lParam.ToInt32()
				}));
				bool flag = wParam.ToInt32() == 1;
				Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "g_battery_boost = " + this.g_battery_boost.ToString());
				if (flag != this.g_battery_boost)
				{
					Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "change");
					this.g_battery_boost = flag;
					this.Monitoring_Page.disable_GPU_overclock_control();
				}
			}
			else if ((long)msg == 32774L)
			{
				Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, string.Concat(new object[]
				{
					"Message: UICMD_PWS_CHANGE, wParam: ",
					wParam.ToInt32(),
					" lParam: ",
					lParam.ToInt32()
				}));
				bool flag2 = false;
				if (wParam.ToInt32() == 1)
				{
					if (this.current_ac_mode != CommonFunction.AC_Mode_Type.a2_AC)
					{
						flag2 = true;
					}
					this.current_ac_mode = CommonFunction.AC_Mode_Type.a2_AC;
				}
				else
				{
					if (this.current_ac_mode == CommonFunction.AC_Mode_Type.a2_AC)
					{
						flag2 = true;
					}
					this.current_ac_mode = CommonFunction.AC_Mode_Type.aNo_AC;
				}
				if (flag2)
				{
					this.Monitoring_Page.disable_GPU_overclock_control();
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000AA81 File Offset: 0x00008C81
		private void stickykey_Checkbox_Checked(object sender, RoutedEventArgs e)
		{
			this.SetStickyKey(true);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "StickyKey", 1U);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000AA9A File Offset: 0x00008C9A
		private void stickykey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
		{
			this.SetStickyKey(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "StickyKey", 0U);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000ACB4 File Offset: 0x00008EB4
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

		// Token: 0x060000F4 RID: 244 RVA: 0x0000AEF8 File Offset: 0x000090F8
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

		// Token: 0x060000F5 RID: 245 RVA: 0x0000AF3C File Offset: 0x0000913C
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "initial_overclock");
			this.Monitoring_Page.disable_GPU_overclock_control();
			Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "load_advance_settings");
			this.load_advance_settings();
			Log.LogWrite(this._log, LogType.Info, MethodBase.GetCurrentMethod().Name, "initial_fan_page");
			this.initial_fan_page();
			this.tokenSource = new CancellationTokenSource();
			this.pollingMainWindowsStatus(this.tokenSource.Token);
			this.initial_flag = false;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000B024 File Offset: 0x00009224
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

		// Token: 0x060000F7 RID: 247 RVA: 0x0000B27C File Offset: 0x0000947C
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

		// Token: 0x060000F8 RID: 248 RVA: 0x0000B2B8 File Offset: 0x000094B8
		private void initial_fan_page()
		{
			this.FanControl_Page.Support_OC_Style();
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

		// Token: 0x060000F9 RID: 249 RVA: 0x0000B580 File Offset: 0x00009780
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

        private void OpenFanCurveEditor_Click(object sender, RoutedEventArgs e)
        {
            var editor = new FanCurveEditor();
            editor.Owner = this;
            editor.ShowDialog();
        }

        // Token: 0x060000FA RID: 250 RVA: 0x0000B6F4 File Offset: 0x000098F4
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

        // Token: 0x060000FB RID: 251 RVA: 0x0000B788 File Offset: 0x00009988
        private void load_advance_settings()
		{
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

		// Token: 0x060000FC RID: 252 RVA: 0x0000B800 File Offset: 0x00009A00
		public void setting_Popup_Opened(object sender, EventArgs e)
		{
			this.Mask_Rectangle.Visibility = Visibility.Visible;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000B80E File Offset: 0x00009A0E
		private void remove_textbox_focus()
		{
			this.Main_Grid.Focusable = true;
			this.Main_Grid.Focus();
			this.Main_Grid.Focusable = false;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000B834 File Offset: 0x00009A34
		private void mainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.MainTitle_Rectangle.Focusable = true;
			this.MainTitle_Rectangle.Focus();
			this.MainTitle_Rectangle.Focusable = false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000B85C File Offset: 0x00009A5C
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

		// Token: 0x06000100 RID: 256 RVA: 0x0000B8C4 File Offset: 0x00009AC4
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

		// Token: 0x06000101 RID: 257 RVA: 0x0000B92C File Offset: 0x00009B2C
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

		// Token: 0x06000102 RID: 258 RVA: 0x0000B95C File Offset: 0x00009B5C
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

		// Token: 0x06000103 RID: 259 RVA: 0x0000B9C1 File Offset: 0x00009BC1
		private void winmenukey_Checkbox_Checked(object sender, RoutedEventArgs e)
		{
			CommonFunction.set_win_menu_key_status(true);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "WinAndMenuKey", 1U);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000B9D9 File Offset: 0x00009BD9
		private void winmenukey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
		{
			CommonFunction.set_win_menu_key_status(false);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "WinAndMenuKey", 0U);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000B9F4 File Offset: 0x00009BF4
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

		// Token: 0x06000106 RID: 262 RVA: 0x0000BA91 File Offset: 0x00009C91
		private void mainWindow_Closing(object sender, CancelEventArgs e)
		{
			this.PollingStop();
			Environment.Exit(0);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000BA9F File Offset: 0x00009C9F
		private void gfe_Button_MouseEnter(object sender, MouseEventArgs e)
		{
			this.gfe_Button.Opacity = 0.9;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		private void gfe_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			this.gfe_Button.Opacity = 0.6;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000BACB File Offset: 0x00009CCB
		private void gfe_Button_Initialized(object sender, EventArgs e)
		{
			this.gfe_Button.Opacity = 0.6;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		private void gfe_Button_Click(object sender, RoutedEventArgs e)
		{
			this.gfe_Button.Opacity = 1.0;
			try
			{
				if (File.Exists(this.NVIDIA_Experience_file64))
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

		// Token: 0x040000F0 RID: 240
		private const string LOG_REGISTRY_PATH = "SOFTWARE\\OEM\\PredatorSense\\Log";

		// Token: 0x040000F1 RID: 241
		private Log _log;

		// Token: 0x040000F2 RID: 242
		private string[] basic_color_table = new string[] { "#FF0000", "#FFA000", "#FFFF00", "#64FF00", "#00FFFF", "#00B4FF", "#304FFE", "#FF3CFF", "#AA00FF", "#FFFFFF" };

		// Token: 0x040000F3 RID: 243
		private string[] g_recent_color = new string[] { "", "", "", "", "", "", "", "", "", "" };

		// Token: 0x040000F4 RID: 244
		private string[] g_lighting_recent_color = new string[] { "", "", "", "", "", "", "", "", "", "" };

		// Token: 0x040000F5 RID: 245
		public bool initial_flag = true;

		// Token: 0x040000F6 RID: 246
		public bool dont_do_oc_flag;

		// Token: 0x040000F7 RID: 247
		private Task temperature_and_usage_pollingTask;

		// Token: 0x040000F8 RID: 248
		private Task fan_speed_pollingTask;

		// Token: 0x040000F9 RID: 249
		private CancellationTokenSource tokenSource;

		// Token: 0x040000FA RID: 250
		private PerformanceCounter cpuCounter;

		// Token: 0x040000FB RID: 251
		public FanControlPage FanControl_Page;

		// Token: 0x040000FC RID: 252
		private OC_MonitoringPage Monitoring_Page;

		// Token: 0x040000FD RID: 253
		public CommonFunction.AC_Mode_Type current_ac_mode;

		// Token: 0x040000FE RID: 254
		public bool g_battery_boost;

		// Token: 0x040000FF RID: 255
		public string NVIDIA_Experience_filex86 = "C:\\Program Files (x86)\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe";

		// Token: 0x04000100 RID: 256
		public string NVIDIA_Experience_file64 = "C:\\Program Files\\NVIDIA Corporation\\NVIDIA GeForce Experience\\NVIDIA GeForce Experience.exe";
	}
}
