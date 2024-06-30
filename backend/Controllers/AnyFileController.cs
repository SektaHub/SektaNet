using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Repo;
using backend.Services;
using backend.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnyFileController : ControllerBase
    {

        private readonly ReelService _reelService;
        private readonly ImageService _imageService;
        private readonly AnyFileRepository _fileRepository;
        private readonly FfmpegService _ffmpegService;
        protected readonly ILogger<AnyFileController> _logger;

        public AnyFileController(ReelService reelService, ImageService imageService, AnyFileRepository fileRepository, ILogger<AnyFileController> logger, FfmpegService ffmpegService)
        {
            _reelService = reelService;
            _imageService = imageService;
            _fileRepository = fileRepository;
            _logger = logger;
            _ffmpegService = ffmpegService;
        }

        [RequestSizeLimit(536_870_912_0)]
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(List<IFormFile> files, string? tags, string? authorizedRoles)
        {
            try
            {
                if(tags == null)
                    tags = "";
                List<string> authorizedRolesList;

                if (string.IsNullOrEmpty(authorizedRoles))
                {
                    authorizedRolesList = new List<string>();
                }
                else
                {
                    authorizedRolesList = authorizedRoles.Split(',').Select(role => role.Trim()).ToList();
                }
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0) continue;

                    string fileType = file.ContentType.Split('/')[0];
                    switch (fileType.ToLower())
                    {
                        case "image":
                            await _fileRepository.SaveImage(HttpContext, file, tags, authorizedRolesList);
                            break;
                        case "video":
                            if(await _ffmpegService.Is9_16AspectRatio(file))
                                await _fileRepository.SaveReel(HttpContext, file, tags, authorizedRolesList);
                            else
                                await _fileRepository.SaveLongVideo(HttpContext, file, tags, authorizedRolesList);
                            break;
                        case "audio":
                            await _fileRepository.SaveAudio(HttpContext, file, tags, authorizedRolesList);
                            break;
                        default:
                            await _fileRepository.SaveGenericFile(HttpContext, file, tags, authorizedRolesList);
                            break;
                    }
                }

                return Ok("Files uploaded and saved successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error uploading files: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [RequestSizeLimit(536_870_912_0)]
        [HttpPost("upload-multiple-from-urls")]
        public async Task<IActionResult> UploadMultipleFromUrls(List<string> urls, string? tags, string? authorizedRoles)
        {
            try
            {
                if (tags == null)
                    tags = "";

                List<string> authorizedRolesList = string.IsNullOrEmpty(authorizedRoles)
                    ? new List<string>()
                    : authorizedRoles.Split(',').Select(role => role.Trim()).ToList();

                using var httpClient = new HttpClient();

                foreach (var url in urls)
                {
                    if (string.IsNullOrEmpty(url)) continue;

                    var response = await httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode) continue;

                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    if (string.IsNullOrEmpty(contentType)) continue;

                    string fileType = contentType.Split('/')[0];
                    var stream = await response.Content.ReadAsStreamAsync();
                    var fileName = Path.GetFileName(new Uri(url).LocalPath);

                    var formFile = new StreamFormFile(stream, fileName, contentType);

                    switch (fileType.ToLower())
                    {
                        case "image":
                            await _fileRepository.SaveImage(HttpContext, formFile, tags, authorizedRolesList, url);
                            break;
                        case "video":
                            if (await _ffmpegService.Is9_16AspectRatio(formFile))
                                await _fileRepository.SaveReel(HttpContext, formFile, tags, authorizedRolesList, url);
                            else
                                await _fileRepository.SaveLongVideo(HttpContext, formFile, tags, authorizedRolesList, url);
                            break;
                        case "audio":
                            await _fileRepository.SaveAudio(HttpContext, formFile, tags, authorizedRolesList, url);
                            break;
                        default:
                            await _fileRepository.SaveGenericFile(HttpContext, formFile, tags, authorizedRolesList, url);
                            break;
                    }
                }
                return Ok("Files downloaded, uploaded, and saved successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error downloading and uploading files: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
