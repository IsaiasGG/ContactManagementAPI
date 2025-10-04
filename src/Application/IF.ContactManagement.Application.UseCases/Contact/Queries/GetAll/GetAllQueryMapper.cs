using AutoMapper;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetAll
{
    public class GetAllQueryMapper : Profile
    {
        public GetAllQueryMapper()
        {
            CreateMap<Domain.Entities.Contact, GetAllQueryResponse>().ReverseMap();
        }
    }
}
