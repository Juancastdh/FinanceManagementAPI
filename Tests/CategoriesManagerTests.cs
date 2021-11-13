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
        public void AddCategory_Adds_Categories_Correctly_To_Repository()
        {
            //Setup
            List<Category> mockCategoriesDatabase = new List<Category>();
            Mock<IRepository<Category>>  mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.Add(It.IsAny<Category>())).Callback((Category category) => mockCategoriesDatabase.Add(category));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);

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
            Category addedCategory = mockCategoriesDatabase.Single();
            string addedCategoryString = JsonSerializer.Serialize(addedCategory);

            //Assert
            Assert.Equal(expectedCategoryString, addedCategoryString);
        }


        [Fact]
        public void GetAllCategories_Returns_All_Categories_From_Repository()
        {
            //Setup
            List<Category> mockCategoriesDatabase = new List<Category>();
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockCategoriesDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);


            //Arrange
            mockCategoriesDatabase.Add(new Category
            {
                Id = 1,
                Name = "TestCategory1",
                FinancialTransactions = new List<FinancialTransaction>(),
                Percentage = 5
            });
            mockCategoriesDatabase.Add(new Category
            {
                Id = 2,
                Name = "TestCategory2",
                FinancialTransactions = new List<FinancialTransaction>(),
                Percentage = 10
                }
            );

            //Act
            IEnumerable<Category> returnedCategories = categoriesManager.GetAllCategories();

            //Assert
            Assert.Equal(returnedCategories, mockCategoriesDatabase);



        }
    }
}