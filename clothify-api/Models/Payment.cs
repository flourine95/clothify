namespace clothify_api.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public byte PaymentMethod { get; set; }
    public byte Status { get; set; }
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Order Order { get; set; }
}