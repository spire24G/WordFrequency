using System.Text.RegularExpressions;
using WordFrequency.Tools;

namespace WordFrequency.Helpers;

public enum EArgumentType
{
    Path = 0,
}

public static class ArgumentHelper
{
    public const string Input = "-input";
    public const string Output = "-output";

    /// <summary>
    /// Validate that arguments contains input and output
    /// </summary>
    /// <param name="args">All command line arguments </param>
    /// <param name="logger"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static bool ValidateCommandLineArguments(string[] args, ILogger logger)
    {
        if (args.Length == 0)
        {
            logger.Log(ELogType.Error, "Command line args can not be null or empty");
            return false;
        }

        if (!CommandLineArgumentsContainsArgument(args, Input))
        {
            logger.Log(ELogType.Error, "Command line args must contain \"-input FilePath\"");
            return false;
        }

        if (!CommandLineArgumentsHasValidArgumentValue(args, Input, EArgumentType.Path))
        {
            logger.Log(ELogType.Error, "Command line args must contain a valid file path as input file");
            return false;
        }

        if (!CommandLineArgumentsContainsArgument(args, Output))
        {
            logger.Log(ELogType.Error, "Command line args must contain \"-ouput FilePath\"");
            return false;
        }

        if (!CommandLineArgumentsHasValidArgumentValue(args, Output, EArgumentType.Path))
        {
            logger.Log(ELogType.Error, "Command line args must contain a valid file path as output file");
            return false;
        }

        return true;
    }

    private static bool CommandLineArgumentsContainsArgument(string[] args, string argument)
    {
        return Array.FindAll(args, x => x == argument).Length == 1;
    }

    /// <summary>
    /// Argument must be inside args
    /// </summary>
    private static bool CommandLineArgumentsHasValidArgumentValue(string[] args, string argument, EArgumentType argumentType)
    {
        int index = Array.IndexOf(args, argument);

        if (index == -1)
        {
            throw new ArgumentException("Argument must be inside args");
        }

        if (index == args.Length - 1)
            return false;

        string nextArg = args[index + 1];

        // TODO 
        //optimization for maxCPU
        // found idea for global path //
        return argumentType == EArgumentType.Path
            ? ValidateGlobalPath(nextArg)
            : true;
    }

    private static bool ValidateGlobalPath(string arg)
    {
        var pattern = @"^[A-Z]:(\\[^\\]+)+\\?$|^/?([^/]+/)+";
        return Regex.IsMatch(arg, pattern);
    }
}

