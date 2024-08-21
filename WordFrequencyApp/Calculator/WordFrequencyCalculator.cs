namespace WordFrequencyApp.Calculator;

public class WordFrequencyCalculator : IWordFrequencyCalculator
{
    public Dictionary<string, int> FindWordFrequency(IReadOnlyCollection<string> data)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        
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
}