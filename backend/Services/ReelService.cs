using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace backend.Services
{
    public class ReelService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        //private readonly FFmpeg FFmpeg;

        public ReelService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper)
        {
            _env = env;
            _dbContext = dbContext;
            _mapper = mapper;

            string FFMpegDownloadPath = Path.Combine(_env.WebRootPath, "FFmpeg");
            FFmpeg.SetExecutablesPath(FFMpegDownloadPath);

        }

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


        public async Task<string> SaveVideo(IFormFile videoFile, Guid reelId)
        {
            var reelFolderPath = Path.Combine(_env.WebRootPath, "Reels");
            var videoFileName = $"{reelId}.mp4";
            var videoFilePath = Path.Combine(reelFolderPath, videoFileName);

            using (var stream = new FileStream(videoFilePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            // After this, the file should be fully written to disk
            return videoFilePath;
        }

        private async Task WaitForFileReady(string filename)
        {
            const int maxAttempts = 40;
            const int delayBetweenAttemptsMs = 100;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                if (IsFileReady(filename))
                {
                    Console.WriteLine("FILEFound");
                    return;
                }

                if (attempt < maxAttempts)
                {
                    // Wait for a short duration before the next attempt
                    await Task.Delay(delayBetweenAttemptsMs);
                }
            }

            // Handle the case where the file is not ready after all attempts
            Console.WriteLine("File not ready after all attempts");
        }

        private bool IsFileReady(string filename)
        {
            try
            {
                var fileInfo = new FileInfo(filename);

                // Check if the file has not been modified for the last second
                return (DateTime.UtcNow - fileInfo.LastWriteTimeUtc).TotalSeconds > 1;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("FileNotFoundException");
                return false;
            }
            catch (IOException)
            {
                Console.WriteLine("IOException");
                // Handle accordingly or wait for the next attempt
                return false;
            }
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

        public int GetVideoDuration(string videoPath)
        {
            try
            {
                var mediaInfo = FFmpeg.GetMediaInfo(videoPath).Result;
                return (int)mediaInfo.Duration.TotalSeconds;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error getting video duration: {ex.Message}");
                return 0; // Return a default value or handle the error accordingly
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


        public void InitDirectories()
        {
            //Create the reel and thumbnail directories if it they do not exist
            var reelFolderPath = Path.Combine(_env.WebRootPath, "Reels");
            if (!Directory.Exists(reelFolderPath))
            {
                Directory.CreateDirectory(reelFolderPath);
            }
            var thumbnailFolderPath = Path.Combine(_env.WebRootPath, "Thumbnails");
            if (!Directory.Exists(thumbnailFolderPath))
            {
                Directory.CreateDirectory(thumbnailFolderPath);
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
