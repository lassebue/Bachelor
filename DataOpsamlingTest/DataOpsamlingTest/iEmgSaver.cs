using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmgDataModel;

namespace DataOpsamlingTest
{
    public interface IEmgSaver
    {
        void SaveEmgData(EmgDataSample emgData);
        void FinalizeSave();
        ObservableCollection<string> PrintOutList
        { get; set; }

    }

}
