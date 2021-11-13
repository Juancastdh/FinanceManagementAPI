using FinanceManagement.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.DataAccess
{
    public static class PrepareDatabase
    {      
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            IEnumerable<Category> categoriesToAdd = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "PC",
                    Percentage = 15
                },
                new Category
                {
                    Id = 2,
                    Name = "Survival",
                    Percentage = 25
                }
            };

            IEnumerable<Period> periodsToAdd = new List<Period>
            {
                new Period
                {
                    Id = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(13)
                },
                new Period
                {
                    Id = 2,
                    StartDate = DateTime.Now.AddDays(13),
                    EndDate = DateTime.Now.AddDays(26)
                }
            };

            IEnumerable<FinancialTransaction> financialTransactionsToAdd = new List<FinancialTransaction>
            {
                new FinancialTransaction
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Description = "GPU",
                    IsExpense = true,
                    CategoryId = 1,
                    PeriodId = 1,
                    Value = 30000
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Description = "Wendys",
                    IsExpense = true,
                    CategoryId = 2,
                    PeriodId = 1,
                    Value = 575
                },
                new FinancialTransaction
                {
                    Id = 3,
                    Date = DateTime.Now,
                    Description = "Initial Income PC",
                    IsExpense = false,
                    CategoryId = 1,
                    PeriodId = 1,
                    Value = 10000
                }
            };



            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                using( var context = serviceScope.ServiceProvider.GetService<DatabaseContext>())
                {
                    if (!context.Categories.Any())
                    {
                        context.Categories.AddRange(categoriesToAdd);                       
                    }

                    if (!context.Periods.Any())
                    {
                        context.Periods.AddRange(periodsToAdd);
                    }

                    if (!context.FinancialTransactions.Any())
                    {
                        context.FinancialTransactions.AddRange(financialTransactionsToAdd);
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
