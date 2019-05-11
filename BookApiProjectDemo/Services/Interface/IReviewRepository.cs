using BookApiProjectDemo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IReviewRepository
    {
        ICollection<ReviewDto> GetAllReviews();
        ReviewDto GetReviewById(int reviewId);
        ICollection<ReviewDto> GetAllReviewsOfABook(int bookId);
        BookDto GetBookOfAReview(int reviewId);
        ReviewerDto GetReviewerOfAReview(int reviewid);
        bool ReviewExist(int reviewId);
    }
}
