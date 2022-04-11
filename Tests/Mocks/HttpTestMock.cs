using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Tests.Mocks;

/// <summary>
/// The HTTP mocking is not trivial because of non-overridable methods in HttpClient.
/// </summary>
public class HttpTestMock
{
#nullable disable
    private Mock<HttpMessageHandler> MockMessageHandler { get; set; }
    private Mock<IHttpClientFactory> MockHttpClientFactory { get; set; }
#nullable enable
    public IHttpClientFactory HttpClientFactory => MockHttpClientFactory.Object;
    
    public void VerifyRequest(Func<HttpRequestMessage, bool> verifier, Times times)
    {
        MockMessageHandler
            .Protected()
            .Verify("SendAsync", times,
                ItExpr.Is<HttpRequestMessage>(request => verifier.Invoke(request)),
                ItExpr.IsAny<CancellationToken>());
    }
    
    public void MockOnce(HttpStatusCode status, string json)
    {
        // Contains slight modification to answer https://stackoverflow.com/a/53081867
        // License: CC BY-S.A 4.0 
        var response = new HttpResponseMessage(status)
        {
            Content =  new StringContent(json, System.Text.Encoding.UTF8, "application/json" )
        };
        
        MockMessageHandler = new Mock<HttpMessageHandler>();
        MockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response)
            .Verifiable();
        MockHttpClientFactory = CreateHttpClientFactory(new HttpClient(MockMessageHandler.Object));
    }
    
    private static Mock<IHttpClientFactory> CreateHttpClientFactory(HttpClient client)
    {
        var mock = new Mock<IHttpClientFactory>();
        mock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(client);
        return mock;
    }
}
