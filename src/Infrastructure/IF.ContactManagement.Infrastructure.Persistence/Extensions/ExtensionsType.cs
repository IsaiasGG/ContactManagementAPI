using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IF.ContactManagement.Infrastructure.Persistence.Extensions
{
    public static class ExtensionsType
    {
        public static bool MustSkipTenantValidation(this Type t)
        {
            var identityTypes = new List<Type>
        {
            typeof(User),
            typeof(IdentityRole),
            typeof(IdentityUserClaim<string>),
            typeof(IdentityUserRole<string>),
            typeof(IdentityUserLogin<string>),
            typeof(IdentityRoleClaim<string>),
            typeof(IdentityUserToken<string>)
        };

            var booleans = identityTypes.Select(identityType => identityType.IsAssignableFrom(t)).ToList();
            booleans.Add(typeof(ISharedEntity).IsAssignableFrom(t));

            return booleans.Any(b => b);
        }
    }
}
