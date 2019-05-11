using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Review Headline can not be less than 10 and more than 200 characters.")]
        public string HeadLine  { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 50, ErrorMessage = "Review Text must be between 50 and 2000 characters")]
        public string ReviewText { get; set; }

        [Required]
        [Range(1,5,ErrorMessage = "Rating Must be between 1 and  5 stars")]
        public int Rating { get; set; }
        public Book Book { get; set; }
        public Reviewer Reviewer { get; set; }
    }
}
