using AutoMapper;
using BookApiProjectDemo.ApiRequestModels;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
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
        private readonly IMapper _mapper;

        public CountriesController(ICountryRepository countriesRepository,ILogger<CountriesController> logger,IAuthorRepository authorRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
           _logger = logger;
            _authorRepository = authorRepository;
            _mapper = mapper;
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
        [HttpGet("{countryId}", Name = "GetCountryById")]
        public IActionResult GetCountryById (int countryId)

        {
                var country = _countriesRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_countriesRepository.CountryExist(countryId))
                {
                    return NotFound($"the country with the countryId {countryId} could not be found");
                }
                //if (country == null)
                //{
                //    return NotFound($"the country with the countryId {countryId} could not be found");
                //}
            }


            catch (Exception ex)
            {
                _logger.LogError("an error occured in GetCountryById", ex);

                return StatusCode(500);
            }
            var countrydto = new CountriesDto()
            {
                 Id = country.Id,
                 Name = country.Name
            };

            return Ok(countrydto);

        }




        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountriesDto))]
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


        //api/coountries
        [HttpPost]
        [ProducesResponseType(201, Type =  typeof(CountryRequest))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry ([FromBody]Country countryToCreate)
        {
            if (countryToCreate == null)
                return BadRequest(ModelState);

            var country = _countriesRepository.GetCountries().FirstOrDefault(c =>
                                c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper());

            if (country != null)
            {
                ModelState.AddModelError("", $"Oopss,Country {countryToCreate.Name}, already Exist.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countriesRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Somthing went wrong saving {countryToCreate.Name}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountryById", new { countryId = countryToCreate.Id }, countryToCreate);
            
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult Updatecountry(int countryId, [FromBody] Country UpdatedCountryInfo)
        {

            if (UpdatedCountryInfo == null)
                return BadRequest(ModelState);


            if (countryId != UpdatedCountryInfo.Id)
                return BadRequest(ModelState);

            if (!_countriesRepository.CountryExist(countryId))
                return NotFound($"the country with the id {countryId}passed was not found");

            if (_countriesRepository.IsDuplicateCountryName(countryId, UpdatedCountryInfo.Name))
            {
                ModelState.AddModelError("", $"Country {UpdatedCountryInfo.Name}, already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_countriesRepository.UpdateCountry(UpdatedCountryInfo))
            {
                ModelState.AddModelError("", $"Somthing went wrong saving {UpdatedCountryInfo.Name}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

          //api/countries/countryId
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public IActionResult DeleteCountry(int countryId)
        {
            //if (deletedCountryInfo == null)
               // return BadRequest(ModelState);

            if (!_countriesRepository.CountryExist(countryId))
                return NotFound($"the country with the Id {countryId}, can not be found.");

            var  countryToDelete = _countriesRepository.GetCountry(countryId);

            //if (countryId != deletedCountryInfo.Id)
             //   return BadRequest(ModelState);
            

            if (_countriesRepository.GetAuthorsOfACountry(countryId).Count() > 0 )
            {
                ModelState.AddModelError("", $"Sorry the country {countryToDelete.Name}, can not be deleted because there are at least 1 author from it.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countriesRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Name}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
