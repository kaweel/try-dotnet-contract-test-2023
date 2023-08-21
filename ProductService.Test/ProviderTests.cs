using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace ProductService.Test
{
    public class ProviderTests : IDisposable
    {
        private static readonly Uri _providerUri = new("http://localhost:5195");

        private readonly IHost _server;
        private readonly PactVerifier _verifier;

        public ProviderTests(ITestOutputHelper output)
        {
            _server = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(webBuilder =>
                              {
                                  webBuilder.UseUrls(_providerUri.ToString());
                                  webBuilder.UseStartup<Startup>();
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
        [Trait("Category", "Provider")]
        public void Verify()
        {
            var pactFile = new FileInfo(Path.Join("..", "..", "..", "..", "pacts", "Order-GetProductsById-Product.json"));
            _verifier
            .ServiceProvider("Product", _providerUri)
            .WithFileSource(pactFile)
            .Verify();
        }
    }
}
