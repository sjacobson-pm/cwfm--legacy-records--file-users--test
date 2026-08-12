using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LegacyRecords.CaseWareFileUsers.Core.Models.PlanteMoranApis;
using LegacyRecords.CaseWareFileUsers.Core.Wrappers;

namespace LegacyRecords.CaseWareFileUsers.Core.HttpClients
{
    public abstract class HttpClientBase
    {
        protected HttpClientBase(HttpClient httpClient)
        {
            this.HttpClient = new HttpClientWrapper(httpClient);
        }

        protected internal IHttpClientWrapper HttpClient { get; set; }

        internal async Task ExecuteHttpDeleteRequestAsync(string requestUri)
        {
            var response = await this.HttpClient.DeleteAsync(requestUri);

            await this.EnsureSuccessfulResponseStatusCodeAsync(response);
        }

        internal async Task<(T? Result, HttpResponseHeaders Headers)> ExecuteHttpGetRequestAsync<T>(string requestUri, bool allow404NotFound = false)
        {
            var response = await this.HttpClient.GetAsync(requestUri);
            var statusCode = await this.EnsureSuccessfulResponseStatusCodeAsync(response, allow404NotFound);

            // if the record was not found, return default(T)
            if (allow404NotFound && statusCode == HttpStatusCode.NotFound)
            {
                return (default, response.Headers);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);

            return (result, response.Headers);
        }

        internal async Task<(IEnumerable<T> Results, PaginationMetadata PagingMetadata)> ExecutePagedHttpGetRequestAsync<T>(
            string requestUri,
            bool allow404NotFound = false)
        {
            var response = await this.HttpClient.GetAsync(requestUri);
            var statusCode = await this.EnsureSuccessfulResponseStatusCodeAsync(response, allow404NotFound);

            // if the record was not found, return default(T)
            if (allow404NotFound && statusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<T>>(content)!;
            var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.GetValues("X-Pagination").Single())!;

            return (results, paginationMetadata);
        }

        internal async Task ExecuteHttpPatchRequestAsync(string requestUri, string requestBodyContent)
        {
            var stringContent = new StringContent(requestBodyContent, System.Text.Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUri) { Content = stringContent };
            var response = await this.HttpClient.SendAsync(request);

            await this.EnsureSuccessfulResponseStatusCodeAsync(response);
        }

        internal async Task<(T Result, HttpResponseHeaders Headers)> ExecuteHttpPostRequestAsync<T>(string requestUri, string? requestBodyContent)
        {
            var response = await this.GetHttpPostResponseAsync(requestUri, requestBodyContent);
            await this.EnsureSuccessfulResponseStatusCodeAsync(response);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content)!;

            return (result, response.Headers);
        }

        internal async Task ExecuteHttpPostRequestAsync(string requestUri, string? requestBodyContent)
        {
            var response = await this.GetHttpPostResponseAsync(requestUri, requestBodyContent);
            await this.EnsureSuccessfulResponseStatusCodeAsync(response);
        }

        internal async Task ExecuteHttpPutRequestAsync(string requestUri, string requestBodyContent)
        {
            var stringContent = new StringContent(requestBodyContent, System.Text.Encoding.UTF8, "application/json");
            var response = await this.HttpClient.PutAsync(requestUri, stringContent);

            await this.EnsureSuccessfulResponseStatusCodeAsync(response);
        }

        internal virtual async Task<HttpStatusCode> EnsureSuccessfulResponseStatusCodeAsync(
            HttpResponseMessage response,
            bool allow404NotFound = false)
        {
            var successfulCodes = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent };

            // if we're allowing 404's to be considered successful
            // then add it to the successful codes list
            if (allow404NotFound)
            {
                successfulCodes.Add(HttpStatusCode.NotFound);
            }

            // if we're successful, return the status code
            if (successfulCodes.Contains(response.StatusCode))
            {
                return response.StatusCode;
            }

            //// we're not successful
            //// process the response and throw a HttpRequestException

            var responseContent = await response.Content.ReadAsStringAsync();
            var exceptionMessage = $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.\n{responseContent}".Trim();
            var exception = new HttpRequestException(exceptionMessage);

            exception.Data.Add("status-code", response.StatusCode);
            exception.Data.Add("reason-phrase", response.ReasonPhrase);
            exception.Data.Add("response-content", responseContent);

            throw exception;
        }

        internal virtual async Task<HttpResponseMessage> GetHttpPostResponseAsync(string requestUri, string? requestBodyContent)
        {
            requestBodyContent ??= string.Empty;

            var stringContent = new StringContent(requestBodyContent, System.Text.Encoding.UTF8, "application/json");
            var response = await this.HttpClient.PostAsync(requestUri, stringContent);

            return response;
        }
    }
}
