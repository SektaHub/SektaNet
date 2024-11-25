using backend.Models.Dto;
using backend.Models.Entity;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogpostController : ControllerBase
    {

        private readonly BlogpostService _blogpostService;

        public BlogpostController(BlogpostService blogpostService)
        {
            _blogpostService = blogpostService;
        }


        // GET: api/Blogpost
        [HttpGet]
        public async Task<IActionResult> GetBlogposts()
        {
            var blogposts = await _blogpostService.GetBlogpostsAsync();
            return Ok(blogposts);
        }

        // GET: api/Blogpost/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogpost(Guid id)
        {
            var blogpost = await _blogpostService.GetBlogpostAsync(id);
            if (blogpost == null)
            {
                return NotFound();
            }
            return Ok(blogpost);
        }

        // POST: api/Blogpost
        [HttpPost]
        public async Task<IActionResult> PostBlogpost([FromBody] BlogpostRequest blogpostRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogpostResponse = await _blogpostService.CreateBlogpostAsync(HttpContext, blogpostRequest);
            return CreatedAtAction(nameof(GetBlogpost), new { id = blogpostResponse.Id }, blogpostResponse);
        }


    }
}
