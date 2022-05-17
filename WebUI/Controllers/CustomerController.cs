using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Customer.Commands.CreateCustomer;
using Application.UseCases.Customer.Commands.UpdateCustomer;
using Application.UseCases.Customer.Queries.GetCustomer;

namespace WebUI.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCustomerCommand command)
        {
            try
            {
                command.UserName = User.Identity.Name;

                await Mediator.Send(command);

                return RedirectToAction("Cabinet", "Account");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            try
            {
                var entity = await Mediator.Send(
                    new GetCustomerQuery
                    {
                        UserName = User.Identity.Name
                    });

                return View(new UpdateCustomerCommand
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Address = entity.Address,
                    City = entity.City,
                    Country = entity.Country,
                    Phone = entity.Phone,
                    CreditCardNumber = entity.CreditCardNumber,
                    CardExpMonth = entity.CardExpMonth,
                    CardExpYear = entity.CardExpYear
                });
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateCustomerCommand command)
        {
            try
            {
                command.UserName = User.Identity.Name;

                await Mediator.Send(command);

                return RedirectToAction("Cabinet", "Account");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}
