using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{
    public delegate void EmgWindowEventHandler(object sender, EmgWindEventArgs e);

    public class EmgWindowRecognition : CrustCrawlerApp.IEmgWindowRecognition, IDisposable
    {
        private int sampleCount = 0;
        private static int sensorCount = 8;
        private int windSize = 256;
        private List<System.Array> emgWindowContainer = new List<Array>(sensorCount);
        private MyoEmgController myoControl;

        //private ObservableCollection<string> _printOutList = new ObservableCollection<string>();
        private List<string> _printOutPoseList = new List<string>();

        public EmgWindowRecognition(int windowSize)
        {
            windSize = windowSize;
            System.Array tmpArray;
            myoControl = new MyoEmgController(this);

            for (int i = 0; i < sensorCount; i++)
            {
                tmpArray = new double[windSize];
                emgWindowContainer.Add(tmpArray);
            }
        }

        public event EmgWindowEventHandler WindowFilled;

        protected virtual void OnWindowFilled(EmgWindEventArgs e)
        {
            if (WindowFilled != null)
                WindowFilled(this, e);
        }

        public void SaveEmgData(EmgDataSample emgSample)
        {
            for (int i = 0; i < 8; i++)
            {
               emgWindowContainer.ElementAt(i).SetValue((double)emgSample.SensorValues[i], sampleCount);
            }
            sampleCount++;
            if (sampleCount == windSize)
            {
                OnWindowFilled(new EmgWindEventArgs(emgWindowContainer));
                sampleCount = 0;

            }
        }

        public void Dispose()
        {
            myoControl.Dispose();
        }

        //public void StartRecognition()
        //{
        //    myoControl.Changed += new ChangedEventHandler(SaveEmgData);
        //}

        //public void StopRecognition()
        //{
        //    myoControl.Changed -= new ChangedEventHandler(SaveEmgData);
        //}
}


    public class EmgWindEventArgs : EventArgs
    {

        public EmgWindEventArgs(List<System.Array> emgWind)
        {
            _emgWind = emgWind;
        }

        private List<System.Array> _emgWind;
        public List<System.Array> EmgWindow
        {
            get { return _emgWind; }
        }

    }

}
