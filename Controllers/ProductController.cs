using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagement.models;

namespace ShopManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var thisProduct = await _context.Products.FirstOrDefaultAsync(x => x.Price == id);

            if (thisProduct == null)
                return NotFound();


            return Ok(thisProduct);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            var thisProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (thisProduct != null)
                return Conflict("Cannot create the item because it already exists.");

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(product.Name));

            return Created(resourceUrl, product);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Product product)
        {
            var thisProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (thisProduct == null)
                return BadRequest("Cannot update a non existing term.");

            thisProduct.Name = product.Name;
            thisProduct.Price = product.Price;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var thisProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (thisProduct == null)
                return NotFound();

            _context.Products.Remove(thisProduct);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}