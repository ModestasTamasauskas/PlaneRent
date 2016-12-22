using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.ServiceModel;
using Newtonsoft.Json;

namespace PlaneRental.Client.Proxies
{
    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient : IInventoryService
    {
        private HttpClient _httpClient;

        private InventoryClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5001/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #region Async operations

        public async Task<Plane> UpdatePlaneAsync(Plane Plane)
        {
            HttpResponseMessage response;
            string postBody = Newtonsoft.Json.JsonConvert.SerializeObject(Plane);
            response = await _httpClient.PostAsync("api/InventoryManager/UpdatePlane", new StringContent(postBody, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Plane>();
            }

            return null;
        }

        public async Task DeletePlaneAsync(int PlaneId)
        {

            HttpResponseMessage response;
            response = await _httpClient.GetAsync($"api/InventoryManager/DeletePlane/{PlaneId}");

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        public async Task<Plane> GetPlaneAsync(int PlaneId)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = await _httpClient.PostAsync("api/InventoryManager/PlaneId", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Plane>();
            }

            return null;
        }

        public async Task<Plane[]> GetAllPlanesAsync()
        {
            HttpResponseMessage response;
            response = await _httpClient.GetAsync("api/InventoryManager/GetAllPlanes");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Plane[]>();
            }

            return null;
        }

        public async  Task<Plane[]> GetAvailablePlanesAsync(DateTime pickupDate, DateTime returnDate)
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync($"api/InventoryManager/GetAvailablePlanes?pickupDate={pickupDate}&returnDate={returnDate}").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Plane[]>().Result;
            }

            return null;
        }

        #endregion
    }
}
