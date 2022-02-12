using System.IO;
using Microsoft.Extensions.Configuration;

namespace Movies.Core.Response
{
	public static class GetResponseCode
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        }
        public static string GetDescription(string RC)
        {
            IConfiguration config = GetConfiguration();
            return config.GetSection("ResponseCodes:"+RC).Get<string>();
        }
    }
}
