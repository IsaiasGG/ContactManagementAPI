
using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetById
{
    public sealed record GetByIdQuery : IRequest<Response<GetByIdQueryResponse>>
    {
        public int ContactId { get; set; }
        public GetByIdQuery(int contactId)
        {
            ContactId = contactId;
        }
    }
}
