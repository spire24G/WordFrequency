using System.Collections.Concurrent;
using WordFrequencyApp.Helpers;

namespace WordFrequencyApp.FrequencyCalculator;

public class WordFrequencyCalculator : IWordFrequencyCalculator
{
    public ConcurrentDictionary<string, int> ComputeDictionaryOfFrequencies(
        ConcurrentDictionary<string, int> allFrequencies,
        IReadOnlyCollection<List<string>> currentAllLines, 
        int degreeOfParallelism)
    {
        return
            currentAllLines
                .AsParallel()
                .WithDegreeOfParallelism(degreeOfParallelism) 
                .Select(FindWordFrequency)
                .Aggregate(allFrequencies, (acc, dictionary) =>
                {
                    DictionaryHelper.MergeDictionaries(acc, dictionary);
                    return acc;
                });
    }

    internal Dictionary<string, int> FindWordFrequency(IReadOnlyCollection<string> data)
    {
        Dictionary<string, int> result = new(new StringIgnoreCaseComparer());

        foreach (string line in data)
        {
            string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (string word in words)
            {
                if (!result.TryAdd(word, 1))
                    result[word]++;
            }
        }

        return result;
    }

    public class StringIgnoreCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            if (y == null) return false;

            return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
    }
}