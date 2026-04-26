using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x0200003A RID: 58
	public class ColorToBrushConverter : IValueConverter
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00019812 File Offset: 0x00017A12
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new SolidColorBrush((Color)value);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0001981F File Offset: 0x00017A1F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
