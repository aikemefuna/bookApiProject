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
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
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
        [HttpGet("{bookId}", Name = "GetBook")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBook(int bookId)
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
            catch (Exception ex)
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
        public IActionResult GetBook(string bookIsbn)
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

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Title = book.Title,
                DatePublished = book.DatePublished
            };
            return Ok(bookDto);
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



        private StatusCodeResult ValidateBook(List<int> authId, List<int> catId, Book book)
        {
            if (book == null || authId.Count <= 0 || catId.Count <= 0)
            {
                ModelState.AddModelError("", "missing book author or catgory");
                return BadRequest();
            }
            if (_bookRepository.IsDuplicateIsbn(book.Id, book.Isbn))
            {
                ModelState.AddModelError("", "Is Duplicate ISbn");
                return StatusCode(422);

            }

            foreach (var Id in authId)
            {
                if (!_authorRepository.AuthorExist(Id))
                {
                    ModelState.AddModelError("", "Author not found");
                    return StatusCode(404);
                }
            }

            foreach (var Id in catId)
            {
                if (!_categoryRepository.CategoryExist(Id))
                {
                    ModelState.AddModelError("", "Category not found");
                    return StatusCode(404);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Critical error");
                return BadRequest();
            }



            return NoContent();

        }


        //api/books?authId=1&authId=2&catId=1&catId=2
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Book))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public IActionResult CreateBook([FromQuery] List<int> authId, [FromQuery] List<int> catId, [FromBody] Book bookToCreate)
        {
            var statusCode = ValidateBook(authId, catId, bookToCreate);
            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (!_bookRepository.CreateBook(authId, catId, bookToCreate)) ;
            {
                ModelState.AddModelError("", $"Something went wrong saving the book {bookToCreate.Title}");
            }

            return CreatedAtRoute("GetBook", new { bookId = bookToCreate.Id }, bookToCreate);
        }


        //api/bookId?authId=1&authId=2&catId=1&catId=2
        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId,[FromQuery] List<int> authId, [FromQuery] List<int> catId, [FromBody] Book bookToUpdate)
        {
            var statusCode = ValidateBook(authId, catId, bookToUpdate);
            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (bookId != bookToUpdate.Id)
                return BadRequest();

            if (!_bookRepository.BookExist(bookId))
                return NotFound("book does no exist");

            if (!_bookRepository.UpdateBook(authId, catId, bookToUpdate)) 
            {
                ModelState.AddModelError("", $"Something went wrong Updating the book {bookToUpdate.Title}");
            }

            return NoContent();
        }


        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        //api/books/bookId
        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound($"reviewer with Id of {bookId}, cannot be found..");

            var bookToDelete = _bookRepository.GetBook(bookId);
            var bookReviewsToDelete = _reviewRepository.GetAllReviewsOfABook(bookId);

            if (!_bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting book..");
                return StatusCode(500, ModelState);
            }

            if (!_reviewRepository.DeleteReviews(bookReviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer..");
                return StatusCode(500, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }
    }
}
