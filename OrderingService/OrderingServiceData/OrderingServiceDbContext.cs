using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderingServiceData
{
    public class OrderingServiceDbContext : DbContext
    {
        private string DbPath { get; }

        public DbSet<Entities.Customer> Customers { get; set; }
        public DbSet<Entities.Item> Items { get; set; }
        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<Entities.ApplicationLog> ApplicationLogs { get; set; }

        public OrderingServiceDbContext()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join("../..", "ordering_service.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationLog>()
                .Property(u => u.Event)
                .HasConversion<short>();

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderID, oi.ItemID });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany()
                .HasForeignKey(oi => oi.ItemID);
        }
    }
}
