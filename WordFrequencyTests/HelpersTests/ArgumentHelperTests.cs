using WordFrequencyApp.Helpers;
using WordFrequencyApp.Models;

namespace WordFrequencyTests.HelpersTest
{
    [TestClass]
    public class ArgumentHelperTests
    {
        [TestMethod]
        public void TestFailWhenEmptyArgs()
        {
            string[] args = [];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);


            Assert.AreEqual(ArgumentHelper.NeedTwoArguments, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenOnlyOneArgument()
        {

            string[] args = ["c:/tmp"];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.NeedTwoArguments, argumentsInformation.ErrorMessage);

        }

        [TestMethod]
        public void TestFailWhenTooMuchArg()
        {

            string[] args = ["c:/tmp", "c/tmp2", "c:/tmp3"];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.NeedTwoArguments, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenInputArgUnknown()
        {

            string[] args = ["./error.txt", "."];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.InputFileNotExist, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenInputArgIsFolder()
        {

            string[] args = [Directory.GetCurrentDirectory(), "."];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.InputFileNotExist, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenInputArgTooLong()
        {

            string[] args = ["./errorerrorerrorerrorerrorerrorerrorerrorerrorerrorerrorerror" +
                             "errorerrorerrorerrorerrorerrorerrorerror" +
                             "errorerrorerrorerrorerrorerrorerrorerror" +
                             "errorerrorerrorerrorerrorerrorerrorerror" +
                             "errorerrorerrorerrorerrorerror.txt", "."];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.InputFileNotExist, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenInputContainsUnacceptableCharacter()
        {
            const string message = "Incorrect value in input or output args should not be valid: ";

            string inputPath = Path.GetTempFileName();
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string[][] argsList =
                [[currentDirectoryPath + "\\<test.txt", ".\\test.txt"],
                [currentDirectoryPath + "\\>test.txt", ".\\test.txt"],
                [currentDirectoryPath + "\\:test.txt", ".\\test.txt"],
                [currentDirectoryPath + "\\|test.txt", ".\\test.txt"],
                [currentDirectoryPath + "\\?test.txt", ".\\test.txt"],
                [currentDirectoryPath + "\\*test.txt", ".\\test.txt"]];

            AssertGetCommandLineArgumentsInformation(argsList, ArgumentHelper.InputIncorrectCharacters, message);

            argsList =
                [[inputPath, ".\\<test.txt"],
                [inputPath, ".\\>test.txt"],
                [inputPath, ".\\:test.txt"],
                [inputPath, ".\\|test.txt"],
                [inputPath, ".\\?test.txt"],
                [inputPath, ".\\*test.txt"]];
            AssertGetCommandLineArgumentsInformation(argsList, ArgumentHelper.OutputIncorrectCharacters, message);

        }

        [TestMethod]
        public void TestFailWhenOutputArgUnknown()
        {

            string[] args = [Path.GetTempFileName(), "c:/unknownFolder/test.txt"];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.OutputDirectoryNotExist, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenOutputArgIsUnknownFolder()
        {

            string[] args = [Path.GetTempFileName(), Directory.GetCurrentDirectory() + @"\unknown\"];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.OutputDirectoryNotExist, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestFailWhenOutputArgIsKnownFolder()
        {

            string[] args = [Path.GetTempFileName(), "..\\"];

            ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

            Assert.AreEqual(ArgumentHelper.OutputPathIsAlreadyADirectory, argumentsInformation.ErrorMessage);
        }

        [TestMethod]
        public void TestSuccessWhenInputAndOutputCorrectGlobalValue()
        {
            const string message = "Correct value input and ouput in args should be valid : ";

            string inputPath = Path.GetTempFileName();
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string[][] argsList =
                [[inputPath, ".\\test.txt"],
                [inputPath, currentDirectoryPath + "\\test.txt"]];

            AssertGetCommandLineArgumentsInformation(argsList, string.Empty, message);
        }

        private static void AssertGetCommandLineArgumentsInformation(string[][] argsList, string expectedErrorMessage, string message)
        {
            foreach (string[] args in argsList)
            {
                Assert.AreEqual(expectedErrorMessage, ArgumentHelper.GetCommandLineArgumentsInformation(args).ErrorMessage, ConcatArgsAndMessage(args, message));
            }
        }

        private static string ConcatArgsAndMessage(string[] args, string message)
        {
            return $"{message} {string.Join(" ", args)}";
        }
    }
}