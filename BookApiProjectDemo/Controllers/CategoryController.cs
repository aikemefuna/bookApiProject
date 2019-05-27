using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using BookApiProjectDemo.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }


        //api/categories
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof (IEnumerable<CategoryDto>))]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        } 


        //api/category/categoryId
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [HttpGet("{categoryId}", Name = "GetCategoryById")]

        public IActionResult GetCategory (int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = _categoryRepository.GetCategory(categoryId);
            try
            {
                if (category == null)
                {
                    return NotFound($"The Category with the category ID {categoryId}, is not found.");
                }
            }

            catch(Exception ex)
            {
                _logger.LogError("There was an error in GetCategory..", ex);
                return StatusCode(500);
            }
            var categoryDto = new CategoryDto() {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }


        //api/categories/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetAllCategoriesOfABook(int bookId)
        {
            var categories = _categoryRepository.GetAllCategoriesOfABook(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            return Ok(categories);
        }


        //api/categories/categoryId/books
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetAllBooksFromACategory(int categoryId)
        {
            var books = _categoryRepository.GetAllBooksOfACategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (books == null)
                {
                    return NotFound($"The Categories of the Selected book Id {categoryId}, cannot be retrived, because No Book with the BookId of {categoryId}, can be found.");
                }
            }

            catch
            {
                return StatusCode(500);
            }

            return Ok(books);
        }

        //api/countries
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(204)]
        public IActionResult CreateCategory([FromBody] Category categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);
            var category = _categoryRepository.GetCategories().FirstOrDefault(c => 
            c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper());

            if (category != null)
            {
                ModelState.AddModelError("",$"the category with name {categoryToCreate.Name}, cannot be created because theres is another category with the same name");
            }
            if (categoryToCreate.Name.Contains(",;'[]=:_-"))
            {
                ModelState.AddModelError("", "Please cross check the country name and try again");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ( !_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"There was an error creating {categoryToCreate.Name}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoryById", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }


        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category categoryToUpdate)
        {
            if (categoryToUpdate == null)
                return BadRequest(ModelState);

            if (categoryId != categoryToUpdate.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound($"The category ,with the Id of {categoryId}, cannot be found");
            if (_categoryRepository.IsDuplicateCategoryName(categoryId,categoryToUpdate.Name))
            {
                ModelState.AddModelError("", "Sorry, you are trying to update a category using a name that already exist");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_categoryRepository.UpdateCategory(categoryToUpdate))
            {
                ModelState.AddModelError("",$"there was an error updating the category with id of {categoryId} and name {categoryToUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //api/categories/categoryId
        [HttpDelete("{categoryId}")]
        public IActionResult DeleteCountry(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound($"the category with the Id of {categoryId},cannot be found.");

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (_categoryRepository.GetAllBooksOfACategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"The Action cannot be completed, there is atleast one books from the category");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"There was an error deleting {categoryToDelete.Name}");
            }
            return NoContent();
        }
    }
}
