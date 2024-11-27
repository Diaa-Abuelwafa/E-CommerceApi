using E_CommerceDomain.Interfaces.Caching_Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_CommerceApi.Custom_Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int ExpireMinutes;
        public CachedAttribute(int ExpireMinutes)
        {
            this.ExpireMinutes = ExpireMinutes;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CachedService = context.HttpContext.RequestServices.GetRequiredService<ICachedService>();
            var Key = GetKeyFromRequest(context.HttpContext.Request);

            var DataFromMemory = CachedService.Get(Key);

            if(DataFromMemory is not null)
            {
                var Response = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = 200,
                    Content = DataFromMemory
                };

                context.Result = Response;
                return;
            }

            var Result = await next.Invoke();

            if(Result.Result is OkObjectResult)
            {
                var Data = Result.Result;

                CachedService.Set(Key, Data, ExpireMinutes);
            }

            return;
        }

        private string GetKeyFromRequest(HttpRequest Request)
        {
            StringBuilder MyKey = new StringBuilder();

            MyKey.Append(Request.Path);

            foreach(var (key, value) in Request.Query.OrderBy(x => x.Key))
            {
                MyKey.Append($"|{key}/{value}");
            }

            return MyKey.ToString();
        }
    }
}
