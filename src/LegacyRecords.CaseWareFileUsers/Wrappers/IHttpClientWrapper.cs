using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LegacyRecords.CaseWareFileUsers.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> DeleteAsync(string requestUri);

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsync(string requestUri, StringContent stringContent);

        Task<HttpResponseMessage> PutAsync(string requestUri, StringContent stringContent);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);

        void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeaderValue);

        void SetIfMatchHeader(EntityTagHeaderValue entityTagHeaderValue);
    }
}
