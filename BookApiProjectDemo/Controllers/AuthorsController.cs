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
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorsController> _logger;
        private readonly ICountryRepository _countryRepository;

        public AuthorsController(IAuthorRepository authorRepository,ILogger<AuthorsController> logger, ICountryRepository countryRepository)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _countryRepository = countryRepository;
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
        [HttpGet("{authorId}", Name = "GetAuthor")]
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
            var authorDto = new AuthorDto()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };

            return Ok(authorDto);
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


        //api/authors
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType (500)]
        [ProducesResponseType (404)]
        [ProducesResponseType(201, Type = typeof(Author))]

        public IActionResult CreateAuthor([FromBody] Author authorToCreate)
        {
            if (authorToCreate == null)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExist(authorToCreate.Country.Id))
            {
                ModelState.AddModelError("", $"Sorry Country doesnot exist.  does not exist.");
                return StatusCode(404, ModelState);
            }


            authorToCreate.Country = _countryRepository.GetCountry(authorToCreate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (!_authorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {authorToCreate.FirstName}{authorToCreate.LastName}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorToCreate.Id }, authorToCreate);
        }

        //api/authors/authorId
        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)] // no content
        public IActionResult UpdateAuthor (int authorId, [FromBody] Author UpdatedAuthorInfo)
        {
            if (UpdatedAuthorInfo == null)
                return BadRequest(ModelState);

            if (authorId != UpdatedAuthorInfo.Id)
                BadRequest(ModelState);

            if (!_countryRepository.CountryExist(UpdatedAuthorInfo.Country.Id))
                ModelState.AddModelError("", $"Sorry Country doesnot exist.  does not exist.");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            UpdatedAuthorInfo.Country = _countryRepository.GetCountry(UpdatedAuthorInfo.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_authorRepository.UpdateAuthor(UpdatedAuthorInfo))
            {
                ModelState.AddModelError("", "Error occured when updating Author");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


       // api/authors/authorId
        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]//no content
        public IActionResult DeleteAuthor (int authorId)
        {
            if (!_authorRepository.AuthorExist(authorId)) 
            return NotFound($"The author with the Id of {authorId}, is not found.");

            var authorTodelete = _authorRepository.GetAuthor(authorId);

            if (_authorRepository.GetAllBooksByAnAuthor(authorId).Count > 0)
            {
                ModelState.AddModelError("", $"the author with the name {authorTodelete.FirstName}{authorTodelete.LastName}," +
                    $" cannot be deleted because there are atleast one book by the Author.");
                return StatusCode(409, ModelState);
                    
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.DeleteAuthor(authorTodelete))
            {
                ModelState.AddModelError("", $"there was an error deleting author with the id of {authorId}, with name {authorTodelete.FirstName}{authorTodelete.LastName}");
            }

            return NoContent();
        }



    }
}
