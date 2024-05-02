using MasterAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;
        public ProductsController(ShopContext context)
        {

            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            return product != null ? Ok(product) : NotFound();

        }

        [HttpGet("available")]
        public async Task<ActionResult<Product>> GetProductAvailable()
        {
            var product = await _context.Products.Where(x => x.IsAvailable).ToListAsync();

            return product != null ? Ok(product) : NotFound();

        }

        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new
                {
                    id = product.Id
                },
                product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(
            int id, [FromBody] Product product)
        {
            if (product.Id != id)
            {
                return BadRequest(product);
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Products.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            return Ok(product);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();

            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(product);

        }

        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteProducts(int[] ids)
        {
            List<Product> products = new List<Product>();

            foreach (var id in ids)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                {
                    return NotFound(product);
                }

                products.Add(product);
            }
            _context.RemoveRange(products);

            await _context.SaveChangesAsync();
            return Ok();

        }


    }
}
