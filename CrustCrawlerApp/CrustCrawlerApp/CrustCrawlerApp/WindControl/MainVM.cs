using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace CrustCrawlerApp.WindControl
{
    //private readonly BackgroundWorker worker = new BackgroundWorker();


    public class MainVM : INotifyPropertyChanged, IDisplayPose
    {
        private string _currentPose;

        // Skal ikke initeres her!!!
        // Skal nok have et interface!

        private int _windowCount;

        private EmgWindowRecognition windRecogn;

        public MainVM()
        {
            MatlabInit = new InitMatlab(this);
        }

        public InitMatlab MatlabInit { get; set; }

        public int WindowCount
        {
            get { return _windowCount; }
            set
            {
                _windowCount = value;
                Notify();
            }
        }

        public string CurrentPose
        {
            get { return _currentPose; }
            set
            {
                _currentPose = value;
                Notify();
            }
        }


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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

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

            MessageBox.Show("Orientation stuff wtf!!!!");

            myoControl.OrientationReceived -= UpdateOrientation;

            myoControl.Dispose();
        }

        private string _orientation = "";

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
            Orientation = "Orientation: " + e.Orientation;
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
            windRecogn = new EmgWindowRecognition(128);
            MatlabInit.EmgRecognition = windRecogn;
            MatlabInit.StartEmgRecognition();
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
            MatlabInit.StopEmgRecognition();
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
            MatlabInit.OpenClaw();
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
            MatlabInit.CloseClaw();
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