namespace Movies.Core.Response;

public static class ResponseCodes
{
	public const string Success = "RC0000";
	public const string NotNullable = "RC0001";
	public const string Failed = "RC0002";
	public const string NotFound = "RC0003";
	public const string Unauthorized = "RC0004";
	public const string BadRequest = "RC0005";
	public const string InvalidToken = "RC1000";
	public const string ExpireToken = "RC1001";
}