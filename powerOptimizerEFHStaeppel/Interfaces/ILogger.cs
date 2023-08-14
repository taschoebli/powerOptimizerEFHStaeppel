namespace powerOptimizerEFHStaeppel.Interfaces
{
    public interface ILogger
    {
        string FolderName { get; }

        string FileNamePrefix { get;  }

        string FileType { get;  }

        string LoggerDirectoryFullPath { get; }

        IList<string> MessageLineItems { get; }

        void AddMessageLine(string message);

        void WriteMessagesToLogfile();
    }
}