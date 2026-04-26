using System;
using System.Globalization;
using System.IO;

namespace TsDotNetLib
{
	// Token: 0x02000019 RID: 25
	public class Log
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public Log(string logFileName, string projectName)
		{
			this.logFileName = logFileName;
			this.projectName = projectName;
			if (!Directory.Exists(Path.GetDirectoryName(logFileName)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
			}
			this.lockObject = new object();
			this.logLevel = LogType.Warning;
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense", "LogLevel"))
			{
				this.logLevel = (LogType)Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense", "LogLevel", 0U);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020CB File Offset: 0x000002CB
		public static void LogWrite(Log log, LogType logType, string functionName, string message)
		{
			if (log == null)
			{
				return;
			}
			log.Write(logType, functionName, message);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020DC File Offset: 0x000002DC
		private void Write(LogType logType, string functionName, string message)
		{
			DateTime now = DateTime.Now;
			string text = "";
			switch (logType)
			{
			case LogType.Info:
				text = "[INFO]     ";
				break;
			case LogType.Warning:
				text = "[WARNING]  ";
				break;
			case LogType.Error:
				text = "[ERROR]    ";
				break;
			case LogType.Exception:
				text = "[EXCEPTION]";
				break;
			}
			lock (this.lockObject)
			{
				try
				{
					StreamWriter streamWriter = new StreamWriter(this.logFileName, true);
					streamWriter.WriteLine(string.Concat(new string[]
					{
						text,
						" ",
						now.ToString("G", DateTimeFormatInfo.InvariantInfo),
						".",
						now.Millisecond.ToString("d3"),
						" | ",
						this.projectName,
						" | ",
						functionName,
						" | ",
						message
					}));
					streamWriter.Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x040000DF RID: 223
		private string logFileName;

		// Token: 0x040000E0 RID: 224
		private string projectName;

		// Token: 0x040000E1 RID: 225
		private object lockObject;

		// Token: 0x040000E2 RID: 226
		private LogType logLevel;
	}
}
