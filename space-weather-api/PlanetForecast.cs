using space_weather_api.Models;

namespace space_weather_api
{
    public class PlanetForecast : PlanetModel
    {

        public required string Id { get; set; }
        public int TemperatureC { get; set; }
        public required string Summary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
