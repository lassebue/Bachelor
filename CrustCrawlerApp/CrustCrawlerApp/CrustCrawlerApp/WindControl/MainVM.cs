using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrustCrawlerApp.WindControl
{
    public class MainVM
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
            //MessageBox.Show("Save stuff wtf!!!!");
        }
        #endregion


    }
}
