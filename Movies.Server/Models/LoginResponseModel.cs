using System;
using System.Runtime.Serialization;

namespace Movies.Server.Models
{
	[DataContract]
	public class LoginResponseModel
	{
		[DataMember] public string UserName { get; set; }
		[DataMember] public string AccessToken { get; set; }
		[DataMember] public string RefreshToken { get; set; }
		[DataMember] public DateTime ExpireDateTime { get; set; }
	}
}
