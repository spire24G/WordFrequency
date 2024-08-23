namespace WordFrequencyApp.Logger;

public interface ILogger
{
    void Log(ELogType logType, object message);
}