﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.Demo.DataService;

namespace Sample.Demo.DataService.Migrations
{
    [DbContext(typeof(DemoDbContext))]
    [Migration("20190618093038_AuditLogImpl")]
    partial class AuditLogImpl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sample.Demo.Data.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("UserTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Employee"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = 3,
                            Name = "HR"
                        });
                });

            modelBuilder.Entity("Sample.Shared.Utilities.Audit.AuditLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventType")
                        .HasMaxLength(100);

                    b.Property<string>("NewValue");

                    b.Property<string>("OriginalValue");

                    b.Property<string>("PrimaryKeyNames")
                        .HasMaxLength(100);

                    b.Property<string>("PrimaryKeyValues")
                        .HasMaxLength(100);

                    b.Property<string>("PropertyName")
                        .HasMaxLength(100);

                    b.Property<string>("TableName")
                        .HasMaxLength(100);

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });
#pragma warning restore 612, 618
        }
    }
}