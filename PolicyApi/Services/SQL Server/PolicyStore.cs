using System;
using Microsoft.EntityFrameworkCore;

namespace PolicyApi.Services.SQLServer
{
    public class PolicyStore : DbContext
    {
        public PolicyStore(DbContextOptions<PolicyStore> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
