using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteAlohWork.Data.DTOs.CityDTOs;
using RemoteAlohWork.Data.Entities;
using RemoteAlohWork.Data.Repository;

namespace RemoteAlohWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        MyDbContext context = new MyDbContext();

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CityGetAllResponseDto> cities = context.Cities.Include(c => c.Country).Select(c => new CityGetAllResponseDto()
            {
                Id = c.Id,
                CityName = c.CityName,
                CountryName = c.Country.CountryName
            }).ToList();

            if (cities.Count > 0)
            {
                return Ok(cities);
            }
            else return BadRequest("There is no city in the database!");
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CityGetByIdResponseDto city = context.Cities.Include(c => c.Country).Select(c => new CityGetByIdResponseDto()
            {
                Id = c.Id,
                CityName = c.CityName,
                CountryName = c.Country.CountryName
            }).FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest($"Error !\n\nThere is no city with Id {id}");
            }
            else
            {
                //CityGetByIdResponseDto cityDto = 
                return Ok(city);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(CityCreateRequestDto cityyDto)
        {
            City city = new City()
            {
                CityName = cityyDto.CityName.Trim(),
                CountryId = cityyDto.CountryId
            };

            if (!context.Countries.Any(c => c.Id == city.CountryId))
            {
                return BadRequest($"Error !\n\nThere is no country with Id {city.CountryId}");
            }
            else
            {
                if (context.Cities.Any(c => c.CityName == city.CityName))
                {
                    return BadRequest($"Error !\n\nThere is already a city with name {city.CityName}");
                }
                else
                {
                    context.Cities.Add(city);
                    context.SaveChanges();
                    return Ok("Success.\n\nThe city has been added.");
                }
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            City city = context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest($"Error !\n\nThere is no city with id {id}");
            }
            else
            {
                context.Cities.Remove(city);
                context.SaveChanges();
                return Ok($"Success.\n\nThe {city.CityName} has been removed!");
            }

        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateById(int id, CityUpdateRequestDto cityDto)
        {
            if (context.Cities.Any(c => c.CityName.ToLower() == cityDto.CityName.ToLower().Trim()))
            {
                return BadRequest("There is a city with the same name !");
            }
            else
            {
                City city = context.Cities.Include(c => c.Country).FirstOrDefault(c => c.Id == id);
                if (city == null)
                {
                    return NotFound($"There is no city with Id {id}");
                }
                else
                {
                    city.CityName = cityDto.CityName;
                    city.CountryId = cityDto.CountryId;
                    context.SaveChanges();
                    return Ok(new CityUpdateResponseDto()
                    {
                        Id = city.Id,
                        CityName = city.CityName,
                        CountryName = city.Country.CountryName
                    });
                }
            }
        }
    }
}
