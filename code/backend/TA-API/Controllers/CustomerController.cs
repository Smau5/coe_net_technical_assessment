using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TA_API.Models.Responses;
using TA_API.Services.Data;

namespace TA_API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        AssessmentDbContext AssessmentDbContext;
        public CustomerController(AssessmentDbContext AssessmentDbContext)
        {
            this.AssessmentDbContext = AssessmentDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var orders = await AssessmentDbContext.Customers
                .ToListAsync();

            var response = orders.Select(o => new CustomerResponse
            {
                Name = o.Name,
                Id = o.Id
            }).ToList();

            return Ok(response);
        }
    }
}
