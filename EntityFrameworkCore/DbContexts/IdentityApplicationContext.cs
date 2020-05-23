using Common.Entities.UserEnts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.DbContexts
{
    public class IdentityApplicationContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<JwtRefreshToken> JwtRefreshTokens { get; set; }
        public IdentityApplicationContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationStoreoptions) : base (options, operationStoreoptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //database is seeded in here if it requires it - but requires some more invesitagation to get this sorted
            //base.OnModelCreating(builder.Seed);
        }
    }
}
