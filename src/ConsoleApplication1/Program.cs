using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                try
                {
                    Engine engine = new Engine(args[0]);
                    engine.Run();
                }
                catch (Exception e)
                {
                    Log.WriteLine("Error : " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Usage: testtool.exe [config file path]");
            }
            Log.Close();
        }
    }
}
