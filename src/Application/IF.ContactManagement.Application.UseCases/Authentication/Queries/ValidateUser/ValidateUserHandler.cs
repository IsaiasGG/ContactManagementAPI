using IF.ContactManagement.Application.DTO.SystemModule;
using IF.ContactManagement.Application.DTO.User;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Application.Interfaces.Services;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Authentication.Queries.ValidateUser
{
    public class ValidateUserHandler : IRequestHandler<ValidateUserQuery, Response<AuthResponseDTO>>
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ValidateUserHandler> _logger;
        protected readonly ApplicationDbContext _context;

        public ValidateUserHandler(IIdentityService identityService,
                                   ITokenGenerator tokenGenerator,
                                   IUnitOfWork unitOfWork,
                                   ILogger<ValidateUserHandler> logger,
                                   ApplicationDbContext context)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
            this.unitOfWork = unitOfWork;
            _logger = logger;
            _context = context;
        }

        public async Task<Response<AuthResponseDTO>> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<AuthResponseDTO>();

            try
            {
                await Task.Delay(100, cancellationToken); // Simulate async work

                var result = await _identityService.SigninUserAsync(request.UserName, request.Password);
                if (!result)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid user or password.";
                    return response;
                }

                var userId = await _identityService.GetUserIdAsync(request.UserName);
                var user = await _identityService.GetUserDetailsAsync(userId);

                // Get user roles
                var roles = await _identityService.GetUserRolesAsync(user.Id);

                // Get roles directly from assignments
                var roleNames = await _context.UserPermissionsAssignments
                    .Where(x => x.UserId == user.Id)
                    .Select(x => x.Role.Name)
                    .ToListAsync(cancellationToken);

                // Get user permissions
                var permissions = await _context.UserPermissionsAssignments
                    .Where(upa => upa.UserId == user.Id)
                    .SelectMany(upa => upa.Role.RoleModulePermissions)
                    .Select(rmp => new UserModulePermissionDTO
                    {
                        ModuleId = rmp.SystemModuleId,
                        ModuleName = rmp.SystemModule!.Name,
                        ModuleUrl = rmp.SystemModule!.Url,
                        PermissionTypeId = rmp.PermissionTypeId,
                        PermissionTypeName = rmp.PermissionType!.Name
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);

                // Get accessible modules
                var modules = await _context.UserPermissionsAssignments
                    .Where(upa => upa.UserId == user.Id)
                    .SelectMany(upa => upa.Role!.RoleModulePermissions)
                    .Select(rmp => rmp.SystemModule!)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                // Build hierarchical module tree
                List<SystemModuleDTO> BuildModuleTree(List<Domain.Entities.SystemModule> flatModules, string? parentId = null)
                {
                    return flatModules
                        .Where(m => m.ParentId == parentId)
                        .OrderBy(m => m.Order)
                        .Select(m => new SystemModuleDTO
                        {
                            Id = m.SystemModuleId,
                            Name = m.Name,
                            Url = m.Url,
                            Icon = m.Icon,
                            Order = m.Order,
                            Children = BuildModuleTree(flatModules, m.SystemModuleId)
                        })
                        .ToList();
                }

                var moduleTree = BuildModuleTree(modules);

                // Generate tokens
                string accessToken = _tokenGenerator.GenerateJWTToken(user, roles);
                string refreshToken = _tokenGenerator.GenerateRefreshToken();

                // Save refresh token
                var refreshTokenEntity = new UserRefreshToken()
                {
                    UserRefreshTokenId = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Token = refreshToken
                };
                refreshTokenEntity = await unitOfWork.UserRefreshTokenRepository.AddAsync(refreshTokenEntity, user.UserName);

                response.IsSuccess = true;
                response.Message = "User signed in successfully.";
                response.Data = new AuthResponseDTO()
                {
                    UserId = user.Id,
                    Name = user.UserName,
                    AccessToken = accessToken,
                    RefreshToken = refreshTokenEntity.Token,
                    UserRoles = roles,
                    ModulePermissions = permissions,
                    AccessibleModules = moduleTree
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);

                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = new List<BaseError>
                {
                    new BaseError
                    {
                        PropertyMessage = ex.Source,
                        ErrorMessage = ex.StackTrace
                    }
                };
            }

            return response;
        }
    }
}
