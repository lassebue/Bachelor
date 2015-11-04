using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EmgDataModel;


namespace DataOpsamlingTest
{
    public class NpController : INotifyPropertyChanged
    {
        private string _newPoseName = "";
        public string NewPoseName
        {
            get { return _newPoseName; }
            set
            {
                _newPoseName = value;
            }
        }

        #region newPose
        private ICommand _savePoseCommand;

        public ICommand SavePoseCommandHandler
        {
            get { return _savePoseCommand ?? (_savePoseCommand = new RelayCommand(SavePose)); }
        }

        private void SavePose()
        {
            //var sprintList = ((IEmgSaver)Application.Current.FindResource("SprintListModel"));
            
            var poseCol = ((PoseCollection)Application.Current.FindResource("poseCollection"));

            var newP = true;
            var highestId = -1;
            foreach (var item in poseCol.Poses)
            {
                if (NewPoseName == item.PoseName)
                {
                    newP = false;
                    break;
                }
                if (highestId < item.PoseId)
                {
                    highestId = item.PoseId;
                }
            }
            if (newP)
	        {
                var newPose = new Pose();
                highestId++;
                newPose.PoseId = highestId;
                newPose.PoseName = NewPoseName;
                poseCol.Poses.Add(newPose);
                SavePoseOnline(newPose);
                
	        }
            MessageBox.Show("Save Pose!");
        }
        #endregion

        private async void SavePoseOnline(Pose newPose)
        {
            await newPose.SaveAsync();

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
