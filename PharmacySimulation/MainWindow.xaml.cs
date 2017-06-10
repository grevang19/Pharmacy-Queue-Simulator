using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PharmacySimulation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SimulationSystem simulationSystem;
        internal static MainWindow main;

        public MainWindow()
        {
            InitializeComponent();
            simulationSystem = new SimulationSystem();
            main = this;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            simulationSystem.Timer.Start();
        }

        internal string Time
        {
            get { return timeLabel.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { timeLabel.Content = value; })); }
        }

        internal string StatusTextBlock
        {
            get { return statusTextBlock.Text.ToString(); }
            set 
            { 
                Dispatcher.Invoke(new Action(() => { statusTextBlock.Text += "\n"+value; }));
            }
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            simulationSystem.Timer.Stop();
            dataGrid.ItemsSource = simulationSystem.getCustomersHasBeenServed();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            simulationSystem.CurrentTime = 0;
            simulationSystem.Timer.Stop();
            simulationSystem.countAvarage();
            timeLabel.Content = "Time : 0";
        }

        private void setIntervalButton_Click(object sender, RoutedEventArgs e)
        {
            simulationSystem.Timer.Interval = Convert.ToDouble(intervalTextbox.Text) ;
        }

        internal List<Customer> Data
        {
            set { Dispatcher.Invoke(new Action(() => { dataGrid.ItemsSource = value; })); }
        }

        public void tableRefresh()
        {
            Dispatcher.Invoke(new Action(() => { dataGrid.Items.Refresh(); }));
        }

    }
}
