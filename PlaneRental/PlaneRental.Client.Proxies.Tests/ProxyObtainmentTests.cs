﻿using System;
using PlaneRental.Client.Bootstrapper;
using PlaneRental.Client.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using Core.Common.Contracts;

namespace PlaneRental.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MefLoader.Init();
        }

        [TestMethod]
        public void obtain_proxy_from_container_using_service_contract()
        {
            IInventoryService proxy 
                = ObjectBase.Container.GetExportedValue<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void obtain_proxy_from_service_factory()
        {
            IServiceFactory factory = new ServiceFactory();
            IInventoryService proxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void obtain_service_factory_and_proxy_from_container()
        {
            IServiceFactory factory =
                ObjectBase.Container.GetExportedValue<IServiceFactory>();

            IInventoryService proxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }
    }
}