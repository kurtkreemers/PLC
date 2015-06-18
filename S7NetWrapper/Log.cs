using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S7NetWrapper
{
    public class Log
    {
        public static void writeLog(string input)
        {
            string dirname = @"C:\Log\";
            string bestand = dirname + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
            try
            {
                Directory.CreateDirectory(dirname);
                using (var writestream = new StreamWriter(bestand,true))
                {

                    writestream.WriteLine( DateTime.Now.ToShortTimeString() + " : " + input);
                  
                }
            }
            catch (IOException)
            {

                throw new Exception("Error opening file!!!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
