using System.Collections.Concurrent;

namespace WordFrequencyApp.FrequencyCalculator;

public interface IWordFrequencyCalculator
{
    public ConcurrentDictionary<string, int> ComputeDictionaryOfFrequencies(
        ConcurrentDictionary<string, int> allFrequencies,
        IReadOnlyCollection<List<string>> currentAllLines, 
        int numberOfPackage);
}