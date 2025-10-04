using AutoMapper;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetById
{
    public class GetByIdQueryMapper : Profile
    {
        public GetByIdQueryMapper() 
        {
            CreateMap<Domain.Entities.Contact, GetByIdQueryResponse>().ReverseMap();
        }
    }
}
