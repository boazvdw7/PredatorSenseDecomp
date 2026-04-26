using System;

namespace PredatorSense
{
	// Token: 0x02000003 RID: 3
	internal class dataflow
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000025F0 File Offset: 0x000007F0
		public dataflow()
		{
			this.dataType = null;
			this.dataName = null;
			this.data = null;
			this.location = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002614 File Offset: 0x00000814
		public void setData(object type, object name, object d)
		{
			this.dataType = type;
			this.dataName = name;
			this.data = d;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000262B File Offset: 0x0000082B
		public void setLocation(object loc)
		{
			this.location = loc;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002634 File Offset: 0x00000834
		public object getType()
		{
			return this.dataType;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000263C File Offset: 0x0000083C
		public object getName()
		{
			return this.dataName;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002644 File Offset: 0x00000844
		public object getdata()
		{
			return this.data;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000264C File Offset: 0x0000084C
		public object getloc()
		{
			return this.location;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002654 File Offset: 0x00000854
		public new string ToString()
		{
			return string.Concat(new object[] { this.dataType, " ", this.dataName, " ", this.data });
		}

		// Token: 0x0400000B RID: 11
		private object dataType;

		// Token: 0x0400000C RID: 12
		private object dataName;

		// Token: 0x0400000D RID: 13
		private object data;

		// Token: 0x0400000E RID: 14
		private object location;
	}
}
