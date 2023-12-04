using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReelController : ControllerBase
    {

        private readonly ILogger<ReelController> _logger;
        private readonly ReelService _reelService;

        public ReelController(ILogger<ReelController> logger, ReelService reelService)
        {
            _logger = logger;
            _reelService = reelService;
        }

        [HttpGet(Name = "GetRandomReel")]
        public IActionResult Get()
        {
            // Replace the path with the actual path to your video file
            var videoPath = _reelService.GetRandomReel();
            //var videoPath = "C:\\Users\\Borjan\\Documents\\GitHub\\SektaGram\\backend\\wwwroot\\Reels\\tateWorkout.mp4";
            var videoStream = new FileStream(videoPath, FileMode.Open);

            return File(videoStream, "video/mp4", enableRangeProcessing: true);
        }

        //[HttpGet("{videoId}", Name = "GetReel")]
        //public IActionResult GetReel(int videoId)
        //{
        //    // Generate a dynamic path or use a lookup mechanism to get the actual video path based on the videoId
        //    var videoPath = _reelService.GetReelPath(videoId);

        //    if (string.IsNullOrEmpty(videoPath))
        //    {
        //        return NotFound(); // Handle the case when the video is not found
        //    }

        //    var videoStream = new FileStream(videoPath, FileMode.Open);
        //    return File(videoStream, "video/mp4", enableRangeProcessing: true);
        //}

        [HttpGet("{videoId}", Name = "GetReel")]
        public IActionResult GetReel(int videoId)
        {
            var videoPath = _reelService.GetReelPath(videoId);

            if (string.IsNullOrEmpty(videoPath))
            {
                return NotFound();
            }

            try
            {
                var videoStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(videoStream, "video/mp4", enableRangeProcessing: true);
            }
            catch (IOException)
            {
                return StatusCode(500, "An error occurred while attempting to read the video file.");
            }
        }


    }
}
