using Microsoft.Extensions.DependencyInjection;
using WordFrequencyApp.FrequencyCalculator;
using WordFrequencyApp.Helpers;
using WordFrequencyApp.Logger;
using WordFrequencyApp.Models;
using WordFrequencyApp.Reader;
using WordFrequencyApp.Writer;

namespace WordFrequencyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Init DI
            ServiceProvider serviceProvider = RegisterService();

            ILogger logger = serviceProvider.GetService<ILogger>()!;
            IWriter fileWriter = serviceProvider.GetService<IWriter>()!;
            IFrequencyReader frequencyReader = serviceProvider.GetService<IFrequencyReader>()!;

            try
            {
                logger.Log(ELogType.Information, "Starting application");

                //Validate and get arguments information
                ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

                //Display error message if invalid args
                if (!string.IsNullOrEmpty(argumentsInformation.ErrorMessage))
                {
                    DisplayResult(isSuccess: false, argumentsInformation.ErrorMessage);
                    return;
                }

                logger.Log(ELogType.Information, $"input path: {argumentsInformation.InputPath}");
                logger.Log(ELogType.Information, $"output path: {argumentsInformation.OutputPath}");

                // Read data and compute frequencies
                bool resultReading = 
                    frequencyReader.TryReadAndComputeFrequencies(
                        argumentsInformation.InputPath, 
                        out IReadOnlyCollection<string> frequenciesToWrite);

                // Display error if frequencies is null, because we only have null in the catch part
                if (!resultReading)
                {
                    DisplayResult(isSuccess: false, "Reading input file failed, see the logs for more details");
                    return;
                }

                // Write frequencies in file
                bool resultWriting = fileWriter.WriteData(frequenciesToWrite, argumentsInformation.OutputPath);

                // Display error if writing was not a success
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
                .AddSingleton<IFrequencyReader, FrequencyReader>()
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
                Console.WriteLine(
                    string.IsNullOrEmpty(message) 
                        ? "FAILURE!" 
                        : $"FAILURE! {message}");
            }

            Console.ResetColor();
        }
    }
}

