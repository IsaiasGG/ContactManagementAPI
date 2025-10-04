using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund
{
    public sealed record GetByFundQuery : IRequest<Response<List<GetByFundQueryResponse>>>
    {
        public int FundId {  get; set; }
        public GetByFundQuery(int fundId)
        {
            FundId = fundId;
        }
    }
}
