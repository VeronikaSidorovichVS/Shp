using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Shops.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleName);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    typeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    typeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_types", x => x.typeId);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Existing = table.Column<bool>(type: "boolean", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: false),
                    Img = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    typeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_product_types_typeId",
                        column: x => x.typeId,
                        principalTable: "types",
                        principalColumn: "typeId");
                });

            migrationBuilder.CreateTable(
                name: "Characteristic",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristic", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Characteristic_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: false),
                    userName = table.Column<string>(type: "text", nullable: false),
                    roleName = table.Column<string>(type: "text", nullable: false),
                    roleName1 = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                    table.ForeignKey(
                        name: "FK_User_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_User_roles_roleName1",
                        column: x => x.roleName1,
                        principalTable: "roles",
                        principalColumn: "roleName");
                });

            migrationBuilder.CreateTable(
                name: "baskets",
                columns: table => new
                {
                    basketId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baskets", x => x.basketId);
                    table.ForeignKey(
                        name: "FK_baskets_User_userId1",
                        column: x => x.userId1,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "productsInBaskets",
                columns: table => new
                {
                    basketProductId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productId = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    basketId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productsInBaskets", x => x.basketProductId);
                    table.ForeignKey(
                        name: "FK_productsInBaskets_baskets_basketId",
                        column: x => x.basketId,
                        principalTable: "baskets",
                        principalColumn: "basketId");
                    table.ForeignKey(
                        name: "FK_productsInBaskets_product_productId",
                        column: x => x.productId,
                        principalTable: "product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristic_ProductId",
                table: "Characteristic",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProductId",
                table: "User",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleName1",
                table: "User",
                column: "roleName1");

            migrationBuilder.CreateIndex(
                name: "IX_baskets_userId1",
                table: "baskets",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_product_typeId",
                table: "product",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "IX_productsInBaskets_basketId",
                table: "productsInBaskets",
                column: "basketId");

            migrationBuilder.CreateIndex(
                name: "IX_productsInBaskets_productId",
                table: "productsInBaskets",
                column: "productId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristic");

            migrationBuilder.DropTable(
                name: "productsInBaskets");

            migrationBuilder.DropTable(
                name: "baskets");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "types");
        }
    }
}
