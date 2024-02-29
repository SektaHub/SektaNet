using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IdentityService _identityService;

        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("current-user")]
        [Authorize]
        public IActionResult GetCurrentUserUsername()
        {
            string username = _identityService.GetCurrentUserUsername(HttpContext);
            // Return the user information
            return Ok(new { Username = username });
        }


    }
}
