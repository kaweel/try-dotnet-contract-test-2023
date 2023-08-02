using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace Provider.Tests
{
    public class ProviderTests : IDisposable
    {
        private static readonly Uri _providerUri = new("http://localhost:5000");

        private static readonly JsonSerializerSettings _options = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly IHost _server;
        private readonly PactVerifier _verifier;

        public ProviderTests(ITestOutputHelper output)
        {
            _server = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(webBuilder =>
                              {
                                  webBuilder.UseUrls(_providerUri.ToString());
                                  webBuilder.UseStartup<TestStartup>();
                              })
                              .Build();

            _server.Start();

            _verifier = new PactVerifier(
                new PactVerifierConfig
                {
                    LogLevel = PactLogLevel.Debug,
                    Outputters = new List<IOutput>
                {
                    new XunitOutput(output)
                }
                });
        }

        public void Dispose()
        {
            _server.Dispose();
            _verifier.Dispose();
        }

        [Fact]
        public void Verify()
        {
            var pactFile = new FileInfo(Path.Join("..", "..", "..", "..", "pacts", "ProductsClient-Product API.json"));
            _verifier
            .ServiceProvider("Product API", _providerUri)
            .WithFileSource(pactFile)
            .WithProviderStateUrl(new Uri(_providerUri, "/provider-states"))
            .Verify();
        }
    }
}
