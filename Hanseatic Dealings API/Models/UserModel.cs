using System.Text.Json.Serialization;

namespace Hanseatic_Dealings_API.Models;

public class UserModel
{
    public int Id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string? token { get; set; }
    [JsonIgnore]
    public List<ShipModel>? Ships { get; set; }
}
