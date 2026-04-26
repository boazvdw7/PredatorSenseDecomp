using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x02000008 RID: 8
	public partial class MonitoringPage : UserControl
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000092 RID: 146 RVA: 0x000064B4 File Offset: 0x000046B4
		// (remove) Token: 0x06000093 RID: 147 RVA: 0x000064EC File Offset: 0x000046EC
		public event MonitoringPage.PsMouseEventHandler MouseMoveInChart_CPU;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000094 RID: 148 RVA: 0x00006524 File Offset: 0x00004724
		// (remove) Token: 0x06000095 RID: 149 RVA: 0x0000655C File Offset: 0x0000475C
		public event MonitoringPage.PsMouseEventHandler MouseMoveInChart_GPU;

		// Token: 0x06000096 RID: 150 RVA: 0x00006594 File Offset: 0x00004794
		public MonitoringPage()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			int num = 0;
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "Degree"))
			{
				num = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings", "Degree", 0U);
			}
			if (num == 0)
			{
				this.update_title_text(CommonFunction.Degree_Type.dCelsius);
			}
			else
			{
				this.update_title_text(CommonFunction.Degree_Type.dFahrenheit);
			}
			this.GPU1_title_TextBlock.Text = Startup.langRd["MUI_GPU_NAME"].ToString();
			base.Loaded += this.Window_Loaded;
			this._hoverLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
			this._hoverLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
			this._hoverLine.StrokeThickness = 1.0;
			this._hoverLine.Y1 = 0.0;
			this._hoverLine.Y2 = 99.0;
			this.CPU_chartGrid.MouseMove += this.CPU_chartGrid_MouseMove;
			this.CPU_chartGrid.MouseLeave += this.CPU_chartGrid_MouseLeave;
			this.GPU_chartGrid.MouseMove += this.GPU_chartGrid_MouseMove;
			this.GPU_chartGrid.MouseLeave += this.GPU_chartGrid_MouseLeave;
			this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : --°";
			this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : --°";
			this.CPU_Templature.Text = "--°";
			this.CPU_Usage.Text = "-";
			this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : --°";
			this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : --°";
			this.GPU_Templature.Text = "--°";
			this.GPU_Usage.Text = "-";
			this._popupGrid_CPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_CPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_CPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU2.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU2.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_GPU2.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_System.RowDefinitions.Add(new RowDefinition());
			this._popupGrid_System.RowDefinitions.Add(new RowDefinition());
			this._DiscretepopupGrid_GPU.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(16.0)
			});
			this._DiscretepopupGrid_GPU.RowDefinitions.Add(new RowDefinition());
			this._timeTextBlockInGrid_CPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._timeTextBlockInGrid_CPU.VerticalAlignment = VerticalAlignment.Center;
			this._timeTextBlockInGrid_CPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 153, 153, 153));
			this._timeTextBlockInGrid_CPU.FontSize = 14.0;
			this._timeTextBlockInGrid_CPU.TextWrapping = TextWrapping.NoWrap;
			this._tempTextBlockInGrid_CPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._tempTextBlockInGrid_CPU.VerticalAlignment = VerticalAlignment.Center;
			this._tempTextBlockInGrid_CPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 230, 0, 0));
			this._tempTextBlockInGrid_CPU.FontSize = 14.0;
			this._tempTextBlockInGrid_CPU.TextWrapping = TextWrapping.NoWrap;
			this._loadTextBlockInGrid_CPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._loadTextBlockInGrid_CPU.VerticalAlignment = VerticalAlignment.Center;
			this._loadTextBlockInGrid_CPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, 120, 0));
			this._loadTextBlockInGrid_CPU.FontSize = 14.0;
			this._loadTextBlockInGrid_CPU.TextWrapping = TextWrapping.NoWrap;
			this._timeTextBlockInGrid_GPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._timeTextBlockInGrid_GPU.VerticalAlignment = VerticalAlignment.Center;
			this._timeTextBlockInGrid_GPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 153, 153, 153));
			this._timeTextBlockInGrid_GPU.FontSize = 12.5;
			this._timeTextBlockInGrid_GPU.TextWrapping = TextWrapping.NoWrap;
			this._tempTextBlockInGrid_GPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._tempTextBlockInGrid_GPU.VerticalAlignment = VerticalAlignment.Center;
			this._tempTextBlockInGrid_GPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 230, 0, 0));
			this._tempTextBlockInGrid_GPU.FontSize = 12.5;
			this._tempTextBlockInGrid_GPU.TextWrapping = TextWrapping.NoWrap;
			this._loadTextBlockInGrid_GPU.HorizontalAlignment = HorizontalAlignment.Center;
			this._loadTextBlockInGrid_GPU.VerticalAlignment = VerticalAlignment.Center;
			this._loadTextBlockInGrid_GPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, 120, 0));
			this._loadTextBlockInGrid_GPU.FontSize = 12.5;
			this._loadTextBlockInGrid_GPU.TextWrapping = TextWrapping.NoWrap;
			this._timeTextBlockInGrid_DiscreteGPU.HorizontalAlignment = HorizontalAlignment.Left;
			this._timeTextBlockInGrid_DiscreteGPU.VerticalAlignment = VerticalAlignment.Top;
			this._timeTextBlockInGrid_DiscreteGPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 153, 153, 153));
			this._timeTextBlockInGrid_DiscreteGPU.FontSize = 12.5;
			this._timeTextBlockInGrid_DiscreteGPU.TextWrapping = TextWrapping.NoWrap;
			this._noticeTextBlockInGrid_DiscreteGPU.HorizontalAlignment = HorizontalAlignment.Left;
			this._noticeTextBlockInGrid_DiscreteGPU.VerticalAlignment = VerticalAlignment.Top;
			this._noticeTextBlockInGrid_DiscreteGPU.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			this._noticeTextBlockInGrid_DiscreteGPU.FontSize = 12.5;
			this._noticeTextBlockInGrid_DiscreteGPU.Width = 151.57;
			this._noticeTextBlockInGrid_DiscreteGPU.TextWrapping = TextWrapping.Wrap;
			Grid.SetRow(this._timeTextBlockInGrid_CPU, 0);
			this._popupGrid_CPU.Children.Add(this._timeTextBlockInGrid_CPU);
			Grid.SetRow(this._tempTextBlockInGrid_CPU, 1);
			this._popupGrid_CPU.Children.Add(this._tempTextBlockInGrid_CPU);
			Grid.SetRow(this._loadTextBlockInGrid_CPU, 2);
			this._popupGrid_CPU.Children.Add(this._loadTextBlockInGrid_CPU);
			Grid.SetRow(this._timeTextBlockInGrid_GPU, 0);
			this._popupGrid_GPU.Children.Add(this._timeTextBlockInGrid_GPU);
			Grid.SetRow(this._tempTextBlockInGrid_GPU, 1);
			this._popupGrid_GPU.Children.Add(this._tempTextBlockInGrid_GPU);
			Grid.SetRow(this._loadTextBlockInGrid_GPU, 2);
			this._popupGrid_GPU.Children.Add(this._loadTextBlockInGrid_GPU);
			Grid.SetRow(this._timeTextBlockInGrid_DiscreteGPU, 0);
			this._DiscretepopupGrid_GPU.Children.Add(this._timeTextBlockInGrid_DiscreteGPU);
			Grid.SetRow(this._noticeTextBlockInGrid_DiscreteGPU, 1);
			this._DiscretepopupGrid_GPU.Children.Add(this._noticeTextBlockInGrid_DiscreteGPU);
			this.AddLineDefinition_CPU("#E60000");
			this.AddLineDefinition_CPU("#FF7800");
			this.AddLineDefinition_GPU("#E60000");
			this.AddLineDefinition_GPU("#FF7800");
			this.AddLineDefinition_GPU2("#E60000");
			this.AddLineDefinition_GPU2("#FF7800");
			this.AddLineDefinition_System("#E60000");
			this.MouseMoveInChart_CPU += this.CPU_lineChart_MouseMoveInChart;
			this.MouseMoveInChart_GPU += this.GPU_lineChart_MouseMoveInChart;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006F7C File Offset: 0x0000517C
		public void update_title_text(CommonFunction.Degree_Type degree_type)
		{
			if (degree_type == CommonFunction.Degree_Type.dCelsius)
			{
				this.Unit_info_TextBlock.Text = string.Concat(new string[]
				{
					Startup.langRd["MUI_TEMPERATURE"].ToString(),
					" (",
					CommonFunction.celsius_name,
					") / ",
					Startup.langRd["MUI_Loading"].ToString(),
					" (%)"
				});
			}
			else
			{
				this.Unit_info_TextBlock.Text = string.Concat(new string[]
				{
					Startup.langRd["MUI_TEMPERATURE"].ToString(),
					" (",
					CommonFunction.fahrenheit_name,
					") / ",
					Startup.langRd["MUI_Loading"].ToString(),
					" (%)"
				});
			}
			this.change_min_max_temperature_text(degree_type);
			this.Degree_Type = degree_type;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00007068 File Offset: 0x00005268
		private void change_min_max_temperature_text(CommonFunction.Degree_Type degree_type)
		{
			if (degree_type == CommonFunction.Degree_Type.dCelsius)
			{
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + this._minTemp_CPU.ToString() + "°";
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + this._maxTemp_CPU.ToString() + "°";
				if (this._minTemp_GPU != 200f && this._maxTemp_GPU != 0f)
				{
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + this._minTemp_GPU.ToString() + "°";
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + this._maxTemp_GPU.ToString() + "°";
					return;
				}
			}
			else
			{
				int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_CPU);
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + num.ToString() + "°";
				num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_CPU);
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + num.ToString() + "°";
				if (this._minTemp_GPU != 200f && this._maxTemp_GPU != 0f)
				{
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_GPU);
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + num.ToString() + "°";
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_GPU);
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + num.ToString() + "°";
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000072AC File Offset: 0x000054AC
		public void AddLineDefinition_CPU(string colorValue)
		{
			this._linesData_CPU.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			this._linesData_CPU_F.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			Polyline polyline = new Polyline();
			polyline.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polyline.StrokeThickness = 1.0;
			this._lines_CPU.Add(polyline);
			Polygon polygon = new Polygon();
			polygon.Stroke = Brushes.Transparent;
			polygon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polygon.Opacity = 0.15;
			this._linesShadow_CPU.Add(polygon);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00007370 File Offset: 0x00005570
		public void AddLineDefinition_GPU(string colorValue)
		{
			this._linesData_GPU.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			this._linesData_GPU_F.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			Polyline polyline = new Polyline();
			polyline.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polyline.StrokeThickness = 1.0;
			this._lines_GPU.Add(polyline);
			Polygon polygon = new Polygon();
			polygon.Stroke = Brushes.Transparent;
			polygon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polygon.Opacity = 0.15;
			this._linesShadow_GPU.Add(polygon);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00007434 File Offset: 0x00005634
		public void AddLineDefinition_GPU2(string colorValue)
		{
			this._linesData_GPU2.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			this._linesData_GPU2_F.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			Polyline polyline = new Polyline();
			polyline.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polyline.StrokeThickness = 1.0;
			this._lines_GPU2.Add(polyline);
			Polygon polygon = new Polygon();
			polygon.Stroke = Brushes.Transparent;
			polygon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polygon.Opacity = 0.15;
			this._linesShadow_GPU2.Add(polygon);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000074F8 File Offset: 0x000056F8
		public void AddLineDefinition_System(string colorValue)
		{
			this._linesData_System.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			this._linesData_System_F.Add(Enumerable.Repeat<float>(float.NaN, 720).ToArray<float>());
			Polyline polyline = new Polyline();
			polyline.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polyline.StrokeThickness = 1.0;
			this._lines_System.Add(polyline);
			Polygon polygon = new Polygon();
			polygon.Stroke = Brushes.Transparent;
			polygon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorValue));
			polygon.Opacity = 0.15;
			this._linesShadow_System.Add(polygon);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000075BC File Offset: 0x000057BC
		public void RemoveLineDefinition(int index)
		{
			this._linesData_CPU.RemoveAt(index);
			this._linesData_GPU.RemoveAt(index);
			this._linesData_GPU2.RemoveAt(index);
			this._linesData_System.RemoveAt(index);
			this._lines_CPU.RemoveAt(index);
			this._lines_GPU.RemoveAt(index);
			this._lines_GPU2.RemoveAt(index);
			this._lines_System.RemoveAt(index);
			this._linesShadow_CPU.RemoveAt(index);
			this._linesShadow_GPU.RemoveAt(index);
			this._linesShadow_GPU2.RemoveAt(index);
			this._linesShadow_System.RemoveAt(index);
			this.DrawLines_CPU();
			this.DrawLines_GPU();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007668 File Offset: 0x00005868
		public void PushData_CPU(DateTime time, params float[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException();
			}
			if (data.Length != this._linesData_CPU.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (data.Length != this._linesData_CPU_F.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			this._times_CPU[this._oldestDataIdx_CPU] = time;
			for (int i = 0; i < this._linesData_CPU.Count; i++)
			{
				this._linesData_CPU[i][this._oldestDataIdx_CPU] = data[i];
			}
			this._linesData_CPU_F[0][this._oldestDataIdx_CPU] = (float)CommonFunction.transfer_celsius_to_fahrenheit_name((int)data[0]);
			this._linesData_CPU_F[1][this._oldestDataIdx_CPU] = data[1];
			if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
			{
				this.CPU_Templature.Text = data[0].ToString() + "°";
			}
			else
			{
				this.CPU_Templature.Text = CommonFunction.transfer_celsius_to_fahrenheit_name((int)data[0]).ToString() + "°";
			}
			this.CPU_Usage.Text = data[1].ToString() + " ";
			this._oldestDataIdx_CPU++;
			if (this._oldestDataIdx_CPU >= 720)
			{
				this._oldestDataIdx_CPU -= 720;
			}
			if (data[0] > this._maxTemp_CPU)
			{
				this._maxTemp_CPU = data[0];
			}
			if (data[0] < this._minTemp_CPU)
			{
				this._minTemp_CPU = data[0];
			}
			if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
			{
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + this._minTemp_CPU.ToString() + "°";
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + this._maxTemp_CPU.ToString() + "°";
			}
			else
			{
				int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_CPU);
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + num.ToString() + "°";
				num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_CPU);
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + num.ToString() + "°";
			}
			this.DrawLines_CPU();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000078F4 File Offset: 0x00005AF4
		public void PushData_GPU(DateTime time, params float[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException();
			}
			if (data.Length != this._linesData_GPU.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (data.Length != this._linesData_GPU_F.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			this._times_GPU[this._oldestDataIdx_GPU] = time;
			for (int i = 0; i < this._linesData_GPU.Count; i++)
			{
				this._linesData_GPU[i][this._oldestDataIdx_GPU] = data[i];
			}
			this._linesData_GPU_F[0][this._oldestDataIdx_GPU] = (float)(((int)data[0] > 0) ? CommonFunction.transfer_celsius_to_fahrenheit_name((int)data[0]) : 0);
			this._linesData_GPU_F[1][this._oldestDataIdx_GPU] = data[1];
			if (data[0] > 0f)
			{
				if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
				{
					this.GPU_Templature.Text = data[0].ToString() + "°";
				}
				else
				{
					this.GPU_Templature.Text = CommonFunction.transfer_celsius_to_fahrenheit_name((int)data[0]).ToString() + "°";
				}
				this.GPU_Usage.Text = data[1].ToString() + " ";
			}
			else
			{
				this.GPU_Templature.Text = "--°";
				this.GPU_Usage.Text = "-- ";
			}
			this._oldestDataIdx_GPU++;
			if (this._oldestDataIdx_GPU >= 720)
			{
				this._oldestDataIdx_GPU -= 720;
			}
			if (data[0] > 0f)
			{
				if (data[0] > this._maxTemp_GPU)
				{
					this._maxTemp_GPU = data[0];
				}
				if (data[0] < this._minTemp_GPU)
				{
					this._minTemp_GPU = data[0];
				}
				if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
				{
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + this._minTemp_GPU.ToString() + "°";
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + this._maxTemp_GPU.ToString() + "°";
				}
				else
				{
					int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_GPU);
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : " + num.ToString() + "°";
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_GPU);
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : " + num.ToString() + "°";
				}
			}
			this.DrawLines_GPU();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007BC1 File Offset: 0x00005DC1
		public void PushData_CPU(params float[] data)
		{
			this.PushData_CPU(DateTime.Now, data);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00007BCF File Offset: 0x00005DCF
		public void PushData_GPU(params float[] data)
		{
			this.PushData_GPU(DateTime.Now, data);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007BDD File Offset: 0x00005DDD
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.GetMonitoringUICoordinate();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007BE8 File Offset: 0x00005DE8
		private void GetMonitoringUICoordinate()
		{
			if (!this.MonitoringUICoordinate.X.Equals(double.NaN) && !this.MonitoringUICoordinate.Y.Equals(double.NaN))
			{
				return;
			}
			Window window = Window.GetWindow(this);
			this.MonitoringUICoordinate = base.TranslatePoint(new Point(window.Left, window.Top), Window.GetWindow(this));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007C5C File Offset: 0x00005E5C
		private void CPU_lineChart_MouseMoveInChart(object sender, MonitoringPage.MouseEventArgsWithChartData e)
		{
			MonitoringPage monitoringPage = sender as MonitoringPage;
			if (monitoringPage == null)
			{
				return;
			}
			this._timeTextBlockInGrid_CPU.Text = e.Time.ToString("HH:mm:ss");
			this._tempTextBlockInGrid_CPU.Text = e.ChartData[0] + "°";
			this._loadTextBlockInGrid_CPU.Text = e.ChartData[1] + "%";
			monitoringPage.CPU_dataPopupContent.Content = this._popupGrid_CPU;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007CE8 File Offset: 0x00005EE8
		private void GPU_lineChart_MouseMoveInChart(object sender, MonitoringPage.MouseEventArgsWithChartData e)
		{
			MonitoringPage monitoringPage = sender as MonitoringPage;
			if (monitoringPage == null)
			{
				return;
			}
			if (e.ChartData[0] > 0f)
			{
				this._timeTextBlockInGrid_GPU.Text = e.Time.ToString("HH:mm:ss");
				this._tempTextBlockInGrid_GPU.Text = e.ChartData[0] + "°";
				this._loadTextBlockInGrid_GPU.Text = e.ChartData[1] + "%";
				monitoringPage.GPU_dataPopupContent.Content = this._popupGrid_GPU;
				return;
			}
			this._timeTextBlockInGrid_DiscreteGPU.Text = e.Time.ToString("HH:mm:ss");
			this._noticeTextBlockInGrid_DiscreteGPU.Text = Startup.langRd["MUI_GPU_Idle"].ToString();
			monitoringPage.GPU_DiscretedataPopupContent.Content = this._DiscretepopupGrid_GPU;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007DD4 File Offset: 0x00005FD4
		private void Clear()
		{
			for (int i = 0; i < this._linesData_GPU2.Count; i++)
			{
				this._linesShadow_GPU2[i].Points.Clear();
			}
			for (int j = 0; j < this._linesData_GPU2.Count; j++)
			{
				this._lines_GPU2[j].Points.Clear();
			}
			for (int k = 0; k < this._linesData_GPU2.Count; k++)
			{
				for (int l = 0; l < 720; l++)
				{
					this._linesData_GPU2[k][l] = float.NaN;
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007E74 File Offset: 0x00006074
		private void DrawLines_CPU()
		{
			this.CPU_chartGrid.Children.Clear();
			for (int i = 0; i < this._linesData_CPU.Count; i++)
			{
				this._linesShadow_CPU[i].Points.Clear();
				this._linesShadow_CPU[i].Points.Add(this.LineShadowBottomRightPoint);
				this._linesShadow_CPU[i].Points.Add(new Point((double)(float.IsNaN(this._linesData_CPU[i][this._oldestDataIdx_CPU]) ? (720 - this._oldestDataIdx_CPU) : 0), 99.0));
				int num = 0;
				int num2 = this._oldestDataIdx_CPU;
				do
				{
					if (!float.IsNaN(this._linesData_CPU[i][num2]))
					{
						float num3 = ((this._linesData_CPU[i][num2] < 0f) ? 0f : ((this._linesData_CPU[i][num2] > 100f) ? 100f : this._linesData_CPU[i][num2]));
						num3 = 100f - num3 + 1f;
						this._linesShadow_CPU[i].Points.Add(new Point((double)num, (double)num3));
					}
					num++;
					num2++;
					if (num2 >= 720)
					{
						num2 -= 720;
					}
				}
				while (num2 != this._oldestDataIdx_CPU);
				this.CPU_chartGrid.Children.Add(this._linesShadow_CPU[i]);
			}
			for (int j = 0; j < this._linesData_CPU.Count; j++)
			{
				this._lines_CPU[j].Points.Clear();
				int num4 = 0;
				int num5 = this._oldestDataIdx_CPU;
				do
				{
					if (!float.IsNaN(this._linesData_CPU[j][num5]))
					{
						float num6 = ((this._linesData_CPU[j][num5] < 0f) ? 0f : ((this._linesData_CPU[j][num5] > 100f) ? 100f : this._linesData_CPU[j][num5]));
						num6 = 100f - num6 + 1f;
						this._lines_CPU[j].Points.Add(new Point((double)num4, (double)num6));
					}
					num4++;
					num5++;
					if (num5 >= 720)
					{
						num5 -= 720;
					}
				}
				while (num5 != this._oldestDataIdx_CPU);
				this.CPU_chartGrid.Children.Add(this._lines_CPU[j]);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00008128 File Offset: 0x00006328
		private void DrawLines_GPU()
		{
			this.GPU_chartGrid.Children.Clear();
			for (int i = 0; i < this._linesData_GPU.Count; i++)
			{
				this._linesShadow_GPU[i].Points.Clear();
				this._linesShadow_GPU[i].Points.Add(this.LineShadowBottomRightPoint);
				this._linesShadow_GPU[i].Points.Add(new Point((double)(float.IsNaN(this._linesData_GPU[i][this._oldestDataIdx_GPU]) ? (720 - this._oldestDataIdx_GPU) : 0), 99.0));
				int num = 0;
				int num2 = this._oldestDataIdx_GPU;
				do
				{
					if (!float.IsNaN(this._linesData_GPU[i][num2]))
					{
						float num3 = ((this._linesData_GPU[i][num2] < 0f) ? 0f : ((this._linesData_GPU[i][num2] > 100f) ? 100f : this._linesData_GPU[i][num2]));
						num3 = 100f - num3 + 1f;
						this._linesShadow_GPU[i].Points.Add(new Point((double)num, (double)num3));
					}
					num++;
					num2++;
					if (num2 >= 720)
					{
						num2 -= 720;
					}
				}
				while (num2 != this._oldestDataIdx_GPU);
				this.GPU_chartGrid.Children.Add(this._linesShadow_GPU[i]);
			}
			for (int j = 0; j < this._linesData_GPU.Count; j++)
			{
				this._lines_GPU[j].Points.Clear();
				int num4 = 0;
				int num5 = this._oldestDataIdx_GPU;
				do
				{
					if (!float.IsNaN(this._linesData_GPU[j][num5]))
					{
						float num6 = ((this._linesData_GPU[j][num5] < 0f) ? 0f : ((this._linesData_GPU[j][num5] > 100f) ? 100f : this._linesData_GPU[j][num5]));
						num6 = 100f - num6 + 1f;
						this._lines_GPU[j].Points.Add(new Point((double)num4, (double)num6));
					}
					num4++;
					num5++;
					if (num5 >= 720)
					{
						num5 -= 720;
					}
				}
				while (num5 != this._oldestDataIdx_GPU);
				this.GPU_chartGrid.Children.Add(this._lines_GPU[j]);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000083DC File Offset: 0x000065DC
		private void CPU_chartGrid_MouseMove(object sender, MouseEventArgs e)
		{
			Point position = e.GetPosition(this.CPU_chartGrid);
			if (position.X == 0.0)
			{
				return;
			}
			if (this._linesData_CPU.Count <= 0)
			{
				return;
			}
			if (this._linesData_CPU_F.Count <= 0)
			{
				return;
			}
			int num = this._oldestDataIdx_CPU + (int)position.X;
			if (float.IsNaN((num < 720) ? this._linesData_CPU[0][num] : this._linesData_CPU[0][num - 720]) || (int)position.X >= 720)
			{
				this.CPU_chartGrid.Children.Remove(this._hoverLine);
				if (this.CPU_dataPopup.IsOpen)
				{
					this.CPU_dataPopup.IsOpen = false;
				}
				return;
			}
			this.CPU_chartGrid.Children.Remove(this._hoverLine);
			this._hoverLine.X1 = position.X;
			this._hoverLine.X2 = position.X;
			this.CPU_chartGrid.Children.Add(this._hoverLine);
			DateTime dateTime = ((num < 720) ? this._times_CPU[num] : this._times_CPU[num - 720]);
			float[] array = new float[this._linesData_CPU.Count];
			if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
			{
				for (int i = 0; i < this._linesData_CPU.Count; i++)
				{
					array[i] = ((num < 720) ? this._linesData_CPU[i][num] : this._linesData_CPU[i][num - 720]);
				}
			}
			else
			{
				for (int j = 0; j < this._linesData_CPU_F.Count; j++)
				{
					array[j] = ((num < 720) ? this._linesData_CPU_F[j][num] : this._linesData_CPU_F[j][num - 720]);
				}
			}
			this.MouseMoveInChart_CPU(this, new MonitoringPage.MouseEventArgsWithChartData(e.MouseDevice, e.Timestamp, e.StylusDevice, dateTime, array));
			this.CPU_dataPopup.HorizontalOffset = position.X + 10.0;
			this.CPU_dataPopup.VerticalOffset = position.Y + 15.0;
			if (!this.CPU_dataPopup.IsOpen)
			{
				this.CPU_dataPopup.IsOpen = true;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00008658 File Offset: 0x00006858
		private void GPU_chartGrid_MouseMove(object sender, MouseEventArgs e)
		{
			this.g_GPU_mouse_enter = true;
			if (this.GPU_DiscretePopup.Visibility == Visibility.Visible)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Collapsed;
			}
			Point position = e.GetPosition(this.GPU_chartGrid);
			if (position.X == 0.0)
			{
				return;
			}
			if (this._linesData_GPU.Count <= 0)
			{
				return;
			}
			if (this._linesData_GPU_F.Count <= 0)
			{
				return;
			}
			int num = this._oldestDataIdx_GPU + (int)position.X;
			if (float.IsNaN((num < 720) ? this._linesData_GPU[0][num] : this._linesData_GPU[0][num - 720]) || (int)position.X >= 720)
			{
				this.GPU_chartGrid.Children.Remove(this._hoverLine);
				if (this.GPU_dataPopup.IsOpen)
				{
					this.GPU_dataPopup.IsOpen = false;
				}
				if (this.GPU_DiscretedataPopup.IsOpen)
				{
					this.GPU_DiscretedataPopup.IsOpen = false;
				}
				return;
			}
			this.GPU_chartGrid.Children.Remove(this._hoverLine);
			this._hoverLine.X1 = position.X;
			this._hoverLine.X2 = position.X;
			this.GPU_chartGrid.Children.Add(this._hoverLine);
			DateTime dateTime = ((num < 720) ? this._times_GPU[num] : this._times_GPU[num - 720]);
			float[] array = new float[this._linesData_GPU.Count];
			if (this.Degree_Type == CommonFunction.Degree_Type.dCelsius)
			{
				for (int i = 0; i < this._linesData_GPU.Count; i++)
				{
					array[i] = ((num < 720) ? this._linesData_GPU[i][num] : this._linesData_GPU[i][num - 720]);
				}
			}
			else
			{
				for (int j = 0; j < this._linesData_GPU_F.Count; j++)
				{
					array[j] = ((num < 720) ? this._linesData_GPU_F[j][num] : this._linesData_GPU_F[j][num - 720]);
				}
			}
			this.MouseMoveInChart_GPU(this, new MonitoringPage.MouseEventArgsWithChartData(e.MouseDevice, e.Timestamp, e.StylusDevice, dateTime, array));
			this.GPU_dataPopup.HorizontalOffset = position.X + 10.0;
			this.GPU_dataPopup.VerticalOffset = position.Y + 15.0;
			this.GPU_DiscretedataPopup.HorizontalOffset = position.X + 10.0;
			this.GPU_DiscretedataPopup.VerticalOffset = position.Y + 15.0;
			if (array[0] > 0f)
			{
				if (!this.GPU_dataPopup.IsOpen)
				{
					this.GPU_dataPopup.IsOpen = true;
				}
				if (this.GPU_DiscretedataPopup.IsOpen)
				{
					this.GPU_DiscretedataPopup.IsOpen = false;
					return;
				}
			}
			else
			{
				if (!this.GPU_DiscretedataPopup.IsOpen)
				{
					this.GPU_DiscretedataPopup.IsOpen = true;
				}
				if (this.GPU_dataPopup.IsOpen)
				{
					this.GPU_dataPopup.IsOpen = false;
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00008999 File Offset: 0x00006B99
		private void CPU_chartGrid_MouseLeave(object sender, MouseEventArgs e)
		{
			this.CPU_dataPopup.IsOpen = false;
			this.CPU_chartGrid.Children.Remove(this._hoverLine);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000089C0 File Offset: 0x00006BC0
		private void GPU_chartGrid_MouseLeave(object sender, MouseEventArgs e)
		{
			this.GPU_dataPopup.IsOpen = false;
			this.GPU_DiscretedataPopup.IsOpen = false;
			this.GPU_chartGrid.Children.Remove(this._hoverLine);
			this.g_GPU_mouse_enter = false;
			if (!this.gpu_enable_flag)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00008A16 File Offset: 0x00006C16
		public ContentControl CPU_DataPopupContent
		{
			get
			{
				return this.CPU_dataPopupContent;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00008A1E File Offset: 0x00006C1E
		public ContentControl GPU_DataPopupContent
		{
			get
			{
				return this.GPU_dataPopupContent;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00008A26 File Offset: 0x00006C26
		public ContentControl GPU_discretedataPopupContent
		{
			get
			{
				return this.GPU_DiscretedataPopupContent;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00008A30 File Offset: 0x00006C30
		private void Common_Popup_Closed(object sender, EventArgs e)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Closed(sender, e);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00008A54 File Offset: 0x00006C54
		private void Common_Popup_Opened(object sender, EventArgs e)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Opened(sender, e);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00008A78 File Offset: 0x00006C78
		public void update_discrete_gpu_state(bool state)
		{
			Visibility visibility = Visibility.Collapsed;
			this.gpu_enable_flag = state;
			if (!this.gpu_enable_flag && !this.g_GPU_mouse_enter)
			{
				visibility = Visibility.Visible;
			}
			if (this.GPU_DiscretePopup.Visibility != visibility)
			{
				this.GPU_DiscretePopup.Visibility = visibility;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008ABA File Offset: 0x00006CBA
		private void GPU_DiscretePopup_MouseMove(object sender, MouseEventArgs e)
		{
			this.g_GPU_mouse_enter = true;
			if (this.GPU_DiscretePopup.Visibility == Visibility.Visible)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00008ADC File Offset: 0x00006CDC
		private void GPU_DiscretePopup_MouseLeave(object sender, MouseEventArgs e)
		{
			this.g_GPU_mouse_enter = false;
			if (!this.gpu_enable_flag)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x04000082 RID: 130
		private const int MaxNumOfData = 720;

		// Token: 0x04000085 RID: 133
		private readonly Point LineShadowBottomRightPoint = new Point(719.0, 99.0);

		// Token: 0x04000086 RID: 134
		private int _oldestDataIdx_CPU;

		// Token: 0x04000087 RID: 135
		private int _oldestDataIdx_GPU;

		// Token: 0x04000088 RID: 136
		private float _maxTemp_CPU;

		// Token: 0x04000089 RID: 137
		private float _maxTemp_GPU;

		// Token: 0x0400008A RID: 138
		private float _minTemp_CPU = 200f;

		// Token: 0x0400008B RID: 139
		private float _minTemp_GPU = 200f;

		// Token: 0x0400008C RID: 140
		private DateTime[] _times_CPU = new DateTime[720];

		// Token: 0x0400008D RID: 141
		private DateTime[] _times_GPU = new DateTime[720];

		// Token: 0x0400008E RID: 142
		private DateTime[] _times_GPU2 = new DateTime[720];

		// Token: 0x0400008F RID: 143
		private DateTime[] _times_System = new DateTime[720];

		// Token: 0x04000090 RID: 144
		private List<float[]> _linesData_CPU = new List<float[]>();

		// Token: 0x04000091 RID: 145
		private List<float[]> _linesData_GPU = new List<float[]>();

		// Token: 0x04000092 RID: 146
		private List<float[]> _linesData_GPU2 = new List<float[]>();

		// Token: 0x04000093 RID: 147
		private List<float[]> _linesData_System = new List<float[]>();

		// Token: 0x04000094 RID: 148
		private List<float[]> _linesData_CPU_F = new List<float[]>();

		// Token: 0x04000095 RID: 149
		private List<float[]> _linesData_GPU_F = new List<float[]>();

		// Token: 0x04000096 RID: 150
		private List<float[]> _linesData_GPU2_F = new List<float[]>();

		// Token: 0x04000097 RID: 151
		private List<float[]> _linesData_System_F = new List<float[]>();

		// Token: 0x04000098 RID: 152
		private List<Polyline> _lines_CPU = new List<Polyline>();

		// Token: 0x04000099 RID: 153
		private List<Polyline> _lines_GPU = new List<Polyline>();

		// Token: 0x0400009A RID: 154
		private List<Polyline> _lines_GPU2 = new List<Polyline>();

		// Token: 0x0400009B RID: 155
		private List<Polyline> _lines_System = new List<Polyline>();

		// Token: 0x0400009C RID: 156
		private List<Polygon> _linesShadow_CPU = new List<Polygon>();

		// Token: 0x0400009D RID: 157
		private List<Polygon> _linesShadow_GPU = new List<Polygon>();

		// Token: 0x0400009E RID: 158
		private List<Polygon> _linesShadow_GPU2 = new List<Polygon>();

		// Token: 0x0400009F RID: 159
		private List<Polygon> _linesShadow_System = new List<Polygon>();

		// Token: 0x040000A0 RID: 160
		private Line _hoverLine = new Line();

		// Token: 0x040000A1 RID: 161
		private Grid _popupGrid_CPU = new Grid();

		// Token: 0x040000A2 RID: 162
		private Grid _popupGrid_GPU = new Grid();

		// Token: 0x040000A3 RID: 163
		private Grid _popupGrid_GPU2 = new Grid();

		// Token: 0x040000A4 RID: 164
		private Grid _popupGrid_System = new Grid();

		// Token: 0x040000A5 RID: 165
		private Grid _DiscretepopupGrid_GPU = new Grid();

		// Token: 0x040000A6 RID: 166
		private TextBlock _timeTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x040000A7 RID: 167
		private TextBlock _tempTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x040000A8 RID: 168
		private TextBlock _loadTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x040000A9 RID: 169
		private TextBlock _timeTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x040000AA RID: 170
		private TextBlock _tempTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x040000AB RID: 171
		private TextBlock _loadTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x040000AC RID: 172
		private TextBlock _timeTextBlockInGrid_DiscreteGPU = new TextBlock();

		// Token: 0x040000AD RID: 173
		private TextBlock _noticeTextBlockInGrid_DiscreteGPU = new TextBlock();

		// Token: 0x040000AE RID: 174
		private Point MonitoringUICoordinate = new Point(double.NaN, double.NaN);

		// Token: 0x040000AF RID: 175
		private CommonFunction.Degree_Type Degree_Type;

		// Token: 0x040000B0 RID: 176
		public bool gpu_enable_flag;

		// Token: 0x040000B1 RID: 177
		private bool g_GPU_mouse_enter;

		// Token: 0x02000009 RID: 9
		public class MouseEventArgsWithChartData : MouseEventArgs
		{
			// Token: 0x060000B7 RID: 183 RVA: 0x00008CE2 File Offset: 0x00006EE2
			public MouseEventArgsWithChartData(MouseDevice mouse, int timestamp, StylusDevice stylusDevice, DateTime time, float[] data)
				: base(mouse, timestamp, stylusDevice)
			{
				this._time = time;
				this._data = data;
			}

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x060000B8 RID: 184 RVA: 0x00008CFD File Offset: 0x00006EFD
			public DateTime Time
			{
				get
				{
					return this._time;
				}
			}

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x00008D05 File Offset: 0x00006F05
			public float[] ChartData
			{
				get
				{
					return this._data;
				}
			}

			// Token: 0x040000C8 RID: 200
			private DateTime _time;

			// Token: 0x040000C9 RID: 201
			private float[] _data;
		}

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x060000BB RID: 187
		public delegate void PsMouseEventHandler(object sender, MonitoringPage.MouseEventArgsWithChartData e);
	}
}
