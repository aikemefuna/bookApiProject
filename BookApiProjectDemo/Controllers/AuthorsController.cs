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
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorRepository authorRepository,ILogger<AuthorsController> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        //api/authors
        [HttpGet]
        [ProducesResponseType (200, Type = typeof(IEnumerable<AuthorDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAllAuthors().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(authors);
        }



        //api/authors/authorId
        [HttpGet("{authorId}")]
        public IActionResult GetAuthorById(int authorId)
        {
            var author = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (author == null)
                {
                    return NotFound("the author with the authorId {authorId}, can not be found");
                }
            }

            catch(Exception ex)
            {
                _logger.LogDebug("thee was an ero in GetAuhor", ex);
                return StatusCode(500);
            }

            return Ok(author);
        }


        //api/authors/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof (IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsOfABook(int bookId)
        {
            var authors = _authorRepository.GetAllAuthorsOfABook(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (authors == null)
                {
                    return NotFound($"the authors for the book with the Id of {bookId}, can not be found");
                }
            }

            catch(Exception ex)
            {
                _logger.LogError("there was an error in GetAuthorsofABook",ex);
                return StatusCode(500);
            }

            return Ok(authors);
        }

       
        //api/authors/authorId/books
        [HttpGet("{authorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooksofAnAuthor(int authorId)
        {
            var books = _authorRepository.GetAllBooksByAnAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (books == null)
                {
                    return NotFound($"the books by the Author with the Id of {authorId}, can not be found");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError("there was an error in GetAuthorsofABook", ex);
                return StatusCode(500);
            }

            return Ok(books);
        }
    }
}
