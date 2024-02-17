using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.Json.Nodes;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace backend.Services
{
    public class ReelService : BaseFileContentService<Reel, ReelDto>
    {
        private readonly FfmpegService _ffmpegService;

        public ReelService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper, FfmpegService ffmpegService, AnyFileRepository anyFileRepository, MongoDBRepository mongoDBRepository) : base(env, mapper, dbContext, mongoDBRepository, anyFileRepository)
        {
            _ffmpegService = ffmpegService;
        }

        //public async override Task<List<ReelDto>> UploadMultiple(List<IFormFile> videoFiles)
        //{
        //    // ...omitting initial checks and try-catch for brevity

        //    List<string> videoPaths = new List<string>();
        //    List<ReelDto> reelDtos = new List<ReelDto>();

        //    foreach (var videoFile in videoFiles)
        //    {
        //        if (videoFile == null || videoFile.Length == 0) continue;

        //        string reelId = ObjectId.GenerateNewId().ToString();

        //        var reelDto = new ReelDto
        //        {
        //            Id = reelId,
        //            FileExtension = videoFile.ContentType.Split('/')[1],
        //            AudioTranscription = null,
        //            Duration = null, // Will be set after getting duration
        //        };
        //        reelDtos.Add(reelDto);

        //        var videoPath = await SaveFile(videoFile, reelId, reelDto.FileExtension);
        //        videoPaths.Add(videoPath);
        //    }

        //    // Ensure all files are saved before proceeding
        //    for (int i = 0; i < videoPaths.Count; i++)
        //    {
        //        var reelDto = reelDtos[i];
        //        var videoPath = videoPaths[i];

        //        // Set the duration of the video
        //        reelDto.Duration = await _ffmpegService.GetVideoDurationAsync(videoPath);

        //        // Save the Reel entity to the database
        //        var newReel = _mapper.Map<Reel>(reelDto);
        //        _dbContext.Reels.Add(newReel);
        //    }
        //    _dbContext.SaveChanges();

        //    // Now generate thumbnails
        //    var thumbnailTasks = reelDtos.Select(reelDto =>
        //        _ffmpegService.ExtractThumbnailAsync(GetFilePath(reelDto.Id), Path.Combine(_env.WebRootPath, "Thumbnails"))
        //    ).ToList();

        //    await Task.WhenAll(thumbnailTasks);

        //    return reelDtos;
        //}

        public IQueryable<ReelDto> GetReelsWithoutTranscription()
        {
            // Filter images where GeneratedCaption is null
            var filteredEntities = _dbContext.Set<Reel>()
                .Where(reel => reel.AudioTranscription == null)
                .AsQueryable();

            var filteredDtoList = _mapper.ProjectTo<ReelDto>(filteredEntities);
            return filteredDtoList;
        }


        public Reel GetRandomVideo()
        {

            return _dbContext.Set<Reel>().OrderBy(r => new ObjectId()).FirstOrDefault();

        }

        public async Task<string> UploadReel(IFormFile file)
        {
            return await _anyFileRepository.SaveReel(file);
        }

        public async Task DeleteReel(string id)
        {
            await _anyFileRepository.DeleteReel(id);
        }

    }
}