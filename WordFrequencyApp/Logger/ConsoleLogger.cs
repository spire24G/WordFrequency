using System.Text;

namespace WordFrequencyApp.Logger;

public class ConsoleLogger : ILogger
{
    private const string separator = " | ";
    public void Log(ELogType logType, object message)
    {
        StringBuilder sb = new();
        sb.Append(DateTime.Now.ToString("u"));
        sb.Append(separator);
        sb.Append(logType.ToString());
        sb.Append(separator);
        sb.Append(message);
        Console.WriteLine(sb.ToString());
    }
}