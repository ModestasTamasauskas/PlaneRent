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
    public class RentalsViewModelTests
    {
        [TestMethod]
        public void TestViewLoaded()
        {

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            RentalsViewModel viewModel = new RentalsViewModel(mockServiceFactory.Object);

            //Assert.IsTrue(viewModel.Planes == null);

            object loaded = viewModel.ViewLoaded; // fires off the OnViewLoaded protected method

            //Assert.IsTrue(viewModel.Planes != null && viewModel.Planes.Length == data.Length && viewModel.Planes[0] == data[0]);
        }
    }
}
