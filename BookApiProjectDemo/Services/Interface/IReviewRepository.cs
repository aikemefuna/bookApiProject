using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IReviewRepository
    {
        ICollection<ReviewDto> GetAllReviews();
        Review GetReviewById(int reviewId);
        ICollection<Review> GetAllReviewsOfABook(int bookId);
        BookDto GetBookOfAReview(int reviewId);
        ReviewerDto GetReviewerOfAReview(int reviewid);
        bool ReviewExist(int reviewId);

        bool CreateReview(Review review);
        bool DeleteReview(Review review);
        bool UpdateReview(Review review);
        bool Save();
        bool DeleteReviews(List<Review> reviews);
    }
}
