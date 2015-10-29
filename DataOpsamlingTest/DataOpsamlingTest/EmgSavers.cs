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

            System.Windows.Application.Current.Dispatcher.Invoke(
        
                System.Windows.Threading.DispatcherPriority.Normal,
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

    class EmgFileSavers : IEmgSaver
    {
        private string _headerString = "time,emg1,emg2,emg3,emg4,emg5,emg6,emg7,emg8,hand,pose,orientation,testPerson";
        private string _filePath;
        private EmgDataSet _dataSet;

        public EmgFileSavers(string filePath, EmgDataSet dataSet)
        {
            _dataSet = dataSet;

            // Tmp user                                     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            _dataSet.User = new ParseUser();
            _dataSet.User.Username = "BueBaby!";

            _filePath = filePath;
            _dataSet.EmgDataFile = _filePath;

            if (!File.Exists(_filePath))
            {
                // Create the .csv file to save the EMG data in
                using (StreamWriter steamWriter = File.CreateText(_filePath))
                {
                    steamWriter.WriteLine(_headerString); 
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

        public void FinalizeSave()
        {
            if (File.Exists(_filePath))
            {
                // Saves the current samples in the file 
                using (StreamWriter steamWriter = File.AppendText(_filePath))
                {
                    foreach (var item in dataList)
                    {
                        if (dataList.IndexOf(item) == 0){
		                    steamWriter.WriteLine(item +","+ _dataSet.Hand+","+_dataSet.Pose.PoseId+","+_dataSet.Orientation+","+_dataSet.User.Username);
	                    } else{
                            steamWriter.WriteLine(item);
	                    }
                    }
                }
            }
            else
            {
                // Create the .csv file to save the EMG data in
                using (StreamWriter steamWriter = File.CreateText(_filePath))
                {
                    steamWriter.WriteLine(_headerString);
                    foreach (var item in dataList)
                    {
                        steamWriter.WriteLine(item);
                    }
                }
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
                //var abe = emgWindowContainer.ElementAt(1);
                //System.Array pr = new double[4];
                //pr.SetValue(11, 0);
                //pr.SetValue(12, 1);
                //pr.SetValue(13, 2);
                //pr.SetValue(14, 3);

                //System.Array pi = new double[4];
                //pi.SetValue(1, 0);
                //pi.SetValue(2, 1);
                //pi.SetValue(3, 2);
                //pi.SetValue(4, 3);

                //matlab.PutFullMatrix("a", "base", pr, pi);
                //matlab.PutCharArray("abe", "base", "asdfasdfasd");
                //matlab.PutFullMatrix("a", "base", emgWindowContainer.ElementAt(0),
                //                                  emgWindowContainer.ElementAt(1));
                
                //matlab.PutFullMatrix("b", "base", emgWindowContainer.ElementAt(2),
                //                                  emgWindowContainer.ElementAt(3));
  
                //matlab.PutFullMatrix("c", "base", emgWindowContainer.ElementAt(4),
                //                                  emgWindowContainer.ElementAt(5));
  
                //matlab.PutFullMatrix("d", "base", emgWindowContainer.ElementAt(6),
                //                                  emgWindowContainer.ElementAt(7));


                //matlab.Execute("emg1Collection = a(1,:)");
                //matlab.Execute("emg2Collection = a(2,:)");

                //matlab.Execute("emg3Collection = b(1,:)");
                //matlab.Execute("emg4Collection = b(2,:)");

                //matlab.Execute("emg5Collection = c(1,:)");
                //matlab.Execute("emg6Collection = c(2,:)");

                //matlab.Execute("emg7Collection = d(1,:)");
                //matlab.Execute("emg8Collection = d(2,:)");


                //matlab.Feval("posePredictor", 1, out result);

                //object[] res = result as object[];

                //System.Windows.Application.Current.Dispatcher.Invoke(

                //System.Windows.Threading.DispatcherPriority.Normal,
                //(Action)delegate()
                //{
                //    PrintOutList.Add((string)res[0]);
                //});

                //var _controller = ((Controller)App.Current.FindResource("controller"));
                //_controller.MyoDispose();
                emgWindowContainer.Clear();
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
