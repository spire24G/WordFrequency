namespace WordFrequencyApp.Calculator;

public interface IWordFrequencyCalculator
{
    public Dictionary<string, int> FindWordFrequency(IReadOnlyCollection<string> data);
}