using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Provider.Products;
using Provider.Test;

namespace Provider.Tests
{
    /// <summary>
    /// Middleware for handling provider state requests
    /// </summary>
    public class ProviderStateMiddleware
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IDictionary<string, Func<IDictionary<string, object>, Task>> _providerStates;
        private readonly RequestDelegate _next;
        private readonly IProductService _products;

        /// <summary>
        /// Initialises a new instance of the <see cref="ProviderStateMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next request delegate</param>
        /// <param name="orders">Orders repository for actioning provider state requests</param>
        public ProviderStateMiddleware(RequestDelegate next, IProductService products)
        {
            _next = next;
            _products = products;
            _providerStates = new Dictionary<string, Func<IDictionary<string, object>, Task>>
            {
                ["a product with id {id} exists"] = this.EnsureProductExistsAsync
            };

        }

        private async Task EnsureProductExistsAsync(IDictionary<string, object> parameters)
        {
            JsonElement id = (JsonElement)parameters["id"];
            await _products.InsertAsync(new Product(id.GetInt32(), "CREDIT_CARD", "GEM Visa", "v2"));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("context.Request.Path.Value: " + context.Request.Path.Value);
            if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false))
            {
                await _next.Invoke(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status200OK;

            if (context.Request.Method == HttpMethod.Post.ToString())
            {
                string jsonRequestBody;

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                try
                {
                    ProviderState? providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, Options);

                    if (!string.IsNullOrEmpty(providerState?.State))
                    {
                        await _providerStates[providerState.State].Invoke(providerState.Params);
                    }
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Failed to deserialise JSON provider state body:");
                    await context.Response.WriteAsync(jsonRequestBody);
                    await context.Response.WriteAsync(string.Empty);
                    await context.Response.WriteAsync(e.ToString());
                }
            }
        }
    }
}
