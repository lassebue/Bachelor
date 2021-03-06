﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace CrustCrawlerApp.WindControl
{
    public class MainVM : INotifyPropertyChanged, IDisplayPose
    {
        private EmgWindowRecognition windRecogn;

        public CCManagement ccm { get; set; }
        public Recognition Rec { get; set; }
        public Matlab matlab { get; set; }
        
        public MainVM()
        {
            matlab = new Matlab();
            ccm = new CCManagement(matlab);
            Rec = new Recognition(this, ccm, matlab);

        }

        #region Window count. For Test!!!!
        private int _windowCount;


        public int WindowCount
        {
            get { return _windowCount; }
            set
            {
                _windowCount = value;
                Notify();
            }
        }
        #endregion 
        
        #region Current window. For test!!!
        private int _currentWindow;

        public int CurrentWindow
        {
            get { return _currentWindow; }
            set
            {
                _currentWindow = value;
                Notify();
            }
        }
        #endregion  

        #region Current pose
        private string _currentPose;

        public string CurrentPose
        {
            get { return _currentPose; }
            set
            {
                _currentPose = value;
                Notify();
            }
        }
        #endregion 
        
        #region Username
        private string _userName = "Username";

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                Notify();
            }
        }
        #endregion 

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        // UI commands

        #region Check Orientation

        private ICommand _checkOrientationCommand;

        public ICommand CheckOrientationCommandHandler
        {
            get { return _checkOrientationCommand ?? (_checkOrientationCommand = new RelayCommand(CheckOrientation)); }
        }


        private void CheckOrientation()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            var myoControl = new MyoController();
            myoControl.OrientationReceived += UpdateOrientation;

            MessageBox.Show("Click OK to save the current Orientation!");

            myoControl.OrientationReceived -= UpdateOrientation;

            myoControl.Dispose();
        }

        private string _orientation = "";

        public int OrientationValue { get; set; }
        public string Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                Notify();
            }
        }

        private void UpdateOrientation(object sender, OrientationEventArgs e)
        {
            OrientationValue = e.Orientation;
            Orientation = "Orientation: " + OrientationValue;
        }

        #endregion

        #region Start Recognition

        private ICommand _startRecognCommand;

        public ICommand StartRecognCommandHandler
        {
            get { return _startRecognCommand ?? (_startRecognCommand = new RelayCommand(StartRecognition)); }
        }

        public void StartRecognition()
        {
            windRecogn = new EmgWindowRecognition(128, this);

            Rec.WindowRecognition = windRecogn;
            Rec.StartEmgRecognition();
        }

        #endregion

        #region Stop Recognition

        private ICommand _stopRecognCommand;

        public ICommand StopRecognCommandHandler
        {
            get { return _stopRecognCommand ?? (_stopRecognCommand = new RelayCommand(StopRecognition)); }
        }

        public void StopRecognition()
        {
            Rec.StopEmgRecognition();
        }

        #endregion

        #region Open Claw

        private ICommand _openClawCommand;

        public ICommand openClawCommandHandler
        {
            get { return _openClawCommand ?? (_openClawCommand = new RelayCommand(OpenClaw)); }
        }

        private void OpenClaw()
        {
            ccm.OpenHandPoseAction();
        }

        #endregion

        #region Close Claw

        private ICommand _closeClawCommand;

        public ICommand closeClawCommandHandler
        {
            get { return _closeClawCommand ?? (_closeClawCommand = new RelayCommand(CloseClaw)); }
        }

        private void CloseClaw()
        {
            ccm.ClosedHandPoseAction();
        }

        #endregion

        #region exit

        private ICommand _exitCommand;

        public ICommand exitCommandHandler
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(ExitBtn)); }
        }

        private void ExitBtn()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}