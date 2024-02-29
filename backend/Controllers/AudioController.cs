using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : BaseFileContentController<Audio, AudioDto, AudioService>
    {
        public AudioController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<Audio, AudioDto, AudioService>> logger, AudioService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {
        }
    }
}
