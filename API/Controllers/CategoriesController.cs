using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.Categories;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesManager CategoriesManager;
        private readonly IMapper Mapper;

        public CategoriesController(ICategoriesManager categoriesManager, IMapper mapper)
        {
            CategoriesManager = categoriesManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryReadDto>), 200)]
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categories = CategoriesManager.GetAllCategories();

            IEnumerable<CategoryReadDto> categoryReadDtos = Mapper.Map<IEnumerable<CategoryReadDto>>(categories);

            return Ok(categoryReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryReadDto), 201)]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto category)
        {
            Category categoryToCreate = Mapper.Map<Category>(category);

            CategoriesManager.AddCategory(categoryToCreate);

            CategoryReadDto categoryReadDto = Mapper.Map<CategoryReadDto>(categoryToCreate);

            return CreatedAtRoute("GetCategoryById", new { id = categoryReadDto.Id }, categoryReadDto);
            
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(typeof(CategoryReadDto), 200)]
        public IActionResult GetCategoryById(int id)
        {
            Category category = CategoriesManager.GetCategoryById(id);
            
            CategoryReadDto categoryReadDto = Mapper.Map<CategoryReadDto>(category);

            return Ok(categoryReadDto);

        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateCategory([FromBody] CategoryReadDto category)
        {
            Category categoryToBeUpdated = Mapper.Map<Category>(category);

            CategoriesManager.UpdateCategory(categoryToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteCategoryById(int id)
        {
            CategoriesManager.DeleteCategoryById(id);

            return Ok();
        }
    }
}
