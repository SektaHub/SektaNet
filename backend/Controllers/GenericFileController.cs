using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericFileController : BaseFileContentController<GenericFile, GenericFileDto, GenericFileService>
    {
        public GenericFileController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<GenericFile, GenericFileDto, GenericFileService>> logger, GenericFileService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {
        }
    }
}
