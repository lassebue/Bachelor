using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CrustCrawlerApp;

namespace CrustCrawlerApp.WindControl
{
    //private readonly BackgroundWorker worker = new BackgroundWorker();



    public class MainVM: INotifyPropertyChanged, IDisplayPose
    {

        private EmgWindowRecognition windRecogn;

        // Skal ikke initeres her!!!
        // Skal nok have et interface!
        private InitMatlab _im;
        public InitMatlab MatlabInit
        {
            get { return _im; }
            set { _im = value; }
        }

        public MainVM()
        {
            MatlabInit = new InitMatlab( this );
        }

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
            var myoControl =  new MyoController();
            myoControl.OrientationReceived += new OrientationEventHandler(UpdateOrientation);
            
            MessageBox.Show("Orientation stuff wtf!!!!");

            myoControl.OrientationReceived -= new OrientationEventHandler(UpdateOrientation);

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

        private void UpdateOrientation( object sender, OrientationEventArgs e)
        {
            Orientation = "Orientation: "+ e.Orientation;
        }

        #endregion

        #region Start Recognition

        public void StartRecognition()
        {
            windRecogn = new EmgWindowRecognition(128);
            MatlabInit.EmgRecognition = windRecogn;
            MatlabInit.StartEmgRecognition();

        }

        #endregion

        #region Stop Recognition

        public void StopRecognition()
        {
            MatlabInit.EmgRecognition = windRecogn;
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
