using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
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
        public void DeleteFinancialTransaction_Removes_FinancialTransactions_Correctly_From_Repository()
        {

            //Setup
            List<FinancialTransaction> mockFinancialTransactionsDatabase = new List<FinancialTransaction>();
            Mock<IRepository<FinancialTransaction>> mockFinancialTransactionsRepository = new Mock<IRepository<FinancialTransaction>>();
            mockFinancialTransactionsRepository.Setup(repository => repository.DeleteById(It.IsAny<int>())).Callback((int financialTransactionId) => { 
                FinancialTransaction financialTransactionToDelete = mockFinancialTransactionsDatabase.Find(finanacialTransaction => finanacialTransaction.Id == financialTransactionId); 
                mockFinancialTransactionsDatabase.Remove(financialTransactionToDelete); 
            });
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<FinancialTransaction>()).Returns(mockFinancialTransactionsRepository.Object);
            FinancialTransactionsManager financialTransactionsManager = new FinancialTransactionsManager(mockUnitOfWork.Object);

            //Arrange
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

            IEnumerable<FinancialTransaction> orderedMockFinancialTransactionsDatabase = mockFinancialTransactionsDatabase.OrderBy(x => x.Id);

            IEnumerable<FinancialTransaction> expectedFinancialTransactionsDatabase = new List<FinancialTransaction>
            {
                transactionToRemain
            };

            expectedFinancialTransactionsDatabase = expectedFinancialTransactionsDatabase.OrderBy(x => x.Id);

            //Act
            financialTransactionsManager.DeleteFinancialTransaction(transactionToBeDeleted);

            //Assert
            Assert.Equal(expectedFinancialTransactionsDatabase, orderedMockFinancialTransactionsDatabase);

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
        public void UpdateFinancialTransaction_Update_FinancialTransactions_Correctly_To_Repository()
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


    }
}
