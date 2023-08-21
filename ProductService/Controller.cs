using Microsoft.AspNetCore.Mvc;

namespace ProductService
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _product;

        public ProductsController(IProductService product)
        {
            _product = product;
        }

        // [HttpGet("")]
        // public async Task<ActionResult<List<Product>>> GetAllProductsAsync()
        // {
        //     try
        //     {
        //         var products = await this._product.GetAllProductsAsync();
        //         return Ok(products);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine("APP Exception: " + e.Message);
        //         return this.NotFound();
        //     }
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await this._product.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine("APP Exception: " + e.Message);
                return this.NotFound();
            }
        }
    }
}