using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit3;
using Newtonsoft.Json;
using NSubstitute;
using LegacyRecords.CaseWareFileUsers.Models.PlanteMoranApis;
using LegacyRecords.CaseWareFileUsers.Tests.Fakes;
using LegacyRecords.CaseWareFileUsers.Wrappers;
using Shouldly;
using Xunit;

// async calls do not need to be awaited when checking received calls on substitutes
#pragma warning disable CS4014

namespace LegacyRecords.CaseWareFileUsers.Tests.HttpClients
{
    public class HttpClientBaseTests
    {
        private readonly IFixture fixture;
        private readonly IHttpClientWrapper httpClient;

        public HttpClientBaseTests()
        {
            this.fixture = new Fixture();
            this.httpClient = Substitute.For<IHttpClientWrapper>();
        }

        public static IEnumerable<object[]> GetUnsuccessfulHttpStatusCodes()
        {
            var successfulCodes = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent };

            var httpStatusCodes = Enum.GetValues(typeof(HttpStatusCode))
                                      .Cast<HttpStatusCode>()
                                      .Where(o => !successfulCodes.Contains(o))
                                      .Distinct()
                                      .OrderBy(o => o)
                                      .ToList();

            foreach (var statusCode in httpStatusCodes)
            {
                yield return new object[] { statusCode };
            }
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpDeleteRequestAsync_DefaultCase_EnsuresSuccessfulResult(string requestUri)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.DeleteAsync(requestUri).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpDeleteRequestAsync(requestUri);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>(), Arg.Any<bool>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpGetRequestAsync_DefaultCase_EnsuresSuccessfulResult(string requestUri, bool allow404NotFound)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>(), Arg.Any<bool>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpGetRequestAsync_Allow404IsTrueAndResponseIs404_ReturnsNullResult(string requestUri)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = true;
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.NotFound);

            // Act
            var actual = await sut.ExecuteHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            actual.Result.ShouldBeNull();
            actual.Headers.ShouldBe(httpResponseMessage.Headers);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpGetRequestAsync_Allow404IsFalse_ReturnsProperResults(string requestUri, FakeDto dto)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = false;
            var httpResponseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(dto)) };

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.OK);

            // Act
            var actual = await sut.ExecuteHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            actual.Result.ShouldBeEquivalentTo(dto);
            actual.Headers.ShouldBe(httpResponseMessage.Headers);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpGetRequestAsync_Allow404IsTrueAndResponseIsNot404_ReturnsProperResults(string requestUri, FakeDto dto)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = true;
            var httpResponseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(dto)) };

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.OK);

            // Act
            var actual = await sut.ExecuteHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            actual.Result.ShouldBeEquivalentTo(dto);
            actual.Headers.ShouldBe(httpResponseMessage.Headers);
        }

        [Theory]
        [AutoData]
        public async Task ExecutePagedHttpGetRequestAsync_DefaultCase_EnsuresSuccessfulResult(string requestUri)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = true;
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.NotFound);

            // Act
            await sut.ExecutePagedHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>(), Arg.Any<bool>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound);
        }

        [Theory]
        [AutoData]
        public async Task ExecutePagedHttpGetRequestAsync_Allow404IsTrueAndResponseIs404_ReturnsNull(string requestUri)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = true;
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.NotFound);

            // Act
            var actual = await sut.ExecutePagedHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            actual.PagingMetadata.ShouldBeNull();
            actual.Results.ShouldBeNull();
        }

        [Theory]
        [AutoData]
        public async Task ExecutePagedHttpGetRequestAsync_Allow404IsFalse_ReturnsProperResults(
            string requestUri,
            List<FakeDto> dtoList,
            PaginationMetadata paginationMetadata)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = false;

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(dtoList)),
                Headers = { { "X-Pagination", JsonConvert.SerializeObject(paginationMetadata) } },
            };

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.OK);

            // Act
            var actual = await sut.ExecutePagedHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            var resultsList = actual.Results.ToList();
            actual.PagingMetadata.ShouldBeEquivalentTo(paginationMetadata);
            resultsList.ForEach(actualDto => actualDto.ShouldBeEquivalentTo(dtoList[resultsList.IndexOf(actualDto)]));
        }

        [Theory]
        [AutoData]
        public async Task ExecutePagedHttpGetRequestAsync_Allow404IsTrueAndResponseIsNot404_ReturnsProperResults(
            string requestUri,
            List<FakeDto> dtoList,
            PaginationMetadata paginationMetadata)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = true;

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(dtoList)),
                Headers = { { "X-Pagination", JsonConvert.SerializeObject(paginationMetadata) } },
            };

            this.httpClient.GetAsync(requestUri).Returns(httpResponseMessage);
            sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound).Returns(HttpStatusCode.OK);

            // Act
            var actual = await sut.ExecutePagedHttpGetRequestAsync<FakeDto>(requestUri, allow404NotFound);

            // Assert
            var resultsList = actual.Results.ToList();
            actual.PagingMetadata.ShouldBeEquivalentTo(paginationMetadata);
            resultsList.ForEach(actualDto => actualDto.ShouldBeEquivalentTo(dtoList[resultsList.IndexOf(actualDto)]));
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPatchRequestAsync_DefaultCase_EnsuresSuccessfulResult(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPatchRequestAsync(requestUri, requestBodyContent);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPatchRequestAsync_DefaultCase_SendsProperPatchRequest(Uri requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPatchRequestAsync(requestUri.AbsoluteUri, requestBodyContent);

            // Assert
            this.httpClient.ReceivedWithAnyArgs(1).SendAsync(Arg.Any<HttpRequestMessage>());

            this.httpClient.Received(1)
                .SendAsync(
                     Arg.Is<HttpRequestMessage>(
                         o => o.Method == HttpMethod.Patch &&
                              o.RequestUri == requestUri &&
                              o.Content != null &&
                              o.Content.Headers.ContentType != null &&
                              o.Content.Headers.ContentType.CharSet == "utf-8" &&
                              o.Content.Headers.ContentType.MediaType == "application/json-patch+json"));
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPostRequestAsync_1_DefaultCase_EnsuresSuccessfulResult(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            sut.GetHttpPostResponseAsync(requestUri, requestBodyContent).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPostRequestAsync<FakeDto>(requestUri, requestBodyContent);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPostRequestAsync_1_DefaultCase_ReturnsProperResult(string requestUri, string requestBodyContent, FakeDto dto)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(dto)) };

            sut.GetHttpPostResponseAsync(requestUri, requestBodyContent).Returns(httpResponseMessage);

            // Act
            var actual = await sut.ExecuteHttpPostRequestAsync<FakeDto>(requestUri, requestBodyContent);

            // Assert
            actual.Result.ShouldBeEquivalentTo(dto);
            actual.Headers.ShouldBe(httpResponseMessage.Headers);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPostRequestAsync_2_DefaultCase_EnsuresSuccessfulResult(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            sut.GetHttpPostResponseAsync(requestUri, requestBodyContent).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPostRequestAsync(requestUri, requestBodyContent);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPutRequestAsync_DefaultCase_EnsuresSuccessfulResult(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.PutAsync(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPutRequestAsync(requestUri, requestBodyContent);

            // Assert
            sut.ReceivedWithAnyArgs(1).EnsureSuccessfulResponseStatusCodeAsync(Arg.Any<HttpResponseMessage>());
            sut.Received(1).EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task ExecuteHttpPutRequestAsync_DefaultCase_SendsProperPutRequest(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.PutAsync(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(httpResponseMessage);

            // Act
            await sut.ExecuteHttpPutRequestAsync(requestUri, requestBodyContent);

            // Assert
            this.httpClient.ReceivedWithAnyArgs(1).PutAsync(Arg.Any<string>(), Arg.Any<StringContent>());

            this.httpClient.Received(1)
                .PutAsync(
                     requestUri,
                     Arg.Is<StringContent>(
                         o => o.Headers.ContentType != null &&
                              o.Headers.ContentType.CharSet == "utf-8" &&
                              o.Headers.ContentType.MediaType == "application/json"));
        }

        [Theory]
        [InlineAutoData(HttpStatusCode.OK)]
        [InlineAutoData(HttpStatusCode.Created)]
        [InlineAutoData(HttpStatusCode.NoContent)]
        public async Task EnsureSuccessfulResponseStatusCodeAsync_SuccessfulStatusCode_ReturnsProperStatusCode(
            HttpStatusCode statusCode,
            bool allow404NotFound)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage { StatusCode = statusCode };

            sut.WhenForAnyArgs(o => o.EnsureSuccessfulResponseStatusCodeAsync(default!)).CallBase();

            // Act
            var actual = await sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound);

            // Assert
            actual.ShouldBe(statusCode);
        }

        [Theory]
        [MemberData(nameof(GetUnsuccessfulHttpStatusCodes))]
        public async Task EnsureSuccessfulResponseStatusCodeAsync_NotSuccessfulStatusCode_ThrowsException(HttpStatusCode statusCode)
        {
            // Arrange
            var sut = this.CreateSut();
            var allow404NotFound = false;
            var reasonPhrase = this.fixture.Create<string>();
            var responseContent = this.fixture.Create<string>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode, ReasonPhrase = reasonPhrase, Content = new StringContent(responseContent),
            };

            sut.WhenForAnyArgs(o => o.EnsureSuccessfulResponseStatusCodeAsync(default!)).CallBase();

            // Act
            var actual = await Should.ThrowAsync<HttpRequestException>(
                async () => await sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound));

            // Assert
            actual.Data["status-code"].ShouldBe(statusCode);
            actual.Data["reason-phrase"].ShouldBe(reasonPhrase);
            actual.Data["response-content"].ShouldBe(responseContent);
        }

        [Fact]
        public async Task EnsureSuccessfulResponseStatusCodeAsync_Allow404IsTrueAndStatusIs404_ReturnsProperStatusCode()
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            var allow404NotFound = true;

            sut.WhenForAnyArgs(o => o.EnsureSuccessfulResponseStatusCodeAsync(default!)).CallBase();

            // Act
            var actual = await sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound);

            // Assert
            actual.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task EnsureSuccessfulResponseStatusCodeAsync_Allow404IsFalseAndStatusIs404_ThrowsException()
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            var allow404NotFound = false;

            sut.WhenForAnyArgs(o => o.EnsureSuccessfulResponseStatusCodeAsync(default!)).CallBase();

            // Act
            var actual = await Should.ThrowAsync<HttpRequestException>(
                async () => await sut.EnsureSuccessfulResponseStatusCodeAsync(httpResponseMessage, allow404NotFound));

            // Assert
            actual.Data["status-code"].ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineAutoData(null)]
        public async Task GetHttpPostResponseAsync_RequestBodyContentIsNull_ReturnsProperResponse(string? requestBodyContent, string requestUri)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.PostAsync(
                     requestUri,
                     Arg.Is<StringContent>(
                         o => o.Headers.ContentType != null &&
                              o.Headers.ContentType.CharSet == "utf-8" &&
                              o.Headers.ContentType.MediaType == "application/json"))
                .Returns(httpResponseMessage);

            sut.WhenForAnyArgs(o => o.GetHttpPostResponseAsync(default!, default)).CallBase();

            // Act
            var actual = await sut.GetHttpPostResponseAsync(requestUri, requestBodyContent);

            // Assert
            actual.ShouldBe(httpResponseMessage);
        }

        [Theory]
        [AutoData]
        public async Task GetHttpPostResponseAsync_RequestBodyContentIsNotNull_ReturnsProperResponse(string requestUri, string requestBodyContent)
        {
            // Arrange
            var sut = this.CreateSut();
            var httpResponseMessage = new HttpResponseMessage();

            this.httpClient.PostAsync(
                     requestUri,
                     Arg.Is<StringContent>(
                         o => o.Headers.ContentType != null &&
                              o.Headers.ContentType.CharSet == "utf-8" &&
                              o.Headers.ContentType.MediaType == "application/json"))
                .Returns(httpResponseMessage);

            sut.WhenForAnyArgs(o => o.GetHttpPostResponseAsync(default!, default)).CallBase();

            // Act
            var actual = await sut.GetHttpPostResponseAsync(requestUri, requestBodyContent);

            // Assert
            actual.ShouldBe(httpResponseMessage);
        }

        private FakeHttpClient CreateSut()
        {
            var sutHttpClient = (HttpClient)null!;
            var sut = Substitute.ForPartsOf<FakeHttpClient>(sutHttpClient);
            sut.HttpClient = this.httpClient;

            sut.WhenForAnyArgs(o => o.EnsureSuccessfulResponseStatusCodeAsync(default!)).DoNotCallBase();
            sut.WhenForAnyArgs(o => o.GetHttpPostResponseAsync(default!, default)).DoNotCallBase();

            return sut;
        }
    }
}
