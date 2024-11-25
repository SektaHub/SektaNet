using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.Models;

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

        public ApplicationUser GetCurrentUser(HttpContext httpContext)
        {
            // Retrieve the current user's claims
            var userClaims = httpContext.User.Claims;

            // Example: Retrieve other user information
            var userIdClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userEmailClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            // Example: Create an ApplicationUser object
            var user = new ApplicationUser
            {
                Id = userIdClaim?.Value,
                Email = userEmailClaim?.Value,
                UserName = GetCurrentUserUsername(httpContext),
                // Add other properties as needed
            };

            return user;
        }
        
        public List<string> GetCurrentUserRoles(HttpContext httpContext)
        {
            // Retrieve the current user's claims
            var userClaims = httpContext.User.Claims;

            // Retrieve role claims
            var roleClaims = userClaims.Where(c => c.Type == ClaimTypes.Role);

            // Extract role values
            var roles = roleClaims.Select(c => c.Value).ToList();

            return roles;
        }
    }
}
