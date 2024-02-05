using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Workintech02RestApiDemo.Business.City;
using Workintech02RestApiDemo.Domain.Entities;

namespace Workintech02RestApiDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [Authorize(Roles="admin,employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _cityService.GetCities();
            Log.Logger.Information("Cities are fetched. @{cities}",cities);
            return Ok(cities);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var city = _cityService.GetById(id);
            if (city == null)
            {
                Log.Logger.Warning("City is not found. @{id}",id);
                return NotFound();
            }
            Log.Logger.Information("City is fetched. @{city}",city);
            return Ok(city);
        }

        [HttpPost]
        public City AddCity(Workintech02RestApiDemo.Domain.Entities.City city)
        {
            var addedCity = _cityService.AddCity(city);
            Log.Logger.Information("City is added. @{city}",city);
            return addedCity;
        }
    }
}
