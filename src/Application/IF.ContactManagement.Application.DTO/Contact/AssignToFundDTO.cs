
using System.ComponentModel.DataAnnotations;

namespace IF.ContactManagement.Application.DTO.Contact
{
    public class AssignToFundDTO
    {
        [Required]
        public int FundId { get; set; }
        [Required]
        public int ContactId { get; set; }
    }
}
