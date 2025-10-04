namespace IF.ContactManagement.Seed
{
    public class SystemModuleCategorie
    {
        public static List<Domain.Entities.SystemModuleCategory> GetDataSeed()
        {
            return new List<Domain.Entities.SystemModuleCategory>()
            {
                new Domain.Entities.SystemModuleCategory()
                {
                    SystemModuleCategoryId = "1",
                    Name = "Contact Management",
                    Description = "Contact Management",
                    Icon = "fas fa-cogs",
                    Order = 1
                }
            };
        }
    }
}
