using AutoFixture;
using Microsoft.Extensions.Options;
using NSubstitute;
using LegacyRecords.CaseWareFileUsers.Core.HttpClients;
using LegacyRecords.CaseWareFileUsers.Core.Options;
using LegacyRecords.CaseWareFileUsers.Core.Wrappers;
using Shouldly;
using Xunit;

namespace LegacyRecords.CaseWareFileUsers.Core.Tests.HttpClients
{
    public class FakeApiHttpClientTests
    {
        private readonly IFixture fixture;
        private readonly IHttpClientWrapper httpClient;
        private readonly IOptions<ConfigurationOptions> optionsAccessor;
        private readonly ConfigurationOptions configOptions;

        public FakeApiHttpClientTests()
        {
            this.fixture = new Fixture();
            this.httpClient = Substitute.For<IHttpClientWrapper>();
            this.optionsAccessor = Substitute.For<IOptions<ConfigurationOptions>>();

            var apiOptions = this.fixture.Build<FakeApiOptions>().With(o => o.Authority, "https://example.com/example").Create();
            this.configOptions = this.fixture.Build<ConfigurationOptions>().With(o => o.FakeApi, apiOptions).Create();
            this.optionsAccessor.Value.Returns(this.configOptions);
        }

        [Fact]
        public void PrepareAuthenticatedClientAsync_SampleTest_RemoveMe()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act

            // Assert
            sut.ShouldNotBeNull();
        }

        public FakeApiHttpClient CreateSut()
        {
            var sut = new FakeApiHttpClient(null!, this.optionsAccessor) { HttpClient = this.httpClient };

            return sut;
        }
    }
}
