using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using CrustCrawlerApp.WindControl;
using System.Threading;

namespace CrustCrawlerApp
{
    public class InitMatlab
    {
        public delegate void DoWorkEventHandler(object sender, DoRecognitionEventArgs e);

        public delegate void OrientationEventHandler(object sender, OrientationEventArgs e);

        private ThreadLocal<List<Array>> emgWindowThreadData;

        private readonly IDisplayPose mv;

        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();


        //private DateTime interval;
        private DateTime executeStartTime;

        private MLApp.MLApp matlab; // = new MLApp.MLApp();
        private readonly TimeSpan thresshold = new TimeSpan(0, 0, 0, 1, 400);


        // Inits the matlab server and start the predictions of the server 
        public InitMatlab(IDisplayPose mv)
        {
            this.mv = mv;
           
            // Inits the matlab server and start the predictions of the server 
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();



        }

        public EmgWindowRecognition EmgRecognition { get; set; }

        public void OpenClaw()
        {
            object result = null;

            try
            {
                matlab.Feval("OpenClaw", 0, out result, 70);
            }
            catch (Exception)
            {
            }
        }

        public void CloseClaw()
        {
            object result = null;

            try
            {
                matlab.Feval("CloseClaw", 0, out result, 70);
            }
            catch (Exception)
            {
            }
        }


        public void StartEmgRecognition()
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled += RecognizeEmgWindow;
                //worker.DoWork += worker_DoRecognition;
                //worker.DoWork += (obj, e) => worker_DoRecognition(  emg1,
                //                                                    emg2,
                //                                                    emg3,
                //                                                    emg4,
                //                                                    emg5,
                //                                                    emg6,
                //                                                    emg7,
                //                                                    emg8);


                worker.DoWork += worker_DoRecognition;
                worker.RunWorkerCompleted += worker_RecognitionCompleted;
                worker2.DoWork += worker_DoRecognition;
                worker2.RunWorkerCompleted += worker2_RecognitionCompleted;
            }
        }

        public void StopEmgRecognition()
        {
            if (EmgRecognition != null)
            {

                EmgRecognition.WindowFilled -= RecognizeEmgWindow;
                worker.DoWork -= worker_DoRecognition;
                worker.RunWorkerCompleted -= worker_RecognitionCompleted;

                worker2.DoWork -= worker_DoRecognition;
                worker2.RunWorkerCompleted -= worker2_RecognitionCompleted;


                EmgRecognition.Dispose();
            }
        }


        private void RecognizeEmgWindow(object sender, EmgWindEventArgs e)
        {
            object result = null;


            var currentTime = DateTime.UtcNow;

            //if (mv.WindowCount == 0)
            //{
            //    executeStartTime = DateTime.UtcNow;
            //    var time0 = DateTime.UtcNow;

            //    matlab.Feval("posePredictor", 1, out result, e.EmgWindow.ElementAt(0),
            //        e.EmgWindow.ElementAt(1),
            //        e.EmgWindow.ElementAt(2),
            //        e.EmgWindow.ElementAt(3),
            //        e.EmgWindow.ElementAt(4),
            //        e.EmgWindow.ElementAt(5),
            //        e.EmgWindow.ElementAt(6),
            //        e.EmgWindow.ElementAt(7));


            //    var time1 = DateTime.UtcNow;
            //    var calTime = time1 - time0;

            //    var res = result as object[];
            //    mv.CurrentPose = "The current pose is: " + (string) res[0];

            //    switch ((string) res[0])
            //    {
            //        case "RightFingerSpreadBue":
            //            OpenClaw();
            //            break;

            //        case "RightClosedBue":
            //            CloseClaw();
            //            break;

            //        case "RightRelaxedBue":
            //            break;
            //    }
            //}
            //else
            //{
            //    var interval = (currentTime - executeStartTime);
            //    if (interval > thresshold)
            //    {
            //        executeStartTime = DateTime.UtcNow;
            //        var time0 = DateTime.UtcNow;

            //        matlab.Feval("posePredictor", 1, out result,
            //            e.EmgWindow.ElementAt(0),
            //            e.EmgWindow.ElementAt(1),
            //            e.EmgWindow.ElementAt(2),
            //            e.EmgWindow.ElementAt(3),
            //            e.EmgWindow.ElementAt(4),
            //            e.EmgWindow.ElementAt(5),
            //            e.EmgWindow.ElementAt(6),
            //            e.EmgWindow.ElementAt(7));


            //        mv.CurrentWindow = mv.WindowCount;

            //        var time1 = DateTime.UtcNow;
            //        var calTime = time1 - time0;

            //        var res = result as object[];
            //        mv.CurrentPose = "The current pose is: " + (string) res[0];

            //        switch ((string) res[0])
            //        {
            //            case "RightFingerSpreadBue":
            //                OpenClaw();
            //                break;

            //            case "RightClosedBue":
            //                CloseClaw();
            //                break;

            //            case "RightRelaxedBue":
            //                break;
            //        }
            //    }
            //}
            

            

            var time0 = DateTime.UtcNow;




            emgWindowThreadData = new ThreadLocal<List<Array>>(() => e.EmgWindow);

            if (myWindow==0)
            {

                worker.RunWorkerAsync();
                mv.CurrentWindow = mv.WindowCount;
            }

            //mv.WindowCount++;
            myWindow++;
            
        }

        private int myWindow =0;

        private void worker_DoRecognition(object sender, DoWorkEventArgs e)
        {
            isMatlabExecuting = true;

            object result = null;
            var time0 = DateTime.UtcNow;
            var EmgWindow = emgWindowThreadData.Value;

            matlab.Feval("posePredictor", 1, out result,
                        EmgWindow.ElementAt(0),
                        EmgWindow.ElementAt(1),
                        EmgWindow.ElementAt(2),
                        EmgWindow.ElementAt(3),
                        EmgWindow.ElementAt(4),
                        EmgWindow.ElementAt(5),
                        EmgWindow.ElementAt(6),
                        EmgWindow.ElementAt(7));

            mv.CurrentWindow = myWindow;

            var time1 = DateTime.UtcNow;
            var calTime = time1 - time0;

            var res = result as object[];

            e.Result = (string)res[0];
            mv.CurrentPose = "Pose:  " + e.Result;


        }


        private bool isMatlabExecuting = false;

        private void worker_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            var res = (string) e.Result;

            switch (res)
            {
                case "RightFingerSpreadBue":
                    OpenClaw();
                    
                    break;

                case "RightClosedBue":
                    CloseClaw();
                    break;

                case "RightRelaxedBue":
                    break;
            }

            worker2.RunWorkerAsync();
        }
        
        private void worker2_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var res = (string)e.Result;

            switch (res)
            {
                case "RightFingerSpreadBue":

                    OpenClaw();
                    break;

                case "RightClosedBue":
                    CloseClaw();
                    break;

                case "RightRelaxedBue":
                    break;
            }
            worker.RunWorkerAsync();

        }


        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = new MLApp.MLApp();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            matlab = ((MLApp.MLApp) e.Result);

            var currentDir = Directory.GetCurrentDirectory();

            var folders = currentDir.Split('\\');

            var folderPath = "";

            var targFolderNum = folders.Length - 4;
            for (var i = 0; i < targFolderNum; i++)
            {
                folderPath = folderPath + "\\" + folders[i];
                if (i == 0)
                    folderPath = folders[0];
            }

            var dirBefore = Directory.GetDirectories(folderPath);
            var path = dirBefore[0];

            // Change to the directory where the matlab function folder is located 
            var changeDir = @"cd('" + path + "')";
            matlab.Execute(changeDir);

            //Load Library Dynamixel.dll
            object result = null;
            matlab.Feval("LoadLib", 0, out result);
            
            worker.DoWork -= worker_DoWork;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
        }
        public class DoRecognitionEventArgs : DoWorkEventArgs
        {
            public DoRecognitionEventArgs(object argument, List<Array> emgWindow ) : base(argument)
            {
                _emgWindow = emgWindow;
            }

            private List<Array> _emgWindow;

            public List<Array> EmgWindow
            {
                get { return _emgWindow; }
            }
 
        }

    }

}