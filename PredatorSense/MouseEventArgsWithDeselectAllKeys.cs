using System;
using System.Windows.Input;

namespace PredatorSense
{
	// Token: 0x02000035 RID: 53
	public class MouseEventArgsWithDeselectAllKeys : MouseEventArgs
	{
		// Token: 0x0600024B RID: 587 RVA: 0x000182C9 File Offset: 0x000164C9
		public MouseEventArgsWithDeselectAllKeys(MouseDevice mouse, int timestamp, StylusDevice stylusDevice)
			: base(mouse, timestamp, stylusDevice)
		{
		}
	}
}
