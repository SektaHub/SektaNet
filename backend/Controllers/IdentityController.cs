using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : Controller
    {

        [HttpGet("current-user")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            // Retrieve the current user's claims
            var userClaims = HttpContext.User.Claims;

            // Example: Retrieve the username
            var usernameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var username = usernameClaim?.Value;

            // You can include any other user information you need

            // Return the user information
            return Ok(new { Username = username });
        }
    }
}
