using System.Collections.Concurrent;
using WordFrequencyApp.Helpers;

namespace WordFrequencyTests.HelpersTest;

[TestClass]
public class DictionaryHelperTests
{
    [TestMethod]
    public void TestMergeBaseNull()
    {
        ConcurrentDictionary<string, int> baseDictionary = null!;
        Dictionary<string, int> mergeDictionary = new();

        try
        {
            DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);
            Assert.Fail("Should raise exception");
        }
        catch (ArgumentNullException)
        {
            // we want this 
        }
    }

    [TestMethod]
    public void TestMergeDicoWithNull()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();
        baseDictionary["test"] = 1;
        Dictionary<string, int> mergeDictionary = null!;

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 1, "base dictionary should contain one element");
        Assert.IsTrue(baseDictionary.ContainsKey("test"), "base dictionary should contain key test");
        Assert.AreEqual(expected: 1, baseDictionary["test"], "key test should be associate to 1");
    }

    [TestMethod]
    public void TestMergeEmptyWithEmpty()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();
        Dictionary<string, int> mergeDictionary = new();

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 0);
    }

    [TestMethod]
    public void TestMergeEmptyWithNoEmpty()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();

        Dictionary<string, int> mergeDictionary = new()
        {
            {"test",1}
        };

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 1, "base dictionary should contain one element");
        Assert.IsTrue(baseDictionary.ContainsKey("test"), "base dictionary should contain key test");
        Assert.AreEqual(expected: 1, baseDictionary["test"], "key test should be associate to 1");
    }


    [TestMethod]
    public void TestMergeNoEmptyWithEmpty()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();
        baseDictionary["test"] = 1;

        Dictionary<string, int> mergeDictionary = new();

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 1, "base dictionary should contain one element");
        Assert.IsTrue(baseDictionary.ContainsKey("test"), "base dictionary should contain key test");
        Assert.AreEqual(expected: 1, baseDictionary["test"], "key test should be associate to 1");
    }

    [TestMethod]
    public void TestMergeTwoDictionariesWithNoCommonData()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();
        baseDictionary["test"] = 1;

        Dictionary<string, int> mergeDictionary = new()
        {
            {"test2",2}
        };

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 2, "base dictionary should contain one element");
        Assert.IsTrue(baseDictionary.ContainsKey("test"), "base dictionary should contain key test");
        Assert.IsTrue(baseDictionary.ContainsKey("test2"), "base dictionary should contain key test2");
        Assert.AreEqual(expected: 1, baseDictionary["test"], "key test should be associate to 1");
        Assert.AreEqual(expected: 2, baseDictionary["test2"], "key test2 should be associate to 2");
    }

    [TestMethod]
    public void TestMergeTwoDictionariesWithCommonData()
    {
        ConcurrentDictionary<string, int> baseDictionary = new();
        baseDictionary["test"] = 1;
        Dictionary<string, int> mergeDictionary = new()
        {
            {"test",2}
        };

        DictionaryHelper.MergeDictionaries(baseDictionary, mergeDictionary);

        Assert.IsTrue(baseDictionary.Count == 1, "base dictionary should contain one element");
        Assert.IsTrue(baseDictionary.ContainsKey("test"), "base dictionary should contain key test");
        Assert.AreEqual(expected: 3, baseDictionary["test"], "key test should be associate to 3 (1+2)");
    }

}