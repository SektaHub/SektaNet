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
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IdentityService _identityService;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseFileContentService(IWebHostEnvironment env, IMapper mapper, ApplicationDbContext dbContext, MongoDBRepository mongoRepo, AnyFileRepository anyFileRepository, UserManager<ApplicationUser> userManager, IdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _mapper = mapper;
            _dbContext = dbContext;
            _mongoRepo = mongoRepo;
            _anyFileRepository = anyFileRepository;
            _userManager = userManager;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<TDto> GetAll()
        {
            var entities = _dbContext.Set<TEntity>();
            var dtos = _mapper.ProjectTo<TDto>(entities);
            return dtos;
        }

        public IQueryable<TEntity> GetAllowed()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = _identityService.GetCurrentUser(httpContext);
            var userRoles = _identityService.GetCurrentUserRoles(httpContext);

            var entities = _dbContext.Set<TEntity>().AsQueryable();

            var allowedEntities = entities.Where(e =>
                e.OwnerId == user.Id ||
                !e.AuthorizedRoles.Any() ||
                e.AuthorizedRoles.Any(role => userRoles.Contains(role))
            );

            return allowedEntities;
        }

        public PaginatedResponseDto<TDto> GetPaginated(int page, int pageSize)
        {
            var allowedEntities = GetAllowed();

            var totalCount = allowedEntities.Count();

            var paginatedEntities = allowedEntities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<TDto>>(paginatedEntities);

            var response = new PaginatedResponseDto<TDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };

            return response;
        }

        public TDto GetDtoById(Guid id)
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

        public TEntity GetById(Guid id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            if (entity == null)
            {
                // Entity with the specified ID was not found
                //throw new EntityNotFoundException($"Entity with ID '{id}' not found.");
                throw new Exception($"Entity with ID '{id}' not found.");
            }

            return entity;
        }

        public TEntity GetById(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            // Include related entities if any are specified
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var entity = query.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                throw new Exception($"Entity with ID '{id}' not found.");
            }

            return entity;
        }


        public TDto GetMetaData(Guid id)
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

        public TDto Put(Guid fileId, TDto updatedDto)
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

        public async Task<Stream> GetFileStreamAsync(string id)
        {
            return await _mongoRepo.GetFileStreamAsync(id);
        }

    }
}
