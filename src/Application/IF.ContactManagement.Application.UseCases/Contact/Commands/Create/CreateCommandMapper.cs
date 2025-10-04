
using AutoMapper;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Create
{
    public class CreateCommandMapper : Profile
    {
        public CreateCommandMapper() 
        {
            CreateMap<CreateCommand, Domain.Entities.Contact>().ReverseMap();
        }
    }
}
