using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
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
                    Id = 1,
                    Name = "TestCategory",
                    Percentage = 5
                },
                AccountId = 1,
                Account = new Account
                {
                    Id = 1,
                    Identifier = "123456789",
                    Description = "Test Account"
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
            mockFinancialTransactionsRepository.Setup(repository => repository.DeleteById(2)).Callback((int financialTransactionId) =>
            {
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
                },
                AccountId = 1,
                Account = new Account
                {
                    Id = 1,
                    Identifier = "123456789",
                    Description = "Test Account"
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
                },
                AccountId = 2,
                Account = new Account
                {
                    Id = 2,
                    Identifier = "1011121314",
                    Description = "Updated Test Account"
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
                PeriodId = 1,
                AccountId = 1
            };
            FinancialTransaction financialTransaction2 = new FinancialTransaction
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-15),
                Description = "",
                IsExpense = false,
                Value = 1000,
                CategoryId = 1,
                PeriodId = 1,
                AccountId = 1
            };
            FinancialTransaction financialTransaction3 = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now.AddDays(20),
                Description = "",
                IsExpense = false,
                Value = 900,
                CategoryId = 1,
                PeriodId = 1,
                AccountId = 1
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

        [Fact]
        public void AddFinancialTransactions_Adds_FinancialTransactions_Correctly_To_Repository()
        {
            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.Add(It.IsAny<FinancialTransaction>())).Callback((FinancialTransaction financialTransaction) => mockFinancialTransactionsDatabase.Add(financialTransaction));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            IEnumerable<FinancialTransaction> expectedFinancialTransactions = new List<FinancialTransaction>{
                new FinancialTransaction
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Description = "Test Transaction",
                    IsExpense = true,
                    Value = 1000,
                    CategoryId = 1,
                    Category = new Category
                    {
                        Id = 1,
                        Name = "TestCategory",
                        Percentage = 5
                    },
                    AccountId = 1,
                    Account = new Account{
                        Id = 1,
                        Identifier = "123456789",
                        Description = "Test Account 1"
                    }
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Description = "Test Transaction 2",
                    IsExpense = true,
                    Value = 1500,
                    CategoryId = 2,
                    Category = new Category
                    {
                        Id = 2,
                        Name = "TestCategory 2",
                        Percentage = 10
                    },
                    AccountId = 2,
                    Account = new Account{
                        Id = 2,
                        Identifier = "1011121314",
                        Description = "Test Account 2"
                    }
                },
                new FinancialTransaction
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Description = "Test Transaction 3",
                    IsExpense = true,
                    Value = 7200,
                    CategoryId = 3,
                    Category = new Category
                    {
                        Id = 3,
                        Name = "TestCategory 3",
                        Percentage = 21
                    },
                    AccountId = 3,
                    Account = new Account{
                        Id = 3,
                        Identifier = "1516171819",
                        Description = "Test Account 3"
                    }
                }
            };
            string expectedFinancialTransactionsString = JsonSerializer.Serialize(expectedFinancialTransactions);

            //Act
            financialTransactionsManager.AddFinancialTransactions(expectedFinancialTransactions);
            IEnumerable<FinancialTransaction> addedFinancialTransactions = mockFinancialTransactionsDatabase;
            string addedFinancialTransactionsString = JsonSerializer.Serialize(addedFinancialTransactions);

            //Assert
            Assert.Equal(expectedFinancialTransactionsString, addedFinancialTransactionsString);
        }

        [Theory]
        [InlineData(-100, false)]
        [InlineData(0, false)]
        [InlineData(100, true)]
        public void GetFixedFinancialTransaction_Corrects_IsExpense_Correctly(int value, bool expectedIsExpense)
        {
            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.Add(It.IsAny<FinancialTransaction>())).Callback((FinancialTransaction financialTransaction) => mockFinancialTransactionsDatabase.Add(financialTransaction));
            List<Period> mockPeriodsDatabase = new List<Period>();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.Add(It.IsAny<Period>())).Callback((Period period) => mockPeriodsDatabase.Add(period));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);


            //Arrange
            FinancialTransaction financialTransactionToFix = new FinancialTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Transaction",
                Value = value,
                CategoryId = 1
            };


            //Act
            FinancialTransaction fixedFinancialTransaction = financialTransactionsManager.GetFixedFinancialTransaction(financialTransactionToFix);
            bool fixedIsExpense = fixedFinancialTransaction.IsExpense;

            //Assert
            Assert.Equal(expectedIsExpense, fixedIsExpense);
        }

        public static readonly object[][] CorrectPeriodData =
{
            new object[] { new DateTime(2024, 1, 18), 1},
            new object[] { new DateTime(2024, 1, 29), 2},
            new object[] { new DateTime(2024, 2, 12), 2},
            new object[] { new DateTime(2024, 2, 20), 3},
            new object[] { new DateTime(2024, 2, 27), 3}
        };

        [Theory, MemberData(nameof(CorrectPeriodData))]
        public void GetFixedFinancialTransactions_Corrects_Period_Correctly(DateTime financialTransactionDate, int expectedPeriodId)
        {
            //Setup
            Period period1 = new Period
            {
                Id = 1,
                StartDate = new DateTime(2024, 1, 13),
                EndDate = new DateTime(2024, 1, 28)
            };
            Period period2 = new Period
            {
                Id = 2,
                StartDate = new DateTime(2024, 1, 28),
                EndDate = new DateTime(2024, 2, 13)
            };
            Period period3 = new Period
            {
                Id = 3,
                StartDate = new DateTime(2024, 2, 13),
                EndDate = new DateTime(2024, 2, 28)
            };

            IEnumerable<Period> mockPeriodsDatabase = new List<Period>
            {
                period1,
                period2,
                period3
            };
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.Add(It.IsAny<FinancialTransaction>())).Callback((FinancialTransaction financialTransaction) => mockFinancialTransactionsDatabase.Add(financialTransaction));
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<Period, bool>>>(),
            It.IsAny<Func<IQueryable<Period>, IOrderedQueryable<Period>>>(),
            It.IsAny<string>())).Returns(mockPeriodsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            FinancialTransaction financialTransactionToFix = new FinancialTransaction
            {
                Id = 1,
                Date = financialTransactionDate,
                Description = "Test Transaction",
                Value = 100,
                CategoryId = 1
            };

            //Act
            FinancialTransaction fixedFinancialTransaction = financialTransactionsManager.GetFixedFinancialTransaction(financialTransactionToFix);
            int periodId = fixedFinancialTransaction.PeriodId;

            //Assert
            Assert.Equal(expectedPeriodId, periodId);

        }






    }
}
