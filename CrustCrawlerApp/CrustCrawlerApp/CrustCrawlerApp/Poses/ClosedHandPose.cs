using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class ClosedHandPose: Pose
    {
        public ClosedHandPose(string poseName, int poseId )   : base( poseName, poseId )
        {

        }

        public void CallClosedHandAction(IPoseHandlerMkII handler)
        {
            handler.ClosedHandPoseAction();
        }
    }
}
