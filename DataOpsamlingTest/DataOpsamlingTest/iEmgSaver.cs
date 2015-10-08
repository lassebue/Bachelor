using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataOpsamlingTest
{
    public interface IEmgSaver
    {
        void SaveEmgData(List<Tuple<double, int>> emgData);
        ObservableCollection<string> PrintOutList
        { get; set; }

    }

}
