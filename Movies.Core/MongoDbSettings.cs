using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core
{
	public interface IMongoDbSettings
	{
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
	public class MongoDbSettings: IMongoDbSettings
	{
		public MongoDbSettings(IConfiguration config)
		{
			ConnectionString = config.GetSection("MongoDbSettings:" + "ConnectionString").Get<string>();
			DatabaseName = config.GetSection("MongoDbSettings:" + "DatabaseName").Get<string>();
		}

		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}
