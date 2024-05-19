using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReelController : BaseFileContentController<Reel, ReelDto, ReelService>
    {

        public ReelController(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Reel, ReelDto, ReelService>> logger, ReelService fileConentService, MongoDBRepository mongoDBRepo, AnyFileRepository anyFileRepository) : base(mapper, webHostEnvironment, logger, fileConentService)
        {

        }


        [HttpGet("GetReelsWithoutTranscription")]
        [Authorize(Roles = "Admin")]
        public IQueryable<ReelDto> GetReelsWithoutTranscription()
        {
            return _fileConentService.GetReelsWithoutTranscription();
        }

        [HttpGet("RandomVideoId", Name = "GetRandomVideoId")]
        public IActionResult GetRandomVideoId()
        {
            try
            {
                var randomVideo = _fileConentService.GetRandomVideo();

                if (randomVideo == null)
                {
                    return NotFound("No videos found in the database.");
                }

                return Ok(new { id = randomVideo.Id });
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


        //[HttpPost("upload")]
        //public async Task<IActionResult> Upload(IFormFile videoFile)
        //{
        //    if (videoFile == null || videoFile.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    try
        //    {
                
        //        // Assuming you've injected MongoDBService as _mongoDBService
        //        var fileId = await _fileConentService.UploadReel(videoFile);

        //        // Here you can link fileId with your reel entity if necessary

        //        return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error uploading video: {ex.Message}");
        //        return StatusCode(500, "An error occurred while uploading the video.");
        //    }
        //}


        [HttpGet("{videoId}/Content")]
        [AllowAnonymous]
        public async override Task<IActionResult> GetFileContent(Guid videoId) // Assuming videoId is the string representation of MongoDB's ObjectId
        {
            //if (string.IsNullOrEmpty(videoId))
            //{
            //    return NotFound();
            //}

            var videoEntity = _fileConentService.GetById(videoId);

            try
            {
                // Assuming `_mongoDBService` is already injected and accessible in your controller
                var videoStream = await _fileConentService.GetFileStreamAsync(videoEntity.ContentId);

                if (videoStream.Length == 0)
                {
                    return NotFound();
                }

                // Return the video stream, enabling range processing for bufferable streaming
                return File(videoStream, "video/mp4", enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while attempting to read the video file: {ex.Message}");
                return StatusCode(500, "An error occurred while attempting to read the video file.");
            }
        }

        [HttpGet("{id}/Thumbnail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetThumbnail(Guid id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            try
            {
                var reelEntity = _fileConentService.GetById(id);

                string? thumbnailId = reelEntity.Thumbnail.ContentId;

                if (thumbnailId.IsNullOrEmpty())
                {
                    return NotFound();
                }

                var imageStream = await _fileConentService.GetFileStreamAsync(thumbnailId);

                if (imageStream.Length == 0)
                {
                    return NotFound();
                }

                // Return the image stream
                return File(imageStream, $"image/jpeg"); // Adjust the content type based on your image format
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while attempting to read the image file: {ex.Message}");
                return StatusCode(500, "An error occurred while attempting to read the image file.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async override Task<IActionResult> DeleteFileContent(Guid id)
        {
            try
            {
               await _fileConentService.DeleteReel(id);
               return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting reel: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the reel or thumbnail");
            }
        }


    }
}
