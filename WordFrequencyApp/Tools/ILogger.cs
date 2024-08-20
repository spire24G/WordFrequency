namespace WordFrequency.Tools;

public interface ILogger
{
    void Log(ELogType logType, object message);
}