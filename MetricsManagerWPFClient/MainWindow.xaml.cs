using System;
using System.Windows;

namespace MetricsManagerWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTimeOffset fromDate, toDate; 
        public MainWindow()
        {
            InitializeComponent();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                SetAgentId.Items.Add(random.Next(100));
            }
        }

        private void GetCPU_OnClick(object sender, RoutedEventArgs e)
        {
            fromDate = (DateTimeOffset)FromDate.SelectedDate;
            toDate = (DateTimeOffset) ToDate.SelectedDate;
            CpuChart.ColumnServiesValues[0].Values.Add(25d);
        }

        private void GetHDD_OnClick(object sender, RoutedEventArgs e)
        {
            fromDate = (DateTimeOffset)FromDate.SelectedDate;
            toDate = (DateTimeOffset)ToDate.SelectedDate;
            HddChart.ColumnServiesValues[0].Values.Add(25d);
        }
        private void GetNetwork_OnClick(object sender, RoutedEventArgs e)
        {
            fromDate = (DateTimeOffset)FromDate.SelectedDate;
            toDate = (DateTimeOffset)ToDate.SelectedDate;
            //NetworkChart.ColumnServiesValues[0].Values.Add(25d);
        }
        private void GetDotNet_OnClick(object sender, RoutedEventArgs e)
        {
            fromDate = (DateTimeOffset)FromDate.SelectedDate;
            toDate = (DateTimeOffset)ToDate.SelectedDate;
            DotNetChart.ColumnServiesValues[0].Values.Add(25d);
        }
        private void GetRAM_OnClick(object sender, RoutedEventArgs e)
        {
            fromDate = (DateTimeOffset)FromDate.SelectedDate;
            toDate = (DateTimeOffset)ToDate.SelectedDate;
            RamChart.ColumnServiesValues[0].Values.Add(25d);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new TextMetrics();
            win.Show();
        }
    }
}
