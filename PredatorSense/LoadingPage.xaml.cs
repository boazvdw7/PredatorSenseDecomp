using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000007 RID: 7
	public partial class LoadingPage : Window
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00005FD8 File Offset: 0x000041D8
		public LoadingPage()
		{
			this.InitializeComponent();
			base.Resources = Startup.styled;
			this.AdjustWindowSizeAndPos(false);
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense", "Model_Name_1st"))
			{
				Registry.GetStringLM("SOFTWARE\\OEM\\PredatorSense", "Model_Name_1st");
			}
			Startup._support_OC = true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006034 File Offset: 0x00004234
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.SvcCheckTimer = new DispatcherTimer();
			this.SvcCheckTimer.Tick += this.SvcCheckTimer_Tick;
			this.SvcCheckTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
			this.SvcCheckTimer.Start();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006084 File Offset: 0x00004284
		private void Window_StateChanged(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Minimized) ? WindowState.Minimized : WindowState.Normal);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000609C File Offset: 0x0000429C
		private void min_Close_Button_Click(object sender, RoutedEventArgs e)
		{
			this.loadingQuit = true;
			try
			{
				Application.Current.Shutdown();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000060D0 File Offset: 0x000042D0
		private void min_Minimize_Button_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000060DC File Offset: 0x000042DC
		private void SvcCheckTimer_Tick(object sender, EventArgs e)
		{
			this.SvcCheckTimer.Stop();
			try
			{
				int num = this._count % 75 + 1;
				CommonFunction.UpdateImage(this.splash_Image, "splash.png");
			}
			catch (Exception)
			{
			}
			this._count++;
			if (this._count > 20 && !this._svc_exist)
			{
				ServiceController serviceController = new ServiceController();
				try
				{
					serviceController.ServiceName = "PSSvc";
					if (serviceController.Status == ServiceControllerStatus.Running)
					{
						this._svc_exist = true;
					}
				}
				catch (Exception)
				{
				}
			}
			if (this._svc_exist && !this._ac_mode_check && !this.ac_mode_check_lock)
			{
				this.check_ac_mode();
			}
			if (this._svc_exist && !this._battery_boost_check && !this.battery_boost_lock)
			{
				this.check_battery_boost();
			}
			if (!this.loadingQuit)
			{
				if (this._count % 75 + 1 == 75 && this._svc_exist && this._ac_mode_check && this._battery_boost_check)
				{
					this.complete = true;
					base.Close();
					return;
				}
				if (this._count % 75 + 1 == 75 && this._count == 224 && this._svc_exist)
				{
					this.complete = true;
					CommonFunction.complete_loading = true;
					base.Close();
					return;
				}
				this.SvcCheckTimer.Start();
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006270 File Offset: 0x00004470
		private void check_ac_mode()
		{
			if (Startup._support_OC)
			{
				ThreadStart threadStart = delegate
				{
					this.ac_mode_check_lock = true;
					this._ac_mode_check = CommonFunction.get_current_ac_mode(ref this._ac_mode);
					this.ac_mode_check_lock = false;
				};
				new Thread(threadStart).Start();
				return;
			}
			this._ac_mode_check = true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000062CC File Offset: 0x000044CC
		private void check_battery_boost()
		{
			if (Startup._support_OC)
			{
				ThreadStart threadStart = delegate
				{
					this.battery_boost_lock = true;
					this._battery_boost_check = CommonFunction.get_wmi_battery_boost_status(ref this._battery_boost);
					this.battery_boost_lock = false;
				};
				new Thread(threadStart).Start();
				return;
			}
			this._battery_boost_check = true;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00006308 File Offset: 0x00004508
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

		// Token: 0x04000071 RID: 113
		private DispatcherTimer SvcCheckTimer;

		// Token: 0x04000072 RID: 114
		public bool loadingQuit;

		// Token: 0x04000073 RID: 115
		public bool complete;

		// Token: 0x04000074 RID: 116
		private int _count = 1;

		// Token: 0x04000075 RID: 117
		public CommonFunction.AC_Mode_Type _ac_mode;

		// Token: 0x04000076 RID: 118
		public bool _ac_mode_check;

		// Token: 0x04000077 RID: 119
		public bool _battery_boost_check;

		// Token: 0x04000078 RID: 120
		public bool ac_mode_check_lock;

		// Token: 0x04000079 RID: 121
		public bool battery_boost_lock;

		// Token: 0x0400007A RID: 122
		private bool _svc_exist;

		// Token: 0x0400007B RID: 123
		public bool _battery_boost;
	}
}
