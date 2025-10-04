using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund
{
    public sealed record AssignToFundCommand : IRequest<Response<bool>>
    {
        public int FundId { get; set; }
        public int ContactId { get; set; }
        public AssignToFundCommand(int fundId, int contactId)
        {
            FundId = fundId;
            ContactId = contactId;
        }
    }
}
