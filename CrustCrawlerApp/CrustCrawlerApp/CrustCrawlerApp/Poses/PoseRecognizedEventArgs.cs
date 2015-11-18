using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{


    public delegate void PoseRecognizedHandler(object sender, PoseRecognizedEventArgs e);
    public class PoseRecognizedEventArgs : EventArgs
    {
        public PoseRecognizedEventArgs(Pose pose)
        {
            _pose = pose; 
        }
        
        private Pose _pose;
        public Pose pose
        {
            get { return _pose; }
        }

    }
}
