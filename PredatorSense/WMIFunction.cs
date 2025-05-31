using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000067 RID: 103
	public class WMIFunction
	{
		// Token: 0x06000316 RID: 790 RVA: 0x000243A8 File Offset: 0x000225A8
		public static async Task<uint> SetAcerGamingPorfileConfiguration(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 9, new object[] { intput });
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

		// Token: 0x06000317 RID: 791 RVA: 0x0002461C File Offset: 0x0002281C
		public static async Task<ulong> GetAcerGamingPorfileConfiguration(uint intput)
		{
			ulong num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<ulong> tsk = Task.Run<ulong>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 10, new object[] { intput });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[13];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt64(array, 5);
				});
				ulong output = 0UL;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
            catch (Exception)
            {
                num = unchecked((ulong)(-1));
            }
            return num;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00024890 File Offset: 0x00022A90
		public static async Task<uint> SetAcerGamingLEDGroupColor(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 11, new object[] { intput });
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

		// Token: 0x06000319 RID: 793 RVA: 0x00024B04 File Offset: 0x00022D04
		public static async Task<ulong> GetAcerGamingLEDGroupColor(uint intput)
		{
			ulong num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<ulong> tsk = Task.Run<ulong>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 12, new object[] { intput });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[13];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt64(array, 5);
				});
				ulong output = 0UL;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = unchecked((ulong)(-1));
			}
			return num;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00024D78 File Offset: 0x00022F78
		public static async Task<ulong> GetAcerGamingSystemInformation(uint intput)
		{
			ulong num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<ulong> tsk = Task.Run<ulong>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 13, new object[] { intput });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[13];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt64(array, 5);
				});
				ulong output = 0UL;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = unchecked((ulong)(-1));
			}
			return num;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00024FEC File Offset: 0x000231EC
		public static async Task<uint> SetAcerGamingFanGroupBehavior(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 15, new object[] { intput });
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

		// Token: 0x0600031C RID: 796 RVA: 0x00025260 File Offset: 0x00023460
		public static async Task<uint> SetAcerGamingFanGroupSpeed(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 16, new object[] { intput });
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

		// Token: 0x0600031D RID: 797 RVA: 0x000254D4 File Offset: 0x000236D4
		public static async Task<uint> WMISetFunction(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 17, new object[] { intput });
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

		// Token: 0x0600031E RID: 798 RVA: 0x00025748 File Offset: 0x00023948
		public static async Task<ulong> WMIGetFunction(uint intput)
		{
			ulong num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<ulong> tsk = Task.Run<ulong>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 20, new object[] { intput });
					cline_stream.WaitForPipeDrain();
					byte[] array = new byte[13];
					cline_stream.Read(array, 0, array.Length);
					return BitConverter.ToUInt64(array, 5);
				});
				ulong output = 0UL;
				output = await tsk.ConfigureAwait(false);
				cline_stream.Close();
				num = output;
			}
			catch (Exception)
			{
				num = unchecked((ulong)(-1));
			}
			return num;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000259BC File Offset: 0x00023BBC
		public static async Task<uint> WMISetLogoLED(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 18, new object[] { intput });
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

		// Token: 0x06000320 RID: 800 RVA: 0x00025C30 File Offset: 0x00023E30
		public static async Task<uint> WMISetLogoStatus(ulong intput)
		{
			uint num;
			try
			{
				NamedPipeClientStream cline_stream = new NamedPipeClientStream(".", "predatorsense_service_namedpipe", PipeDirection.InOut);
				cline_stream.Connect();
				Task<uint> tsk = Task.Run<uint>(delegate
				{
					IPCMethods.SendCommandByNamedPipe(cline_stream, 19, new object[] { intput });
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
