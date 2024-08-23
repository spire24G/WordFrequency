namespace WordFrequencyApp.Reader;

public interface IFrequencyReader
{
    IReadOnlyCollection<string> ReadAndComputeFrequencies(string fileNamePath);
}