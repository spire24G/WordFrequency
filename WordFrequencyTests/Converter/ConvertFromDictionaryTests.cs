using System.Collections.Concurrent;
using WordFrequencyApp.Converter;

namespace WordFrequencyTests.Converter;

[TestClass]
public class ConvertFromDictionaryTests
{
    [TestMethod]
    public void TestEmptyData()
    {
        ConcurrentDictionary<string, int> data = new();
      

        IReadOnlyCollection<string> result = FrequencyConverter.ConvertFromDictionary(data);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count, "There should no element");
    }

    [TestMethod]
    public void TestOneData()
    {
        ConcurrentDictionary<string, int> data = new();
        data["Hello"] = 2;

        IReadOnlyCollection<string> result = FrequencyConverter.ConvertFromDictionary(data);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count, "There should be one element");

        Assert.AreEqual("Hello,2", result.First(), "Hello should be first");
    }

    [TestMethod]
    public void TestDescendingOrder()
    {
        ConcurrentDictionary<string, int> data = new();
        data["Hello"] = 2;
        data["World"] = 1;

        IReadOnlyCollection<string> result = FrequencyConverter.ConvertFromDictionary(data);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count, "There should be two elements");
     
        Assert.AreEqual("Hello,2", result.First(), "Hello should be first");
        Assert.AreEqual("World,1", result.Last(), "Hello should be first");
    }
}