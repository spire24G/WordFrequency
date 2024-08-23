using System.Collections.Concurrent;

namespace WordFrequencyApp.Converter;

public static class FrequencyConverter
{
    public static IReadOnlyCollection<string> ConvertFromDictionary(ConcurrentDictionary<string, int> data)
    {
        List<string> result = new List<string>();

        while (data.Count > 0)
        {
            KeyValuePair<string, int> keyAndValue = data.MaxBy(x => x.Value);
            result.Add(GetMessage(keyAndValue.Key, keyAndValue.Value));
            data.Remove(keyAndValue.Key, out _);
        }

        return result;
    }

    private static string GetMessage(string key, int value)
    {
        return $"{key},{value}";
    }
}