using EmgDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{
    public class EmgWindowSaver
    {
        private MLApp.MLApp matlab;
        private int _sampleCount = 0;
        private int windSize = 256;
        private List<System.Array> emgWindowContainer = new List<Array>(8);
        //private ObservableCollection<string> _printOutList = new ObservableCollection<string>();
        private List<string> _printOutPoseList = new List<string>();

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
                emgWindowContainer.ElementAt(i).SetValue((double)emgData.SensorValues[i], _sampleCount);
            }
            _sampleCount++;
            if (_sampleCount == windSize)
            {
                object result = null;

                matlab.Feval("posePredictor", 1, out result, emgWindowContainer.ElementAt(0),
                                                            emgWindowContainer.ElementAt(1),
                                                            emgWindowContainer.ElementAt(2),
                                                            emgWindowContainer.ElementAt(3),
                                                            emgWindowContainer.ElementAt(4),
                                                            emgWindowContainer.ElementAt(5),
                                                            emgWindowContainer.ElementAt(6),
                                                            emgWindowContainer.ElementAt(7));

                object[] res = result as object[];

                //System.Windows.Application.Current.Dispatcher.Invoke(

                //System.Windows.Threading.DispatcherPriority.Normal,
                //(Action)delegate()
                //{
                //    _printOutPoseList.Add((string)res[0]);
                //});

                _printOutPoseList.Add((string)res[0]);

                //var _controller = ((Controller)App.Current.FindResource("controller"));
                //_controller.MyoDispose();
                _sampleCount = 0;
            }
        }
    }
}
