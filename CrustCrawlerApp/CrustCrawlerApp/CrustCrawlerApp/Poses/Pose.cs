using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    public class Pose
    {
        #region PoseName
        protected string _poseName;
        public string PoseName
        {
            get { return _poseName; }
        }
        #endregion

        #region PoseId
        protected int _poseId;
        public int PoseId   
        {
            get { return _poseId; }
        }
        #endregion

        public Pose()
        {
        }

        public Pose( string poseName, int poseId)
        {
            _poseName   = poseName;
            _poseId = poseId;
        }

        public virtual void PoseAction(IPoseHandlerMkI handler)
        {
 
        }
    }
}
