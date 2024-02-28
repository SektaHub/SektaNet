using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Repo;
using backend.Services.Common;

namespace backend.Services
{
    public class LongVideoService : BaseFileContentService<LongVideo, LongVideoDto>
    {
        public LongVideoService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository) : base(env, mapper, dbContext, mongoRepo, anyFileRepository)
        {
        }

        public async Task DeleteLongVideo(string id)
        {
            await _anyFileRepository.DeleteLongVideo(id);
        }

    }
}
