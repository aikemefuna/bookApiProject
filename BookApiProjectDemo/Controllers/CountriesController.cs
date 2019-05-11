using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;
        private readonly IAuthorRepository _authorRepository;

        public CountriesController(ICountryRepository countriesRepository,ILogger<CountriesController> logger,IAuthorRepository authorRepository)
        {
            _countriesRepository = countriesRepository;
           _logger = logger;
            _authorRepository = authorRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type = typeof(IEnumerable<CountriesDto>) )]
        public IActionResult GetAllCountries()
        {
            try
            {
                var countries = _countriesRepository.GetCountries().ToList();
                return Ok(countries);
            }


            catch (Exception ex)
            {
                _logger.LogDebug("There was an error while running the GetAllCountries Action..", ex);
                return StatusCode(400);
            }    
        }

        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountriesDto))]
        //api/countries/id
        [HttpGet("{countryId}")]
        public IActionResult GetCountryById (int countryId)

        {
                var country = _countriesRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //if (!_countriesRepository.CountryExist(countryId))
                //{
                //    return NotFound($"the country with the countryId {countryId} could not be found");
                //}
                if (country == null)
                {
                    return NotFound($"the country with the countryId {countryId} could not be found");
                }
            }


            catch (Exception ex)
            {
                _logger.LogError("an error occured in GetCountryById", ex);

                return StatusCode(500);
            }

            return Ok(country);

        }



        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountriesDto))]

        [HttpGet("authors/{authorId}")]
        public IActionResult GetCountryOfAnAuthor (int authorId)
        {
            var authorCountry = _countriesRepository.GetCountryOfAnAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (!_authorRepository.AuthorExist(authorId))
               {
                    if (authorCountry == null)
                    {
                        return NotFound($"the author with the authorId {authorId} could not be found, therefore the country can not be retrieved");
                    }
                  return NotFound();
               }
            }

            catch (Exception ex)
            {
                _logger.LogError("There was an error in the GetCountryOfAnAuthor", ex);
                return StatusCode(500);

            }

            return Ok(authorCountry);
        }



        //api/countries/countryId/Authors
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetAuthorsOfACountry(int countryId)
        {
            //if (!_countriesRepository.CountryExist(countryId))
            //    return NotFound();
            var authors = _countriesRepository.GetAuthorsOfACountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var authorsDto = new List<AuthorDto>();

            //foreach (var author in authors)
            //{
            //    authorsDto.Add(new AuthorDto
            //    {
            //        Id = author.Id,
            //        FirstName = author.FirstName,
            //        LastName = author.LastName
            //    });
            //}

            
            try
            {
                if (authors == null)
                    return NotFound($"the country with the countryId {countryId} could not be found, therefore the Authors  can not be retrieved");
            }

            catch (Exception ex)
            {
                _logger.LogDebug("There was an error in GetauthorsOfACountry", ex);
                return StatusCode(500);
            }

            return Ok(authors);
        }
    }
}
