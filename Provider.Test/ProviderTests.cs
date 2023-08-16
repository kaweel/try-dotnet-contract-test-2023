using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace Provider.Tests
{
    public class ProviderTests : IDisposable
    {
        private static readonly Uri _providerUri = new("http://localhost:5195");
        private static readonly Uri _providerStateUri = new("http://localhost:5196");

        private readonly IHost _server;
        private readonly PactVerifier _verifier;

        public ProviderTests(ITestOutputHelper output)
        {
            _server = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(webBuilder =>
                              {
                                  webBuilder.UseUrls(_providerStateUri.ToString());
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
            var pactFile = new FileInfo(Path.Join("..", "..", "..", "..", "pacts", "Comsumer-Provider.json"));
            _verifier
            .ServiceProvider("Provider", _providerUri)
            .WithFileSource(pactFile)
            .WithProviderStateUrl(new Uri(_providerStateUri, "/provider-states"))
            .Verify();
        }
    }
}
