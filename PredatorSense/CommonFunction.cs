using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000044 RID: 68
	public class CommonFunction
	{
		// Token: 0x060002C0 RID: 704
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

		// Token: 0x060002C1 RID: 705
		[DllImport("user32.dll")]
		private static extern bool SystemParametersInfo(uint action, uint param, ref CommonFunction.SKEY vparam, uint init);

		// Token: 0x060002C2 RID: 706
		[DllImport("kernel32")]
		private static extern void GetSystemPowerStatus(ref CommonFunction.SYSTEM_POWER_STATUS lpSystemPowerStatus);

		// Token: 0x060002C3 RID: 707 RVA: 0x000200EC File Offset: 0x0001E2EC
		public static void exception_move_folder(string source, string dest, string new_name)
		{
			if (Directory.Exists(CommonFunction._temp_dir_path))
			{
				Directory.Delete(CommonFunction._temp_dir_path, true);
			}
			Directory.CreateDirectory(CommonFunction._temp_dir_path + "\\" + new_name);
			CommonFunction.DirectoryCopy(source, CommonFunction._temp_dir_path + "\\" + new_name, true);
			Task<int> task = CommonFunction.delete_file(source);
			int result = task.Result;
			Directory.Move(CommonFunction._temp_dir_path + "\\" + new_name, dest);
			Directory.Delete(CommonFunction._temp_dir_path, true);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0002016C File Offset: 0x0001E36C
		public static void exception_move_file(string source, string dest, string new_name)
		{
			if (Directory.Exists(CommonFunction._temp_dir_path))
			{
				Directory.Delete(CommonFunction._temp_dir_path, true);
			}
			Directory.CreateDirectory(CommonFunction._temp_dir_path);
			File.Copy(source, CommonFunction._temp_dir_path + "\\" + new_name, true);
			Task<int> task = CommonFunction.delete_file(source);
			int result = task.Result;
			Directory.Move(CommonFunction._temp_dir_path + "\\" + new_name, dest);
			Directory.Delete(CommonFunction._temp_dir_path, true);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000201E4 File Offset: 0x0001E3E4
		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				string text = Path.Combine(destDirName, fileInfo.Name);
				fileInfo.CopyTo(text, false);
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string text2 = Path.Combine(destDirName, directoryInfo2.Name);
					CommonFunction.DirectoryCopy(directoryInfo2.FullName, text2, copySubDirs);
				}
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00020290 File Offset: 0x0001E490
		public static bool UpdateImage(Image sender, string name)
		{
			bool flag;
			try
			{
				if (!File.Exists(Startup._Image_path + name))
				{
					flag = false;
				}
				else
				{
					Uri uri = new Uri(Startup._Image_path + name, UriKind.Absolute);
					sender.Source = new BitmapImage(uri);
					flag = true;
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000202EC File Offset: 0x0001E4EC
		public static int get_battery_life_percent()
		{
			CommonFunction.SYSTEM_POWER_STATUS system_POWER_STATUS = default(CommonFunction.SYSTEM_POWER_STATUS);
			CommonFunction.GetSystemPowerStatus(ref system_POWER_STATUS);
			return (int)system_POWER_STATUS.BatteryLifePercent;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00020544 File Offset: 0x0001E744
		public static async Task<int> delete_file(string file_path)
		{
			int output = 0;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 3, new object[] { file_path });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000207C0 File Offset: 0x0001E9C0
		public static async Task<int> delete_folder(string file_path)
		{
			int output = 0;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 4, new object[] { file_path });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00020808 File Offset: 0x0001EA08
		public static Color ColorFromString(string htmlColor)
		{
			Color color;
			try
			{
				htmlColor = htmlColor.Replace("#", "");
				byte b = byte.MaxValue;
				byte b2 = 0;
				byte b3 = 0;
				byte b4 = 0;
				switch (htmlColor.Length)
				{
				case 3:
					b2 = byte.Parse(htmlColor.Substring(0, 1), NumberStyles.HexNumber);
					b3 = byte.Parse(htmlColor.Substring(1, 1), NumberStyles.HexNumber);
					b4 = byte.Parse(htmlColor.Substring(2, 1), NumberStyles.HexNumber);
					break;
				case 4:
					b = byte.Parse(htmlColor.Substring(0, 1), NumberStyles.HexNumber);
					b2 = byte.Parse(htmlColor.Substring(1, 1), NumberStyles.HexNumber);
					b3 = byte.Parse(htmlColor.Substring(2, 1), NumberStyles.HexNumber);
					b4 = byte.Parse(htmlColor.Substring(3, 1), NumberStyles.HexNumber);
					break;
				case 6:
					b2 = byte.Parse(htmlColor.Substring(0, 2), NumberStyles.HexNumber);
					b3 = byte.Parse(htmlColor.Substring(2, 2), NumberStyles.HexNumber);
					b4 = byte.Parse(htmlColor.Substring(4, 2), NumberStyles.HexNumber);
					break;
				case 8:
					b = byte.Parse(htmlColor.Substring(0, 2), NumberStyles.HexNumber);
					b2 = byte.Parse(htmlColor.Substring(2, 2), NumberStyles.HexNumber);
					b3 = byte.Parse(htmlColor.Substring(4, 2), NumberStyles.HexNumber);
					b4 = byte.Parse(htmlColor.Substring(6, 2), NumberStyles.HexNumber);
					break;
				}
				color = Color.FromArgb(b, b2, b3, b4);
			}
			catch (Exception)
			{
				color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			}
			return color;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000209B4 File Offset: 0x0001EBB4
		public static string StringFromColor(Color bg_color)
		{
			string text2;
			try
			{
				string text = string.Empty;
				text = string.Format("#{0:X2}{1:X2}{2:X2}", bg_color.R, bg_color.G, bg_color.B);
				text2 = text;
			}
			catch (Exception)
			{
				text2 = "#FFFF00";
			}
			return text2;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00020A14 File Offset: 0x0001EC14
		public static void Get_type_and_option_name(int type_index, int option_index, ref string type_name, ref string option_name)
		{
			if (type_index == 0)
			{
				type_name = Startup.langRd["MUI_CPU_OC_Switch"].ToString();
				if (option_index == 0)
				{
					option_name = string.Concat(new string[]
					{
						Startup.langRd["MUI_Normal"].ToString(),
						" > ",
						Startup.langRd["MUI_Faster"].ToString(),
						" > ",
						Startup.langRd["MUI_Turbo"].ToString()
					});
					return;
				}
				if (1 == option_index)
				{
					option_name = Startup.langRd["MUI_Normal"].ToString() + " <> " + Startup.langRd["MUI_Faster"].ToString();
					return;
				}
				if (2 == option_index)
				{
					option_name = Startup.langRd["MUI_Faster"].ToString() + " <> " + Startup.langRd["MUI_Turbo"].ToString();
					return;
				}
				if (3 == option_index)
				{
					option_name = Startup.langRd["MUI_Normal"].ToString() + " <> " + Startup.langRd["MUI_Turbo"].ToString();
					return;
				}
			}
			else if (1 == type_index)
			{
				type_name = Startup.langRd["MUI_GPU_OC_Switch"].ToString();
				if (option_index == 0)
				{
					option_name = string.Concat(new string[]
					{
						Startup.langRd["MUI_Normal"].ToString(),
						" > ",
						Startup.langRd["MUI_Faster"].ToString(),
						" > ",
						Startup.langRd["MUI_Turbo"].ToString()
					});
					return;
				}
				if (1 == option_index)
				{
					option_name = Startup.langRd["MUI_Normal"].ToString() + " <> " + Startup.langRd["MUI_Faster"].ToString();
					return;
				}
				if (2 == option_index)
				{
					option_name = Startup.langRd["MUI_Faster"].ToString() + " <> " + Startup.langRd["MUI_Turbo"].ToString();
					return;
				}
				if (3 == option_index)
				{
					option_name = Startup.langRd["MUI_Normal"].ToString() + " <> " + Startup.langRd["MUI_Turbo"].ToString();
					return;
				}
			}
			else if (2 == type_index)
			{
				type_name = Startup.langRd["MUI_Fan_Switch"].ToString();
				if (option_index == 0)
				{
					option_name = string.Concat(new string[]
					{
						Startup.langRd["MUI_Auto"].ToString(),
						" > ",
						Startup.langRd["MUI_Max"].ToString(),
						" > ",
						Startup.langRd["MUI_Custom"].ToString()
					});
					return;
				}
				if (1 == option_index)
				{
					option_name = Startup.langRd["MUI_Auto"].ToString() + " <> " + Startup.langRd["MUI_Max"].ToString();
					return;
				}
				if (2 == option_index)
				{
					option_name = Startup.langRd["MUI_Max"].ToString() + " <> " + Startup.langRd["MUI_Custom"].ToString();
					return;
				}
				if (3 == option_index)
				{
					option_name = Startup.langRd["MUI_Auto"].ToString() + " <> " + Startup.langRd["MUI_Custom"].ToString();
					return;
				}
			}
			else
			{
				if (3 == type_index)
				{
					type_name = Startup.langRd["MUI_Launch_App"].ToString();
					return;
				}
				if (4 == type_index)
				{
					type_name = Startup.langRd["MUI_ED_Advance_Setting"].ToString();
					if (option_index == 0)
					{
						option_name = Startup.langRd["MUI_Sticky_Key"].ToString();
						return;
					}
					if (1 == option_index)
					{
						option_name = Startup.langRd["MUI_Win_And_Menu_Key"].ToString();
						return;
					}
					if (2 == option_index)
					{
						option_name = Startup.langRd["MUI_LCD_Overdrive"].ToString();
						return;
					}
				}
				else
				{
					if (5 == type_index)
					{
						type_name = Startup.langRd["MUI_Macro"].ToString();
						return;
					}
					if (6 == type_index)
					{
						type_name = "(" + Startup.langRd["MUI_None"].ToString() + ")";
					}
				}
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00020E84 File Offset: 0x0001F084
		public static long GetDirectorySize(string dir)
		{
			long num2;
			try
			{
				string[] files = Directory.GetFiles(dir);
				string[] directories = Directory.GetDirectories(dir);
				long num = 0L;
				foreach (string text in files)
				{
					FileInfo fileInfo = new FileInfo(text);
					num += fileInfo.Length;
				}
				foreach (string text2 in directories)
				{
					num += CommonFunction.GetDirectorySize(text2);
				}
				num2 = num;
			}
			catch (Exception)
			{
				num2 = 0L;
			}
			return num2;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00020F18 File Offset: 0x0001F118
		public static double Get_Free_Size(string drive)
		{
			double num5;
			try
			{
				double num = 0.0;
				ulong num2;
				ulong num3;
				ulong num4;
				if (!CommonFunction.GetDiskFreeSpaceEx(drive, out num2, out num3, out num4))
				{
					num5 = num;
				}
				else
				{
					double num6 = (double)num4 / 1024.0;
					num = num6 / 1024.0;
					num5 = num;
				}
			}
			catch (Exception)
			{
				num5 = 0.0;
			}
			return num5;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00020F88 File Offset: 0x0001F188
		public static string Get_Group_Head_Name(int group_index)
		{
			string text = "";
			if (group_index == 0)
			{
				text = "r";
			}
			else if (group_index == 1)
			{
				text = "b";
			}
			else if (group_index == 2)
			{
				text = "g";
			}
			return text;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00020FF0 File Offset: 0x0001F1F0
		public static bool get_current_group_index(ref int group_index)
		{
			uint input = 0U;
			Task<ulong> task = Task<ulong>.Factory.StartNew(() => WMIFunction.GetAcerGamingPorfileConfiguration(input).GetAwaiter().GetResult());
			ulong result = task.Result;
			if ((result & 255UL) == 0UL)
			{
				group_index = (int)((result >> 32) & 255UL);
				if (group_index == 0)
				{
					group_index = 1;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0002104C File Offset: 0x0001F24C
		public static void set_current_group_index(int group_index)
		{
			ulong num = (ulong)(((long)group_index << 32) | 4L);
			WMIFunction.SetAcerGamingPorfileConfiguration(num).GetAwaiter();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00021070 File Offset: 0x0001F270
		public static void set_win_menu_key_status(bool status)
		{
			ulong num = ((status ? 1UL : 0UL) << 24) | 2UL;
			WMIFunction.SetAcerGamingPorfileConfiguration(num).GetAwaiter();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0002109C File Offset: 0x0001F29C
		public static void set_lcd_over_drive(bool state)
		{
			ulong num = 16UL;
			num |= (state ? 281474976710656UL : 0UL);
			WMIFunction.SetAcerGamingPorfileConfiguration(num).GetAwaiter();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000210CC File Offset: 0x0001F2CC
		public static void set_group_wmi_color(int group_index, Color color)
		{
			ulong num = Convert.ToUInt64(color.R);
			num = Convert.ToUInt64(Math.Ceiling(num * 0.5));
			ulong num2 = Convert.ToUInt64(color.G);
			ulong num3 = Convert.ToUInt64(color.B);
			ulong num4 = (ulong)((long)(255 & group_index));
			num4 |= (num << 8) | (num2 << 16) | (num3 << 24);
			WMIFunction.SetAcerGamingLEDGroupColor(num4).GetAwaiter();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00021140 File Offset: 0x0001F340
		public static bool get_wmi_system_health_info(ref int info_data, CommonFunction.System_Health_Information_Index index)
		{
            uint num = ((uint)CommonFunction.System_Health_Information_Index.sCPU_Temperature) | ((uint)index << 8); 
			Task<ulong> acerGamingSystemInformation = WMIFunction.GetAcerGamingSystemInformation(num);
			ulong result = acerGamingSystemInformation.Result;
			if ((result & 255UL) == 0UL)
			{
				info_data = (int)((result >> 8) & 65535UL);
				return true;
			}
			return false;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00021180 File Offset: 0x0001F380
		public static bool get_wmi_battery_boost_status(ref bool status)
		{
			status = false;
			uint num = 2U;
			Task<ulong> acerGamingSystemInformation = WMIFunction.GetAcerGamingSystemInformation(num);
			ulong result = acerGamingSystemInformation.Result;
			if ((result & 255UL) == 0UL)
			{
				if (((result >> 40) & 255UL) == 1UL)
				{
					status = true;
				}
				else
				{
					status = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000213D0 File Offset: 0x0001F5D0
		public static async Task<List<ulong>> get_gpu_usage_loading()
		{
			List<ulong> output = new List<ulong>();
			List<ulong> list;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<List<ulong>> tsk = Task.Run<List<ulong>>(delegate
				{
					List<ulong> list2 = new List<ulong>();
					IPCMethods.SendCommandByNamedPipe(cline_stream, 14, new object[0]);
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[25];
					cline_stream.Read(array, 0, array.Length);
					list2.Add(BitConverter.ToUInt64(array, 5));
					list2.Add(BitConverter.ToUInt64(array, 17));
					return list2;
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				list = output;
			}
			catch (Exception)
			{
				list = output;
			}
			return list;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00021690 File Offset: 0x0001F890
		public static async Task<List<int>> get_gpu_frequency(CommonFunction.Frequency_Mode frequency_mode)
		{
			List<int> output = new List<int>();
			List<int> list;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<List<int>> tsk = Task.Run<List<int>>(delegate
				{
					List<int> list2 = new List<int>();
					if (frequency_mode == CommonFunction.Frequency_Mode.fMax)
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 21, new object[] { 2 });
					}
					else if (frequency_mode == CommonFunction.Frequency_Mode.fNormal)
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 21, new object[] { 1 });
					}
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[17];
					cline_stream.Read(array, 0, array.Length);
					list2.Add(BitConverter.ToInt32(array, 5));
					list2.Add(BitConverter.ToInt32(array, 13));
					return list2;
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				list = output;
			}
			catch (Exception)
			{
				list = output;
			}
			return list;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0002193C File Offset: 0x0001FB3C
		public static async Task<int> get_cpu_frequency(CommonFunction.Frequency_Mode frequency_mode)
		{
			int output = -1;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					if (frequency_mode == CommonFunction.Frequency_Mode.fMax)
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 22, new object[] { 3 });
					}
					else if (frequency_mode == CommonFunction.Frequency_Mode.fNormal)
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 22, new object[] { 2 });
					}
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00021BB4 File Offset: 0x0001FDB4
		public static async Task<int> set_cpu_oc_level(CommonFunction.Overclock_Mode_Type level)
		{
			int output = -1;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 24, new object[] { (int)level });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00021E2C File Offset: 0x0002002C
		public static async Task<int> set_gpu_oc_level(CommonFunction.Overclock_Mode_Type level)
		{
			int output = -1;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 23, new object[] { (int)level });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00021E74 File Offset: 0x00020074
		public static int transfer_celsius_to_fahrenheit_name(int celsius)
		{
			float num = (float)celsius * 9f / 5f + 32f;
			return (int)num;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00021E98 File Offset: 0x00020098
		public static int transfer_fahrenheit_to_celsius_name(int fahrenheit)
		{
			float num = (float)((fahrenheit - 32) * 5 / 9);
			return (int)num;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00021EB4 File Offset: 0x000200B4
		public static bool set_all_fan_mode(CommonFunction.Fan_Mode_Type mode_index)
		{
			ulong num = 9UL;
			if (mode_index == CommonFunction.Fan_Mode_Type.Auto)
			{
				num |= 4259840UL;
			}
			else if (mode_index == CommonFunction.Fan_Mode_Type.Max)
			{
				num |= 8519680UL;
			}
			else if (mode_index == CommonFunction.Fan_Mode_Type.Custom)
			{
				num |= 12779520UL;
			}
			uint result = WMIFunction.SetAcerGamingFanGroupBehavior(num).GetAwaiter().GetResult();
			return (result & 255U) == 0U;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00021F10 File Offset: 0x00020110
		public static bool set_coolboost_state(bool state)
		{
			ulong num = (ulong)(7L | ((state ? 1L : 0L) << 16));
			uint result = WMIFunction.WMISetFunction(num).GetAwaiter().GetResult();
			return (result & 255U) == 0U;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00021F50 File Offset: 0x00020150
		public static bool set_auto_coolboost_state(bool state)
		{
			ulong num = 65543UL;
			uint result = WMIFunction.WMISetFunction(num).GetAwaiter().GetResult();
			return (result & 255U) == 0U;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00021F88 File Offset: 0x00020188
		public static bool set_auto_normal_state(bool state)
		{
			ulong num = 7UL;
			uint result = WMIFunction.WMISetFunction(num).GetAwaiter().GetResult();
			return (result & 255U) == 0U;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00021FBC File Offset: 0x000201BC
		public static bool set_auto_silence_state(bool state)
		{
			ulong num = 131079UL;
			uint result = WMIFunction.WMISetFunction(num).GetAwaiter().GetResult();
			return (result & 255U) == 0U;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00021FF4 File Offset: 0x000201F4
		public static bool set_all_custom_fan_state(List<bool> auto, List<ulong> percentage)
		{
			bool flag = true;
			ulong num = (auto[0] ? 1UL : 3UL);
			ulong num2 = (auto[1] ? 1UL : 3UL);
			ulong num3 = 9UL | (num << 16) | (num2 << 22);
			uint num4 = WMIFunction.SetAcerGamingFanGroupBehavior(num3).GetAwaiter().GetResult();
			if ((num4 & 255U) != 0U)
			{
				flag = false;
			}
			for (int i = 0; i < 2; i++)
			{
				if (i == 0)
				{
					num3 = 1UL;
				}
				else if (i == 1)
				{
					num3 = 4UL;
				}
				num3 |= percentage[i] << 8;
				num4 = WMIFunction.SetAcerGamingFanGroupSpeed(num3).GetAwaiter().GetResult();
				if ((num4 & 255U) != 0U)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000220B0 File Offset: 0x000202B0
		public static bool set_single_custom_fan_state(bool auto, ulong percentage, CommonFunction.Fan_Group_Type fan_group_type)
		{
			bool flag = true;
			ulong num = 0UL;
			ulong num2 = 0UL;
			ulong num3 = (auto ? 1UL : 3UL);
			if (fan_group_type == CommonFunction.Fan_Group_Type.fCPU)
			{
				num = 1UL | (num3 << 16);
				num2 = 1UL;
			}
			else if (fan_group_type == CommonFunction.Fan_Group_Type.fGPU)
			{
				num = 8UL | (num3 << 22);
				num2 = 4UL;
			}
			uint num4 = WMIFunction.SetAcerGamingFanGroupBehavior(num).GetAwaiter().GetResult();
			if ((num4 & 255U) != 0U)
			{
				flag = false;
			}
			num2 |= percentage << 8;
			num4 = WMIFunction.SetAcerGamingFanGroupSpeed(num2).GetAwaiter().GetResult();
			if ((num4 & 255U) != 0U)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00022140 File Offset: 0x00020340
		public static bool set_single_custom_fan_speed(ulong percentage, CommonFunction.Fan_Group_Type fan_group_type)
		{
			bool flag = true;
			ulong num = 0UL;
			if (fan_group_type == CommonFunction.Fan_Group_Type.fCPU)
			{
				num = 1UL;
			}
			else if (fan_group_type == CommonFunction.Fan_Group_Type.fGPU)
			{
				num = 4UL;
			}
			num |= percentage << 8;
			WMIFunction.SetAcerGamingFanGroupSpeed(num).GetAwaiter();
			return flag;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00022174 File Offset: 0x00020374
		public static bool get_current_ac_mode(ref CommonFunction.AC_Mode_Type current_ac_mode)
		{
			CommonFunction.SYSTEM_POWER_STATUS system_POWER_STATUS = default(CommonFunction.SYSTEM_POWER_STATUS);
			CommonFunction.GetSystemPowerStatus(ref system_POWER_STATUS);
			if (system_POWER_STATUS.ACLineStatus == 1)
			{
				current_ac_mode = CommonFunction.AC_Mode_Type.a2_AC;
			}
			else
			{
				current_ac_mode = CommonFunction.AC_Mode_Type.aNo_AC;
			}
			return true;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000221A4 File Offset: 0x000203A4
		public static bool set_usb_kb_color(List<Color> colors)
		{
			bool flag = true;
			byte[] array = new byte[512];
			int num = 0;
			foreach (Color color in colors)
			{
				byte r = color.R;
				byte g = color.G;
				byte b = color.B;
				array[num * 4 + 1] = r;
				array[num * 4 + 2] = g;
				array[num * 4 + 3] = b;
				num++;
			}
			uint result = USBFunction.SetUSBKeyboardColor(array).GetAwaiter().GetResult();
			if ((result & 255U) != 0U)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0002225C File Offset: 0x0002045C
		public static bool set_usb_kb_pattern(ulong iEffect, ulong iSpeed, Color cColor, ulong iDirect)
		{
			bool flag = true;
			byte[] array = new byte[] { 0, cColor.R, cColor.G, cColor.B };
			uint num = 0U;
			USBFunction.SetUSBPattern(Convert.ToByte(iEffect), Convert.ToByte(Math.Abs((int)iSpeed - 10)), array, Convert.ToByte(iDirect)).GetAwaiter();
			if ((num & 255U) != 0U)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000222C8 File Offset: 0x000204C8
		public static bool set_usb_kb_off()
		{
			bool flag = true;
			byte[] array = new byte[4];
			uint result = USBFunction.SetUSBPattern(Convert.ToByte(0), Convert.ToByte(0), array, Convert.ToByte(0)).GetAwaiter().GetResult();
			if ((result & 255U) != 0U)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00022312 File Offset: 0x00020512
		public static bool set_usb_kb_on_color(List<Color> colors)
		{
			return false;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00022318 File Offset: 0x00020518
		public static bool set_usb_kb_on_pattern(ulong iEffect, ulong iSpeed, Color cColor, ulong iDirect)
		{
			bool flag = true;
			byte[] array = new byte[] { 0, cColor.R, cColor.G, cColor.B };
			uint num = 0U;
			USBFunction.SetUSBPattern(Convert.ToByte(iEffect), Convert.ToByte(Math.Abs((int)iSpeed - 10)), array, Convert.ToByte(iDirect)).GetAwaiter();
			if ((num & 255U) != 0U)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00022384 File Offset: 0x00020584
		public static int get_usb_pad_direction()
		{
			return (int)USBFunction.GetUSBPad().GetAwaiter().GetResult();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000223A8 File Offset: 0x000205A8
		public static bool set_usb_pad_off()
		{
			USBFunction.SetUSBPad(0UL, null).GetAwaiter().GetResult();
			return false;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000223CC File Offset: 0x000205CC
		public static bool set_usb_pad_on()
		{
			USBFunction.SetUSBPad(1UL, null).GetAwaiter().GetResult();
			return false;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000223F0 File Offset: 0x000205F0
		public static bool set_usb_pad_on_color(Color color)
		{
			USBFunction.SetUSBPad(2UL, new byte[] { 0, color.R, color.G, color.B }).GetAwaiter();
			return false;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00022434 File Offset: 0x00020634
		public static bool set_wmi_Logo_color(int index, Color color)
		{
			uint num = 0U;
			ulong num2 = Convert.ToUInt64(Convert.ToDouble(Convert.ToUInt64(color.R)) * CommonFunction.adjust_red);
			ulong num3 = (ulong)color.G;
			ulong num4 = (ulong)color.B;
			switch (index)
			{
			case 1:
			{
				ulong num5 = 1UL;
				ulong num6 = num5 | (num2 << 8) | (num3 << 16) | (num4 << 24);
				WMIFunction.WMISetLogoLED(num6).GetAwaiter();
				break;
			}
			case 2:
			{
				ulong num5 = 2UL;
				ulong num6 = num5 | (num2 << 8) | (num3 << 16) | (num4 << 24);
				WMIFunction.WMISetLogoLED(num6).GetAwaiter();
				num5 = 3UL;
				num6 = num5 | (num2 << 8) | (num3 << 16) | (num4 << 24);
				WMIFunction.WMISetLogoLED(num6).GetAwaiter();
				break;
			}
			case 3:
			{
				ulong num5 = 5UL;
				ulong num6 = num5 | (num2 << 8) | (num3 << 16) | (num4 << 24);
				WMIFunction.WMISetLogoLED(num6).GetAwaiter();
				num5 = 6UL;
				num6 = num5 | (num2 << 8) | (num3 << 16) | (num4 << 24);
				WMIFunction.WMISetLogoLED(num6).GetAwaiter();
				break;
			}
			}
			return (num & 255U) == 0U;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00022550 File Offset: 0x00020750
		public static bool set_wmi_Logo_Breath_behavior(bool state)
		{
			ulong num = (ulong)(7L | ((state ? 63L : 21L) << 16));
			WMIFunction.WMISetLogoStatus(num).GetAwaiter();
			return false;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00022580 File Offset: 0x00020780
		public static bool set_wmi_Logo_OnOff(int index, bool state)
		{
			ulong num = 0UL;
			if (index == 1)
			{
				num = (ulong)(1L | ((state ? 1L : 0L) << 16));
			}
			else if (index == 2)
			{
				num = (ulong)(6L | ((state ? 20L : 0L) << 16));
			}
			else if (index == 3)
			{
				num = (ulong)(48L | ((state ? 6L : 0L) << 24));
			}
			WMIFunction.WMISetLogoStatus(num).GetAwaiter();
			return false;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000225DC File Offset: 0x000207DC
		public static int get_usb_Keyboard_Type()
		{
			uint num = USBFunction.GetUSBKeyboardType().GetAwaiter().GetResult();
			if (num == 3U)
			{
				num = 0U;
			}
			return (int)num;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000227EC File Offset: 0x000209EC
		public static async Task<int> get_gpu_coproc_status()
		{
			int output = -1;
			int num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<int> tsk = Task.Run<int>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 26, new object[0]);
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToInt32(array, 5);
				});
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = output;
			}
			return num;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0002282C File Offset: 0x00020A2C
		public static bool Get_STICKYK_Status()
		{
			uint num = 1U;
			uint num2 = 4U;
			uint num3 = 58U;
			CommonFunction.SKEY skey = default(CommonFunction.SKEY);
			uint num4 = 8U;
			skey.cbSize = num4;
			CommonFunction.SystemParametersInfo(num3, num4, ref skey, 0U);
			CommonFunction.SKEY skey2 = skey;
			return (skey2.dwFlags & num2) == num2 && (skey2.dwFlags & num) == num;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00022880 File Offset: 0x00020A80
		public static string get_wmi_battery_boost()
		{
			string text = "";
			uint num = 2U;
			Task<ulong> acerGamingSystemInformation = WMIFunction.GetAcerGamingSystemInformation(num);
			ulong result = acerGamingSystemInformation.Result;
			if ((result & 255UL) == 0UL)
			{
				if (((result >> 40) & 255UL) == 1UL)
				{
					text = ">40";
				}
				else
				{
					text = ">40";
				}
				return text;
			}
			return text;
		}

		// Token: 0x04000381 RID: 897
		public static string session_id = Process.GetCurrentProcess().SessionId.ToString();

		// Token: 0x04000382 RID: 898
		public static string fahrenheit_name = Startup.langRd["MUI_Fahrenheit"].ToString();

		// Token: 0x04000383 RID: 899
		public static string celsius_name = Startup.langRd["MUI_Celsius"].ToString();

		// Token: 0x04000384 RID: 900
		public static string select_macro_name = "--" + Startup.langRd["MUI_Select_Macro"].ToString() + "--";

		// Token: 0x04000385 RID: 901
		public static string macro_content_name = Startup.langRd["MUI_Macro"].ToString();

		// Token: 0x04000386 RID: 902
		public static string profile_content_name = Startup.langRd["MUI_Profile"].ToString();

		// Token: 0x04000387 RID: 903
		public static string default_content_name = "Default";

		// Token: 0x04000388 RID: 904
		public static string hotkey_profile_path = "C:\\ProgramData\\oem\\PredatorSense\\ProfilePool\\HotkeyProfilePool\\";

		// Token: 0x04000389 RID: 905
		public static string lighting_profile_path = "C:\\ProgramData\\oem\\PredatorSense\\ProfilePool\\LightProfilePool\\";

		// Token: 0x0400038A RID: 906
		public static string macro_profile_path = "C:\\ProgramData\\oem\\PredatorSense\\ProfilePool\\HotkeyProfilePool\\%HotkeyProfileName%\\MacroKeyPool\\";

		// Token: 0x0400038B RID: 907
		public static string _temp_dir_path = "C:\\ProgramData\\oem\\PredatorSense\\ProfilePool\\exception_temp";

		// Token: 0x0400038C RID: 908
		public static string macro_profile_path_replace_name = "%HotkeyProfileName%";

		// Token: 0x0400038D RID: 909
		public static char[] all_whitespaces = new char[]
		{
			' ', '\u1680', '\u180e', '\u2000', '\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006',
			'\u2007', '\u2008', '\u2009', '\u200a', '\u202f', '\u205f', '\u3000', '\u2028', '\u2029', '\t',
			'\n', '\v', '\f', '\r', '\u0085', '\u00a0', '\u200b', '\ufeff'
		};

		// Token: 0x0400038E RID: 910
		public static double adjust_red = 0.5;

		// Token: 0x0400038F RID: 911
		public static bool complete_loading = false;

		// Token: 0x02000045 RID: 69
		public struct SYSTEM_POWER_STATUS
		{
			// Token: 0x04000390 RID: 912
			public byte ACLineStatus;

			// Token: 0x04000391 RID: 913
			public byte BatteryFlag;

			// Token: 0x04000392 RID: 914
			public byte BatteryLifePercent;

			// Token: 0x04000393 RID: 915
			public byte Reserved1;

			// Token: 0x04000394 RID: 916
			public int BatteryLifeTime;

			// Token: 0x04000395 RID: 917
			public int BatteryFullLifeTime;
		}

		// Token: 0x02000046 RID: 70
		public struct SKEY
		{
			// Token: 0x04000396 RID: 918
			public uint cbSize;

			// Token: 0x04000397 RID: 919
			public uint dwFlags;
		}

		// Token: 0x02000047 RID: 71
		public enum Power_Plan_Mode
		{
			// Token: 0x04000399 RID: 921
			Power_Saver,
			// Token: 0x0400039A RID: 922
			Balance,
			// Token: 0x0400039B RID: 923
			High_Performance
		}

		// Token: 0x02000048 RID: 72
		public enum System_Health_Information_Index
		{
			// Token: 0x0400039D RID: 925
			sCPU_Temperature = 1,
			// Token: 0x0400039E RID: 926
			sCPU_Fan_Speed,
			// Token: 0x0400039F RID: 927
			sSystem_Temperature,
			// Token: 0x040003A0 RID: 928
			sSystem_Fan_Speed,
			// Token: 0x040003A1 RID: 929
			sFrostCore,
			// Token: 0x040003A2 RID: 930
			sGPU_Fan_Speed,
			// Token: 0x040003A3 RID: 931
			sSystem2_Temperature,
			// Token: 0x040003A4 RID: 932
			sSystem2_Fan_Speed,
			// Token: 0x040003A5 RID: 933
			sGPU2_Fan_Speed,
			// Token: 0x040003A6 RID: 934
			sGPU1_Temperature,
			// Token: 0x040003A7 RID: 935
			sGPU2_Temperature
		}

		// Token: 0x02000049 RID: 73
		public enum Color_Button_Type
		{
			// Token: 0x040003A9 RID: 937
			cHotkey_Group1,
			// Token: 0x040003AA RID: 938
			cHotkey_Group2,
			// Token: 0x040003AB RID: 939
			cHotkey_Group3
		}

		// Token: 0x0200004A RID: 74
		public enum Export_Profile_Type
		{
			// Token: 0x040003AD RID: 941
			eSingle,
			// Token: 0x040003AE RID: 942
			aMultiple
		}

		// Token: 0x0200004B RID: 75
		public enum Attention_Type
		{
			// Token: 0x040003B0 RID: 944
			aDelete_Profile,
			// Token: 0x040003B1 RID: 945
			aEnough_Storage,
			// Token: 0x040003B2 RID: 946
			aImport_Failed,
			// Token: 0x040003B3 RID: 947
			aMacro_Record_Max,
			// Token: 0x040003B4 RID: 948
			aProfile_Same_Name,
			// Token: 0x040003B5 RID: 949
			aMacro_Same_Name,
			// Token: 0x040003B6 RID: 950
			aGroup_Same_Name,
			// Token: 0x040003B7 RID: 951
			aDelete_Macro
		}

		// Token: 0x0200004C RID: 76
		public enum Profile_Type
		{
			// Token: 0x040003B9 RID: 953
			pHotkey,
			// Token: 0x040003BA RID: 954
			pLighting,
			// Token: 0x040003BB RID: 955
			pMacro,
			// Token: 0x040003BC RID: 956
			pRename_Macro
		}

		// Token: 0x0200004D RID: 77
		public enum Key_Assignment_Type
		{
			// Token: 0x040003BE RID: 958
			kCPU_Overclocking_Switch,
			// Token: 0x040003BF RID: 959
			kGPU_Overclocking_Switch,
			// Token: 0x040003C0 RID: 960
			kFan_Speed_Switch,
			// Token: 0x040003C1 RID: 961
			kLaunchApp,
			// Token: 0x040003C2 RID: 962
			kAdvanced_Setting_Switch,
			// Token: 0x040003C3 RID: 963
			kMacro,
			// Token: 0x040003C4 RID: 964
			kNone
		}

		// Token: 0x0200004E RID: 78
		public enum CPU_Assignment_Option
		{
			// Token: 0x040003C6 RID: 966
			oNormal_Faster_Turbo,
			// Token: 0x040003C7 RID: 967
			oNormal_Faster,
			// Token: 0x040003C8 RID: 968
			oFaster_Turbo,
			// Token: 0x040003C9 RID: 969
			oNormal_Turbo
		}

		// Token: 0x0200004F RID: 79
		public enum GPU_Assignment_Option
		{
			// Token: 0x040003CB RID: 971
			oNormal_Faster_Turbo,
			// Token: 0x040003CC RID: 972
			oNormal_Faster,
			// Token: 0x040003CD RID: 973
			oFaster_Turbo,
			// Token: 0x040003CE RID: 974
			oNormal_Turbo
		}

		// Token: 0x02000050 RID: 80
		public enum Fan_Assignment_Option
		{
			// Token: 0x040003D0 RID: 976
			oAuto_Max_Custom,
			// Token: 0x040003D1 RID: 977
			oAuto_Max,
			// Token: 0x040003D2 RID: 978
			oMax_Custom,
			// Token: 0x040003D3 RID: 979
			oAuto_Custom
		}

		// Token: 0x02000051 RID: 81
		public enum Setting_Assignment_Option
		{
			// Token: 0x040003D5 RID: 981
			oSticky_Keys,
			// Token: 0x040003D6 RID: 982
			oWindows_menu_key,
			// Token: 0x040003D7 RID: 983
			oLCD_Overdrive
		}

		// Token: 0x02000052 RID: 82
		public enum Degree_Type
		{
			// Token: 0x040003D9 RID: 985
			dCelsius,
			// Token: 0x040003DA RID: 986
			dFahrenheit
		}

		// Token: 0x02000053 RID: 83
		public enum Fan_Mode_Type
		{
			// Token: 0x040003DC RID: 988
			Auto,
			// Token: 0x040003DD RID: 989
			Max,
			// Token: 0x040003DE RID: 990
			Custom
		}

		// Token: 0x02000054 RID: 84
		public enum Fan_AutoMode_Type
		{
			// Token: 0x040003E0 RID: 992
			Normal,
			// Token: 0x040003E1 RID: 993
			Silence,
			// Token: 0x040003E2 RID: 994
			CoolBoost
		}

		// Token: 0x02000055 RID: 85
		public enum Fan_Group_Type
		{
			// Token: 0x040003E4 RID: 996
			fCPU,
			// Token: 0x040003E5 RID: 997
			fGPU
		}

		// Token: 0x02000056 RID: 86
		public enum Backlight_Status
		{
			// Token: 0x040003E7 RID: 999
			tab_static,
			// Token: 0x040003E8 RID: 1000
			tab_dynamic,
			// Token: 0x040003E9 RID: 1001
			tab_off
		}

		// Token: 0x02000057 RID: 87
		public enum Zone_Color_Type
		{
			// Token: 0x040003EB RID: 1003
			Zone_1 = 1,
			// Token: 0x040003EC RID: 1004
			Zone_2,
			// Token: 0x040003ED RID: 1005
			Zone_3,
			// Token: 0x040003EE RID: 1006
			Zone_4
		}

		// Token: 0x02000058 RID: 88
		public enum Overclock_Mode_Type
		{
			// Token: 0x040003F0 RID: 1008
			Normal,
			// Token: 0x040003F1 RID: 1009
			Faster,
			// Token: 0x040003F2 RID: 1010
			Turbo
		}

		// Token: 0x02000059 RID: 89
		public enum AC_Mode_Type
		{
			// Token: 0x040003F4 RID: 1012
			aNo_AC,
			// Token: 0x040003F5 RID: 1013
			a1_AC,
			// Token: 0x040003F6 RID: 1014
			a2_AC
		}

		// Token: 0x0200005A RID: 90
		public enum GPU_Info_Type
		{
			// Token: 0x040003F8 RID: 1016
			gGet_GPU_Frequency = 1,
			// Token: 0x040003F9 RID: 1017
			gGet_GPU_Max_Frequency
		}

		// Token: 0x0200005B RID: 91
		public enum CPU_Info_Type
		{
			// Token: 0x040003FB RID: 1019
			cIsSupportOC = 1,
			// Token: 0x040003FC RID: 1020
			cGet_CPU_Frequency,
			// Token: 0x040003FD RID: 1021
			cGet_CPU_Max_Frequency
		}

		// Token: 0x0200005C RID: 92
		public enum Frequency_Mode
		{
			// Token: 0x040003FF RID: 1023
			fNormal,
			// Token: 0x04000400 RID: 1024
			fMax
		}
	}
}
