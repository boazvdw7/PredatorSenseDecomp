using System;
using System.Runtime.InteropServices;

namespace TsDotNetLib
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct DEVICEINFO_P
	{
		// Token: 0x0400001E RID: 30
		public DEVICEINFO deviceinfo;

		// Token: 0x0400001F RID: 31
		public uint priority;
	}
}
