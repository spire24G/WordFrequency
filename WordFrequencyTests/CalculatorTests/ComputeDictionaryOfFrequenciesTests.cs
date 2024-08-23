using System.Collections.Concurrent;
using WordFrequencyApp.FrequencyCalculator;

namespace WordFrequencyTests.CalculatorTests;

[TestClass]
public class ComputeDictionaryOfFrequenciesTests
{

    private WordFrequencyCalculator _wordFrequencyCalculator = null!;
    private ConcurrentDictionary<string, int> _frequencies = new();
    private const int DegreeOfParallelism = 2;

    [TestInitialize]
    public void Init()
    {
        _wordFrequencyCalculator = new WordFrequencyCalculator();
    }

    [TestMethod]
    public void TestEmptyDataWorks()
    {
        List<List<string>> data = [];

        ConcurrentDictionary<string, int> result = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(_frequencies, data, DegreeOfParallelism);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count, "There should not be any data");
    }

    [TestMethod]
    public void TestOneLineInOnePackageDataWorks()
    {
        List<List<string>> data = [["Hello World"]];

        ConcurrentDictionary<string, int> result = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(_frequencies, data, DegreeOfParallelism);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count, "There should be two frequencies");
    }

    [TestMethod]
    public void TestTwoLineInOnePackageDataWorks()
    {
        List<List<string>> data =
        [
            [
                "Hello",
                "World"
            ]
        ];

        ConcurrentDictionary<string, int> result = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(_frequencies, data, DegreeOfParallelism);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count, "There should be two frequencies");
    }

    [TestMethod]
    public void TestOneLineInTwoPackageDataWorks()
    {
        List<List<string>> data =
        [
            [
                "Hello World"
            ],

            [
                "Hello2 World2"
            ]
        ];

        ConcurrentDictionary<string, int> result = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(_frequencies, data, DegreeOfParallelism);

        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count, "There should be four frequencies");
    }

    [TestMethod]
    public void TestTwoLineInTwoPackageDataWorks()
    {
        List<List<string>> data =
        [
            [
                "Hello",
                "World"
            ],

            [
                "Hello2",
                "World2"
            ]
        ];

        ConcurrentDictionary<string, int> result = _wordFrequencyCalculator.ComputeDictionaryOfFrequencies(_frequencies, data, DegreeOfParallelism);

        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count, "There should be four frequencies");
    }
}