using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmgDataModel
{
    [ParseClassName("TestPerson")]
    public class TestPerson: ParseObject 
    {
        public TestPerson() { }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }


    }

     [ParseClassName("Pose")]
    public class Pose: ParseObject 
    {
        public Pose() { }

        [ParseFieldName("poseName")]
        public string PoseName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }


    }

    //[ParseClassName("EmgDataSample")]
    //public class ParseEmgDataSample: ParseObject
    //{
    //    public ParseEmgDataSample() { }


    //    [ParseFieldName("timeMs")]
    //    public int TimeMs
    //    {
    //        get { return GetProperty<int>(); }
    //        set { SetProperty<int>(value); }
    //    }
    //    [ParseFieldName("timeStamp")]
    //    public double TimeStamp
    //    {
    //        get { return GetProperty<double>(); }
    //        set { SetProperty<double>(value); }
    //    }

    //    [ParseFieldName("timeMs")]
    //    public long TimeMs
    //    {
    //        get { return GetProperty<long>(); }
    //        set { SetProperty<long>(value); }
    //    }

    //    public IList<int> SensorValues
    //    {
    //        get { return GetProperty<IList<int>>(); }
    //        set { SetProperty<IList<int>>(value); }
    //    }

    //}

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
            get { return _sensorValues;}
            set { _sensorValues = value;}
        }


    }

    [ParseClassName("EmgDataSet")]
    public class EmgDataSet : ParseObject  
    {

        public EmgDataSet(){}

        // left hand = 0, right hand = 1
        [ParseFieldName("hand")]
        public int Hand
        {
            get { return GetProperty<int>(); }
            set { 
                if(value > 0 ) { SetProperty<int>(1); }
                else SetProperty<int>(0);
            }
        }

        [ParseFieldName("user")]
        public ParseRelation<ParseUser> User
        {
            get { return GetRelationProperty<ParseUser>(); }
        }

        // Between 0-360, if not within the that parameter, value = -1.
        [ParseFieldName("orientation")]
        public int Orientation
        {
            get { return GetProperty<int>(); }
            set
            {
                if (value >= 0||value <=360) { SetProperty<int>(value); }
                else SetProperty<int>(-1);
            }
        }

        [ParseFieldName("emgDataFile")]
        public string EmgDataFile
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty<string>(value);
            }
        }

        [ParseFieldName("pose")]
        public ParseRelation<Pose> Pose
        {
            get { return GetRelationProperty<Pose>(); }
        }
    }

}
