using backend.Models.Discord;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpPost("upload")]
        [RequestSizeLimit(2_147_483_648_0)]
        public async Task<IActionResult> UploadAndProcess(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var serializer = new JsonSerializer();

            // Process the JSON in chunks
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    var serverDto = serializer.Deserialize<DiscordServerDto>(jsonReader);
                    await ProcessServerDto(serverDto);
                }
            }

            return Ok("Processing complete");
        }

        private async Task ProcessServerDto(DiscordServerDto serverDto)
        {
            _discordService.Create(serverDto);
        }

        [HttpGet("{id}/ChatExport")]
        public ActionResult<DiscordServerDto> GenerateChat(string id)
        {
            var chatjson = _discordService.GenerateChatJson(id);
            return Ok(chatjson);
        }

        [HttpGet("{id}/ChatExportFile")]
        public IActionResult GenerateChatFile(string id)
        {
            var chatJson = _discordService.GenerateChatJson(id);

            // Generate a unique filename
            string fileName = $"ChatExport_{id}_{DateTime.Now:yyyyMMddHHmmss}.json";

            // Specify the directory where you want to save the file
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "ChatExports");

            // Ensure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Combine the directory and filename
            string filePath = Path.Combine(directory, fileName);

            // Write the JSON to the file
            System.IO.File.WriteAllText(filePath, chatJson);

            // Return a success message with the file path
            return Ok($"Chat export saved to: {filePath}");
        }

        [HttpGet("{id}/ChatExportFiles")]
        public async Task<IActionResult> GenerateChatFiles(string id)
        {
            // Specify the directory where you want to save the files
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "ChatExports", id);

            // Ensure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Generate the chat files
            var fileNames = await _discordService.GenerateChatJsonFiles(id, directory);

            // Return a success message with the file paths
            return Ok($"Chat exports saved to: {string.Join(", ", fileNames)}");
        }

        [HttpGet("{id}/ChatExportFilesWithAttachments")]
        public async Task<IActionResult> GenerateChatFilesWithAttachments(string id)
        {
            // Specify the directory where you want to save the files
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "ChatExports", id);

            // Ensure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Generate the chat files
            var fileNames = await _discordService.GenerateChatJsonFilesWithAttachments(id, directory);

            // Return a success message with the file paths
            return Ok($"Chat exports saved to: {string.Join(", ", fileNames)}");
        }

        [HttpGet("{id}/AttachmentUrlExport")]
        public ActionResult<DiscordServerDto> GenerateChatAttachments(string id)
        {
            var chatjson = _discordService.GenerateAttachmentUrlsJson(id);
            return Ok(chatjson);
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
