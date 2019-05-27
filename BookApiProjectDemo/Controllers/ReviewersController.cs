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
    public class ReviewersController : Controller 
    {
        private readonly IReviewerRepository _reviewersRepository;
        private readonly ILogger<ReviewersController> _logger;
        private readonly IReviewRepository _reviewRepository;

        public ReviewersController(IReviewerRepository reviewersRepository, ILogger<ReviewersController> logger, IReviewRepository reviewRepository)
        {
            _reviewersRepository = reviewersRepository;
            _logger = logger;
            _reviewRepository = reviewRepository;
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
        [HttpGet("{reviewerId}", Name = "GetReviewer")]
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
            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
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


        //api/reviewers
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public IActionResult CreateReviewer([FromBody] Reviewer reviewerToCreate)
        {
            if (reviewerToCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogDebug("Attempting to create the reviewer...........");

            if (!_reviewersRepository.CreateReviewer(reviewerToCreate))
            {
                ModelState.AddModelError("", "there was an error creating the reviewer...");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { reviewerid = reviewerToCreate.Id }, reviewerToCreate);            
        }


        //api/reviewers/reviewerId
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody]Reviewer reviewerToUpdate)
        {
            if (reviewerToUpdate == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewerToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewersRepository.ReviewerExist(reviewerId))
                return NotFound($"the reviewer with Id {reviewerId}, cannot be found");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewersRepository.UpdateReviewer(reviewerToUpdate)) ;
            {
                ModelState.AddModelError("", "Something went wrong Updating Reviewer..");
            }

            return NoContent();
        }


        //api/reviewers/reviewerId
        [HttpDelete("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewersRepository.ReviewerExist(reviewerId)) 
                return NotFound($"reviewer with Id of {reviewerId}, cannot be found..");

           var reviewerToDelete = _reviewersRepository.GetReviewerById(reviewerId);
            var reviewsByTheReviewerToDelete = _reviewersRepository.GetAllReviewsByAReviewer(reviewerId);

            if(!_reviewersRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer..");
                return StatusCode(500, ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewsByTheReviewerToDelete.ToList()))
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
