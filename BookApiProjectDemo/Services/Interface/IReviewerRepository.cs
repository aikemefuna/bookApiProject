using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IReviewerRepository
    {
        ICollection<ReviewerDto> GetAllReviewers();
        Reviewer GetReviewerById(int reviewerId);
        ReviewerDto GetReviewerOfAreview(int reviewId );
        ICollection<Review> GetAllReviewsByAReviewer(int reviewerId);
        bool ReviewerExist(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer( Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();

    }
}
