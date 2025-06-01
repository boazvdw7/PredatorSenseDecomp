using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using TsDotNetLib;

namespace PredatorSense
{
    public partial class FanCurveEditor : Window
    {
        public ObservableCollection<FanCurvePoint> FanCurve { get; set; }
        private static CancellationTokenSource _globalCts; // Shared across all editors
        private static ObservableCollection<FanCurvePoint> _globalFanCurve; // Shared curve
        private static readonly string FanCurveConfigPath =
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PredatorSense", "fan_curve.xml");

        private bool _isCustomCurveEnabled = false;
        private int? _draggingIndex = null;
        private Point _dragStart;
        private double _dragStartX, _dragStartY;
        private ObservableCollection<FanCurvePoint> _editingFanCurve;

        public FanCurveEditor()
        {
            InitializeComponent();

            // Load saved curve or use default
            if (_globalFanCurve == null)
            {
                _globalFanCurve = LoadFanCurve() ?? new ObservableCollection<FanCurvePoint>
            {
                new FanCurvePoint { Temperature = 40, FanSpeed = 20 },
                new FanCurvePoint { Temperature = 50, FanSpeed = 30 },
                new FanCurvePoint { Temperature = 60, FanSpeed = 50 },
                new FanCurvePoint { Temperature = 70, FanSpeed = 70 },
                new FanCurvePoint { Temperature = 80, FanSpeed = 100 }
            };
            }

            // Deep copy the global curve for editing
            _editingFanCurve = new ObservableCollection<FanCurvePoint>();
            if (_globalFanCurve != null)
            {
                foreach (var pt in _globalFanCurve)
                    _editingFanCurve.Add(new FanCurvePoint { Temperature = pt.Temperature, FanSpeed = pt.FanSpeed });
            }
            else
            {
                _editingFanCurve = new ObservableCollection<FanCurvePoint>
                {
                    new FanCurvePoint { Temperature = 40, FanSpeed = 20 },
                    new FanCurvePoint { Temperature = 50, FanSpeed = 30 },
                    new FanCurvePoint { Temperature = 60, FanSpeed = 50 },
                    new FanCurvePoint { Temperature = 70, FanSpeed = 70 },
                    new FanCurvePoint { Temperature = 80, FanSpeed = 100 }
                };
            }
            FanCurve = _editingFanCurve;
            FanCurveGrid.ItemsSource = FanCurve;

            // Load checkbox state
            _isCustomCurveEnabled = LoadFanCurveEnabled();
            fancurvekey_Checkbox.IsChecked = _isCustomCurveEnabled;
            SetFanCurveUIEnabled(_isCustomCurveEnabled);

            fancurvekey_Checkbox.Checked += fancurvekey_Checkbox_Checked;
            fancurvekey_Checkbox.Unchecked += fancurvekey_Checkbox_Unchecked;

            this.Loaded += (s, e) => DrawFanCurve();
        }

        private void SetFanCurveUIEnabled(bool enabled)
        {
            FanCurveGrid.IsEnabled = enabled;
            ChartCanvas.IsEnabled = enabled;
            ApplyButton.IsEnabled = enabled;
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (!_isCustomCurveEnabled)
            {
                MessageBox.Show("Enable the custom fan curve checkbox first.");
                return;
            }

            // Copy edited curve to the global curve
            _globalFanCurve.Clear();
            foreach (var pt in _editingFanCurve)
                _globalFanCurve.Add(new FanCurvePoint { Temperature = pt.Temperature, FanSpeed = pt.FanSpeed });

            _globalCts?.Cancel();
            SaveFanCurve(_globalFanCurve);
            SaveFanCurveEnabled(_isCustomCurveEnabled);

            CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Custom);

            _globalCts = new CancellationTokenSource();
            // Use the main window as owner for UI updates
            Dispatcher ownerDispatcher = this.Owner.Dispatcher;
            Task.Run(() => PollFanCurve(_globalCts.Token, ownerDispatcher), _globalCts.Token);

            MessageBox.Show("Fan curve applied.");
        }


        private static void SaveFanCurve(ObservableCollection<FanCurvePoint> curve)
        {
            try
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FanCurveConfigPath));
                using (var stream = File.Create(FanCurveConfigPath))
                {
                    var serializer = new XmlSerializer(typeof(List<FanCurvePoint>));
                    serializer.Serialize(stream, new List<FanCurvePoint>(curve));
                }
            }
            catch
            {

            }
        }

        private static ObservableCollection<FanCurvePoint> LoadFanCurve()
        {
            try
            {
                if (File.Exists(FanCurveConfigPath))
                {
                    using (var stream = File.OpenRead(FanCurveConfigPath))
                    {
                        var serializer = new XmlSerializer(typeof(List<FanCurvePoint>));
                        var list = (List<FanCurvePoint>)serializer.Deserialize(stream);
                        return new ObservableCollection<FanCurvePoint>(list);
                    }
                }
            }
            catch { /* Optionally log error */ }
            return null;
        }

        private static void PollFanCurve(CancellationToken token, Dispatcher dispatcher)
        {
            while (!token.IsCancellationRequested)
            {
                int cpuTemp = -1;
                CommonFunction.get_wmi_system_health_info(ref cpuTemp, CommonFunction.System_Health_Information_Index.sCPU_Temperature);

                if (_globalFanCurve == null || _globalFanCurve.Count == 0)
                {
                    Thread.Sleep(2000);
                    continue;
                }

                FanCurvePoint selectedPoint = _globalFanCurve[0];
                foreach (var point in _globalFanCurve)
                {
                    if (cpuTemp >= point.Temperature)
                        selectedPoint = point;
                    else
                        break;
                }

                int cpuPercent = selectedPoint.FanSpeed;
                int gpuPercent = cpuPercent;

                Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CurrentFanMode", 2U);
                Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanPercentage", (uint)cpuPercent * 10U);
                Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanPercentage", (uint)gpuPercent * 10U);
                Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "CPUFanCustomAuto", 0U);
                Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\FanControl", "GPU1FanCustomAuto", 0U);

                int cpuSliderValue = (int)Math.Round(cpuPercent / 10.0);
                int gpuSliderValue = (int)Math.Round(gpuPercent / 10.0);

                cpuSliderValue = Math.Max(0, Math.Min(10, cpuSliderValue));
                gpuSliderValue = Math.Max(0, Math.Min(10, gpuSliderValue));

                // Update the UI on the main thread
                dispatcher.Invoke(() =>
                {
                    if (Application.Current.MainWindow is OC_MainWindow ocMain)
                    {
                        ocMain.FanControl_Page.SetCustomFanSpeedMode(cpuSliderValue, gpuSliderValue);
                    }
                });

                Thread.Sleep(2000);
            }
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DrawFanCurve();
            FanCurve.CollectionChanged += (s, e) => DrawFanCurve();
            FanCurveGrid.CellEditEnding += (s, e) => Dispatcher.BeginInvoke((Action)DrawFanCurve);
        }

        private void DrawFanCurve()
        {
            if (ChartCanvas == null) return;
            ChartCanvas.Children.Clear();

            double width = ChartCanvas.ActualWidth > 0 ? ChartCanvas.ActualWidth : 300;
            double height = ChartCanvas.ActualHeight > 0 ? ChartCanvas.ActualHeight : 300;

            // Get min/max for scaling
            int minTemp = int.MaxValue, maxTemp = int.MinValue;
            int minSpeed = 0, maxSpeed = 100;
            foreach (var pt in FanCurve)
            {
                if (pt.Temperature < minTemp) minTemp = pt.Temperature;
                if (pt.Temperature > maxTemp) maxTemp = pt.Temperature;
            }
            if (maxTemp == minTemp) maxTemp = minTemp + 1;

            // Draw grid lines and axis labels
            for (int i = 0; i <= 5; i++)
            {
                double y = height - i * height / 5;
                int speedLabel = minSpeed + (maxSpeed - minSpeed) * i / 5;
                ChartCanvas.Children.Add(new Line
                {
                    X1 = 0,
                    X2 = width,
                    Y1 = y,
                    Y2 = y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                });
                // Y axis labels
                var label = new TextBlock
                {
                    Text = $"{speedLabel}%",
                    Foreground = Brushes.Gray,
                    FontSize = 10
                };
                Canvas.SetLeft(label, 0);
                Canvas.SetTop(label, y - 10);
                ChartCanvas.Children.Add(label);
            }
            for (int i = 0; i <= 5; i++)
            {
                double x = i * width / 5;
                int tempLabel = minTemp + (maxTemp - minTemp) * i / 5;
                ChartCanvas.Children.Add(new Line
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = height,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                });
                // X axis labels
                var label = new TextBlock
                {
                    Text = $"{tempLabel}°C",
                    Foreground = Brushes.Gray,
                    FontSize = 10
                };
                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, height - 18);
                ChartCanvas.Children.Add(label);
            }

            double y10 = height - (10 - minSpeed) * height / (maxSpeed - minSpeed);
            double x40 = (40 - minTemp) * width / (maxTemp - minTemp);
            ChartCanvas.Children.Add(new Line { X1 = 0, X2 = width, Y1 = y10, Y2 = y10, Stroke = Brushes.Orange, StrokeDashArray = new DoubleCollection { 2 }, StrokeThickness = 1 });
            ChartCanvas.Children.Add(new Line { X1 = x40, X2 = x40, Y1 = 0, Y2 = height, Stroke = Brushes.Orange, StrokeDashArray = new DoubleCollection { 2 }, StrokeThickness = 1 });

            // Draw curve
            var polyline = new Polyline { Stroke = Brushes.Blue, StrokeThickness = 2 };
            List<Point> pointPositions = new List<Point>();
            foreach (var pt in FanCurve)
            {
                double x = (pt.Temperature - minTemp) * width / (maxTemp - minTemp);
                double y = height - (pt.FanSpeed - minSpeed) * height / (maxSpeed - minSpeed);
                polyline.Points.Add(new Point(x, y));
                pointPositions.Add(new Point(x, y));
            }
            ChartCanvas.Children.Add(polyline);

            // Draw draggable points
            for (int i = 0; i < pointPositions.Count; i++)
            {
                var ellipse = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Tag = i
                };
                Canvas.SetLeft(ellipse, pointPositions[i].X - 6);
                Canvas.SetTop(ellipse, pointPositions[i].Y - 6);

                ellipse.MouseLeftButtonDown += Point_MouseLeftButtonDown;
                ellipse.MouseMove += Point_MouseMove;
                ellipse.MouseLeftButtonUp += Point_MouseLeftButtonUp;
                ChartCanvas.Children.Add(ellipse);
            }
        }

        private void Point_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                _draggingIndex = (int)ellipse.Tag;
                _dragStart = e.GetPosition(ChartCanvas);
                _dragStartX = _dragStart.X;
                _dragStartY = _dragStart.Y;
                ellipse.CaptureMouse();
            }
        }

        private void Point_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingIndex.HasValue && e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(ChartCanvas);
                double width = ChartCanvas.ActualWidth > 0 ? ChartCanvas.ActualWidth : 300;
                double height = ChartCanvas.ActualHeight > 0 ? ChartCanvas.ActualHeight : 300;

                // Get min/max for scaling
                int minTemp = int.MaxValue, maxTemp = int.MinValue;
                int minSpeed = 0, maxSpeed = 100;
                foreach (var pt in FanCurve)
                {
                    if (pt.Temperature < minTemp) minTemp = pt.Temperature;
                    if (pt.Temperature > maxTemp) maxTemp = pt.Temperature;
                }
                if (maxTemp == minTemp) maxTemp = minTemp + 1;

                // Convert position to temperature and fan speed
                int newTemp = (int)Math.Round((pos.X / width) * (maxTemp - minTemp) + minTemp);
                int newSpeed = (int)Math.Round((height - pos.Y) / height * (maxSpeed - minSpeed) + minSpeed);

                // Clamp and keep order
                if (_draggingIndex > 0)
                    newTemp = Math.Max(newTemp, FanCurve[_draggingIndex.Value - 1].Temperature + 1);
                if (_draggingIndex < FanCurve.Count - 1)
                    newTemp = Math.Min(newTemp, FanCurve[_draggingIndex.Value + 1].Temperature - 1);

                newTemp = Math.Max(minTemp, Math.Min(maxTemp, newTemp));
                newSpeed = Math.Max(minSpeed, Math.Min(maxSpeed, newSpeed));

                // Update the model (this will update the table and trigger redraw)
                FanCurve[_draggingIndex.Value].Temperature = newTemp;
                FanCurve[_draggingIndex.Value].FanSpeed = newSpeed;
            }
        }

        private void Point_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                ellipse.ReleaseMouseCapture();
                _draggingIndex = null;
                DrawFanCurve(); // Final redraw
            }
        }

        private void fancurvekey_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            _isCustomCurveEnabled = true;
            SetFanCurveUIEnabled(true);
            SaveFanCurveEnabled(_isCustomCurveEnabled);

            var mainWindow = this.Owner as OC_MainWindow;
            if (mainWindow != null)
            {
                mainWindow.main_fan_mode_switch(2); // 2 = Custom mode
            }
        }

        private void fancurvekey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isCustomCurveEnabled = false;
            SetFanCurveUIEnabled(false);
            _globalCts?.Cancel();
            SaveFanCurveEnabled(_isCustomCurveEnabled);

            // Set fan mode to Auto in the main window
            var mainWindow = this.Owner as OC_MainWindow;
            if (mainWindow != null)
            {
                mainWindow.main_fan_mode_switch(0); // 0 = Auto mode
            }
        }


        // Save/load the checkbox state in a simple text file:
        private static readonly string FanCurveEnabledPath =
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PredatorSense", "fan_curve_enabled.txt");

        private static void SaveFanCurveEnabled(bool enabled)
        {
            try
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FanCurveEnabledPath));
                File.WriteAllText(FanCurveEnabledPath, enabled ? "1" : "0");
            }
            catch { }
        }

        private static bool LoadFanCurveEnabled()
        {
            try
            {
                if (File.Exists(FanCurveEnabledPath))
                    return File.ReadAllText(FanCurveEnabledPath).Trim() == "1";
            }
            catch { }
            return false;
        }

        // In OnClosed, save the state:
        protected override void OnClosed(EventArgs e)
        {
            SaveFanCurve(FanCurve);
            SaveFanCurveEnabled(_isCustomCurveEnabled);
            base.OnClosed(e);
        }

        public static void ApplyFanCurveIfEnabled(Window owner)
        {
            if (LoadFanCurveEnabled())
            {
                if (_globalFanCurve == null)
                    _globalFanCurve = LoadFanCurve() ?? new ObservableCollection<FanCurvePoint>
            {
                new FanCurvePoint { Temperature = 40, FanSpeed = 20 },
                new FanCurvePoint { Temperature = 50, FanSpeed = 30 },
                new FanCurvePoint { Temperature = 60, FanSpeed = 50 },
                new FanCurvePoint { Temperature = 70, FanSpeed = 70 },
                new FanCurvePoint { Temperature = 80, FanSpeed = 100 }
            };

                _globalCts?.Cancel();
                CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Custom);

                _globalCts = new CancellationTokenSource();
                Task.Run(() => PollFanCurve(_globalCts.Token, owner.Dispatcher), _globalCts.Token);
            }
        }
    }

    public class FanCurvePoint : INotifyPropertyChanged
    {
        private int _temperature;
        private int _fanSpeed;

        public int Temperature
        {
            get => _temperature;
            set { if (_temperature != value) { _temperature = value; OnPropertyChanged(nameof(Temperature)); } }
        }
        public int FanSpeed
        {
            get => _fanSpeed;
            set { if (_fanSpeed != value) { _fanSpeed = value; OnPropertyChanged(nameof(FanSpeed)); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}