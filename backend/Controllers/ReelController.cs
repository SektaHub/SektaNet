using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReelController(ILogger<ReelController> logger, ReelService reelService, ApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _reelService = reelService;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<ReelDto> Get()
        {
            var entities = _dbContext.Set<Reel>().ToList();
            var dtoList = _mapper.Map<List<ReelDto>>(entities);
            return dtoList;
        }

        [HttpGet("{videoId}", Name = "GetReel")]
        public IActionResult GetReel(Guid videoId)
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

        [HttpGet("RandomVideoId", Name = "GetRandomVideoId")]
        public IActionResult GetRandomVideoId()
        {
            try
            {
                var randomVideo = _dbContext.Set<Reel>().OrderBy(r => Guid.NewGuid()).FirstOrDefault();

                if (randomVideo == null)
                {
                    return NotFound("No videos found in the database.");
                }

                return Ok(randomVideo.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting random video ID: {ex.Message}");
                return StatusCode(500, "An error occurred while getting the random video ID.");
            }
        }

        [HttpGet("Test/{videoId}", Name = "GetReelTest")]
        public IActionResult GetReelTest(int videoId)
        {
            var videoPath = _reelService.GetReelPathTest(videoId);

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

        [HttpPost("upload")]
        public IActionResult UploadReel(IFormFile videoFile)
        {
            ReelDto reelDto = new ReelDto { 
            Id = Guid.NewGuid(),
                AudioTranscription = null,
                Duration = null,
            };

            if (videoFile == null || videoFile.Length == 0)
            {
                return BadRequest("No video file provided");
            }

            // Validate other properties in the ReelDto if needed

            try
            {
                _reelService.SaveVideo(reelDto, videoFile);
                return Ok("Video uploaded and saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                return StatusCode(500, "An error occurred while uploading the video");
            }
        }


    }
}
