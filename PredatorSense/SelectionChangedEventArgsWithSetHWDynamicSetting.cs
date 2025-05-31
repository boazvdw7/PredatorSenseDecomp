using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PredatorSense
{
	// Token: 0x0200001F RID: 31
	public class SelectionChangedEventArgsWithSetHWDynamicSetting : SelectionChangedEventArgs
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00013AC1 File Offset: 0x00011CC1
		public SelectionChangedEventArgsWithSetHWDynamicSetting(RoutedEvent id, IList removedItems, IList addedItems, params int[] SetHWDynamicSetting)
			: base(id, removedItems, addedItems)
		{
			this._setHWDynamicSetting = SetHWDynamicSetting;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00013AD4 File Offset: 0x00011CD4
		public int[] SetHWDynamicSetting
		{
			get
			{
				return this._setHWDynamicSetting;
			}
		}

		// Token: 0x040001FD RID: 509
		private int[] _setHWDynamicSetting;
	}
}
