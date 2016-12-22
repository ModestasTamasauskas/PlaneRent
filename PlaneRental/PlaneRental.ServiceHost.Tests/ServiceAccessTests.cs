using System;
using System.Collections.Generic;
using System.ServiceModel;
using PlaneRental.Business.Contracts;
using PlaneRental.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PlaneRental.ServiceHost.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_manager_as_service()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6827/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpResponseMessage response = client.PostAsync("/api/todo").Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    string data = response.Content.ReadAsStringAsync().Result;
                //    var todos = JsonConvert.DeserializeObject<List<TodoItem>>(data);
                //    return todos;
                //}
            }
        }

        [TestMethod]
        public void test_rental_manager_as_service()
        {
            ChannelFactory<IRentalService> channelFactory =
                new ChannelFactory<IRentalService>("");

            IRentalService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_account_manager_as_service()
        {
            ChannelFactory<IAccountService> channelFactory =
                new ChannelFactory<IAccountService>("");

            IAccountService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }
    }
}
