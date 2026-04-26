using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PredatorSense
{
	// Token: 0x0200001E RID: 30
	public class SelectionChangedEventArgsWithReadDynamicSetting : SelectionChangedEventArgs
	{
		// Token: 0x060001BC RID: 444 RVA: 0x00013AA6 File Offset: 0x00011CA6
		public SelectionChangedEventArgsWithReadDynamicSetting(RoutedEvent id, IList removedItems, IList addedItems, int SelectionChangedIndex)
			: base(id, removedItems, addedItems)
		{
			this._selectionChangedIndex = SelectionChangedIndex;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00013AB9 File Offset: 0x00011CB9
		public int SelectionChangedIndex
		{
			get
			{
				return this._selectionChangedIndex;
			}
		}

		// Token: 0x040001FC RID: 508
		private int _selectionChangedIndex;
	}
}
