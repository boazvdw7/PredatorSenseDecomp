using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PredatorSense
{
	// Token: 0x0200001C RID: 28
	public class SelectionChangedEventArgsWithWriteDynamicSetting : SelectionChangedEventArgs
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x00013A70 File Offset: 0x00011C70
		public SelectionChangedEventArgsWithWriteDynamicSetting(RoutedEvent id, IList removedItems, IList addedItems, params int[] WriteDynamicSetting)
			: base(id, removedItems, addedItems)
		{
			this._writeDynamicSetting = WriteDynamicSetting;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00013A83 File Offset: 0x00011C83
		public int[] WriteDynamicSetting
		{
			get
			{
				return this._writeDynamicSetting;
			}
		}

		// Token: 0x040001FA RID: 506
		private int[] _writeDynamicSetting;
	}
}
