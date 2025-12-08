using AutoMapper;
using BLL.Interface.Services.Abstractions;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PL.DTOS;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving products", details = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return Ok(_mapper.Map<ProductDTO>(product));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving product", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid product data", errors = ModelState });

            var product = _mapper.Map<Product>(dto);
            var created = await _productRepository.CreateAsync(product);

            return CreatedAtAction(nameof(GetProduct),
                new { id = created.Id },
                _mapper.Map<ProductDTO>(created));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating product", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDTO dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch between URL and body" });

            var product = _mapper.Map<Product>(dto);
            var updated = await _productRepository.UpdateAsync(id, product);

            if (updated == null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return Ok(_mapper.Map<ProductDTO>(updated));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating product", details = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting product", details = ex.Message });
        }
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAllProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            foreach (var p in products)
                await _productRepository.DeleteAsync(p.Id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting all products", details = ex.Message });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] ProductSearchDTO search)
    {
        try
        {
            if (search == null)
                return BadRequest(new { message = "Search parameters cannot be null" });

            var products = await _productRepository.SearchAsync(search);
            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error searching products", details = ex.Message });
        }
    }

    [HttpGet("{id}/FinalPrice")]
    public async Task<IActionResult> CalculateFinalPrice(int id)
    {
        try
        {
            var final = await _productRepository.CalculateFinalPriceAsync(id);
            if (final == null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return Ok(new { FinalPrice = final });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error calculating final price", details = ex.Message });
        }
    }
}