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
        MLApp.MLApp matlab = new MLApp.MLApp();

        // Inits the matlab server and start the predictions of the server 
        public InitMatlab()
        {
            string path = "cd('" +
                          @"Z:\Users\KSG\Google Drev\Dokumenter\7.Semester\Bachelor\Bachelor\CrustCrawlerApp\CCController" +
                          "')";

            matlab.Execute(path);
            object result = null;
            matlab.Feval("LoadLib", 0, out result);

            
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
    }
}
