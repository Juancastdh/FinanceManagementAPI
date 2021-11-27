using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using Xunit;

namespace FinanceManagement.Tests
{
    public class FinancialTransactionsManagerTests
    {

        [Fact]
        public void AddFinancialTransaction_Adds_FinancialTransactions_Correctly_To_Repository()
        {
            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.Add(It.IsAny<FinancialTransaction>())).Callback((FinancialTransaction financialTransaction) => mockFinancialTransactionsDatabase.Add(financialTransaction));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            FinancialTransaction expectedFinancialTransaction = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Transaction",
                IsExpense = true,
                Value = 1000,
                CategoryId = 1,
                Category = new Category
                {
                    Id=1,
                    Name = "TestCategory",
                    Percentage = 5
                }
            };

            string expectedFinancialTransactionString = JsonSerializer.Serialize(expectedFinancialTransaction);

            //Act
            financialTransactionsManager.AddFinancialTransaction(expectedFinancialTransaction);
            FinancialTransaction addedFinancialTransaction = mockFinancialTransactionsDatabase.Single();
            string addedFinancialTransactionString = JsonSerializer.Serialize(addedFinancialTransaction);

            //Assert
            Assert.Equal(expectedFinancialTransactionString, addedFinancialTransactionString);
        }

        [Fact]
        public void DeleteFinancialTransactionById_Removes_FinancialTransactions_Correctly_From_Repository()
        {

            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            FinancialTransaction transactionToRemain = new FinancialTransaction
            {
                Id = 1,
                Description = "Transaction that should remain in the repository",
                IsExpense = true,
                Value = 1000
            };

            FinancialTransaction transactionToBeDeleted = new FinancialTransaction
            {
                Id = 2,
                Description = "Transaction that should be removed from the repository",
                IsExpense = false,
                Value = 2500
            };

            mockFinancialTransactionsDatabase.Add(transactionToRemain);
            mockFinancialTransactionsDatabase.Add(transactionToBeDeleted);
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.DeleteById(2)).Callback((int financialTransactionId) => {
                mockFinancialTransactionsDatabase.Remove(transactionToBeDeleted); 
            });
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange


            IEnumerable<FinancialTransaction> expectedFinancialTransactionsDatabase = new List<FinancialTransaction>
            {
                transactionToRemain
            };


            //Act
            financialTransactionsManager.DeleteFinancialTransactionById(transactionToBeDeleted.Id);

            //Assert
            Assert.Equal(expectedFinancialTransactionsDatabase, mockFinancialTransactionsDatabase);

        }

        [Fact]
        public void GetAllFinancialTransactions_Returns_All_Transactions_From_Repository()
        {
            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);


            //Arrange
            mockFinancialTransactionsDatabase.Add(new FinancialTransaction
            {
                Id = 1,
                Description = "Test Transaction 1",
                IsExpense = true,
                Value = 1000
            });
            mockFinancialTransactionsDatabase.Add(new FinancialTransaction
            {
                Id = 2,
                Description = "Test Transaction 2",
                IsExpense = false,
                Value = 2500
            });

            //Act
            IEnumerable<FinancialTransaction> returnedFinancialTransactions = financialTransactionsManager.GetAllFinancialTransactions();

            //Assert
            Assert.Equal(mockFinancialTransactionsDatabase, returnedFinancialTransactions);
        }



        [Fact]
        public void GetSumOfFinancialTransactionValues_Returns_Sum_Of_All_Incomes_Minus_All_Expenses()
        {

            //Setup    
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();          
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            List<FinancialTransaction> financialTransactions = new List<FinancialTransaction>();
            financialTransactions.Add(new FinancialTransaction
            {
                Id = 1,
                Description = "Test Income 1",
                IsExpense = false,
                Value = 300,
                PeriodId = 1,
                CategoryId = 1
            });
            financialTransactions.Add(new FinancialTransaction
            {
                Id = 2,
                Description = "Test Income 2",
                IsExpense = false,
                Value = 200,
                PeriodId = 2,
                CategoryId = 2
            });
            financialTransactions.Add(new FinancialTransaction
            {
                Id = 3,
                Description = "Test Income 3",
                IsExpense = false,
                Value = 50,
                PeriodId = 3,
                CategoryId = 1
            });
            financialTransactions.Add(new FinancialTransaction
            {
                Id = 3,
                Description = "Test Expense 1",
                IsExpense = true,
                Value = 100,
                PeriodId = 1,
                CategoryId = 1
            });
            financialTransactions.Add(new FinancialTransaction
            {
                Id = 4,
                Description = "Test Expense 2",
                IsExpense = true,
                Value = 150,
                PeriodId = 3,
                CategoryId = 4
            });
            decimal obtainedResult = financialTransactionsManager.GetSumOfFinancialTransactionValues(financialTransactions);
            decimal expectedResult = 300;

            //Assert
            Assert.Equal(expectedResult, obtainedResult);

        }


        [Fact]
        public void UpdateFinancialTransaction_Updates_FinancialTransactions_Correctly_To_Repository()
        {
            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.Update(It.IsAny<FinancialTransaction>())).Callback((FinancialTransaction financialTransaction) => mockFinancialTransactionsDatabase[0] = financialTransaction);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange

            FinancialTransaction originalFinancialTransaction = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Original Test Transaction",
                IsExpense = true,
                Value = 500,
                CategoryId = 1,
                Category = new Category
                {
                    Id = 1,
                    Name = "TestCategory",
                    Percentage = 5
                }
            };

            mockFinancialTransactionsDatabase.Add(originalFinancialTransaction);

            FinancialTransaction updatedFinancialTransaction = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Updated Test Transaction",
                IsExpense = true,
                Value = 1000,
                CategoryId = 1,
                Category = new Category
                {
                    Id = 1,
                    Name = "TestCategory",
                    Percentage = 5
                }
            };
            
            string updatedFinancialTransactionString = JsonSerializer.Serialize(updatedFinancialTransaction);

            //Act
            financialTransactionsManager.UpdateFinancialTransaction(updatedFinancialTransaction);
            FinancialTransaction obtainedUpdatedFinancialTransaction = mockFinancialTransactionsDatabase.Single();

            string obtainedUpdatedFinancialTransactionString = JsonSerializer.Serialize(obtainedUpdatedFinancialTransaction);

            //Assert
            Assert.Equal(updatedFinancialTransactionString, obtainedUpdatedFinancialTransactionString);
        }


        [Fact]
        public void GetFinancialTransactionById_Returns_Correct_FinancialTransaction_From_Repository()
        {
            //Setup and arrange       
            FinancialTransaction expectedFinancialTransaction = new FinancialTransaction
            {
                Id = 5,
                Date = DateTime.Now,
                Description = "Expected Financial Transaction",
                IsExpense = true,
                Value = 500,
                CategoryId = 1,
                Category = new Category
                {
                    Id = 1,
                    Name = "TestCategory",
                    Percentage = 5
                }
            };
            string expectedFinancialTransactionString = JsonSerializer.Serialize(expectedFinancialTransaction);
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetById(5)).Returns(expectedFinancialTransaction);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Act
            FinancialTransaction obtainedFinancialTransaction = financialTransactionsManager.GetFinancialTransactionById(5);
            string obtainedFinancialTransactionString = JsonSerializer.Serialize(obtainedFinancialTransaction);

            //Assert
            Assert.Equal(expectedFinancialTransactionString, obtainedFinancialTransactionString);

        }

        [Fact]
        public void GetFinancialReport_Orders_Transactions_By_Date()
        {
            //Setup and Arrange
            FinancialTransaction financialTransaction1 = new FinancialTransaction
            {
                Id = 5,
                Date = DateTime.Now,
                Description = "",
                IsExpense = true,
                Value = 500,
                CategoryId = 1,
                PeriodId = 1
            };
            FinancialTransaction financialTransaction2 = new FinancialTransaction
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-15),
                Description = "",
                IsExpense = false,
                Value = 1000,
                CategoryId = 1,
                PeriodId = 1
            };
            FinancialTransaction financialTransaction3 = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now.AddDays(20),
                Description = "",
                IsExpense = false,
                Value = 900,
                CategoryId = 1,
                PeriodId = 1
            };

            IEnumerable<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction> 
            {
                financialTransaction1,
                financialTransaction2,
                financialTransaction3        
            };
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<FinancialTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<FinancialTransaction>, IOrderedQueryable<FinancialTransaction>>>(),
            It.IsAny<string>())).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            IEnumerable<FinancialTransaction> expectedFinancialTransactions = mockFinancialTransactionsDatabase.OrderBy(t => t.Date);

            //Act
            FinancialReport obtainedFinancialReport = financialTransactionsManager.GetFinancialReport();
            IEnumerable<FinancialTransaction> obtainedFinancialTransactions = obtainedFinancialReport.FinancialTransactions;

            //Assert
            Assert.Equal(expectedFinancialTransactions, obtainedFinancialTransactions);
        }

        [Theory]
        [InlineData(200, false, 300, false, 100, true, 400)]
        [InlineData(200, true, 300, false, 100, true, 0)]
        [InlineData(100, true, 200, true, 300, true, -600)]
        [InlineData(500, false, 150, false, 1000, false, 1650)]
        public void GetFinancialReport_TotalValue_Is_SumOfIncomeTransactionValues_Minus_SumOfExpenseTransactionValues(int transactionValue1, bool isExpense1, int transactionValue2, bool isExpense2, int transactionValue3, bool isExpense3, int expectedTotalValue)
        {
            //Setup and Arrange
            FinancialTransaction financialTransaction1 = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "",
                IsExpense = isExpense1,
                Value = transactionValue1,
                CategoryId = 1,
                PeriodId = 1
            };
            FinancialTransaction financialTransaction2 = new FinancialTransaction
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-15),
                Description = "",
                IsExpense = isExpense2,
                Value = transactionValue2,
                CategoryId = 1,
                PeriodId = 1
            };
            FinancialTransaction financialTransaction3 = new FinancialTransaction
            {
                Id = 3,
                Date = DateTime.Now.AddDays(20),
                Description = "",
                IsExpense = isExpense3,
                Value = transactionValue3,
                CategoryId = 1,
                PeriodId = 1
            };

            IEnumerable<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>
            {
                financialTransaction1,
                financialTransaction2,
                financialTransaction3
            };
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<FinancialTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<FinancialTransaction>, IOrderedQueryable<FinancialTransaction>>>(),
            It.IsAny<string>())).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Act
            FinancialReport obtainedFinancialReport = financialTransactionsManager.GetFinancialReport();
            decimal obtainedTotalValue = obtainedFinancialReport.TotalValue;

            //Assert
            Assert.Equal(expectedTotalValue, obtainedTotalValue);
        }


        [Fact]
        public void GetFinancialReport_Throws_InvalidOperationException_When_EndDate_is_lower_than_StartDate()
        {
            //Setup and Arrange
            IEnumerable<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<FinancialTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<FinancialTransaction>, IOrderedQueryable<FinancialTransaction>>>(),
            It.IsAny<string>())).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            DateTime startDate = new DateTime(2021, 6, 10);
            DateTime endDate = new DateTime(2021, 6, 9);

            //Assert
            Assert.Throws<InvalidOperationException>(() => financialTransactionsManager.GetFinancialReport(null, null, null, startDate, endDate));

        }

        [Fact]
        public void GetFinancialReport_DoesNotThrow_InvalidOperationException_When_EndDate_is_higher_than_StartDate()
        {
            //Setup and Arrange
            IEnumerable<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<FinancialTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<FinancialTransaction>, IOrderedQueryable<FinancialTransaction>>>(),
            It.IsAny<string>())).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            DateTime endDate = new DateTime(2021, 6, 10);
            DateTime startDate = new DateTime(2021, 6, 9);

            //Assert
            financialTransactionsManager.GetFinancialReport(null, null, null, startDate, endDate);
        }

        public static readonly object[][] CorrectData =
        {
            new object[] { new DateTime(2021, 6, 10), null},
            new object[] { null, new DateTime(2021, 6, 9) },
            new object[] { null, null}
        };

        [Theory, MemberData(nameof(CorrectData))]
        public void GetFinancialReport_DoesNotThrow_InvalidOperationException_When_Either_Date_Is_Null(DateTime? startDate, DateTime? endDate)
        {
            //Setup and Arrange
            IEnumerable<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<FinancialTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<FinancialTransaction>, IOrderedQueryable<FinancialTransaction>>>(),
            It.IsAny<string>())).Returns(mockFinancialTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Assert
            financialTransactionsManager.GetFinancialReport(null, null, null, startDate, endDate);
        }






    }
}
