using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class RelaxedHandPose:Pose
    {
        public RelaxedHandPose()
        {
            _poseName = "RelaxedHand";
            _poseId = 2;
        }

        public override void PoseAction(IPoseHandlerMkI handler)
        {
            CallRelaxedHandAction((IPoseHandlerMkII)handler);
        }

        public void CallRelaxedHandAction(IPoseHandlerMkII handler)
        {
            handler.RelaxedHandPoseAction();
        }
    }
}
