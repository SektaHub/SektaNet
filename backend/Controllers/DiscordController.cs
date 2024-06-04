using backend.Models.Discord;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class DiscordController : ControllerBase
    {
        private readonly DiscordService _discordService;

        public DiscordController(DiscordService discordService)
        {
            _discordService = discordService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var dto = _discordService.GetAll();
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<DiscordServerDto> Get(Guid id)
        {
            var dto = _discordService.GetDtoById(id);
            if (dto == null)
            {
                return NotFound();
            }
            return dto;
        }

        [HttpPost]
        public IActionResult Create(DiscordServerDto createDto)
        {
            DiscordServerDto dto = _discordService.Create(createDto);
            return Ok(dto);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create()
        //{
        //    try
        //    {
        //        using (var reader = new StreamReader(Request.Body))
        //        {
        //            var jsonString = await reader.ReadToEndAsync();
        //            var serverDto = JsonConvert.DeserializeObject<DiscordServerDto>(jsonString);
        //            var createdDto = _discordService.Create(serverDto);
        //            return Ok(createdDto);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle potential errors during deserialization or service call
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
