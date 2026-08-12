using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LegacyRecords.CaseWareFileUsers.Wrappers
{
    [ExcludeFromCodeCoverage(Justification = "This class cannot be tested as it serves to act upon the actual http client.")]
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await this.httpClient.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await this.httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, StringContent stringContent)
        {
            return await this.httpClient.PostAsync(requestUri, stringContent);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, StringContent stringContent)
        {
            return await this.httpClient.PutAsync(requestUri, stringContent);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            return await this.httpClient.SendAsync(httpRequestMessage);
        }

        public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeaderValue)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        public void SetIfMatchHeader(EntityTagHeaderValue entityTagHeaderValue)
        {
            this.httpClient.DefaultRequestHeaders.IfMatch.Clear();
            this.httpClient.DefaultRequestHeaders.IfMatch.Add(entityTagHeaderValue);
        }
    }
}
