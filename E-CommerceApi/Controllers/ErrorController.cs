using E_CommerceApi.HandlingErrors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error()
        {
            return NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, "Not Found EndPoint"));
        }
    }
}
