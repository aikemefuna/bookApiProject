using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApiProjectDemo.Entities
{
    public class Reviewer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "First Name can not be more than 100 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Last Name can not be more than 100 characters.")]
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
