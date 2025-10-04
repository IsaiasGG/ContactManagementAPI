
using IF.ContactManagement.Domain.Constants;

namespace IF.ContactManagement.Seed
{
    public class Fund
    {
        public static List<Domain.Entities.Fund> GetDataSeed()
        {
            var now = DateTime.UtcNow;
            return new List<Domain.Entities.Fund>()
            {
                new Domain.Entities.Fund
                {
                    Id = 1,
                    Name = "Alpha Growth Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 2,
                    Name = "Beta Income Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 3,
                    Name = "Gamma Equity Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 4,
                    Name = "Delta Balanced Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 5,
                    Name = "Epsilon Opportunity Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 6,
                    Name = "Zeta Horizon Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 7,
                    Name = "Eta Capital Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 8,
                    Name = "Theta Global Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 9,
                    Name = "Iota Strategic Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                },
                new Domain.Entities.Fund
                {
                    Id = 10,
                    Name = "Kappa Diversified Fund",
                    CreatedAt = now,
                    CreatedBy = ConstantsCreatedBy.SeedData
                }

            };
        }
    }
}
