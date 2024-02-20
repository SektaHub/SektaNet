using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Services
{
    public class IdentityService
    {
        public string GetCurrentUserUsername(HttpContext httpContext)
        {
            // Retrieve the current user's claims
            var userClaims = httpContext.User.Claims;

            // Example: Retrieve the username
            var usernameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var username = usernameClaim?.Value;
            return username;
        }
    }
}
