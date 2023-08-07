﻿using IdentityServer4.Models;

namespace Gym13
{
    public class ClientConfiguration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            var apiRess = new ApiResource[] {
                new ApiResource
                {
                    Name = "Gym13Api",
                    DisplayName = "Gym13 Api",
                    Scopes = new[]
                    {
                        "Gym13ToApi"
                    }
                }
            };
            return apiRess;
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            var apiRess = new ApiScope[] {
                new ApiScope
                {
                    Name = "Gym13ToApi",
                    DisplayName = "Gym13 web, mobile clients To Api",
                }
            };
            return apiRess;
        }


        public static IEnumerable<Client> GetClients()
        {
            var Gym13GrantTypes = new List<string>();
            Gym13GrantTypes.AddRange(GrantTypes.ResourceOwnerPassword);
            Gym13GrantTypes.AddRange(GrantTypes.ClientCredentials);

            var apiGrantTypes = new List<string> { };
            apiGrantTypes.AddRange(GrantTypes.ResourceOwnerPassword);
            apiGrantTypes.AddRange(GrantTypes.ClientCredentials);

            var clients = new List<Client>{
                new Client
                {
                    ClientId = "Gym13Client",
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = apiGrantTypes,
                    ClientSecrets = new[] { new Secret("Gym13Secret".Sha256()) },
                    AllowedScopes = new[] {"Gym13ToApi" },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600 * 24 * 365 * 10 //10 years
                }
            };

            return clients;
        }
    }
}