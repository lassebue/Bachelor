using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataOpsamlingTest
{
    class EmgListSaver : IEmgSaver
    {
        private List<List<Tuple<double, int>>> _data;

        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            throw new NotImplementedException();
        }
    }

    class EmgPrinterSaver : IEmgSaver
    {
        ObservableCollection<string> _printOutList;
        public EmgPrinterSaver(ObservableCollection<string> printeOutList)
        {
            _printOutList = printeOutList;
        }
        
        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            string emgOutPut = "Time " + emgData[0].Item1 + ", emg1 " + emgData[0].Item2 + ", emg2 " + emgData[1].Item2 +
                ", emg3 " + emgData[2].Item2 + ", emg4 " + emgData[3].Item2 + ", emg5 " + emgData[4].Item2 +
                ", emg7 " + emgData[5].Item2 + ", emg1 " + emgData[7].Item2 + ", emg1 " + emgData[7].Item2;
            _printOutList.Add(emgOutPut);
        }
    }

    class EmgFileSavers : IEmgSaver
    {

        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            throw new NotImplementedException();
        }
    }

    class EmgOnlineSavers : IEmgSaver
    {

        public void SaveEmgData(List<Tuple<double, int>> emgData)
        {
            throw new NotImplementedException();
        }
    }
}
