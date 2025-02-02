using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Managers.Implementations;
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
    public class AccountsManagerTests
    {

        [Fact]
        public void AddAccount_Adds_Accounts_Correctly_To_Repository()
        {
            //Setup
            List<Account> mockAccountsDatabase = new List<Account>();
            Mock<IRepository<Account>> mockAccountsRepository = new Mock<IRepository<Account>>();
            mockAccountsRepository.Setup(repository => repository.Add(It.IsAny<Account>())).Callback((Account account) => mockAccountsDatabase.Add(account));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Account>()).Returns(mockAccountsRepository.Object);
            AccountsManager accountsManager = new AccountsManager(mockUnitOfWork.Object);

            //Arrange
            Account expectedAccount = new Account
            {
                Id = 1,
                Identifier = "123456789",
                Description = "Test"
            };
            string expectedAccountString = JsonSerializer.Serialize(expectedAccount);

            //Act
            accountsManager.AddAccount(expectedAccount);
            Account addedAccount = mockAccountsDatabase.Single();
            string addedAccountString = JsonSerializer.Serialize(addedAccount);

            //Assert
            Assert.Equal(expectedAccountString, addedAccountString);
        }


        [Fact]
        public void GetAllAccounts_Returns_All_Accounts_From_Repository()
        {
            //Setup and arrange
            IEnumerable<Account> mockAccountsDatabase = GenerateAccountsRepository();
            Mock<IRepository<Account>> mockAccountsRepository = new Mock<IRepository<Account>>();
            mockAccountsRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockAccountsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Account>()).Returns(mockAccountsRepository.Object);
            AccountsManager accountsManager = new AccountsManager(mockUnitOfWork.Object);

            //Act
            IEnumerable<Account> returnedAccounts = accountsManager.GetAllAccounts();

            //Assert
            Assert.Equal(mockAccountsDatabase, returnedAccounts);
        }


        [Fact]
        public void GetAccountById_Returns_Correct_Account_From_Repository()
        {
            //Setup and arrange       
            Account expectedAccount = new Account
            {
                Id = 5,
                Identifier = "123456789",
                Description = "Test account"
            };
            string expectedAccountString = JsonSerializer.Serialize(expectedAccount);
            Mock<IRepository<Account>> mockAccountsRepository = new Mock<IRepository<Account>>();
            mockAccountsRepository.Setup(repository => repository.GetById(5)).Returns(expectedAccount);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Account>()).Returns(mockAccountsRepository.Object);
            AccountsManager accountsManager = new AccountsManager(mockUnitOfWork.Object);

            //Act
            Account obtainedAccount = accountsManager.GetAccountById(5);
            string obtainedAccountString = JsonSerializer.Serialize(obtainedAccount);

            //Assert
            Assert.Equal(expectedAccountString, obtainedAccountString);

        }


        [Fact]
        public void UpdateAccount_Updates_Accounts_Correctly_To_Repository()
        {
            //Setup
            List<Account> mockAccountsDatabase = new List<Account>();
            Mock<IRepository<Account>> mockAccountsRepository = new Mock<IRepository<Account>>();
            mockAccountsRepository.Setup(repository => repository.Update(It.IsAny<Account>())).Callback((Account account) => mockAccountsDatabase[0] = account);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Account>()).Returns(mockAccountsRepository.Object);
            AccountsManager accountsManager = new AccountsManager(mockUnitOfWork.Object);

            //Arrange

            Account originalAccount = new Account
            {
                Id = 1,
                Identifier = "123456789",
                Description = "Original account"
            };

            mockAccountsDatabase.Add(originalAccount);

            Account updatedAccount = new Account
            {
                Id = 1,
                Identifier = "123456789",
                Description = "Updated account"
            };

            string updatedAccountString = JsonSerializer.Serialize(updatedAccount);

            //Act
            accountsManager.UpdateAccount(updatedAccount);
            Account obtainedUpdatedAccount = mockAccountsDatabase.Single();

            string obtainedUpdatedAccountString = JsonSerializer.Serialize(obtainedUpdatedAccount);

            //Assert
            Assert.Equal(updatedAccountString, obtainedUpdatedAccountString);
        }

        private IEnumerable<Account> GenerateAccountsRepository()
        {
            List<Account> accountsRepository = new List<Account>
            {
                new Account
                {
                    Id = 1,
                    Identifier = "123456789",
                    Description  = "Test Account 1"
                },
                new Account
                {
                    Id = 2,
                    Identifier = "101112131415",
                    Description = "Test Account 2"
                }
            };

            return accountsRepository;

        }


        [Fact]
        public void DeleteAccountById_SoftDeletes_Accounts_Correctly_From_Repository()
        {

            //Setup
            Account accountToDelete = new Account
            {
                Id = 1,
                Identifier = "123456789",
                Description = "Account to be soft deleted"

            };
            List<Account> mockAccountsDatabase = new List<Account>();
            Mock<IRepository<Account>> mockAccountsRepository = new Mock<IRepository<Account>>();
            mockAccountsRepository.Setup(repository => repository.Update(It.IsAny<Account>())).Callback((Account account) => mockAccountsDatabase[0] = account);
            mockAccountsRepository.Setup(repository => repository.GetById(1)).Returns(accountToDelete);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Account>()).Returns(mockAccountsRepository.Object);
            AccountsManager accountsManager = new AccountsManager(mockUnitOfWork.Object);


            //Arrange
            mockAccountsDatabase.Add(accountToDelete);

            bool expectedDeletedValue = true;

            //Act
            accountsManager.DeleteAccountById(accountToDelete.Id);
            Account obtainedDeletedAccount = mockAccountsDatabase.Single();

            Assert.Equal(expectedDeletedValue, obtainedDeletedAccount.Deleted);

        }

    }
}