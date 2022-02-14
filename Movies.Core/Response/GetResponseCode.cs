using Microsoft.Extensions.Configuration;
using System.IO;

namespace Movies.Core.Response;

public static class GetResponseCode
{
	public static IConfiguration GetConfiguration() =>
		new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", true, true).Build();

	public static string GetDescription(string RC)
	{
		var config = GetConfiguration();
		return config.GetSection("ResponseCodes:" + RC).Get<string>();
	}
}