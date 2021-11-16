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
        public void GetPeriods_Returns_All_Periods_From_Repository()
        {
            //Setup
            List<Period> mockPeriodsDatabase = new List<Period>();
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.GetAll(null, null, "")).Returns(mockPeriodsDatabase);
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);


            //Arrange
            mockPeriodsDatabase.Add(new Period
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)
            });
            mockPeriodsDatabase.Add(new Period
            {
                Id = 2,
                StartDate = DateTime.Now.AddDays(13),
                EndDate = DateTime.Now.AddDays(26)
            }
            );

            //Act
            IEnumerable<Period> returnedPeriods = periodsManager.GetAllPeriods();

            //Assert
            Assert.Equal(mockPeriodsDatabase, returnedPeriods);

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
        public void DeletePeriodById_Removes_Periods_Correctly_From_Repository()
        {

            //Setup
            List<Period> mockPeriodsDatabase = new List<Period>();
            Period periodToRemain = new Period
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)
            };

            Period periodToBeDeleted = new Period
            {
                Id = 2,
                StartDate = DateTime.Now.AddDays(13),
                EndDate = DateTime.Now.AddDays(26)
            };

            mockPeriodsDatabase.Add(periodToRemain);
            mockPeriodsDatabase.Add(periodToBeDeleted);
            Mock<IRepository<Period>> mockPeriodsRepository = new Mock<IRepository<Period>>();
            mockPeriodsRepository.Setup(repository => repository.DeleteById(2)).Callback((int periodId) => {
                mockPeriodsDatabase.Remove(periodToBeDeleted);
            });
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Period>()).Returns(mockPeriodsRepository.Object);
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object);

            //Arrange


            IEnumerable<Period> expectedPeriodsDatabase = new List<Period>
            {
                periodToRemain
            };

            expectedPeriodsDatabase = expectedPeriodsDatabase.OrderBy(x => x.Id);

            //Act
            periodsManager.DeletePeriodById(periodToBeDeleted.Id);

            //Assert
            Assert.Equal(expectedPeriodsDatabase, mockPeriodsDatabase);

        }
    }
}
