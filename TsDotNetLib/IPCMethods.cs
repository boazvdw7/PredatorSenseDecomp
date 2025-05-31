using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;

namespace TsDotNetLib
{
	// Token: 0x0200001A RID: 26
	public class IPCMethods
	{
		// Token: 0x06000005 RID: 5
		[DllImport("User32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000006 RID: 6
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool PostMessage(IntPtr hwnd, uint wMsg, UIntPtr wParam, IntPtr lParam);

		// Token: 0x06000007 RID: 7 RVA: 0x00002210 File Offset: 0x00000410
		public static void SendCommandByNamedPipe(NamedPipeClientStream client, int cmdCode, params object[] args)
		{
			int num = 3;
			for (int i = 0; i < args.Length; i++)
			{
				int sizeOnMemory = IPCMethods.GetSizeOnMemory(args[i]);
				num += 4 + sizeOnMemory;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			byte[] array2 = BitConverter.GetBytes(cmdCode);
			Array.Copy(array2, 0, array, num2, 2);
			num2 += 2;
			array2 = BitConverter.GetBytes(args.Length);
			Array.Copy(array2, 0, array, num2, 1);
			num2++;
			for (int j = 0; j < args.Length; j++)
			{
				int sizeOnMemory2 = IPCMethods.GetSizeOnMemory(args[j]);
				array2 = BitConverter.GetBytes(sizeOnMemory2);
				Array.Copy(array2, 0, array, num2, 4);
				num2 += 4;
				if (sizeOnMemory2 > 0)
				{
					if (args[j].GetType() == typeof(string))
					{
						string text = (string)args[j];
						array2 = Encoding.Unicode.GetBytes(text + "\0");
					}
					else if (args[j].GetType() == typeof(byte[]))
					{
						array2 = (byte[])args[j];
					}
					else
					{
						array2 = IPCMethods.AnyToBytes(args[j]);
					}
					Array.Copy(array2, 0, array, num2, sizeOnMemory2);
					num2 += sizeOnMemory2;
				}
			}
			if (num != num2)
			{
				throw new Exception("Something wrong while creating command message.");
			}
			client.Write(array, 0, array.Length);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002364 File Offset: 0x00000564
		public static bool SendCommandByWindowMessage(string className, string windowName, uint message, uint wParam, int lParam)
		{
			IntPtr intPtr = IPCMethods.FindWindow(className, windowName);
			return !(intPtr == IntPtr.Zero) && IPCMethods.PostMessage(intPtr, message, new UIntPtr(wParam), new IntPtr(lParam));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000239C File Offset: 0x0000059C
		private static int GetSizeOnMemory(object obj)
		{
			int num;
			if (obj.GetType() == typeof(string))
			{
				string text = (string)obj;
				num = text.Length * 2 + 2;
			}
			else if (obj.GetType() == typeof(byte[]))
			{
				byte[] array = (byte[])obj;
				num = array.Length;
			}
			else
			{
				num = Marshal.SizeOf(obj);
			}
			return num;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002400 File Offset: 0x00000600
		private static byte[] AnyToBytes(object obj)
		{
			int num = Marshal.SizeOf(obj);
			byte[] array = new byte[num];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			Marshal.StructureToPtr(obj, gchandle.AddrOfPinnedObject(), false);
			gchandle.Free();
			return array;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000243C File Offset: 0x0000063C
		public static T BytesToStruct<T>(byte[] bytes)
		{
			if (bytes == null)
			{
				return default(T);
			}
			if (bytes.Length <= 0)
			{
				return default(T);
			}
			int num = bytes.Length;
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			T t;
			try
			{
				Marshal.Copy(bytes, 0, intPtr, num);
				t = (T)((object)Marshal.PtrToStructure(intPtr, typeof(T)));
			}
			catch (Exception ex)
			{
				throw new Exception("Error in BytesToStruct ! " + ex.Message);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return t;
		}

		// Token: 0x040000E3 RID: 227
		private const int CMF_CMD_INDEX = 0;

		// Token: 0x040000E4 RID: 228
		private const int CMF_CMD_SIZE = 2;

		// Token: 0x040000E5 RID: 229
		private const int CMF_NUM_ARG_INDEX = 2;

		// Token: 0x040000E6 RID: 230
		private const int CMF_NUM_ARG_SIZE = 1;

		// Token: 0x040000E7 RID: 231
		private const int CMF_ARG_SIZE_SIZE = 4;

		// Token: 0x040000E8 RID: 232
		private const int RMF_NUM_ARG_INDEX = 0;

		// Token: 0x040000E9 RID: 233
		private const int RMF_NUM_ARG_SIZE = 1;

		// Token: 0x040000EA RID: 234
		private const int RMF_ARG_SIZE_SIZE = 4;
	}
}
