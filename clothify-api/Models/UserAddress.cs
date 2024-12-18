namespace clothify_api.Models;

public class UserAddress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string WardId { get; set; }
    public string DistrictId { get; set; }
    public string ProvinceId { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public bool IsDefault { get; set; }

}