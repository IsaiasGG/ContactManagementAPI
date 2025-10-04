
namespace IF.ContactManagement.Application.DTO.SystemModule
{
    public class SystemModuleDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }
        public List<SystemModuleDTO> Children { get; set; } = new();
    }
}
