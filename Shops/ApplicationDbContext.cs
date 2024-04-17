using Microsoft.EntityFrameworkCore;
using Shops.Models;
using System;

namespace Shops
{
    public class ApplicationDbContext : DbContext
    {
        private string? connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public ApplicationDbContext(string? connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Models.Type> types { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ProductsInBasket> productsInBaskets { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Basket> baskets { get; set; }
        public object Users { get; internal set; }
    }

    }
