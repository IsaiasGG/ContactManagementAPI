
namespace IF.ContactManagement.Seed
{
    public class UserPermissionsAssignment
    {
        public static List<Domain.Entities.UserPermissionsAssignment> GetDataSeed()
        {
            return new List<Domain.Entities.UserPermissionsAssignment>()
            {
                new Domain.Entities.UserPermissionsAssignment()
                {
                    UserId = "e9fd6fd3-4063-4894-b0ed-37e3589f51dc",
                    RoleId = "A-1"
                }
            };
        }

    }
}
