using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext storeContext;

        public ProductController(StoreContext _storeContext)
        {
            storeContext = _storeContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // IEnumerable<Product> listOfProduct = await storeContext.Products.ToListAsync();
            // return Ok(listOfProduct);

            return await storeContext.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            Product? responseProduct = await storeContext.Products.FindAsync(id);
            return responseProduct == null ? NotFound() : Ok(responseProduct);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product addProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data");
            await storeContext.AddAsync(addProduct);
            await storeContext.SaveChangesAsync();
            return addProduct;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if(id<=0)
                return BadRequest("Invalid Product Id.");
            if (ProductExists(id))
            {
                storeContext.Products.Update(product);
                await storeContext.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest($"No Data found for the given Id {id}");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if(id <=0)
                return BadRequest("Invalid Product Id or Product Not Found");
            Product? responseProduct = await storeContext.Products.FindAsync(id);
            if(responseProduct == null)
                return BadRequest("Invalid Product");
            storeContext.Products.Remove(responseProduct);
            await storeContext.SaveChangesAsync();
            return Ok("Product Removed Successfully.");
        }


        private bool ProductExists(int id)
        {
            return storeContext.Products.Any(x => x.Id == id);
        }
    }
}
