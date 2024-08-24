using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using WordFrequencyApp.Converter;
using WordFrequencyApp.FrequencyCalculator;
using WordFrequencyApp.Helpers;
using WordFrequencyApp.Localisation;
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
                logger.Log(ELogType.Debug, Language.ApplicationStart);

                //Validate and get arguments information
                ArgumentsInformation argumentsInformation = ArgumentHelper.GetCommandLineArgumentsInformation(args);

                //Display error message if invalid args
                if (!string.IsNullOrEmpty(argumentsInformation.ErrorMessage))
                {
                    DisplayResult(isSuccess: false, argumentsInformation.ErrorMessage);
                    return;
                }

                logger.Log(ELogType.Information, $"{Language.InputPathLabel} {argumentsInformation.InputPath}");
                logger.Log(ELogType.Information, $"{Language.OutputPathLabel} {argumentsInformation.OutputPath}");

                // Read data and compute frequencies
                bool resultReading =
                    frequencyReader.TryReadAndComputeFrequencies(
                        argumentsInformation.InputPath,
                        out ConcurrentDictionary<string,int> allFrequencies);

                // Display error if frequencies is null, because we only have null in the catch part
                if (!resultReading)
                {
                    DisplayResult(isSuccess: false, Language.ReadingFailedDetails);
                    return;
                }

                //Convert a dictionary into a list ordered by max
                IEnumerable<string> frequenciesToWrite = FrequencyConverter.ConvertToFrequencyOrderListFromDictionary(allFrequencies);

                // Write frequencies in file
                bool resultWriting = fileWriter.WriteData(frequenciesToWrite, argumentsInformation.OutputPath);

                // Display error if writing was not a success
                if (!resultWriting)
                {
                    DisplayResult(isSuccess: false, Language.WritingFailedDetails);
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Log(ELogType.Error, $"{Language.InternalError} {ex.Message}");
                DisplayResult(isSuccess: false, Language.InternalErrorDetails);
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
                Console.WriteLine(Localisation.Language.Success);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    string.IsNullOrEmpty(message)
                        ? Localisation.Language.Failure
                        : $"{Localisation.Language.Failure} {message}");
            }

            Console.ResetColor();
        }
    }
}

