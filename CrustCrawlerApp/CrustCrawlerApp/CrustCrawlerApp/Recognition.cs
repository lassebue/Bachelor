using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CrustCrawlerApp.Poses;
using CrustCrawlerApp.WindControl;

namespace CrustCrawlerApp
{
    public interface IListenToRecognition
    {
        event PoseRecognizedHandler PoseRecognized;
    }

    public class Recognition : IListenToRecognition
    {
        private readonly IMatlab matlab;
        private readonly IDisplayPose mv;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();

        private ThreadLocal<List<Array>> emgWindowThreadData;
        private List<int> modelPoseIds;
        private List<Pose> modelPoseList;

        private int myWindow;
        private readonly List<Pose> poseList;
        private readonly IPoseListener poseListener;

        public Recognition(IDisplayPose mv, IPoseListener poseListener, IMatlab matlab)
        {
            this.mv = mv;
            this.poseListener = poseListener;
            this.matlab = matlab;
            modelPoseIds = new List<int>();

            // List of poses to compare with matlab recognition result. If new poses are added to the system, they should be added here too!
            poseList = new List<Pose> {new OpenHandPose(), new ClosedHandPose(), new RelaxedHandPose()};
        }

        public EmgWindowRecognition WindowRecognition { get; set; }

        public event PoseRecognizedHandler PoseRecognized;

        protected virtual void OnPoseRecognized(PoseRecognizedEventArgs e)
        {
            if (PoseRecognized != null)
                PoseRecognized(this, e);
        }

        private void ActionOnPose(int poseId, BackgroundWorker worker)
        {
            mv.CurrentWindow = mv.WindowCount;

            var pose = poseList.Find(x => x.PoseId == poseId);

            mv.CurrentPose = "Pose: " + pose.PoseName;

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
            emgWindowThreadData = new ThreadLocal<List<Array>>(() => e.EmgWindow);

            if (myWindow == 0)
            {
                //setModelPoseIds();
                worker.RunWorkerAsync();
                mv.CurrentWindow = mv.WindowCount;
                myWindow++;
            }
        }

        //private void setModelPoseIds()
        //{
        //    var res = matlab.MatlabZeroParam("getModelPoseIds", 1);
        //    var objArray = res as object[];
        //    System.Array idArray = objArray[0] as System.Array;
        //    var length = idArray.Length;
        //    int[] pos = new int[2];

        //    int id;

        //    for (int i = 0; i < length; i++)
        //    {
        //        pos = new int[2] { 0, i };
        //        id = (int)((double)idArray.GetValue(pos));

        //        modelPoseList.Add(poseList.Find(x => x.PoseId == id));
        //    }

        //}

        private void worker_DoRecognition(object sender, DoWorkEventArgs e)
        {
            var emgWindow = emgWindowThreadData.Value;

            var res = matlab.MatlabNineParam("posePredictor", 1,
                emgWindow.ElementAt(0),
                emgWindow.ElementAt(1),
                emgWindow.ElementAt(2),
                emgWindow.ElementAt(3),
                emgWindow.ElementAt(4),
                emgWindow.ElementAt(5),
                emgWindow.ElementAt(6),
                emgWindow.ElementAt(7),
                mv.OrientationValue
                );

            //var res = matlab.MatlabEightParam("posePredictor2", 1,
            //    emgWindow.ElementAt(0),
            //    emgWindow.ElementAt(1),
            //    emgWindow.ElementAt(2),
            //    emgWindow.ElementAt(3),
            //    emgWindow.ElementAt(4),
            //    emgWindow.ElementAt(5),
            //    emgWindow.ElementAt(6),
            //    emgWindow.ElementAt(7));


            var result = res as object[];

            e.Result = (int) ((double) result[0]);
        }

        private void worker_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = (int) e.Result;
            ActionOnPose(res, worker2);
        }

        private void worker2_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = (int) e.Result;
            ActionOnPose(res, worker);
        }
    }
}