using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Models.Entities;

namespace Admin.DAL
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyCon")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .Property(x => x.TaxRate)
                .HasPrecision(3, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.BuyPrice)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.SalesPrice)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Invoice>()
                .Property(x => x.Quantity)
                .HasPrecision(8, 4);

            modelBuilder.Entity<Invoice>()
                .Property(x => x.Price)
                .HasPrecision(9, 3);

            modelBuilder.Entity<Invoice>()
                .Property(x => x.Discount)
                .HasPrecision(3, 2);
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
