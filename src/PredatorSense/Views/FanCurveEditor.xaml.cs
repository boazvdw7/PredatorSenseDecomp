using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        private static readonly object _pollStateLock = new object();
        private static readonly string FanCurveConfigPath =
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PredatorSense", "fan_curve.xml");
        private static bool _powerEventsRegistered;
        private static DateTime _ignoreTemperatureUntilUtc = DateTime.MinValue;
        private static int? _lastStableCpuTemp;
        private static int? _pendingCpuTemp;
        private static WeakReference<FanCurveEditor> _activeEditor;

        private bool _isCustomCurveEnabled = false;
        private int? _draggingIndex = null;
        private Point _dragStart;
        private double _dragStartX, _dragStartY;
        private ObservableCollection<FanCurvePoint> _editingFanCurve;
        private bool _isSortingFanCurve;
        private bool _suppressFanCurveCheckboxHandler;
        private static readonly IReadOnlyList<FanCurvePoint> DefaultFanCurveTemplate = new List<FanCurvePoint>
        {
            new FanCurvePoint { Temperature = 40, FanSpeed = 20 },
            new FanCurvePoint { Temperature = 50, FanSpeed = 30 },
            new FanCurvePoint { Temperature = 60, FanSpeed = 50 },
            new FanCurvePoint { Temperature = 70, FanSpeed = 70 },
            new FanCurvePoint { Temperature = 80, FanSpeed = 100 }
        };

        public FanCurveEditor()
        {
            InitializeComponent();
            base.Resources = Startup.styled;
            EnsurePowerEventRegistration();
            _activeEditor = new WeakReference<FanCurveEditor>(this);

            // Load saved curve or use default
            if (_globalFanCurve == null)
            {
                _globalFanCurve = LoadFanCurve() ?? CreateDefaultFanCurve();
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
                _editingFanCurve = CreateDefaultFanCurve();
            }
            FanCurve = _editingFanCurve;
            FanCurveGrid.ItemsSource = FanCurve;
            InitializeFanCurveEditingState();
            SortFanCurveByTemperature();

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
            SaveButton.IsEnabled = enabled;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SortFanCurveByTemperature();
            CopyEditingCurveToGlobal();

            SaveFanCurve(_globalFanCurve);

            MessageBox.Show("Fan curve saved.");
        }


        private static void SaveFanCurve(ObservableCollection<FanCurvePoint> curve)
        {
            try
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FanCurveConfigPath));
                using (var stream = File.Create(FanCurveConfigPath))
                {
                    var serializer = new XmlSerializer(typeof(List<FanCurvePoint>));
                    serializer.Serialize(stream, curve.OrderBy(point => point.Temperature).ToList());
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
                        return new ObservableCollection<FanCurvePoint>(list.OrderBy(point => point.Temperature));
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
                if (DateTime.UtcNow < _ignoreTemperatureUntilUtc)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (!TryGetStableCpuTemperature(out int cpuTemp))
                {
                    Thread.Sleep(2000);
                    continue;
                }

                if (_globalFanCurve == null || _globalFanCurve.Count == 0)
                {
                    Thread.Sleep(2000);
                    continue;
                }

                List<FanCurvePoint> orderedPoints = _globalFanCurve.OrderBy(point => point.Temperature).ToList();
                FanCurvePoint selectedPoint = orderedPoints[0];
                foreach (var point in orderedPoints)
                {
                    if (cpuTemp >= point.Temperature)
                    {
                        selectedPoint = point;
                    }
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

        private static void EnsurePowerEventRegistration()
        {
            lock (_pollStateLock)
            {
                if (_powerEventsRegistered)
                {
                    return;
                }

                Microsoft.Win32.SystemEvents.PowerModeChanged += OnPowerModeChanged;
                _powerEventsRegistered = true;
            }
        }

        private static void OnPowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            lock (_pollStateLock)
            {
                if (e.Mode == Microsoft.Win32.PowerModes.Resume)
                {
                    // Ignore wake-up sensor noise for a short period and require fresh stable samples.
                    _ignoreTemperatureUntilUtc = DateTime.UtcNow.AddSeconds(10);
                    _lastStableCpuTemp = null;
                    _pendingCpuTemp = null;
                }
                else if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
                {
                    _pendingCpuTemp = null;
                }
            }
        }

        private static bool TryGetStableCpuTemperature(out int cpuTemp)
        {
            cpuTemp = -1;
            int rawCpuTemp = -1;

            if (!CommonFunction.get_wmi_system_health_info(ref rawCpuTemp, CommonFunction.System_Health_Information_Index.sCPU_Temperature))
            {
                return false;
            }

            if (rawCpuTemp < 1 || rawCpuTemp > 110)
            {
                return false;
            }

            lock (_pollStateLock)
            {
                if (_lastStableCpuTemp.HasValue)
                {
                    int delta = Math.Abs(rawCpuTemp - _lastStableCpuTemp.Value);
                    if (delta > 20)
                    {
                        if (_pendingCpuTemp.HasValue && Math.Abs(_pendingCpuTemp.Value - rawCpuTemp) <= 5)
                        {
                            _lastStableCpuTemp = rawCpuTemp;
                            _pendingCpuTemp = null;
                            cpuTemp = rawCpuTemp;
                            return true;
                        }

                        _pendingCpuTemp = rawCpuTemp;
                        return false;
                    }
                }

                _pendingCpuTemp = null;
                _lastStableCpuTemp = rawCpuTemp;
                cpuTemp = rawCpuTemp;
                return true;
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
        }

        private void InitializeFanCurveEditingState()
        {
            FanCurve.CollectionChanged += FanCurve_CollectionChanged;
            foreach (var point in FanCurve)
            {
                point.PropertyChanged += FanCurvePoint_PropertyChanged;
            }
            FanCurveGrid.CellEditEnding += (s, e) =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (e.EditAction == DataGridEditAction.Commit &&
                        e.Column != null &&
                        e.Column.DisplayIndex == 0)
                    {
                        SortFanCurveByTemperature();
                    }
                    DrawFanCurve();
                }));
            };
        }

        private void FanCurve_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (FanCurvePoint point in e.OldItems)
                {
                    point.PropertyChanged -= FanCurvePoint_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (FanCurvePoint point in e.NewItems)
                {
                    point.PropertyChanged += FanCurvePoint_PropertyChanged;
                }
            }

            if (!_isSortingFanCurve)
            {
                SortFanCurveByTemperature();
            }
            DrawFanCurve();
        }

        private void FanCurvePoint_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_isSortingFanCurve)
            {
                return;
            }
        }

        private void SortFanCurveByTemperature(FanCurvePoint selectedPoint = null)
        {
            if (_isSortingFanCurve || FanCurve == null || FanCurve.Count < 2)
            {
                return;
            }

            _isSortingFanCurve = true;
            try
            {
                selectedPoint = selectedPoint ?? FanCurveGrid.SelectedItem as FanCurvePoint;
                List<FanCurvePoint> orderedPoints = FanCurve
                    .OrderBy(point => point.Temperature)
                    .ThenBy(point => point.FanSpeed)
                    .ToList();

                for (int i = 0; i < orderedPoints.Count; i++)
                {
                    int currentIndex = FanCurve.IndexOf(orderedPoints[i]);
                    if (currentIndex != i)
                    {
                        FanCurve.Move(currentIndex, i);
                    }
                }

                if (selectedPoint != null)
                {
                    FanCurveGrid.SelectedItem = selectedPoint;
                    FanCurveGrid.ScrollIntoView(selectedPoint);
                }
            }
            finally
            {
                _isSortingFanCurve = false;
            }
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
                    Text = $"{tempLabel} C",
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
            var polyline = new Polyline { Stroke = Brushes.DarkRed, StrokeThickness = 2 };
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
                // Always capture the mouse while dragging to avoid losing events
                var ellipse = sender as Ellipse;
                if (ellipse != null && !ellipse.IsMouseCaptured)
                {
                    ellipse.CaptureMouse();
                }

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

                // Only update if value actually changed
                if (FanCurve[_draggingIndex.Value].Temperature != newTemp ||
                    FanCurve[_draggingIndex.Value].FanSpeed != newSpeed)
                {
                    FanCurve[_draggingIndex.Value].Temperature = newTemp;
                    FanCurve[_draggingIndex.Value].FanSpeed = newSpeed;
                    DrawFanCurve();
                }

                e.Handled = true;
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
            if (_suppressFanCurveCheckboxHandler)
            {
                return;
            }
            ApplyCustomCurveEnabledState(true, true);

            var mainWindow = this.Owner as OC_MainWindow;
            if (mainWindow != null)
            {
                mainWindow.main_fan_mode_switch(2); // 2 = Custom mode
            }
        }

        private void fancurvekey_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_suppressFanCurveCheckboxHandler)
            {
                return;
            }
            ApplyCustomCurveEnabledState(false, true);

            // Set fan mode to Auto in the main window
            var mainWindow = this.Owner as OC_MainWindow;
            if (mainWindow != null)
            {
                mainWindow.main_fan_mode_switch(0); // 0 = Auto mode
            }
        }

        private void ApplyCustomCurveEnabledState(bool enabled, bool updateToggle)
        {
            _isCustomCurveEnabled = enabled;
            SetFanCurveUIEnabled(enabled);
            Dispatcher ownerDispatcher = this.Owner != null ? this.Owner.Dispatcher : this.Dispatcher;
            if (enabled)
            {
                SortFanCurveByTemperature();
                CopyEditingCurveToGlobal();
                SaveFanCurve(_globalFanCurve);
                StartGlobalFanCurvePolling(ownerDispatcher);
            }
            else
            {
                StopGlobalFanCurvePolling();
            }
            SaveFanCurveEnabled(enabled);

            if (updateToggle && fancurvekey_Checkbox != null && fancurvekey_Checkbox.IsChecked != enabled)
            {
                _suppressFanCurveCheckboxHandler = true;
                fancurvekey_Checkbox.IsChecked = enabled;
                _suppressFanCurveCheckboxHandler = false;
            }
        }

        public static void SyncCustomCurveEnabledWithFanMode(bool enabled)
        {
            SaveFanCurveEnabled(enabled);
            if (enabled)
            {
                Dispatcher dispatcher = Application.Current?.MainWindow?.Dispatcher ?? Application.Current?.Dispatcher;
                if (dispatcher != null)
                {
                    StartGlobalFanCurvePolling(dispatcher);
                }
            }
            else
            {
                StopGlobalFanCurvePolling();
            }

            if (_activeEditor != null && _activeEditor.TryGetTarget(out var activeEditor))
            {
                activeEditor.Dispatcher.BeginInvoke((Action)(() =>
                {
                    activeEditor.ApplyCustomCurveEnabledState(enabled, true);
                }));
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
            SaveFanCurveEnabled(_isCustomCurveEnabled);
            if (_activeEditor != null && _activeEditor.TryGetTarget(out var activeEditor) && ReferenceEquals(activeEditor, this))
            {
                _activeEditor = null;
            }
            base.OnClosed(e);
        }

        public static void ApplyFanCurveIfEnabled(Window owner)
        {
            if (LoadFanCurveEnabled())
            {
                StartGlobalFanCurvePolling(owner.Dispatcher);
            }
        }

        private static ObservableCollection<FanCurvePoint> CreateDefaultFanCurve()
        {
            return new ObservableCollection<FanCurvePoint>(
                DefaultFanCurveTemplate.Select(point => new FanCurvePoint
                {
                    Temperature = point.Temperature,
                    FanSpeed = point.FanSpeed
                }));
        }

        private void CopyEditingCurveToGlobal()
        {
            if (_globalFanCurve == null)
            {
                _globalFanCurve = new ObservableCollection<FanCurvePoint>();
            }

            _globalFanCurve.Clear();
            foreach (var pt in FanCurve.OrderBy(point => point.Temperature).ThenBy(point => point.FanSpeed))
            {
                _globalFanCurve.Add(new FanCurvePoint
                {
                    Temperature = pt.Temperature,
                    FanSpeed = pt.FanSpeed
                });
            }
        }

        private static void EnsureGlobalCurveLoaded()
        {
            if (_globalFanCurve == null || _globalFanCurve.Count == 0)
            {
                _globalFanCurve = LoadFanCurve() ?? CreateDefaultFanCurve();
            }
        }

        private static void StartGlobalFanCurvePolling(Dispatcher dispatcher)
        {
            try
            {
                EnsurePowerEventRegistration();
                EnsureGlobalCurveLoaded();
                StopGlobalFanCurvePolling();
                ThreadStart threadStart = delegate
                {
                    CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Custom);
                };
                new Thread(threadStart).Start();
                _globalCts = new CancellationTokenSource();
                Task.Run(() => PollFanCurve(_globalCts.Token, dispatcher), _globalCts.Token);
            }
            catch
            {
                StopGlobalFanCurvePolling();
            }
        }

        private static void StopGlobalFanCurvePolling()
        {
            _globalCts?.Cancel();
            _globalCts = null;
        }

        private void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            // Insert a new point between the selected point and the next, or at the end
            int insertIndex = FanCurveGrid.SelectedIndex;
            if (insertIndex < 0 || insertIndex >= FanCurve.Count - 1)
                insertIndex = FanCurve.Count - 2;

            if (insertIndex < 0) insertIndex = 0;

            // Calculate new point values between neighbors
            var pt1 = FanCurve[insertIndex];
            var pt2 = FanCurve[insertIndex + 1];
            int newTemp = (pt1.Temperature + pt2.Temperature) / 2;
            int newSpeed = (pt1.FanSpeed + pt2.FanSpeed) / 2;

            FanCurve.Insert(insertIndex + 1, new FanCurvePoint { Temperature = newTemp, FanSpeed = newSpeed });
            SortFanCurveByTemperature();
            DrawFanCurve();
        }

        private void RemovePoint_Click(object sender, RoutedEventArgs e)
        {
            if (FanCurve.Count <= 2) return; // Always keep at least 2 points
            int removeIndex = FanCurveGrid.SelectedIndex;
            if (removeIndex > 0 && removeIndex < FanCurve.Count - 1)
            {
                FanCurve.RemoveAt(removeIndex);
                DrawFanCurve();
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
