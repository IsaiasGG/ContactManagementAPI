using IF.ContactManagement.Application.DTO.User;
using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Authentication.Queries.ValidateUser
{
    public sealed record ValidateUserQuery : IRequest<Response<AuthResponseDTO>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public ValidateUserQuery(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
