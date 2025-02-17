﻿// <auto-generated />
using System;
using FinanceManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250208201748_AddPeriodSoftDelete")]
    partial class AddPeriodSoftDelete
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FinanceManagement.Core.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.FinancialTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountIdentifier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsExpense")
                        .HasColumnType("bit");

                    b.Property<int>("PeriodId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AccountIdentifier");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PeriodId");

                    b.ToTable("FinancialTransactions");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.Period", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.FinancialTransaction", b =>
                {
                    b.HasOne("FinanceManagement.Core.Entities.Account", "Account")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("AccountIdentifier")
                        .HasPrincipalKey("Identifier");

                    b.HasOne("FinanceManagement.Core.Entities.Category", "Category")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinanceManagement.Core.Entities.Period", "Period")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Category");

                    b.Navigation("Period");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.Account", b =>
                {
                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.Category", b =>
                {
                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("FinanceManagement.Core.Entities.Period", b =>
                {
                    b.Navigation("FinancialTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
