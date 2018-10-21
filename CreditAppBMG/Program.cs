using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CreditAppBMG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
            {
                var environment = webHostBuilderContext.HostingEnvironment;
                string pathOfCommonSettingsFile = Path.Combine(environment.ContentRootPath, "..", "Common");
                configurationbuilder
                        .AddJsonFile("appSettings.json", optional: true)
                        .AddJsonFile(Path.Combine(pathOfCommonSettingsFile, "CommonSettings.json"), optional: true);

                configurationbuilder.AddEnvironmentVariables();
            })
                .UseStartup<Startup>();

    }
}
