using AutoMapper;

namespace IF.ContactManagement.Application.UseCases.Fund.Queries.GetAll
{
    public class GetAllQueryMapper : Profile
    {
        public GetAllQueryMapper()
        {
            CreateMap<Domain.Entities.Fund, GetAllQueryResponse>().ReverseMap();
        }
    }
}
