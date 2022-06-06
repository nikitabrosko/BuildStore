using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Email.Commands.SendEmail;
using Application.UseCases.Identity.User.Queries.GetUser;

namespace WebUI.Controllers
{
    public class EmailController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Feedback()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});

                return View("_Feedback", new SendEmailCommand {EmailAddress = user.Email});
            }

            return View("_Feedback", new SendEmailCommand {EmailAddress = string.Empty});
        }

        [HttpPost]
        public async Task<IActionResult> FeedBack([FromForm] SendEmailCommand command)
        {
            await Mediator.Send(command);

            return RedirectToAction("Index", "Category");
        }
    }
}
