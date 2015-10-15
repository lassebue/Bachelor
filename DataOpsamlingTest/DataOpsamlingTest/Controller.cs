using EmgDataModel;
using Microsoft.Win32;
using MvvmFoundation.Wpf;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataOpsamlingTest
{
    class Controller : INotifyPropertyChanged
    {
        // Fields
        private bool _recording;
        // UI Commands
        #region saveData
        private ICommand _saveEmgDataCommand;

        public ICommand SaveEmgDataCommandHandler
        {
            get { return _saveEmgDataCommand ?? (_saveEmgDataCommand = new RelayCommand(SaveEmgData)); }
        }

        private void SaveEmgData()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            MessageBox.Show("Save stuff wtf!!!!");
        }
        #endregion

        #region startRecording
        private ICommand _startDataRecordingCommand;

        public ICommand StartDataRecordingCommandHandler
        {
            get { return _startDataRecordingCommand ?? (_startDataRecordingCommand = new RelayCommand(StartDataRecording)); }
        }

        private void StartDataRecording()
        {
            if (_recording)
            {
                MessageBox.Show("You are already recording!");
            }
            else
            {
                _recording = true;
                sampleCount = 0;

                MessageBox.Show("recording stuff");
                SaveFileDialog saveFileDia = new SaveFileDialog();
                saveFileDia.Filter = "csv|*.csv";
                if (saveFileDia.ShowDialog() == true)
                {
                    _emgLogger = new EmgFileSavers(saveFileDia.FileName);
                }
                InitMyo();
                _channel.StartListening();



            }

            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
        }
        #endregion

        #region stopRecording
        private ICommand _stopDataRecordingCommand;

        public ICommand StopDataRecordingCommandHandler
        {
            get { return _stopDataRecordingCommand ?? (_stopDataRecordingCommand = new RelayCommand(StopDataRecording)); }
        }

        private void StopDataRecording()
        {
            if (!_recording)
            {
                MessageBox.Show("You're not recording");
            }
            else
            {
                _recording = false;
                MessageBox.Show("stop recording stuff");
                _channel.StopListening();
                
                //System.Windows.Application.Current.Dispatcher.Invoke(
                //System.Windows.Threading.DispatcherPriority.Normal,
                //(Action)delegate()
                //{

                //});

                _emgLogger.FinalizeSave();
                MyoDispose();
            }

            //System.Windows.Application.Current.Dispatcher.Invoke(

            //    System.Windows.Threading.DispatcherPriority.Normal,
            //    (Action)delegate()
            //    {
            //        _channel.StopListening();
            //    });

            //MyoDispose();

        }
        #endregion

        #region checkOrientation
        // Check orintentation of the Myo armband on the arm
        private ICommand _checkOrientationCommand;

        public ICommand CheckOrientationCommandHandler
        {
            get { return _checkOrientationCommand ?? (_checkOrientationCommand = new RelayCommand(CheckOrientation)); }
        }

        private void CheckOrientation()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            MessageBox.Show("Orientation wtf!!!!");
        }
        #endregion

        #region deleteData
        private ICommand _deleteDataCommand;

        public ICommand DeleteDataCommandHandler
        {
            get { return _deleteDataCommand ?? (_deleteDataCommand = new RelayCommand(DeleteDataRecording)); }
        }

        private void DeleteDataRecording()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            MessageBox.Show("Deleting stuff");
        }
        #endregion



        // Myo initiation
        private IChannel _channel;
        private IHub _hub;
        private readonly DateTime _startTime = DateTime.UtcNow;
        private const int SENSOR_COUNT = 8;
        private IEmgSaver _emgLogger;
        private long sampleCount = 0;
        private long samplePeriode = 5;
        private IEmgSaver _printer;
        public IEmgSaver Printer
        {
            get { return _printer; }
            set
            {
                _printer = value;
                Notify();
            }
        }

        public void InitMyo()
        {
            _channel = Channel.Create(
                 ChannelDriver.Create(ChannelBridge.Create(),
                 MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            _hub = Hub.Create(_channel);
            // listen for when the Myo connects
            _hub.MyoConnected += (sender, e) =>
            {
                e.Myo.Vibrate(VibrationType.Short);
                e.Myo.EmgDataAcquired += MyoEmgDataAcquired;
                e.Myo.SetEmgStreaming(true);
            };

            // listen for when the Myo disconnects
            _hub.MyoDisconnected += (sender, e) =>
            {
                e.Myo.SetEmgStreaming(false);
                e.Myo.EmgDataAcquired -= MyoEmgDataAcquired;
            };

            //start listening for Myo data
            //_channel.StartListening();


            //_channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));
            //_hub = Hub.Create(_channel);
            //_hub.MyoConnected += HubMyoConnected;
            //_hub.MyoDisconnected += HubMyoDisconnected;
            Printer = new EmgPrinterSaver();
        }
        public void MyoDispose()
        {
            _channel.Dispose();
            _hub.Dispose();
        }


        #region Events
        private void HubMyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired -= MyoEmgDataAcquired;
            e.Myo.SetEmgStreaming(false);

        }

        private void HubMyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired += MyoEmgDataAcquired;
            e.Myo.SetEmgStreaming(true);
        }

        private void MyoEmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            EmgDataSample sample = new EmgDataSample(SENSOR_COUNT);
            sample.TimeStamp = (e.Timestamp - _startTime).TotalSeconds;
            sample.TimeMs = samplePeriode * sampleCount;
            sampleCount++;

            // pull data from each sensor
            for (int i = 0; i < SENSOR_COUNT; i++)
            {
                sample.SensorValues.Add(e.EmgData.GetDataForSensor(i));
            }
            Printer.SaveEmgData(sample);
            _emgLogger.SaveEmgData(sample);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName]string propName = null)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

        }
    }
}
