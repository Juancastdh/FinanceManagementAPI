using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
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
    public class CategoriesManagerTests
    {

        [Fact]
        public void AddCategory_Adds_Categories_Correctly()
        {
            //Setup
            List<Category> mockCategoryDatabase = new List<Category>();
            Mock<IRepository<Category>>  MockCategoryRepository = new Mock<IRepository<Category>>();
            MockCategoryRepository.Setup(repository => repository.Add(It.IsAny<Category>())).Callback((Category category) => mockCategoryDatabase.Add(category));
            Mock<IUnitOfWork> MockUnitOfWork = new Mock<IUnitOfWork>();
            MockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(MockCategoryRepository.Object);
            ICategoriesManager categoriesManager = new CategoriesManager(MockUnitOfWork.Object);

            //Arrange
            Category expectedCategory = new Category
            {
                Id = 1,
                Name = "TestCategory",
                FinancialTransactions = new List<FinancialTransaction>(),
                Percentage = 10
            };
            string expectedCategoryString = JsonSerializer.Serialize(expectedCategory);

            //Act
            categoriesManager.AddCategory(expectedCategory);
            Category addedCategory = mockCategoryDatabase.Single();
            string addedCategoryString = JsonSerializer.Serialize(addedCategory);

            //Assert
            Assert.Equal(expectedCategoryString, addedCategoryString);

        }
    }
}