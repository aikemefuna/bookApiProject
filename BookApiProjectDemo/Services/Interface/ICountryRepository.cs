using BookApiProjectDemo.ApiRequestModels;
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
         Country GetCountry(int countryId);
        CountriesDto GetCountryOfAnAuthor(int authorId);
        ICollection<AuthorDto> GetAuthorsOfACountry(int countryId);
        bool CountryExist(int countryId);
        bool IsDuplicateCountryName(int countryId, string countryName);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
       
    }
}
