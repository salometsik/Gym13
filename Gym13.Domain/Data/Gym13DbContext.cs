﻿using Gym13.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Gym13.Domain.Data
{
    public class Gym13DbContext : DbContext
    {
        readonly IConfiguration configuration;
        public Gym13DbContext(DbContextOptions<Gym13DbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public Gym13DbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<PersistedGrant>().ToTable("PersistedGrants").HasKey(x => x.Key);
            builder.Entity<DeviceFlowCodes>().ToTable("DeviceFlowCodes").HasKey(x => x.UserCode);
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
    }
}
