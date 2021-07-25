using System;
using Microsoft.EntityFrameworkCore;
using PolicyApi.DataModels;

namespace PolicyApi.Services.SQLServer
{
    public class PolicyStore : DbContext
    {
        public PolicyStore(DbContextOptions<PolicyStore> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public DbSet<PermissionCategories> PERMISSION_CATEGORIES { get; set; }
        public DbSet<Permissions> PERMISSIONS { get; set; }
        public DbSet<ProfileRolePermissions> PROFILE_ROLE_PERMISSIONS { get; set; }
        public DbSet<ProfileRoles> PROFILE_ROLES { get; set; }
        public DbSet<Roles> ROLES { get; set; }
    }
}
