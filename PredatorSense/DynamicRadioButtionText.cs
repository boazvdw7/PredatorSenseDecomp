using System;

namespace PredatorSense
{
	// Token: 0x02000019 RID: 25
	public class DynamicRadioButtionText
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000139F0 File Offset: 0x00011BF0
		// (set) Token: 0x060001AB RID: 427 RVA: 0x000139F8 File Offset: 0x00011BF8
		public string _DynamicRadioButtionText
		{
			get
			{
				return this._dynamicRadioButtionText;
			}
			set
			{
				this._dynamicRadioButtionText = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00013A01 File Offset: 0x00011C01
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00013A09 File Offset: 0x00011C09
		public double _DynamicRadioButtionGridRowIndex
		{
			get
			{
				return this._dynamicRadioButtionGridRowIndex;
			}
			set
			{
				this._dynamicRadioButtionGridRowIndex = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00013A12 File Offset: 0x00011C12
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00013A1A File Offset: 0x00011C1A
		public double _DynamicRadioButtionGridColumnIndex
		{
			get
			{
				return this._dynamicRadioButtionGridColumnIndex;
			}
			set
			{
				this._dynamicRadioButtionGridColumnIndex = value;
			}
		}

		// Token: 0x040001F4 RID: 500
		private string _dynamicRadioButtionText;

		// Token: 0x040001F5 RID: 501
		private double _dynamicRadioButtionGridRowIndex;

		// Token: 0x040001F6 RID: 502
		private double _dynamicRadioButtionGridColumnIndex;
	}
}
