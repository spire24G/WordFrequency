using System.Collections.Concurrent;

namespace WordFrequencyApp.Converter;

public static class FrequencyConverter
{
    public static IEnumerable<string> ConvertToFrequencyOrderListFromDictionary(ConcurrentDictionary<string, int> data)
    {

        return data
            .OrderByDescending(x => x.Value)
            .Select(x => GetOutputMessage(x.Key, x.Value));
    }

    private static string GetOutputMessage(string key, int value)
    {
        return $"{key},{value}";
    }
}