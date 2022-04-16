using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller][action]")]
    public abstract class ApiControllerBase : Controller
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
