using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TA_API.Migrations;
using TA_API.Models.Requests;
using TA_API.Models.Responses;
using TA_API.Services.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TA_API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    AssessmentDbContext AssessmentDbContext;
    public OrdersController(AssessmentDbContext AssessmentDbContext)
    {
        this.AssessmentDbContext = AssessmentDbContext;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var orders = await AssessmentDbContext.Orders
            .Include(o => o.Customer).ToListAsync();

        var response = orders.Select(o => new OrderResponse
        {
            Customer = new CustomerResponse
            {
                Name = o.Customer.Name,
                Id = o.CustomerId
            },
            Id = o.Id,
            Status = o.Status.ToString()
        }).ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid Id")]
        string id)
    {
        int orderId;
        bool success = int.TryParse(id, out orderId);

        if (!success)
        {
            return BadRequest("Id is not an integer");
        }
        var order = await AssessmentDbContext.Orders.Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if(order is null)
        {
            return NotFound("Order not found");

        }

        var response = new OrderResponse
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Customer = new CustomerResponse
            {
                Name = order.Customer.Name,
                Id = order.Customer.Id
            }
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] OrderCreateRequest request)
    {

        // TODO: Validate request fields as part of the assessment.
        int customerId;
        bool success = int.TryParse(request.CustomerId, out customerId);

        if (!success)
        {
            return BadRequest("Customer Id is not an integer");
        }

        bool enumParsed = Enum.TryParse(request.Status, out OrderStatus orderStatus);

        if (!enumParsed)
        {
            return BadRequest("Status is not valid");
        }

        var customer = await AssessmentDbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
        {
            return BadRequest("Customer with the provided Id does not exist");
        }

        var order = new Order
        {
            Status = orderStatus,
            Customer = customer
        };

        AssessmentDbContext.Add(order);

        await AssessmentDbContext.SaveChangesAsync();

        var response = new OrderResponse
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Customer = new CustomerResponse
            {
                Name = order.Customer.Name,
                Id = order.Customer.Id
            }
        };

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid Id")]
        string id, 
        [FromBody] OrderUpdateRequest request)
    {
        // TODO: Validate request fields as part of the assessment.
        int orderId;
        bool success = int.TryParse(id, out orderId);

        if (!success)
        {
            return BadRequest("Id is not an integer");
        }

        var order = await AssessmentDbContext.Orders.Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
        {
            return NotFound("order not found");
        }


        bool enumParsed = Enum.TryParse(request.Status, out OrderStatus orderStatus);

        if (!enumParsed)
        {
            return BadRequest("Status is not valid");
        }


        order.Status = orderStatus;


        await AssessmentDbContext.SaveChangesAsync();

        var response = new OrderResponse
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Customer = new CustomerResponse
            {
                Name = order.Customer.Name,
                Id = order.Customer.Id
            }
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid Id")] 
        string id)
    {
        int orderId;
        bool success = int.TryParse(id, out orderId);

        if (!success)
        {
            return BadRequest("Id is not an integer");
        }

        var order = await AssessmentDbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if(order is null)
        {
            return NotFound("Order not found");
        }

        AssessmentDbContext.Orders.Remove(order);
        await AssessmentDbContext.SaveChangesAsync();

        return Ok($"Order: {id} removed");
    }
}
