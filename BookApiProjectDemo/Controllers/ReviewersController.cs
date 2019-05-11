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
    public class ReviewersController : Controller 
    {
        private readonly IReviewerRepository _reviewersRepository;
        private readonly ILogger<ReviewersController> _logger;

        public ReviewersController(IReviewerRepository reviewersRepository, ILogger<ReviewersController> logger)
        {
            _reviewersRepository = reviewersRepository;
            _logger = logger;
        }


        //api/reviewers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof (IEnumerable<ReviewerDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllReviewers()
        {
           var reviewers =  _reviewersRepository.GetAllReviewers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }

        //api/reviewer/reviewerid
        [HttpGet("{reviewerId}")]
        public IActionResult GetReviewersById (int reviewerId)
        {
            var reviewer = _reviewersRepository.GetReviewerById(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (!_reviewersRepository.ReviewerExist(reviewerId))
                {
                    return NotFound($"the reviewer with the reviewerId {reviewerId}, cannot be found.");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError("There was an error in GetReviewerbyId", ex);
                return StatusCode(500);
            }

            return Ok(reviewer);
        }


        //ap1/reviewer/reviwerId/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType (404)]
        [ProducesResponseType (400)]
        [ProducesResponseType (200, Type = typeof(IEnumerable<ReviewDto>))]

        public IActionResult GetAllReviewsByAReviewer(int reviewerId)
        {
            var reviews = _reviewersRepository.GetAllReviewsByAReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (reviews == null)
                {
                    return NotFound($"the reviews for reviewer with the reviewerId {reviewerId}, cannot be found.");
                }
            }

            catch(Exception ex)
            {
                _logger.LogError("there was an error in GetAllReviewsByAReviwer",ex);
                return StatusCode(500);
            }

            return Ok(reviews);
        }

        //api/reviewers/reviews/reviewId
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        
        [HttpGet("reviews/{reviewId}")]
        public IActionResult GetReviewerOfAreview(int reviewId)
        {
            var reviewer = _reviewersRepository.GetReviewerOfAreview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (reviewer == null)
                {
                    return NotFound($"The reviewer of a reviewId of {reviewId}, cannot be found.");
                }
            }

            catch (Exception ex)
            {
                _logger.LogWarning("there was an error in GeteviewerOfAReview.",ex);
                return StatusCode(500);
            }

            return Ok(reviewer);
        }
    }
}
