using backend.Models.Dto;
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

        public AnyFileController(ReelService reelService, ImageService imageService)
        {
            _reelService = reelService;
            _imageService = imageService;
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(List<IFormFile> files)
        {

            List<IFormFile> imageFiles = new List<IFormFile>();
            List<IFormFile> videoFiles = new List<IFormFile>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;

                string fileType = file.ContentType.Split('/')[0];
                switch(fileType.ToLower())
                {
                    case "image":
                        imageFiles.Add(file);
                        break;
                    case "video":
                        videoFiles.Add(file);
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
    }
}
