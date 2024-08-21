namespace WordFrequencyApp;

public class ArgumentsInformation
{
    public IReadOnlyDictionary<string,string> CommandLineArgument { get; set; }

    public bool HasErrors { get; set; }
}