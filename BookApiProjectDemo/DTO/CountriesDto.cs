using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.DTO
{
    [Table("Country")]
    public class CountriesDto
    {
       
        public int Id { get; set; }
        public string Name{ get; set; }
    }
}
