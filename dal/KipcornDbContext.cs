using Microsoft.EntityFrameworkCore;
using models;
using System;
using System.Collections.Generic;
using System.Text;

namespace dal
{
    public class KipcornDbContext : DbContext
    {
        public DbSet<Artikel> Artikels { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Categorie> Categorieen { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Orderlijn> Orderlijnen { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Vestiging> Vestigingen { get; set; }
        public DbSet<WinkelmandItem> WinkelmandItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Kipcorn;Trusted_Connection=True;");
        }
    }
}
