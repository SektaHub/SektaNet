﻿using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        [Authorize (Roles = "Sektash, Admin")]
        public IQueryable<TDto> Get()
        {
            return _fileConentService.GetAll();
        }

        [HttpGet("Paginated")]
        [Authorize(Roles = "Sektash, Admin")]
        public virtual ActionResult<PaginatedResponseDto<TDto>> GetWithPagination(int page, int pageSize)
        {
            return _fileConentService.GetPaginated(page, pageSize);
        }

        [HttpPost("upload-multiple")]
        public virtual async Task<IActionResult> UploadMultiple(List<IFormFile> imageFiles)
        {
            return StatusCode(501, "UploadMultiple method not implemented in the derived class.");
        }

        [HttpGet("{id}/Content")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetFileContent(Guid id)
        {
            return StatusCode(501, "GetFileContent method not implemented in the derived class.");
        }

        [HttpGet("{id}/MetaData")]
        [AllowAnonymous]
        public ActionResult<TDto> GetFileMetadata(Guid id)
        {
            return _fileConentService.GetMetaData(id);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async virtual Task<IActionResult> DeleteFileContent(Guid id)
        {
            throw new NotImplementedException("DeleteFileContent method not implemented in the derived class.");
        }

        [HttpPut("{fileId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(Guid fileId, TDto updatedDto)
        {
            _fileConentService.Put(fileId, updatedDto);
            return Ok();
        }

        [HttpPatch("{id}")]
        [AllowAnonymous]
        public IActionResult Patch(Guid id, JsonPatchDocument<TDto> patchDocument)
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
