using System.Collections.Concurrent;

namespace Provider.Products
{
    public class FakeProductService : IProductService
    {

        private readonly ConcurrentDictionary<int, Product> _products = new();

        public FakeProductService()
        {

        }

        // public Task<List<Product>> GetAllProductsAsync()
        // {
        //     return Task.FromResult(_products);
        // }

        public Task<Product> GetProductByIdAsync(int id) { 
            return Task.FromResult(_products[id]);
        }

        public Task InsertAsync(Product product)
        {
            _products[product.id] = product;
            return Task.CompletedTask;
        }
    }

}