using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Commands.DeleteOrder;
using Application.Features.Orders.Commands.UpdateOrder;
using Application.Features.Orders.Queries.GetAllOrders;
using Application.Features.Orders.Queries.GetByIdOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllOrdersQuery()));
        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdOrderQuery { Id = id };
            var order = await _mediator.Send(query);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.Send(command);

            return Ok();
        }

        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] UpdateOrderCommand command)
        {
            command.Id = id;
            bool success = await _mediator.Send(command);

            if (success)
            {
                return Ok("Order updated successfully.");
            }

            return NotFound("Order not found.");
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var command = new DeleteOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (result > 0)
            {
                return Ok($"Order with Id = {id} was successfully deleted from database"); 
            }
            else
            {
                return NotFound($"Order with Id = {id} wasn't found."); 
            }
        }
    }
}
