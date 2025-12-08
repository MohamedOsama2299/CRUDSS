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
        try
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(categories));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving categories", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            return Ok(_mapper.Map<CategoryDTO>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving category", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data", errors = ModelState });

            var category = _mapper.Map<DAL.Models.Category>(dto);
            await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetCategoryById),
                new { id = category.Id },
                _mapper.Map<CategoryDTO>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating category", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryDTO dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch between request and body" });

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            _mapper.Map(dto, category);
            await _categoryRepository.UpdateAsync(category);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating category", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting category", details = ex.Message });
        }
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAllCategories()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync();

            foreach (var category in categories)
                await _categoryRepository.DeleteAsync(category.Id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting all categories", details = ex.Message });
        }
    }
}
