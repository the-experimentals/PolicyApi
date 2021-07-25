using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PolicyApi.Services.SQLServer
{
    public class PolicyStoreFactory : IDesignTimeDbContextFactory<PolicyStore>
    {
        public PolicyStore CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PolicyStore>();

            var connectionString = configuration.GetConnectionString("PolicyStoreConnectionString");

            builder.UseSqlServer(connectionString);

            return new PolicyStore(builder.Options);
        }
    }
}
