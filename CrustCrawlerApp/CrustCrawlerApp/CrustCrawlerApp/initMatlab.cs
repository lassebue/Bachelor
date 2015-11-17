using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CrustCrawlerApp.WindControl;

namespace CrustCrawlerApp
{
    public class InitMatlab
    {
        public delegate void OrientationEventHandler(object sender, OrientationEventArgs e);

        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker1 = new BackgroundWorker();

        private readonly BackgroundWorker worker2 = new BackgroundWorker();

        private MLApp.MLApp matlab; // = new MLApp.MLApp();
        private readonly IDisplayPose mv;


        // Inits the matlab server and start the predictions of the server 
        public InitMatlab(IDisplayPose mv)
        {
            this.mv = mv;

            // Inits the matlab server and start the predictions of the server 
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

            //worker1.DoWork += worker_DoWork1;
            //worker1.RunWorkerCompleted += worker1_RunWorkerCompleted;
            //worker1.RunWorkerAsync();

            //worker2.DoWork += worker_DoWork1;
            //worker2.RunWorkerCompleted += worker1_RunWorkerCompleted;
            //worker2.RunWorkerAsync();
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
            }
        }

        public void StopEmgRecognition()
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled -= RecognizeEmgWindow;
                EmgRecognition.Dispose();
            }
        }

        private void RecognizeEmgWindow(object sender, EmgWindEventArgs e)
        {
            object result = null;


            //var time0 = DateTime.UtcNow;

            mv.WindowCount++;

            //var inteval = (time0 - e.WindowTime).Milliseconds;

            matlab.Feval("posePredictor", 1, out result, e.EmgWindow.ElementAt(0),
                e.EmgWindow.ElementAt(1),
                e.EmgWindow.ElementAt(2),
                e.EmgWindow.ElementAt(3),
                e.EmgWindow.ElementAt(4),
                e.EmgWindow.ElementAt(5),
                e.EmgWindow.ElementAt(6),
                e.EmgWindow.ElementAt(7));


            //var time1 = DateTime.UtcNow;
            //var calTime = time1 - time0;

            var res = result as object[];
            mv.CurrentPose = "The current pose is: " + (string) res[0];

            switch ((string) res[0])
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
        }

        //}
        //{

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //}
        //{

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
    }
}