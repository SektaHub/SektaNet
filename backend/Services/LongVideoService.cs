using AutoMapper;
using backend.Models;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class LongVideoService : BaseFileContentService<LongVideo, LongVideoDto>
    {
        public LongVideoService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager) : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager)
        {
        }

        public async Task DeleteLongVideo(string id)
        {
            await _anyFileRepository.DeleteLongVideo(id);
        }

    }
}
