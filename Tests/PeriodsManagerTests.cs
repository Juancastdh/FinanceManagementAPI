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
    public class PeriodsManagerTests
    {

        [Fact]
        public void AddPeriod_Adds_Periods_Correctly_To_Repository()
        {
            //Setup
            List<Period> mockPeriodsDatabase = new List<Period>();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.Add(It.IsAny<Period>())).Callback((Period period) => mockPeriodsDatabase.Add(period));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Arrange
            Period expectedPeriod = new Period
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)

            };
            string expectedPeriodString = JsonSerializer.Serialize(expectedPeriod);

            //Act
            periodsManager.AddPeriod(expectedPeriod);
            Period addedPeriod = mockPeriodsDatabase.Single();
            string addedPeriodString = JsonSerializer.Serialize(addedPeriod);

            //Assert
            Assert.Equal(expectedPeriodString, addedPeriodString);
        }


        [Fact]
        public void GetAllPeriods_Returns_All_NonDeleted_Periods_From_Repository()
        {
            //Setup and arrange
            IEnumerable<Period> mockPeriodsDatabase = GeneratePeriodsRepository();
            IEnumerable<Period> mockNonDeletedPeriodsDatabase = GenerateNonDeletedPeriodsRepository();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.GetAll(null, It.IsAny<Func<IQueryable<Period>, IOrderedQueryable<Period>>>(), It.IsAny<string>())).Returns(mockPeriodsDatabase);
            mockPeriodsRepository.Setup(repository => repository.GetAll(period => period.Deleted == false, It.IsAny<Func<IQueryable<Period>, IOrderedQueryable<Period>>>(), It.IsAny<string>())).Returns(mockNonDeletedPeriodsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Act
            IEnumerable<Period> returnedPeriods = periodsManager.GetAllPeriods();

            //Assert
            Assert.Equal(mockNonDeletedPeriodsDatabase, returnedPeriods);
        }

        [Fact]
        public void GetPeriodById_Returns_Correct_Period_From_Repository()
        {
            //Setup and arrange       
            Period expectedPeriod = new Period
            {
                Id = 5,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)
            };
            string expectedPeriodString = JsonSerializer.Serialize(expectedPeriod);
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.GetById(5)).Returns(expectedPeriod);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Act
            Period obtainedPeriod = periodsManager.GetPeriodById(5);
            string obtainedPeriodString = JsonSerializer.Serialize(obtainedPeriod);

            //Assert
            Assert.Equal(expectedPeriodString, obtainedPeriodString);

        }


        [Fact]
        public void UpdatePeriod_Updates_Periods_Correctly_To_Repository()
        {
            //Setup
            List<Period> mockPeriodsDatabase = new List<Period>();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.Update(It.IsAny<Period>())).Callback((Period period) => mockPeriodsDatabase[0] = period);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Arrange

            Period originalPeriod = new Period
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)
            };

            mockPeriodsDatabase.Add(originalPeriod);

            Period updatedPeriod = new Period
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(13),
                EndDate = DateTime.Now.AddDays(26)
            };

            string updatedPeriodString = JsonSerializer.Serialize(updatedPeriod);

            //Act
            periodsManager.UpdatePeriod(updatedPeriod);
            Period obtainedUpdatedPeriod = mockPeriodsDatabase.Single();

            string obtainedUpdatedPeriodString = JsonSerializer.Serialize(obtainedUpdatedPeriod);

            //Assert
            Assert.Equal(updatedPeriodString, obtainedUpdatedPeriodString);
        }

        [Fact]
        public void DeletePeriodById_SoftDeletes_Periods_Correctly_From_Repository()
        {

            //Setup
            Period periodToDelete = new Period
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10)

            };
            List<Period> mockPeriodsDatabase = new List<Period>();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.GetById(1)).Returns(periodToDelete);
            mockPeriodsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<Period, bool>>>(), It.IsAny<Func<IQueryable<Period>, IOrderedQueryable<Period>>>(), It.IsAny<string>())).Returns(mockPeriodsDatabase);
            mockPeriodsRepository.Setup(repository => repository.Update(It.IsAny<Period>())).Callback((Period period) => mockPeriodsDatabase[0] = period);     
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);


            //Arrange
            mockPeriodsDatabase.Add(periodToDelete);

            bool expectedDeletedValue = true;

            //Act
            periodsManager.DeletePeriodById(periodToDelete.Id);
            Period obtainedDeletedPeriod = mockPeriodsDatabase.Single();

            Assert.Equal(expectedDeletedValue, obtainedDeletedPeriod.Deleted);

        }

        private List<Period> GeneratePeriodsRepository()
        {
            List<Period> periodsRepository = new List<Period>
            {
                new Period
                {
                    Id = 1,
                    StartDate = DateTime.Now.AddDays(-26),
                    EndDate = DateTime.Now.AddDays(-13)
                },
                new Period
                {
                    Id = 2,
                    StartDate = DateTime.Now.AddDays(-13),
                    EndDate = DateTime.Now
                },
                new Period{
                    Id = 3,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(13),
                    Deleted = true
                }
            };

            return periodsRepository;

        }

        private List<Period> GenerateNonDeletedPeriodsRepository()
        {

            List<Period> periodsRepository = new List<Period>
            {
                new Period
                {
                    Id = 1,
                    StartDate = DateTime.Now.AddDays(-26),
                    EndDate = DateTime.Now.AddDays(-13)
                },
                new Period
                {
                    Id = 2,
                    StartDate = DateTime.Now.AddDays(-13),
                    EndDate = DateTime.Now
                }
            };

            return periodsRepository;
        }

        [Fact]
        public void DeletePeriodById_Gives_Error_If_Not_Latest_Period(){

            //Setup and arrange
            IList<Period> mockPeriodsDatabase = GeneratePeriodsRepository();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            int periodIdToDelete = 2;
            int periodIndexToDelete = 1;
            Period periodToDelete = mockPeriodsDatabase[periodIndexToDelete];
            mockPeriodsRepository.Setup(repository => repository.GetById(periodIdToDelete)).Returns(periodToDelete);
            mockPeriodsRepository.Setup(repository => repository.GetAll(It.IsAny<Expression<Func<Period, bool>>>(), It.IsAny<Func<IQueryable<Period>, IOrderedQueryable<Period>>>(), It.IsAny<string>())).Returns(mockPeriodsDatabase);
            mockPeriodsRepository.Setup(repository => repository.Update(It.IsAny<Period>())).Callback((Period period) => mockPeriodsDatabase[periodIndexToDelete] = period);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Act
            

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => periodsManager.DeletePeriodById(periodIdToDelete));

        }
    }
}
