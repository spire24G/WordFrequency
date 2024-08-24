using System.Collections.Concurrent;

namespace WordFrequencyApp.Reader;

public interface IFrequencyReader
{
    bool TryReadAndComputeFrequencies(
        string fileNamePath,
        out ConcurrentDictionary<string, int> readOnlyCollection);
}