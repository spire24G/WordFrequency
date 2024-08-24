namespace WordFrequencyApp.Writer;

public interface IWriter
{
    bool WriteData(IEnumerable<string> data, string outPutPath);
}