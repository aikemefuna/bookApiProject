using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApiProjectDemo.ApiRequestModels;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using BookApiProjectDemo.Services.Interface;
using Microsoft.Extensions.Logging;

namespace BookApiProjectDemo.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly BookApiDbContext _countryContext;
        private readonly IMapper _mapper;
        

        public CountryRepository(BookApiDbContext countryContext, IMapper mapper)
        {
            _countryContext = countryContext;
            _mapper = mapper;
            
        }

        public bool CountryExist(int countryId)
        {
            return _countryContext.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            _countryContext.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _countryContext.Remove(country);
            //_mapper.Map< CountriesDto>(deletedCountry);
            
            return Save();
        }

        public ICollection<AuthorDto> GetAuthorsOfACountry(int countryId)
        {
           var countriesAuthor = _countryContext.Authors.Where(c => c.Country.Id == countryId).ToList();
            var countriesAuthorQuery = _mapper.Map<IList<AuthorDto>>(countriesAuthor);

            return countriesAuthorQuery;
        }

        public ICollection<CountriesDto> GetCountries()
        {
            var countries =  _countryContext.Countries.ToList();
            var listCountries = _mapper.Map<IList<CountriesDto>>(countries); 

            return listCountries;
        }

        public Country GetCountry(int countryId)
        {
            var country =_countryContext.Countries.SingleOrDefault(c => c.Id == countryId);
           // var specificCountry = _mapper.Map<CountriesDto>(country);

            return country;
        }

        public CountriesDto GetCountryOfAnAuthor(int authorId)
        {
             var authorCountry = _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).SingleOrDefault();
            var CountryOfAuthor = _mapper.Map<CountriesDto>(authorCountry);
            return CountryOfAuthor;
        }

        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country = _countryContext.Countries.SingleOrDefault(c => c.Name.Trim().ToUpper().Equals(countryName.Trim().ToUpper()) && c.Id != countryId);
            return country == null ? false : true;
        }

        public bool Save()
        {
            var IsSaved = _countryContext.SaveChanges();
            return IsSaved >= 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
             _countryContext.Update(country);
           
            return Save();
        }

      
    }
}
