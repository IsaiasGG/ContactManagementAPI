
using System.ComponentModel.DataAnnotations;

namespace IF.ContactManagement.Application.DTO.Contact
{
    public class CreateContactDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
