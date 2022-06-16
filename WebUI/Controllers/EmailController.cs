using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Email.Commands.SendEmail;

namespace WebUI.Controllers
{
    public class EmailController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> FeedBack([FromForm] string name, [FromForm] string email)
        {
            await Mediator.Send(new SendEmailCommand { Subject = $"Здравствуйте {name}!", EmailAddress = email });

            return RedirectToAction("Contacts", "Home");
        }
    }
}
