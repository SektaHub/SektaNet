using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;

namespace backend.Services
{
    public class GenericFileService : BaseFileContentService<GenericFile, GenericFileDto>
    {
        public GenericFileService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository) : base(env, mapper, dbContext, mongoRepo, anyFileRepository)
        {
        }
    }
}
