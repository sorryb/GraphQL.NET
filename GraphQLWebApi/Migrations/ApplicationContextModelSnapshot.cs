﻿// <auto-generated />
using System;
using GraphQLWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GraphQLWebApi.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GraphQLWebApi.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a7ecfad1-6452-4f0b-a1a9-b94e02edc0ca"),
                            Description = "Cash account for our users",
                            OwnerId = new Guid("fce63739-eb06-4a55-b58f-eca4dbdd8265"),
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("a97140f6-2cb7-4906-990b-1d588ed0a89c"),
                            Description = "Savings account for our users",
                            OwnerId = new Guid("e5597d0a-0b5b-4c1e-9d5a-b56f759f3cfb"),
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("e2089dfb-26f7-4bea-80b1-20e3606c3834"),
                            Description = "Income account for our users",
                            OwnerId = new Guid("e5597d0a-0b5b-4c1e-9d5a-b56f759f3cfb"),
                            Type = 3
                        });
                });

            modelBuilder.Entity("GraphQLWebApi.Entities.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Owners");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fce63739-eb06-4a55-b58f-eca4dbdd8265"),
                            Address = "John Doe's address",
                            Name = "John Doe"
                        },
                        new
                        {
                            Id = new Guid("e5597d0a-0b5b-4c1e-9d5a-b56f759f3cfb"),
                            Address = "Jane Doe's address",
                            Name = "Jane Doe"
                        });
                });

            modelBuilder.Entity("GraphQLWebApi.Entities.Account", b =>
                {
                    b.HasOne("GraphQLWebApi.Entities.Owner", "Owner")
                        .WithMany("Accounts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("GraphQLWebApi.Entities.Owner", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
