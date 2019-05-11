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
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewsController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewsController(IReviewRepository reviewRepository, ILogger<ReviewsController> logger, IBookRepository bookRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _bookRepository = bookRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]

        public IActionResult GetAllReviews()
        {
            var reviews = _reviewRepository.GetAllReviews();
            return Ok(reviews);
        }

        //api/reviews/reviewId
        [HttpGet("{reviewId}")]
        public IActionResult GetReviewByid(int reviewId)
        {
            var review = _reviewRepository.GetReviewById(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (review == null)
                {
                    return NotFound($"the review with the reviewId of {reviewId}, can not be found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("there was an error in getting GetReviewById", ex);
                return StatusCode(500);
            }

            return  Ok( review);
        }



        //api/reviews/booksId/reviews
        [HttpGet("books/{bookId}")]
        public IActionResult GetAllReviewsForABook(int bookId)
        {
            var AllReviews = _reviewRepository.GetAllReviewsOfABook(bookId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
            if (!_bookRepository.BookExist(bookId))
                {
                    if (AllReviews == null)
                    {
                        return NotFound($"the reviews for the book id {bookId}, can not be found");
                    }
                    return NotFound("Book Not Found");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("there was an error in GetAllReviewsForAbook", ex);
                return StatusCode(500);
            }

            return Ok(AllReviews);
        }



        //api/reviews/reviewId/book
        [HttpGet("{reviewid}/book")]
        public IActionResult GetBookOfAReview (int reviewId)
        {
            var book = _reviewRepository.GetBookOfAReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (book == null)
                {
                    return NotFound($"the book with  the reviewId {reviewId}, can not be found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("there was an error in GetAllReviewsForAbook", ex);
                return StatusCode(500);
            }

            return Ok(book);
        }


        //api/reviews/reviewId/reviewer
        [HttpGet("{reviewId}/reviewer")]
        public IActionResult GetReviewerOfAReview(int reviewid)
        {
            var reviewer = _reviewRepository.GetReviewerOfAReview(reviewid);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (reviewer == null)
                {
                    return NotFound("Reviewer not found");
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("there was an error",ex);
                return StatusCode(500);
            }
            return Ok(reviewer);
        }
    }
}
