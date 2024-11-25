namespace E_CommerceApi.HandlingErrors
{
    public class ApiValidationErrorResponse : ApiErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public ApiValidationErrorResponse(int StatusCode, List<string> Errors, string? Message = null) : base(StatusCode, Message)
        {
            this.Errors = Errors;
        }
    }
}
