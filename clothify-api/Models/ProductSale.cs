namespace WebApplication1.Models;

public class ProductSale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SaleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Product Product { get; set; }
    public Sale Sale { get; set; }
}