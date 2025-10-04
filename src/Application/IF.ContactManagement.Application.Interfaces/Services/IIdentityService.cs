using IF.ContactManagement.Application.DTO.Pagination;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace IF.ContactManagement.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<(bool isSucceed, string userId, IEnumerable<IdentityError> errors)> CreateUserAsync(string userName, string password, string email, string fullName, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner);
        Task<bool> SigninUserAsync(string userName, string password);
        Task<bool> SignOutUserAsync(string userName);
        Task<string> GetUserIdAsync(string userName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserDetailsAsync(string userId);
        Task<User?> GetUserDetailsByUserNameAsync(string userName);
        Task<PagedResult<User>> GetPagedUsersAsync(int page,
                                             int limit,
                                             bool includeErased = false,
                                             Expression<Func<User, bool>> predicate = null,
                                             Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
                                             List<Expression<Func<User, object>>> includes = null);
        Task<string> GetUserNameAsync(string username);
        Task<string> GetUserNameByIdAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<(string id, string fullName, string userName, string email)>> GetAllUsersAsync();
        Task<List<(string id, string userName, string fullName, string email, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, IList<string> roles)>> GetAllUsersDetailsAsync();
        Task<bool> UpdateUserProfile(string id, string email, string fullName, string firstName, string lastName, string phoneNumber, Gender gender, bool isOwner, List<string> roles);
        // Method to reset user password
        Task<bool> ResetPasswordAsync(string userId, string newPassword);
        Task<string?> GeneratePasswordResetTokenAsync(User user);
        Task<(bool, IList<string>)> ConfirmPasswordResetAsync(string email, string token, string newPassword);

        // Role Section
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<(string id, string roleName)> GetRoleByIdAsync(string id);
        Task<bool> UpdateRole(string id, string roleName);
        Task<PagedResult<Role>> GetPagedAsync(int page,
                                              int limit,
                                              bool includeErased = false,
                                              Expression<Func<Role, bool>> predicate = null,
                                              Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = null,
                                              List<Expression<Func<Role, object>>> includes = null);

        // User's Role section
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignUserToRole(string userName, IList<string> roles);
        Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);
    }
}
