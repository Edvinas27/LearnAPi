using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //keep basic functionality
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "User",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            //add functionaltiy which would delete all comments of a stock if deleting stock.
            modelBuilder.Entity<Comment>().HasOne(s => s.Stock)
                                          .WithMany(c => c.Comments)
                                          .HasForeignKey(s => s.StockId)
                                          .OnDelete(DeleteBehavior.Cascade);
        }
    }
}