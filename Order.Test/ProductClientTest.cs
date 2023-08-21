using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using Xunit.Abstractions;

namespace Order.Test
{
    public class ProductClientTest
    {
        private readonly IPactBuilderV3 _pact;
        private readonly Mock<IHttpClientFactory> _mockFactory;

        public ProductClientTest(ITestOutputHelper output)
        {

            _mockFactory = new Mock<IHttpClientFactory>();

            var config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "..", "pacts"),
                Outputters = new[]
                {
                    new XunitOutput(output)
                },
                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                },
                LogLevel = PactLogLevel.Debug
            };

            _pact = Pact.V3("Order-GetProductsById", "Product", config).WithHttpInteractions();
        }

        [Fact]
        [Trait("Category", "Consumer")]
        public async Task GetProductsByIdAsyncExists()
        {
            var expected = new Product(9, "CL500", "Motorcycle", "InStock");
            _pact
                .UponReceiving("request for a product by id")
                    .Given("a product with id `9` exists")
                    .WithRequest(HttpMethod.Get, "/api/product/9")
                    .WithHeader("Accept", "application/json; charset=utf-8")
                .WillRespond()
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithStatus(HttpStatusCode.OK)
                    .WithJsonBody(
                        new
                        {
                            expected.Id,
                            expected.Name,
                            expected.Type,
                            expected.Status
                        }
                    );
            // .WithJsonBody(new
            // {
            //     Id = Match.Integer(expected.Id),
            //     Name = Match.Type(expected.Name),
            //     Type = Match.Type(expected.Type),
            //     Version = Match.Type(expected.Version),
            // });

            await _pact.VerifyAsync(async ctx =>
            {
                _mockFactory
                    .Setup(f => f.CreateClient("Product"))
                    .Returns(() => new HttpClient
                    {
                        BaseAddress = ctx.MockServerUri,
                        DefaultRequestHeaders =
                        {
                            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json; charset=utf-8") }
                        }
                    });

                var client = new ProductClient(_mockFactory.Object);
                Product product = await client.GetProductsByIdAsync(9);

                product.Id.Equals(9);
                product.Name.Equals("CL500");
                product.Type.Equals("Motorcycle");
            });
        }

        [Fact]
        [Trait("Category", "Consumer")]
        public async Task GetProductsByIdAsyncDoesNotExists()
        {
            _pact
                .UponReceiving("request for a product by id")
                    .Given("a product with id `10` doesn't exists")
                    .WithRequest(HttpMethod.Get, "/api/product/10")
                    .WithHeader("Accept", "application/json; charset=utf-8")
                .WillRespond()
                    .WithHeader("Content-Type", "application/problem+json; charset=utf-8")
                    .WithStatus(HttpStatusCode.NotFound);

            await _pact.VerifyAsync(async ctx =>
            {
                _mockFactory
                    .Setup(f => f.CreateClient("Product"))
                    .Returns(() => new HttpClient
                    {
                        BaseAddress = ctx.MockServerUri,
                        DefaultRequestHeaders =
                        {
                            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json; charset=utf-8") }
                        }
                    });

                var client = new ProductClient(_mockFactory.Object);

                await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetProductsByIdAsync(10));
            });
        }
    }
}