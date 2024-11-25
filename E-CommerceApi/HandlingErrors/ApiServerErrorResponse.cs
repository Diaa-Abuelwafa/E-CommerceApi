namespace E_CommerceApi.HandlingErrors
{
    public class ApiServerErrorResponse : ApiErrorResponse
    {
        public string Details { get; set; }

        public ApiServerErrorResponse(int StatusCode, string? Details = null, string? Message = null) : base(StatusCode, Message)
        {
            this.Details = Details;
        }
    }
}
