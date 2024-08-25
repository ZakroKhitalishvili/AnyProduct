﻿// <auto-generated />
using System;
using AnyProduct.Orders.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnyProduct.Orders.Infrastructure.Migrations
{
    [DbContext(typeof(OrderContext))]
    [Migration("20240823152531_Orders_MadeSomePropertiesOptional")]
    partial class Orders_MadeSomePropertiesOptional
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("orders")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Balance.Payment", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExternalPaymentRequestId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Orderid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PaymentMethodDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("AggregateId");

                    b.ToTable("payments", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Basket.BasketItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CustomerBasketAggregateId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Units")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerBasketAggregateId");

                    b.ToTable("BasketItem", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Basket.CustomerBasket", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AggregateId");

                    b.HasIndex("AggregateId")
                        .IsUnique();

                    b.ToTable("baskets", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Buyer.Buyer", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AggregateId");

                    b.HasIndex("AggregateId")
                        .IsUnique();

                    b.ToTable("buyers", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Buyer.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("BuyerAggregateId")
                        .HasColumnType("uuid");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CardType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BuyerAggregateId");

                    b.ToTable("payment_methods", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Order.Order", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuyerId")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("integer");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("uuid");

                    b.Property<string>("PaymentMethodId")
                        .HasColumnType("text");

                    b.HasKey("AggregateId");

                    b.HasIndex("AggregateId")
                        .IsUnique();

                    b.ToTable("orders", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Order.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<Guid?>("OrderAggregateId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<decimal?>("UnitPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("Units")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderAggregateId");

                    b.ToTable("order_items", "orders");
                });

            modelBuilder.Entity("AnyProduct.OutBox.EF.IntegrationEventLogEntry", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("TimesSent")
                        .HasColumnType("integer");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventLog", "orders");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Basket.BasketItem", b =>
                {
                    b.HasOne("AnyProduct.Orders.Domain.Entities.Basket.CustomerBasket", null)
                        .WithMany("Items")
                        .HasForeignKey("CustomerBasketAggregateId");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Buyer.PaymentMethod", b =>
                {
                    b.HasOne("AnyProduct.Orders.Domain.Entities.Buyer.Buyer", null)
                        .WithMany("PaymentMethods")
                        .HasForeignKey("BuyerAggregateId");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Order.Order", b =>
                {
                    b.OwnsOne("AnyProduct.Orders.Domain.Entities.Order.AddressValueObject", "Address", b1 =>
                        {
                            b1.Property<Guid>("OrderAggregateId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OrderAggregateId");

                            b1.ToTable("orders", "orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderAggregateId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Order.OrderItem", b =>
                {
                    b.HasOne("AnyProduct.Orders.Domain.Entities.Order.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderAggregateId");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Basket.CustomerBasket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Buyer.Buyer", b =>
                {
                    b.Navigation("PaymentMethods");
                });

            modelBuilder.Entity("AnyProduct.Orders.Domain.Entities.Order.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
