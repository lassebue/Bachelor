using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatlabTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the MATLAB instance 
            Console.WriteLine("Starting Matlab...");
            
            MLApp.MLApp matlab = new MLApp.MLApp();
            
            Console.WriteLine("Done!");

            Console.WriteLine("Loading function...");



            var abe = Directory.GetCurrentDirectory();
            
            var tmpDir = Directory.GetParent(abe);

            int length = 3;
            for (int i = 0; i < length; i++)
            {
                tmpDir = tmpDir.Parent;
            }
            var bibi = tmpDir.GetDirectories();
            var bob = bibi[0];
            //buller.MoveTo("CCController");

            // Change to the directory where the function is located 
            //C:\Users\Lasse Bue Svendsen\Desktop\Matlab import\myfunc.m"
            var path = "cd('" + bob.FullName +"')";
            matlab.Execute(path);

            //matlab.Execute(@"cd('Z:\Bachelor\CrustCrawlerApp')");

            Console.WriteLine("Done!");
            
            // Define the output
            
            object result = null;

            //object result = null;

            matlab.Feval("LoadLib", 0, out result);

            var yes = "";
            while (yes!="yes")
            {
                Console.WriteLine("Write that degree!!");

                double buller = double.Parse(Console.ReadLine());
                Console.WriteLine("Executing function...");
                try
                {
                    //matlab.Execute(@"clear");

                    matlab.Feval("MoveServo", 0, out result, 7, buller, 50);
                }
                catch (Exception e)
                {
                    e.ToString();
                    //Console.WriteLine(e.ToString());
                }

                // Call the MATLAB function myfunc
                //matlab.Feval("checkEmgDataFunction", 1, out result,1

                Console.WriteLine("End session?");

                //// Display result 
                //object[] res = result as object[];
                yes = Console.ReadLine();
                //Console.WriteLine(res[0]);
            }
            
            try
            {
                matlab.Feval("UnloadLib", 0, out result);

            }
            catch (Exception)
            {
                
            }
        }
    }
}
