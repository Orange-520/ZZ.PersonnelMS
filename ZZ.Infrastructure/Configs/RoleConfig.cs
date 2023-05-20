﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Identity;

namespace ZZ.Infrastructure.Configs
{
	public class RoleConfig : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("T_Roles");
		}
	}
}