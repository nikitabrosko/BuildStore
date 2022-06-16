﻿using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Delivery.Commands.CreateDelivery;
using Application.UseCases.Delivery.Commands.DeleteDelivery;
using Application.UseCases.Delivery.Commands.UpdateDelivery;
using Application.UseCases.Delivery.Queries.GetDeliveriesWithPagination;
using Application.UseCases.Delivery.Queries.GetDelivery;
using Application.UseCases.Order.Commands.UpdateOrder;
using Application.UseCases.Order.Queries.GetOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class DeliveryController : ApiControllerBase
    {
        private string _returnUrl;

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetDeliveriesWithPaginationQuery query)
        {
            return View(await Mediator.Send(query));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return View(await Mediator.Send(new GetDeliveryQuery { Id = id }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{orderId:int}")]
        public IActionResult Create(int orderId)
        {
            return View(new CreateDeliveryCommand { OrderId = orderId });
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
    }
}
