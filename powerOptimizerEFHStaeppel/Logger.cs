using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using powerOptimizerEFHStaeppel.Interfaces;

namespace powerOptimizerEFHStaeppel
{
    public class Logger : ILogger
    {
        #region private Fields

        #endregion

        public Logger(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider;

            InitCulture();
        }

        #region Properties

        public string FolderName { get; } = "logFiles";

        public string FileNamePrefix { get; } = "log_pv_optimizer_";

        public string FileType { get; } = ".txt";

        public string LoggerDirectoryFullPath => CreateLoggerDirectoryInCurrentDirectory();

        public IList<string> MessageLineItems { get; } = new List<string>();

        private IDateTimeProvider DateTimeProvider { get; }

        private DateTime Now => DateTimeProvider.DateTimeNow;


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
            writer.WriteLine($"{Now.ToLongTimeString()} {Now.ToShortDateString()}");
            foreach (var messageLine in MessageLineItems)
            {
                writer.WriteLine(messageLine);
            }
#if DEBUG
            Console.WriteLine($"{Now.ToLongTimeString()} {Now.ToShortDateString()}");
            foreach (var messageLine in MessageLineItems)
            {
                Console.WriteLine(messageLine);
            }
#endif
            MessageLineItems.Clear();
        }

        private void InitCulture()
        {
            CultureInfo specificCulture = CultureInfo.CreateSpecificCulture("de-CH");
            specificCulture.DateTimeFormat.ShortDatePattern = "yyyy_MM_dd";
            specificCulture.DateTimeFormat.TimeSeparator = "-";

            CultureInfo.CurrentCulture = specificCulture;
        }

        private string GetLogFileFullPath()
        {
            var separator = Path.DirectorySeparatorChar;
            var result = $"{LoggerDirectoryFullPath}{separator}{FileNamePrefix}{Now.ToShortDateString()}{FileType}";

            return result;
        }

        #region Factory

        protected virtual StreamWriter CreateStreamWriter()
        {
            return File.AppendText(GetLogFileFullPath());
        }

        protected virtual string CreateLoggerDirectoryInCurrentDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryPath = string.Empty;

            if (currentDirectory != null)
            {
                var separator = Path.DirectorySeparatorChar;
                directoryPath = currentDirectory + separator + FolderName;
            }

            var directoryInfo = Directory.CreateDirectory(directoryPath);

            if(directoryInfo != null)
            {
                return directoryInfo.FullName;
            }

            return string.Empty;
        }

        #endregion

        #endregion

    }
}
