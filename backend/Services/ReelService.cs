using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ReelService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ReelService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper)
        {
            _env = env;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //Returns a semi-random reel based on the inputed int Id
        //Used for testing purposes vefore inplementing Actual video ids and database
        public string GetReelPathTest(int videoId)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "Reels");

            if (!Directory.Exists(folderPath))
            {
                throw new InvalidOperationException("The specified folder path does not exist.");
            }

            var videoFiles = Directory.GetFiles(folderPath, "*.mp4");
            if (videoFiles.Length > 0)
            {
                var randomIndex = videoId % videoFiles.Length;
                return videoFiles[randomIndex];
            }
            else
            {
                throw new InvalidOperationException("No video files found in the specified folder.");
            }
        }

        public string GetReelPath(Guid videoId)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "Reels");
            var videoFileName = $"{videoId}.mp4";
            var videoPath = Path.Combine(folderPath, videoFileName);

            if (File.Exists(videoPath))
            {
                return videoPath;
            }
            else
            {
                throw new InvalidOperationException($"Video file with Id {videoId} not found in the specified folder.");
            }
        }


        public void SaveVideo(ReelDto reelDto, IFormFile videoFile)
        {
            // Process and save the video file to the wwwroot/Reels folder
            var reelFolderPath = Path.Combine(_env.WebRootPath, "Reels");
            var videoFileName = $"{reelDto.Id}.mp4";
            var videoFilePath = Path.Combine(reelFolderPath, videoFileName);

            using (var stream = new FileStream(videoFilePath, FileMode.Create))
            {
                videoFile.CopyTo(stream);
            }

            // Map ReelDto to Reel entity
            var newReel = _mapper.Map<Reel>(reelDto);

            // Set VideoPath property to the path or URL of the uploaded video file (NOT NEEDED BECAUSE THE PATH IS IN THE ID)
            //newReel.VideoPath = $"Reels/{videoFileName}";

            // Save the Reel entity to the database
            _dbContext.Reels.Add(newReel);
            _dbContext.SaveChanges();
        }

    }
}
