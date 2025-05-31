using System;
using System.IO.Pipes;
using Microsoft.Win32;

namespace TsDotNetLib
{
	// Token: 0x0200001B RID: 27
	public class Registry
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000024D8 File Offset: 0x000006D8
		public static bool ValueExistsLM(string RegPath, string RegValue)
		{
			bool flag = false;
			try
			{
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegPath, false);
				if (registryKey != null)
				{
					object value = registryKey.GetValue(RegValue);
					if (value != null)
					{
						flag = true;
					}
					registryKey.Close();
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002520 File Offset: 0x00000720
		public static int CheckLM(string RegPath, string RegValue, uint defaultValue = 0U)
		{
			int num = -1;
			try
			{
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegPath, false);
				if (registryKey != null)
				{
					object value = registryKey.GetValue(RegValue);
					if (value != null)
					{
						num = Convert.ToInt32(value);
					}
					registryKey.Close();
				}
			}
			catch
			{
			}
			if (num == -1)
			{
				Registry.SetValueLM(RegPath, RegValue, defaultValue);
				num = (int)defaultValue;
			}
			return num;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000257C File Offset: 0x0000077C
		public static int CheckCU(string RegPath, string RegValue)
		{
			int num = -1;
			try
			{
				RegistryKey registryKey = global::Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath, false);
				if (registryKey != null)
				{
					object value = registryKey.GetValue(RegValue);
					if (value != null)
					{
						num = Convert.ToInt32(value);
					}
					registryKey.Close();
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025CC File Offset: 0x000007CC
		public static string GetStringLM(string RegPath, string RegValue)
		{
			string text = null;
			try
			{
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegPath, false);
				if (registryKey != null)
				{
					object value = registryKey.GetValue(RegValue);
					if (value != null)
					{
						text = value.ToString();
					}
					registryKey.Close();
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000261C File Offset: 0x0000081C
		public static void SetValueLM(string subkeyPath, string valueName, uint value)
		{
			try
			{
				string text = "HKEY_LOCAL_MACHINE\\" + subkeyPath;
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				namedPipeClientStream.Connect();
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkeyPath, false);
				if (registryKey == null)
				{
					IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 1, new object[] { text });
					namedPipeClientStream.WaitForPipeDrain();
				}
				else
				{
					registryKey.Close();
				}
				IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 3, new object[] { text, valueName, 4, value });
				namedPipeClientStream.WaitForPipeDrain();
				namedPipeClientStream.Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026D0 File Offset: 0x000008D0
		public static void SetStringLM(string subkeyPath, string valueName, string value)
		{
			try
			{
				string text = "HKEY_LOCAL_MACHINE\\" + subkeyPath;
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				namedPipeClientStream.Connect();
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkeyPath, false);
				if (registryKey == null)
				{
					IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 1, new object[] { text });
					namedPipeClientStream.WaitForPipeDrain();
				}
				else
				{
					registryKey.Close();
				}
				IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 3, new object[] { text, valueName, 1, value });
				namedPipeClientStream.WaitForPipeDrain();
				namedPipeClientStream.Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000277C File Offset: 0x0000097C
		public static void SetBinaryLM(string subkeyPath, string valueName, Guid value)
		{
			try
			{
				string text = "HKEY_LOCAL_MACHINE\\" + subkeyPath;
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				namedPipeClientStream.Connect();
				RegistryKey registryKey = global::Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkeyPath, false);
				if (registryKey == null)
				{
					IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 1, new object[] { text });
					namedPipeClientStream.WaitForPipeDrain();
				}
				else
				{
					registryKey.Close();
				}
				IPCMethods.SendCommandByNamedPipe(namedPipeClientStream, 3, new object[] { text, valueName, 3, value });
				namedPipeClientStream.WaitForPipeDrain();
				namedPipeClientStream.Close();
			}
			catch
			{
			}
		}
	}
}
