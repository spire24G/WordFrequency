using System.Text;
using WordFrequencyApp.Logger;
using ILogger = WordFrequencyApp.Logger.ILogger;

namespace WordFrequencyApp.Writer;

public class FileWriter : IWriter
{
    private readonly ILogger _logger;
    private readonly Encoding _encoding;

    public FileWriter(ILogger logger)
    {
        _logger = logger;
        _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1252)
                    ?? throw new ApplicationException("Error while getting Windows-1252 Encoding");
    }
    public bool WriteData(IReadOnlyCollection<string> data, string outputPath)
    {
        try
        {
            using (StreamWriter sw = new(outputPath, false, _encoding))
            {
                foreach (string line in data)
                {
                    sw.WriteLine(line);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Log(ELogType.Error, $"Error while writing file {outputPath}: {ex.Message}");
            return false;
        }
    }
}