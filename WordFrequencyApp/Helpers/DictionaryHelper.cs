using System.Collections.Concurrent;

namespace WordFrequencyApp.Helpers;

public static class DictionaryHelper
{
    /// <summary>
    /// <paramref name="baseDictionary"/> will receive <paramref name="dictionaryToMerge"/> data
    /// If the key already exists, we sum the value of the two dictionaries
    /// </summary>
    /// <param name="baseDictionary">the dictionary where we want to add new value. Not nullable</param>
    /// <param name="dictionaryToMerge">the dictionary we want to merge</param>
    public static void MergeDictionaries(ConcurrentDictionary<string, int> baseDictionary, IReadOnlyDictionary<string, int> dictionaryToMerge)
    {
        if (baseDictionary == null)
            throw new ArgumentNullException(nameof(baseDictionary));

        if (dictionaryToMerge == null)
            return;

        foreach (KeyValuePair<string, int> keyValuePair in dictionaryToMerge)
        {
            baseDictionary.AddOrUpdate(keyValuePair.Key, s => keyValuePair.Value, (s, i) => keyValuePair.Value + i);
        }
    }
}