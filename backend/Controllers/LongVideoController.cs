using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LongVideoController : BaseFileContentController<LongVideo, LongVideoDto, LongVideoService>
    {
        public LongVideoController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<LongVideo, LongVideoDto, LongVideoService>> logger, LongVideoService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {
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
                var videoEntity = _fileConentService.GetById(id);

                Guid? thumbnailId = videoEntity.ThumbnailId;

                //if (thumbnailId.IsNullOrEmpty())
                //{
                //    return NotFound();
                //}

                var thumbnailContentId = videoEntity.Thumbnail.ContentId;

                var imageStream = await _fileConentService.GetFileStreamAsync(thumbnailContentId);

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

        [Authorize(Roles = "Admin")]
        public async override Task<IActionResult> DeleteFileContent(Guid id)
        {
            try
            {
                await _fileConentService.DeleteLongVideo(id);
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
