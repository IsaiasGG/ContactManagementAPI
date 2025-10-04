
namespace IF.ContactManagement.Seed
{
    public class Role
    {
        public static List<Domain.Entities.Role> GetDataSeed()
        {
            return new List<Domain.Entities.Role>()
            {
                new Domain.Entities.Role()
                {
                    Id = "A-1",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Description = "System administrator",
                    Icon = "fas fa-user-cog"
                },
                new Domain.Entities.Role()
                {
                    Id = "A-2",
                    Name = "Basic User",
                    NormalizedName = "USER",
                    Description = "Generic user",
                    Icon = "fas fa-user"
                },
            };
        }
    }
}
