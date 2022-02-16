using System.Runtime.Serialization;

namespace Movies.Server.Models
{
	[DataContract]
	public class RefreshTokenRequestModel
	{
		[DataMember] public string UserName { get; set; }
		[DataMember] public string RefreshToken { get; set; }
	}
}
