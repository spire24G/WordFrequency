using WordFrequencyApp.Models;

namespace WordFrequencyApp.Helpers;

public static class ArgumentHelper
{
    internal const string NeedTwoArguments = "They must have two command line arguments";
    internal const string InputIncorrectCharacters = "Input file has incorrect characters";
    internal const string OutputIncorrectCharacters = "Output file has incorrect characters";
    internal const string InputFileNotExist = "Input file does not exist";
    internal const string OutputDirectoryNotExist = "Output file's directory does not exist";
    internal const string OutputPathIsAlreadyADirectory = "Output file path is an existing directory";

    /// <summary>
    /// Validate that arguments contains input and output
    /// </summary>
    /// <param name="args">All command line arguments </param>
    public static ArgumentsInformation GetCommandLineArgumentsInformation(string[] args)
    {
        if (args.Length != 2)
        {
            return new ArgumentsInformation
            {
                ErrorMessage = NeedTwoArguments
            };
        }

        string inputPath = args[0];
        string outputPath = args[1];

        string inputFileName = Path.GetFileName(inputPath);
        string outputFileName = Path.GetFileName(outputPath);

        if (inputPath.IndexOfAny(Path.GetInvalidPathChars()) != -1
            || inputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            return new ArgumentsInformation
            {
                ErrorMessage = InputIncorrectCharacters,
            };
        }

        if (!File.Exists(inputPath))
        {
            return new ArgumentsInformation
            {
                ErrorMessage = InputFileNotExist,
            };
        }

        if (outputPath.IndexOfAny(Path.GetInvalidPathChars()) != -1
            || outputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            return new ArgumentsInformation
            {
                ErrorMessage = OutputIncorrectCharacters,
            };
        }

        string extension = Path.GetExtension(outputPath);

        if(string.IsNullOrEmpty(extension))
            if(Directory.Exists(outputPath))
                return new ArgumentsInformation
                {
                    ErrorMessage = OutputPathIsAlreadyADirectory,
                };

        string? directoryName = Path.GetDirectoryName(outputPath);

        if (!Directory.Exists(directoryName))
        {
            return new ArgumentsInformation
            {
                ErrorMessage = OutputDirectoryNotExist,
            };
        }

        return new ArgumentsInformation
        {
            InputPath = inputPath,
            OutputPath = outputPath,
        };
    }
}

