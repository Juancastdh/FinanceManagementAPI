using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
                IsExpense = true
            };

            FinancialTransaction transactionToBeDeleted = new FinancialTransaction
            {
                Id = 2,
                Description = "Transaction that should be removed from the repository",
                IsExpense = false
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
            Assert.Equal(orderedMockFinancialTransactionsDatabase, expectedFinancialTransactionsDatabase);

        }

    }
}
