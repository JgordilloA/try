using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    // GET /api/orders
    [HttpGet]
    public async Task<ActionResult<List<ItemListOrderDto>>> List() =>
        Ok(await orderService.ListOrderAsync());

    // POST /api/orders
    [HttpPost]
    public async Task<ActionResult<CreateOrderResponseDto>> Create([FromBody] CreateOrderDto order)
    {
        var result = await orderService.CreateOrderAsync(order);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}