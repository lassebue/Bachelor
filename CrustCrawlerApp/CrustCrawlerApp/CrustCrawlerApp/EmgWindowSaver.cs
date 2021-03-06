﻿using CrustCrawlerApp.WindControl;
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
        IDisplayPose mv;


        public EmgWindowRecognition(int windowSize, IDisplayPose mv)
        {
            this.mv = mv;
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
                mv.WindowCount++;
                var windowTime = DateTime.UtcNow;
                OnWindowFilled(new EmgWindEventArgs(emgWindowContainer,windowTime));
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

        public EmgWindEventArgs(List<System.Array> emgWind, DateTime windowTime)
        {
            _emgWind = emgWind;
            _windowTime = windowTime;
        }

        private List<System.Array> _emgWind;
        public List<System.Array> EmgWindow
        {
            get { return _emgWind; }
        }

        // Tmp property 
        private DateTime _windowTime;
        public DateTime WindowTime
        {
            get { return _windowTime; }
        }

    }

}
