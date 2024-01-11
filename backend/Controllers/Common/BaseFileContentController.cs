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

namespace backend.Controllers.Common
{

    public class BaseFileContentController<TEntity, TDto, FileConentService> : ControllerBase
        where TEntity : BaseFileContentEntity
        where TDto : BaseFileContentDto
        where FileConentService : BaseFileContentService<TEntity, TDto>
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IWebHostEnvironment _env;
        protected readonly ILogger<BaseFileContentController<TEntity, TDto, FileConentService>> _logger;
        protected readonly FileConentService _fileConentService;

        public BaseFileContentController(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<TEntity, TDto, FileConentService>> logger, FileConentService fileConentService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _env = webHostEnvironment;
            _logger = logger;
            _fileConentService = fileConentService;
        }

        [HttpGet()]
        public IQueryable<TDto> Get()
        {
            var entities = _dbContext.Set<TEntity>();
            var dtoList = _mapper.Map<List<TDto>>(entities);
            return dtoList.AsQueryable();
        }

        [HttpPost("upload-multiple")]
        public virtual async Task<IActionResult> UploadMultiple(List<IFormFile> imageFiles)
        {
            return StatusCode(501, "UploadMultiple method not implemented in the derived class.");
        }

        [HttpGet("{id}/Content", Name = "GetImageStream")]
        public virtual IActionResult GetFileContent(Guid id)
        {
            return StatusCode(501, "UploadMultiple method not implemented in the derived class.");
        }

        [HttpGet("{id}/MetaData")]
        public ActionResult<TDto> GetFileMetadata(Guid id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var imageDto = _mapper.Map<TDto>(entity);
            return imageDto;
        }

        [HttpDelete("{id}")]
        public virtual IActionResult DeleteFileContent(Guid id)
        {
            try
            {
                // Delete file
                var filePath = _fileConentService.GetFilePath(id);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete database entry
                var file = _dbContext.Set<Reel>().Find(id);
                if (file != null)
                {
                    _dbContext.Set<Reel>().Remove(file);
                    _dbContext.SaveChanges();
                }

                return Ok("File deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting File: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the file");
            }
        }

        [HttpPut("{fileId}")]
        public IActionResult Put(Guid fileId, TDto updatedDto)
        {
            if (updatedDto == null || fileId != updatedDto.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var existingEntity = _dbContext.Set<TEntity>().Find(fileId);

            if (existingEntity == null)
            {
                return NotFound();
            }

            // Update entity properties based on the provided DTO
            _mapper.Map(updatedDto, existingEntity);

            // Perform the update in the database
            _dbContext.SaveChanges();

            // Additional processing or actions after successful update

            return NoContent();
        }

        [HttpPatch("{fileId}")]
        public IActionResult Patch(Guid fileId, JsonPatchDocument<TDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingEntity = _dbContext.Set<TEntity>().Find(fileId);

            if (existingEntity == null)
            {
                return NotFound();
            }

            // Map the existing entity to a DTO for patching
            var dtoToPatch = _mapper.Map<TDto>(existingEntity);

            // Apply the patch document to the DTO without casting ModelState
            patchDocument.ApplyTo(dtoToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update entity properties based on the patched DTO
            _mapper.Map(dtoToPatch, existingEntity);

            // Perform the update in the database
            _dbContext.SaveChanges();

            // Additional processing or actions after successful patch

            return NoContent();
        }



    }
}
