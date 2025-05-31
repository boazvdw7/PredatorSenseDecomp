using System;
using System.Windows;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x02000034 RID: 52
	public class RoutedEventArgsWithKeyData : RoutedEventArgs
	{
		// Token: 0x06000248 RID: 584 RVA: 0x000182A0 File Offset: 0x000164A0
		public RoutedEventArgsWithKeyData(RoutedEvent routedEvent, object source, int[] selectedTaglist, Brush[] selectedColorlist)
			: base(routedEvent, source)
		{
			this._selectedTaglist = selectedTaglist;
			this._selectedColorlist = selectedColorlist;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000182B9 File Offset: 0x000164B9
		public int[] SelectedTaglist
		{
			get
			{
				return this._selectedTaglist;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000182C1 File Offset: 0x000164C1
		public Brush[] SelectedColorlist
		{
			get
			{
				return this._selectedColorlist;
			}
		}

		// Token: 0x0400027C RID: 636
		private int[] _selectedTaglist;

		// Token: 0x0400027D RID: 637
		private Brush[] _selectedColorlist;
	}
}
