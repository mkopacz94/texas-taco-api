﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TexasTaco.Authentication.Core.Data.EF;

#nullable disable

namespace TexasTaco.Authentication.Core.Data.EF.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    partial class AuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TexasTaco.Authentication.Core.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.ToTable("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
