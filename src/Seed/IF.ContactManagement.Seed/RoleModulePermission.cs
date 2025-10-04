
namespace IF.ContactManagement.Seed
{
    public class RoleModulePermission
    {
        public static List<Domain.Entities.RoleModulePermission> GetDataSeed()
        {
            return new List<Domain.Entities.RoleModulePermission>()
            {
                // RoleId: A-1
                // SystemModuleId: 1
                new Domain.Entities.RoleModulePermission()
                {
                    RoleId = "A-1",
                    SystemModuleId = "1",
                    PermissionTypeId = "ADD",
                },
                new Domain.Entities.RoleModulePermission()
                {
                    RoleId = "A-1",
                    SystemModuleId = "1",
                    PermissionTypeId = "EDIT",

                },
                new Domain.Entities.RoleModulePermission()
                {
                    RoleId = "A-1",
                    SystemModuleId = "1",
                    PermissionTypeId = "DELETE",

                },
                new Domain.Entities.RoleModulePermission()
                {
                    RoleId = "A-1",
                    SystemModuleId = "1",
                    PermissionTypeId = "VIEW",

                }
            };
        }
    }
}
