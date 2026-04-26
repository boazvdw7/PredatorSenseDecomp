using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PredatorSense.Properties
{
	// Token: 0x0200006C RID: 108
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00029929 File Offset: 0x00027B29
		internal Resources()
		{
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00029934 File Offset: 0x00027B34
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("PredatorSense.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0002996D File Offset: 0x00027B6D
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00029974 File Offset: 0x00027B74
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000452 RID: 1106
		private static ResourceManager resourceMan;

		// Token: 0x04000453 RID: 1107
		private static CultureInfo resourceCulture;
	}
}
