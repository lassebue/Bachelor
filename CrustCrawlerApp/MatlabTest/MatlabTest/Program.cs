﻿using System;
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

            matlab.Execute(@"cd('Z:\Bachelor\CrustCrawlerApp')");
            




            Console.WriteLine("Done!");
            
            // Define the output
            
            object result = null;

            //object result = null;
            Console.WriteLine("Executing function...");
            try
            {
                matlab.Execute(@"clear");

                matlab.Feval("LoadLib", 0, out result);
                matlab.Feval("MoveServo",0,out result, 90,6,50);
                matlab.Feval("UnloadLib",0, out result);
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }

            // Call the MATLAB function myfunc
            //matlab.Feval("checkEmgDataFunction", 1, out result,1);
            

            Console.WriteLine("Done!");


            //// Display result 
            //object[] res = result as object[];

            //Console.WriteLine(res[0]);
            Console.ReadLine(); 
        }
    }
}