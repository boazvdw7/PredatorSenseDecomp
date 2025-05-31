using System;
using System.Runtime.InteropServices;

namespace TsDotNetLib
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct HOSTEDNETWORKINFO
	{
		// Token: 0x0400001C RID: 28
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string netSSID;

		// Token: 0x0400001D RID: 29
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string netPWD;
	}
}
