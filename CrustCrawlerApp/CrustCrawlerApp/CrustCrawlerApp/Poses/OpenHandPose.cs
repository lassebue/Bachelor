using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class OpenHandPose: Pose
    {
        public OpenHandPose()
        {
            _poseName = "OpenHand";
            _poseId = 0;
        }

        public override void PoseAction(IPoseHandlerMkI handler)
        {
            CallOpenHandAction(handler);
        }

        public void CallOpenHandAction(IPoseHandlerMkI handler)
        {
            handler.OpenHandPoseAction();
        }
    }
}
