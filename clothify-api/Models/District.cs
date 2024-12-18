namespace clothify_api.Models;

public class District
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ProvinceId { get; set; }
    public ICollection<Ward> Wards { get; set; } = new List<Ward>();


}