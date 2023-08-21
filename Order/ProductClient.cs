using System.Text.Json;
using System.Text.Json.Serialization;

namespace Order
{
    public class ProductClient : IProductClient
    {
        private readonly IHttpClientFactory _factory;

        private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public ProductClient(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<Product> GetProductsByIdAsync(int id)
        {
            using HttpClient client = _factory.CreateClient("Product");
            var httpResponseMessage = await client.GetAsync($"/api/product/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();
            var json = await httpResponseMessage.Content.ReadAsStreamAsync();
            var resp = await JsonSerializer.DeserializeAsync<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return resp!;
        }
    }
}
