using System;
using System.Windows.Input;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x02000033 RID: 51
	public class MouseEventArgsWithKeyData : MouseEventArgs
	{
		// Token: 0x06000245 RID: 581 RVA: 0x00018275 File Offset: 0x00016475
		public MouseEventArgsWithKeyData(MouseDevice mouse, int timestamp, StylusDevice stylusDevice, int[] selectedTaglist, Brush[] selectedColorlist)
			: base(mouse, timestamp, stylusDevice)
		{
			this._selectedTaglist = selectedTaglist;
			this._selectedColorlist = selectedColorlist;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00018290 File Offset: 0x00016490
		public int[] SelectedTaglist
		{
			get
			{
				return this._selectedTaglist;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00018298 File Offset: 0x00016498
		public Brush[] SelectedColorlist
		{
			get
			{
				return this._selectedColorlist;
			}
		}

		// Token: 0x0400027A RID: 634
		private int[] _selectedTaglist;

		// Token: 0x0400027B RID: 635
		private Brush[] _selectedColorlist;
	}
}
