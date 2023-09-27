using IdentityModel.Client;
using System.Net;

namespace Gym13.Extensions
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> GetAsync(string requestUri, Dictionary<string, string> headers);

        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, HttpClientHandler httpClientHandler = null);

        Task<TokenResponse> RequestPasswordTokenAsync(PasswordTokenRequest request);

        Task<TokenRevocationResponse> RevokeTokenAsync(TokenRevocationRequest request);

        Task<TokenResponse> RequestRefreshTokenAsync(RefreshTokenRequest request);

        Task<TokenResponse> RequestClientCredentialsTokenAsync(ClientCredentialsTokenRequest request);

    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(requestUri);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, Dictionary<string, string> headers)
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                return await client.GetAsync(requestUri);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, HttpClientHandler httpClientHandler = null)
        {
            using (var client = new HttpClient(httpClientHandler ?? new HttpClientHandler()))
            {
                return await client.PostAsync(requestUri, content);
            }
        }

        public async Task<TokenResponse> RequestPasswordTokenAsync(PasswordTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                return await client.RequestPasswordTokenAsync(request);
            }
        }

        public async Task<TokenRevocationResponse> RevokeTokenAsync(TokenRevocationRequest request)
        {
            using (var client = new HttpClient())
            {
                return await client.RevokeTokenAsync(request);
            }
        }

        public async Task<TokenResponse> RequestRefreshTokenAsync(RefreshTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                return await client.RequestRefreshTokenAsync(request);
            }
        }

        public async Task<TokenResponse> RequestClientCredentialsTokenAsync(ClientCredentialsTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                return await client.RequestClientCredentialsTokenAsync(request);
            }
        }
    }
}
