using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.Common
{
    public class aLMSDbContextFactory : IDesignTimeDbContextFactory<aLMSDbContext>
    {
        public aLMSDbContext CreateDbContext(string[] args)
        {
            
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../aLMS.Api");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<aLMSDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new aLMSDbContext(optionsBuilder.Options);
        }
    }
}
