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

    public delegate void ChangedEventHandler(object sender, EmgSampleEventArgs e);
    public delegate void OrientationEventHandler(object sender, OrientationEventArgs e);

    public class EmgSampleEventArgs : EventArgs
    {
        public EmgSampleEventArgs(EmgDataSample sample)
        {
            _sample = sample;
        }

        private EmgDataSample _sample;
        public EmgDataSample Sample
        {
            get { return _sample; }
        }

    }
    public class OrientationEventArgs : EventArgs
    {
        public OrientationEventArgs(int ori)
        {
            _orientation = ori;
        }

        private int _orientation;
        public int Orientation
        {
            get { return _orientation; }
        }

    }


    public class MyoController : IDisposable, INotifyPropertyChanged
    {
        // Myo initiation
        private readonly IChannel _channel;
        private readonly IHub _hub;
        public readonly DateTime _startTime = DateTime.UtcNow;
        public int sensorCount = 8;
        public long samplePeriode = 5;
        public long sampleCount = 0;


        //private Controller _troller;
        public MyoController()
        {
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

        public event OrientationEventHandler OrientationReceived;
        protected virtual void OnOrientationReceived(OrientationEventArgs e)
        {
            if (OrientationReceived != null)
                OrientationReceived(this, e);
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

            OnOrientationReceived( new OrientationEventArgs(roll_w) );
            
            //_vm.Orientation = "Roll: " + roll_w;
            
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


    public class MyoEmgController : MyoController//, CrustCrawlerApp.IMyoEmgController
    {

        private int _sampleCount = 0;
        private IEmgWindowRecognition emgWindRecogn;
        private readonly int windSize = 256;
        private List<System.Array> emgWindowContainer = new List<Array>(8);
        

        public MyoEmgController( IEmgWindowRecognition emgWindRecogn ): base()
        {
            this.emgWindRecogn = emgWindRecogn;
        }

        //public event ChangedEventHandler Changed;

        //protected virtual void OnChanged(EmgSampleEventArgs e)
        //{
        //    if (Changed != null)
        //        Changed(this, e);
        //}

        public override void DataAcquired(object sender, EmgDataEventArgs e)
        {
            EmgDataSample sample = new EmgDataSample(8);

            sample.TimeStamp = (e.Timestamp - _startTime).TotalSeconds;
            sample.TimeMs = samplePeriode * sampleCount;
            _sampleCount++;

            // pull data from each sensor
            for (int i = 0; i < sensorCount; i++)
            {
                sample.SensorValues.Add(e.EmgData.GetDataForSensor(i));
            }

            emgWindRecogn.SaveEmgData(sample);
            //OnChanged(new EmgSampleEventArgs(sample));



            //for (int i = 0; i < 8; i++)
            //{
            //    emgWindowContainer.ElementAt(i).SetValue((double)emgData.SensorValues[i], _sampleCount);
            //}
            //_sampleCount++;
        
        }

    }
}
