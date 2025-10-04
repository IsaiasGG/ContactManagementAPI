
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class FundContact : BaseAuditableEntity, ISharedEntity
    {
        public int FundId { get; set; }
        public Fund Fund { get; set; } = null!;
        public int ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
    }
}
