using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consumer
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

        // public async Task<List<Product>> GetAllProductsAsync()
        // {
        //     using HttpClient client = _factory.CreateClient("Products");
        //     var httpResponseMessage = await client.GetAsync("/api/products");
        //     httpResponseMessage.EnsureSuccessStatusCode();
        //     var json = await httpResponseMessage.Content.ReadAsStreamAsync();
        //     var resp = await JsonSerializer.DeserializeAsync<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //     return resp!;
        // }
        public async Task<Product> GetProductsByIdAsync(int id)
        {
            using HttpClient client = _factory.CreateClient("Products");
            var httpResponseMessage = await client.GetAsync($"/api/products/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();
            var json = await httpResponseMessage.Content.ReadAsStreamAsync();
            var resp = await JsonSerializer.DeserializeAsync<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return resp!;
        }
    }
}
