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
        var products = await _productRepository.GetAllAsync();
        return Ok(_mapper.Map<List<ProductDTO>>(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(_mapper.Map<ProductDTO>(product));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        var created = await _productRepository.CreateAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, _mapper.Map<ProductDTO>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDTO dto)
    {
        if (id != dto.Id) return BadRequest();
        var product = _mapper.Map<Product>(dto);
        var updated = await _productRepository.UpdateAsync(id, product);
        if (updated == null) return NotFound();
        return Ok(_mapper.Map<ProductDTO>(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productRepository.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAllProducts()
    {
        var products = await _productRepository.GetAllAsync();
        foreach (var p in products) await _productRepository.DeleteAsync(p.Id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] ProductSearchDTO search)
    {
        if (search == null) return BadRequest("Search parameters cannot be null.");
        var products = await _productRepository.SearchAsync(search);
        return Ok(_mapper.Map<List<ProductDTO>>(products));
    }

    [HttpGet("{id}/FinalPrice")]
    public async Task<IActionResult> CalculateFinalPrice(int id)
    {
        var final = await _productRepository.CalculateFinalPriceAsync(id);
        if (final == null) return NotFound();
        return Ok(new { FinalPrice = final });
    }

}
