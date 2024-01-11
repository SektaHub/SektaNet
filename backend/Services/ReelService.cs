using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services.Common;
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
    public class ReelService : BaseFileContentService<Reel, ReelDto>
    {
        public ReelService(IWebHostEnvironment env, ApplicationDbContext dbContext, IMapper mapper) : base(env, dbContext, mapper)
        {
            FolderName = "Reels";
        }

        public override void InitDirectories()
        {
            //Create the reel and thumbnail directories if it they do not exist
            base.InitDirectories();
            
            var thumbnailFolderPath = Path.Combine(_env.WebRootPath, "Thumbnails");
            if (!Directory.Exists(thumbnailFolderPath))
            {
                Directory.CreateDirectory(thumbnailFolderPath);
            }
        }


    }
}
