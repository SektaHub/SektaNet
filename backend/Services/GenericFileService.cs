using AutoMapper;
using backend.Models;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class GenericFileService : BaseFileContentService<GenericFile, GenericFileDto>
    {
        public GenericFileService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, IdentityService identityService, IHttpContextAccessor httpContextAccessor) : base(env, mapper, dbContext, mongoRepo, anyFileRepository, userManager, identityService, httpContextAccessor)
        {
        }
    }
}
