using System;
using System.Windows.Controls;

namespace PredatorSense
{
	// Token: 0x02000021 RID: 33
	public class UserControlWithHidecolorpanel : UserControl
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00013AF7 File Offset: 0x00011CF7
		public UserControlWithHidecolorpanel(bool Hidecolorpanel)
		{
			this._hidecolorpanel = Hidecolorpanel;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00013B06 File Offset: 0x00011D06
		public bool Hidecolorpanel
		{
			get
			{
				return this._hidecolorpanel;
			}
		}

		// Token: 0x040001FF RID: 511
		private bool _hidecolorpanel;
	}
}
