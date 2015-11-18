using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{
    public interface IPoseHandlerMkI
    {
        public void RelaxedHandPoseAction();
        public void ClosedHandPoseAction();
        public void OpenHandPoseAction();

    }
}
