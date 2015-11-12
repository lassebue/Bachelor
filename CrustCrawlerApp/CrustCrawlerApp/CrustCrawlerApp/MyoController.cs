using CrustCrawlerApp.WindControl;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrustCrawlerApp
{
    public class MyoController : IDisposable, INotifyPropertyChanged
    {
        // Myo initiation
        private readonly IChannel _channel;
        private readonly IHub _hub;
        private MainVM _vm;
        //private Controller _troller;
        public MyoController(MainVM mv) 
        {
            _vm = mv;
             _channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            _hub = Hub.Create(_channel);

            _hub.MyoConnected += (sender, e) =>
            {
                e.Myo.Vibrate(VibrationType.Short);
                e.Myo.EmgDataAcquired += DataAcquired;
                //e.Myo.EmgDataAcquired += MyoOrientatiomAcquired;
                e.Myo.SetEmgStreaming(true);
            };

            // listen for when the Myo disconnects
            _hub.MyoDisconnected += (sender, e) =>
            {
                e.Myo.SetEmgStreaming(false);
                e.Myo.EmgDataAcquired -= DataAcquired;
                //e.Myo.EmgDataAcquired -= MyoOrientatiomAcquired;
            };
            _channel.StartListening();
            
        }

        public virtual void DataAcquired(object sender, EmgDataEventArgs e)
        {
            // Orientation test code
            var abe = e.Myo.Orientation;
            var roll = QuaternionF.Roll(abe);
            var yaw = QuaternionF.Yaw(abe);
            var pitch = QuaternionF.Pitch(abe);

            var roll_w = (int)((roll + (float)Math.PI) / (Math.PI * 2.0f) * 50);
            var pitch_w = (int)((pitch + (float)Math.PI / 2.0f) / Math.PI * 50);
            var yaw_w = (int)((yaw + (float)Math.PI) / (Math.PI * 2.0f) * 50);

            var acc = e.Myo.Accelerometer;
            var gyro = e.Myo.Gyroscope;

            _vm.Orientation = "Roll: " + roll_w;
            //var controller = ((Controller)Application.Current.FindResource("controller"));
            
            //controller.Roll = "Roll: " + roll_w;
            //var dataSet = ((EmgDataSet)Application.Current.FindResource("emgDataSet"));
            //dataSet.Orientation = roll_w;
        }

        public void Dispose()
        {
            _channel.Dispose();
            _hub.Dispose();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName]string propName = null)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class MyoEmgController : MyoController
    {
        //Controller controller;
        public MyoEmgController(MainVM vm): base(vm)
        {
            //controller = ((Controller)Application.Current.FindResource("controller"));
        }

        public override void DataAcquired(object sender, EmgDataEventArgs e)
        {
           // EmgDataSample sample = new EmgDataSample(controller.sensorCount);
           // sample.TimeStamp = (e.Timestamp - controller.startTime).TotalSeconds;
           // sample.TimeMs = controller.samplePeriode * controller.sampleCount;
           // controller.sampleCount++;

           // // pull data from each sensor
           // for (int i = 0; i < controller.sensorCount; i++)
           // {
           //     sample.SensorValues.Add(e.EmgData.GetDataForSensor(i));
           // }

           // controller.Printer.SaveEmgData(sample);
           //controller.EmgLogger.SaveEmgData(sample);
        }

    }
}
