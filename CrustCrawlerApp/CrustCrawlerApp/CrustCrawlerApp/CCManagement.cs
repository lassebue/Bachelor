using CrustCrawlerApp.Poses;
using System;
namespace CrustCrawlerApp
{
    public class CCManagement: IPoseHandlerMkI, IDisposable
    {
        private readonly Matlab matlab = new Matlab();
        private readonly int speed = 120;

        private IListenToRecognition _mv;

        public CCManagement(IListenToRecognition mv)
        {
            _mv = mv;
            mv.PoseRecognized += RecognizedPose;
        }

        public void OpenClaw()
        {
            matlab.MatlabOneParam("OpenClaw", 0, speed);
        }

        public void CloseClaw()
        {
            matlab.MatlabOneParam("CloseClaw", 0, speed);
        }

        public void RecognizedPose(object sender, PoseRecognizedEventArgs e)
        {
            e.pose.PoseAction(this);
        }
        public void RelaxedHandPoseAction()
        {
            //throw new System.NotImplementedException();
        }

        public void ClosedHandPoseAction()
        {
            CloseClaw();
        }

        public void OpenHandPoseAction()
        {
            OpenClaw();
        }

        public void Dispose()
        {
            _mv.PoseRecognized -= RecognizedPose;
        }
    }
}