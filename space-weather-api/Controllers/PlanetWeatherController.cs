using Microsoft.AspNetCore.Mvc;
using space_weather_api.Models;
using space_weather_api.Services;

namespace space_weather_api.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class PlanetWeatherController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpPost]
        public IActionResult Post(PlanetModel planet)
        {
            PlanetForecast newPlanet = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = planet.Name,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            PlanetService.PlanetList.Add(newPlanet);
            
            return CreatedAtAction(nameof(Get), new { id = newPlanet.Id }, newPlanet);
        }

        [HttpGet]
        public IActionResult Get(string sort = "asc", int page = 1, bool status = true)
        {
            var planets = PlanetService.PlanetList;

            planets = status ? planets.FindAll(p => p.IsActive) : planets.FindAll(p => !p.IsActive);

            planets = sort == "asc" ? planets.OrderBy(p => p.CreatedAt).ToList() : planets.OrderByDescending(p => p.CreatedAt).ToList();

            var pageSize = 3;
            var skip = (page - 1) * pageSize;
            var pagedPlanets = planets.Skip(skip).Take(pageSize).ToList();

            return Ok(pagedPlanets);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var planet = PlanetService.PlanetList.Find(p => p.Id == id);
            if (planet == null)
            {
                return NotFound();
            }
            return Ok(planet);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, PlanetModel planet)
        {
            var planetToUpdate = PlanetService.PlanetList.Find(p => p.Id == id);
            if (planetToUpdate == null)
            {
                return NotFound();
            }
            planetToUpdate.Name = planet.Name;
            return Ok(planetToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var planetToDelete = PlanetService.PlanetList.Find(p => p.Id == id);
            if (planetToDelete == null)
            {
                return NotFound();
            }
            PlanetService.PlanetList.Remove(planetToDelete);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, bool activeStatus)
        {
            var planetToUpdate = PlanetService.PlanetList.Find(p => p.Id == id);
            if (planetToUpdate == null)
            {
                return NotFound();
            }
            planetToUpdate.IsActive = activeStatus;
            return Ok(planetToUpdate);
        }
    }
}
