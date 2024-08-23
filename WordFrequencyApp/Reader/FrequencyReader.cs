using System.Collections.Concurrent;
using System.Text;
using WordFrequencyApp.Converter;
using WordFrequencyApp.FrequencyCalculator;
using WordFrequencyApp.Logger;

namespace WordFrequencyApp.Reader;

public class FrequencyReader : IFrequencyReader
{
    private readonly IWordFrequencyCalculator _wordFrequencyCalculator;
    private readonly ILogger _logger;
    private readonly Encoding _encoding;
    private const double SizeMaxPerDataToAnalyse = 1000; // 1kB because 1 windows-1252 character is coded in 1 byte
    private const int numberOfPackageForEachAction = 100; // We have 1KB per package so 100kB for each computing max 

    public FrequencyReader(
        IWordFrequencyCalculator wordFrequencyCalculator,
        ILogger logger)
    {
        _wordFrequencyCalculator = wordFrequencyCalculator;
        _logger = logger;
        _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1252)
                    ?? throw new ApplicationException("Error while getting Windows-1252 Encoding");
    }

    public bool TryReadAndComputeFrequencies(
        string fileNamePath,
        out IReadOnlyCollection<string> frequencies)
    {
        frequencies = new List<string>();
        ConcurrentDictionary<string, int> allFrequencies = new();
        List<List<string>> currentAllLines = new();
        List<string> currentLines = new();
        long bufferSize = 0;

        try
        {
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
                        bufferSize += line.Length; //this is possible because one windows-1252 character is coded in 1byte.
                    }
                    else
                    {
                        // the buffer is full, we need to store it and clean it before continuing reading
                        currentAllLines.Add(currentLines);
                        currentLines = new();
                        if (currentAllLines.Count == numberOfPackageForEachAction)
                        {
                            // We have enough data to compute frequencies
                            // We don't need more threads than the number of Package
                            allFrequencies = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(allFrequencies, currentAllLines, degreeOfParallelism: numberOfPackageForEachAction);
                            currentAllLines.Clear();
                        }
                        string? line = sr.ReadLine();
                        if (line == null)
                            break;
                        currentLines.Add(line);
                        bufferSize = line.Length; //this is possible because one windows-1252 character is coded in 1byte.
                    }
                }
            }

            // case where the buffer was not full at the end of the document
            if (currentLines.Count > 0)
            {
                currentAllLines.Add(currentLines);
            }

            // case where we did not have enough package to compute frequencies at the end of the document
            if (currentAllLines.Count > 0)
            {
                // We don't need more threads than the number of Package
                allFrequencies = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(allFrequencies, currentAllLines, degreeOfParallelism: currentAllLines.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.Log(ELogType.Error, $"Error while reading file {fileNamePath}: {ex.Message}");
            return false;
        }

        // Transform dictionary to a list of frequencies sorted in descending order of occurrence
        frequencies = FrequencyConverter.ConvertFromDictionary(allFrequencies);
        return true;
    }
}