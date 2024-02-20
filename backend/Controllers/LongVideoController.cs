using AutoMapper;
using backend.Controllers.Common;
using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LongVideoController : BaseFileContentController<LongVideo, LongVideoDto, LongVideoService>
    {
        public LongVideoController(IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BaseFileContentController<LongVideo, LongVideoDto, LongVideoService>> logger, LongVideoService fileConentService) : base(mapper, webHostEnvironment, logger, fileConentService)
        {
        }
    }
}
