using Microsoft.AspNetCore.Mvc;

namespace ProductService
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await this._service.GetProductByIdAsync(id);
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