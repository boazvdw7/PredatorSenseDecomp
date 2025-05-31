using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000068 RID: 104
	public class USBFunction
	{
		// Token: 0x06000322 RID: 802 RVA: 0x00025EB0 File Offset: 0x000240B0
		public static async Task<uint> SetUSBKeyboardColor(byte[] input)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 5, new object[] { input });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt32(array, 5);
				});
				uint output = 0U;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = uint.MaxValue;
			}
			return num;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00026194 File Offset: 0x00024394
		public static async Task<uint> SetUSBPattern(byte iEffect, byte iSpeed, byte[] cColor, byte iDirect)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 6, new object[] { iEffect, iSpeed, cColor, iDirect });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt32(array, 5);
				});
				uint output = 0U;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = uint.MaxValue;
			}
			return num;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0002647C File Offset: 0x0002467C
		public static async Task<uint> SetUSBPad(ulong intput, byte[] color = null)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					if (color != null)
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 7, new object[] { intput, color });
					}
					else
					{
						IPCMethods.SendCommandByNamedPipe(cline_stream, 7, new object[] { intput });
					}
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt32(array, 5);
				});
				uint output = 0U;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = uint.MaxValue;
			}
			return num;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000266B8 File Offset: 0x000248B8
		public static async Task<uint> GetUSBPad()
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 8, new object[0]);
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt32(array, 5);
				});
				uint output = 0U;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = uint.MaxValue;
			}
			return num;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000268E4 File Offset: 0x00024AE4
		public static async Task<uint> GetUSBKeyboardType()
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_admin_agent_" + CommonFunction.session_id, PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 10, new object[0]);
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[9];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt32(array, 5);
				});
				uint output = 0U;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = uint.MaxValue;
			}
			return num;
		}
	}
}
