using System.Net;
using System.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PactNet;
using Xunit.Abstractions;
using Match = PactNet.Matchers.Match;

namespace Consumer.Test
{
    public class ProductsClientTests
    {
        private readonly IPactBuilderV3 _pact;
        private readonly Mock<IHttpClientFactory> _mockFactory;
        // private readonly List<Product> _products;

        public ProductsClientTests(ITestOutputHelper output)
        {

            // _products = new List<Product>()
            // {
            //     new Product(9,"CREDIT_CARD","GEM Visa","v2"),
            //     new Product(10,"CREDIT_CARD","28 Degrees","v1"),
            // };

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
                    Converters = new JsonConverter[] { new StringEnumConverter() }
                },
                LogLevel = PactLogLevel.Debug
            };

            _pact = Pact.V3("ProductsClient", "Product API", config).WithHttpInteractions();
        }

        [Fact]
        public async Task GetProductsByIdAsync()
        {
            var expected = new Product(9, "CREDIT_CARD", "GEM Visa", "v2");
            _pact
                .UponReceiving("request for a product by id")
                    .Given("a product with id {id} exists", new Dictionary<string, string> { ["id"] = "9" })
                    .WithRequest(HttpMethod.Get, "/api/products/9")
                    .WithHeader("Accept", "application/json")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithJsonBody(new
                    {
                        Id = Match.Integer(expected.Id),
                        Name = Match.Type(expected.Name),
                        Type = Match.Type(expected.Type),
                        Version = Match.Type(expected.Version),
                    });

            await _pact.VerifyAsync(async ctx =>
            {
                _mockFactory
                    .Setup(f => f.CreateClient("Products"))
                    .Returns(() => new HttpClient
                    {
                        BaseAddress = ctx.MockServerUri,
                        DefaultRequestHeaders =
                        {
                            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") }
                        }
                    });

                var client = new ProductClient(_mockFactory.Object);
                Product product = await client.GetProductsByIdAsync(9);

                product.Id.Equals(9);
                product.Type.Equals("GEM Visa");
            });
        }

        // [Fact]
        // public async Task GetAllProducts()
        // {

        //     _pact
        //         .UponReceiving("a request for all products")
        //             .Given("products exists")
        //             .WithRequest(HttpMethod.Get, "/api/products")
        //             .WithHeader("Accept", "application/json")
        //         .WillRespond()
        //             .WithStatus(HttpStatusCode.OK)
        //             .WithJsonBody(new TypeMatcher(_products));

        //     await _pact.VerifyAsync(async ctx =>
        //     {
        //         _mockFactory
        //             .Setup(f => f.CreateClient("Products"))
        //             .Returns(() => new HttpClient
        //             {
        //                 BaseAddress = ctx.MockServerUri,
        //                 DefaultRequestHeaders =
        //                 {
        //                     Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") }
        //                 }
        //             });

        //         var client = new ProductClient(_mockFactory.Object);
        //         List<Product> products = await client.Get();

        //         products.Should().HaveCount(2);
        //     });
        // }
    }
}