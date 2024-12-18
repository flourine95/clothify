namespace clothify_api.Models;

public class Province
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ICollection<District> Districts { get; set; } = new List<District>();
}