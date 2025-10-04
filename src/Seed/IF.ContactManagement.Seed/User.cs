using IF.ContactManagement.Domain.Enums;

namespace IF.ContactManagement.Seed
{
    public class User
    {
        public static string DEFAULT_PASSWORD = "InvestorFlow123.-";

        public static List<Domain.Entities.User> GetDataSeed()
        {
            return new List<Domain.Entities.User>()
            {

                new Domain.Entities.User()
                {
                    Id = "e9fd6fd3-4063-4894-b0ed-37e3589f51dc",
                    FullName = "Isaias Guzman",
                    FirstName = "Isaias",
                    LastName = "Guzman",
                    UserName = "ijguzman",
                    PhoneNumber = "+1 829-432-1417",
                    Gender = Gender.Masculine,
                    IsOwner = true,
                    Email = "isaiasguzman15@gmail.com",
                    SecurityStamp = "e9fd6fd3-4063-4894-b0ed-37e3589f51dc"
                },           
            };
        }
    }
}
