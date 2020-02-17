using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analyst.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Stopwatch stopWatch = new Stopwatch();
            Stopwatch stopWatch = Stopwatch.StartNew();
            stopWatch.Start();
            Thread.Sleep(75 * 1000);//1000 --> 1 second
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
