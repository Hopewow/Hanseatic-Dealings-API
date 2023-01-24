using System.Text.Json.Serialization;

namespace Hanseatic_Dealings_API.Models;

public class ShipModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Money { get; set; }
    [JsonIgnore]
    public List<ShipStorageModel>? Goods { get; set;}
    [JsonIgnore]
    public UserModel? User { get; set; }
    public int UserId { get; set; }
}
