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
    public class CategoriesManagerTests
    {

        [Fact]
        public void AddCategory_Adds_Categories_Correctly_To_Repository()
        {
            //Setup
            List<Category> mockCategoriesDatabase = new List<Category>();
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
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


        public static IEnumerable<object[]> Data()
        {

            bool? deleted = null;

            yield return new object[] { false, GenerateActiveCategoriesRepository() };
            yield return new object[] { true, GenerateDeletedCategoriesRepository() };
            yield return new object[] { deleted, GenerateCategoriesRepository() };

        }

        [Theory]
        [MemberData(nameof(Data))]

        public void GetAllCategories_Returns_All_Categories_From_Repository_With_Correct_filters(bool? deleted, IEnumerable<Category> expectedCategoriesDatabase)
        {
            //Setup and arrange
            IEnumerable<Category> mockCategoriesDatabase = expectedCategoriesDatabase;
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

        private static IEnumerable<Category> GenerateActiveCategoriesRepository()
        {
            List<Category> activeCategoriesRepository = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "TestCategory1",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 5,
                    Deleted = false
                },
                new Category
                {
                    Id = 2,
                    Name = "TestCategory2",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 10,
                    Deleted = false
                }
            };

            return activeCategoriesRepository;

        }

        private static IEnumerable<Category> GenerateDeletedCategoriesRepository()
        {
            List<Category> deletedCategoriesRepository = new List<Category>
            {
                new Category
                {
                    Id = 3,
                    Name = "Deleted TestCategory1",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 7,
                    Deleted = true
                },
                new Category
                {
                    Id = 4,
                    Name = "Deleted TestCategory2",
                    FinancialTransactions = new List<FinancialTransaction>(),
                    Percentage = 20,
                    Deleted = true
                }
            };

            return deletedCategoriesRepository;
        }

        private static IEnumerable<Category> GenerateCategoriesRepository()
        {
            List<Category> categoriesRepository =
            [
                .. GenerateActiveCategoriesRepository(),
                .. GenerateDeletedCategoriesRepository(),
            ];
            return categoriesRepository;

        }


        [Fact]
        public void DeleteCategoryById_SoftDeletes_Categories_Correctly_From_Repository()
        {

            //Setup
            Category categoryToDelete = new Category
            {
                Id = 1,
                Name = "Category to delete",
                Percentage = 15
            };
            List<Category> mockCategoriesDatabase = new List<Category>();
            Mock<IRepository<Category>> mockCategoriesRepository = new Mock<IRepository<Category>>();
            mockCategoriesRepository.Setup(repository => repository.Update(It.IsAny<Category>())).Callback((Category category) => mockCategoriesDatabase[0] = category);
            mockCategoriesRepository.Setup(repository => repository.GetById(1)).Returns(categoryToDelete);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>()).Returns(mockCategoriesRepository.Object);
            CategoriesManager categoriesManager = new CategoriesManager(mockUnitOfWork.Object);


            //Arrange
            mockCategoriesDatabase.Add(categoryToDelete);

            bool expectedDeletedValue = true;

            //Act
            categoriesManager.DeleteCategoryById(categoryToDelete.Id);
            Category obtainedDeletedCategory = mockCategoriesDatabase.Single();

            Assert.Equal(expectedDeletedValue, obtainedDeletedCategory.Deleted);
        }

    }
}