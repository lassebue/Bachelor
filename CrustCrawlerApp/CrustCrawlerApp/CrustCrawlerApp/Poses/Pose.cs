using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp.Poses
{
    class Pose
    {
        #region PoseName
        private string _poseName;
        public string PoseName
        {
            get { return _poseName; }
        }
        #endregion

        #region PoseId
        private int _poseId;
        public int PoseId   
        {
            get { return _poseId; }
        }
        #endregion

        public Pose( string poseName, int poseId)
        {
            _poseName   = poseName;
            _poseId = poseId;
        }
    }
}
