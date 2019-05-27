using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
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

        public bool CreateReviewer(Reviewer reviewer)
        {
             _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public ICollection<ReviewerDto> GetAllReviewers()
        {
            var reviewers = _context.Reviewers.ToList();
            var reviewersQuery = _mapper.Map<IList<ReviewerDto>>(reviewers);

            return reviewersQuery;
        }

        public ICollection<Review> GetAllReviewsByAReviewer(int reviewerId)
        {
            var reviews = _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
            //var reviewsQuery = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return reviews;
        } 

        public Reviewer GetReviewerById(int reviewerId)
        {
            var reviewer = _context.Reviewers.SingleOrDefault(r => r.Id == reviewerId);
           // var reviewerQuery = _mapper.Map<ReviewerDto>(reviewer);

            return reviewer;
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

        public bool Save()
        {
            var IsSaved = _context.SaveChanges();
            return IsSaved > 0 ? true : false;
        }

        public bool UpdateReviewer( Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
