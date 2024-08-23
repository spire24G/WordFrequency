using WordFrequencyApp.FrequencyCalculator;

namespace WordFrequencyTests.CalculatorTests;

[TestClass]
public class FindWordFrequencyTests
{
    private WordFrequencyCalculator _wordFrequencyCalculator = null!;

    [TestInitialize]
    public void Init()
    {
        _wordFrequencyCalculator = new WordFrequencyCalculator();
    }

    [TestMethod]
    public void TestSameWordMatch()
    {
        List<string> data = ["test test"];

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The key named test must be inside the dictionary");
        Assert.AreEqual(expected: 2, frequencies["test"], "Test is used twice");
    }

    [TestMethod]
    public void TestSameWordMatchWithDifferentCaseMatch()
    {
        List<string> data =
        [
            "test",
            "TEST",
            "Test",
            "teSt",
            "tesT"
        ];

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The key named test must be inside the dictionary");
        Assert.AreEqual(expected: 5, frequencies["test"], "Test is used twice, no matter the case");
    }

    [TestMethod]
    public void TestSameWordMatchOnDifferentLineMatch()
    {
        List<string> data =
        [
            "test",
            "test"
        ];

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The key named test must be inside the dictionary");
        Assert.AreEqual(expected: 2, frequencies["test"], "Test is used twice, no matter the lines");
    }

    [TestMethod]
    public void TestMultipleSpacesNotUsedAsWord()
    {
        List<string> data =
        [
            "test     test",
            "test   "
        ];

        Dictionary<string, int> frequencies = _wordFrequencyCalculator.FindWordFrequency(data);

        Assert.IsNotNull(frequencies, "Frequencies can not be null");
        Assert.AreEqual(expected: 1, frequencies.Count, "There must be only one element");
        Assert.IsTrue(frequencies.ContainsKey("test"), "The key named test must be inside the dictionary");
        Assert.AreEqual(expected: 3, frequencies["test"], "Test is used three times, no matter the spaces and the lines");
    }
}