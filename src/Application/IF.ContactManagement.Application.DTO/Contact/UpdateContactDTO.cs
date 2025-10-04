
namespace IF.ContactManagement.Application.DTO.Contact
{
    public class UpdateContactDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
