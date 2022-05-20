using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Delivery.Commands.CreateDelivery;
using Application.UseCases.Delivery.Commands.DeleteDelivery;
using Application.UseCases.Delivery.Commands.UpdateDelivery;
using Application.UseCases.Delivery.Queries.GetDeliveriesWithPagination;
using Application.UseCases.Delivery.Queries.GetDelivery;
using Application.UseCases.Order.Commands.UpdateOrder;
using Application.UseCases.Order.Queries.GetOrder;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class DeliveryController : ApiControllerBase
    {
        private string _returnUrl;

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetDeliveriesWithPaginationQuery query)
        {
            return View(await Mediator.Send(query));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return View(await Mediator.Send(new GetDeliveryQuery { Id = id }));
        }

        [HttpGet("{orderId:int}")]
        public IActionResult Create(int orderId)
        {
            return View(new CreateDeliveryCommand {OrderId = orderId});
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> Create([FromForm] CreateDeliveryCommand command)
        {
            var deliveryId = await Mediator.Send(command);

            var delivery = await Mediator.Send(new GetDeliveryQuery {Id = deliveryId});

            await Mediator.Send(new UpdateOrderCommand
            {
                Id = command.OrderId,
                Delivery = delivery
            });

            return RedirectToAction("Create", "Payment", new {orderId = command.OrderId});
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, string returnUrl = null)
        {
            ViewBag.Title = "Update Delivery";

            _returnUrl = returnUrl;

            try
            {
                var entity = await Mediator.Send(new GetDeliveryQuery { Id = id });
                
                var command = new UpdateDeliveryCommand
                {
                    Id = id,
                    Type = entity.Type,
                    Fulfilled = entity.Fulfilled
                };

                return View(command);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> Update([FromForm] UpdateDeliveryCommand command)
        {
            try
            {
                await Mediator.Send(command);

                if (_returnUrl != null)
                {
                    return Redirect(_returnUrl);
                }

                return RedirectToAction("Index", "Delivery");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, string returnUrl = null)
        {
            try
            {
                await Mediator.Send(new DeleteDeliveryCommand { Id = id });

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Delivery");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CancelCreation(int orderId)
        {
            var order = await Mediator.Send(new GetOrderQuery {Id = orderId});
            await Mediator.Send(new DeleteDeliveryCommand {Id = order.DeliveryId});

            return RedirectToAction("CancelCreation", "Order", new { orderId });
        }
    }
}
