using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetAll
{
    public class GetAllQuery : IRequest<Response<List<GetAllQueryResponse>>>{ }
}
