using System.ComponentModel;
using System.Text.RegularExpressions;
using WordFrequencyApp.Logger;

namespace WordFrequencyApp.Helpers;

public static class ArgumentHelper
{
    public const string InputCommandLineArgument = "-input";
    public const string OutputCommandLineArgument = "-output";

    private static Dictionary<string, string> _commandLineArgumentsWithCorrectFormat = new Dictionary<string, string>
    {
        { InputCommandLineArgument, "-input FilePath" },
        { OutputCommandLineArgument, "-output FilePath" },
    };

    /// <summary>
    /// Validate that arguments contains input and output
    /// </summary>
    /// <param name="args">All command line arguments </param>
    /// <param name="logger">logger to log errors</param>
    public static ArgumentsInformation GetCommandLineArgumentsInformation(string[] args, ILogger logger)
    {
        if (args.Length == 0)
        {
            logger.Log(ELogType.Error, "Command line args can not be null or empty");
            return new ArgumentsInformation { HasErrors = true };
        }

        Dictionary<string, string> arguments = new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> argumentWithFormat in _commandLineArgumentsWithCorrectFormat)
        {
            if (!TryGetCommandLineArgument(args, argumentWithFormat.Key, out string argumentValue))
            {
                logger.Log(ELogType.Error, $"Command line args must contain \"{argumentWithFormat.Value}\" one time");
                return new ArgumentsInformation { HasErrors = true };
            }
            arguments[argumentWithFormat.Key] = argumentValue;
        }

        return new ArgumentsInformation
        {
            CommandLineArgument = arguments,
            HasErrors = false,
        };
    }

    private static bool CheckArgsContainOnlyOneTimeArgument(string[] args, string argument)
    {
        return Array.FindAll(args, x => x == argument).Length == 1;
    }

    /// <summary>
    /// Argument must be inside args
    /// </summary>
    private static bool TryGetCommandLineArgument(
        string[] args,
        string argument,
        out string argumentValue)
    {
        argumentValue = string.Empty;

        if (!CheckArgsContainOnlyOneTimeArgument(args, argument))
            return false;

        int index = Array.IndexOf(args, argument);

        if (index == args.Length - 1 || index == -1)
            return false;

        string nextArg = args[index + 1];

        if (!CheckValueIsNotArgument(nextArg))
            return false;

        argumentValue = nextArg;
        return true;
    }

    private static bool CheckValueIsNotArgument(string arg)
    {
        if (string.IsNullOrEmpty(arg))
            return false;

        if (arg[0] == '-')
            return false;

        if (_commandLineArgumentsWithCorrectFormat.ContainsKey(arg))
            return false;

        return true;
    }
}

