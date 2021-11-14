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
            Mock<ILogger<PeriodsManager>> mockLogger = new Mock<ILogger<PeriodsManager>>();
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object, mockLogger.Object);

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
            Mock<ILogger<PeriodsManager>> mockLogger = new Mock<ILogger<PeriodsManager>>();
            PeriodsManager periodsManager = new PeriodsManager(mockUnitOfWork.Object, mockLogger.Object);


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
    }
}
