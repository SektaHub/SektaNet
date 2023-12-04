using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class ReelService
    {
        private readonly IWebHostEnvironment _env;

        public ReelService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetRandomReel()
        {
            // Assuming "Videos" is a folder within the wwwroot folder
            var videosFolder = Path.Combine(_env.WebRootPath, "Reels");
            var randomVideo = GetRandomReelFromFolder(videosFolder);

            return Path.Combine(videosFolder, randomVideo);
        }
        private string GetRandomReelFromFolder(string folderPath)
        {
            var videoFiles = Directory.GetFiles(folderPath, "*.mp4");
            if (videoFiles.Length > 0)
            {
                var randomIndex = new Random().Next(0, videoFiles.Length);
                return Path.GetFileName(videoFiles[randomIndex]);
            }

            // Handle the case when no video files are found
            throw new InvalidOperationException("No video files found in the specified folder.");
        }

        public string GetReelPath(int videoId)
        {
            // Implement logic to map videoId to the actual video path
            // This could involve a lookup in a database, a configuration file, or some other mechanism
            // Return the actual video path or null if not found
            // For simplicity, you can use a switch statement, but in a real application, you might have a more dynamic mapping
            var folderPath = Path.Combine(_env.WebRootPath, "Reels");

            // Ensuring the directory exists
            if (!Directory.Exists(folderPath))
            {
                throw new InvalidOperationException("The specified folder path does not exist.");
            }

            // Get all video files in the directory
            var videoFiles = Directory.GetFiles(folderPath, "*.mp4");
            if (videoFiles.Length > 0)
            {
                // Using videoId to get a consistent index for the same ID
                var randomIndex = videoId % videoFiles.Length;
                // Ensure we are returning the full file path
                return videoFiles[randomIndex];
            }
            else
            {
                // Handle the case when no video files are found
                throw new InvalidOperationException("No video files found in the specified folder.");
            }
        }
    }
}
