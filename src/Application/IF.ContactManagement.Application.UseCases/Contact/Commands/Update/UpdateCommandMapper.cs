using AutoMapper;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Update
{
    public class UpdateCommandMapper : Profile
    {
        public UpdateCommandMapper() 
        {
            CreateMap<UpdateCommand, Domain.Entities.Contact>().ReverseMap();
        }

    }
}
