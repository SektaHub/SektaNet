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
            var randomVideo = GetRandomVideoFromFolder(videosFolder);

            return Path.Combine(videosFolder, randomVideo);
        }
        private string GetRandomVideoFromFolder(string folderPath)
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
    }
}
