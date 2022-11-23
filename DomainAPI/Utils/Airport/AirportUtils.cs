using System.IO;
using Microsoft.Extensions.Configuration;

namespace DomainAPI.Utils.Airport
{
    public class AirportUtils
    {
        public static IConfigurationRoot Configuration { get; set; }

        //Busca o endereço do endpoit no arquivo JSON appsettings
        public static string GetAPIUri(string uriJsonName)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
            return Configuration["DatabaseSettings:" + uriJsonName];
        }
    }
}
