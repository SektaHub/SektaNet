using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Repo;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnyFileController : ControllerBase
    {

        private readonly ReelService _reelService;
        private readonly ImageService _imageService;
        private readonly AnyFileRepository _fileRepository;
        protected readonly ILogger<AnyFileController> _logger;

        public AnyFileController(ReelService reelService, ImageService imageService, AnyFileRepository fileRepository, ILogger<AnyFileController> logger)
        {
            _reelService = reelService;
            _imageService = imageService;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        [RequestSizeLimit(536_870_912)]
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(List<IFormFile> files)
        {
            try
            {
                List<IFormFile> imageFiles = new List<IFormFile>();
                List<IFormFile> videoFiles = new List<IFormFile>();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0) continue;

                    string fileType = file.ContentType.Split('/')[0];
                    switch (fileType.ToLower())
                    {
                        case "image":
                            //imageFiles.Add(file);
                            await _fileRepository.SaveImage(file);
                            break;
                        case "video":
                            //videoFiles.Add(file);
                            await _fileRepository.SaveReel(file);
                            break;
                        default:
                            Console.WriteLine("Unrecognized file type");
                            break;
                    }
                }

                await _imageService.UploadMultiple(imageFiles);
                await _reelService.UploadMultiple(videoFiles);

                return Ok("Files uploaded and saved successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error uploading files: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
