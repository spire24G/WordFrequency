using System.Collections.Concurrent;

namespace WordFrequencyApp.Converter;

public static class FrequencyConverter
{
    public static IReadOnlyCollection<string> ConvertFromDictionary(ConcurrentDictionary<string, int> data)
    {
        List<string> result = new();

        while (!data.IsEmpty)
        {
            KeyValuePair<string, int> keyAndValue = data.MaxBy(x => x.Value);
            result.Add(GetOutputMessage(keyAndValue.Key, keyAndValue.Value));
            data.Remove(keyAndValue.Key, out _);
        }

        return result;
    }

    private static string GetOutputMessage(string key, int value)
    {
        return $"{key},{value}";
    }
}