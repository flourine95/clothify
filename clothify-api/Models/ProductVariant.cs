﻿namespace clothify_api.Models;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } 
    public string Color { get; set; } 
    public decimal Price { get; set; } 
    public int Stock { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}
