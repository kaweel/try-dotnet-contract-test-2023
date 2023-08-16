using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Provider.Products;
using Provider.Test;

namespace Provider.Tests
{

    public class ProviderStateMiddleware
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IDictionary<string, Action> _providerStates;
        private readonly RequestDelegate _next;
        private readonly FakeProductService _products;


        public ProviderStateMiddleware(RequestDelegate next, IProductService products)
        {
            _next = next;
            _products = (FakeProductService)products;
            _providerStates = new Dictionary<string, Action>
            {
                ["a product with id `9` exists"] = this.InsertProductId9,
                ["a product with id `10` doesn't exists"] = this.EnsureProduct9DoesNotExists,
            };

        }

        private async void InsertProductId9()
        {
            await _products.InsertAsync(new Product(9, "CREDIT_CARD", "GEM Visa", "v2"));
        }

        private async void EnsureProduct9DoesNotExists()
        {
            await _products.removeAllAsync();
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            // context.Response.StatusCode = StatusCodes.Status200OK;
            if (context.Request.Method != HttpMethod.Post.ToString())
            {
                return;
            }

            string jsonRequestBody;

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            ProviderState? providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, _jsonSerializerOptions);
            if (providerState != null && !string.IsNullOrEmpty(providerState.State))
            {
                _providerStates[providerState.State].Invoke();
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value!.StartsWith("/provider-states"))
            {
                await HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(text: string.Empty);
                return;
            }

            await _next.Invoke(context);
            return;
        }
    }
}
