using System.Security.Claims;

namespace WebTool
{
    internal static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            Claim userIdClaim = principal?.FindFirst("user_id");

            return userIdClaim?.Value;
        }
    }
}