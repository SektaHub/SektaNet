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

        public async Task<IFormFile> GenerateThumbnail(IFormFile videoFile)
        {
            string tempVideoPath = Path.GetTempFileName(); // Temp path for video.
            string thumbnailPath = Path.ChangeExtension(tempVideoPath, ".jpg"); // Temp path for thumbnail.
            try
            {
                await using (var fileStream = new FileStream(tempVideoPath, FileMode.Create))
                {
                    // Copy the video file to a temporary path
                    await videoFile.CopyToAsync(fileStream);
                }

                // Extract the video file name without extension for thumbnail naming
                string videoFileName = Path.GetFileNameWithoutExtension(videoFile.FileName);

                // Take a snapshot at 1 second mark
                IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(tempVideoPath, thumbnailPath, TimeSpan.FromSeconds(1));
                await conversion.Start();

                // After generating the thumbnail, open and convert it to IFormFile
                await using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
                {
                    var fileInfo = new FileInfo(thumbnailPath);
                    var formFile = new FormFile(thumbnailStream, 0, fileInfo.Length, videoFileName, fileInfo.Name);

                    return formFile; // Returning the generated thumbnail as an IFormFile
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while generating thumbnail: {ex.Message}");
                return null;
            }
            finally
            {
                //// Cleanup: Delete temporary files
                //if (File.Exists(tempVideoPath))
                //{
                //    File.Delete(tempVideoPath);
                //}
                //if (File.Exists(thumbnailPath))
                //{
                //    File.Delete(thumbnailPath);
                //}
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

        public async Task<bool> Is9_16AspectRatio(IFormFile file)
        {
            try
            {
                // Save the uploaded file to a temporary location
                var filePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Get media info from the temporary file
                var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

                if (videoStream != null)
                {
                    // Calculate aspect ratio
                    double aspectRatio = (double)videoStream.Width / videoStream.Height;

                    // Check if aspect ratio is approximately 16:9
                    return Math.Abs(aspectRatio - (9.0 / 16.0)) < 0.02;
                }
                else
                {
                    // No video stream found
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                //_logger.LogError($"Error checking aspect ratio: {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetVideoDuration(IFormFile file)
        {
            try
            {
                // Save the uploaded file to a temporary location
                var filePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Get media info from the temporary file
                var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

                if (videoStream != null)
                {
                    // Extract duration from video stream
                    return (int)videoStream.Duration.TotalSeconds;
                }
                else
                {
                    // No video stream found
                    throw new InvalidOperationException("No video stream found in the provided file.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error extracting video duration: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }

    }
}
