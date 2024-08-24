using WordFrequencyApp.Localisation;
using WordFrequencyApp.Models;

namespace WordFrequencyApp.Helpers;

public static class ArgumentHelper
{
    /// <summary>
    /// Validates that arguments contains input and output
    /// </summary>
    /// <param name="args">All command line arguments</param>
    public static ArgumentsInformation GetCommandLineArgumentsInformation(string[] args)
    {
        if (args.Length != 2)
            return new ArgumentsInformation(Language.NeedTwoArguments);

        string inputPath = args[0];
        string outputPath = args[1];

        string inputFileName = Path.GetFileName(inputPath);
        string outputFileName = Path.GetFileName(outputPath);

        if (inputPath.IndexOfAny(Path.GetInvalidPathChars()) != -1
            || inputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            return new ArgumentsInformation(Language.InputIncorrectCharacters);


        if (!File.Exists(inputPath))
            return new ArgumentsInformation(Language.InputFileNotExist);


        if (outputPath.IndexOfAny(Path.GetInvalidPathChars()) != -1
            || outputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            return new ArgumentsInformation(Language.OutputIncorrectCharacters);

        string extension = Path.GetExtension(outputPath);

        if (string.IsNullOrEmpty(extension) && Directory.Exists(outputPath))
            return new ArgumentsInformation(Language.OutputPathIsAlreadyADirectory);

        string? directoryName = Path.GetDirectoryName(outputPath);

        if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
            return new ArgumentsInformation(Language.OutputDirectoryNotExist);

        return new ArgumentsInformation(inputPath, outputPath);
    }
}

