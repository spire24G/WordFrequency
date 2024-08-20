using Microsoft.Extensions.DependencyInjection;
using WordFrequency.Helpers;
using WordFrequency.Tools;

namespace WordFrequency
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger, Logger>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger>();
            logger.Log(ELogType.Information, "Starting application");

            // Display the number of command line arguments.
            logger.Log(ELogType.Information, $"number of arguments : {args.Length}");

            try
            {
                bool argumentsValid = ArgumentHelper.ValidateCommandLineArguments(args, logger);
            }
            catch (Exception ex)
            {
                logger.Log(ELogType.Error,$"Internal error {ex.Message}");
            }
            finally
            {
                logger.Log(ELogType.Information, "All done!");

            }
        }


    }
}

