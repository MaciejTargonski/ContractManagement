using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using ContractManagement.WebApi.App_Start;

namespace ContractManagement.WebApi.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //needed for local MDF file for database stored in other projects folder.
            //Need to point DataDir to the correct folder
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, @"App_Data"));

            string baseAddress = "http://localhost:24454/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Starting API at: " + baseAddress);
                Console.ReadKey();
                Console.WriteLine("Stopping...");
            }
        }
    }
}
