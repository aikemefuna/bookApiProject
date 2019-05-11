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
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger)
        {
           _bookRepository = bookRepository;
           _logger = logger;
        }

        //api/books
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetAllBook()
        {
            var books = _bookRepository.GetAllBooks().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(books);
        }

        //api/books/bookId
      [HttpGet("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookById(int bookId)
        {
            var book = _bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_bookRepository.BookExist(bookId))
                {
                    return NotFound($"The book with the Id of {bookId}, can not be found");
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("There was an error in GetBookById", ex);
                return StatusCode(500);
            }
            return Ok(book);
        }

        //api/books/isbn/booksIsbn
        [HttpGet("ISBN/{bookIsbn}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBookByIsbn(string bookIsbn)
        {
            var book = _bookRepository.GetBook(bookIsbn);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_bookRepository.BookExist(bookIsbn))
                {
                    return NotFound($"The book with the ISBN of {bookIsbn}, can not be found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("There was an error in GetBookById", ex);
               // return StatusCode(500);
            }
            return Ok(book);
        }


        //api/books/bookId/rating
        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public IActionResult GetBookRating(int bookId)
        {
            if (!_bookRepository.BookExist(bookId)) 
            return NotFound($"the book id of {bookId}, was not founmd and the rating can not be calculated.");

            var rating = _bookRepository.GetBookRating(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogDebug($"the rating for the book with the Id of {bookId}, is {rating}");
            return Ok(rating);
        }
    }
}
