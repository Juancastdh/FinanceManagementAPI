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

                    context.SaveChanges();
                }
            }
        }
    }
}
