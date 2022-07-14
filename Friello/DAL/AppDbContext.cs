using System;
using Friello.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Friello.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bio>().HasData(
                new Bio
                {
                    Id = 1,
                    ImageUrl = "favicon.png",
                    AuthorName = "Samir",
                    Facebook = "facebook.com",
                    Linkedin = "linkedin.com"
                }
                );
        }
    }
}
