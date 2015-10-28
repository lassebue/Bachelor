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
using Parse;
using EmgDataModel;
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
        public ObservableCollection<string> PrintData;
        public IEmgSaver Printer;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Title = "Data Collection";
            //_startTime = DateTime.UtcNow;
            //_channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));
            //Printer = new EmgPrinterSaver();

            //SaveFileDialog saveFileDia = new SaveFileDialog();
            //saveFileDia.Filter = "csv|*.csv";
            //if (saveFileDia.ShowDialog() == true)
            //{
            //    _emgLogger = new EmgFileSavers(saveFileDia.FileName);
            //}


            //MyList.ItemsSource = DataSaver.PrintOutList;

            //DataContext = Printer.PrintOutList


            Loaded += WindowLoaded;
            Closed += WindowClosed;
            var abe = new Pose();
            var buller = new Pose();
            var bullie = new Pose();

            abe.PoseName = "SpreadFinger";
            abe.PoseId = 0;
            buller.PoseName = "CloseHand";
            buller.PoseId = 1;
            bullie.PoseName = "Relaxed hand";
            bullie.PoseId = 2;
            var poseC = ((PoseCollection)FindResource("poseCollection"));
            poseC.Poses.Add(abe);
            poseC.Poses.Add(buller);
            poseC.Poses.Add(bullie);
            //_hub = Hub.Create(_chanel);
            //_hub.MyoConnected +=HubMyoConnected;
            //_hub.MyoDisconnected +=HubMyoDisconnected;
        }

        //#region Events
        //private void HubMyoDisconnected(object sender, MyoEventArgs e)
        //{
        //    e.Myo.EmgDataAcquired -= MyoEmgDataAcquired;
        //}

        //private void HubMyoConnected(object sender, MyoEventArgs e)
        //{
        //    e.Myo.EmgDataAcquired += MyoEmgDataAcquired;
        //    e.Myo.SetEmgStreaming(true);
        //} 

        //private void MyoEmgDataAcquired(object sender, EmgDataEventArgs e)
        //{
        //    _sensorSamples = new List<Tuple<double, int>>(SENSOR_COUNT);
            
        //    EmgDataSample sample = new EmgDataSample(SENSOR_COUNT);
        //    sample.TimeStamp = (e.Timestamp - _startTime).TotalSeconds;
        //    sample.TimeMs = samplePeriode * sampleCount;

        //    // pull data from each sensor
        //    for (int i = 0; i < SENSOR_COUNT; i++)
        //    {
        //        sample.SensorValues.Add(e.EmgData.GetDataForSensor(i));
        //        //_sensorSamples.Add(new Tuple< double, int>((e.Timestamp - _startTime).TotalSeconds, e.EmgData.GetDataForSensor(i)));
        //    }
        //    //Printer.SaveEmgData(_sensorSamples);
        //    //_emgLogger.SaveEmgData(_sensorSamples);
        //    Printer.SaveEmgData(sample);
        //    _emgLogger.SaveEmgData(sample);
        //}
        //#endregion

        #region Methodes



        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // We will start to listening after the Myo data herer
            var testObject = new ParseObject("TestObject");
            testObject["foo"] = "bar";
            await testObject.SaveAsync();

            //var _controller = ((Controller)FindResource("controller"));
            //_controller.InitMyo();
            //_channel.StartListening();
        }

        private void WindowClosed(object sender, EventArgs e)
        {

            //_channel.Dispose();
            //_hub.Dispose();
            var _controller = ((Controller)FindResource("controller"));
            _controller.MyoDispose();
            Loaded -= WindowLoaded;
            Closed -= WindowClosed;
            base.OnClosed(e);
        }
        #endregion


    }
}