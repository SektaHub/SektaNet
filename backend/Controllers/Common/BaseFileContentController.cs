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
using Xabe.FFmpeg;

namespace backend.Controllers.Common
{

    public class BaseFileContentController<TEntity, TDto, FileConentService> : ControllerBase
        where TEntity : BaseFileContentEntity
        where TDto : BaseFileContentDto
        where FileConentService : BaseFileContentService<TEntity, TDto>
    {
        protected readonly IMapper _mapper;
        protected readonly IWebHostEnvironment _env;
        protected readonly ILogger<BaseFileContentController<TEntity, TDto, FileConentService>> _logger;
        protected readonly FileConentService _fileConentService;

        public BaseFileContentController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<TEntity, TDto, FileConentService>> logger, FileConentService fileConentService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _env = webHostEnvironment;
            _logger = logger;
            _fileConentService = fileConentService;
        }

        [HttpGet()]
        public IQueryable<TDto> Get()
        {
            return _fileConentService.GetAll();
        }

        [HttpPost("upload-multiple")]
        public virtual async Task<IActionResult> UploadMultiple(List<IFormFile> imageFiles)
        {
            return StatusCode(501, "UploadMultiple method not implemented in the derived class.");
        }

        [HttpGet("{id}/Content", Name = "GetImageStream")]
        public virtual IActionResult GetFileContent(string id)
        {
            return StatusCode(501, "UploadMultiple method not implemented in the derived class.");
        }

        [HttpGet("{id}/MetaData")]
        public ActionResult<TDto> GetFileMetadata(string id)
        {
            return _fileConentService.GetMetaData(id);
        }

        [HttpDelete("{id}")]
        public virtual IActionResult DeleteFileContent(string id)
        {
            throw new NotImplementedException();
        }

        //[HttpDelete("{id}")]
        //public virtual IActionResult DeleteFileContent(string id)
        //{
        //    try
        //    {
        //        // Delete file
        //        var filePath = _fileConentService.GetFilePath(id);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //        }

        //        // Delete database entry
        //        var file = _dbContext.Set<TEntity>().Find(id);
        //        if (file != null)
        //        {
        //            _dbContext.Set<TEntity>().Remove(file);
        //            _dbContext.SaveChanges();
        //        }

        //        return Ok("File deleted successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error deleting File: {ex.Message}");
        //        return StatusCode(500, "An error occurred while deleting the file");
        //    }
        //}

        [HttpPut("{fileId}")]
        public IActionResult Put(string fileId, TDto updatedDto)
        {
            _fileConentService.Put(fileId, updatedDto);
            return Ok();
        }

        [HttpPatch("{fileId}")]
        public IActionResult Patch(string id, JsonPatchDocument<TDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingEntity = _fileConentService.GetById(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            var dtoToPatch = _mapper.Map<TDto>(existingEntity);

            // Apply the patch document to the DTO without casting ModelState
            patchDocument.ApplyTo(dtoToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(dtoToPatch, existingEntity);

            _fileConentService.Update();

            return NoContent();

        }





    }
}
