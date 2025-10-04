using IF.ContactManagement.Domain.Constants;

namespace IF.ContactManagement.Seed
{
    public class PermissionType
    {
        public static List<Domain.Entities.PermissionType> GetDataSeed()
        {
            var now = DateTime.UtcNow;
            return new List<Domain.Entities.PermissionType>()
            {
                new Domain.Entities.PermissionType()
                {
                    PermissionTypeId = "ADD",
                    CreatedAt =now,
                    CreatedBy = ConstantsCreatedBy.SeedData,
                    Name = "Add",
                    Description = "Add",
                    Icon = "fa-solid fa-plus"
                },
                new Domain.Entities.PermissionType()
                {
                    PermissionTypeId = "EDIT",
                    CreatedAt =now,
                    CreatedBy = ConstantsCreatedBy.SeedData,
                    Name = "Edit",
                    Description = "Edit",
                    Icon = "fa-solid fa-edit"
                },
                new Domain.Entities.PermissionType()
                {
                    PermissionTypeId = "DELETE",
                    CreatedAt =now,
                    CreatedBy = ConstantsCreatedBy.SeedData,
                    Name = "Delete",
                    Description = "Delete",
                    Icon = "fa-solid fa-trash"
                },
                new Domain.Entities.PermissionType()
                {
                    PermissionTypeId = "VIEW",
                    CreatedAt =now,
                    CreatedBy = ConstantsCreatedBy.SeedData,
                    Name = "View",
                    Description = "View",
                    Icon = "fa-solid fa-eye"
                },
            };
        }
    }
}
