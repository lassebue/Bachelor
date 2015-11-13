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



    public class MainVM: INotifyPropertyChanged
    {
            // Skal ikke initeres her!!!
        private InitMatlab _im = new InitMatlab();

        public InitMatlab MatlabInit
        {
            get { return _im; }
            set { _im = value; }
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
            var myoControl =  new MyoController(this);
            
            MessageBox.Show("Orientation stuff wtf!!!!");

            myoControl.Dispose();

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

        private string _orientation = "";
        public string Orientation
        {
            get { return _orientation; }
            set { 
                _orientation = value;
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
