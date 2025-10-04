using IF.ContactManagement.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;

namespace IF.ContactManagement.Presentation.WebAPI.Services.Tenant
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                return string.Empty;
            }

            var user = httpContext.User;

            if (user is null)
            {
                return string.Empty;
            }

            var claimTenant = user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
            //var otherclaims = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier);

            if (claimTenant is null)
            {
                return string.Empty;
            }

            return claimTenant.Value;
        }
    }
}
