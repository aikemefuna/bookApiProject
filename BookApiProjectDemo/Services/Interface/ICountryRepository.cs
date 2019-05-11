using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface ICountryRepository
    {
        ICollection<CountriesDto> GetCountries();
        CountriesDto GetCountry(int countryId);
        CountriesDto GetCountryOfAnAuthor(int authorId);
        ICollection<AuthorDto> GetAuthorsOfACountry(int countryId);
        bool CountryExist(int countryId);
        bool IsDuplicateCountryName(int countryId, string countryName);
    }
}
