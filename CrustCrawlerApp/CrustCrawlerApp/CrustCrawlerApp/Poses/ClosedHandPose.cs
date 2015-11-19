using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class ClosedHandPose: Pose
    {
       

        public ClosedHandPose()
        {
            _poseName = "CloseHand";
            _poseId = 1;
        }


        public override void PoseAction(IPoseHandlerMkI handler)
        {
            CallClosedHandAction((IPoseHandlerMkII)handler);
        }
        public void CallClosedHandAction(IPoseHandlerMkII handler)
        {
            handler.ClosedHandPoseAction();
        }
    }
}
