using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services
{
    public class ReviewerRepository : IReviewerRepository 
    {
        private readonly BookApiDbContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(BookApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<ReviewerDto> GetAllReviewers()
        {
            var reviewers = _context.Reviewers.ToList();
            var reviewersQuery = _mapper.Map<IList<ReviewerDto>>(reviewers);

            return reviewersQuery;
        }

        public ICollection<ReviewDto> GetAllReviewsByAReviewer(int reviewerId)
        {
            var reviews = _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
            var reviewsQuery = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return reviewsQuery;
        } 

        public ReviewerDto GetReviewerById(int reviewerId)
        {
            var reviewer = _context.Reviewers.SingleOrDefault(r => r.Id == reviewerId);
            var reviewerQuery = _mapper.Map<ReviewerDto>(reviewer);

            return reviewerQuery;
        }

        public ReviewerDto GetReviewerOfAreview(int reviewId)
        {
            var reviewerId = _context.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer.Id).SingleOrDefault();
            var reviewer = _context.Reviewers.SingleOrDefault(r => r.Id == reviewerId);
            var reviewerQuery = _mapper.Map<ReviewerDto>(reviewer);

            return reviewerQuery;
        }

        public bool ReviewerExist(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }
    }
}
