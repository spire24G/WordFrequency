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

            var logger = serviceProvider.GetService<ILogger>();
            var fileWriter = serviceProvider.GetService<IWriter>();
            var calculator = serviceProvider.GetService<IWordFrequencyCalculator>();

            logger.Log(ELogType.Information, "Starting application");

            // Display the number of command line arguments.
            logger.Log(ELogType.Information, $"number of arguments : {args.Length}");

            try
            {
                ArgumentsInformation argumentsInformation = ArgumentHelper.ValidateCommandLineArguments(args, logger);
                if (argumentsInformation.hasErrors)
                {
                    DisplayResult(isSuccess: false, "Arguments invalid, see the logs for more details");
                    return;
                }

                string inputPath = argumentsInformation.InputFilePath;
                string outputPath = argumentsInformation.OutPutFilePath;

                if (!File.Exists(inputPath))
                {
                    // TODO display erreur
                }

                //if (!Directory.Exists(outputPath))
                //{
                //    // TODO display erreur
                //}

                Encoding? w1252 = CodePagesEncodingProvider.Instance.GetEncoding(1252);

                if (w1252 == null)
                {
                    DisplayResult(isSuccess: false, "Error while getting the encoding");
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
                        logger.Log(ELogType.Debug, $"Read : {line}");
                    }
                }

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
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<IWriter, FileWriter>()
                .AddSingleton<IWordFrequencyCalculator, WordFrequencyCalculator>()
                .BuildServiceProvider();

            return serviceProvider;
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

