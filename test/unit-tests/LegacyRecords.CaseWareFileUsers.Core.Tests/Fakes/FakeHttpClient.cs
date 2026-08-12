using System.Net.Http;
using LegacyRecords.CaseWareFileUsers.Core.HttpClients;

namespace LegacyRecords.CaseWareFileUsers.Core.Tests.Fakes
{
    public class FakeHttpClient : HttpClientBase
    {
        public FakeHttpClient(HttpClient httpClient)
            : base(httpClient)
        {
        }
    }
}
