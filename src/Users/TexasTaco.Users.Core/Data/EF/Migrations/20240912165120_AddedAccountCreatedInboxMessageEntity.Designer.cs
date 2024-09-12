﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TexasTaco.Users.Core.Data.EF;

#nullable disable

namespace TexasTaco.Users.Core.Data.EF.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20240912165120_AddedAccountCreatedInboxMessageEntity")]
    partial class AddedAccountCreatedInboxMessageEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TexasTaco.Users.Core.Entities.AccountCreatedInboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MessageStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Processed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Received")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("AccountCreatedInboxMessages");
                });

            modelBuilder.Entity("TexasTaco.Users.Core.Entities.Address", b =>
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

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AddressLine");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("TexasTaco.Users.Core.Entities.User", b =>
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

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TexasTaco.Users.Core.Entities.Address", b =>
                {
                    b.HasOne("TexasTaco.Users.Core.Entities.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("TexasTaco.Users.Core.Entities.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TexasTaco.Users.Core.Entities.User", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
