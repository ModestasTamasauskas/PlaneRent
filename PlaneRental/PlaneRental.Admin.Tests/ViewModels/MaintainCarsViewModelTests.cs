using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class MaintainPlanesViewModelTests
    {
        [TestMethod]
        public void TestViewLoaded()
        {
            Plane[] data = new List<Plane>()
                {
                    new Plane() { PlaneId = 1 },
                    new Plane() { PlaneId = 2 }
                }.ToArray();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();
            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().GetAllPlanes()).Returns(data);

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);

            Assert.IsTrue(viewModel.Planes == null);

            object loaded = viewModel.ViewLoaded; // fires off the OnViewLoaded protected method

            Assert.IsTrue(viewModel.Planes != null && viewModel.Planes.Count == data.Length && viewModel.Planes[0] == data[0]);
        }

        [TestMethod]
        public void TestCurrentPlaneSetting()
        {
            Plane Plane = new Plane() { PlaneId = 1 };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);

            Assert.IsTrue(viewModel.CurrentPlaneViewModel == null);

            viewModel.EditPlaneCommand.Execute(Plane);

            Assert.IsTrue(viewModel.CurrentPlaneViewModel != null && viewModel.CurrentPlaneViewModel.Plane.PlaneId == Plane.PlaneId);
        }

        [TestMethod]
        public void TestEditPlaneCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);

            viewModel.Planes = new ObservableCollection<Plane>()
                {
                    Plane
                };

            Assert.IsTrue(viewModel.Planes[0].Color == "White");
            Assert.IsTrue(viewModel.CurrentPlaneViewModel == null);

            viewModel.EditPlaneCommand.Execute(Plane);

            Assert.IsTrue(viewModel.CurrentPlaneViewModel != null);

            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().UpdatePlane(It.IsAny<Plane>())).Returns(viewModel.CurrentPlaneViewModel.Plane);

            viewModel.CurrentPlaneViewModel.Plane.Color = "Black";
            viewModel.CurrentPlaneViewModel.SaveCommand.Execute(null);

            Assert.IsTrue(viewModel.Planes[0].Color == "Black");
        }

        [TestMethod]
        public void TestDeletePlaneCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();
            //mockServiceFactory.Setup(mock => mock.CreateClient<IRentalService>().IsPlaneCurrentlyRented(Plane.PlaneId)).Returns(false);
           // mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().DeletePlane(Plane.PlaneId));

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);
            viewModel.Planes = new ObservableCollection<Plane>()
                {
                    Plane
                };

            viewModel.ConfirmDelete += (s, e) => e.Cancel = false;

            Assert.IsTrue(viewModel.Planes.Count == 1);

            viewModel.DeletePlaneCommand.Execute(Plane);

            Assert.IsTrue(viewModel.Planes.Count == 0);
        }

        [TestMethod]
        public void TestDeletePlaneCommandWithCancel()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();
            //mockServiceFactory.Setup(mock => mock.CreateClient<IRentalService>().IsPlaneCurrentlyRented(Plane.PlaneId)).Returns(false);
            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().DeletePlane(Plane.PlaneId));

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);
            viewModel.Planes = new ObservableCollection<Plane>()
                {
                    Plane
                };

            viewModel.ConfirmDelete += (s, e) => e.Cancel = true; // cancel the deletion

            Assert.IsTrue(viewModel.Planes.Count == 1);

            viewModel.DeletePlaneCommand.Execute(Plane);

            Assert.IsTrue(viewModel.Planes.Count == 1);
        }

        [TestMethod]
        public void TestDeletePlaneCommandWithError()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();
            //mockServiceFactory.Setup(mock => mock.CreateClient<IRentalService>().IsPlaneCurrentlyRented(Plane.PlaneId)).Returns(true); // currently rented
            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().DeletePlane(Plane.PlaneId));

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);
            viewModel.Planes = new ObservableCollection<Plane>()
            {
                Plane
            };

            bool errorOccured = false;
            viewModel.ErrorOccured += (s, e) => errorOccured = true;

            Assert.IsTrue(viewModel.Planes.Count == 1);

            viewModel.DeletePlaneCommand.Execute(Plane);

            Assert.IsTrue(errorOccured && viewModel.Planes.Count == 1);
        }

        [TestMethod]
        public void TestAddPlaneCommand()
        {
            Plane Plane = new Plane() { PlaneId = 1, Color = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            MaintainPlanesViewModel viewModel = new MaintainPlanesViewModel(mockServiceFactory.Object);
            viewModel.Planes = new ObservableCollection<Plane>();

            Assert.IsTrue(viewModel.CurrentPlaneViewModel == null);

            viewModel.AddPlaneCommand.Execute(Plane);

            Assert.IsTrue(viewModel.CurrentPlaneViewModel != null);

            //mockServiceFactory.Setup(mock => mock.CreateClient<IInventoryService>().UpdatePlane(It.IsAny<Plane>())).Returns(viewModel.CurrentPlaneViewModel.Plane);

            viewModel.CurrentPlaneViewModel.SaveCommand.Execute(null);

            Assert.IsTrue(viewModel.Planes != null && viewModel.Planes.Count == 1);
        }
    }
}
