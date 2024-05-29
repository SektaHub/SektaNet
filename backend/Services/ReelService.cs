using AutoMapper;
using backend.Models;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public ReelService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, FfmpegService ffmpegService, IdentityService identityService, IHttpContextAccessor httpContextAccessor) : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager, identityService, httpContextAccessor)
        {
            _ffmpegService = ffmpegService;
        }

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

            return _dbContext.Set<Reel>().OrderBy(r => Guid.NewGuid()).FirstOrDefault();

        }

        public async Task DeleteReel(Guid id)
        {
            await _anyFileRepository.DeleteReel(id);
        }

    }
}