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

        private readonly FfmpegService _ffmpegService;

        public ReelController(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Reel, ReelDto, ReelService>> logger, ReelService fileConentService, FfmpegService ffmpegService) : base(dbContext, mapper, webHostEnvironment, logger, fileConentService)
        {
            _ffmpegService = ffmpegService;
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
            // ...omitting initial checks and try-catch for brevity

            List<string> videoPaths = new List<string>();
            List<ReelDto> reelDtos = new List<ReelDto>();

            foreach (var videoFile in videoFiles)
            {
                if (videoFile == null || videoFile.Length == 0) continue;

                var reelId = Guid.NewGuid();

                var reelDto = new ReelDto
                {
                    Id = reelId,
                    FileExtension = videoFile.ContentType.Split('/')[1],
                    AudioTranscription = null,
                    Duration = null, // Will be set after getting duration
                };
                reelDtos.Add(reelDto);

                var videoPath = await _fileConentService.SaveFile(videoFile, reelId, reelDto.FileExtension);
                videoPaths.Add(videoPath);
            }

            // Ensure all files are saved before proceeding
            for (int i = 0; i < videoPaths.Count; i++)
            {
                var reelDto = reelDtos[i];
                var videoPath = videoPaths[i];

                // Set the duration of the video
                reelDto.Duration = await _ffmpegService.GetVideoDurationAsync(videoPath);

                // Save the Reel entity to the database
                var newReel = _mapper.Map<Reel>(reelDto);
                _dbContext.Reels.Add(newReel);
            }
            _dbContext.SaveChanges();

            // Now generate thumbnails
            var thumbnailTasks = reelDtos.Select(reelDto =>
                _ffmpegService.ExtractThumbnailAsync(_fileConentService.GetFilePath(reelDto.Id), Path.Combine(_env.WebRootPath, "Thumbnails"))
            ).ToList();

            await Task.WhenAll(thumbnailTasks);

            return Ok(new { Message = "Videos uploaded and saved successfully", Reels = reelDtos });
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
