using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PlaneRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GenericPrincipal principal = new GenericPrincipal(
               new GenericIdentity("Miguel"), new string[] { "Administrators", "PlaneRentalAdmin" });
            Thread.CurrentPrincipal = principal;
        }
        [TestMethod]
        public void UpdatePlane_AddNew()
        {
            Plane newPlane = new Plane();
            Plane addedPlane = new Plane() { PlaneId = 1 };

            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(mock => mock.GetDataRepository<IPlaneRepository>().Add(newPlane)).Returns(addedPlane);

            InventoryManager manager = new InventoryManager(mockDataRepositoryFactory.Object);

            Plane updatePlaneResults = manager.UpdatePlane(newPlane);

            Assert.IsTrue(updatePlaneResults == addedPlane);
        }

        [TestMethod]
        public void UpdatePlane_UpdateExisting()
        {
            Plane existingPlane = new Plane() { PlaneId = 1 };
            Plane updatedPlane = new Plane() { PlaneId = 1 };

            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(mock => mock.GetDataRepository<IPlaneRepository>().Update(existingPlane)).Returns(updatedPlane);

            InventoryManager manager = new InventoryManager(mockDataRepositoryFactory.Object);

            Plane updatePlaneResults = manager.UpdatePlane(existingPlane);

            Assert.IsTrue(updatePlaneResults == updatedPlane);
        }

        [TestMethod]
        public void DeletePlane()
        {

        }

        [TestMethod]
        public void GetPlane()
        {

        }

        [TestMethod]
        public void GetAllPlanes()
        {

        }

        [TestMethod]
        public void GetAvailablePlanes()
        {

        }
    }
}
