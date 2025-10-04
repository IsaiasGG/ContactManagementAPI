using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Fund.Queries.GetAll
{
    public sealed record GetAllQuery : IRequest<Response<List<GetAllQueryResponse>>> { }
}
