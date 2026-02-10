using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Entities;

public class OrderEntity
{
    [Key]
    public Guid Id {get; set;}
    [Required]
    public string ProductName {get; set;} = default!;
    public int Quantity {get; set;}
    public decimal Price {get; set;}
    [Required]
    public string CustomerId {get; set;} = default!;
    public string Status {get; set;} = "Created";
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}