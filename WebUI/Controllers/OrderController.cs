using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Order.Commands.DeleteOrder;
using Application.UseCases.Order.Commands.UpdateOrder;
using Application.UseCases.Order.Queries.GetOrder;
using Application.UseCases.Order.Queries.GetOrdersWithPagination;
using Application.UseCases.Order.Queries.SearchOrdersWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Order;

namespace WebUI.Controllers
{
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetOrdersWithPaginationQuery query)
        {
            var suppliers = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (suppliers.Items.Count is 0 && suppliers.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForOrders
                {
                    Orders = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForOrders
            {
                Orders = suppliers,
                CategoriesForHeader = categoriesForHeader
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateOrder model)
        {
            await Mediator.Send(new UpdateOrderCommand
            {
                Id = model.Id,
                DeliveryFulfilled = model.IsDeliveryComplete,
                PaymentAllowed = model.IsPaymentAllowed
            });

            return View("_OrderPartial", new ModelForOrderPartial
            {
                Order = await Mediator.Send(new GetOrderQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteOrderCommand
            {
                Id = id
            });

            return NoContent();
        }
    }
}
