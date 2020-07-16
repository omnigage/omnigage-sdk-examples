using System.IO;
using Microsoft.Extensions.Configuration;
using Omnigage;

namespace call_number
{
    public class OmnigageSettings
    {
        public string ApiTokenKey { get; set; }

        public string ApiTokenSecret { get; set; }

        public string ApiHost { get; set; }
    }

    public class Util
    {
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var config = configuration.GetSection("Omnigage")
                                      .Get<OmnigageSettings>();

            // Initialize SDK
            OmnigageClient.Init(config.ApiTokenKey, config.ApiTokenSecret, config.ApiHost);
        }
    }
}
