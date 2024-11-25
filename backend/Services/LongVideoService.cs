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
        public LongVideoService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, IdentityService identityService, IHttpContextAccessor httpContextAccessor) : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager, identityService, httpContextAccessor)
        {

        }

        public async Task DeleteLongVideo(Guid id)
        {
            await _anyFileRepository.DeleteLongVideo(id);
        }

    }
}
