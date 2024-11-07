using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LivrosController : ControllerBase
    {

        public LivrosController()
        {

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            return "";
        }
    }
}
