using IF.ContactManagement.Application.DTO.Pagination;
using IF.ContactManagement.Application.Interfaces.Services;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Domain.Enums;
using IF.ContactManagement.Transversal.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IF.ContactManagement.Infrastructure.Persistence.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;


        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.roleManager = roleManager;
        }


        public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }
            return result.Succeeded;
        }


        // Return multiple value
        public async Task<(bool isSucceed, string userId, IEnumerable<IdentityError> errors)> CreateUserAsync(string userName, string password, string email, string fullName, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner)
        {
            IEnumerable<IdentityError> errorList = Enumerable.Empty<IdentityError>();

            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = fullName,
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Gender = gender,
                IsOwner = isOwner
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return (false, user.Id, result.Errors);
            }

            return (result.Succeeded, user.Id, errorList);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var roleDetails = await roleManager.FindByIdAsync(roleId);
            if (roleDetails == null)
            {
                throw new NotFoundException("Role not found");
            }

            if (roleDetails.Name == "Administrator")
            {
                throw new BadRequestException("You can not delete Administrator Role");
            }
            var result = await roleManager.DeleteAsync(roleDetails);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }

            if (user.UserName == "system" || user.UserName == "admin")
            {
                throw new Exception("You can not delete system or admin user");
                //throw new BadRequestException("You can not delete system or admin user");
            }
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<(string id, string fullName, string userName, string email)>> GetAllUsersAsync()
        {
            var users = await userManager.Users.Select(x => new
            {
                x.Id,
                x.FullName,
                x.UserName,
                x.Email
            }).ToListAsync();

            return users.Select(user => (user.Id, user.FullName, user.UserName, user.Email)).ToList();
        }

        public async Task<List<(string id, string userName, string fullName, string email, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, IList<string> roles)>> GetAllUsersDetailsAsync()
        {
            var userList = new List<(string id, string userName, string fullName, string email, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, IList<string> roles)>();

            var users = await userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                (string id, string userName, string fullName, string email, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, IList<string> roles) newUser = (user.Id, user.UserName, user.FullName, user.Email, user.FirstName, user.LastName, user.PhoneNumber, user.Gender, user.IsOwner, roles);
                userList.Add(newUser);
            }

            return userList;
        }

        public async Task<List<(string id, string roleName)>> GetRolesAsync()
        {
            var roles = await roleManager.Roles.Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();

            return roles.Select(role => (role.Id, role.Name)).ToList();
        }

        public async Task<User?> GetUserDetailsAsync(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await userManager.GetRolesAsync(user);
            return user;
        }

        public async Task<User?> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await userManager.GetRolesAsync(user);
            return user;
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await userManager.GetUserIdAsync(user);
        }

        public async Task<string> GetUserNameAsync(string username)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return user.FullName;
        }

        public async Task<string> GetUserNameByIdAsync(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return user.FullName;
        }
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return await userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await userManager.FindByNameAsync(userName) == null;
        }

        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;
        }

        public async Task<bool> SignOutUserAsync(string userName)
        {
            var userEntity = await userManager.FindByNameAsync(userName);
            var result = await userManager.UpdateSecurityStampAsync(userEntity);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserProfile(string id, string email, string fullName, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, List<string> roles)
        {
            var user = await userManager.FindByIdAsync(id);
            user.FullName = fullName;
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.PhoneNumber = phoneNumber;
            user.Gender = gender;
            user.IsOwner = isOwner;

            var result = await userManager.UpdateAsync(user);

            if (roles.Count > 0)
            {
                var addUserRole = await userManager.AddToRolesAsync(user, roles);

                if (!addUserRole.Succeeded)
                {
                    throw new ValidationException(addUserRole.Errors);
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<(string id, string roleName)> GetRoleByIdAsync(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return (role.Id, role.Name);
        }

        public async Task<bool> UpdateRole(string id, string roleName)
        {
            if (roleName != null)
            {
                var role = await roleManager.FindByIdAsync(id);
                role.Name = roleName;
                var result = await roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        {
            var user = await userManager.FindByNameAsync(userName);
            var existingRoles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, existingRoles);
            result = await userManager.AddToRolesAsync(user, usersRole);

            return result.Succeeded;
        }


        public async Task<PagedResult<Role>> GetPagedAsync(int page, int limit, bool includeErased = false, Expression<Func<Role, bool>> predicate = null, Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = null, List<Expression<Func<Role, object>>> includes = null)
        {
            IQueryable<Role> query = roleManager.Roles;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var include in includes ?? new List<Expression<Func<Role, object>>>())
            {
                query = query.Include(include);
            }

            var totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query.Skip((page - 1) * limit)
                                   .Take(limit)
                                   .ToListAsync();

            return new PagedResult<Role>
            {
                Items = items,
                Meta = new GenericPaginationResponse
                {
                    TotalItems = totalItems,
                    ItemCount = items.Count,
                    ItemsPerPage = limit,
                    TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / limit) : 0,
                    CurrentPage = page
                }
            };
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            email = email.ToUpper();
            return userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == email);
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(User user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<(bool, IList<string>)> ConfirmPasswordResetAsync(string email, string token, string newPassword)
        {
            // get the user by the email
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            // validate if the user is null
            if (user == null)
            {
                return (false, new List<string> { $"User with email '{email}' not found" });
            }

            // reset the password
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(x => x.Description).ToList());
            }

            return (true, new List<string>());
        }

        public async Task<PagedResult<User>> GetPagedUsersAsync(int page, int limit, bool includeErased = false, Expression<Func<User, bool>> predicate = null, Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, List<Expression<Func<User, object>>> includes = null)
        {
            IQueryable<User> query = userManager.Users;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var include in includes ?? new List<Expression<Func<User, object>>>())
            {
                query = query.Include(include);
            }

            var totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }



            var items = await query.OrderBy(x => x.FullName).Skip((page - 1) * limit)
                                   .Take(limit)
                                   .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                Meta = new GenericPaginationResponse
                {
                    TotalItems = totalItems,
                    ItemCount = items.Count,
                    ItemsPerPage = limit,
                    TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / limit) : 0,
                    CurrentPage = page
                }
            };
        }
    }
}
