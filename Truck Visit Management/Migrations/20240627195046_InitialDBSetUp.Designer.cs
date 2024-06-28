﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Truck_Visit_Management.Data;

#nullable disable

namespace Truck_Visit_Management.Migrations
{
    [DbContext(typeof(TruckVisitDbContext))]
    [Migration("20240627195046_InitialDBSetUp")]
    partial class InitialDBSetUp
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Truck_Visit_Management.Entities.ActivityEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VisitRecordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VisitRecordId");

                    b.ToTable("ActivityEntity");
                });

            modelBuilder.Entity("Truck_Visit_Management.Entities.VisitRecordEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TruckLicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("VisitRecordEntity");
                });

            modelBuilder.Entity("Truck_Visit_Management.Entities.ActivityEntity", b =>
                {
                    b.HasOne("Truck_Visit_Management.Entities.VisitRecordEntity", "VisitRecord")
                        .WithMany("Activities")
                        .HasForeignKey("VisitRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VisitRecord");
                });

            modelBuilder.Entity("Truck_Visit_Management.Entities.VisitRecordEntity", b =>
                {
                    b.OwnsOne("Truck_Visit_Management.Entities.DriverInformationEntity", "Driver", b1 =>
                        {
                            b1.Property<int>("VisitRecordEntityId")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("VisitRecordEntityId");

                            b1.ToTable("DriverInformationEntity");

                            b1.WithOwner()
                                .HasForeignKey("VisitRecordEntityId");
                        });

                    b.Navigation("Driver")
                        .IsRequired();
                });

            modelBuilder.Entity("Truck_Visit_Management.Entities.VisitRecordEntity", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
