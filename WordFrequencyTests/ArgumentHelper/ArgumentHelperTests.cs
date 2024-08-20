using Moq;
using WordFrequency.Helpers;
using WordFrequency.Tools;

namespace WorkFrequencyTests
{
    [TestClass]
    public class ArgumentHelperTests
    {
        private Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Mock<ILogger>();
            _logger.Setup(x => x.Log(It.IsAny<ELogType>(), It.IsAny<object>()));
        }


        [TestMethod]
        public void TestFailWhenEmptyArgs()
        {
            string[] args = [];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "Empty args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenInputArgMissing()
        {

            string[] args = ["test", "-output", "c:/tmp"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "No input in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenTooMuchInputArg()
        {

            string[] args = ["-input", "c:/tmp", "-input", "c/tmp2", "-output", "c:/tmp"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "Two input in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenInputValueMissing()
        {
            string[] args = ["-input", "-output", "c:/tmp"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "No input value in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenInputIncorrectValue()
        {
            string[] args = ["-input", "test", "-output", "c:/tmp"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "Incorrect value input in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenOutputArgMissing()
        {

            string[] args = ["-input", "c:/tmp", "test"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "No output in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenTooMuchOutputArg()
        {

            string[] args = ["-input", "c:/tmp", "-output", "c/tmp2", "-output", "c:/tmp2"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "Two output in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenOutputValueMissing()
        {
            string[] args = ["-input", "c:/tmp", "-output"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "No output value in args should not be valid");
        }

        [TestMethod]
        public void TestFailWhenOutputIncorrectValue()
        {
            string[] args = ["-input", "c:/tmp", "-output", "test"];

            bool result = ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object);

            Assert.IsFalse(result, "Incorrect value output in args should not be valid");
        }

        [TestMethod]
        public void TestSuccessWhenInputAndOutputCorrectGlobalValue()
        {
            const string message = "Correct global value input and ouput in args should be valid : ";

            string GetMessage(string[] args)
            {
                return $"{message} {string.Join(" ", args)}";
            }

            string[][] argsList =
                [["-input", "c:/tmp/text.txt", "-output", "c:/tmp/text.txt"],
                 ["-input", "/tmp/text.txt", "-output", "/tmp/text.txt"],
                 ["-input", "/tmp/subtmp/text.txt", "-output", "/tmp/subtmp/text.txt"],
                 ["-input", "d:/tmp/text.txt", "-output", "d:/tmp/text.txt"]];

            foreach (string[] args in argsList)
            {
                Assert.IsTrue(ArgumentHelper.ValidateCommandLineArguments(args, _logger.Object), GetMessage(args));
            }
        }
    }
}