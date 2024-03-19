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
                .HasIndex(i => i.ClipEmbedding)
                .HasMethod("hnsw")
                .HasOperators("vector_l2_ops")
                .HasStorageParameter("m", 16)
                .HasStorageParameter("ef_construction", 64);
        }

        public DbSet<Reel> Reels { get; set; }
        public DbSet<LongVideo> LongVideos { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Audio> Audio { get; set; }
        public DbSet<GenericFile> Files { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
    }
}
