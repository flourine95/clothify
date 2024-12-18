namespace clothify_api.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int UserAddressId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public byte Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}