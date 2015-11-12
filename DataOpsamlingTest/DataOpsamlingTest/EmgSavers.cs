using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EmgDataModel;
using Parse;
using System.Windows;
using Microsoft.Win32;

namespace DataOpsamlingTest
{
    class EmgListSaver : IEmgSaver
    {

        public void SaveEmgData( EmgDataSample emgData )
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<string> PrintOutList
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void FinalizeSave()
        {
            throw new NotImplementedException();
        }
    }

    public class EmgPrinterSaver : INotifyPropertyChanged, IEmgSaver
    {
        ObservableCollection<string> _printOutList = new ObservableCollection<string>();

        public void SaveEmgData(EmgDataSample emgData)
        {
            string emgOutPut = "Time " + emgData.TimeMs + ", " + emgData.SensorValues[0] + ", " + emgData.SensorValues[1] +
                ", " + emgData.SensorValues[2] + ", " + emgData.SensorValues[3] + ", " + emgData.SensorValues[4] +
                ", " + emgData.SensorValues[5] + ", " + emgData.SensorValues[6] + ", " + emgData.SensorValues[7];


            //PrintOutList.Add(emgOutPut);

            //Application.Current.Dispatcher.Invoke((Action)(() =>
            //{
            //    PrintOutList.Add(emgOutPut);
            //}));

            System.Windows.Application.Current.Dispatcher.Invoke(

                System.Windows.Threading.DispatcherPriority.Background,
                (Action)delegate()
                {
                    PrintOutList.Add(emgOutPut);
                });
        }

        public ObservableCollection<string> PrintOutList
        {
            get
            {
                return _printOutList;
            }
            set
            {
                _printOutList = value;
                Notify();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName]string propName = null)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        

        public void FinalizeSave()
        {
            throw new NotImplementedException();
        }
    }

    class EmgFileSavers : IEmgSaver, INotifyPropertyChanged
    {
        private string _headerString = "time,emg1,emg2,emg3,emg4,emg5,emg6,emg7,emg8,hand,pose,orientation,testPerson";
        private string _filePath;
        private EmgDataSet _dataSet;

        public EmgFileSavers(EmgDataSet dataSet)
        {
            _dataSet = dataSet;

            _filePath = _dataSet.EmgDataFile ;
            _dataSet.EmgDataFile = _filePath;

            saveFile(_filePath);
        }

        public void saveFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Create the .csv file to save the EMG data in
                using (StreamWriter steamWriter = File.CreateText(filePath))
                {
                    steamWriter.WriteLine(_headerString);
                }
            }
            else
            {
                try
                {
                    // Delete the existing file
                    File.Delete(filePath);
                    // Create the .csv file to save the EMG data in
                    //using (StreamWriter steamWriter = File.CreateText(_filePath))
                    //{
                    //    steamWriter.WriteLine(_headerString);
                    //}
                }
                catch (Exception e)
                {
                    MessageBox.Show("The file could not be overwritten, try another name...");
                    SaveFileDialog saveFileDia = new SaveFileDialog();
                    saveFileDia.Filter = "csv|*.csv";
                    if (saveFileDia.ShowDialog() == true)
                    {
                        saveFile(saveFileDia.FileName);
                    }
                }
            }
        }

        List<string> dataList = new List<string>();

        public void SaveEmgData(EmgDataSample emgData)
        {
            // The format follows the '_headerString'
            string emgDataLine = emgData.TimeMs + "," + emgData.SensorValues[0] + "," + emgData.SensorValues[1] +
                "," + emgData.SensorValues[2] + "," + emgData.SensorValues[3] + "," + emgData.SensorValues[4] +
                "," + emgData.SensorValues[5] + "," + emgData.SensorValues[6] + "," + emgData.SensorValues[7];

            dataList.Add(emgDataLine);
        }

        public ObservableCollection<string> PrintOutList
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        double _progress = 0;
        public double Progress
        {
            get { return _progress; }
            set {
                ((Controller)App.Current.FindResource("controller")).FileProgress = value;
                _progress = value;
                Notify();

            }
        }
        public void FinalizeSave()
        {
            var controller = ((Controller)App.Current.FindResource("controller"));
            double index = 0;
            double count = dataList.Count;

            Task.Factory.StartNew(() =>
            {
                if (File.Exists(_filePath))
                {
                    // Saves the current samples in the file 
                    using (StreamWriter streamWriter = File.AppendText(_filePath))
                    {


                        foreach (var item in dataList)
                        {
                            if (dataList.IndexOf(item) == 0)
                            {
                                streamWriter.WriteLine(item + "," + _dataSet.Hand + "," + _dataSet.Pose.PoseId + "," + _dataSet.Orientation + "," + _dataSet.UserName);
                            }
                            else
                            {
                                streamWriter.WriteLine(item);
                                index = dataList.IndexOf(item);
                                Progress = index / count * 100;
                            }
                        }
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.AppendText(_filePath))
                    {
                        streamWriter.WriteLine(_headerString);

                        foreach (var item in dataList)
                        {
                            if (dataList.IndexOf(item) == 0)
                            {
                                streamWriter.WriteLine(item + "," + _dataSet.Hand + "," + _dataSet.Pose.PoseId + "," + _dataSet.Orientation + "," + _dataSet.UserName);
                            }
                            else
                            {
                                streamWriter.WriteLine(item);
                                index = dataList.IndexOf(item);
                                Progress = index / count * 100;
                            }
                        }
                    }
                }
            });
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

    public class EmgWindowSaver : IEmgSaver
    {
        private MLApp.MLApp matlab;
        private int _sampleCount = 0;
        private int windSize = 256;
        private List<System.Array> emgWindowContainer = new List<Array>(8);
        private ObservableCollection<string> _printOutList = new ObservableCollection<string>();

        public EmgWindowSaver(MLApp.MLApp _matlab, int windowSize)
        {
            windSize = windowSize;
            matlab = _matlab;
            System.Array tmpArray;
            for (int i = 0; i < 8; i++)
			{

                tmpArray = new double[windSize];
                emgWindowContainer.Add(tmpArray);
			}
        }

        public void SaveEmgData(EmgDataSample emgData)
        {

            for (int i = 0; i < 8; i++)
            {
                emgWindowContainer.ElementAt(i).SetValue((double)emgData.SensorValues[i],_sampleCount);
            }
            _sampleCount++;
            if (_sampleCount == windSize)
            {
                object result = null;

                matlab.Feval("posePredictor", 1, out result,emgWindowContainer.ElementAt(0),
                                                            emgWindowContainer.ElementAt(1),
                                                            emgWindowContainer.ElementAt(2),
                                                            emgWindowContainer.ElementAt(3),
                                                            emgWindowContainer.ElementAt(4),
                                                            emgWindowContainer.ElementAt(5),
                                                            emgWindowContainer.ElementAt(6),
                                                            emgWindowContainer.ElementAt(7));

                object[] res = result as object[];

                System.Windows.Application.Current.Dispatcher.Invoke(

                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    PrintOutList.Add((string)res[0]);
                });

                //var _controller = ((Controller)App.Current.FindResource("controller"));
                //_controller.MyoDispose();
                _sampleCount = 0;
            }
        }
      
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName]string propName = null)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void FinalizeSave()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<string> PrintOutList
        {
            get
            {
                return _printOutList;
            }
            set
            {
                _printOutList = value;
                Notify();
            }
        }
    }

    class EmgOnlineSavers : IEmgSaver
    {

        public void SaveEmgData(EmgDataSample emgData)
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<string> PrintOutList
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void FinalizeSave()
        {
            throw new NotImplementedException();
        }
    }
}
