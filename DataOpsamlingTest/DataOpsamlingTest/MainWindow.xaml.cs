using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyoSharp.Device;
using MyoSharp.Communication;
using Microsoft.Win32;

namespace DataOpsamlingTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants
        private const int SENSOR_COUNT = 8;
        #endregion
        #region Fields
        private readonly IChannel _channel;
        private readonly IHub _hub;
        private readonly DateTime _startTime;
        private List<Tuple<double, int>> _sensorSamples;
        //private List<List<Tuple<double, int>>> _data;
        public ObservableCollection<string> PrintData;
        public ObservableCollection<string> Abe;
        public IEmgSaver Printer;
        private IEmgSaver _emgLogger;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
                 
            _startTime = DateTime.UtcNow;
            _channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));
            Printer = new EmgPrinterSaver();

            SaveFileDialog saveFileDia = new SaveFileDialog();
            saveFileDia.Filter = "csv|*.csv";
            if (saveFileDia.ShowDialog() == true)
            {
                _emgLogger = new EmgFileSavers(saveFileDia.FileName);

            }

            //MyList.ItemsSource = DataSaver.PrintOutList;
            DataContext = Printer.PrintOutList;

            Loaded += WindowLoaded;
            Closed += WindowClosed;
            _hub = Hub.Create(_channel);
            _hub.MyoConnected +=HubMyoConnected;
            _hub.MyoDisconnected +=HubMyoDisconnected;
        }

        #region Events
        private void HubMyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired -= Myo_EmgDataAcquired;
        }

        private void HubMyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired += Myo_EmgDataAcquired;
            e.Myo.SetEmgStreaming(true);
        } 

        private void Myo_EmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            _sensorSamples = new List<Tuple<double, int>>(SENSOR_COUNT);

            // pull data from each sensor
            for (int i = 0; i < SENSOR_COUNT-1; i++)
            {
                _sensorSamples.Add(new Tuple< double, int>((e.Timestamp - _startTime).TotalSeconds, e.EmgData.GetDataForSensor(i)));
            }
            Printer.SaveEmgData(_sensorSamples);
            _emgLogger.SaveEmgData(_sensorSamples);
        }
        #endregion

        #region Methodes

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // We will start to listening after the Myo data herer

            _channel.StartListening();
        }


        private void WindowClosed(object sender, EventArgs e)
        {

            _channel.Dispose();
            _hub.Dispose();
            Loaded -= WindowLoaded;
            Closed -= WindowClosed;
            base.OnClosed(e);
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Abe.Add("Bullie!");
        }
    }
}