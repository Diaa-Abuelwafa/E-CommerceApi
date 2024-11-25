using E_CommerceApi.HandlingErrors;
using System.Net;
using System.Text.Json;

namespace E_CommerceApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly IHostEnvironment Env;

        public ExceptionMiddleware(RequestDelegate Next, IHostEnvironment Env)
        {
            this.Next = Next;
            this.Env = Env;
        }
        public async Task InvokeAsync(HttpContext Context)
        {
            try
            {
                await Next(Context);
            }
            catch(Exception Ex)
            {
                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if(Env.IsDevelopment())
                {
                    var Response = new ApiServerErrorResponse((int)HttpStatusCode.InternalServerError, Ex.StackTrace.ToString());

                    var JsonResponse = JsonSerializer.Serialize(Response);

                    await Context.Response.WriteAsync(JsonResponse);
                }
                else
                {
                    var Response = new ApiServerErrorResponse((int)HttpStatusCode.InternalServerError);

                    var JsonResponse = JsonSerializer.Serialize(Response);

                    await Context.Response.WriteAsync(JsonResponse);
                }

            }
        }
    }
}
