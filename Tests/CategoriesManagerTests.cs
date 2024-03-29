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
            //Setup and arrange
            IEnumerable<Category> mockCategoriesDatabase = GenerateCategoriesRepository();
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockCategoriesDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);

            //Act
            IEnumerable<Category> returnedCategories = categoriesManager.GetAllCategories();

            //Assert
            Assert.Equal(mockCategoriesDatabase, returnedCategories);
        }


        [Fact]
        public void GetCategoryById_Returns_Correct_Category_From_Repository()
        {
            //Setup and arrange       
            Category expectedCategory = new Category
            {
                Id = 5,
                Name = "Expected Category",
                Percentage = 15
            };      
            string expectedCategoryString = JsonSerializer.Serialize(expectedCategory);
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.GetById(5)).Returns(expectedCategory);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);

            //Act
            Category obtainedCategory = categoriesManager.GetCategoryById(5);
            string obtainedCategoryString = JsonSerializer.Serialize(obtainedCategory);

            //Assert
            Assert.Equal(expectedCategoryString, obtainedCategoryString);

        }


        [Fact]
        public void UpdateCategory_Updates_Categories_Correctly_To_Repository()
        {
            //Setup
            List<Category> mockCategoriesDatabase = new List<Category>();
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.Update(It.IsAny<Category>())).Callback((Category category) => mockCategoriesDatabase[0] = category);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);

            //Arrange

            Category originalCategory = new Category
            {
                Id = 1,
                Name = "Original category",
                Percentage = 15
            };

            mockCategoriesDatabase.Add(originalCategory);

            Category updatedCategory = new Category
            {
                Id = 1,
                Name = "Updated category",
                Percentage = 20
            };

            string updatedCategoryString = JsonSerializer.Serialize(updatedCategory);

            //Act
            categoriesManager.UpdateCategory(updatedCategory);
            Category obtainedUpdatedCategory = mockCategoriesDatabase.Single();

            string obtainedUpdatedCategoryString = JsonSerializer.Serialize(obtainedUpdatedCategory);

            //Assert
            Assert.Equal(updatedCategoryString, obtainedUpdatedCategoryString);
        }

        private IEnumerable<Category> GenerateCategoriesRepository()
        {
            List<Category> categoriesRepository = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "TestCategory1",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 5
                },
                new Category
                {
                    Id = 2,
                    Name = "TestCategory2",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 10
                }
            };

            return categoriesRepository;
            
        }


        [Fact]
        public void DeleteCategoryById_Removes_Categories_Correctly_From_Repository()
        {

            //Setup
            List<Category> mockCategoriesDatabase = new List<Category>();
            Category categoryToRemain = new Category
            {
                Id = 1,
                Name = "Category that should remain in the repository",
                Percentage = 10
            };

            Category categoryToBeDeleted = new Category
            {
                Id = 2,
                Name = "Category that should be removed from the repository",
                Percentage = 50
            };

            mockCategoriesDatabase.Add(categoryToRemain);
            mockCategoriesDatabase.Add(categoryToBeDeleted);
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.DeleteById(2)).Callback((int categoryId) => {
                mockCategoriesDatabase.Remove(categoryToBeDeleted);
            });
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);

            //Arrange

            IEnumerable<Category> expectedCategoriesDatabase = new List<Category>
            {
                categoryToRemain
            };

            //Act
            categoriesManager.DeleteCategoryById(categoryToBeDeleted.Id);

            //Assert
            Assert.Equal(expectedCategoriesDatabase, mockCategoriesDatabase);

        }
    }
}