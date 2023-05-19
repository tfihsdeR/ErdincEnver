using Microsoft.AspNetCore.Mvc;
using RemoteAlohWork.Data.DTOs.CountryDTOs;
using RemoteAlohWork.Data.Entities;
using RemoteAlohWork.Data.Repository;
using System.Linq;

namespace RemoteAlohWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        MyDbContext context = new MyDbContext();

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CountryGetAllResponseDto> countries = context.Countries.Select(c => new CountryGetAllResponseDto()
            {
                Id = c.Id,
                CountryName = c.CountryName
            }).ToList();
            if (countries.Count != 0)
                return Ok(countries);
            else return BadRequest("There is no country in the database!");
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CountryGetByIdResponseDto country = context.Countries.Select(c => new CountryGetByIdResponseDto()
            {
                Id= c.Id,
                CountryName = c.CountryName
            }).FirstOrDefault(c => c.Id == id);

            if (country != null) return Ok(country);
            else return BadRequest("There is no country in the database!");
        }

        [HttpPost("create")]
        public IActionResult Create(CountryCreateRequestDto countryDto)
        {
            Country country = new Country()
            {
                CountryName = countryDto.CountryName.Trim()
            };
            if (context.Countries.FirstOrDefault(c => c.CountryName.ToLower() == country.CountryName.ToLower()) == null)
            {
                context.Countries.Add(country);
                context.SaveChanges();
                return Ok("Success.\n\nNew country has been created.");
            }
            else
            {
                return BadRequest($"Error !\n\n There is already a country with name {country.CountryName}");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            Country country = context.Countries.FirstOrDefault(c => c.Id == id);

            if (country != null)
            {
                context.Countries.Remove(country);
                context.SaveChanges();
                return Ok("Success.\n\nCountry has been deleted.");
            }
            else return BadRequest($"Error!\n\nThere is no country with id {id}");
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateById(int id, CountryUpdateRequestDto countryDto)
        {
            if (context.Countries.Any(c => c.CountryName.ToLower() == countryDto.CountryName.ToLower().Trim()))
            {
                return BadRequest("There is a country with the same name!");
            }
            else
            {
                Country country = context.Countries.FirstOrDefault(c => c.Id == id);
                if (country != null)
                {
                    country.CountryName = countryDto.CountryName.Trim();
                    context.SaveChanges();
                    return Ok(new CountryUpdateResponseDto()
                    {
                        Id = country.Id,
                        CountryName = countryDto.CountryName
                    });
                }
                else
                {
                    return BadRequest($"There is no country with Id {id}");
                }
            }
        }
    }
}
