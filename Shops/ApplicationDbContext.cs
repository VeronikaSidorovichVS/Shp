using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Controllers;
using Shops.Models;
using System;
using static Shops.Models.modelcs;

namespace Shops
{
    public class ApplicationDbContext : DbContext
    {
        private string? connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);
           

        }
        //public ApplicationDbContext(string? connectionString)
        //{
        //    this.connectionString = connectionString;
        //}
        
        public DbSet<Models.Type> types { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ProductsInBasket> productsInBaskets { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Basket> baskets { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderData> OrderDatas { get; set; }
       
    }

    }
