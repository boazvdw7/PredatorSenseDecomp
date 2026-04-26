using System;
using System.Windows;

namespace PredatorSense
{
	// Token: 0x02000020 RID: 32
	public class RoutedPropertyChangedEventArgsWithSetHWDynamicSetting : RoutedPropertyChangedEventArgs<double>
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00013ADC File Offset: 0x00011CDC
		public RoutedPropertyChangedEventArgsWithSetHWDynamicSetting(double oldValue, double newValue, RoutedEvent routedEvent, params int[] SetHWDynamicSetting)
			: base(oldValue, newValue, routedEvent)
		{
			this._setHWDynamicSetting = SetHWDynamicSetting;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00013AEF File Offset: 0x00011CEF
		public int[] SetHWDynamicSetting
		{
			get
			{
				return this._setHWDynamicSetting;
			}
		}

		// Token: 0x040001FE RID: 510
		private int[] _setHWDynamicSetting;
	}
}
