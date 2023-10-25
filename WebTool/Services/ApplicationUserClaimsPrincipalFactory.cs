using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using System.Threading.Tasks;

namespace WebTool
{
    internal class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options)
            : base(userManager, options) { }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            identity.AddClaim(new Claim("user_id", user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            return principal;
        }
    }
}