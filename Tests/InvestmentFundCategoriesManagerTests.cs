using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace FinanceManagement.Tests
{
    public class InvestmentFundCategoriesManagerTests
    {

        [Fact]
        public void AddInvestmentFundCategory_Adds_InvestmentFundCategory_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFundCategory> mockInvestmentFundCategoriesDatabase = new List<InvestmentFundCategory>();
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.Add(It.IsAny<InvestmentFundCategory>())).Callback((InvestmentFundCategory investmentFundCategory) => mockInvestmentFundCategoriesDatabase.Add(investmentFundCategory));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);
            InvestmentFundCategoriesManager investmentFundCategoriesManager = new InvestmentFundCategoriesManager(mockUnitOfWork.Object);

            //Arrange
            InvestmentFundCategory expectedInvestmentFundCategory = new InvestmentFundCategory
            {
                Id = 1,
                Name = "TestInvestmentFundCategory",
                InvestmentFundId = 1,
                InvestmentFund = new InvestmentFund
                {
                    Id = 1,
                    Name = "TestInvestmentFund",
                    Description = "TestInvestmentFundDescription",
                    Deleted = false
                },
                Deleted = false
            };
            string expectedInvestmentFundCategoryString = JsonSerializer.Serialize(expectedInvestmentFundCategory);

            //Act
            investmentFundCategoriesManager.AddInvestmentFundCategory(expectedInvestmentFundCategory);
            InvestmentFundCategory addedInvestmentFundCategory = mockInvestmentFundCategoriesDatabase.Single();
            string addedInvestmentFundCategoryString = JsonSerializer.Serialize(addedInvestmentFundCategory);

            //Assert
            Assert.Equal(expectedInvestmentFundCategoryString, addedInvestmentFundCategoryString);
        }


        public static IEnumerable<object[]> Data()
        {

            bool? deleted = null;

            yield return new object[] { false, GenerateActiveInvestmentFundCategoriesRepository() };
            yield return new object[] { true, GenerateDeletedInvestmentFundCategoriesRepository() };
            yield return new object[] { deleted, GenerateInvestmentFundCategoriesRepository() };

        }

        [Theory]
        [MemberData(nameof(Data))]

        public void GetAllInvestmentFundCategories_Returns_All_Categories_From_Repository_With_Correct_filters(bool? deleted, IEnumerable<InvestmentFundCategory> expectedInvestmentFundCategoriesDatabase)
        {
            //Setup and arrange
            IEnumerable<InvestmentFundCategory> mockInvestmentFundCategoriesDatabase = expectedInvestmentFundCategoriesDatabase;
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.GetAll(null, null, It.IsAny<string>())).Returns(mockInvestmentFundCategoriesDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);
            InvestmentFundCategoriesManager investmentFundCategoriesManager = new InvestmentFundCategoriesManager(mockUnitOfWork.Object);

            //Act
            IEnumerable<InvestmentFundCategory> returnedInvestmentFundCategories = investmentFundCategoriesManager.GetAllInvestmentFundCategories();

            //Assert
            Assert.Equal(mockInvestmentFundCategoriesDatabase, returnedInvestmentFundCategories);
        }


        [Fact]
        public void GetInvestmentFundCategoryById_Returns_Correct_Category_From_Repository()
        {
            //Setup and arrange       
            InvestmentFundCategory expectedInvestmentFundCategory = new InvestmentFundCategory
            {
                Id = 5,
                Name = "Expected Category",
                InvestmentFundId = 1
            };
            string expectedInvestmentFundCategoryString = JsonSerializer.Serialize(expectedInvestmentFundCategory);
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.GetById(5)).Returns(expectedInvestmentFundCategory);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);
            InvestmentFundCategoriesManager investmentFundCategoriesManager = new InvestmentFundCategoriesManager(mockUnitOfWork.Object);

            //Act
            InvestmentFundCategory obtainedInvestmentFundCategory = investmentFundCategoriesManager.GetInvestmentFundCategoryById(5);
            string obtainedInvestmentFundCategoryString = JsonSerializer.Serialize(obtainedInvestmentFundCategory);

            //Assert
            Assert.Equal(expectedInvestmentFundCategoryString, obtainedInvestmentFundCategoryString);

        }


        [Fact]
        public void UpdateInvestmentFundCategory_Updates_Categories_Correctly_To_Repository()
        {
            //Setup
            List<InvestmentFundCategory> mockInvestmentFundCategoriesDatabase = new List<InvestmentFundCategory>();
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.Update(It.IsAny<InvestmentFundCategory>())).Callback((InvestmentFundCategory category) => mockInvestmentFundCategoriesDatabase[0] = category);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);
            InvestmentFundCategoriesManager investmentFundCategoriesManager = new InvestmentFundCategoriesManager(mockUnitOfWork.Object);

            //Arrange

            InvestmentFundCategory originalInvestmentFundCategory = new InvestmentFundCategory
            {
                Id = 1,
                Name = "Original category",
                InvestmentFundId = 1,
                InvestmentFund = new InvestmentFund
                {
                    Id = 1,
                    Name = "TestInvestmentFund",
                    Description = "TestInvestmentFundDescription",
                    Deleted = false
                },
                Deleted = false
            };

            mockInvestmentFundCategoriesDatabase.Add(originalInvestmentFundCategory);

            InvestmentFundCategory updatedInvestmentFundCategory = new InvestmentFundCategory
            {
                Id = 1,
                Name = "Updated category",
                InvestmentFundId = 1,
                InvestmentFund = new InvestmentFund
                {
                    Id = 1,
                    Name = "TestInvestmentFund",
                    Description = "TestInvestmentFundDescription",
                    Deleted = false
                },
                Deleted = false
            };

            string updatedInvestmentFundCategoryString = JsonSerializer.Serialize(updatedInvestmentFundCategory);

            //Act
            investmentFundCategoriesManager.UpdateInvestmentFundCategory(updatedInvestmentFundCategory);
            InvestmentFundCategory obtainedUpdatedInvestmentFundCategory = mockInvestmentFundCategoriesDatabase.Single();

            string obtainedUpdatedInvestmentFundCategoryString = JsonSerializer.Serialize(obtainedUpdatedInvestmentFundCategory);

            //Assert
            Assert.Equal(updatedInvestmentFundCategoryString, obtainedUpdatedInvestmentFundCategoryString);
        }

        private static IEnumerable<InvestmentFundCategory> GenerateActiveInvestmentFundCategoriesRepository()
        {
            List<InvestmentFundCategory> activeInvestmentFundCategoriesRepository = new List<InvestmentFundCategory>
            {
                new InvestmentFundCategory
                {
                    Id = 1,
                    Name = "TestInvestmentFundCategory1",
                    InvestmentFundId = 1,
                    InvestmentFund = new InvestmentFund
                    {
                        Id = 1,
                        Name = "TestInvestmentFund",
                        Description = "TestInvestmentFundDescription",
                        Deleted = false
                    },
                    Deleted = false
                },
                new InvestmentFundCategory
                {
                    Id = 2,
                    Name = "TestInvestmentFundCategory2",
                    InvestmentFundId = 1,
                    InvestmentFund = new InvestmentFund
                    {
                        Id = 1,
                        Name = "TestInvestmentFund",
                        Description = "TestInvestmentFundDescription",
                        Deleted = false
                    },
                    Deleted = false
                }
            };

            return activeInvestmentFundCategoriesRepository;

        }

        private static IEnumerable<InvestmentFundCategory> GenerateDeletedInvestmentFundCategoriesRepository()
        {
            List<InvestmentFundCategory> deletedInvestmentFundCategoriesRepository = new List<InvestmentFundCategory>
            {
                new InvestmentFundCategory
                {
                    Id = 3,
                    Name = "Deleted TestInvestmentFundCategory1",
                    InvestmentFundId = 1,
                    InvestmentFund = new InvestmentFund
                    {
                        Id = 1,
                        Name = "TestInvestmentFund",
                        Description = "TestInvestmentFundDescription",
                        Deleted = false
                    },
                    Deleted = true
                },
                new InvestmentFundCategory
                {
                    Id = 4,
                    Name = "Deleted TestInvestmentFundCategory2",
                    InvestmentFundId = 1,
                    InvestmentFund = new InvestmentFund
                    {
                        Id = 1,
                        Name = "TestInvestmentFund",
                        Description = "TestInvestmentFundDescription",
                        Deleted = false
                    },
                    Deleted = true
                }
            };

            return deletedInvestmentFundCategoriesRepository;
        }

        private static IEnumerable<InvestmentFundCategory> GenerateInvestmentFundCategoriesRepository()
        {
            List<InvestmentFundCategory> investmentFundCategoriesRepository =
            [
                .. GenerateActiveInvestmentFundCategoriesRepository(),
                .. GenerateDeletedInvestmentFundCategoriesRepository(),
            ];

            return investmentFundCategoriesRepository;

        }


        [Fact]
        public void DeleteInvestmentFundCategoryById_SoftDeletes_Categories_Correctly_From_Repository()
        {

            //Setup
            InvestmentFundCategory categoryToDelete = new InvestmentFundCategory
            {
                Id = 1,
                Name = "Category to delete",
                InvestmentFundId = 1,
                InvestmentFund = new InvestmentFund
                {
                    Id = 1,
                    Name = "TestInvestmentFund",
                    Description = "TestInvestmentFundDescription",
                    Deleted = false
                },
                Deleted = false
            };
            List<InvestmentFundCategory> mockInvestmentFundCategoriesDatabase = new List<InvestmentFundCategory>();
            Mock<IRepository<InvestmentFundCategory>> mockInvestmentFundCategoriesRepository = new Mock<IRepository<InvestmentFundCategory>>();
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.Update(It.IsAny<InvestmentFundCategory>())).Callback((InvestmentFundCategory category) => mockInvestmentFundCategoriesDatabase[0] = category);
            mockInvestmentFundCategoriesRepository.Setup(repository => repository.GetById(1)).Returns(categoryToDelete);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<InvestmentFundCategory>()).Returns(mockInvestmentFundCategoriesRepository.Object);
            InvestmentFundCategoriesManager investmentFundCategoriesManager = new InvestmentFundCategoriesManager(mockUnitOfWork.Object);


            //Arrange
            mockInvestmentFundCategoriesDatabase.Add(categoryToDelete);

            bool expectedDeletedValue = true;

            //Act
            investmentFundCategoriesManager.DeleteInvestmentFundCategoryById(categoryToDelete.Id);
            InvestmentFundCategory obtainedDeletedCategory = mockInvestmentFundCategoriesDatabase.Single();

            Assert.Equal(expectedDeletedValue, obtainedDeletedCategory.Deleted);
        }

    }
}