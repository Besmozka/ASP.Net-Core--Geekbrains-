using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sb = new StringBuilder();
                PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();

                var desiredCategories = new HashSet<string> { "Process", "Memory" };

                foreach (var category in categories)
                {
                    sb.AppendLine("Category: " + category.CategoryName);
                    if (desiredCategories.Contains(category.CategoryName))
                    {
                        PerformanceCounter[] counters;
                        try
                        {
                            counters = category.GetCounters("devenv");
                        }
                        catch (Exception)
                        {
                            counters = category.GetCounters();
                        }

                        foreach (var counter in counters)
                        {
                            sb.AppendLine(counter.CounterName + ": " + counter.CounterHelp);
                        }
                    }
                }
                File.WriteAllText(@"C:\New\performanceCounters.txt", sb.ToString());





            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
            }
            // отлов всех исключений в рамках работы приложения
            catch (Exception exception)
            {
                //NLog: устанавливаем отлов исключений
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // остановка логера 
                NLog.LogManager.Shutdown();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders(); // создание провайдеров логирования
                logging.SetMinimumLevel(LogLevel.Trace); // устанавливаем минимальный уровень логирования
            }).UseNLog(); // добавляем библиотеку nlog
    }
}
