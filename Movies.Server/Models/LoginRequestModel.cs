using System.Runtime.Serialization;

namespace Movies.Server.Models
{
	[DataContract]
	public class LoginRequestModel
	{
		[DataMember]
		public string UserName { get; set; }
		[DataMember] public string Password { get; set; }
	}
}
