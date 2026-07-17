using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace FinanceManagement.Tests
{
    public class InvestmentFundsManagerTests
    {

        [Fact]
        public void AddInvestmentFund_Adds_InvestmentFunds_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFund> mockInvestmentFundsDatabase = new List<InvestmentFund>();
            Mock<IRepository<InvestmentFund>> mockInvestmentFundsRepository = new Mock<IRepository<InvestmentFund>>();
            mockInvestmentFundsRepository.Setup(repository => repository.Add(It.IsAny<InvestmentFund>())).Callback((InvestmentFund investmentFund) => mockInvestmentFundsDatabase.Add(investmentFund));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFund>()).Returns(mockInvestmentFundsRepository.Object);
            InvestmentFundsManager investmentFundsManager = new InvestmentFundsManager(mockUnitOfWork.Object);

            //Arrange
            InvestmentFund expectedInvestmentFund = new InvestmentFund
            {
                Id = 1,
                Name = "TestInvestmentFund",
                Description = "TestDescription",
                Deleted = false,
                Company = "TestCompany",
                Currency = "USD"
            };
            string expectedInvestmentFundString = JsonSerializer.Serialize(expectedInvestmentFund);

            //Act
            investmentFundsManager.AddInvestmentFund(expectedInvestmentFund);
            InvestmentFund addedInvestmentFund = mockInvestmentFundsDatabase.Single();
            string addedInvestmentFundString = JsonSerializer.Serialize(addedInvestmentFund);

            //Assert
            Assert.Equal(expectedInvestmentFundString, addedInvestmentFundString);
        }


        public static IEnumerable<object[]> Data()
        {

            bool? deleted = null;

            yield return new object[] { false, GenerateActiveInvestmentFundsRepository() };
            yield return new object[] { true, GenerateDeletedInvestmentFundsRepository() };
            yield return new object[] { deleted, GenerateInvestmentFundsRepository() };

        }

        [Theory]
        [MemberData(nameof(Data))]

        public void GetAllInvestmentFunds_Returns_All_InvestmentFunds_From_Repository_With_Correct_filters(bool? deleted, IEnumerable<InvestmentFund> expectedInvestmentFundsDatabase)
        {
            //Setup and arrange
            IEnumerable<InvestmentFund> mockInvestmentFundsDatabase = expectedInvestmentFundsDatabase;
            Mock<IRepository<InvestmentFund>> mockInvestmentFundsRepository = new Mock<IRepository<InvestmentFund>>();
            mockInvestmentFundsRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockInvestmentFundsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFund>()).Returns(mockInvestmentFundsRepository.Object);
            InvestmentFundsManager investmentFundsManager = new InvestmentFundsManager(mockUnitOfWork.Object);

            //Act
            IEnumerable<InvestmentFund> returnedInvestmentFunds = investmentFundsManager.GetAllInvestmentFunds();

            //Assert
            Assert.Equal(mockInvestmentFundsDatabase, returnedInvestmentFunds);
        }


        [Fact]
        public void GetInvestmentFundById_Returns_Correct_InvestmentFund_From_Repository()
        {
            //Setup and arrange       
            InvestmentFund expectedInvestmentFund = new InvestmentFund
            {
                Id = 5,
                Name = "Expected Investment Fund",
                Description = "Expected Description",
                Deleted = false,
                Company = "Expected Company",
                Currency = "USD"
            };
            string expectedInvestmentFundString = JsonSerializer.Serialize(expectedInvestmentFund);
            Mock<IRepository<InvestmentFund>> mockInvestmentFundsRepository = new Mock<IRepository<InvestmentFund>>();
            mockInvestmentFundsRepository.Setup(repository => repository.GetById(5)).Returns(expectedInvestmentFund);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFund>()).Returns(mockInvestmentFundsRepository.Object);
            InvestmentFundsManager investmentFundsManager = new InvestmentFundsManager(mockUnitOfWork.Object);

            //Act
            InvestmentFund obtainedInvestmentFund = investmentFundsManager.GetInvestmentFundById(5);
            string obtainedInvestmentFundString = JsonSerializer.Serialize(obtainedInvestmentFund);

            //Assert
            Assert.Equal(expectedInvestmentFundString, obtainedInvestmentFundString);

        }


        [Fact]
        public void UpdateInvestmentFund_Updates_InvestmentFunds_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFund> mockInvestmentFundsDatabase = new List<InvestmentFund>();
            Mock<IRepository<InvestmentFund>> mockInvestmentFundsRepository = new Mock<IRepository<InvestmentFund>>();
            mockInvestmentFundsRepository.Setup(repository => repository.Update(It.IsAny<InvestmentFund>())).Callback((InvestmentFund investmentFund) => mockInvestmentFundsDatabase[0] = investmentFund);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFund>()).Returns(mockInvestmentFundsRepository.Object);
            InvestmentFundsManager investmentFundsManager = new InvestmentFundsManager(mockUnitOfWork.Object);

            //Arrange

            InvestmentFund originalInvestmentFund = new InvestmentFund
            {
                Id = 1,
                Name = "Original Investment Fund",
                Description = "Original Description",
                Deleted = false,
                Company = "Original Company",
                Currency = "USD"
            };

            mockInvestmentFundsDatabase.Add(originalInvestmentFund);

            InvestmentFund updatedInvestmentFund = new InvestmentFund
            {
                Id = 1,
                Name = "Updated Investment Fund",
                Description = "Updated Description",
                Deleted = false,
                Company = "Updated Company",
                Currency = "EUR"
            };

            string updatedInvestmentFundString = JsonSerializer.Serialize(updatedInvestmentFund);

            //Act
            investmentFundsManager.UpdateInvestmentFund(updatedInvestmentFund);
            InvestmentFund obtainedUpdatedInvestmentFund = mockInvestmentFundsDatabase.Single();

            string obtainedUpdatedInvestmentFundString = JsonSerializer.Serialize(obtainedUpdatedInvestmentFund);

            //Assert
            Assert.Equal(updatedInvestmentFundString, obtainedUpdatedInvestmentFundString);
        }

        private static IEnumerable<InvestmentFund> GenerateActiveInvestmentFundsRepository()
        {
            List<InvestmentFund> activeInvestmentFundsRepository = new List<InvestmentFund>
            {
                new InvestmentFund
                {
                    Id = 1,
                    Name = "TestCategory1",
                    Description = "TestDescription1",
                    Company = "TestCompany1",
                    Currency = "USD",
                    Deleted = false
                },
                new InvestmentFund
                {
                    Id = 2,
                    Name = "TestCategory2",
                    Description = "TestDescription2",
                    Company = "TestCompany2",
                    Currency = "EUR",
                    Deleted = false
                }
            };

            return activeInvestmentFundsRepository;

        }

        private static IEnumerable<InvestmentFund> GenerateDeletedInvestmentFundsRepository()
        {
            List<InvestmentFund> deletedInvestmentFundsRepository = new List<InvestmentFund>
            {
                new InvestmentFund
                {
                    Id = 3,
                    Name = "Deleted TestCategory1",
                    Description = "Deleted TestDescription1",
                    Company = "Deleted TestCompany1",
                    Currency = "GBP",
                    Deleted = true
                },
                new InvestmentFund
                {
                    Id = 4,
                    Name = "Deleted TestCategory2",
                    Description = "Deleted TestDescription2",
                    Company = "Deleted TestCompany2",
                    Currency = "JPY",
                    Deleted = true
                }
            };

            return deletedInvestmentFundsRepository;
        }

        private static IEnumerable<InvestmentFund> GenerateInvestmentFundsRepository()
        {
            List<InvestmentFund> investmentFundsRepository =
            [
                .. GenerateActiveInvestmentFundsRepository(),
                .. GenerateDeletedInvestmentFundsRepository(),
            ];
            return investmentFundsRepository;

        }


        [Fact]
        public void DeleteInvestmentFundById_SoftDeletes_InvestmentFunds_Correctly_From_Repository()
        {

            //Setup
            InvestmentFund investmentFundToDelete = new InvestmentFund
            {
                Id = 1,
                Name = "Investment Fund to delete",
                Description = "Description to delete",
                Company = "Company to delete",
                Currency = "USD",
                Deleted = false
            };
            List<InvestmentFund> mockInvestmentFundsDatabase = new List<InvestmentFund>();
            Mock<IRepository<InvestmentFund>> mockInvestmentFundsRepository = new Mock<IRepository<InvestmentFund>>();
            mockInvestmentFundsRepository.Setup(repository => repository.Update(It.IsAny<InvestmentFund>())).Callback((InvestmentFund investmentFund) => mockInvestmentFundsDatabase[0] = investmentFund);
            mockInvestmentFundsRepository.Setup(repository => repository.GetById(1)).Returns(investmentFundToDelete);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFund>()).Returns(mockInvestmentFundsRepository.Object);
            InvestmentFundsManager investmentFundsManager = new InvestmentFundsManager(mockUnitOfWork.Object);


            //Arrange
            mockInvestmentFundsDatabase.Add(investmentFundToDelete);

            bool expectedDeletedValue = true;

            //Act
            investmentFundsManager.DeleteInvestmentFundById(investmentFundToDelete.Id);
            InvestmentFund obtainedDeletedInvestmentFund = mockInvestmentFundsDatabase.Single();

            Assert.Equal(expectedDeletedValue, obtainedDeletedInvestmentFund.Deleted);
        }

    }
}