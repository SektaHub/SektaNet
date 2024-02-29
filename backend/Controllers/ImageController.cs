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

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ImageController : BaseFileContentController<Image, ImageDto, ImageService>
    {


        public ImageController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Image, ImageDto, ImageService>> logger, ImageService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {

        }

        [HttpGet("{id}/GetVisuallySimmilarImages")]
        public async Task<IEnumerable<ImageDto>> GetConceptuallySimmilarImages(string id)
        {
            return await _fileConentService.GetVisuallySimmilar(id); ;
        }

        [HttpGet("GetImagesByCaption")]
        public IQueryable<ImageDto> GetImagesByCaption(string caption)
        {
            return _fileConentService.GetImagesByCaption(caption);
        }

        [HttpGet("GetImagesWithoutCaption")]
        [AllowAnonymous]
        public IQueryable<ImageDto> GetImagesWithoutCaption()
        {
            return _fileConentService.GetImagesWithoutCaption();
        }


        [HttpGet("{id}/Content", Name = "GetImageStream")]
        [AllowAnonymous]
        public async override Task<IActionResult> GetFileContent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var imageEntity = _fileConentService.GetById(id);

                if (imageEntity == null)
                {
                    return NotFound();
                }

                var imageStream = await _fileConentService.GetFileStreamAsync(id);

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

        //[HttpPost("upload")]
        //public async Task<IActionResult> Upload(IFormFile imageFile)
        //{
        //    if (imageFile == null || imageFile.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    try
        //    {

        //        // Assuming you've injected MongoDBService as _mongoDBService
        //        var fileId = await _fileConentService.UploadImage(imageFile);

        //        // Here you can link fileId with your reel entity if necessary

        //        return Ok(new { Message = "Image uploaded successfully", FileId = fileId });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error uploading image: {ex.Message}");
        //        return StatusCode(500, "An error occurred while uploading the image.");
        //    }
        //}

        //[HttpPost("upload-multiple")]
        //public async override Task<IActionResult> UploadMultiple(List<IFormFile> files)
        //{

        //    var imageDtos = await _fileConentService.UploadImage(files);

        //    return Ok(new { Message = "Images uploaded and saved successfully", UploadedFiles = imageDtos });
        //}

        [HttpDelete("{imageId}")]
        public async override Task<IActionResult> DeleteFileContent(string imageId)
        {
            try
            {
                await _fileConentService.DeleteImage(imageId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting image: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }

        [HttpPatch("{fileId}/PatchClipEmbedding")]
        [AllowAnonymous]
        public IActionResult Patch(string fileId, EmbeddingDto embedding)
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
