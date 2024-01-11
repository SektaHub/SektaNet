using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace backend.Services
{
    public class FfmpegService
    {

        private readonly IWebHostEnvironment _env;

        public FfmpegService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task DownloadFFmpeg()
        {
            string FFMpegDownloadPath = Path.Combine(_env.WebRootPath, "FFmpeg");
            FFmpeg.SetExecutablesPath(FFMpegDownloadPath);
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, FFMpegDownloadPath).ConfigureAwait(false);
        }

        public async Task<string> ExtractThumbnailAsync(string videoPath, string outputPath)
        {
            try
            {
                // Get the media info
                var info = await FFmpeg.GetMediaInfo(videoPath);

                // Extract the video file name without extension
                string videoFileName = Path.GetFileNameWithoutExtension(videoPath);

                // Specify the full path for the output thumbnail file
                string thumbnailPath = Path.Combine(outputPath, $"{videoFileName}.jpg");

                // Take a snapshot at 1 second mark
                var thumbnail = await FFmpeg.Conversions.FromSnippet.Snapshot(
                    videoPath, thumbnailPath, TimeSpan.FromSeconds(1)
                );

                var result = await thumbnail.Start();

                // Return the path to the generated thumbnail
                return thumbnailPath;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error extracting thumbnail: {ex.Message}");
                return null;
            }
        }

        public async Task<int> GetVideoDurationAsync(string videoPath)
        {
            try
            {
                var mediaInfo = await FFmpeg.GetMediaInfo(videoPath);
                return (int)mediaInfo.Duration.TotalSeconds;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error getting video duration: {ex.Message}");
                return 0; // Return a default value or handle the error accordingly
            }
        }

        public void SetFFmpegPermissions()
        {
            // Use the appropriate paths based on your application structure
            string ffprobePath = "/app/wwwroot/FFmpeg/ffprobe";
            string ffmpegPath = "/app/wwwroot/FFmpeg/ffmpeg";

            // Set execute permissions for FFmpeg binaries
            ExecuteCommand($"chmod +x {ffprobePath}");
            ExecuteCommand($"chmod +x {ffmpegPath}");
        }

        private static void ExecuteCommand(string command)
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
            }
        }

    }
}
