using System.Net.Http;
using LegacyRecords.CaseWareFileUsers.HttpClients;

namespace LegacyRecords.CaseWareFileUsers.Tests.Fakes
{
    public class FakeHttpClient : HttpClientBase
    {
        public FakeHttpClient(HttpClient httpClient)
            : base(httpClient)
        {
        }
    }
}
