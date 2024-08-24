using System.Collections.Concurrent;
using System.Text;
using WordFrequencyApp.FrequencyCalculator;
using WordFrequencyApp.Localisation;
using WordFrequencyApp.Logger;

namespace WordFrequencyApp.Reader;

public class FrequencyReader : IFrequencyReader
{
    private readonly IWordFrequencyCalculator _wordFrequencyCalculator;
    private readonly ILogger _logger;
    private readonly Encoding _encoding;

    // 1kB because 1 windows-1252 character is coded in 1 byte
    private const double SizeMaxPerDataToAnalyse = 1000;

    // It will be used for the parallel computing (100 threads in parallel)
    // We have 1KB per package so 100kB for each computing max 
    // This number has a god ratio Performance/CPU usage
    private const int numberOfPackageForEachAction = 100; 

    public FrequencyReader(
        IWordFrequencyCalculator wordFrequencyCalculator,
        ILogger logger)
    {
        _wordFrequencyCalculator = wordFrequencyCalculator;
        _logger = logger;
        _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1252)
                    ?? throw new ApplicationException(Language.EncodingError);
    }

    /// <summary>
    /// We read each line of <paramref name="fileNamePath"/>.
    /// Once we have read 100KB of data, we compute the frequencies on this package of data.
    /// We clear the previous line read, then we repeat this action until the end of the file
    /// </summary>
    /// <param name="fileNamePath">path of the input file</param>
    /// <param name="allFrequencies">dictionary of frequency of every word in the file</param>
    /// <returns>true if no error, otherwise false</returns>
    public bool TryReadAndComputeFrequencies(
        string fileNamePath,
        out ConcurrentDictionary<string,int> allFrequencies)
    {
        allFrequencies = new ConcurrentDictionary<string,int>();
        List<List<string>> currentPackageOfLines = [];
        List<string> currentLines = [];
        long bufferSize = 0;

        try
        {
            using (StreamReader sr = new(fileNamePath, _encoding))
            {
                while (sr.Peek() >= 0)
                {
                    // using a buffer prevents tha application from storing all data in memory
                    // only allFrequencies store all the data
                    // (which is way less than the file if we have common words) 
                    if (bufferSize < SizeMaxPerDataToAnalyse)
                    {
                        // the buffer is not full, we can continue to read
                        string? line = sr.ReadLine();
                        if (line == null) //security code but sr.Peek check this for us
                            break;
                        currentLines.Add(line);
                        bufferSize += line.Length; //this is possible because one windows-1252 character is coded in 1byte.
                    }
                    else
                    {
                        // the buffer is full, we need to store it and clean it before continuing reading
                        currentPackageOfLines.Add(currentLines);
                        currentLines = [];
                        
                        // We have enough data to launch a parallel calculation
                        if (currentPackageOfLines.Count == numberOfPackageForEachAction)
                        {
                            // We have enough data to compute frequencies
                            // We don't need more threads than the number of Package
                            allFrequencies = 
                                _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(
                                    allFrequencies, 
                                    currentPackageOfLines, 
                                    degreeOfParallelism: numberOfPackageForEachAction);
                            currentPackageOfLines.Clear();
                        }

                        string? line = sr.ReadLine();
                        if (line == null) //security code but sr.Peek check this for us
                            break;
                        currentLines.Add(line);
                        bufferSize = line.Length; //this is possible because one windows-1252 character is coded in 1byte.
                    }
                }
            }

            // case where the buffer was not full at the end of the document
            if (currentLines.Count > 0)
            {
                currentPackageOfLines.Add(currentLines);
            }

            // case where we did not have enough package to compute frequencies at the end of the document
            if (currentPackageOfLines.Count > 0)
            {
                // We don't need more threads than the number of Package
                allFrequencies = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(allFrequencies, currentPackageOfLines, degreeOfParallelism: currentPackageOfLines.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.Log(ELogType.Error, $"{Language.ReadingFailed} {fileNamePath}: {ex.Message}");
            return false;
        }

        return true;
    }
}