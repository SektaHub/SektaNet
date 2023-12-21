using backend.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace backend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseInMemoryDatabase("YourInMemoryDatabaseName");
        }
        public DbSet<Reel> Reels { get; set; }
    }
}
