using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace CrustCrawlerApp
{
    public class Matlab
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public MLApp.MLApp matlab;

        // Inits the matlab server and start the predictions of the server 
        public Matlab()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
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

        public object MatlabFourParam(string funcName, int noOfOutputs, object param1, object param2, object param3)
        {
            object res = null;
            try
            {
                matlab.Feval(funcName, noOfOutputs, out res, param1, param2, param3);
            }
            catch (Exception)
            {
                //NB! Unknown exeption which causes no damage
            }
            return res;
        }

        public object MatlabZeroParam(string funcName, int noOfOutputs)
        {
            object res = null;
            try
            {
                matlab.Feval(funcName, noOfOutputs, out res);
            }
            catch (Exception e )
            {
                e.ToString();
                //NB! Unknown exeption which causes no damage
            }
            return res;
        }
        public object MatlabOneParam(string funcName, int noOfOutputs, object param1)
        {
            object res = null;
            try
            {
                matlab.Feval(funcName, noOfOutputs, out res, param1);
            }
            catch (Exception)
            {
                //NB! Unknown exeption which causes no damage
            }
            return res;
        }

        public object MatlabEightParam(string funcName, int noOfOutputs, object param1, object param2, object param3, object param4, object param5, object param6, object param7, object param8)
        {
            object res = null;
            try
            {
                matlab.Feval(funcName, noOfOutputs, out res, param1, param2, param3, param4, param5, param6, param7, param8);
            }
            catch (Exception e)
            {
                e.ToString();
                //NB! Unknown exeption which causes no damage
            }
            return res;
        }

        public object MatlabNineParam(string funcName, int noOfOutputs, object param1, object param2, object param3, object param4, object param5, object param6, object param7, object param8, object param9 )
        {
            object res = null;
            try
            {
                matlab.Feval("posePredictor", noOfOutputs, out res, param1, param2, param3, param4, param5, param6, param7, param8, param9);
            }
            catch (Exception)
            {
                //NB! Unknown exeption which causes no damage
            }
            return res;
        }
    }
}