﻿
namespace IF.ContactManagement.Application.DTO.Contact
{
    public class CreateContactDTO
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
