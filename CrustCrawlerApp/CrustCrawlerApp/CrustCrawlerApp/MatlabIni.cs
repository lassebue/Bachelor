using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrustCrawlerApp
{
    class MatlabIni
    {
        MLApp.MLApp matlab;
        private readonly BackgroundWorker worker = new BackgroundWorker();


        // Inits the matlab server and start the predictions of the server 
        public void InitMatlab()
        {
            matlab = new MLApp.MLApp();
            //worker.DoWork += worker_DoWork;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }
    }
}
