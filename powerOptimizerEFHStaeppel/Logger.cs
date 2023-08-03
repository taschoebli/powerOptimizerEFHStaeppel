using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel
{
    public class Logger
    {
        //private StreamWriter? _streamWriter;

        private string LogFilePath => $"PV_Log_{DateTime.Now.ToShortDateString()}.txt";
        //private StreamWriter Writer => _streamWriter ??= CreateStreamWriter();

        public void Log(string logMessage)
        {
            using (StreamWriter writer = CreateStreamWriter())
            {
                writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToShortDateString()} {logMessage}");
#if DEBUG
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToShortDateString()} {logMessage}");
#endif
            }
        }

        protected virtual StreamWriter CreateStreamWriter()
        {
            return File.AppendText(LogFilePath);
        }
    }
}
