using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class OpenHandPose: Pose
    {
        public OpenHandPose(string poseName, int poseId )   : base( poseName, poseId )
        {

        }

        public void CallOpenHandAction(IPoseHandlerMkII handler)
        {
            handler.OpenHandPoseAction();
        }
    }
}
