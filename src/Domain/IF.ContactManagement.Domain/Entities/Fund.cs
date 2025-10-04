
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class Fund : BaseAuditableEntity, ISharedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<FundContact> FundContacts { get; set; } = new List<FundContact>();
    }
}
