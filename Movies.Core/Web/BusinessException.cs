using Movies.Core.Response;
using System;

namespace Movies.Core.Web;

public class BusinessException : Exception
{
	public BusinessException()
	{
	}

	public BusinessException(string rc, string detail)
	{
		RC = rc;
		Message = GetResponseCode.GetDescription(RC);
		Details = detail;
	}

	public string Message { get; set; }
	public string RC { get; set; }
	public string Details { get; set; }

	public static string GetDescription(string RC) => GetResponseCode.GetDescription(RC);
}