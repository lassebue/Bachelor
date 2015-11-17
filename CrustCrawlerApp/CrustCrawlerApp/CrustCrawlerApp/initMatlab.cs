using CrustCrawlerApp.WindControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{


    public class InitMatlab
    {
        private MLApp.MLApp matlab;// = new MLApp.MLApp();
        private IDisplayPose mv;

        private readonly BackgroundWorker worker = new BackgroundWorker();

        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        private readonly BackgroundWorker worker1 = new BackgroundWorker();


        public delegate void OrientationEventHandler(object sender, OrientationEventArgs e);


        // Inits the matlab server and start the predictions of the server 
        public InitMatlab( IDisplayPose mv )
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

        public void OpenClaw()
        {
            object result = null;

            try
            {
                matlab.Feval("OpenClaw", 0, out result, 70);
            }
            catch (Exception){}
        }

        public void CloseClaw()
        {
            object result = null;

            try
            {
                matlab.Feval("CloseClaw", 0, out result, 70);
            }
            catch (Exception) { }
        }

        private EmgWindowRecognition _emgRecognition;
        public EmgWindowRecognition EmgRecognition
        {
            get { return _emgRecognition; }
            set { _emgRecognition = value; }
        }

        
        public void StartEmgRecognition(  )
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled += new EmgWindowEventHandler(RecognizeEmgWindow);
            }
            else { }
        }

        public void StopEmgRecognition()
        {
            if (EmgRecognition != null)
            {
                EmgRecognition.WindowFilled -= new EmgWindowEventHandler(RecognizeEmgWindow);
                EmgRecognition.Dispose();
            }
            else { }
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

            object[] res = result as object[];
            mv.CurrentPose = "The current pose is: " + (string)res[0];

            switch ((string)res[0])
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
            matlab = ((MLApp.MLApp)e.Result);

            var currentDir = Directory.GetCurrentDirectory();

            var folders = currentDir.Split('\\');

            var folderPath = "";

            var targFolderNum = folders.Length - 4;
            for (int i = 0; i < targFolderNum; i++)
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

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //}

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}

    }
}
