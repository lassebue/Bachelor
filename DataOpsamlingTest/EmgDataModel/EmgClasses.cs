using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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


        [ParseFieldName("poseId")]
        public int PoseId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }
    }
     
    [ParseClassName("PoseCollection")]
     public class PoseCollection: ParseObject, INotifyPropertyChanged
     {
        //private ObservableCollection<Pose> _poses;

        [ParseFieldName("Poses")]
        public ObservableCollection<Pose> Poses
         {
             get { return GetProperty<ObservableCollection<Pose>>(); }
             set
             {
                 SetProperty<ObservableCollection<Pose>>(value); 
                 Notify();
             }
         }
         public PoseCollection()
         {
             Poses = new ObservableCollection<Pose>();
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
    public class EmgDataSet : ParseObject  , INotifyPropertyChanged
    {

        public EmgDataSet(int ori, int hand, string user, Pose pose, string filePath )
        {
            Orientation = ori;
            Hand = hand;
            Pose = pose;
            UserName = user;
            EmgDataFile = filePath;

        }

        public EmgDataSet() { 
            Orientation = -1;
            UserName = "Username";
        }

        // left hand = 0, right hand = 1
        [ParseFieldName("hand")]
        public int Hand
        {
            get { return GetProperty<int>(); }
            set { 
                if(value > 0 ) { SetProperty<int>(1); }
                else SetProperty<int>(0);
                Notify();
            }
        }

        //[ParseFieldName("user")]
        //public ParseUser User
        //{
        //    get { return GetProperty<ParseUser>(); }
        //    set
        //    {
        //        SetProperty<ParseUser>(value);
        //        Notify();
        //    }
        //}

        [ParseFieldName("userName")]
        public string UserName
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty<string>(value);
                Notify();
            }
        }

        // Between 0-360, if not within the that parameter, value = -1.
        [ParseFieldName("orientation")]
        public int Orientation
        {
            get { return GetProperty<int>(); }
            set
            {
                if (value >= 0||value <=100) { SetProperty<int>(value); }
                else SetProperty<int>(-1);
                Notify();

            }
        }

        private ParseFile _parseFile = null;

        [ParseFieldName("onlineFile")]
        public ParseFile OnlineFile
        {
            get { return _parseFile; }
            set
            {
                _parseFile = value;
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
        public Pose Pose
        {
            get { return GetProperty<Pose>(); }
            set
            {
                SetProperty<Pose>(value);
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

}
