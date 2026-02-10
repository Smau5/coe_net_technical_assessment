using TA_API.Services.Data;

namespace TA_API.Models.Responses;



public class OrderResponse
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public CustomerResponse? Customer { get; set; }
}


public class CustomerResponse
{

    public string Name { get; set; }

}
