using System.Security.Claims;

namespace SmartFarmer.API.Extension
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserMasterAdminId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Actor);
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
