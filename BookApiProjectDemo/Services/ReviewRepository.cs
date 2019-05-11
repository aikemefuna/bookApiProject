using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookApiDbContext _reviewContext;
        private readonly IMapper _mapper;

        public ReviewRepository(BookApiDbContext reviewContext, IMapper mapper)
        {
            _reviewContext = reviewContext;
            _mapper = mapper;
        }
        public ICollection<ReviewDto> GetAllReviews()
        {
            var reviews = _reviewContext.Reviews.OrderBy(r => r.Rating).ToList();
            var reviewsQuery = _mapper.Map<IList<ReviewDto>>(reviews);
            return reviewsQuery;
        }

        public ICollection<ReviewDto> GetAllReviewsOfABook(int bookId)
        {
            var reviews = _reviewContext.Reviews.Where(b => b.Book.Id == bookId).ToList();
            var reviewsQuery = _mapper.Map<ICollection<ReviewDto>>(reviews);
            return reviewsQuery;
        }

        public BookDto GetBookOfAReview(int reviewId)
        {
            var bookId = _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(b => b.Book.Id).SingleOrDefault();
            var book = _reviewContext.Books.SingleOrDefault(b => b.Id == bookId);
            var bookOfReview = _mapper.Map<BookDto>(book);
            return bookOfReview;
        }

        public ReviewDto GetReviewById(int reviewId)
        {
            var review = _reviewContext.Reviews.SingleOrDefault(r => r.Id == reviewId);
            var reviewQuery = _mapper.Map<ReviewDto>(review);

            return reviewQuery;
        }

        public ReviewerDto GetReviewerOfAReview(int reviewid)
        {
            var reviewerId = _reviewContext.Reviews.Where(r => r.Id == reviewid).Select(rr => rr.Reviewer.Id).SingleOrDefault();
            var reviewerQuery = _reviewContext.Reviewers.SingleOrDefault(rr => rr.Id == reviewerId);
            var reviewer = _mapper.Map<ReviewerDto>(reviewerQuery);

            return reviewer;
            
        }

        public bool ReviewExist(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
