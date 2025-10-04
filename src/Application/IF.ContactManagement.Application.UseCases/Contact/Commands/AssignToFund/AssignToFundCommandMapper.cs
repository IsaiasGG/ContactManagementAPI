using AutoMapper;
using IF.ContactManagement.Domain.Entities;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund
{
    public class AssignToFundCommandMapper : Profile
    {
        public AssignToFundCommandMapper() 
        {
            CreateMap<AssignToFundCommand, FundContact>().ReverseMap();
        }
    }
}
