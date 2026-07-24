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
    public class InvestmentFundTransactionsManagerTests
    {

        [Fact]
        public void AddInvestmentFundTransaction_Adds_InvestmentFundTransactions_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>();
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.Add(It.IsAny<InvestmentFundTransaction>())).Callback((InvestmentFundTransaction investmentFundTransaction) => mockInvestmentFundTransactionsDatabase.Add(investmentFundTransaction));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            InvestmentFundTransaction expectedInvestmentFundTransaction = new InvestmentFundTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Transaction",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            string expectedInvestmentFundTransactionString = JsonSerializer.Serialize(expectedInvestmentFundTransaction);

            //Act
            investmentFundTransactionsManager.AddInvestmentFundTransaction(expectedInvestmentFundTransaction);
            InvestmentFundTransaction addedInvestmentFundTransaction = mockInvestmentFundTransactionsDatabase.Single();
            string addedInvestmentFundTransactionString = JsonSerializer.Serialize(addedInvestmentFundTransaction);

            //Assert
            Assert.Equal(expectedInvestmentFundTransactionString, addedInvestmentFundTransactionString);
        }

        [Fact]
        public void DeleteInvestmentFundTransactionById_Removes_InvestmentFundTransactions_Correctly_From_Repository()
        {

            //Setup
            List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>();
            InvestmentFundTransaction transactionToRemain = new InvestmentFundTransaction
            {   
                Id = 1,
                Description = "Transaction that should remain in the repository",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Withdrawal,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            InvestmentFundTransaction transactionToBeDeleted = new InvestmentFundTransaction
            {
                Id = 2,
                Description = "Transaction that should be removed from the repository",
                Amount = 2500,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            mockInvestmentFundTransactionsDatabase.Add(transactionToRemain);
            mockInvestmentFundTransactionsDatabase.Add(transactionToBeDeleted);
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.DeleteById(2)).Callback((int investmentFundTransactionId) =>
            {
                mockInvestmentFundTransactionsDatabase.Remove(transactionToBeDeleted);
            });
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Arrange


            IEnumerable<InvestmentFundTransaction> expectedInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>
            {
                transactionToRemain
            };


            //Act
            investmentFundTransactionsManager.DeleteInvestmentFundTransactionById(transactionToBeDeleted.Id);

            //Assert
            Assert.Equal(expectedInvestmentFundTransactionsDatabase, mockInvestmentFundTransactionsDatabase);

        }

        [Fact]
        public void GetAllInvestmentFundTransactions_Returns_All_Transactions_From_Repository()
        {
            //Setup
            List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>();
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<InvestmentFundTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<InvestmentFundTransaction>, IOrderedQueryable<InvestmentFundTransaction>>>(),
            It.IsAny<string>())).Returns(mockInvestmentFundTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);


            //Arrange
            mockInvestmentFundTransactionsDatabase.Add(new InvestmentFundTransaction
            {
                Id = 1,
                Description = "Test Transaction 1",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            });
            mockInvestmentFundTransactionsDatabase.Add(new InvestmentFundTransaction
            {
                Id = 2,
                Description = "Test Transaction 2",
                Amount = 2500,
                Type = InvestmentFundTransactionType.Withdrawal,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            });

            //Act
            IEnumerable<InvestmentFundTransaction> returnedInvestmentFundTransactions = investmentFundTransactionsManager.GetAllInvestmentFundTransactions();

            //Assert
            Assert.Equal(mockInvestmentFundTransactionsDatabase, returnedInvestmentFundTransactions);
        }



        [Fact]
        public void GetSumOfInvestmentFundTransactionValues_Returns_Sum_Of_All_Contributions_And_Dividends_Minus_All_Withdrawals()
        {

            //Setup
             List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase =
             [
                 new InvestmentFundTransaction
                 {
                     Id = 1,
                     Description = "Test Transaction 1",
                     Amount = 1000,
                     Type = InvestmentFundTransactionType.Contribution,
                     InvestmentFundId = 1,
                     InvestmentFundCategoryId = 1
                 },
                 new InvestmentFundTransaction
                 {
                     Id = 2,
                     Description = "Test Transaction 2",
                     Amount = 2500,
                     Type = InvestmentFundTransactionType.Withdrawal,
                     InvestmentFundId = 1,
                     InvestmentFundCategoryId = 1
                 },
                 new InvestmentFundTransaction
                 {
                     Id = 3,
                     Description = "Test Transaction 3",
                     Amount = 500,
                     Type = InvestmentFundTransactionType.Dividend,
                     InvestmentFundId = 1,
                     InvestmentFundCategoryId = 1
                 },
             ];
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<InvestmentFundTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<InvestmentFundTransaction>, IOrderedQueryable<InvestmentFundTransaction>>>(),
            It.IsAny<string>())).Returns(mockInvestmentFundTransactionsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Arrange
           
            decimal obtainedResult = investmentFundTransactionsManager.GetSumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId(1);
            decimal expectedResult = mockInvestmentFundTransactionsDatabase.Where(t => t.Type == InvestmentFundTransactionType.Contribution || t.Type == InvestmentFundTransactionType.Dividend).Sum(t => t.Amount) -
                mockInvestmentFundTransactionsDatabase .Where(t => t.Type == InvestmentFundTransactionType.Withdrawal).Sum(t => t.Amount);

            //Assert
            Assert.Equal(expectedResult, obtainedResult);

        }


        [Fact]
        public void UpdateInvestmentFundTransaction_Updates_InvestmentFundTransactions_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>();
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.Update(It.IsAny<InvestmentFundTransaction>())).Callback((InvestmentFundTransaction investmentFundTransaction) => mockInvestmentFundTransactionsDatabase[0] = investmentFundTransaction);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Arrange

            InvestmentFundTransaction originalInvestmentFundTransaction = new InvestmentFundTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Original Test Transaction",
                Amount = 500,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            mockInvestmentFundTransactionsDatabase.Add(originalInvestmentFundTransaction);

            InvestmentFundTransaction updatedInvestmentFundTransaction = new InvestmentFundTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Updated Test Transaction",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            string updatedInvestmentFundTransactionString = JsonSerializer.Serialize(updatedInvestmentFundTransaction);

            //Act
            investmentFundTransactionsManager.UpdateInvestmentFundTransaction(updatedInvestmentFundTransaction);
            InvestmentFundTransaction obtainedUpdatedInvestmentFundTransaction = mockInvestmentFundTransactionsDatabase.Single();

            string obtainedUpdatedInvestmentFundTransactionString = JsonSerializer.Serialize(obtainedUpdatedInvestmentFundTransaction);

            //Assert
            Assert.Equal(updatedInvestmentFundTransactionString, obtainedUpdatedInvestmentFundTransactionString);
        }


        [Fact]
        public void GetInvestmentFundTransactionById_Returns_Correct_InvestmentFundTransaction_From_Repository()
        {
            //Setup and arrange       
            InvestmentFundTransaction expectedInvestmentFundTransaction = new InvestmentFundTransaction
            {
                Id = 5,
                Date = DateTime.Now,
                Description = "Expected Investment Fund Transaction",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };
            string expectedInvestmentFundTransactionString = JsonSerializer.Serialize(expectedInvestmentFundTransaction);
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.GetById(5)).Returns(expectedInvestmentFundTransaction);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Act
            InvestmentFundTransaction obtainedInvestmentFundTransaction = investmentFundTransactionsManager.GetInvestmentFundTransactionById(5);
            string obtainedInvestmentFundTransactionString = JsonSerializer.Serialize(obtainedInvestmentFundTransaction);

            //Assert
            Assert.Equal(expectedInvestmentFundTransactionString, obtainedInvestmentFundTransactionString);

        }

        [Fact]
        public void AddInvestmentFundTransaction_Adds_SimplifiedContributionInvestmentFundTransactions_When_Dividend_Transaction_Has_Null_InvestmentFundCategoryId()
        {
            //Setup
                //Categories are required to be able to add simplified contribution transactions when a dividend transaction has a null InvestmentFundCategoryId
            List<InvestmentFundCategory> mockInvestmentFundCategoriesDatabase = new List<InvestmentFundCategory>
            {
                new InvestmentFundCategory
                {
                    Id = 1,
                    Name = "Category 1",
                    InvestmentFundId = 1
                },
                new InvestmentFundCategory
                {
                    Id = 2,
                    Name = "Category 2",
                    InvestmentFundId = 1
                }
            };
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<InvestmentFundCategory, bool>>>(),
            It.IsAny<Func<IQueryable<InvestmentFundCategory>, IOrderedQueryable<InvestmentFundCategory>>>(),
            It.IsAny<string>())).Returns(mockInvestmentFundCategoriesDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);


                //Previous Transactions are required to be able to test adding simplified contribution transactions when a dividend transaction has a null InvestmentFundCategoryId

            InvestmentFundTransaction previousTransaction1 = new InvestmentFundTransaction
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-10),
                Description = "Previous Transaction 1",
                Amount = 3000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            InvestmentFundTransaction previousTransaction2 = new InvestmentFundTransaction
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-5),
                Description = "Previous Transaction 2",
                Amount = 7000,
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 2
            };
            List<InvestmentFundTransaction> mockInvestmentFundTransactionsDatabase = new List<InvestmentFundTransaction>
            {
                previousTransaction1,
                previousTransaction2
            };
            Mock<IRepository<InvestmentFundTransaction>> mockInvestmentFundTransactionsRepository = new Mock<IRepository<InvestmentFundTransaction>>();
            mockInvestmentFundTransactionsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<InvestmentFundTransaction, bool>>>(),
            It.IsAny<Func<IQueryable<InvestmentFundTransaction>, IOrderedQueryable<InvestmentFundTransaction>>>(),
            It.IsAny<string>())).Returns(mockInvestmentFundTransactionsDatabase);
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundTransaction>()).Returns(mockInvestmentFundTransactionsRepository.Object);
            InvestmentFundTransactionsManager investmentFundTransactionsManager = new InvestmentFundTransactionsManager(mockUnitOfWork.Object);

            //Arrange
            InvestmentFundTransaction dividendTransactionWithNullCategoryId = new InvestmentFundTransaction
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Dividend Transaction with Null Category Id",
                Amount = 1000,
                Type = InvestmentFundTransactionType.Dividend,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = null
            };

            InvestmentFundTransaction expectedSimplifiedContributionTransaction1 = new InvestmentFundTransaction
            {
                Id = 3,
                Date = dividendTransactionWithNullCategoryId.Date,
                Description = dividendTransactionWithNullCategoryId.Description,
                Amount = 300, // 30% of the dividend amount
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 1
            };

            InvestmentFundTransaction expectedSimplifiedContributionTransaction2 = new InvestmentFundTransaction
            {
                Id = 4,
                Date = dividendTransactionWithNullCategoryId.Date,
                Description = dividendTransactionWithNullCategoryId.Description,
                Amount = 700, // 70% of the dividend amount
                Type = InvestmentFundTransactionType.Contribution,
                InvestmentFundId = 1,
                InvestmentFundCategoryId = 2
            };

            //Act
            investmentFundTransactionsManager.AddInvestmentFundTransaction(dividendTransactionWithNullCategoryId);

            //Assert
            mockInvestmentFundTransactionsRepository.Verify(repository => repository.Add(It.Is<InvestmentFundTransaction>(t => t.Description == expectedSimplifiedContributionTransaction1.Description && t.Amount == expectedSimplifiedContributionTransaction1.Amount && t.InvestmentFundCategoryId == expectedSimplifiedContributionTransaction1.InvestmentFundCategoryId)), Times.Once);
            mockInvestmentFundTransactionsRepository.Verify(repository => repository.Add(It.Is<InvestmentFundTransaction>(t => t.Description == expectedSimplifiedContributionTransaction2.Description && t.Amount == expectedSimplifiedContributionTransaction2.Amount && t.InvestmentFundCategoryId == expectedSimplifiedContributionTransaction2.InvestmentFundCategoryId)), Times.Once);
        }




    }
}
