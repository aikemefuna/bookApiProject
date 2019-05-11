using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApiProjectDemo.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength (40, ErrorMessage = "First Name can not be more than 40 characters, Please Check and try again")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "First Name can not be more than 40 characters, Please Check and try again")]
        public string LastName { get; set; }

        [Required]
        public Country Country { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }

}
