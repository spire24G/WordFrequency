namespace WordFrequencyApp.Calculator;

public class WordFrequencyCalculator : IWordFrequencyCalculator
{
    public Dictionary<string, int> FindWordFrequency(IReadOnlyCollection<string> data)
    {
        Dictionary<string, int> result = new Dictionary<string, int>(new StringIgnoreCaseComparer());

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

            return x.Equals(y,StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
    }
}