using WordFrequencyApp.Calculator;

namespace WordFrequencyTests.CalculatorTests;

[TestClass]
public class WordFrequencyCalculatorTests
{
    private IWordFrequencyCalculator _wordFrequencyCalculator;

    [TestInitialize]
    public void Init()
    {
        _wordFrequencyCalculator = new WordFrequencyCalculator();
    }

    [TestMethod]
    public void TestSameWordMatch()
    {
        List<string> data = new List<string>
        {
            "test test"
        };

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The test key must be inside the dictionary");
        Assert.AreEqual(expected: 2, frequencies["test"], "Test is used twice");
    }

    [TestMethod]
    public void TestSameWordMatchWithDifferentCaseMatch()
    {
        List<string> data = new List<string>
        {
            "test TEST"
        };

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The test key must be inside the dictionary");
        Assert.AreEqual(expected: 2, frequencies["test"], "Test is used twice, no matter the case");
    }

    [TestMethod]
    public void TestSameWordMatchOnDifferentLineMatch()
    {
        List<string> data = new List<string>
        {
            "test",
            "test"
        };

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The test key must be inside the dictionary");
        Assert.AreEqual(expected: 2, frequencies["test"], "Test is used twice, no matter the line");
    }

    [TestMethod]
    public void TestMultipleSpacesNotUsedAsWord()
    {
        List<string> data = new List<string>
        {
            "test     test",
            "test   "
        };

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The test key must be inside the dictionary");
        Assert.AreEqual(expected: 3, frequencies["test"], "Test is used twice, no matter the spaces");
    }
}