using EmgDataModel;
using Microsoft.Win32;
using MvvmFoundation.Wpf;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Math;
using Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

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
            var dataSet = ((EmgDataSet)Application.Current.FindResource("emgDataSet"));

            if (_recording)
            {
                MessageBox.Show("You are already recording!");
            }
            //else if (dataSet.Orientation ==-1)
            //{
            //    MessageBox.Show("You must check the orientation before recording!");
            //}
            else
            {
                //MessageBox.Show("recording stuff");
                SaveFileDialog saveFileDia = new SaveFileDialog();
                saveFileDia.Filter = "csv|*.csv";
                if (saveFileDia.ShowDialog() == true)
                {
                    _recording = true;
                    sampleCount = 0;

                    // ---------------------------------------------------------"BueBaby!" is a tmp user !!!!
                    dataSet.EmgDataFile = saveFileDia.FileName;
                    dataSet.UserName = "BueBaby!";

                    //var newDataSet = new EmgDataSet(dataSet.Orientation, dataSet.Hand, "BueBaby!", dataSet.Pose, saveFileDia.FileName);
                    _emgLogger = new EmgFileSavers(dataSet);
                    Printer = new EmgPrinterSaver();

                    myoControl = new MyoEmgController();
                    //InitMyo();
                    //_channel.StartListening();
                }

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
                myoControl.Dispose();

                //_channel.StopListening();


                //System.Windows.Application.Current.Dispatcher.Invoke(
                //System.Windows.Threading.DispatcherPriority.Normal,
                //(Action)delegate()
                //{

                //});

                _emgLogger.FinalizeSave();
                //MyoDispose();
                MessageBoxResult result = MessageBox.Show("Would you like to save the recorded file online?", "DCA", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        
                        var dataSet = ((EmgDataSet)Application.Current.FindResource("emgDataSet"));

                        var newDataSet = new EmgDataSet(dataSet.Orientation, dataSet.Hand, dataSet.UserName, dataSet.Pose, dataSet.EmgDataFile);

                        // Prepare for the csv file to be saved
                        var pathArray = dataSet.EmgDataFile.Split('\\');
                        var fileName = pathArray[pathArray.Length - 1];

                        FileStream fs = new FileStream(newDataSet.EmgDataFile, FileMode.Open);

                        ParseFile file = new ParseFile(fileName,fs,null);
                        SaveFileOnline(file, newDataSet);

                        //dataSet.OnlineFile = file;

                        //dataSet["fileFile"] = file;

                        //SaveSomething(file);

                        // Saves the dataSet in Parse.com
                        //SaveDataSetOnline(dataSet);


                        MessageBox.Show("Nice! It's been saved!", "DCA");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            //System.Windows.Application.Current.Dispatcher.Invoke(

            //    System.Windows.Threading.DispatcherPriority.Normal,
            //    (Action)delegate()
            //    {
            //        _channel.StopListening();
            //    });

            //MyoDispose();
        }

        private async void SaveDataSetOnline(EmgDataSet dataSet)
        {
            await dataSet.SaveAsync();
        }

        private async void SaveFileOnline(ParseFile file, EmgDataSet dataSet)
        {
            await file.SaveAsync();
            dataSet["onlineFile"] = file;
            await dataSet.SaveAsync();
        }

        private async void SaveSomething(ParseFile file)
        {
            var jobApplication = new ParseObject("JobApplication");
            jobApplication["applicantName"] = "Joe Smith";
            jobApplication["applicantResumeFile"] = file;
            await jobApplication.SaveAsync();
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
            // Start to listening to events from the Myo
            //_channel.StartListening();

            var myoControl = new MyoController();

            MessageBoxResult result = MessageBox.Show("Click ok and the current Roll orientation will be saved!", "DCA", MessageBoxButton.OK);
            switch (result)
            {
                case MessageBoxResult.OK:
                    myoControl.Dispose();
                    //MyoDispose();
                    //_channel.StopListening();
                    //MessageBox.Show("Nice! It's been saved!", "DCA");
                     //_channel.StopListening();

                    break;
                //case MessageBoxResult.No:
                //    MessageBox.Show("Keep trying!", "DCA");
                //    break;

            }
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

        #region newPose
        private ICommand _newPoseCommand;

        public ICommand NewPoseCommandHandler
        {
            get { return _newPoseCommand ?? (_newPoseCommand = new RelayCommand(NewPose)); }
        }

        private void NewPose()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            
            //MessageBox.Show("New pose!!!!");
            NewPoseWind newPoseWind = new NewPoseWind();
            newPoseWind.ShowDialog();
        }
        #endregion

        #region exit
        private ICommand _exitCommand;

        public ICommand exitCommandHandler
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(ExitBtn)); }
        }

        private void ExitBtn()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region instructions
        private ICommand _instructionsCommand;

        public ICommand instructionCommandHandler
        {
            get { return _instructionsCommand ?? (_instructionsCommand = new RelayCommand(instructionBtn)); }
        }

        private void instructionBtn()
        {
            InstructionsWind instWind = new InstructionsWind();
            instWind.Show();
        }

        #endregion

        // Myo initiation
        private readonly IChannel _channel;
        private readonly IHub _hub;
        public readonly DateTime startTime = DateTime.UtcNow;
        public  int sensorCount = 8;
        public long sampleCount = 0;
        public long samplePeriode = 5;

        private MyoController myoControl;

        // IEmgSaver instances for logging, printing og predicting the EMG data
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
        private IEmgSaver _windowPredictor;
        public IEmgSaver WindowPredictor
        {
            get { return _windowPredictor; }
            set
            {
                _windowPredictor = value;
                Notify();
            }
        }
        private IEmgSaver _emgLogger;
        public IEmgSaver EmgLogger
        {
            get { return _emgLogger; }
            set
            {
                _emgLogger = value;
                Notify();
            }
        }


        public string Roll
        {
            get { return _roll; }
            set
            {
                _roll = value;
                Notify();
            }
        }
        private string _roll = "";

        public void createWindPredictor(MLApp.MLApp matlab)
        {
            WindowPredictor = new EmgWindowSaver(matlab, 128);
        }

        //public void InitMyo()
        //{

        //    // listen for when the Myo connects
        //    _hub.MyoConnected += (sender, e) =>
        //    {
        //        e.Myo.Vibrate(VibrationType.Short);
        //        e.Myo.EmgDataAcquired += MyoEmgDataAcquired;
        //        //e.Myo.EmgDataAcquired += MyoOrientatiomAcquired;
        //        e.Myo.SetEmgStreaming(true);
        //    };

        //    // listen for when the Myo disconnects
        //    _hub.MyoDisconnected += (sender, e) =>
        //    {
        //        e.Myo.SetEmgStreaming(false);
        //        e.Myo.EmgDataAcquired -= MyoEmgDataAcquired;
        //        //e.Myo.EmgDataAcquired -= MyoOrientatiomAcquired;
        //    };

        //    //start listening for Myo data
        //    //_channel.StartListening();


        //    //_channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));
        //    //_hub = Hub.Create(_channel);
        //    //_hub.MyoConnected += HubMyoConnected;
        //    //_hub.MyoDisconnected += HubMyoDisconnected;
        //    Printer = new EmgPrinterSaver();
        //}
        //public void MyoDispose()
        //{
        //    if (_channel != null)
        //    {
        //        _channel.Dispose();
        //    }
        //    if (_hub != null)
        //    {
        //        _hub.Dispose();
        //    }
        //}


        #region Events
        
        //private void MyoOrientatiomAcquired(object sender, EmgDataEventArgs e)
        //{
        //    // Orientation test code
        //    var abe = e.Myo.Orientation;
        //    var roll = QuaternionF.Roll(abe);
        //    var yaw = QuaternionF.Yaw(abe);
        //    var pitch = QuaternionF.Pitch(abe);

        //    var roll_w = (int)((roll + (float)Math.PI) / (Math.PI * 2.0f) * 50);
        //    var pitch_w = (int)((pitch + (float)Math.PI / 2.0f) / Math.PI * 50);
        //    var yaw_w = (int)((yaw + (float)Math.PI) / (Math.PI * 2.0f) * 50);

        //    var acc = e.Myo.Accelerometer;
        //    var gyro = e.Myo.Gyroscope;

        //    Roll = "Roll: " + roll_w;
        //    var dataSet = ((EmgDataSet)Application.Current.FindResource("emgDataSet"));
        //    dataSet.Orientation = roll_w;
        //}

        //private void MyoEmgDataAcquired(object sender, EmgDataEventArgs e)
        //{
        //    EmgDataSample sample = new EmgDataSample(sen);
        //    sample.TimeStamp = (e.Timestamp - _startTime).TotalSeconds;
        //    sample.TimeMs = samplePeriode * sampleCount;
        //    sampleCount++;

        //    // pull data from each sensor
        //    for (int i = 0; i < SENSOR_COUNT; i++)
        //    {
        //        sample.SensorValues.Add(e.EmgData.GetDataForSensor(i));
        //    }
            
        //    Printer.SaveEmgData(sample);
        //    _emgLogger.SaveEmgData(sample);



        //    //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
        //}
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
