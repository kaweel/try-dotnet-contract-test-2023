using System.Collections.Concurrent;

namespace Provider.Products
{
    public class ProductService : IProductService
    {        
        private readonly ConcurrentDictionary<int, Product> _products = new();

        public ProductService()
        {
            _products[9] = new Product(9, "CREDIT_CARD", "GEM Visa", "v2");
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            return Task.FromResult(_products[id]);
        }

        public Task InsertAsync(Product product)
        {
            _products[product.id] = product;
            return Task.CompletedTask;
        }
    }


}
