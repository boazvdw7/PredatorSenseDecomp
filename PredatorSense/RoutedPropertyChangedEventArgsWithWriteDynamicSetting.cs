using System;
using System.Windows;

namespace PredatorSense
{
	// Token: 0x0200001D RID: 29
	public class RoutedPropertyChangedEventArgsWithWriteDynamicSetting : RoutedPropertyChangedEventArgs<double>
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00013A8B File Offset: 0x00011C8B
		public RoutedPropertyChangedEventArgsWithWriteDynamicSetting(double oldValue, double newValue, RoutedEvent routedEvent, params int[] WriteDynamicSetting)
			: base(oldValue, newValue, routedEvent)
		{
			this._writeDynamicSetting = WriteDynamicSetting;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00013A9E File Offset: 0x00011C9E
		public int[] WriteDynamicSetting
		{
			get
			{
				return this._writeDynamicSetting;
			}
		}

		// Token: 0x040001FB RID: 507
		private int[] _writeDynamicSetting;
	}
}
