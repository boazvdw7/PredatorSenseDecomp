using System;

namespace PredatorSense
{
	// Token: 0x0200001A RID: 26
	public class Direction
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00013A2B File Offset: 0x00011C2B
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00013A33 File Offset: 0x00011C33
		public string _DynamicDirection
		{
			get
			{
				return this._dynamicDirection;
			}
			set
			{
				this._dynamicDirection = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00013A3C File Offset: 0x00011C3C
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00013A44 File Offset: 0x00011C44
		public double _DynamicDirectionGridColumnIndex
		{
			get
			{
				return this._dynamicDirectionGridColumnIndex;
			}
			set
			{
				this._dynamicDirectionGridColumnIndex = value;
			}
		}

		// Token: 0x040001F7 RID: 503
		private string _dynamicDirection;

		// Token: 0x040001F8 RID: 504
		private double _dynamicDirectionGridColumnIndex;
	}
}
