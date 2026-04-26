using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PredatorSense
{
	// Token: 0x0200001B RID: 27
	public class SelectionChangedEventArgsWithDynamicRadioButtion : SelectionChangedEventArgs
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00013A55 File Offset: 0x00011C55
		public SelectionChangedEventArgsWithDynamicRadioButtion(RoutedEvent id, IList removedItems, IList addedItems, bool Hidecolorpanel)
			: base(id, removedItems, addedItems)
		{
			this._hidecolorpanel = Hidecolorpanel;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00013A68 File Offset: 0x00011C68
		public bool Hidecolorpanel
		{
			get
			{
				return this._hidecolorpanel;
			}
		}

		// Token: 0x040001F9 RID: 505
		private bool _hidecolorpanel;
	}
}
