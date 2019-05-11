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
        ReviewerDto GetReviewerById(int reviewerId);
        ReviewerDto GetReviewerOfAreview(int reviewId );
        ICollection<ReviewDto> GetAllReviewsByAReviewer(int reviewerId);
        bool ReviewerExist(int reviewerId);

    }
}
