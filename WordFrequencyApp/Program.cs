using Microsoft.Extensions.DependencyInjection;
using System.Text;
using WordFrequencyApp.Calculator;
using WordFrequencyApp.Converter;
using WordFrequencyApp.Helpers;
using WordFrequencyApp.Logger;
using WordFrequencyApp.Writer;

namespace WordFrequencyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //setup our DI
            ServiceProvider serviceProvider = RegisterService();

            ILogger logger = serviceProvider.GetService<ILogger>()!;
            IWriter fileWriter = serviceProvider.GetService<IWriter>()!;
            IWordFrequencyCalculator calculator = serviceProvider.GetService<IWordFrequencyCalculator>()!;

            try
            {
                logger.Log(ELogType.Information, "Starting application");

                ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args, logger);

                if (argumentsInformation.HasErrors)
                {
                    DisplayResult(isSuccess: false, "Arguments were incorrect");
                    return;
                }

                string inputPath = argumentsInformation.CommandLineArgument[ArgumentHelper.InputCommandLineArgument];
                string outputPath = argumentsInformation.CommandLineArgument[ArgumentHelper.OutputCommandLineArgument];

                logger.Log(ELogType.Information, $"input path: {inputPath}");
                logger.Log(ELogType.Information, $"output path: {outputPath}");

                if (!File.Exists(inputPath))
                {
                    DisplayResult(isSuccess: false, "Input file does not exist");
                    return;
                }

                if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                {
                    DisplayResult(isSuccess: false, "Output directory does not exist");
                    return;
                }

                Encoding? w1252 = CodePagesEncodingProvider.Instance.GetEncoding(1252);
                if (w1252 == null)
                {
                    DisplayResult(isSuccess: false, "Error while getting the encoding");
                    return;
                }

                List<string> allLines = new();

                using (StreamReader sr = new(inputPath, w1252))
                {
                    while (sr.Peek() >= 0)
                    {
                        string? line = sr.ReadLine();
                        if (line == null)
                            break;

                        allLines.Add(line);
                    }
                }

                logger.Log(ELogType.Information, "Computing will start");
                Dictionary<string, int> frequencies = calculator.FindWordFrequency(allLines);

                IReadOnlyCollection<string> frequenciesToWrite = FrequencyConverter.ConvertFromDictionary(frequencies);

                bool resultWriting = fileWriter.WriteData(frequenciesToWrite, outputPath);
                if (!resultWriting)
                {
                    DisplayResult(isSuccess: false, "Writing in output file failed, see the logs for more details");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Log(ELogType.Error, $"Internal error {ex.Message}");
                DisplayResult(isSuccess: false, "Internal error, see the logs for more details");
                return;
            }

            DisplayResult(isSuccess: true);
        }

        private static ServiceProvider RegisterService()
        {
            return new ServiceCollection()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<IWriter, FileWriter>()
                .AddSingleton<IWordFrequencyCalculator, WordFrequencyCalculator>()
                .BuildServiceProvider();
        }

        public static void DisplayResult(bool isSuccess, string? message = null)
        {
            if (isSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"SUCCESS!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (string.IsNullOrEmpty(message))
                    Console.WriteLine("FAILURE!");
                else
                    Console.WriteLine($"FAILURE! {message}");
            }

            Console.ResetColor();
        }
    }
}

