

using IF.ContactManagement.Domain.Enums;

namespace IF.ContactManagement.Transversal.Common
{
    public class ResponseGeneric<T>
    {
        public T? Data { get; set; }

        public bool IsSuccess { get; set; } = false;

        public string? Message { get; set; } = null;

        public IEnumerable<BaseError>? Errors { get; set; } = [];
        public List<Error> ErrorList { get; set; }
    }

    public class Error
    {
        public string Code { set; get; } = string.Empty;
        public string Message { set; get; } = string.Empty;
        public ErrorType ErrorType { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public DateTime? DateError { get; set; }

    }

    public class ErrorDetail
    {
        public ErrorDetailType Type { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
