using System;
using System.Globalization;
using System.Management;

namespace PredatorSense
{
	// Token: 0x02000069 RID: 105
	internal class WMIFuncCheck
	{
		// Token: 0x06000329 RID: 809 RVA: 0x00026934 File Offset: 0x00024B34
		private ulong WMIGetFunction(int dataInput)
		{
			ulong num = 0UL;
			try
			{
				ManagementScope managementScope = new ManagementScope("\\\\.\\root\\wmi", null);
				managementScope.Connect();
				ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM APGeAction");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					ManagementBaseObject methodParameters = managementObject.GetMethodParameters("GetFunction");
					methodParameters["uiInput"] = dataInput;
					ManagementBaseObject managementBaseObject2 = managementObject.InvokeMethod("GetFunction", methodParameters, null);
					num = Convert.ToUInt64(managementBaseObject2.Properties["uiOutput"].Value);
				}
				managementObjectSearcher.Dispose();
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00026A14 File Offset: 0x00024C14
		private ulong WMISetFunction(int dataInput)
		{
			ulong num = 0UL;
			try
			{
				ManagementScope managementScope = new ManagementScope("\\\\.\\root\\wmi", null);
				managementScope.Connect();
				ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM APGeAction");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					ManagementBaseObject methodParameters = managementObject.GetMethodParameters("SetFunction");
					methodParameters["uiInput"] = dataInput;
					ManagementBaseObject managementBaseObject2 = managementObject.InvokeMethod("SetFunction", methodParameters, null);
					num = (ulong)Convert.ToUInt32(managementBaseObject2.Properties["uiOutput"].Value);
				}
				managementObjectSearcher.Dispose();
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00026AF4 File Offset: 0x00024CF4
		public string DoSupportRGBKeyboard()
		{
			bool flag = false;
			string text2;
			try
			{
				ManagementScope managementScope = new ManagementScope("\\root\\wmi", new ConnectionOptions
				{
					Impersonation = ImpersonationLevel.Impersonate,
					EnablePrivileges = true
				});
				managementScope.Connect();
				WqlObjectQuery wqlObjectQuery = new WqlObjectQuery("select * from MSSMBios_RawSMBiosTables");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, wqlObjectQuery);
				int num = 0;
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					if (managementObject != null)
					{
						num = int.Parse(managementObject.Properties["Size"].Value.ToString());
						break;
					}
				}
				if (num > 0)
				{
					byte[] array = new byte[num];
					foreach (ManagementBaseObject managementBaseObject2 in managementObjectSearcher.Get())
					{
						ManagementObject managementObject2 = (ManagementObject)managementBaseObject2;
						if (managementObject2 != null)
						{
							array = managementObject2.Properties["SMBiosData"].Value as byte[];
							break;
						}
					}
					int i = 0;
					while (i < num)
					{
						byte b = array[0];
						byte b2 = array[1];
						byte[] array2 = new byte[num + 1];
						Array.Copy(array, (int)b2, array2, 0, num - (int)b2);
						int num2 = 0;
						while (array2[num2] != 0 || array2[num2 + 1] != 0)
						{
							num2++;
						}
						i = i + (int)b2 + num2 + 2;
						Array.Copy(array, (int)b2 + num2 + 2, array, 0, num - ((int)b2 + num2 + 2));
						if (array[0] == 171)
						{
							for (int j = 4; j < (int)array[1]; j += 5)
							{
								int num3 = (int)array[j];
								byte b3 = array[j + 1];
								byte b4 = array[j + 2];
								byte b5 = array[j + 3];
								byte b6 = array[j + 4];
								if (num3 == 19)
								{
									flag = true;
								}
							}
						}
					}
				}
				managementObjectSearcher.Dispose();
				string text = "";
				text += (flag ? " RGBKB=1" : " RGBKB=0");
				text2 = text;
			}
			catch
			{
				text2 = "";
			}
			return text2;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00026D58 File Offset: 0x00024F58
		public string GetModelName()
		{
			string text = "";
			string text2;
			try
			{
				ManagementScope managementScope = new ManagementScope("\\root\\cimv2", new ConnectionOptions
				{
					Impersonation = ImpersonationLevel.Impersonate,
					EnablePrivileges = true
				});
				managementScope.Connect();
				WqlObjectQuery wqlObjectQuery = new WqlObjectQuery("select * from Win32_ComputerSystem");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, wqlObjectQuery);
				if (managementObjectSearcher != null)
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (managementObject["Model"] != null)
						{
							text = managementObject["Model"].ToString().ToLower(CultureInfo.InvariantCulture);
						}
					}
				}
				managementObjectSearcher.Dispose();
				text2 = text;
			}
			catch
			{
				text2 = "";
			}
			return text2;
		}
	}
}
