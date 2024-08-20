using System.Text;

namespace WordFrequency.Tools;

public class Logger : ILogger
{
    private const string separator = " | ";
    public void Log(ELogType logType, object message)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(GetLogTime());
        sb.Append(separator);
        sb.Append(logType.ToString());
        sb.Append(separator);
        sb.Append(message);
        Console.WriteLine(sb.ToString());
    }

    private string GetLogTime()
    {
        return DateTime.Now.ToString("u");
    }
}