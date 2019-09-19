using Microsoft.EntityFrameworkCore;
using Sample.Demo.Data;
using Sample.Shared.Utilities.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.DataService
{
    public class DemoDbContext : AuditDbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedData(); // An extension method written specifically for adding the seed data
        }
    }
}
