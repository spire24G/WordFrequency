namespace WordFrequencyApp.Reader;

public interface IFrequencyReader
{
    bool TryReadAndComputeFrequencies(string fileNamePath,
        out IReadOnlyCollection<string> readOnlyCollection);
}