using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace PlaneRent.IdentityServer
{
    public class Configuration
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                   {
                       new ApiResource("openid", "My API")
                   };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
                   {
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                   };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
                   {
                       new Client 
            {
                Enabled = true,
                ClientName = "MVC Client",
                ClientId = "mvc",
                
                 AllowedGrantTypes = GrantTypes.Implicit,

                RedirectUris = new List<string>
                {
                    "http://localhost:5002"
                },
                
                AllowAccessToAllScopes = true
            }
                   };
        }
    }
}