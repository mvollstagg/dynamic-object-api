﻿// <auto-generated />
using System;
using IODynamicObject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IODynamicObject.Infrastructure.Migrations
{
    [DbContext(typeof(IODataContext))]
    [Migration("20240928182946_OnModelCreatingChanged")]
    partial class OnModelCreatingChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOCustomer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOField", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("ObjectId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ObjectId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOObject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("EntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("IOCustomerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IOOrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IOProductId")
                        .HasColumnType("bigint");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IOCustomerId");

                    b.HasIndex("IOOrderId");

                    b.HasIndex("IOProductId");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("OrderDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("OrderStatus")
                        .HasColumnType("tinyint unsigned");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOOrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOProduct", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("FieldId")
                        .HasColumnType("bigint");

                    b.Property<long>("ModificatedById")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOField", b =>
                {
                    b.HasOne("IODynamicObject.Domain.Entities.IOObject", "Object")
                        .WithMany("Fields")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Object");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOObject", b =>
                {
                    b.HasOne("IODynamicObject.Domain.Entities.IOCustomer", null)
                        .WithMany("DynamicObjects")
                        .HasForeignKey("IOCustomerId");

                    b.HasOne("IODynamicObject.Domain.Entities.IOOrder", null)
                        .WithMany("DynamicObjects")
                        .HasForeignKey("IOOrderId");

                    b.HasOne("IODynamicObject.Domain.Entities.IOProduct", null)
                        .WithMany("DynamicObjects")
                        .HasForeignKey("IOProductId");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOOrder", b =>
                {
                    b.HasOne("IODynamicObject.Domain.Entities.IOCustomer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOOrderItem", b =>
                {
                    b.HasOne("IODynamicObject.Domain.Entities.IOOrder", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IODynamicObject.Domain.Entities.IOProduct", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOValue", b =>
                {
                    b.HasOne("IODynamicObject.Domain.Entities.IOField", "Field")
                        .WithMany("Values")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOCustomer", b =>
                {
                    b.Navigation("DynamicObjects");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOField", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOObject", b =>
                {
                    b.Navigation("Fields");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOOrder", b =>
                {
                    b.Navigation("DynamicObjects");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("IODynamicObject.Domain.Entities.IOProduct", b =>
                {
                    b.Navigation("DynamicObjects");
                });
#pragma warning restore 612, 618
        }
    }
}
