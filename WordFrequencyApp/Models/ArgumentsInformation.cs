namespace WordFrequencyApp.Models;

public class ArgumentsInformation
{
    public ArgumentsInformation(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public ArgumentsInformation(string inputPath, string outputPath)
    {
        InputPath = inputPath;
        OutputPath = outputPath;
    }

    public string InputPath { get; set; } = string.Empty;

    public string OutputPath { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}