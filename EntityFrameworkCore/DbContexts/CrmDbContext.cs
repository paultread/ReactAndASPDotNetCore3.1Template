using Common.Entities.Company;
using Common.Entities.TechInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.DbContexts
{
    public class CrmDbContext: DbContext
    {
        //these must be teh same name as the entity
        public DbSet<Company> Company { get; set; }
        public DbSet<TechInfo> TechInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"> holds the connection string (and other things), for the test env derived from User Secrets > this is obtained & set in DbConfigurer</param>
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {

        }
    }
}
