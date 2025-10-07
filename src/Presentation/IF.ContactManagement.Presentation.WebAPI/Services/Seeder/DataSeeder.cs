using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace IF.ContactManagement.Presentation.WebAPI.Services.Seeder
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly string DEFAULT_PASSWORD;

        public DataSeeder(ApplicationDbContext context,
                          UserManager<User> userManager,
                          RoleManager<Role> roleManager,
                          IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            DEFAULT_PASSWORD = _configuration["Security:DefaultPassword"];
        }

        public async Task SeedAll()
        {
            await SeedRoles();
            await SeedPermissions();
            await SeedUsers();
            await SeedRoleModulePermissions();
            await SeedFund();
            await SeedSystemModule();
            await SeedSystemModuleCategorie();
            await SeedUserPermissionsAssignment();
        }

        private async Task SeedUserPermissionsAssignment()
        {
            var items = Seed.UserPermissionsAssignment.GetDataSeed();
            foreach (var item in items)
            {
                if (!_context.UserPermissionsAssignments.Any(upa => upa.UserId == item.UserId && upa.RoleId == item.RoleId))
                {
                    _context.UserPermissionsAssignments.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedSystemModuleCategorie()
        {
            var items = Seed.SystemModuleCategorie.GetDataSeed();
            foreach (var item in items)
            {
                if (!_context.SystemModuleCategories.Any(smc => smc.SystemModuleCategoryId == item.SystemModuleCategoryId))
                {
                    _context.SystemModuleCategories.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedSystemModule()
        {
            var items = Seed.SystemModule.GetDataSeed();
            foreach (var item in items)
            {
                if (!_context.SystemModules.Any(sm => sm.SystemModuleId == item.SystemModuleId))
                {
                    _context.SystemModules.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedFund()
        {

            var items = Seed.Fund.GetDataSeed();
            foreach (var item in items)
            {
                if (!_context.Funds.Any(f => f.Id == item.Id))
                {
                    _context.Funds.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedRoles()
        {
            var roles = Seed.Role.GetDataSeed(); // suponiendo que tienes un método estático
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task SeedPermissions()
        {
            var permissions = Seed.PermissionType.GetDataSeed();
            foreach (var permission in permissions)
            {
                if (!_context.PermissionTypes.Any(p => p.Name == permission.Name))
                {
                    _context.PermissionTypes.Add(permission);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            var users = Seed.User.GetDataSeed();
            foreach (var user in users)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email!);
                if (existingUser == null)
                {
                    await _userManager.CreateAsync(user, DEFAULT_PASSWORD);
                }
            }
        }

        private async Task SeedRoleModulePermissions()
        {
            var items = Seed.RoleModulePermission.GetDataSeed();
            foreach (var item in items)
            {
                if (!_context.RoleModulePermissions.Any(rmp => rmp.PermissionTypeId == item.PermissionTypeId && rmp.SystemModuleId == item.SystemModuleId && rmp.RoleId == item.RoleId))
                {
                    _context.RoleModulePermissions.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }
    }

}
