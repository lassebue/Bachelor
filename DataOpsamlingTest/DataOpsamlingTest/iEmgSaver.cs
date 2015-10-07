using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataOpsamlingTest
{
    interface IEmgSaver
    {
        void SaveEmgData(List<Tuple<double, int>> emgData);
    }
}
