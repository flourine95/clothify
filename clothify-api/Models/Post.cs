namespace WebApplication1.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; } 
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewsCount { get; set; }
    public string Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();
}