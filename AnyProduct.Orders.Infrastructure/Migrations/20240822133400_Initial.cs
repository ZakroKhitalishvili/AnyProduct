using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnyProduct.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up([NotNull] MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "baskets",
                schema: "orders",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baskets", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "buyers",
                schema: "orders",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyers", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                schema: "orders",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventTypeName = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    TimesSent = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: false),
                    Address_City = table.Column<string>(type: "text", nullable: false),
                    Address_State = table.Column<string>(type: "text", nullable: false),
                    Address_Country = table.Column<string>(type: "text", nullable: false),
                    Address_ZipCode = table.Column<string>(type: "text", nullable: false),
                    BuyerId = table.Column<string>(type: "text", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderStatus = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PaymentMethodId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "orders",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Orderid = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<string>(type: "text", nullable: false),
                    PaymentMethodDescription = table.Column<string>(type: "text", nullable: false),
                    ExternalPaymentRequestId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Units = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerBasketAggregateId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_baskets_CustomerBasketAggregateId",
                        column: x => x.CustomerBasketAggregateId,
                        principalSchema: "orders",
                        principalTable: "baskets",
                        principalColumn: "AggregateId");
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Alias = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    SecurityNumber = table.Column<string>(type: "text", nullable: false),
                    CardHolderName = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CardType = table.Column<int>(type: "integer", nullable: false),
                    BuyerAggregateId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payment_methods_buyers_BuyerAggregateId",
                        column: x => x.BuyerAggregateId,
                        principalSchema: "orders",
                        principalTable: "buyers",
                        principalColumn: "AggregateId");
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Units = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderAggregateId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderAggregateId",
                        column: x => x.OrderAggregateId,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "AggregateId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_CustomerBasketAggregateId",
                schema: "orders",
                table: "BasketItem",
                column: "CustomerBasketAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_baskets_AggregateId",
                schema: "orders",
                table: "baskets",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_buyers_AggregateId",
                schema: "orders",
                table: "buyers",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_items_OrderAggregateId",
                schema: "orders",
                table: "order_items",
                column: "OrderAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_AggregateId",
                schema: "orders",
                table: "orders",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_BuyerAggregateId",
                schema: "orders",
                table: "payment_methods",
                column: "BuyerAggregateId");
        }

        /// <inheritdoc />
        protected override void Down([NotNull] MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItem",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "IntegrationEventLog",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "order_items",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "payment_methods",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "baskets",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "buyers",
                schema: "orders");
        }
    }
}
