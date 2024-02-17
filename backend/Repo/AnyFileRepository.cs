using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using MongoDB.Bson;
using Xabe.FFmpeg;

namespace backend.Repo
{
    public class AnyFileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MongoDBRepository _mongoRepo;
        protected readonly ILogger<AnyFileRepository> _logger;

        public AnyFileRepository(ApplicationDbContext dbContext, MongoDBRepository mongoRepo, ILogger<AnyFileRepository> logger)
        {
            _dbContext = dbContext;
            _mongoRepo = mongoRepo;
            _logger = logger;
        }

        public async  Task<string> SaveReel(IFormFile file)
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

            Reel reel = new Reel
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                AudioTranscription = null,
                Duration = null, // Will be set after getting duration
            };

            _dbContext.Reels.Add(reel);
            _dbContext.SaveChanges();

            return reel.Id;
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

        public async Task<string> SaveImage(IFormFile file)
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

            Image image = new Image
            {
                Id = fileId.ToString(),
                FileExtension = file.ContentType.Split('/')[1],
                GeneratedCaption = null,
                CaptionEmbedding = null, 
            };

            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image.Id;
        }



    }
}
