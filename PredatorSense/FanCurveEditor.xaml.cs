using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PredatorSense
{
    public partial class FanCurveEditor : Window
    {
        public ObservableCollection<FanCurvePoint> FanCurve { get; set; }

        public FanCurveEditor()
        {
            InitializeComponent();
            FanCurve = new ObservableCollection<FanCurvePoint>
            {
                new FanCurvePoint { Temperature = 40, FanSpeed = 20 },
                new FanCurvePoint { Temperature = 50, FanSpeed = 30 },
                new FanCurvePoint { Temperature = 60, FanSpeed = 50 },
                new FanCurvePoint { Temperature = 70, FanSpeed = 70 },
                new FanCurvePoint { Temperature = 80, FanSpeed = 100 }
            };
            FanCurveGrid.ItemsSource = FanCurve;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                // Set fan mode to Custom
                CommonFunction.set_all_fan_mode(CommonFunction.Fan_Mode_Type.Custom);

                // Use a fixed value for testing (e.g., 50% fan speed)
                ulong cpuPercent = 200UL; // 50%
                ulong gpuPercent = 200UL; // 50%

                List<bool> auto = new List<bool> { false, false };
                List<ulong> percentage = new List<ulong> { cpuPercent, gpuPercent };

                CommonFunction.set_all_custom_fan_state(auto, percentage);
            });
            thread.Start();

            MessageBox.Show("Fan curve applied.");
        }
    }

    public class FanCurvePoint
    {
        public int Temperature { get; set; }
        public int FanSpeed { get; set; }
    }
}