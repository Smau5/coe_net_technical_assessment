using System.ComponentModel.DataAnnotations;

namespace TA_API.Models.Requests;

// Asumiendo que status es required 
public record OrderCreateRequest(
    [Required]
    // for numbers that begin from 1
    [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid Id")]
    string? CustomerId,
    [Required] string? Status);
public record OrderUpdateRequest([Required] string? Status);
