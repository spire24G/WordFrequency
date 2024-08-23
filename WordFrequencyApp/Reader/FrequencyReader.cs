using System.Collections.Concurrent;
using System.Text;
using WordFrequencyApp.Converter;
using WordFrequencyApp.FrequencyCalculator;

namespace WordFrequencyApp.Reader;

public class FrequencyReader : IFrequencyReader
{
    private readonly IWordFrequencyCalculator _wordFrequencyCalculator;
    private readonly Encoding _encoding;
    private const double SizeMaxPerDataToAnalyse = 1000; // 1kB
    private const int numberOfPackageForEachAction = 100; // We have 1KB per package so 100kB for each computing max 

    public FrequencyReader(IWordFrequencyCalculator wordFrequencyCalculator)
    {
        _wordFrequencyCalculator = wordFrequencyCalculator;
        _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1252)
                    ?? throw new ApplicationException("Error while getting Windows-1252 Encoding");
    }

    public IReadOnlyCollection<string> ReadAndComputeFrequencies(string fileNamePath)
    {
        ConcurrentDictionary<string, int> allFrequencies = new();
        List<List<string>> currentAllLines = new();
        List<string> currentLines = new();
        long bufferSize = 0;

        using (StreamReader sr = new(fileNamePath, _encoding))
        {
            while (sr.Peek() >= 0)
            {
               
                if (bufferSize < SizeMaxPerDataToAnalyse)
                {
                    // the buffer is not full, we can continue to read
                    string? line = sr.ReadLine();
                    if (line == null)
                        break;
                    currentLines.Add(line);
                    bufferSize += line.Length;
                }
                else
                {
                    // the buffer is full, we need to store it and clean it before continuing reading
                    currentAllLines.Add(currentLines);
                    currentLines = new();
                    if (currentAllLines.Count == numberOfPackageForEachAction)
                    {
                        // We have enough data to compute frequencies
                        allFrequencies = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(allFrequencies, currentAllLines, numberOfPackageForEachAction);
                        currentAllLines.Clear();
                    }
                    string? line = sr.ReadLine();
                    if (line == null)
                        break;
                    currentLines.Add(line);
                    bufferSize = line.Length;
                }
            }
        }

        // case where the buffer was not full at the end of the document
        if (currentLines.Count > 0)
        {
            currentAllLines.Add(currentLines);
        }

        // case where we haven't enough package to compute frequencies at the end of the document
        if (currentAllLines.Count > 0)
        {
            allFrequencies = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(allFrequencies, currentAllLines, currentAllLines.Count);
        }

        // Transform dictionary to a list of frequencies sorted in descending order of occurrence
        return FrequencyConverter.ConvertFromDictionary(allFrequencies);
    }
}