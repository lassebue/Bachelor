using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{
    public class EmgDataSample
    {
        public EmgDataSample(int sensorCount)
        {
            _sensorValues = new List<int>(sensorCount);
        }

        private long _timeMs;
        public long TimeMs
        {
            get { return _timeMs; }
            set { _timeMs = value; }
        }

        private double _timeStamp;
        public double TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        private List<int> _sensorValues;
        public List<int> SensorValues
        {
            get { return _sensorValues; }
            set { _sensorValues = value; }
        }
    }

}
