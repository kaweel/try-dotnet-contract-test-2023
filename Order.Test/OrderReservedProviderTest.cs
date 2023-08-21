using Xunit.Abstractions;
using PactNet.Verifier;
using Order.PubSub;
using PactNet;
using PactNet.Infrastructure.Outputters;

namespace Order.Test
{

    public class OrderReservedProviderTest : IDisposable
    {

        private readonly PactVerifier _verifier;

        public OrderReservedProviderTest(ITestOutputHelper output)
        {
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
            _verifier.Dispose();
        }

        [Fact]
        [Trait("Category", "Provider")]
        public void Verify()
        {
            var pactFile = new FileInfo(Path.Join("..", "..", "..", "..", "pacts", "Product-Reserved-Order.json"));
            _verifier
            .MessagingProvider("Order")
            .WithProviderMessages(scenarios =>
            {
                scenarios.Add("an event mark reserve product", () => new ReservedEvent(9));
            })
            .WithFileSource(pactFile)
            .Verify();
        }
    }

}