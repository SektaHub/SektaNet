using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Pgvector;
using backend.Controllers.Common;
using Microsoft.AspNetCore.JsonPatch;
using System.Globalization;
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using backend.Repo;
using Microsoft.AspNetCore.Authorization;
using backend.Models.Common;
using System.Security.Claims;

using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : BaseFileContentController<Image, ImageDto, ImageService>
    {
        public ImageController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Image, ImageDto, ImageService>> logger, ImageService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {
        }

        [HttpGet("PaginatedWithCaption")]
        public async Task<ActionResult<PaginatedResponseDto<ImageDto>>> GetWithPagination(int page, int pageSize, string? captionSearch)
        {
            // Pass the userId to the service method
            return await _fileConentService.GetPaginated(page, pageSize, captionSearch);
        }


        [HttpGet("{id}/GetVisuallySimmilarImages")]
        public async Task<IEnumerable<ImageDto>> GetVisuallySimmilarImages(Guid id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _fileConentService.GetVisuallySimmilar(id);
        }

        //[HttpGet("GetImagesByCaption")]
        //public IQueryable<ImageDto> GetImagesByCaption(string caption)
        //{
        //    return _fileConentService.GetImagesByCaption(caption);
        //}

        [HttpGet("GetImagesWithoutCaption")]
        public IQueryable<ImageDto> GetImagesWithoutCaption()
        {
            return _fileConentService.GetImagesWithoutCaption();
        }


        [HttpGet("{id}/Content", Name = "GetImageStream")]
        [AllowAnonymous]
        public async override Task<IActionResult> GetFileContent(Guid id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            try
            {
                var imageEntity = _fileConentService.GetById(id);

                if (imageEntity == null)
                {
                    return NotFound();
                }

                var imageStream = await _fileConentService.GetFileStreamAsync(imageEntity.ContentId);

                if (imageStream.Length == 0)
                {
                    return NotFound();
                }

                // Return the image stream
                return File(imageStream, $"image/{imageEntity.FileExtension}"); // Adjust the content type based on your image format
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while attempting to read the image file: {ex.Message}");
                return StatusCode(500, "An error occurred while attempting to read the image file.");
            }
        }

        [HttpGet("{id}/Thumbnail", Name = "GetImageThumbnail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageThumbnail(Guid id)
        {
            try
            {
                var imageEntity = _fileConentService.GetById(id);
                if (imageEntity == null)
                {
                    return NotFound();
                }

                var imageStream = await _fileConentService.GetFileStreamAsync(imageEntity.ContentId);
                if (imageStream.Length == 0)
                {
                    return NotFound();
                }

                using (var image = SixLabors.ImageSharp.Image.Load(imageStream))
                {
                    // Crop to square
                    image.Mutate(x => x.Crop(new SixLabors.ImageSharp.Rectangle(
                        (image.Width - Math.Min(image.Width, image.Height)) / 2,
                        (image.Height - Math.Min(image.Width, image.Height)) / 2,
                        Math.Min(image.Width, image.Height),
                        Math.Min(image.Width, image.Height)
                    )));

                    // Resize to 150x150
                    image.Mutate(x => x.Resize(150, 150));

                    // Save to a new MemoryStream
                    var thumbnailStream = new MemoryStream();
                    image.Save(thumbnailStream, new JpegEncoder());
                    thumbnailStream.Position = 0;

                    // Return the thumbnail
                    return File(thumbnailStream, "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating the thumbnail: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the thumbnail.");
            }
        }

        [HttpDelete("{imageId}")]
        [Authorize(Roles ="Admin")]
        public async override Task<IActionResult> DeleteFileContent(Guid imageId)
        {
            var image = _fileConentService.GetById(imageId);

            try
            {
                await _fileConentService.DeleteImage(image.ContentId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting image: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }

        [HttpPatch("{fileId}/PatchClipEmbedding")]
        [Authorize(Roles = "Admin")]
        public IActionResult Patch(Guid fileId, EmbeddingDto embedding)
        {
            if (embedding == null)
            {
                return BadRequest("Invalid embedding");
            }

            var existingEntity = _fileConentService.GetById(fileId);

            if (existingEntity == null)
            {
                return NotFound();
            }

            // Map the existing entity to a DTO for patching
            var dtoToPatch = _mapper.Map<ImageDto>(existingEntity);

            //List<float> embed = embedding.Embedding;

            Vector emb = new Vector(embedding.Embedding.Replace(" ", ""));

            dtoToPatch.ClipEmbedding = emb;

            // Update entity properties based on the patched DTO
            _mapper.Map(dtoToPatch, existingEntity);

            // Perform the update in the database
            _fileConentService.Update();

            // Additional processing or actions after successful patch

            return Ok(dtoToPatch);
        }

    }
}
