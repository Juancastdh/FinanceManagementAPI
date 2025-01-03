using FinanceManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Period>? Periods { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<FinancialTransaction>? FinancialTransactions { get; set; }
        public DbSet<Account>? Accounts { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Period>();
            modelBuilder.Entity<FinancialTransaction>().Property(p => p.Value).HasPrecision(18, 2);
            modelBuilder.Entity<FinancialTransaction>().HasOne(f => f.Category).WithMany(f => f.FinancialTransactions).HasForeignKey(f => f.CategoryId);
            modelBuilder.Entity<FinancialTransaction>().HasOne(f => f.Period).WithMany(f => f.FinancialTransactions).HasForeignKey(f => f.PeriodId);
            modelBuilder.Entity<Account>();
        }

    }
}
