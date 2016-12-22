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
    public class EditPlaneViewModelTests
    {
        [TestMethod]
        public void TestViewModelConstruction()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditPlaneViewModel viewModel = new EditPlaneViewModel(mockServiceFactory.Object, Plane);

            Assert.IsTrue(viewModel.Plane != null && viewModel.Plane != Plane);
            Assert.IsTrue(viewModel.Plane.PlaneId == Plane.PlaneId && viewModel.Plane.Color == Plane.Color);
        }

        [TestMethod]
        public void TestSaveCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White", Description = "Kia Optima", Year = 2013, RentalPrice = 149.00M };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditPlaneViewModel viewModel = new EditPlaneViewModel(mockServiceFactory.Object, Plane);

            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().UpdatePlane(It.IsAny<Plane>())).Returns(viewModel.Plane);

            viewModel.Plane.Color = "Black";

            bool PlaneUpdated = false;
            string color = string.Empty;
            viewModel.PlaneUpdated += (s, e) =>
            {
                PlaneUpdated = true;
                color = e.Plane.Color;
            };

            viewModel.SaveCommand.Execute(null);

            Assert.IsTrue(PlaneUpdated);
            Assert.IsTrue(color == "Black");
        }

        [TestMethod]
        public void TestCanSaveCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White", Description = "Kia Optima", Year = 2013, RentalPrice = 149 };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditPlaneViewModel viewModel = new EditPlaneViewModel(mockServiceFactory.Object, Plane);

            Assert.IsFalse(viewModel.SaveCommand.CanExecute(null));

            viewModel.Plane.Color = "Black";

            Assert.IsTrue(viewModel.SaveCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestPlaneIsValid()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White", Description = "Kia Optima", Year = 2013 };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditPlaneViewModel viewModel = new EditPlaneViewModel(mockServiceFactory.Object, Plane);

            Assert.IsTrue(!viewModel.Plane.IsValid);

            viewModel.Plane.RentalPrice = 149;

            Assert.IsTrue(viewModel.Plane.IsValid);
        }

        [TestMethod]
        public void TestCancelCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditPlaneViewModel viewModel = new EditPlaneViewModel(mockServiceFactory.Object, Plane);

            bool canceled = false;
            viewModel.CancelEditPlane += (s, e) => canceled = true;

            Assert.IsTrue(!canceled);

            viewModel.CancelCommand.Execute(null);

            Assert.IsTrue(viewModel.CancelCommand.CanExecute(null));

            Assert.IsTrue(canceled);
        }
    }
}
