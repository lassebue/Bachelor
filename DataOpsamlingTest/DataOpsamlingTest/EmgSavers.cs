using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataOpsamlingTest
{
    class EmgListSaver : IEmgSaver
    {

        public void SaveEmgData(List<Tuple<double, int>> emgData)
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
    }

    public class EmgPrinterSaver : INotifyPropertyChanged, IEmgSaver
    {
        ObservableCollection<string> _printOutList = new ObservableCollection<string>();

        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            string emgOutPut = "Time " + emgData[0].Item1 + ", emg1 " + emgData[0].Item2 + ", emg2 " + emgData[1].Item2 +
                ", emg3 " + emgData[2].Item2 + ", emg4 " + emgData[3].Item2 + ", emg5 " + emgData[4].Item2 +
                ", emg7 " + emgData[5].Item2 + ", emg1 " + emgData[6].Item2 + ", emg1 " + emgData[7].Item2;
        
            
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
    }

    class EmgFileSavers : IEmgSaver
    {
        private String _headerString = "timestamp,emg1,emg2,emg3,emg4,emg5,emg6,emg7,emg8";
        private string _filePath;

        public EmgFileSavers(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                // Create the .csv file to save the EMG data in
                using (StreamWriter steamWriter = File.CreateText(_filePath))
                {
                    steamWriter.WriteLine(_headerString); 
                }
            }


        }
        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            // The format follows the '_headerString'
            string emgDataLine = emgData[0].Item1 + "," + emgData[0].Item2 + "," + emgData[1].Item2 +
                "," + emgData[2].Item2 + "," + emgData[3].Item2 + "," + emgData[4].Item2 +
                "," + emgData[5].Item2 + "," + emgData[6].Item2 + "," + emgData[7].Item2;

            if (File.Exists(_filePath))
            {
                // Saves the current samples in the file 
                using (StreamWriter steamWriter = File.AppendText(_filePath))
                {
                    steamWriter.WriteLine(emgDataLine);
                }
            } else 
            {
                // Create the .csv file to save the EMG data in
                using (StreamWriter steamWriter = File.CreateText(_filePath))
                {
                    steamWriter.WriteLine(_headerString);
                    steamWriter.WriteLine(emgDataLine);

                }
            }
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
    }

    class EmgOnlineSavers : IEmgSaver
    {

        public void SaveEmgData(List<Tuple<double, int>> emgData)
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
    }
}
