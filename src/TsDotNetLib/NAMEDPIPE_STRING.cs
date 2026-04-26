using System;
using System.Runtime.InteropServices;

namespace TsDotNetLib
{
	// Token: 0x02000006 RID: 6
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct NAMEDPIPE_STRING
	{
		// Token: 0x0400001A RID: 26
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string sKey;

		// Token: 0x0400001B RID: 27
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string sValue;
	}
}
