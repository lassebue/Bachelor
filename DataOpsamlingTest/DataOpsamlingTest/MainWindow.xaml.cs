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
using System.ComponentModel;
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
        MLApp.MLApp matlab; 
        private readonly BackgroundWorker worker = new BackgroundWorker();

        #endregion

        Pose abe = new Pose();
        Pose buller = new Pose();
        Pose bullie = new Pose();

        public MainWindow()
        {
            InitializeComponent();
            Title = "Data Collection";
            
            // Inits the matlab server and start the predictions of the server 
            ////worker.DoWork += worker_DoWork;
            ////worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            ////worker.RunWorkerAsync();

            //Loaded += WindowLoaded;
            Closed += WindowClosed;

            GetPoses();
        }
        private async void GetPoses()
        {
            //ParseQuery<ParseObject> query = ParseObject.GetQuery("PoseCollection");
            //ParseObject gameScore = await query.GetAsync("xWMyZ4YEGZ");

            //var query = from pose in new ParseQuery<Pose>()
            //            where pose.PoseId > -1
            //            select pose;
            //IEnumerable<Pose> result = await query.FindAsync();

            var poseC = ((PoseCollection)FindResource("poseCollection"));

            var query = new ParseQuery<Pose>();

            IEnumerable<Pose> result = await query.FindAsync();

            foreach (var item in result)
            {
                poseC.Poses.Add(item);
            }
            
            result.ToString();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = new MLApp.MLApp();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            matlab = ((MLApp.MLApp)e.Result);
            matlab.Execute(@"cd('Z:\Bachelor\Matlab import')");

            var _controller = ((Controller)FindResource("controller"));
            
           
            _controller.createWindPredictor(matlab);
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



        //private async void WindowLoaded(object sender, RoutedEventArgs e)
        //{
        //     // We will start to listening after the Myo data herer
        //    var testObject = new ParseObject("TestObject");
        //    testObject["foo"] = "bar";
        //    await testObject.SaveAsync();

        //    var _controller = ((Controller)FindResource("controller"));
        //    _controller.InitMyo();
        //    _channel.StartListening();
        //}

        private void WindowClosed(object sender, EventArgs e)
        {

            ////_channel.Dispose();
            ////_hub.Dispose();
            //var _controller = ((Controller)FindResource("controller"));
            //_controller.MyoDispose();
            //Loaded -= WindowLoaded;
            Closed -= WindowClosed;
            base.OnClosed(e);
        }
        #endregion

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}