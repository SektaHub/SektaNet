using backend.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using Pgvector.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using backend.Models;

namespace backend
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=baza;User Id=postgres;Password=sektaadmin;Trust Server Certificate=True;", o => o.UseVector());
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<Image>()
                .HasIndex(i => i.CaptionEmbedding)
                .HasMethod("ivfflat")
                .HasOperators("vector_l2_ops")
                .HasStorageParameter("lists", 100);
        }

        public DbSet<Reel> Reels { get; set; }
        public DbSet<Image> Images { get; set; }

    }
}
