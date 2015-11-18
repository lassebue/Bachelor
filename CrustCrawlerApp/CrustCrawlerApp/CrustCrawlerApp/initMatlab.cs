using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CrustCrawlerApp.WindControl;

namespace CrustCrawlerApp
{
    public class InitMatlab
    {
        private readonly IDisplayPose mv;                                       //Flyttet -> Recogn & Matlab
        private readonly BackgroundWorker worker = new BackgroundWorker();      //Flyttet -> Recogn & Matlab
        private readonly BackgroundWorker worker2 = new BackgroundWorker();     //Flyttet -> Recogn
        private ThreadLocal<List<Array>> emgWindowThreadData;                   //Flyttet -> Recogn
        private MLApp.MLApp matlab; // = new MLApp.MLApp();                     //Flyttet -> CCM & Matlab & Recogn
        private int myWindow;                                                   //Flyttet -> Recogn


        // Inits the matlab server and start the predictions of the server 
        public InitMatlab(IDisplayPose mv)                                      //Flyttet -> Matlab
        {
            this.mv = mv;
 
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        public EmgWindowRecognition EmgRecognition { get; set; }                //Flyttet -> Recogn

        public void OpenClaw()                                                  //Flyttet -> CCM
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

        public void CloseClaw()                                                 //Flyttet -> CCM
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

        public void StartEmgRecognition()                                       //Flyttet -> Recogn
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled += RecognizeEmgWindow;

                worker.DoWork += worker_DoRecognition;
                worker.RunWorkerCompleted += worker_RecognitionCompleted;
                worker2.DoWork += worker_DoRecognition;
                worker2.RunWorkerCompleted += worker2_RecognitionCompleted;
            }
        }

        public void StopEmgRecognition()                                        //Flyttet -> Recogn
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled -= RecognizeEmgWindow;
                worker.DoWork -= worker_DoRecognition;
                worker.RunWorkerCompleted -= worker_RecognitionCompleted;

                worker2.DoWork -= worker_DoRecognition;
                worker2.RunWorkerCompleted -= worker2_RecognitionCompleted;

                myWindow = 0;

                EmgRecognition.Dispose();
            }
        }


        private void RecognizeEmgWindow(object sender, EmgWindEventArgs e)      //Flyttet -> Recogn
        {
            object result = null;

            emgWindowThreadData = new ThreadLocal<List<Array>>(() => e.EmgWindow);

            if (myWindow == 0)
            {
                worker.RunWorkerAsync();
                mv.CurrentWindow = mv.WindowCount;
                myWindow++;
            }
        }

        private void worker_DoRecognition(object sender, DoWorkEventArgs e)     //Flyttet -> Recogn
        {
            object result = null;
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

            mv.CurrentWindow = mv.WindowCount;

            var res = result as object[];

            e.Result = (string) res[0];
            mv.CurrentPose = "Pose:  " + e.Result;
        }

        private void ActionOnPose(string pose, BackgroundWorker worker)         //Flyttet -> Recogn
        {
            switch (pose)
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

        private void worker_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e) //Flyttet -> Recogn
        {
            var res = (string) e.Result;
            ActionOnPose(res, worker2);
        }

        private void worker2_RecognitionCompleted(object sender, RunWorkerCompletedEventArgs e) //Flyttet -> Recogn
        {
            var res = (string) e.Result;
            ActionOnPose(res, worker);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)            //Flyttet -> Matlab
        {
            e.Result = new MLApp.MLApp();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //Flytte -> Matlab
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
    }
}