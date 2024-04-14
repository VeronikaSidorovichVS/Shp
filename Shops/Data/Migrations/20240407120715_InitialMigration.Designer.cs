﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shops;

#nullable disable

namespace Shops.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240407120715_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shops.Models.Basket", b =>
                {
                    b.Property<Guid>("basketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("userId")
                        .HasColumnType("uuid");

                    b.Property<int?>("userId1")
                        .HasColumnType("integer");

                    b.HasKey("basketId");

                    b.HasIndex("userId1");

                    b.ToTable("baskets");
                });

            modelBuilder.Entity("Shops.Models.Characteristic", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.HasIndex("ProductId");

                    b.ToTable("Characteristic");
                });

            modelBuilder.Entity("Shops.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<int>("Discount")
                        .HasColumnType("integer");

                    b.Property<bool>("Existing")
                        .HasColumnType("boolean");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("typeId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("typeId");

                    b.ToTable("product");
                });

            modelBuilder.Entity("Shops.Models.ProductsInBasket", b =>
                {
                    b.Property<int>("basketProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("basketProductId"));

                    b.Property<Guid?>("basketId")
                        .HasColumnType("uuid");

                    b.Property<int>("productId")
                        .HasColumnType("integer");

                    b.Property<int>("quantity")
                        .HasColumnType("integer");

                    b.HasKey("basketProductId");

                    b.HasIndex("basketId");

                    b.HasIndex("productId");

                    b.ToTable("productsInBaskets");
                });

            modelBuilder.Entity("Shops.Models.Role", b =>
                {
                    b.Property<string>("roleName")
                        .HasColumnType("text");

                    b.HasKey("roleName");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Shops.Models.Type", b =>
                {
                    b.Property<int>("typeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("typeId"));

                    b.Property<string>("typeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("typeId");

                    b.ToTable("types");
                });

            modelBuilder.Entity("Shops.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("userId"));

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("avatar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("roleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("roleName1")
                        .HasColumnType("text");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("userId");

                    b.HasIndex("ProductId");

                    b.HasIndex("roleName1");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Shops.Models.Basket", b =>
                {
                    b.HasOne("Shops.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId1");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Shops.Models.Characteristic", b =>
                {
                    b.HasOne("Shops.Models.Product", null)
                        .WithMany("Characteristics")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Shops.Models.Product", b =>
                {
                    b.HasOne("Shops.Models.Type", null)
                        .WithMany("products")
                        .HasForeignKey("typeId");
                });

            modelBuilder.Entity("Shops.Models.ProductsInBasket", b =>
                {
                    b.HasOne("Shops.Models.Basket", "basket")
                        .WithMany("products")
                        .HasForeignKey("basketId");

                    b.HasOne("Shops.Models.Product", "product")
                        .WithMany()
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("basket");

                    b.Navigation("product");
                });

            modelBuilder.Entity("Shops.Models.User", b =>
                {
                    b.HasOne("Shops.Models.Product", null)
                        .WithMany("ProductsInBasket")
                        .HasForeignKey("ProductId");

                    b.HasOne("Shops.Models.Role", "role")
                        .WithMany("Users")
                        .HasForeignKey("roleName1");

                    b.Navigation("role");
                });

            modelBuilder.Entity("Shops.Models.Basket", b =>
                {
                    b.Navigation("products");
                });

            modelBuilder.Entity("Shops.Models.Product", b =>
                {
                    b.Navigation("Characteristics");

                    b.Navigation("ProductsInBasket");
                });

            modelBuilder.Entity("Shops.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Shops.Models.Type", b =>
                {
                    b.Navigation("products");
                });
#pragma warning restore 612, 618
        }
    }
}