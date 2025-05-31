using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PredatorSense
{
	// Token: 0x02000039 RID: 57
	public partial class Start_Recording_Loading : UserControl
	{
		// Token: 0x0600025D RID: 605 RVA: 0x00019761 File Offset: 0x00017961
		public Start_Recording_Loading()
		{
			this.InitializeComponent();
			this.sb = base.TryFindResource("sb") as Storyboard;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00019785 File Offset: 0x00017985
		public void Start()
		{
			this.sb.Begin();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00019792 File Offset: 0x00017992
		public void Stop()
		{
			this.sb.Stop();
		}

		// Token: 0x040002A8 RID: 680
		private Storyboard sb;
	}
}
