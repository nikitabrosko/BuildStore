using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Order.Commands.UpdateOrder;
using Application.UseCases.Payment.Commands.CreatePayment;
using Application.UseCases.Payment.Commands.DeletePayment;
using Application.UseCases.Payment.Commands.UpdatePayment;
using Application.UseCases.Payment.Queries.GetPayment;
using Application.UseCases.Payment.Queries.GetPaymentsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class PaymentController : ApiControllerBase
    {
        private string _returnUrl;

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetPaymentsWithPaginationQuery query)
        {
            return View(await Mediator.Send(query));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return View(await Mediator.Send(new GetPaymentQuery { Id = id }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{orderId:int}")]
        public IActionResult Create(int orderId)
        {
            return View(new CreatePaymentCommand {OrderId = orderId});
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> Create([FromForm] CreatePaymentCommand command)
        {
            try
            {
                var paymentId = await Mediator.Send(command);

                var payment = await Mediator.Send(new GetPaymentQuery { Id = paymentId });

                await Mediator.Send(new UpdateOrderCommand
                {
                    Id = command.OrderId,
                    Payment = payment
                });

                return RedirectToAction("Clear", "ShoppingCart");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, string returnUrl = null)
        {
            ViewBag.Title = "Update Payment";

            _returnUrl = returnUrl;

            try
            {
                var entity = await Mediator.Send(new GetPaymentQuery { Id = id });

                var command = new UpdatePaymentCommand
                {
                    Id = id,
                    Type = entity.Type,
                    Allowed = entity.Allowed,
                    CreditCardNumber = entity.CreditCardNumber,
                    CardExpMonth = entity.CardExpMonth,
                    CardExpYear = entity.CardExpYear
                };

                return View(command);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Update([FromForm] UpdatePaymentCommand command)
        {
            try
            {
                await Mediator.Send(command);

                if (_returnUrl != null)
                {
                    return Redirect(_returnUrl);
                }

                return RedirectToAction("Index", "Payment");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, string returnUrl = null)
        {
            try
            {
                await Mediator.Send(new DeletePaymentCommand { Id = id });

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Payment");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public IActionResult CancelCreation(int orderId)
        {
            return RedirectToAction("CancelCreation", "Delivery", new {orderId});
        }
    }
}
