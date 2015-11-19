using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CrustCrawlerApp.WindControl;
using CrustCrawlerApp.Poses;

namespace CrustCrawlerApp
{
    public interface IListenToRecognition
    {
        event PoseRecognizedHandler PoseRecognized;
    }
    public class Recognition: IListenToRecognition
    {
        private readonly IDisplayPose mv;
        private IPoseListener poseListener;

        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();

        private ThreadLocal<List<Array>> emgWindowThreadData;

        private readonly Matlab matlab = new Matlab();

        private int myWindow;

        public Recognition(IDisplayPose mv, IPoseListener poseListener)
        {
            this.mv = mv;
            this.poseListener = poseListener;

        }

        public event PoseRecognizedHandler PoseRecognized;

        protected virtual void OnPoseRecognized(PoseRecognizedEventArgs e)
        {
            if (PoseRecognized != null)
                PoseRecognized(this, e);
        }

        public EmgWindowRecognition WindowRecognition { get; set; }

        private void ActionOnPose(string poseName, BackgroundWorker worker)
        {
            Pose pose = null;
            switch (poseName)
            {
                case "RightFingerSpreadBue":
                    pose = new OpenHandPose();
                    //ccm.OpenClaw();
                    break;

                case "RightClosedBue":
                    pose = new ClosedHandPose();
                    //ccm.CloseClaw();
                    break;

                case "RightRelaxedBue":
                    break;
            }
            OnPoseRecognized(new PoseRecognizedEventArgs(pose));
            worker.RunWorkerAsync();
        }

        public void StartEmgRecognition()
        {
            if (WindowRecognition != null)
            {
                WindowRecognition.WindowFilled += RecognizeEmgWindow;

                worker.DoWork += worker_DoRecognition;

                poseListener.StartListening(this);

                worker.RunWorkerCompleted += worker_RecognitionCompleted;
                worker2.DoWork += worker_DoRecognition;
                worker2.RunWorkerCompleted += worker2_RecognitionCompleted;
            }
        }

        public void StopEmgRecognition()
        {
            if (WindowRecognition != null)
            {
                WindowRecognition.WindowFilled -= RecognizeEmgWindow;
                worker.DoWork -= worker_DoRecognition;

                poseListener.StopListening(this);

                worker.RunWorkerCompleted -= worker_RecognitionCompleted;

                worker2.DoWork -= worker_DoRecognition;
                worker2.RunWorkerCompleted -= worker2_RecognitionCompleted;

                myWindow = 0;

                WindowRecognition.Dispose();
            }
        }

        private void RecognizeEmgWindow(object sender, EmgWindEventArgs e)
        {
            //object result = null;

            emgWindowThreadData = new ThreadLocal<List<Array>>(() => e.EmgWindow);

            if (myWindow == 0)
            {
                worker.RunWorkerAsync();
                mv.CurrentWindow = mv.WindowCount;
                myWindow++;
            }
        }

        private void worker_DoRecognition(object sender, DoWorkEventArgs e)
        {
            var emgWindow = emgWindowThreadData.Value;

            var res = matlab.MatlabEightParam("posePredictor", 1,
                emgWindow.ElementAt(0),
                emgWindow.ElementAt(1),
                emgWindow.ElementAt(2),
                emgWindow.ElementAt(3),
                emgWindow.ElementAt(4),
                emgWindow.ElementAt(5),
                emgWindow.ElementAt(6),
                emgWindow.ElementAt(7));

            var result = res as object[];

            e.Result = (string)result[0];
            mv.CurrentWindow = mv.WindowCount;

            mv.CurrentPose = "Pose:  " + e.Result;
        }

        private void worker_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = (string) e.Result;
            ActionOnPose(res, worker2);
        }

        private void worker2_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = (string) e.Result;
            ActionOnPose(res, worker);
        }
    }
}