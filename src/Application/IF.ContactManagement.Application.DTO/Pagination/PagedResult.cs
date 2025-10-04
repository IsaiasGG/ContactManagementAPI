
namespace IF.ContactManagement.Application.DTO.Pagination
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T>? Items { get; set; }
        public GenericPaginationResponse? Meta { get; set; }
    }
}
