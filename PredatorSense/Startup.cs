using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace PredatorSense
{
	// Token: 0x0200003F RID: 63
	public class Startup
	{
		// Token: 0x060002A6 RID: 678
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool GetTokenInformation(IntPtr TokenHandle, Startup.TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

		// Token: 0x060002A7 RID: 679
		[DllImport("advapi32.dll", EntryPoint = "ConvertSidToStringSid", SetLastError = true)]
		private static extern bool externConvertSidToStringSid(IntPtr lpSid, ref IntPtr lpStringSid);

		// Token: 0x060002A8 RID: 680
		[DllImport("User32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x060002A9 RID: 681
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x060002AA RID: 682
		[DllImport("shell32.dll")]
		private static extern int ShellExecute(IntPtr hwnd, string lpszOp, string lpszFile, string lpszParams, string lpszDir, int FsShowCmd);

		// Token: 0x060002AB RID: 683
		[DllImport("gdi32.dll")]
		internal static extern int GetDeviceCaps(IntPtr hdc, int Index);

		// Token: 0x060002AC RID: 684
		[DllImport("user32.dll")]
		internal static extern IntPtr GetDC(IntPtr Hwnd);

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0001F802 File Offset: 0x0001DA02
		internal static double GetDpiX
		{
			get
			{
				return (double)Startup.GetDeviceCaps(Startup.GetDC(IntPtr.Zero), 88);
			}
		}

		// Token: 0x060002AE RID: 686
		[DllImport("User32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

		// Token: 0x060002AF RID: 687
		[DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x060002B0 RID: 688 RVA: 0x0001F818 File Offset: 0x0001DA18
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				string empty = string.Empty;
				string text = "en";
				try
				{
					Startup.EnsureProcessesAlive();
					Startup.langRd = Application.LoadComponent(new Uri("/PredatorSense;component/Lang/" + CultureInfo.CurrentUICulture.Parent.Name + ".xaml", UriKind.Relative)) as ResourceDictionary;
					text = CultureInfo.CurrentUICulture.Parent.Name;
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OEM\\PredatorSense", false);
					if (registryKey != null)
					{
						object value = registryKey.GetValue("Lang");
						if (value != null)
						{
							text = value.ToString();
							string text2 = "/PredatorSense;component/Lang/" + text + ".xaml";
							Startup.langRd = Application.LoadComponent(new Uri(text2, UriKind.Relative)) as ResourceDictionary;
						}
						registryKey.Close();
					}
				}
				catch
				{
					Startup.langRd = Application.LoadComponent(new Uri("/PredatorSense;component/Lang/en.xaml", UriKind.Relative)) as ResourceDictionary;
					text = "en";
				}
				if (text.Equals("en") || text.Equals("de") || text.Equals("it") || text.Equals("fr") || text.Equals("es") || text.Equals("nl") || text.Equals("nb") || text.Equals("fi") || text.Equals("pl") || text.Equals("ru") || text.Equals("cs") || text.Equals("hu") || text.Equals("tr"))
				{
					Startup._TTFont = true;
				}
				string text3 = "100";
				double getDpiX = Startup.GetDpiX;
				if (getDpiX != 96.0)
				{
					if (getDpiX >= 120.0 && getDpiX < 144.0)
					{
						text3 = "125";
					}
					else if (getDpiX >= 144.0 && getDpiX < 192.0)
					{
						text3 = "150";
					}
					else if (getDpiX >= 192.0)
					{
						text3 = "200";
					}
				}
				Startup._Image_path = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + text3 + "\\";
				Startup.styled = Application.LoadComponent(new Uri("/PredatorSense;component/Style/" + text3 + "/PSStyle.xaml", UriKind.Relative)) as ResourceDictionary;
				if (Startup.IsGuest() || Startup.IsDomainGuest)
				{
					string text4 = "PredatorSense";
					try
					{
						RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OEM\\PredatorSense", false);
						if (registryKey2 != null)
						{
							object value2 = registryKey2.GetValue("Product_Name");
							if (value2 != null)
							{
								text4 = value2.ToString();
							}
							registryKey2.Close();
						}
					}
					catch
					{
					}
					MessageBox.Show(Startup.langRd["MUI_MSG_Guest"].ToString().Replace("%ProductName%", text4), text4, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				else
				{
					bool flag = false;
					object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
					string value3 = ((GuidAttribute)customAttributes.First<object>()).Value;
					Startup.s_Mutex = new Mutex(true, value3, out flag);
					if (!Startup.s_Mutex.WaitOne(0, false))
					{
						IntPtr intPtr = IntPtr.Zero;
						intPtr = Startup.FindWindow(null, "PredatorSense ");
						if (intPtr != IntPtr.Zero)
						{
							Startup.SetForegroundWindow(intPtr);
							Startup.ShowWindowAsync(intPtr, 9);
						}
					}
					else
					{
						new App
						{
							Resources = Startup.langRd
						}.Run();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0001FBD8 File Offset: 0x0001DDD8
		public static bool IsDomainGuest
		{
			get
			{
				AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
				using (WindowsIdentity current = WindowsIdentity.GetCurrent())
				{
					uint num = 0U;
					bool flag = Startup.GetTokenInformation(current.Token, Startup.TOKEN_INFORMATION_CLASS.TokenGroups, IntPtr.Zero, num, out num);
					IntPtr intPtr = Marshal.AllocHGlobal((int)num);
					flag = Startup.GetTokenInformation(current.Token, Startup.TOKEN_INFORMATION_CLASS.TokenGroups, intPtr, num, out num);
					if (flag)
					{
						uint num2 = (uint)Marshal.ReadInt32(intPtr);
						IntPtr intPtr2 = (IntPtr)(intPtr.ToInt64() + Marshal.OffsetOf(typeof(Startup.TOKEN_GROUPS), "Groups").ToInt64());
						for (uint num3 = 0U; num3 < num2; num3 += 1U)
						{
                            Startup.SID_AND_ATTRIBUTES sidAndAttributes = (Startup.SID_AND_ATTRIBUTES)Marshal.PtrToStructure(intPtr2, typeof(Startup.SID_AND_ATTRIBUTES));
                            string text = Startup.ConvertSidToStringSid(ref sidAndAttributes.Sid);
                            if (text.EndsWith("-514") && text.StartsWith("S-1-5-21"))
							{
								return true;
							}
							intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)Marshal.SizeOf(typeof(Startup.SID_AND_ATTRIBUTES)));
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001FCF8 File Offset: 0x0001DEF8
		private static string ConvertSidToStringSid(ref IntPtr Sid)
		{
			string text = "";
			IntPtr zero = IntPtr.Zero;
			bool flag = Startup.externConvertSidToStringSid(Sid, ref zero);
			if (flag)
			{
				text = Marshal.PtrToStringAnsi(zero);
			}
			Marshal.FreeHGlobal(zero);
			return text;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001FD30 File Offset: 0x0001DF30
		public static bool IsGuest()
		{
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
				if (windowsPrincipal.IsInRole(WindowsBuiltInRole.Guest))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001FD7C File Offset: 0x0001DF7C
		private static void EnsureProcessesAlive()
		{
			string text = "-noui";
			int num = 3;
			if (Process.GetProcessesByName("PSSvc").Length != 0)
			{
				text += " -nopssvc";
				num--;
			}
			if (Startup.FindWindow("PSADMINAGENT", "PSAdminAgent") != IntPtr.Zero)
			{
				text += " -nopsadminagent";
				num--;
			}
			if (Startup.FindWindow("PSAGENT", "PSAgent") != IntPtr.Zero)
			{
				text += " -nopsagent";
				num--;
			}
			if (num > 0)
			{
				Startup.ShellExecute(IntPtr.Zero, null, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\PSLauncher.exe", text, null, 0);
			}
		}

		// Token: 0x04000359 RID: 857
		private const int LOGPIXELSX = 88;

		// Token: 0x0400035A RID: 858
		private const int ANYSIZE_ARRAY = 100;

		// Token: 0x0400035B RID: 859
		private const int SW_RESTORE = 9;

		// Token: 0x0400035C RID: 860
		public static ResourceDictionary langRd = null;

		// Token: 0x0400035D RID: 861
		public static ResourceDictionary styled = null;

		// Token: 0x0400035E RID: 862
		public static bool _TTFont = false;

		// Token: 0x0400035F RID: 863
		private static Mutex s_Mutex;

		// Token: 0x04000360 RID: 864
		public static string _Image_path;

		// Token: 0x04000361 RID: 865
		public static bool _support_OC = false;

		// Token: 0x02000040 RID: 64
		private enum TOKEN_INFORMATION_CLASS
		{
			// Token: 0x04000363 RID: 867
			TokenUser = 1,
			// Token: 0x04000364 RID: 868
			TokenGroups,
			// Token: 0x04000365 RID: 869
			TokenPrivileges,
			// Token: 0x04000366 RID: 870
			TokenOwner,
			// Token: 0x04000367 RID: 871
			TokenPrimaryGroup,
			// Token: 0x04000368 RID: 872
			TokenDefaultDacl,
			// Token: 0x04000369 RID: 873
			TokenSource,
			// Token: 0x0400036A RID: 874
			TokenType,
			// Token: 0x0400036B RID: 875
			TokenImpersonationLevel,
			// Token: 0x0400036C RID: 876
			TokenStatistics,
			// Token: 0x0400036D RID: 877
			TokenRestrictedSids,
			// Token: 0x0400036E RID: 878
			TokenSessionId,
			// Token: 0x0400036F RID: 879
			TokenGroupsAndPrivileges,
			// Token: 0x04000370 RID: 880
			TokenSessionReference,
			// Token: 0x04000371 RID: 881
			TokenSandBoxInert,
			// Token: 0x04000372 RID: 882
			TokenAuditPolicy,
			// Token: 0x04000373 RID: 883
			TokenOrigin
		}

		// Token: 0x02000041 RID: 65
		private struct TOKEN_GROUPS
		{
			// Token: 0x04000374 RID: 884
			[MarshalAs(UnmanagedType.U4)]
			public uint GroupCount;

			// Token: 0x04000375 RID: 885
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
			public Startup.SID_AND_ATTRIBUTES[] Groups;
		}

		// Token: 0x02000042 RID: 66
		private struct SID_AND_ATTRIBUTES
		{
			// Token: 0x04000376 RID: 886
			public IntPtr Sid;

			// Token: 0x04000377 RID: 887
			public int Attributes;
		}
	}
}
