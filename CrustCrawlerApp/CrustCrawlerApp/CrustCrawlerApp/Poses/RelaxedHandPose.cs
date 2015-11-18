using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class RelaxedHandPose:Pose
    {
        public RelaxedHandPose(string poseName, int poseId )   : base( poseName, poseId )
        {

        }

        public void CallRelaxedHandAction(IPoseHandlerMkII handler)
        {
            handler.RelaxedHandPoseAction();
        }
    }
}
