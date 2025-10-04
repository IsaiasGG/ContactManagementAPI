using AutoMapper;
using IF.ContactManagement.Domain.Entities;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund
{
    public class GetByFundQueryMapper : Profile
    {
        public GetByFundQueryMapper()
        {
            CreateMap<FundContact, GetByFundQueryResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContactId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Contact.Name))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact.PhoneNumber));
        }
    }
}
