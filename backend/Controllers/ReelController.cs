using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReelController : BaseFileContentController<Reel, ReelDto, ReelService>
    {

        MongoDBService _mongoDBService;

        public ReelController(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Reel, ReelDto, ReelService>> logger, ReelService fileConentService, MongoDBService mongoDBService) : base(dbContext, mapper, webHostEnvironment, logger, fileConentService)
        {
            _mongoDBService = mongoDBService;
        }


        [HttpGet("{videoId}/Content")]
        public override IActionResult GetFileContent(Guid videoId)
        {
            var videoPath = _fileConentService.GetFilePath(videoId);

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

        [HttpGet("GetReelsWithoutTranscription")]
        public IQueryable<ReelDto> GetReelsWithoutTranscription()
        {
            // Filter images where GeneratedCaption is null
            var filteredEntities = _dbContext.Set<Reel>()
                .Where(reel => reel.AudioTranscription == null)
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ReelDto>(filteredEntities);
            return filteredDtoList;
        }

        [HttpGet("{videoId}/Thumbnail", Name = "GetReelThumbnail")]
        public IActionResult GetReelThumbnail(Guid videoId)
        {
            var videoPath = _fileConentService.GetFilePath(videoId);
            var outputPath = Path.Combine(_env.WebRootPath, "Thumbnails");

            if (string.IsNullOrEmpty(videoPath))
            {
                return NotFound();
            }

            // Assume the thumbnail file has the same name as the video with a .jpg extension
            var thumbnailFileName = $"{videoId}.jpg";
            var thumbnailPath = Path.Combine(outputPath, thumbnailFileName);

            if (System.IO.File.Exists(thumbnailPath))
            {
                try
                {
                    // Open the file stream without closing it immediately
                    var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                    // Return the file stream directly as the response
                    return File(thumbnailStream, "image/jpeg");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accessing thumbnail file: {ex.Message}");
                    return StatusCode(500, "An error occurred while attempting to read the thumbnail file.");
                }
            }
            else
            {
                return NotFound("Thumbnail not found");
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

        [HttpPost("upload-multiple")]
        public override async Task<IActionResult> UploadMultiple(List<IFormFile> videoFiles)
        {
            var reelDtos = await _fileConentService.UploadMultiple(videoFiles);

            return Ok(new { Message = "Videos uploaded and saved successfully", Reels = reelDtos });
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = videoFile.OpenReadStream())
                {
                    // Assuming you've injected MongoDBService as _mongoDBService
                    var fileId = await _mongoDBService.UploadFileAsync(stream, videoFile.FileName);

                    // Here you can link fileId with your reel entity if necessary

                    return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                return StatusCode(500, "An error occurred while uploading the video.");
            }
        }


        [HttpDelete("{videoId}")]
        public override IActionResult DeleteFileContent(Guid videoId)
        {
            base.DeleteFileContent(videoId);
            try
            {
                // Delete thumbnail file
                var outputPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                var thumbnailFileName = $"{videoId}.jpg";
                var thumbnailPath = Path.Combine(outputPath, thumbnailFileName);
                if (System.IO.File.Exists(thumbnailPath))
                {
                    System.IO.File.Delete(thumbnailPath);
                }
                return Ok("Reel and thumbnail deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting reel: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the reel or thumbnail");
            }
        }



    }
}
