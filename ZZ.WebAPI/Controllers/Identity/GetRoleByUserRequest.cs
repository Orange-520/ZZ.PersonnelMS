using System.ComponentModel.DataAnnotations;

namespace ZZ.WebAPI.Controllers.Identity
{
	public class GetRoleByUserRequest
	{
		[Required]
		public string UserId { get; set; }
	}
}
