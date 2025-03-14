using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product addProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.AddAsync(addProduct);

            if (await _productRepository.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = addProduct.Id }, addProduct);
            }

            return StatusCode(500, "An error occurred while saving the product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id <= 0) return BadRequest("Invalid Product Id.");

            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            if (!await ProductExists(id))
            {
                return NotFound($"No Data found for the given Id {id}");
            }

            _productRepository.Update(product);

            if (await _productRepository.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "An error occurred while updating the product.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (id <= 0) return BadRequest("Invalid Product Id.");

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound("Product not found.");

            _productRepository.Delete(product);

            if (await _productRepository.SaveChangesAsync() > 0)
            {
                return Ok("Product removed successfully.");
            }

            return StatusCode(500, "An error occurred while removing the product.");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            IReadOnlyList<string> brands = await _productRepository.GetBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            IReadOnlyList<string> listOfTypes = await _productRepository.GetTypeAsync();
            return Ok(listOfTypes);
        }
        [HttpGet("filter")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _productRepository.GetProductsAsync(brand, type, sort ?? "Name"));
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _productRepository.ExistsAsync(id);
        }
    }
}
