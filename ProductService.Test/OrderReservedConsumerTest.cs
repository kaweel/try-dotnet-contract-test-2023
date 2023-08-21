using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using ProductService.PubSub;
using Xunit.Abstractions;

namespace ProductService.Test
{
    public class OrderReservedConsumerTest
    {
        private readonly IMessagePactBuilderV3 _pact;
        private readonly IMessageConsumer _messageConsumer;
        private readonly Mock<IProductService> _mockProductService;

        public OrderReservedConsumerTest(ITestOutputHelper output)
        {
            _mockProductService = new Mock<IProductService>();
            _messageConsumer = new MessageConsumer(_mockProductService.Object);
            var config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "..", "pacts"),
                Outputters = new[]
                {
                    new XunitOutput(output)
                },
                LogLevel = PactLogLevel.Debug
            };
            _pact = Pact.V3("Product-Reserved", "Order", config).WithMessageInteractions();
        }

        [Fact]
        [Trait("Category", "Consumer")]
        public async Task OnReservedEventAsync()
        {
            var expected = new ReservedEvent(9);
            await _pact
                      .ExpectsToReceive("an event mark reserve product")
                      .WithJsonContent(new
                      {
                          expected.Id
                      })
                      .VerifyAsync<ReservedEvent>(async message =>
                      {
                          await _messageConsumer.OnReservedEventAsync(message);
                          _mockProductService.Verify(s => s.ReserveProduct(message.Id));
                      });
        }
    }
}