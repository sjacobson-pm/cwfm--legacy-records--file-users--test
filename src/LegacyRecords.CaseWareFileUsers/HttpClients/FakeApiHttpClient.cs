using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using LegacyRecords.CaseWareFileUsers.Options;

namespace LegacyRecords.CaseWareFileUsers.HttpClients
{
    public class FakeApiHttpClient : HttpClientBase, IFakeApiHttpClient
    {
        private readonly ConfigurationOptions options;
        private readonly IConfidentialClientApplication confidentialClientApplication;

        public FakeApiHttpClient(HttpClient httpClient, IOptions<ConfigurationOptions> optionsAccessor)
            : base(httpClient)
        {
            this.options = optionsAccessor.Value;
            var apiOptions = this.options.FakeApi;

            this.confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(apiOptions.ClientId)
                                                                                     .WithClientSecret(apiOptions.ClientSecret)
                                                                                     .WithAuthority(new Uri(apiOptions.Authority))
                                                                                     .Build();
        }

        //// todo:: this method is just a sample; remove me
        //// public async Task<WidgetDto> CreateWidgetAsync(uint apiVersion, WidgetForCreationDto widget)
        //// {
        ////     await this.PrepareAuthenticatedClientAsync();
        ////     var apiVersionQueryParam = $"apiVersion={apiVersion}";
        ////     var requestUri = $"{Endpoints.Widgets}?{apiVersionQueryParam}";
        ////     var requestBodyContent = JsonConvert.SerializeObject(widget);
        ////     var (newWidget, headers) = await this.ExecuteHttpPostRequestAsync<WidgetDto>(requestUri, requestBodyContent);
        ////     var etag = headers.Single(o => o.Key == "ETag").Value.Single();
        ////     newWidget.ETag = etag;
        ////     return newWidget;
        //// }

        //// todo:: this method is just a sample; remove me
        //// public async Task DeleteWidgetAsync(uint apiVersion, int widgetId, string etag)
        //// {
        ////     await this.PrepareAuthenticatedClientAsync();
        ////     var apiVersionQueryParam = $"apiVersion={apiVersion}";
        ////     var requestUri = $"{Endpoints.Widgets}/{widgetId}?{apiVersionQueryParam}";
        ////     this.HttpClient.SetIfMatchHeader(new EntityTagHeaderValue($"\"{etag}\""));
        ////     await this.ExecuteHttpDeleteRequestAsync(requestUri);
        //// }

        //// todo:: this method is just a sample; remove me
        //// public async Task<WidgetDto?> GetWidgetAsync(uint apiVersion, int widgetId, string? fields)
        //// {
        ////     await this.PrepareAuthenticatedClientAsync();
        ////     var apiVersionQueryParam = $"apiVersion={apiVersion}";
        ////     var fieldsQueryParam = fields == null ? string.Empty : $"&fields={fields}";
        ////     var requestUri = $"{Endpoints.Widgets}/{widgetId}?{apiVersionQueryParam}&{fieldsQueryParam}";
        ////     var (widget, headers) = await this.ExecuteHttpGetRequestAsync<WidgetDto>(requestUri, true);
        ////     if (widget != null)
        ////     {
        ////         var etag = headers.Single(o => o.Key == "ETag").Value.Single();
        ////         widget.ETag = etag;
        ////     }
        ////     return widget;
        //// }

        //// todo:: this method is just a sample; remove me
        //// public async Task<(IEnumerable<WidgetDto> Widgets, PaginationMetadata PaginationMetadata)> GetWidgetsAsync(
        ////     uint apiVersion, uint? pageNumber, uint? pageSize, string? filter, string? orderBy, string? fields, string? searchQuery)
        //// {
        ////     await this.PrepareAuthenticatedClientAsync();
        ////     var apiVersionQueryParam = $"apiVersion={apiVersion}";
        ////     var pageNumberQueryParam = pageNumber == null ? string.Empty : $"&pageNumber={pageNumber}";
        ////     var pageSizeQueryParam = pageSize == null ? string.Empty : $"&pageSize={pageSize}";
        ////     var filterQueryParam = filter == null ? string.Empty : $"&filter={filter}";
        ////     var orderByQueryParam = orderBy == null ? string.Empty : $"&orderBy={orderBy}";
        ////     var fieldsQueryParam = fields == null ? string.Empty : $"&fields={fields}";
        ////     var searchQueryQueryParam = searchQuery == null ? string.Empty : $"&searchQuery={searchQuery}";
        ////     var requestUri = $"{Endpoints.Widgets}?{apiVersionQueryParam}&{pageNumberQueryParam}&" +
        ////                      $"{pageSizeQueryParam}&{filterQueryParam}&{orderByQueryParam}&{fieldsQueryParam}&{searchQueryQueryParam}";
        ////     var results = await this.ExecutePagedHttpGetRequestAsync<WidgetDto>(requestUri);
        ////     return results;
        //// }

        //// todo:: this method is just a sample; remove me
        //// public async Task UpdateWidgetAsync(uint apiVersion, int widgetId, string etag, List<JsonPatchOperation> jsonPatchOperations)
        //// {
        ////     await this.PrepareAuthenticatedClientAsync();
        ////     var apiVersionQueryParam = $"apiVersion={apiVersion}";
        ////     var requestUri = $"{Endpoints.Widgets}/{widgetId}?{apiVersionQueryParam}";
        ////     var requestBodyContent = JsonConvert.SerializeObject(jsonPatchOperations);
        ////     this.HttpClient.SetIfMatchHeader(new EntityTagHeaderValue($"\"{etag}\""));
        ////     await this.ExecuteHttpPatchRequestAsync(requestUri, requestBodyContent);
        //// }

        [ExcludeFromCodeCoverage(Justification = "This class cannot be tested as it serves to acquire a token.")]
        protected internal virtual async Task PrepareAuthenticatedClientAsync()
        {
            var apiOptions = this.options.FakeApi;
            var scopes = new[] { apiOptions.AppScope };
            var authenticationResult = await this.confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
            var authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            this.HttpClient.SetAuthorizationHeader(authenticationHeaderValue);
        }

        internal static class Endpoints
        {
            //// todo:: this endpoint is just a sample; remove me
            //// internal const string Widgets = "widgets";
        }
    }
}
