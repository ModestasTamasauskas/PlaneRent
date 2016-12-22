using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.ServiceModel;

namespace PlaneRental.Client.Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountClient : IAccountService
    {
        private HttpClient _httpClient;

        private AccountClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5001/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        #region Async operations

        public async Task<Account> GetCustomerAccountInfoAsync(string loginEmail)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("loginEmail", loginEmail));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response;
            response = await _httpClient.PostAsJsonAsync("api/AccountManager/GetCustomerAccountInfo", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Account>();
            }

            return null;
        }

        public async Task UpdateCustomerAccountInfoAsync(Account account)
        {
            HttpResponseMessage response;
            response = await _httpClient.PostAsJsonAsync("api/AccountManager/UpdateCustomerAccountInfo", account);

            if (response.IsSuccessStatusCode)
            {
                
            }

        }

        #endregion
    }
}
