using System.Text;
using WordFrequencyApp.Localisation;
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
                    ?? throw new ApplicationException(Language.EncodingError);
    }
    public bool WriteData(IEnumerable<string> data, string outputPath)
    {
        try
        {
            File.WriteAllLines(outputPath, data, _encoding);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Log(ELogType.Error, $"{Language.WritingFailed} {outputPath}: {ex.Message}");
            return false;
        }
    }
}