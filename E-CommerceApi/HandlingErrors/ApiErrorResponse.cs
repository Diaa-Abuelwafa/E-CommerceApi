namespace E_CommerceApi.HandlingErrors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiErrorResponse(int StatusCode, string? Message)
        {
            this.StatusCode = StatusCode;

            if (Message is null)
            {
                this.Message = GetMsg(StatusCode);
            }
            else
            {
                this.Message = Message;
            }
        }

        private string GetMsg(int StatusCode)
        {
            var Msg = "";

            switch (StatusCode)
            {
                case 400:
                    Msg = "BadRequest !";
                    break;

                case 500:
                    Msg = "Server Error Accour";
                    break;

                case 401:
                    Msg = "You Are Not Authenticated Consumer";
                    break;

                case 404:
                    Msg = "Not Found !";
                    break;
            }

            return Msg;
        }
    }
}
