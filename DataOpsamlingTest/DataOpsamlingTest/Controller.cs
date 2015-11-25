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
using System.Net.Http;
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
            else if (dataSet.Orientation == -1)
            {
                MessageBox.Show("You must check the orientation before recording!");
            }
            else if (dataSet.UserName == "Username")
            {
                MessageBox.Show("Choose a username.\n You should always use the same username! ");
            }
            else
            {
                SaveFileDialog saveFileDia = new SaveFileDialog();
                saveFileDia.Filter = "csv|*.csv";
                if (saveFileDia.ShowDialog() == true)
                {
                    _recording = true;
                    sampleCount = 0;

                    // ---------------------------------------------------------"BueBaby!" is a tmp user !!!!
                    dataSet.EmgDataFile = saveFileDia.FileName;

                    _emgLogger = new EmgFileSavers(dataSet);
                    Printer = new EmgPrinterSaver();

                    myoControl = new MyoEmgController();
                }
            }
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
            await file.SaveAsync(new Progress<ParseUploadProgressEventArgs>(e =>
            {
                FileProgress = e.Progress*100;
                //MessageBox.Show(FileProgress.ToString());
                // Check e.Progress to get the progress of the file upload
            })); 
            
            //dataSet["onlineFile"] = file;
            dataSet.OnlineFile = file;
            await dataSet.SaveAsync();
            //FileProgress = 75;
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
            var control = ((Controller)App.Current.FindResource("controller"));
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
            //var sprintList = ((IEmgSaver)Application.Current.gResource("SprintListModel"));
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

        #region downloadDataOnline
        private ICommand _downloadDataCommand;
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public ICommand DownloadDataCommandHandler
        {
            get { return _downloadDataCommand ?? (_downloadDataCommand = new RelayCommand(DownloadData)); }
        }

        private void DownloadData()
        {
            GetEmgDataSet();
        }
        private async void GetEmgDataSet()
        {
            SaveFileDialog saveFileDia = new SaveFileDialog();
            saveFileDia.Filter = "csv|*.csv";
            saveFileDia.FileName = "Files will be saved here";

            if (saveFileDia.ShowDialog() == true)
            {
                // Get the directory where the online files should be saved locally
                var path = Path.GetDirectoryName(saveFileDia.FileName);

                // Get list of EmgDataSet's
                var query = ParseObject.GetQuery("EmgDataSet"); //new ParseQuery<EmgDataSet>();
                IEnumerable<ParseObject> result = await query.FindAsync();


                string fileName;

                var fileCount = result.Count();
                int currentFileCount = 0;
                FileProgress = 0;
                
                Task.Factory.StartNew(() => 
                {
                                    
                    foreach (var item in result)
                    {
                        fileName = path + "\\" + Path.GetFileName(item.Get<string>("emgDataFile"));

                        var onlineFile = result.ElementAt(0).Get<ParseFile>("onlineFile");

                        SaveOnlineFiles(fileName, onlineFile);

                        currentFileCount++;

                        FileProgress = currentFileCount / fileCount * 100;
                        
                    }
                });
            }
        }

        private async void SaveOnlineFiles(string fileName, ParseFile file)
        {
            var fileContent = await new HttpClient().GetStringAsync(file.Url);

            if (!File.Exists(fileName))
            {
                // Create the .csv data file locally
                StreamWriter streamWriter = File.CreateText(fileName);
                streamWriter.Write(fileContent);
                streamWriter.Dispose();
            }
            else
            {
                // Create the existing data file and saves the new 
                File.Delete(fileName);
                StreamWriter streamWriter = File.CreateText(fileName);
                streamWriter.Write(fileContent);
                streamWriter.Dispose();
            }
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
        private double _fileProgress = 0;
        public double FileProgress
        {
            get { return _fileProgress; }
            set { 
                _fileProgress = value;
                Notify();
            }
        }

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

       

        #region Events
      
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
