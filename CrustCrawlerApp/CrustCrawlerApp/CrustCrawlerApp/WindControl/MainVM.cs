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

namespace CrustCrawlerApp.WindControl
{
    public class MainVM: INotifyPropertyChanged
    {
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
