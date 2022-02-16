using System.Runtime.Serialization;

namespace Movies.Server.Models
{
	[DataContract]
	public class RefreshTokenResponseModel
	{
		[DataMember] public string AccessToken { get; set; }
	}
}
