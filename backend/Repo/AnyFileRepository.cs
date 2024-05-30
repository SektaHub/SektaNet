using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using backend.Util;
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

        public async Task<Guid> SaveReel(HttpContext httpContext, IFormFile file, string tag, List<string> authorizedRoles)
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
            Guid thubnailId = await SaveThumbnail(httpContext, thumb, file.FileName, tag, authorizedRoles);

            Reel reel = new Reel
            {
                Id = RandomBytesGenerator.GenerateGuid(),
                ContentId = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                AudioTranscription = null,
                Duration = dur, // Will be set after getting duration
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
                ThumbnailId = thubnailId,
            };

            _dbContext.Reels.Add(reel);
            _dbContext.SaveChanges();


            return reel.Id;
        }

        public async Task<Guid> SaveLongVideo(HttpContext httpContext, IFormFile file, string tag, List<string> authorizedRoles)
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
            Guid thubnailId = await SaveThumbnail(httpContext, thumb, file.FileName, tag, authorizedRoles);

            LongVideo video = new LongVideo
            {
                Id = RandomBytesGenerator.GenerateGuid(),
                ContentId = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                AudioTranscription = null,
                Duration = dur, // Will be set after getting duration
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
                ThumbnailId = thubnailId,
            };

            _dbContext.LongVideos.Add(video);
            _dbContext.SaveChanges();


            return video.Id;
        }

        public async Task DeleteReel(Guid id)
        {
            var reel = await _dbContext.Reels.FindAsync(id);

            if (reel != null)
            {
                // Remove the reel from the context
                _dbContext.Reels.Remove(reel);
                await _dbContext.SaveChangesAsync();

                // Delete the associated file from MongoDB
                await _mongoRepo.DeleteFileAsync(reel.ContentId);
            }
        }

        public async Task DeleteLongVideo(Guid id)
        {
            var video = await _dbContext.LongVideos.FindAsync(id);

            if (video != null)
            {
                // Remove the reel from the context
                _dbContext.LongVideos.Remove(video);
                await _dbContext.SaveChangesAsync();

                // Delete the associated file from MongoDB
                await _mongoRepo.DeleteFileAsync(video.ContentId);
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

        public async Task<Guid> SaveImage(HttpContext httpContext, IFormFile file, string tag, List<string> authorizedRoles)
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
                Id = RandomBytesGenerator.GenerateGuid(),
                ContentId = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                GeneratedCaption = null,
                ClipEmbedding = null,
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
            };

            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image.Id;
        }

        public async Task<Guid> SaveThumbnail(HttpContext httpContext, byte[] thumbnailBytes, string fileName, string tag, List<string> authorizedRoles)
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
                Id = RandomBytesGenerator.GenerateGuid(),
                ContentId = fileId.ToString(),
                FileExtension = Path.GetExtension(fileName).TrimStart('.'),
                Tags = tag,
                Name = fileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
            };

            _dbContext.Thumbnails.Add(image);
            _dbContext.SaveChanges();

            return image.Id;
        }

        public async Task<Guid> SaveAudio(HttpContext httpContext, IFormFile file, string tag, List<string> authorizedRoles)
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
                Id = RandomBytesGenerator.GenerateGuid(),
                ContentId = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
            };

            _dbContext.Audio.Add(audio);
            _dbContext.SaveChanges();

            return audio.Id;
        }

        public async Task<Guid> SaveGenericFile(HttpContext httpContext, IFormFile file, string tag, List<string> authorizedRoles)
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
                Id = Guid.NewGuid(),
                ContentId = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                Tags = tag,
                Name = file.FileName,
                DateUploaded = DateTime.Now.ToUniversalTime(),
                AuthorizedRoles = authorizedRoles,
                OwnerId = currentUserId,
            };

            _dbContext.Files.Add(fil);
            _dbContext.SaveChanges();

            return fil.Id;
        }




    }
}
