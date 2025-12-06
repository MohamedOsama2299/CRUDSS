    using AutoMapper;
    using DAL.Contexts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PL.DTOS;

    namespace CRUDSS.Controllers
    {

        [Route("api/[controller]")]
        [ApiController]
        public class ProductController : ControllerBase
        {
            private readonly CRUDSDbContext _context;
            private readonly IMapper _mapper;

            public ProductController(CRUDSDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            [HttpGet]
            public async Task<IActionResult> GetAllProducts()
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .ToListAsync();

                var productDTOs = _mapper.Map<List<ProductDTO>>(products);
                return Ok(productDTOs);
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetProduct(int id)
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                    return NotFound();
                var productDTO = _mapper.Map<ProductDTO>(product);
                return Ok(productDTO);
            }
            [HttpPost("CreateProduct")]
            public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
            {
                var product = _mapper.Map<DAL.Models.Product>(productDTO);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDTO);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }