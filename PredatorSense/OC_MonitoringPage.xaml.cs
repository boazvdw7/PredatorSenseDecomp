using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x0200000F RID: 15
	public partial class OC_MonitoringPage : UserControl
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600010D RID: 269 RVA: 0x0000BF08 File Offset: 0x0000A108
		// (remove) Token: 0x0600010E RID: 270 RVA: 0x0000BF40 File Offset: 0x0000A140
		public event OC_MonitoringPage.PsMouseEventHandler MouseMoveInChart_CPU;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600010F RID: 271 RVA: 0x0000BF78 File Offset: 0x0000A178
		// (remove) Token: 0x06000110 RID: 272 RVA: 0x0000BFB0 File Offset: 0x0000A1B0
		public event OC_MonitoringPage.PsMouseEventHandler MouseMoveInChart_GPU;

		// Token: 0x06000111 RID: 273 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public OC_MonitoringPage()
		{
			this.InitializeComponent();
			this.CPU_MinTemplature_value.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_MaxTemplature_value.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_MinTemplature_value.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_MaxTemplature_value.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_textblock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU1_title_TextBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_Usage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_Usage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.GPU_Templature.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_Templature.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			this.CPU_frequency_TextBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
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
			this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
			this.CPU_MinTemplature_value.Text = "--°";
			this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
			this.CPU_Templature.Text = "--°";
			this.CPU_MaxTemplature.Text = "--°";
			this.CPU_Usage.Text = "-";
			this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
			this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
			this.GPU_Templature.Text = "--°";
			this.GPU_MinTemplature_value.Text = "--°";
			this.GPU_MaxTemplature_value.Text = "--°";
			this.GPU_Usage.Text = "-";
			this.CPU_core_clock_TextBlock.Text = Startup.langRd["MUI_GPU_NAME"].ToString() + " " + Startup.langRd["MUI_Core_Clock"].ToString();
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

		// Token: 0x06000112 RID: 274 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
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

		// Token: 0x06000113 RID: 275 RVA: 0x0000CC8C File Offset: 0x0000AE8C
		private void change_min_max_temperature_text(CommonFunction.Degree_Type degree_type)
		{
			if (degree_type == CommonFunction.Degree_Type.dCelsius)
			{
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
				this.CPU_MinTemplature_value.Text = this._minTemp_CPU.ToString() + "°";
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
				this.CPU_MaxTemplature_value.Text = this._maxTemp_CPU.ToString() + "°";
				if (this._minTemp_GPU != 200f && this._maxTemp_GPU != 0f)
				{
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
					this.GPU_MinTemplature_value.Text = this._minTemp_GPU.ToString() + "°";
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
					this.GPU_MaxTemplature_value.Text = this._maxTemp_GPU.ToString() + "°";
					return;
				}
			}
			else
			{
				int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_CPU);
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
				this.CPU_MinTemplature_value.Text = num.ToString() + "°";
				num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_CPU);
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
				this.CPU_MaxTemplature_value.Text = num.ToString() + "°";
				if (this._minTemp_GPU != 200f && this._maxTemp_GPU != 0f)
				{
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_GPU);
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
					this.GPU_MinTemplature_value.Text = num.ToString() + "°";
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_GPU);
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
					this.GPU_MaxTemplature_value.Text = num.ToString() + "°";
				}
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000CF50 File Offset: 0x0000B150
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

		// Token: 0x06000115 RID: 277 RVA: 0x0000D014 File Offset: 0x0000B214
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

		// Token: 0x06000116 RID: 278 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
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

		// Token: 0x06000117 RID: 279 RVA: 0x0000D19C File Offset: 0x0000B39C
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

		// Token: 0x06000118 RID: 280 RVA: 0x0000D260 File Offset: 0x0000B460
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

		// Token: 0x06000119 RID: 281 RVA: 0x0000D30C File Offset: 0x0000B50C
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
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
				this.CPU_MinTemplature_value.Text = this._minTemp_CPU.ToString() + "°";
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
				this.CPU_MaxTemplature_value.Text = this._maxTemp_CPU.ToString() + "°";
			}
			else
			{
				int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_CPU);
				this.CPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
				this.CPU_MinTemplature_value.Text = num.ToString() + "°";
				num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_CPU);
				this.CPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
				this.CPU_MaxTemplature_value.Text = num.ToString() + "°";
			}
			this.DrawLines_CPU();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000D5DC File Offset: 0x0000B7DC
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
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
					this.GPU_MinTemplature_value.Text = this._minTemp_GPU.ToString() + "°";
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
					this.GPU_MaxTemplature_value.Text = this._maxTemp_GPU.ToString() + "°";
				}
				else
				{
					int num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._minTemp_GPU);
					this.GPU_MinTemplature.Text = Startup.langRd["MUI_Min"].ToString() + " : ";
					this.GPU_MinTemplature_value.Text = num.ToString() + "°";
					num = CommonFunction.transfer_celsius_to_fahrenheit_name((int)this._maxTemp_GPU);
					this.GPU_MaxTemplature.Text = Startup.langRd["MUI_Max"].ToString() + " : ";
					this.GPU_MaxTemplature_value.Text = num.ToString() + "°";
				}
			}
			this.DrawLines_GPU();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public void PushData_CPU(params float[] data)
		{
			this.PushData_CPU(DateTime.Now, data);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000D8FA File Offset: 0x0000BAFA
		public void PushData_GPU(params float[] data)
		{
			this.PushData_GPU(DateTime.Now, data);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000D908 File Offset: 0x0000BB08
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.GetMonitoringUICoordinate();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000D910 File Offset: 0x0000BB10
		private void GetMonitoringUICoordinate()
		{
			if (!this.MonitoringUICoordinate.X.Equals(double.NaN) && !this.MonitoringUICoordinate.Y.Equals(double.NaN))
			{
				return;
			}
			Window window = Window.GetWindow(this);
			this.MonitoringUICoordinate = base.TranslatePoint(new Point(window.Left, window.Top), Window.GetWindow(this));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000D984 File Offset: 0x0000BB84
		private void CPU_lineChart_MouseMoveInChart(object sender, OC_MonitoringPage.MouseEventArgsWithChartData e)
		{
			OC_MonitoringPage oc_MonitoringPage = sender as OC_MonitoringPage;
			if (oc_MonitoringPage == null)
			{
				return;
			}
			this._timeTextBlockInGrid_CPU.Text = e.Time.ToString("HH:mm:ss");
			this._tempTextBlockInGrid_CPU.Text = e.ChartData[0] + "°";
			this._loadTextBlockInGrid_CPU.Text = e.ChartData[1] + "%";
			oc_MonitoringPage.CPU_dataPopupContent.Content = this._popupGrid_CPU;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000DA10 File Offset: 0x0000BC10
		private void GPU_lineChart_MouseMoveInChart(object sender, OC_MonitoringPage.MouseEventArgsWithChartData e)
		{
			OC_MonitoringPage oc_MonitoringPage = sender as OC_MonitoringPage;
			if (oc_MonitoringPage == null)
			{
				return;
			}
			if (e.ChartData[0] > 0f)
			{
				this._timeTextBlockInGrid_GPU.Text = e.Time.ToString("HH:mm:ss");
				this._tempTextBlockInGrid_GPU.Text = e.ChartData[0] + "°";
				this._loadTextBlockInGrid_GPU.Text = e.ChartData[1] + "%";
				oc_MonitoringPage.GPU_dataPopupContent.Content = this._popupGrid_GPU;
				return;
			}
			this._timeTextBlockInGrid_DiscreteGPU.Text = e.Time.ToString("HH:mm:ss");
			this._noticeTextBlockInGrid_DiscreteGPU.Text = Startup.langRd["MUI_GPU_Idle"].ToString();
			oc_MonitoringPage.GPU_DiscretedataPopupContent.Content = this._DiscretepopupGrid_GPU;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000DAFC File Offset: 0x0000BCFC
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

		// Token: 0x06000122 RID: 290 RVA: 0x0000DB9C File Offset: 0x0000BD9C
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

		// Token: 0x06000123 RID: 291 RVA: 0x0000DE50 File Offset: 0x0000C050
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

		// Token: 0x06000124 RID: 292 RVA: 0x0000E104 File Offset: 0x0000C304
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
			this.MouseMoveInChart_CPU(this, new OC_MonitoringPage.MouseEventArgsWithChartData(e.MouseDevice, e.Timestamp, e.StylusDevice, dateTime, array));
			this.CPU_dataPopup.HorizontalOffset = position.X + 10.0;
			this.CPU_dataPopup.VerticalOffset = position.Y + 15.0;
			if (!this.CPU_dataPopup.IsOpen)
			{
				this.CPU_dataPopup.IsOpen = true;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000E380 File Offset: 0x0000C580
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
			this.MouseMoveInChart_GPU(this, new OC_MonitoringPage.MouseEventArgsWithChartData(e.MouseDevice, e.Timestamp, e.StylusDevice, dateTime, array));
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

		// Token: 0x06000126 RID: 294 RVA: 0x0000E6C1 File Offset: 0x0000C8C1
		private void CPU_chartGrid_MouseLeave(object sender, MouseEventArgs e)
		{
			this.CPU_dataPopup.IsOpen = false;
			this.CPU_chartGrid.Children.Remove(this._hoverLine);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000E73E File Offset: 0x0000C93E
		public ContentControl CPU_DataPopupContent
		{
			get
			{
				return this.CPU_dataPopupContent;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000E746 File Offset: 0x0000C946
		public ContentControl GPU_DataPopupContent
		{
			get
			{
				return this.GPU_dataPopupContent;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000E74E File Offset: 0x0000C94E
		public ContentControl GPU_discretedataPopupContent
		{
			get
			{
				return this.GPU_DiscretedataPopupContent;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000E758 File Offset: 0x0000C958
		private void Common_Popup_Closed(object sender, EventArgs e)
		{
			OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
			oc_MainWindow.setting_Popup_Closed(sender, e);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000E77C File Offset: 0x0000C97C
		private void Common_Popup_Opened(object sender, EventArgs e)
		{
			OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
			oc_MainWindow.setting_Popup_Opened(sender, e);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000E7F0 File Offset: 0x0000C9F0
		private void GPU_OC_Level_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
			if (oc_MainWindow.initial_flag || oc_MainWindow.dont_do_oc_flag)
			{
				return;
			}
			RadioButton radioButton = sender as RadioButton;
			CommonFunction.Overclock_Mode_Type level = CommonFunction.Overclock_Mode_Type.Normal;
			if (Convert.ToInt32(radioButton.Tag) == 0)
			{
				level = CommonFunction.Overclock_Mode_Type.Normal;
			}
			else if (Convert.ToInt32(radioButton.Tag) == 1)
			{
				level = CommonFunction.Overclock_Mode_Type.Faster;
			}
			else if (Convert.ToInt32(radioButton.Tag) == 2)
			{
				level = CommonFunction.Overclock_Mode_Type.Turbo;
			}
			if ((ulong)level == (ulong)((long)Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", 0U)))
			{
				return;
			}
			this.lock_gpu_oc_radiobutton(false);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_gpu_oc_level(level).GetAwaiter();
				Thread.Sleep(2000);
				this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
				{
					this.lock_gpu_oc_radiobutton(true);
				}));
			};
			new Thread(threadStart).Start();
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", (uint)level);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000E8C6 File Offset: 0x0000CAC6
		private void lock_gpu_oc_radiobutton(bool flag)
		{
			this.GPU_OC_Normal_RadioButton.IsEnabled = flag;
			this.GPU_OC_Faster_RadioButton.IsEnabled = flag;
			this.GPU_OC_Turbo_RadioButton.IsEnabled = flag;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000E8EC File Offset: 0x0000CAEC
		private void set_GPU_overclock_status()
		{
			if (Registry.ValueExistsLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentCPUMode"))
			{
				int num = Registry.CheckLM("SOFTWARE\\OEM\\PredatorSense\\Overclock", "CurrentGPUMode", 0U);
				if (num == 0)
				{
					this.GPU_OC_Normal_RadioButton.IsChecked = new bool?(true);
					return;
				}
				if (num == 1)
				{
					this.GPU_OC_Faster_RadioButton.IsChecked = new bool?(true);
					return;
				}
				if (num == 2)
				{
					this.GPU_OC_Turbo_RadioButton.IsChecked = new bool?(true);
				}
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000E95C File Offset: 0x0000CB5C
		public void disable_GPU_overclock_control()
		{
			OC_MainWindow oc_MainWindow = (OC_MainWindow)Window.GetWindow(this);
			bool flag = false;
			if (oc_MainWindow.current_ac_mode == CommonFunction.AC_Mode_Type.aNo_AC)
			{
				flag = true;
			}
			else if (!oc_MainWindow.g_battery_boost)
			{
				flag = true;
			}
			oc_MainWindow.dont_do_oc_flag = true;
			if (flag)
			{
				this.GPU_OC_Normal_RadioButton.IsChecked = new bool?(true);
				this.GPU_OC_Faster_RadioButton.IsEnabled = !flag;
				this.GPU_OC_Turbo_RadioButton.IsEnabled = !flag;
				this.oc_notice_Grid.Visibility = Visibility.Visible;
			}
			else
			{
				this.GPU_OC_Faster_RadioButton.IsEnabled = !flag;
				this.GPU_OC_Turbo_RadioButton.IsEnabled = !flag;
				this.set_GPU_overclock_status();
				this.oc_notice_Grid.Visibility = Visibility.Hidden;
			}
			oc_MainWindow.dont_do_oc_flag = false;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000EA0C File Offset: 0x0000CC0C
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

		// Token: 0x06000132 RID: 306 RVA: 0x0000EA4E File Offset: 0x0000CC4E
		private void GPU_DiscretePopup_MouseMove(object sender, MouseEventArgs e)
		{
			this.g_GPU_mouse_enter = true;
			if (this.GPU_DiscretePopup.Visibility == Visibility.Visible)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000EA70 File Offset: 0x0000CC70
		private void GPU_DiscretePopup_MouseLeave(object sender, MouseEventArgs e)
		{
			this.g_GPU_mouse_enter = false;
			if (!this.gpu_enable_flag)
			{
				this.GPU_DiscretePopup.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000EA8D File Offset: 0x0000CC8D
		private void gpu_tip_icon_Button_Click(object sender, RoutedEventArgs e)
		{
			if (!this.gpu_tip_Popup.IsOpen)
			{
				this.gpu_tip_Popup.IsOpen = true;
				return;
			}
			this.gpu_tip_Popup.IsOpen = false;
		}

		// Token: 0x04000115 RID: 277
		private const int MaxNumOfData = 720;

		// Token: 0x04000118 RID: 280
		private readonly Point LineShadowBottomRightPoint = new Point(719.0, 99.0);

		// Token: 0x04000119 RID: 281
		private int _oldestDataIdx_CPU;

		// Token: 0x0400011A RID: 282
		private int _oldestDataIdx_GPU;

		// Token: 0x0400011B RID: 283
		private float _maxTemp_CPU;

		// Token: 0x0400011C RID: 284
		private float _maxTemp_GPU;

		// Token: 0x0400011D RID: 285
		private float _minTemp_CPU = 200f;

		// Token: 0x0400011E RID: 286
		private float _minTemp_GPU = 200f;

		// Token: 0x0400011F RID: 287
		private DateTime[] _times_CPU = new DateTime[720];

		// Token: 0x04000120 RID: 288
		private DateTime[] _times_GPU = new DateTime[720];

		// Token: 0x04000121 RID: 289
		private DateTime[] _times_GPU2 = new DateTime[720];

		// Token: 0x04000122 RID: 290
		private DateTime[] _times_System = new DateTime[720];

		// Token: 0x04000123 RID: 291
		private List<float[]> _linesData_CPU = new List<float[]>();

		// Token: 0x04000124 RID: 292
		private List<float[]> _linesData_GPU = new List<float[]>();

		// Token: 0x04000125 RID: 293
		private List<float[]> _linesData_GPU2 = new List<float[]>();

		// Token: 0x04000126 RID: 294
		private List<float[]> _linesData_System = new List<float[]>();

		// Token: 0x04000127 RID: 295
		private List<float[]> _linesData_CPU_F = new List<float[]>();

		// Token: 0x04000128 RID: 296
		private List<float[]> _linesData_GPU_F = new List<float[]>();

		// Token: 0x04000129 RID: 297
		private List<float[]> _linesData_GPU2_F = new List<float[]>();

		// Token: 0x0400012A RID: 298
		private List<float[]> _linesData_System_F = new List<float[]>();

		// Token: 0x0400012B RID: 299
		private List<Polyline> _lines_CPU = new List<Polyline>();

		// Token: 0x0400012C RID: 300
		private List<Polyline> _lines_GPU = new List<Polyline>();

		// Token: 0x0400012D RID: 301
		private List<Polyline> _lines_GPU2 = new List<Polyline>();

		// Token: 0x0400012E RID: 302
		private List<Polyline> _lines_System = new List<Polyline>();

		// Token: 0x0400012F RID: 303
		private List<Polygon> _linesShadow_CPU = new List<Polygon>();

		// Token: 0x04000130 RID: 304
		private List<Polygon> _linesShadow_GPU = new List<Polygon>();

		// Token: 0x04000131 RID: 305
		private List<Polygon> _linesShadow_GPU2 = new List<Polygon>();

		// Token: 0x04000132 RID: 306
		private List<Polygon> _linesShadow_System = new List<Polygon>();

		// Token: 0x04000133 RID: 307
		private Line _hoverLine = new Line();

		// Token: 0x04000134 RID: 308
		private Grid _popupGrid_CPU = new Grid();

		// Token: 0x04000135 RID: 309
		private Grid _popupGrid_GPU = new Grid();

		// Token: 0x04000136 RID: 310
		private Grid _popupGrid_GPU2 = new Grid();

		// Token: 0x04000137 RID: 311
		private Grid _popupGrid_System = new Grid();

		// Token: 0x04000138 RID: 312
		private Grid _DiscretepopupGrid_GPU = new Grid();

		// Token: 0x04000139 RID: 313
		private TextBlock _timeTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x0400013A RID: 314
		private TextBlock _tempTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x0400013B RID: 315
		private TextBlock _loadTextBlockInGrid_CPU = new TextBlock();

		// Token: 0x0400013C RID: 316
		private TextBlock _timeTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x0400013D RID: 317
		private TextBlock _tempTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x0400013E RID: 318
		private TextBlock _loadTextBlockInGrid_GPU = new TextBlock();

		// Token: 0x0400013F RID: 319
		private TextBlock _timeTextBlockInGrid_DiscreteGPU = new TextBlock();

		// Token: 0x04000140 RID: 320
		private TextBlock _noticeTextBlockInGrid_DiscreteGPU = new TextBlock();

		// Token: 0x04000141 RID: 321
		private Point MonitoringUICoordinate = new Point(double.NaN, double.NaN);

		// Token: 0x04000142 RID: 322
		private CommonFunction.Degree_Type Degree_Type;

		// Token: 0x04000143 RID: 323
		public bool gpu_enable_flag;

		// Token: 0x04000144 RID: 324
		private bool g_GPU_mouse_enter;

		// Token: 0x02000010 RID: 16
		public class MouseEventArgsWithChartData : MouseEventArgs
		{
			// Token: 0x06000137 RID: 311 RVA: 0x0000EE16 File Offset: 0x0000D016
			public MouseEventArgsWithChartData(MouseDevice mouse, int timestamp, StylusDevice stylusDevice, DateTime time, float[] data)
				: base(mouse, timestamp, stylusDevice)
			{
				this._time = time;
				this._data = data;
			}

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000138 RID: 312 RVA: 0x0000EE31 File Offset: 0x0000D031
			public DateTime Time
			{
				get
				{
					return this._time;
				}
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000139 RID: 313 RVA: 0x0000EE39 File Offset: 0x0000D039
			public float[] ChartData
			{
				get
				{
					return this._data;
				}
			}

			// Token: 0x04000169 RID: 361
			private DateTime _time;

			// Token: 0x0400016A RID: 362
			private float[] _data;
		}

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600013B RID: 315
		public delegate void PsMouseEventHandler(object sender, OC_MonitoringPage.MouseEventArgsWithChartData e);
	}
}
