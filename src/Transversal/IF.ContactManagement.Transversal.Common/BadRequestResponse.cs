
namespace IF.ContactManagement.Transversal.Common
{
    public class BadRequestResponse
    {
        public int? Status { get; set; }
        public string? Title { get; set; }

        public string? Message { get; set; }
        public List<Error>? Errors { get; set; }

        public BadRequestResponse()
        {

            Errors = new List<Error>();
        }
    }

    public class BadRequestResponseErrorList
    {
        public int? Status { get; set; }
        public string? Title { get; set; }

        public string? Message { get; set; }

        public List<Error>? Errors { get; set; }

        public BadRequestResponseErrorList()
        {
            Errors = new List<Error>();
        }
    }
}
