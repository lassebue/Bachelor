using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataOpsamlingTest
{
    class Controller
    {
        private ICommand _saveEmgDataCommand;

        public ICommand SaveEmgDataCommandHandler
        {
            get { return _saveEmgDataCommand ?? (_saveEmgDataCommand = new RelayCommand(SaveEmgData)); }
        }

        private void SaveEmgData()
        {
            var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            
            //sprintList.Add(new SprintBacklog() { Name = sprintList.SprintBackName });
        }
    }
}
