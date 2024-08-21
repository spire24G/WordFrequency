namespace WordFrequencyApp.Writer;

public interface IWriter
{
    bool WriteData(IReadOnlyCollection<string> data, string outPutPath);
}