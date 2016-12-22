using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Admin.ViewModels;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PlaneRental.Admin.Tests
{
    [TestClass]
    public class DashboardViewModelTests
    {
        [TestMethod]
        public void TestViewLoaded()
        {
            Plane[] data = new List<Plane>()
            {
                new Plane(),
                new Plane()
            }.ToArray();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();
            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().GetAllPlanes()).Returns(data);

            DashboardViewModel viewModel = new DashboardViewModel(mockServiceFactory.Object);

            Assert.IsTrue(viewModel.Planes == null);

            object loaded = viewModel.ViewLoaded; // fires off the OnViewLoaded protected method

            Assert.IsTrue(viewModel.Planes != null && viewModel.Planes.Length == data.Length && viewModel.Planes[0] == data[0]);
        }
    }
}
