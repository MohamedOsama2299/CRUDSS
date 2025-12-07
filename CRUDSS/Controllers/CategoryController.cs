using AutoMapper;
using BLL.Repository;
using Microsoft.AspNetCore.Mvc;
using PL.DTOS;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(_mapper.Map<List<CategoryDTO>>(categories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(_mapper.Map<CategoryDTO>(category));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryDTO dto)
    {
        var category = _mapper.Map<DAL.Models.Category>(dto);
        await _categoryRepository.AddAsync(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, _mapper.Map<CategoryDTO>(category));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryDTO dto)
    {
        if (id != dto.Id) return BadRequest();
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        _mapper.Map(dto, category);
        await _categoryRepository.UpdateAsync(category);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        await _categoryRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        foreach (var category in categories)
            await _categoryRepository.DeleteAsync(category.Id);
        return NoContent();
    }
}
