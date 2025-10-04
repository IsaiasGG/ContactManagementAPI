using IF.ContactManagement.Transversal.Common;
using MediatR;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund
{
    public sealed record DeleteFundCommand : IRequest<Response<bool>>
    {
        public int FundId { get; set; }     
        public int ContactId { get; set; }   
        public DeleteFundCommand(int fundId, int contactId)
        {
            FundId = fundId;
            ContactId = contactId;
        }
    }
}
