using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Customer.Commands.CreateCustomer;
using Application.UseCases.Customer.Commands.UpdateCustomer;
using Application.UseCases.Customer.Queries.GetCustomer;
using Application.UseCases.Identity.User.Queries.GetUser;

namespace WebUI.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return View(await Mediator.Send(new GetCustomerQuery { Id = id }));
        }

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
                var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});

                var entity = await Mediator.Send(
                    new GetCustomerQuery
                    {
                        Id = user.Customer.Id
                    });

                return View(new UpdateCustomerCommand
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Address = entity.Address,
                    City = entity.City,
                    Country = entity.Country,
                    Phone = entity.Phone
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
