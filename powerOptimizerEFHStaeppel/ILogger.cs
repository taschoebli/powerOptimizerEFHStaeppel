namespace powerOptimizerEFHStaeppel
{
    public interface ILogger
    {
        IList<string> MessageLineItems { get; }

        void AddMessageLine(string message);

        void WriteMessagesToLogfile();
    }
}