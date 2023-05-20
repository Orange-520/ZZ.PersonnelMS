using Microsoft.AspNetCore.Identity;
using ZZ.Domain.Entities.Identity;

namespace ZZ.Domain
{
	public class IdentityService
	{
		private readonly IIdentityRepository identityRepository;
		private readonly ICommonRepository commonRepository;

		public IdentityService(ICommonRepository commonRepository, IIdentityRepository identityRepository)
		{
			this.commonRepository = commonRepository;
			this.identityRepository = identityRepository;
		}

	}
}
