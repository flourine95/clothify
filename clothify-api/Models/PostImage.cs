namespace WebApplication1.Models;

public class PostImage
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string ImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}