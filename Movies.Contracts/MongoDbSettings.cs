namespace Movies.Contracts;

public class MongoDbSettings
{
	public string ConnectionString;
	public string Database;

	//Use to configuration

	#region Const Values

	public const string ConnectionStringValue = nameof(ConnectionString);
	public const string DatabaseValue = nameof(Database);

	#endregion
}