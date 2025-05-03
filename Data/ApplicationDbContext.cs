using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : DbContext
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

            //add functionaltiy which would delete all comments of a stock if deleting stock.
            modelBuilder.Entity<Comment>().HasOne(s => s.Stock)
                                          .WithMany(c => c.Comments)
                                          .HasForeignKey(s => s.StockId)
                                          .OnDelete(DeleteBehavior.Cascade);
        }
    }
}