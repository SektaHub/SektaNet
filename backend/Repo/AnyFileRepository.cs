using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Net.Http;
using System.Security.Claims;
using Xabe.FFmpeg;

namespace backend.Repo
{
    public class AnyFileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MongoDBRepository _mongoRepo;
        protected readonly ILogger<AnyFileRepository> _logger;
        private readonly FfmpegService _ffmpegService;

        public AnyFileRepository(ApplicationDbContext dbContext, MongoDBRepository mongoRepo, ILogger<AnyFileRepository> logger, FfmpegService ffmpegService)
        {
            _dbContext = dbContext;
            _mongoRepo = mongoRepo;
            _logger = logger;
            _ffmpegService = ffmpegService;
        }

        public async Task<string> SaveReel(HttpContext httpContext, IFormFile file, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);
                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int dur = await _ffmpegService.GetVideoDuration(file);

            byte[] thumb = await _ffmpegService.GenerateThumbnail(file);
            string thubnailId = await SaveThumbnail(httpContext, thumb, file.FileName, tag ,isPrivate);

            Reel reel = new Reel
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                AudioTranscription = null,
                Duration = dur, // Will be set after getting duration
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
                ThumbnailId = thubnailId,
            };

            _dbContext.Reels.Add(reel);
            _dbContext.SaveChanges();


            return reel.Id;
        }

        public async Task<string> SaveLongVideo(HttpContext httpContext, IFormFile file, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);
                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int dur = await _ffmpegService.GetVideoDuration(file);

            byte[] thumb = await _ffmpegService.GenerateThumbnail(file);
            string thubnailId = await SaveThumbnail(httpContext, thumb, file.FileName, tag, isPrivate);

            LongVideo video = new LongVideo
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                AudioTranscription = null,
                Duration = dur, // Will be set after getting duration
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
                ThumbnailId = thubnailId,
            };

            _dbContext.LongVideos.Add(video);
            _dbContext.SaveChanges();


            return video.Id;
        }

        public async Task DeleteReel(string id)
        {
            var reel = await _dbContext.Reels.FindAsync(id);

            if (reel != null)
            {
                // Remove the reel from the context
                _dbContext.Reels.Remove(reel);
                await _dbContext.SaveChangesAsync();

                // Delete the associated file from MongoDB
                await _mongoRepo.DeleteFileAsync(id);
            }
        }

        public async Task DeleteLongVideo(string id)
        {
            var video = await _dbContext.LongVideos.FindAsync(id);

            if (video != null)
            {
                // Remove the reel from the context
                _dbContext.LongVideos.Remove(video);
                await _dbContext.SaveChangesAsync();

                // Delete the associated file from MongoDB
                await _mongoRepo.DeleteFileAsync(id);
            }
        }

        public async Task DeleteImage(string id)
        {
            var reel = await _dbContext.Images.FindAsync(id);

            if (reel != null)
            {
                // Remove the reel from the context
                _dbContext.Images.Remove(reel);
                await _dbContext.SaveChangesAsync();

                // Delete the associated file from MongoDB
                await _mongoRepo.DeleteFileAsync(id);
            }
        }

        public async Task<string> SaveImage(HttpContext httpContext, IFormFile file, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);
                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Image image = new Image
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                GeneratedCaption = null,
                ClipEmbedding = null,
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
            };

            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image.Id;
        }

        public async Task<string> SaveThumbnail(HttpContext httpContext, byte[] thumbnailBytes, string fileName, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                // Upload the thumbnail bytes to the repository
                fileId = await _mongoRepo.UploadFileAsync(new MemoryStream(thumbnailBytes), fileName, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading thumb: {ex.Message}");
                // Handle the exception as per your requirement
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Thumbnail image = new Thumbnail
            {
                Id = fileId.ToString(),
                FileExtension = Path.GetExtension(fileName).TrimStart('.'),
                Tags = tag,
                Name = fileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
            };

            _dbContext.Thumbnails.Add(image);
            _dbContext.SaveChanges();

            return image.Id;
        }

        public async Task<string> SaveAudio(HttpContext httpContext, IFormFile file, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);
                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Audio audio = new Audio
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
            };

            _dbContext.Audio.Add(audio);
            _dbContext.SaveChanges();

            return audio.Id;
        }

        public async Task<string> SaveGenericFile(HttpContext httpContext, IFormFile file, string tag, bool isPrivate = false)
        {
            ObjectId fileId = ObjectId.Empty;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);
                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }

            string? currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            GenericFile fil = new GenericFile
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                isPrivate = isPrivate,
                OwnerId = currentUserId,
            };

            _dbContext.Files.Add(fil);
            _dbContext.SaveChanges();

            return fil.Id;
        }




    }
}
