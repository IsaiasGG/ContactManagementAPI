
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class Contact : BaseAuditableEntity, ISharedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<FundContact> FundContacts { get; set; } = new List<FundContact>();
    }
}
