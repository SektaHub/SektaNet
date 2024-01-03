using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageService _imageService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ImageController(ILogger<ImageController> logger, ImageService imageService, ApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _imageService = imageService;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetImages")]
        public IEnumerable<ImageDto> Get()
        {
            var entities = _dbContext.Set<Image>().ToList();
            var dtoList = _mapper.Map<List<ImageDto>>(entities);
            return dtoList;
        }

        [HttpGet("GetImagesByCaption")]
        public IEnumerable<ImageDto> GetImagesByCaption(string caption)
        {
            // Filter images based on the provided caption (case-insensitive)
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.generatedCaption != null && image.generatedCaption.ToLower().Contains(caption.ToLower()))
                .ToList();

            var filteredDtoList = _mapper.Map<List<ImageDto>>(filteredEntities);
            return filteredDtoList;
        }

        [HttpGet("{imageId}/Data", Name = "GetImageData")]
        public ActionResult<ImageDto> GetImageData(Guid imageId)
        {
            var entity = _dbContext.Set<Image>().Find(imageId);

            if (entity == null)
            {
                return NotFound();
            }

            var imageDto = _mapper.Map<ImageDto>(entity);
            return imageDto;
        }

        [HttpGet("{imageId}", Name = "GetImageStream")]
        public IActionResult GetImage(Guid imageId)
        {
            var imagePath = _imageService.GetImagePath(imageId);

            if (string.IsNullOrEmpty(imagePath))
            {
                return NotFound();
            }

            try
            {
                var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(imageStream, "image/jpeg"); // Adjust the content type based on your image format
            }
            catch (IOException)
            {
                return StatusCode(500, "An error occurred while attempting to read the image file.");
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file provided");
            }

            // Validate other properties in the ImageDto if needed

            try
            {
                // Save the image
                ImageDto imageDto = _imageService.SaveImage(imageFile);

                // Additional processing or actions after successful image upload

                return Ok(new { Message = "Image uploaded and saved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading image: {ex.Message}");
                return StatusCode(500, "An error occurred while uploading the image");
            }
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultipleImages(List<IFormFile> imageFiles)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return BadRequest("No image files provided");
            }

            try
            {
                var uploadTasks = imageFiles
                    .Where(imageFile => imageFile != null && imageFile.Length > 0)
                    .Select(async imageFile =>
                    {
                        // Validate other properties in the ImageDto if needed
                        // Save each image asynchronously
                        ImageDto imageDto = await _imageService.SaveImageAsync(imageFile);
                        return imageDto;
                    });

                // Wait for all upload tasks to complete
                var uploadedImages = await Task.WhenAll(uploadTasks);

                // Additional processing or actions after successful image uploads

                return Ok(new { Message = "Images uploaded and saved successfully", UploadedFiles = uploadedImages });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading images: {ex.Message}");
                return StatusCode(500, "An error occurred while uploading the images");
            }
        }

        //[HttpPost("upload-multiple")]
        //public IActionResult UploadMultipleImages(List<IFormFile> imageFiles)
        //{
        //    if (imageFiles == null || imageFiles.Count == 0)
        //    {
        //        return BadRequest("No image files provided");
        //    }

        //    try
        //    {
        //        List<ImageDto> uploadedImages = new List<ImageDto>();

        //        foreach (var imageFile in imageFiles)
        //        {
        //            if (imageFile == null || imageFile.Length == 0)
        //            {
        //                // Skip invalid files
        //                continue;
        //            }

        //            // Validate other properties in the ImageDto if needed

        //            // Save each image
        //            ImageDto imageDto = _imageService.SaveImage(imageFile);
        //            uploadedImages.Add(imageDto);
        //        }

        //        // Additional processing or actions after successful image uploads

        //        return Ok(new { Message = "Images uploaded and saved successfully", UploadedFiles = uploadedImages });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error uploading images: {ex.Message}");
        //        return StatusCode(500, "An error occurred while uploading the images");
        //    }
        //}

        [HttpDelete("{imageId}")]
        public IActionResult DeleteImage(Guid imageId)
        {
            try
            {
                // Delete database entry
                var image = _dbContext.Set<Image>().Find(imageId);
                if (image != null)
                {
                    _dbContext.Set<Image>().Remove(image);
                    _dbContext.SaveChanges();
                }

                // Delete image file
                var imagePath = _imageService.GetImagePath(imageId);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return Ok("Image deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting image: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }
    }
}
