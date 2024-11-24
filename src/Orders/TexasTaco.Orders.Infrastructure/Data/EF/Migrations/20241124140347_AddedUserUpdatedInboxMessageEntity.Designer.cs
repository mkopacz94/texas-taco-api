﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TexasTaco.Orders.Infrastructure.Data.EF;

#nullable disable

namespace TexasTaco.Orders.Infrastructure.Data.EF.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20241124140347_AddedUserUpdatedInboxMessageEntity")]
    partial class AddedUserUpdatedInboxMessageEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TexasTaco.Orders.Domain.AccountCreatedInboxMessages.AccountCreatedInboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<int>("MessageStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Processed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Received")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("AccountCreatedInboxMessages");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Basket.Basket", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Basket.BasketItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("BasketId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Customers.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AddressLine")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AddressLine");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<int>("PointsCollected")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.UserUpdatedInboxMessages.UserUpdatedInboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<int>("MessageStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Processed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Received")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("UserUpdatedInboxMessages");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Basket.BasketItem", b =>
                {
                    b.HasOne("TexasTaco.Orders.Domain.Basket.Basket", "Basket")
                        .WithMany("Items")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Basket");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Customers.Address", b =>
                {
                    b.HasOne("TexasTaco.Orders.Domain.Customers.Customer", "Customer")
                        .WithOne("Address")
                        .HasForeignKey("TexasTaco.Orders.Domain.Customers.Address", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Basket.Basket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("TexasTaco.Orders.Domain.Customers.Customer", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
