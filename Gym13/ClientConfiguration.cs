using IdentityServer4;
using IdentityServer4.Models;

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
            var apiGrantTypes = new List<string> { };
            apiGrantTypes.AddRange(GrantTypes.ResourceOwnerPassword);
            apiGrantTypes.AddRange(GrantTypes.ClientCredentials);

            var clients = new List<Client>{
                new Client
                {
                    ClientId = "Gym13Client",
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets = new[] { new Secret("Gym13Secret".Sha256()) },
                    AllowedScopes = new[] { "Gym13ToApi", IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess },
                    AccessTokenLifetime = 3600 * 24 * 365 * 10, //10 years
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 1200,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    UpdateAccessTokenClaimsOnRefresh = true
                }
            };

            return clients;
        }
    }
}
