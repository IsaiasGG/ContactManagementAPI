using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Delete
{
    public sealed record DeleteCommand : IRequest<Response<bool>>
    {
        public int ContactId { get; set; }
        public DeleteCommand(int contactId)
        {
            ContactId = contactId;
        }
    }
}
