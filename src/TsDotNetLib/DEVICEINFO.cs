using System;
using System.Runtime.InteropServices;

namespace TsDotNetLib
{
	// Token: 0x02000005 RID: 5
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct DEVICEINFO
	{
		// Token: 0x04000013 RID: 19
		public uint PhysicalMediumType;

		// Token: 0x04000014 RID: 20
		public uint MediaType;

		// Token: 0x04000015 RID: 21
		[MarshalAs(UnmanagedType.Bool)]
		public bool IsShareEnalbe;

		// Token: 0x04000016 RID: 22
		[MarshalAs(UnmanagedType.Bool)]
		public bool IsInternetAvailable;

		// Token: 0x04000017 RID: 23
		[MarshalAs(UnmanagedType.Bool)]
		public bool IsVirtual;

		// Token: 0x04000018 RID: 24
		public Guid devGuid;

		// Token: 0x04000019 RID: 25
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string devName;
	}
}
