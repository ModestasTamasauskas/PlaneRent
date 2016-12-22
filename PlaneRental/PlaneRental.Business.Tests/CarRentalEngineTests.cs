using System;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlaneRental.Business;

namespace PlaneRental.Business.Tests
{
    [TestClass]
    public class PlaneRentalEngineTests
    {
        [TestMethod]
        public void IsPlaneCurrentlyRented_any_account()
        {
            Rental rental = new Rental()
            {
            	PlaneId = 1
            };

            Mock<IRentalRepository> mockRentalRepository = new Mock<IRentalRepository>();
            mockRentalRepository.Setup(obj => obj.GetCurrentRentalByPlane(1)).Returns(rental);

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<IRentalRepository>()).Returns(mockRentalRepository.Object);

            PlaneRentalEngine engine = new PlaneRentalEngine(mockRepositoryFactory.Object);

            bool try1 = engine.IsPlaneCurrentlyRented(2);

            Assert.IsFalse(try1);

            bool try2 = engine.IsPlaneCurrentlyRented(1);

            Assert.IsTrue(try2);
        }
    }
}
