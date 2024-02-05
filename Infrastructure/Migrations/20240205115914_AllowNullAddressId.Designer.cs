﻿// <auto-generated />
using System;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20240205115914_AllowNullAddressId")]
    partial class AllowNullAddressId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Entities.AddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("varchar(6)");

                    b.Property<string>("StreetName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Infrastructure.Entities.ContactEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PhoneNumberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumberId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Infrastructure.Entities.PhoneNumberEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.HasKey("Id");

                    b.ToTable("PhoneNumbers");
                });

            modelBuilder.Entity("Infrastructure.Entities.ContactEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.AddressEntity", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("Infrastructure.Entities.PhoneNumberEntity", "PhoneNumber")
                        .WithMany()
                        .HasForeignKey("PhoneNumberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("PhoneNumber");
                });
#pragma warning restore 612, 618
        }
    }
}
