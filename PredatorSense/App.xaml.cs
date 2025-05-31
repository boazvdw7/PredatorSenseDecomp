using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x0200003E RID: 62
	public partial class App : Application
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x0001F750 File Offset: 0x0001D950
		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				base.OnStartup(e);
				int num = RenderCapability.Tier >> 16;
				if (num >= 2)
				{
					RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
				}
				Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
				LoadingPage loadingPage = new LoadingPage();
				loadingPage.ShowDialog();
				if (loadingPage.loadingQuit || !loadingPage.complete)
				{
					base.Shutdown();
				}
				OC_MainWindow oc_MainWindow = new OC_MainWindow(loadingPage._ac_mode, loadingPage._battery_boost);
				oc_MainWindow.Show();
			}
			catch (Exception)
			{
			}
			Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
		[STAThread]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public static void Main()
		{
			App app = new App();
			app.Run();
		}
	}
}
