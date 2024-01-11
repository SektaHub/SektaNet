using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Pgvector;
using backend.Controllers.Common;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : BaseFileContentController<Image, ImageDto, ImageService>
    {
        public ImageController(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Image, ImageDto, ImageService>> logger, ImageService fileConentService) : base(dbContext, mapper, webHostEnvironment, logger, fileConentService)
        {

        }

        [HttpGet("{id}/GetConceptuallySimmilarImages")]
        public async Task<IEnumerable<ImageDto>> GetConceptuallySimmilarImages(Guid id)
        {

            var entity = _dbContext.Set<Image>().Find(id);

            if (entity == null)
            {
                return (IEnumerable<ImageDto>)NotFound();
            }

            var imageDto = _mapper.Map<ImageDto>(entity);

            var entities = await _dbContext.Set<Image>()
                .Where(x => x.Id != id)
                .OrderBy(x => x.CaptionEmbedding!.L2Distance(imageDto.CaptionEmbedding))
                .Take(4)
                .ToListAsync();

            var dtoList = _mapper.Map<List<ImageDto>>(entities);
            return dtoList;
        }

        [HttpGet("GetImagesByCaption")]
        public IEnumerable<ImageDto> GetImagesByCaption(string caption)
        {
            // Filter images based on the provided caption (case-insensitive)
            var filteredEntities = _dbContext.Set<Image>()
                .Where(image => image.GeneratedCaption != null && image.GeneratedCaption.ToLower().Contains(caption.ToLower()))
                .ToList();

            var filteredDtoList = _mapper.Map<List<ImageDto>>(filteredEntities);
            return filteredDtoList;
        }

        [HttpGet("{id}/Content", Name = "GetImageStream")]
        public override IActionResult GetFileContent(Guid id)
        {
            try
            {
                var imageEntity = _dbContext.Images.FirstOrDefault(img => img.Id == id);

                if (imageEntity == null)
                {
                    return NotFound();
                }

                var imagePath = _fileConentService.GetFilePath(id);

                if (string.IsNullOrEmpty(imagePath))
                {
                    return NotFound();
                }

                var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(imageStream, $"image/{imageEntity.FileExtension}"); // Adjust the content type based on your image format
            }
            catch (IOException)
            {
                return StatusCode(500, "An error occurred while attempting to read the image file.");
            }
        }


        [HttpPost("upload-multiple")]
        public async override Task<IActionResult> UploadMultiple(List<IFormFile> files)
        {
            // ...omitting initial checks and try-catch for brevity

            List<string> imagePaths = new List<string>();
            List<ImageDto> imageDtos = new List<ImageDto>();

            foreach (var imageFile in files)
            {
                if (imageFile == null || imageFile.Length == 0) continue;

                var imageId = Guid.NewGuid();

                var imageDto = new ImageDto
                {
                    Id = imageId,
                    FileExtension = imageFile.ContentType.Split('/')[1],
                    generatedCaption = null,
                    CaptionEmbedding = null,
                };
                imageDtos.Add(imageDto);

                var imagePath = await _fileConentService.SaveFile(imageFile, imageId, imageDto.FileExtension);
                imagePaths.Add(imagePath);
            }

            // Ensure all files are saved before proceeding
            for (int i = 0; i < imagePaths.Count; i++)
            {
                var imageDto = imageDtos[i];
                var imagePath = imagePaths[i];

                // Additional processing or actions after successful image uploads

                // Save the Image entity to the database
                var newImage = _mapper.Map<Image>(imageDto);
                _dbContext.Images.Add(newImage);
            }
            _dbContext.SaveChanges();

            // Additional processing or actions after saving images to the database

            return Ok(new { Message = "Images uploaded and saved successfully", UploadedFiles = imageDtos });
        }

    }
}
