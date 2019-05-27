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
        [HttpGet("{reviewId}", Name = "GetReviewById")]
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
            var reviewDto = new ReviewDto() {
                Id = review.Id,
                HeadLine = review.HeadLine,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };

            return  Ok( reviewDto);
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
            var reviewsDto = new List<ReviewDto>();

            foreach (var review in AllReviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                     HeadLine = review.HeadLine,
                      Rating = review.Rating,
                       ReviewText = review.ReviewText
                    
                });
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

        //api/reviews
        [HttpPost]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult CreateReview([FromBody]Review reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExist(reviewToCreate.Reviewer.Id))
                ModelState.AddModelError("", $"Sorry reviewer does not exist.");

            if (!_bookRepository.BookExist(reviewToCreate.Book.Id))
                ModelState.AddModelError("", $"Sorry Book  does not exist.");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewToCreate.Book = _bookRepository.GetBook(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewerById(reviewToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", "there was an error creating Review");
                return StatusCode(500, ModelState);
            }

           
            return CreatedAtRoute("GetReviewById", new { reviewId = reviewToCreate.Id }, reviewToCreate);
        }



        //api/reviews/reviewId
        [HttpPut("{reviewId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview (int reviewId, [FromBody] Review reviewtoUpdate)
        {
            if (reviewtoUpdate == null)
                return BadRequest(ModelState);


            if (reviewId != reviewtoUpdate.Id)
                return BadRequest(ModelState);


            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound($"the review with the Id {reviewId}, cannot be found.");

            if (!_reviewerRepository.ReviewerExist(reviewtoUpdate.Reviewer.Id))
                return NotFound("the reviewer for that review can no be found, hence you review cannot be updated");

            if (!_bookRepository.BookExist(reviewtoUpdate.Book.Id))
                return NotFound("the book for that review can not be found, hence you review cannot be updated");

            if (!ModelState.IsValid)
                return StatusCode(404,ModelState);

            reviewtoUpdate.Book = _bookRepository.GetBook(reviewtoUpdate.Book.Id);
            reviewtoUpdate.Reviewer = _reviewerRepository.GetReviewerById(reviewtoUpdate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogDebug("attempting to update the review.......");

            if (!_reviewRepository.UpdateReview(reviewtoUpdate))
            {
                ModelState.AddModelError("", "There was an error updating your review.");
            }

            _logger.LogDebug("review updated Succesfully...");
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)] 
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound($"the review with Id of {reviewId}, is not found.");

            var reviewToDelete = _reviewRepository.GetReviewById(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"there was an error Deleting the Review..");
            }
            return NoContent();
        }

    }
}
