using BookApiProjectDemo.DTO;
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
        [HttpGet("{categoryId}")]

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

            return Ok(category);
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
    }
}
