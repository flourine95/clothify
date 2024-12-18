namespace WebApplication1.Models;

public class ProductReview
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public byte Rating { get; set; } 
    public string ReviewText { get; set; }
    public bool IsVerified { get; set; }
    public int HelpfulVotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}