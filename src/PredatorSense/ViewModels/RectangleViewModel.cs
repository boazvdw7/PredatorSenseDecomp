using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PredatorSense
{
	// Token: 0x0200003B RID: 59
	public class RectangleViewModel : INotifyPropertyChanged
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0001982E File Offset: 0x00017A2E
		public RectangleViewModel()
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00019838 File Offset: 0x00017A38
		public RectangleViewModel(double x, double y, string selectedRectStyle, double borderwidth, double borderheight, string outerRectStyle, double outerwidth, double outerheight, Color strokecolor, string innerRectStyle, double innerwidth, double innerheight, Color color, string innerStyle, double innerfontwidth, double innerfontheight, double innerRec_font_gap, Color fontcolor, int keytag)
		{
			this.canvas_x = x;
			this.canvas_y = y;
			this.x = x + 65.0;
			this.y = y + 72.0;
			this.borderwidth = borderwidth;
			this.borderheight = borderheight;
			this.v_width_Canvasleft = borderwidth - 10.0;
			this.v_width_Canvastop = borderheight - 10.0;
			this.outerwidth = outerwidth;
			this.outerheight = outerheight;
			this.innerwidth = innerwidth;
			this.innerheight = innerheight;
			this.innerfontwidth = innerfontwidth;
			this.innerfontheight = innerfontheight;
			this.innerfontwidth_Canvasleft = (innerwidth - innerfontwidth) / 2.0;
			this.innerfontwidth_Canvastop = innerRec_font_gap;
			this.color = color;
			this.strokecolor = strokecolor;
			this.fontcolor = fontcolor;
			this.keytag = keytag;
			this.selectedRectStyle = selectedRectStyle;
			this.outerRectStyle = outerRectStyle;
			this.innerRectStyle = innerRectStyle;
			this.innerStyle = innerStyle;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0001993B File Offset: 0x00017B3B
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00019943 File Offset: 0x00017B43
		public double X
		{
			get
			{
				return this.x;
			}
			set
			{
				if (this.x == value)
				{
					return;
				}
				this.x = value;
				this.OnPropertyChanged("X");
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00019961 File Offset: 0x00017B61
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00019969 File Offset: 0x00017B69
		public double Y
		{
			get
			{
				return this.y;
			}
			set
			{
				if (this.y == value)
				{
					return;
				}
				this.y = value;
				this.OnPropertyChanged("Y");
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00019987 File Offset: 0x00017B87
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0001998F File Offset: 0x00017B8F
		public double Canvas_x
		{
			get
			{
				return this.canvas_x;
			}
			set
			{
				if (this.canvas_x == value)
				{
					return;
				}
				this.canvas_x = value;
				this.OnPropertyChanged("Canvas_x");
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600026D RID: 621 RVA: 0x000199AD File Offset: 0x00017BAD
		// (set) Token: 0x0600026E RID: 622 RVA: 0x000199B5 File Offset: 0x00017BB5
		public double Canvas_y
		{
			get
			{
				return this.canvas_y;
			}
			set
			{
				if (this.canvas_y == value)
				{
					return;
				}
				this.canvas_y = value;
				this.OnPropertyChanged("Canvas_y");
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600026F RID: 623 RVA: 0x000199D3 File Offset: 0x00017BD3
		// (set) Token: 0x06000270 RID: 624 RVA: 0x000199DB File Offset: 0x00017BDB
		public double Borderwidth
		{
			get
			{
				return this.borderwidth;
			}
			set
			{
				if (this.borderwidth == value)
				{
					return;
				}
				this.borderwidth = value;
				this.OnPropertyChanged("Borderwidth");
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000271 RID: 625 RVA: 0x000199F9 File Offset: 0x00017BF9
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00019A01 File Offset: 0x00017C01
		public double Borderheighth
		{
			get
			{
				return this.borderheight;
			}
			set
			{
				if (this.borderheight == value)
				{
					return;
				}
				this.borderheight = value;
				this.OnPropertyChanged("Borderheighth");
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00019A1F File Offset: 0x00017C1F
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00019A27 File Offset: 0x00017C27
		public double V_width_Canvasleft
		{
			get
			{
				return this.v_width_Canvasleft;
			}
			set
			{
				if (this.v_width_Canvasleft == value)
				{
					return;
				}
				this.v_width_Canvasleft = value;
				this.OnPropertyChanged("V_width_Canvasleft");
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00019A45 File Offset: 0x00017C45
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00019A4D File Offset: 0x00017C4D
		public double V_width_Canvastop
		{
			get
			{
				return this.v_width_Canvastop;
			}
			set
			{
				if (this.v_width_Canvastop == value)
				{
					return;
				}
				this.v_width_Canvastop = value;
				this.OnPropertyChanged("V_width_Canvastop");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00019A6B File Offset: 0x00017C6B
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00019A73 File Offset: 0x00017C73
		public double Outerwidth
		{
			get
			{
				return this.outerwidth;
			}
			set
			{
				if (this.outerwidth == value)
				{
					return;
				}
				this.outerwidth = value;
				this.OnPropertyChanged("Outerwidth");
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00019A91 File Offset: 0x00017C91
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00019A99 File Offset: 0x00017C99
		public double Outerheight
		{
			get
			{
				return this.outerheight;
			}
			set
			{
				if (this.outerheight == value)
				{
					return;
				}
				this.outerheight = value;
				this.OnPropertyChanged("Outerheight");
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00019AB7 File Offset: 0x00017CB7
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00019ABF File Offset: 0x00017CBF
		public double Innerwidth
		{
			get
			{
				return this.innerwidth;
			}
			set
			{
				if (this.innerwidth == value)
				{
					return;
				}
				this.innerwidth = value;
				this.OnPropertyChanged("Innerwidth");
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00019ADD File Offset: 0x00017CDD
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00019AE5 File Offset: 0x00017CE5
		public double Innerheight
		{
			get
			{
				return this.innerheight;
			}
			set
			{
				if (this.innerheight == value)
				{
					return;
				}
				this.innerheight = value;
				this.OnPropertyChanged("Innerheight");
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00019B03 File Offset: 0x00017D03
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00019B0B File Offset: 0x00017D0B
		public double Innerfontwidth
		{
			get
			{
				return this.innerfontwidth;
			}
			set
			{
				if (this.innerfontwidth == value)
				{
					return;
				}
				this.innerfontwidth = value;
				this.OnPropertyChanged("Innerfontwidth");
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00019B29 File Offset: 0x00017D29
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00019B31 File Offset: 0x00017D31
		public double Innerfontheight
		{
			get
			{
				return this.innerfontheight;
			}
			set
			{
				if (this.innerfontheight == value)
				{
					return;
				}
				this.innerfontheight = value;
				this.OnPropertyChanged("Innerfontheight");
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00019B4F File Offset: 0x00017D4F
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00019B57 File Offset: 0x00017D57
		public double Innerfontwidth_Canvasleft
		{
			get
			{
				return this.innerfontwidth_Canvasleft;
			}
			set
			{
				if (this.innerfontwidth_Canvasleft == value)
				{
					return;
				}
				this.innerfontwidth_Canvasleft = value;
				this.OnPropertyChanged("Innerfontwidth_Canvasleft");
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00019B75 File Offset: 0x00017D75
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00019B7D File Offset: 0x00017D7D
		public double Innerfontwidth_Canvastop
		{
			get
			{
				return this.innerfontwidth_Canvastop;
			}
			set
			{
				if (this.innerfontwidth_Canvastop == value)
				{
					return;
				}
				this.innerfontwidth_Canvastop = value;
				this.OnPropertyChanged("Innerfontwidth_Canvastop");
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00019B9B File Offset: 0x00017D9B
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00019BA3 File Offset: 0x00017DA3
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.color == value)
				{
					return;
				}
				this.color = value;
				this.OnPropertyChanged("Color");
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00019BC6 File Offset: 0x00017DC6
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00019BCE File Offset: 0x00017DCE
		public Color Strokecolor
		{
			get
			{
				return this.strokecolor;
			}
			set
			{
				if (this.strokecolor == value)
				{
					return;
				}
				this.strokecolor = value;
				this.OnPropertyChanged("Strokecolor");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00019BF1 File Offset: 0x00017DF1
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00019BF9 File Offset: 0x00017DF9
		public Color Fontcolor
		{
			get
			{
				return this.fontcolor;
			}
			set
			{
				if (this.fontcolor == value)
				{
					return;
				}
				this.fontcolor = value;
				this.OnPropertyChanged("Fontcolor");
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00019C1C File Offset: 0x00017E1C
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00019C24 File Offset: 0x00017E24
		public int KeyTag
		{
			get
			{
				return this.keytag;
			}
			set
			{
				if (this.keytag == value)
				{
					return;
				}
				this.keytag = value;
				this.OnPropertyChanged("KeyTag");
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00019C42 File Offset: 0x00017E42
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00019C4A File Offset: 0x00017E4A
		public string SelectedRectStyle
		{
			get
			{
				return this.selectedRectStyle;
			}
			set
			{
				if (this.selectedRectStyle == value)
				{
					return;
				}
				this.selectedRectStyle = value;
				this.OnPropertyChanged("SelectedRectStyle");
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00019C6D File Offset: 0x00017E6D
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00019C75 File Offset: 0x00017E75
		public string OuterRectStyle
		{
			get
			{
				return this.outerRectStyle;
			}
			set
			{
				if (this.outerRectStyle == value)
				{
					return;
				}
				this.outerRectStyle = value;
				this.OnPropertyChanged("OuterRectStyle");
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00019C98 File Offset: 0x00017E98
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00019CA0 File Offset: 0x00017EA0
		public string InnerRectStyle
		{
			get
			{
				return this.innerRectStyle;
			}
			set
			{
				if (this.innerRectStyle == value)
				{
					return;
				}
				this.innerRectStyle = value;
				this.OnPropertyChanged("InnerRectStyle");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00019CC3 File Offset: 0x00017EC3
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00019CCB File Offset: 0x00017ECB
		public string InnerStyle
		{
			get
			{
				return this.innerStyle;
			}
			set
			{
				if (this.innerStyle == value)
				{
					return;
				}
				this.innerStyle = value;
				this.OnPropertyChanged("InnerStyle");
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00019CEE File Offset: 0x00017EEE
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00019CF6 File Offset: 0x00017EF6
		public Point ConnectorHotspot
		{
			get
			{
				return this.connectorHotspot;
			}
			set
			{
				if (this.connectorHotspot == value)
				{
					return;
				}
				this.connectorHotspot = value;
				this.OnPropertyChanged("ConnectorHotspot");
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00019D19 File Offset: 0x00017F19
		protected void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600029A RID: 666 RVA: 0x00019D38 File Offset: 0x00017F38
		// (remove) Token: 0x0600029B RID: 667 RVA: 0x00019D70 File Offset: 0x00017F70
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040002AC RID: 684
		private double x;

		// Token: 0x040002AD RID: 685
		private double y;

		// Token: 0x040002AE RID: 686
		private double canvas_x;

		// Token: 0x040002AF RID: 687
		private double canvas_y;

		// Token: 0x040002B0 RID: 688
		private double borderwidth;

		// Token: 0x040002B1 RID: 689
		private double borderheight;

		// Token: 0x040002B2 RID: 690
		private double v_width_Canvasleft;

		// Token: 0x040002B3 RID: 691
		private double v_width_Canvastop;

		// Token: 0x040002B4 RID: 692
		private double outerwidth;

		// Token: 0x040002B5 RID: 693
		private double outerheight;

		// Token: 0x040002B6 RID: 694
		private double innerwidth;

		// Token: 0x040002B7 RID: 695
		private double innerheight;

		// Token: 0x040002B8 RID: 696
		private double innerfontwidth;

		// Token: 0x040002B9 RID: 697
		private double innerfontheight;

		// Token: 0x040002BA RID: 698
		private double innerfontwidth_Canvasleft;

		// Token: 0x040002BB RID: 699
		private double innerfontwidth_Canvastop;

		// Token: 0x040002BC RID: 700
		private Color fontcolor;

		// Token: 0x040002BD RID: 701
		private Color color;

		// Token: 0x040002BE RID: 702
		private Color strokecolor;

		// Token: 0x040002BF RID: 703
		public int keytag;

		// Token: 0x040002C0 RID: 704
		public string selectedRectStyle;

		// Token: 0x040002C1 RID: 705
		public string outerRectStyle;

		// Token: 0x040002C2 RID: 706
		public string innerRectStyle;

		// Token: 0x040002C3 RID: 707
		public string innerStyle;

		// Token: 0x040002C4 RID: 708
		private Point connectorHotspot;
	}
}
