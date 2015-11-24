using CrustCrawlerApp.Poses;

namespace CrustCrawlerApp
{
    public interface IPoseListener
    {
        void StartListening(IListenToRecognition recogn);
        void StopListening(IListenToRecognition recogn);
    }

    public class CCManagement : IPoseHandlerMkI, IPoseListener
    {
        private readonly IMatlab matlab;
        private readonly int speed = 120;

        //private IListenToRecognition recognition;

        public CCManagement(IMatlab matlab)
        {
            this.matlab = matlab;
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


        public void StartListening(IListenToRecognition recognition)
        {
            recognition.PoseRecognized += RecognizedPose;
        }

        public void StopListening(IListenToRecognition recognition)
        {
            recognition.PoseRecognized += RecognizedPose;
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
    }
}