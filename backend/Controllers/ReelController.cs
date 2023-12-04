using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReelController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ReelService _reelService;

        public ReelController(ILogger<WeatherForecastController> logger, ReelService reelService)
        {
            _logger = logger;
            _reelService = reelService;
        }

        [HttpGet(Name = "GetRandomVideo")]
        public IActionResult Get()
        {
            // Replace the path with the actual path to your video file
            var videoPath = _reelService.GetRandomReel();
            var videoStream = new FileStream(videoPath, FileMode.Open);

            return File(videoStream, "video/mp4", enableRangeProcessing: true);
        }

    }
}
