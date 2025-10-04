﻿
namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund
{
    public class GetByFundQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
