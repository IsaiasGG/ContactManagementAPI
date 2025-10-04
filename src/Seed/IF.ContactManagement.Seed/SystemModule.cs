using IF.ContactManagement.Domain.Constants;

namespace IF.ContactManagement.Seed
{
    public class SystemModule
    {
        public static List<Domain.Entities.SystemModule> GetDataSeed()
        {
            var now = DateTime.UtcNow;

            return new List<Domain.Entities.SystemModule>()
            {
                new Domain.Entities.SystemModule()
                {
                    SystemModuleId = "1",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData,
                    Name = "Contact Management",
                    Url = "/contact",
                    Icon = "more-app",
                    Order = 1,
                    SystemModuleCategorieId = "1"
                },             

            };
        }
    }
}
