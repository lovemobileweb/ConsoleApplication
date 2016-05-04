using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    public class Log
    {
        static CsvFileWriter file = new CsvFileWriter(Environment.CurrentDirectory + "\\Log.csv");

        public static void Write(string log)
        {
            try
            {
                //using (CsvFileWriter file = new CsvFileWriter(Environment.CurrentDirectory + "\\Log.txt"))
                {
                    Console.WriteLine(log);
                    CsvRow row = new CsvRow();
                    row.Add(log);
                    file.WriteRow(row);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine("Logging error : " + e.Message);
            }
        }
        public static void WriteLine(string log)
        {
            Write(log);
        }
        public static void Close()
        {
            file.Close();
        }
    }
}
