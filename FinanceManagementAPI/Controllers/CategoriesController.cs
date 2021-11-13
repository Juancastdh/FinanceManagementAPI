using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.Category;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categories = CategoriesManager.GetAllCategories();

            IEnumerable<CategoryReadDto> categoryReadDtos = Mapper.Map<IEnumerable<CategoryReadDto>>(categories);

            return Ok(categoryReadDtos);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto category)
        {
            Category categoryToCreate = Mapper.Map<Category>(category);

            CategoriesManager.AddCategory(categoryToCreate);

            return Ok();
        }
    }
}
