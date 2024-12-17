using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System.Text.RegularExpressions;

namespace backend.Services
{
    public class FfmpegService
    {

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FfmpegService> _logger;

        public FfmpegService(IWebHostEnvironment env, ILogger<FfmpegService> logger)
        {
            _env = env;
            _logger = logger;
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

        public async Task<byte[]> GenerateThumbnail(IFormFile videoFile)
        {
            // Generate file names without creating files
            string tempVideoFileName = Path.GetRandomFileName();
            string tempThumbnailFileName = Path.ChangeExtension(tempVideoFileName, ".jpeg");

            // Construct the full path for video and thumbnail using the temp path
            string tempVideoPath = Path.Combine(Path.GetTempPath(), tempVideoFileName);
            string thumbnailPath = Path.Combine(Path.GetTempPath(), tempThumbnailFileName);

            try
            {
                using (var fileStream = new FileStream(tempVideoPath, FileMode.Create))
                {
                    // Copy the video file to the temporary path first
                    await videoFile.CopyToAsync(fileStream);
                }

                // Now, we assume the file is ready for FFmpeg to process. 
                // No need for the file readiness check loop here.

                // Take a snapshot at the 1-second mark using FFmpeg
                IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(tempVideoPath, thumbnailPath, TimeSpan.FromSeconds(1));
                await conversion.Start();

                // Read the generated thumbnail into a byte array
                byte[] thumbnailBytes = await File.ReadAllBytesAsync(thumbnailPath);

                return thumbnailBytes;  // Returning the thumbnail as a byte array
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while generating thumbnail: {ex.Message}");
                return null;
            }
            finally
            {
                // Cleanup: Deleting temporary files
                if (File.Exists(tempVideoPath))
                {
                    File.Delete(tempVideoPath);
                }
                if (File.Exists(thumbnailPath))
                {
                    File.Delete(thumbnailPath);
                }
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

                // Use FFmpeg to get the video information including rotation
                string rotationOutput = await GetVideoRotation(filePath);
                int rotation = 0;
                if (int.TryParse(rotationOutput, out int parsedRotation))
                {
                    rotation = parsedRotation;
                }

                // Get media info from the temporary file using your previously preferred method
                var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

                if (videoStream != null)
                {
                    bool isRotated90or270 = rotation == 90 || rotation == 270;
                    int effectiveWidth = isRotated90or270 ? videoStream.Height : videoStream.Width;
                    int effectiveHeight = isRotated90or270 ? videoStream.Width : videoStream.Height;

                    // Calculate aspect ratio considering rotation
                    double aspectRatio = (double)effectiveWidth / (double)effectiveHeight;

                    // Check if aspect ratio is approximately 9:16
                    return Math.Abs(aspectRatio - (9.0 / 16.0)) < 0.05;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking aspect ratio: {ex.Message}");
                return false;
            }
        }

        private async Task<string> GetVideoRotation(string filePath)
        {
            string output, rotation = "0";

            using (var process = new Process())
            {
                // Determine the FFmpeg path based on the operating system
                if (OperatingSystem.IsWindows())
                {
                    //string baseDirectory = AppContext.BaseDirectory;
                    //process.StartInfo.FileName = Path.Combine(baseDirectory, "wwwroot", "FFmpeg", "ffmpeg.exe");
                    process.StartInfo.FileName = "C:\\Users\\borja\\Documents\\GitHub\\SektaGram\\backend\\wwwroot\\FFmpeg\\ffmpeg.exe";
                }
                else if (OperatingSystem.IsLinux())
                {
                    process.StartInfo.FileName = "/app/wwwroot/FFmpeg/ffmpeg";
                }
                else
                {
                    process.StartInfo.FileName = "/app/wwwroot/FFmpeg/ffmpeg";
                    //throw new PlatformNotSupportedException("This platform is not supported.");
                }

                process.StartInfo.Arguments = $"-i \"{filePath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true; // FFmpeg writes to stderr
                process.StartInfo.CreateNoWindow = true;

                // Start the process and read output
                process.Start();
                output = await process.StandardError.ReadToEndAsync();
                process.WaitForExit();
            }

            // Extract rotation value from FFmpeg output
            Match match = Regex.Match(output, "rotate\\s*:\\s*(\\d+)");
            if (match.Success)
            {
                rotation = match.Groups[1].Value;
            }

            return rotation;
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
