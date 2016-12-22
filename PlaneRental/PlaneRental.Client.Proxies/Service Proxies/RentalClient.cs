using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.ServiceModel;

namespace PlaneRental.Client.Proxies
{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : IRentalService
    {
        private HttpClient _httpClient;

        private RentalClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5001/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        public async Task<Rental> RentPlaneToCustomerAsync(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("loginEmail", loginEmail));
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            postData.Add(new KeyValuePair<string, string>("rentalDate", rentalDate.ToString()));
            postData.Add(new KeyValuePair<string, string>("dateDueBack", dateDueBack.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/RentPlaneToCustomer", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Rental>().Result;
            }

            return null;
        }

        public async Task AcceptPlaneReturnAsync(int PlaneId)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/AcceptPlaneReturn", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        public async Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("loginEmail", loginEmail));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/GetRentalHistory", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<IEnumerable<Rental>>().Result;
            }

            return null;
        }

        public Reservation GetReservationAsync(int reservationId)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("reservationId", reservationId.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;

            response = _httpClient.GetAsync($"api/RentalManager/GetReservation?reservationId={reservationId}").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Reservation>().Result;
            }

            return null;
        }

        public async Task<Reservation> MakeReservationAsync(string loginEmail, int PlaneId, DateTime rentalDate, DateTime returnDate)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("loginEmail", loginEmail));
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            postData.Add(new KeyValuePair<string, string>("rentalDate", rentalDate.ToString()));
            postData.Add(new KeyValuePair<string, string>("returnDate", returnDate.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/MakeReservation", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Reservation>().Result;
            }

            return null;
        }

        public async Task ExecuteRentalFromReservationAsync(int reservationId)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("reservationId", reservationId.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/ExecuteRentalFromReservation", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        public async Task CancelReservationAsync(int reservationId)
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync($"api/RentalManager/CancelReservation?reservationId={reservationId}").Result;

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        public async Task<CustomerReservationData[]> GetCurrentReservationsAsync()
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync("api/RentalManager/GetCurrentReservations").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<CustomerReservationData[]>().Result;
            }

            return null;
        }

        public async Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail)
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync($"api/RentalManager/GetCustomerReservations?loginEmail={loginEmail}").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<CustomerReservationData[]>().Result;
            }

            return null;
        }

        public async Task<Rental> GetRentalAsync(int rentalId)
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync($"api/RentalManager/GetRental?rentalId={rentalId}").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Rental>().Result;
            }

            return null;
        }

        public async Task<CustomerRentalData[]> GetCurrentRentalsAsync()
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync("api/RentalManager/GetCurrentRentals").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<CustomerRentalData[]>().Result;
            }

            return null;
        }

        public async Task<Reservation[]> GetDeadReservationsAsync()
        {
            HttpResponseMessage response;
            response = _httpClient.GetAsync("api/RentalManager/GetDeadReservations").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Reservation[]>().Result;
            }

            return null;
        }

        public async Task<bool> IsPlaneCurrentlyRentedAsync(int PlaneId)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/IsPlaneCurrentlyRented", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<bool>().Result;
            }

            return true;
        }

        public async Task<Rental> RentPlaneToCustomerAsync(string loginEmail, int PlaneId, DateTime dateDueBack)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("loginEmail", loginEmail));
            postData.Add(new KeyValuePair<string, string>("PlaneId", PlaneId.ToString()));
            postData.Add(new KeyValuePair<string, string>("dateDueBack", dateDueBack.ToString()));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = _httpClient.PostAsync("api/RentalManager/RentPlaneToCustomer", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Rental>().Result;
            }

            return null;
        }
    }
}
