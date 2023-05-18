using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteAlohWork.Data.DTOs.DistrictDTOs;
using RemoteAlohWork.Data.Entities;
using RemoteAlohWork.Data.Repository;

namespace RemoteAlohWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        MyDbContext context = new MyDbContext();

        [HttpGet]
        public IActionResult GetAll()
        {
            List<DistrictGetAllResponseDto> districts = context.Districts.Include(d => d.City).ThenInclude(c => c.Country).Select(d => new DistrictGetAllResponseDto()
            {
                Id = d.Id,
                DistrictName = d.DistrictName,
                Cityname = d.City.CityName,
                Countryname = d.City.Country.CountryName
            }).ToList();

            if (districts.Count > 0)
            {
                return Ok(districts);
            }
            else return BadRequest("There is no districts in the database!");
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            DistrictGetByIdResponseDto district = context.Districts.Include(d => d.City).ThenInclude(c => c.Country).
                Select(d => new DistrictGetByIdResponseDto()
            {
                Id = d.Id,
                DistrictName = d.DistrictName,
                Cityname = d.City.CityName,
                Countryname = d.City.Country.CountryName
            }).FirstOrDefault(d => d.Id == id);
            if (district == null)
            {
                return BadRequest($"Error !\n\nThere is no district with Id {id}");
            }
            else
            {
                //CityGetByIdResponseDto cityDto = 
                return Ok(district);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(DistrictCreateRequestDto districtDto)
        {
            District district = new District()
            {
                DistrictName = districtDto.DistrictName.Trim(),
                CityId = districtDto.CityId
            };

            if (!context.Cities.Any(c => c.Id == district.CityId))
            {
                return BadRequest($"Error !\n\nThere is no city with Id {district.CityId}");
            }
            else
            {
                if (context.Districts.Any(d => d.DistrictName == districtDto.DistrictName))
                {
                    return BadRequest($"Error !\n\nThere is already a district with name {district.DistrictName}");
                }
                else
                {
                    context.Districts.Add(district);
                    context.SaveChanges();
                    return Ok("Success.\n\nThe district has been added.");
                }
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            District district = context.Districts.FirstOrDefault(d => d.Id == id);
            if (district == null)
            {
                return BadRequest($"Error !\n\nThere is no district with id {id}");
            }
            else
            {
                context.Districts.Remove(district);
                return Ok($"Success.\n\nThe {district.DistrictName} has been removed!");
            }
        }
    }
}
