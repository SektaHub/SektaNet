using AutoMapper;
using backend.Models.Common;
using backend.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Bson;
using System.Xml.XPath;
using Xabe.FFmpeg;

using AutoMapper;
using backend.Models.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using backend.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.XPath;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using MongoDB.Bson;
using backend.Repo;

namespace backend.Services.Common
{
    public class BaseFileContentService<TEntity, TDto>
        where TEntity : BaseFileContentEntity
        where TDto : BaseFileContentDto
    {

        protected readonly IWebHostEnvironment _env;
        protected readonly IMapper _mapper;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly MongoDBRepository _mongoRepo;
        protected readonly AnyFileRepository _anyFileRepository;


        public BaseFileContentService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository)
        {
            _env = env;
            _mapper = mapper;
            _dbContext = dbContext;
            _mongoRepo = mongoRepo;
            _anyFileRepository = anyFileRepository;
        }

        public IQueryable<TDto> GetAll()
        {
            var entities = _dbContext.Set<TEntity>();
            var dtoList = _mapper.Map<List<TDto>>(entities);
            return dtoList.AsQueryable();
        }

        public TDto GetDtoById(string id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            if (entity == null)
            {
                // Entity with the specified ID was not found
                //throw new EntityNotFoundException($"Entity with ID '{id}' not found.");
            }

            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }

        public TEntity GetById(string id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            if (entity == null)
            {
                // Entity with the specified ID was not found
                //throw new EntityNotFoundException($"Entity with ID '{id}' not found.");
            }

            return entity;
        }

        public TDto GetMetaData(string id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            if (entity == null)
            {
                // Entity not found, handle appropriately, e.g., throw NotFoundException
                //throw new NotFoundException($"Entity with ID '{id}' not found");
            }

            var imageDto = _mapper.Map<TDto>(entity);
            return imageDto;
        }

        public TDto Put(string fileId, TDto updatedDto)
        {
            if (updatedDto == null || fileId != updatedDto.Id)
            {
                //return BadRequest("Invalid request data.");
            }

            var existingEntity = _dbContext.Set<TEntity>().Find(fileId);

            if (existingEntity == null)
            {
                //return NotFound();
            }

            // Update entity properties based on the provided DTO
            _mapper.Map(updatedDto, existingEntity);

            // Perform the update in the database
            _dbContext.SaveChanges();

            // Additional processing or actions after successful update

            return updatedDto;

            //return NoContent();
        }

        public void Update()
        {
            _dbContext.SaveChanges();
        }

        public virtual async Task<List<TDto>> UploadMultiple(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //return BadRequest("No file uploaded.");
                return null;
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    // Assuming you've injected MongoDBService as _mongoDBService
                    var fileId = await _mongoRepo.UploadFileAsync(stream, file.FileName);

                    // Here you can link fileId with your reel entity if necessary

                    //return Ok(new { Message = "Video uploaded successfully", FileId = fileId });
                    return fileId.ToString();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error uploading video: {ex.Message}");
                //return StatusCode(500, "An error occurred while uploading the video.");
            }
            return null;
        }

        public async Task<Stream> GetFileStreamAsync(string id)
        {
            return await _mongoRepo.GetFileStreamAsync(id);
        }

    }
}
