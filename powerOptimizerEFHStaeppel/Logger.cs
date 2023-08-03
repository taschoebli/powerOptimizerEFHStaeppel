using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel
{
    public class Logger : ILogger
    {
        #region private Fields
        
        #endregion

        #region Properties

        public IList<string> MessageLineItems { get; } = new List<string>();

        private string LogFilePath => $"PV_Log_{DateTime.Now.ToShortDateString()}.txt";

        #endregion

        #region Methods

        public void AddMessageLine(string message)
        {
            if (message != null)
            {
                MessageLineItems?.Add(message);
            }
        }

        public void WriteMessagesToLogfile()
        {
            using StreamWriter writer = CreateStreamWriter();
            writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToShortDateString()}");
            foreach ( var messageLine in MessageLineItems )
            {
                writer.WriteLine( messageLine );
            }
#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToShortDateString()}");
            foreach (var messageLine in MessageLineItems)
            {
                Console.WriteLine(messageLine);
            }
#endif
            MessageLineItems.Clear();
        }

        #region Factory

        protected virtual StreamWriter CreateStreamWriter()
        {
            return File.AppendText(LogFilePath);
        }

        #endregion

        #endregion

    }
}
