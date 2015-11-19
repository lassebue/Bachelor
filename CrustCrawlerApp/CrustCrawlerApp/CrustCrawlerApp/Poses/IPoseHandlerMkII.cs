using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    public interface IPoseHandlerMkII : IPoseHandlerMkI
    {
        void VictoryHandPose();
        void HeldLeftHandPose();
    }
}
