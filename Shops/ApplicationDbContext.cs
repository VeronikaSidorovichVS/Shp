using Microsoft.EntityFrameworkCore;
using Shops.Models;
using System;

namespace Shops
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Typee> types { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ProductsInBasket> productsInBaskets { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Basket> baskets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

    }

}
