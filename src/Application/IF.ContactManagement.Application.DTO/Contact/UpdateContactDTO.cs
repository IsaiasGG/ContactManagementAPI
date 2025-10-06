
using System.ComponentModel.DataAnnotations;

namespace IF.ContactManagement.Application.DTO.Contact
{
    public class UpdateContactDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
